var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all IPs and a specific port
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5346); // This will listen on all local IP addresses on port 5000
//});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.Run();

