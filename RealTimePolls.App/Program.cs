using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using RealTimePolls.Data;
using RealTimePolls.Mappings;
using RealTimePolls.Repositories;
using SignalRChat.Hubs;
using Serilog;
using RealTimePolls;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddAuthentication(options =>
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

if (builder.Environment.IsProduction())
{
    string connectionString = Environment.GetEnvironmentVariable("RealTimePollsConnectionString");
    builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    string connectionString = builder.Configuration.GetConnectionString("RealTimePollsConnectionString");
    builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
        options.UseNpgsql(connectionString));
}

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
{
    builder.Services.AddDbContext<RealTimePollsDbContext>(options =>
    {
        options.UseInMemoryDatabase("InMemoryDbForTesting");
    });
}

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RealTimePollsDbContext>();
    dbContext.Database.Migrate();
}

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IHomeRepository, SQLHomeRepository>();
builder.Services.AddScoped<IPollsApiRepository, SQLPollsApiRepository>();
builder.Services.AddScoped<IPollRepository, SQLPollRepository>();
builder.Services.AddScoped<IHelpersRepository, SQLHelpersRepository>();
builder.Services.AddScoped<IAuthRepository, SQLAuthRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddSignalR();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedProto });

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapHub<PollHub>("/pollHub");

app.Run();

public partial class Program { }
