using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Google oAuth2

builder.Services.AddAuthentication(o =>
    {
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(o =>
    {
        o.LoginPath = "/oauth/google-login";
    })
    .AddGoogle(o =>
    {
        o.ClientId = builder.Configuration.GetSection("GoogleCredentials").GetSection("ClientId").Value;
        o.ClientSecret = builder.Configuration.GetSection("GoogleCredentials").GetSection("ClientSecret").Value;
        o.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    });

#endregion

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();