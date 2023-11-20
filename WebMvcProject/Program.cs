using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebMvcProject.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProjectDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opts =>
{
    opts.Cookie.Name = "WebApplication2.auth";//kullanýcý bilgilerini burada sakla
    opts.ExpireTimeSpan = TimeSpan.FromDays(7); //7 güne kadar tutacak
    opts.SlidingExpiration = false; //sürenin uzamasýný engellemek için false
    opts.LoginPath = "/Account/Login"; //cookie bulunamadýðýnda yönlendireceði sayfa
    opts.LogoutPath = "/Account/Logout";
    opts.AccessDeniedPath = "/Home/AccessDenied"; //yetkisi olmadýðýnda yönlendirilecek sayfa
});

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