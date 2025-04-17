
using FinanceDashboard.Services;
using FinanceDashboard.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<CsvService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.Urls.Add("http://127.0.0.1:5001");
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
