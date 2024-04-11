using furni.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using furni.Models.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Thêm dịch vụ controllers và views
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "1022495641683-82pto6eboauvum4ua67am2frec33io2a.apps.googleusercontent.com";
    options.ClientSecret = "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";
})
.AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = "YOUR_FACEBOOK_APP_ID";
    facebookOptions.AppSecret = "YOUR_FACEBOOK_APP_SECRET";
});


builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


var app = builder.Build();

var supportedCultures = new[] { "en", "vi" };

var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapControllerRoute(
    //  name: "areas",
    //  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    //);
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapRazorPages();

app.Run();
