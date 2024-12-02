using TeraFrontendMVC.Controllers;
using TeraFrontendMVC.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ConfigureServices(builder.Services, builder.Configuration);

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
ConfigurePipeline(app);

app.Run();

// Method to configure services, add sessions, and filters
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    ConfigureSession(services);
    ConfigureHttpClients(services, configuration);
}


// Method to configure HttpClient services for each controller
void ConfigureHttpClients(IServiceCollection services, IConfiguration configuration)
{
    var backendBaseUrl = configuration.GetValue<string>("BackendSettings:BaseUrl");

    // Registering HttpClient with the same base URL for each controller
    services.AddHttpClient<ResultsController>(client =>
    {
        client.BaseAddress = new Uri(backendBaseUrl);
    });

    services.AddHttpClient<AccountController>(client =>
    {
        client.BaseAddress = new Uri(backendBaseUrl);
    });
}

// Method to configure session options
void ConfigureSession(IServiceCollection services)
{
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromHours(24);
        options.Cookie.HttpOnly = true; // Increases cookie security
        options.Cookie.IsEssential = true; // Allows the session cookie when the user has not consented to it
    });
}

// Method to configure the app's request pipeline
void ConfigurePipeline(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();
    app.UseSession();
    app.UseRouting();

    // Auth custom middleware
    app.UseMiddleware<AuthorizationMiddleware>();

    app.UseAuthorization();

    // Configure the app to listen on port 80
    app.Urls.Add("http://*:80");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}
