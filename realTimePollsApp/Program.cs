using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using realTimePolls.Models;
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var secretsPath = "/home/app/.microsoft/usersecrets";
    var userSecretsId = builder.Configuration["UserSecretsId"];
    var secretFilePath = Path.Combine(secretsPath, userSecretsId, "secrets.json");

    if (File.Exists(secretFilePath))
    {
        builder.Configuration.AddJsonFile(secretFilePath, optional: true, reloadOnChange: true);
    }
}
else if (builder.Environment.IsProduction()) { }

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
            options.ClientId = builder.Environment.IsProduction()
                ? Environment.GetEnvironmentVariable("GoogleKeys_ClientId")
                : builder.Configuration["GoogleKeys:ClientId"];
            options.ClientSecret = builder.Environment.IsProduction()
                ? Environment.GetEnvironmentVariable("GoogleKeys_ClientSecret")
                : builder.Configuration["GoogleKeys:ClientSecret"];
            options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        }
    );

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RealTimePollsContext>(options =>
{
    string connectionString;

    if (builder.Environment.IsProduction())
    {
        connectionString = Environment.GetEnvironmentVariable("DatabaseConnection");
    }
    else
    {
        connectionString = builder.Configuration["ConnectionStrings:DatabaseConnection"];
    }

    options.UseNpgsql(connectionString);
});

// This adds services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RealTimePollsContext>(options =>
{
    var connectionString = string.Empty;

    connectionString = builder.Configuration["ConnectionStrings:DatabaseConnection"];

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
