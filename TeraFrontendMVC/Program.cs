using TeraFrontendMVC.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura HttpClient para que use el nombre del servicio del backend en Docker
var backendBaseUrl = builder.Configuration.GetValue<string>("BackendSettings:BaseUrl");
builder.Services.AddHttpClient<Users>(client =>
{
    client.BaseAddress = new Uri(backendBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure the app to listen on port 80
app.Urls.Add("http://*:80"); // Asegúrate de que la aplicación escuche en el puerto 80

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
