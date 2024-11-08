using TeraFrontendMVC.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura HttpClient para que use el nombre del servicio del backend en Docker
var backendBaseUrl = builder.Configuration.GetValue<string>("BackendSettings:BaseUrl");
builder.Services.AddHttpClient<ResultsController>(client =>
{
    client.BaseAddress = new Uri(backendBaseUrl);
});

builder.Services.AddHttpClient<AccountController>(client =>
{
    client.BaseAddress = new Uri(backendBaseUrl);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true; // Increases cookie security
    options.Cookie.IsEssential = true; // Allows the session cookie when the user has not consented to it
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Configure the app to listen on port 80
app.Urls.Add("http://*:80"); // Asegúrate de que la aplicación escuche en el puerto 80

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
