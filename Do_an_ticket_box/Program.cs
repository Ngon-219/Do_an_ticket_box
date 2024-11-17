using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("defaultString");
    options.UseSqlServer(connectionString);
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true; // nếu cần thiết
    options.AppendTrailingSlash = false;
});

builder.Services.AddFluentEmail(builder.Configuration["Email:SenderEmail"], builder.Configuration["Email:Sender"])
    .AddSmtpSender(new SmtpClient(builder.Configuration["Email:Host"])
    {
        Port = builder.Configuration.GetValue<int>("Email:Port"),
        EnableSsl = false,
        Credentials = new NetworkCredential(
            builder.Configuration["Email:SenderEmail"],  
            "Hanhtinhsongsong219@" // Nhập password Email của m ở đây
        )
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";          // Đường dẫn trang đăng nhập
        options.LogoutPath = "/Account/Logout";        // Đường dẫn trang đăng xuất
        options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn truy cập bị từ chối (nếu có)
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn cookie
        options.SlidingExpiration = true;              // Cho phép cookie tự động gia hạn khi người dùng hoạt động
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "Home",
    pattern: "Home/{action}/{id?}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/Error404");
    }
});

app.Run();
