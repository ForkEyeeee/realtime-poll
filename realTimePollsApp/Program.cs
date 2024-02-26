using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using realTimePolls.Models;
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(
        GoogleDefaults.AuthenticationScheme,
        options =>
        {
            options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
            options.ClientSecret = builder
                .Configuration.GetSection("GoogleKeys:ClientSecret")
                .Value;
            options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        }
    );

// This adds services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RealTimePollsContext>(options =>
{
    var connectionString = string.Empty;

    if (Environment.GetEnvironmentVariable("IS_DOCKER") == "true")
        connectionString = builder.Configuration.GetConnectionString("DockerConnection");
    else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    else
        connectionString = builder.Configuration.GetConnectionString("DevelopmentConnection");

    options.UseNpgsql(connectionString);
});
builder.Services.AddSignalR();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
{
    builder.Services.AddDbContext<RealTimePollsContext>(options =>
    {
        options.UseInMemoryDatabase("InMemoryDbForTesting");
    });
}

var app = builder.Build();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedProto }
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapHub<PollHub>("/pollHub");

app.Run();

public partial class Program { }
