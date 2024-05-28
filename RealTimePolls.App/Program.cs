using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Polly;
using RealTimePolls.Data;
using RealTimePolls.Mappings;
using RealTimePolls.Repositories;
using Serilog;
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/RealTimePolls_Logs.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

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
            options.ClientId = builder.Configuration["GoogleKeys:ClientId"] ?? Environment.GetEnvironmentVariable("GoogleKeys_ClientId");
            options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"] ?? Environment.GetEnvironmentVariable("GoogleKeys_ClientSecret");
            options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        }
    );

string connectionString = builder.Configuration["RealTimePollsConnectionString"] ??
                          Environment.GetEnvironmentVariable("RealTimePollsConnectionString");

builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
    options.UseNpgsql(connectionString));

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
{
    builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
    {
        options.UseInMemoryDatabase("InMemoryDbForTesting");
    });
}

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IPollsApiService, PollsApiService>();
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IHelpersService, HelpersService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddSignalR();

var app = builder.Build();

var retryPolicy = Policy
    .Handle<Exception>()
    .WaitAndRetry(new[]
    {
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(15)
    });

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RealTimePollsDbContext>();
    retryPolicy.Execute(() =>
    {
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    });
}

app.UseForwardedHeaders(
    new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedProto }
);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapHub<PollHub>("/pollHub");

app.Run();

public partial class Program { }