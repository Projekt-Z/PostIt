using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PostIt.Web.Data;
using PostIt.Web.Hubs;
using PostIt.Web.Services;
using PostIt.Web.Services.Chat;
using PostIt.Web.Services.DefaultAuthentication;
using PostIt.Web.Services.Posts;
using PostIt.Web.Services.Smtp;

var builder = WebApplication.CreateBuilder(args);

// Postgres
builder.Services.AddDbContext<ApplicationContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

#region redis

builder.Services.AddDistributedRedisCache(o =>
{
    o.Configuration = builder.Configuration.GetConnectionString("RedisCache");
    o.InstanceName = "redisOne";
});

#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSignalR();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IDefaultAuthenticationService, DefaultAuthenticationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ISmtpService, SmtpService>();

#region Google oAuth2

builder.Services.AddAuthentication(o =>
    {
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(o =>
    {
        o.LoginPath = "/oauth/google-login";
        o.ExpireTimeSpan = TimeSpan.FromDays(7);
    })
    .AddGoogle(o =>
    {
        o.ClientId = builder.Configuration.GetSection("GoogleCredentials").GetSection("ClientId").Value;
        o.ClientSecret = builder.Configuration.GetSection("GoogleCredentials").GetSection("ClientSecret").Value;
        o.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        o.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
        o.ClaimActions.Clear();
        o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        o.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        o.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
        o.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
        o.ClaimActions.MapJsonKey("urn:google:profile", "link");
        o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        o.ClaimActions.MapJsonKey("image", "picture");
    });

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.Run();