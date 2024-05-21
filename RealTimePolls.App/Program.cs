using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Mappings;
using RealTimePolls.Repositories;
using SignalRChat.Hubs;
using Serilog;

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

builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RealTimePollsConnectionString"))
);

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
{
    builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
    {
        options.UseInMemoryDatabase("InMemoryDbForTesting");
    });
}

// This adds services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IHomeRepository, SQLHomeRepository>();
builder.Services.AddScoped<IPollsApiRepository, SQLPollsApiRepository>();
builder.Services.AddScoped<IPollRepository, SQLPollRepository>();
builder.Services.AddScoped<IHelpersRepository, SQLHelpersRepository>();

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);



builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddSignalR();

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