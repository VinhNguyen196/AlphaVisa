using System.Globalization;
using AlphaVisa.Application;
using AlphaVisa.Infrastructure;
using AlphaVisa.Infrastructure.Data;
using AlphaVisa.Web;
using AlphaVisa.Web.Exceptions;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

// Enable cors
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromMinutes(42));
    });

    opts.DefaultPolicyName = myAllowSpecificOrigins;
});

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);
builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");
var supportedCultures = new[] { "en", "vi" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

builder.Services.AddAntiforgery();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.RegisterAppExceptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseExceptionHandler();
    // app.UseHttpsRedirection(); let proxy handle this operation
}

app.UseCors(myAllowSpecificOrigins);
app.UseHealthChecks("/health");
app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);

// Add OpenAPI 3.0 document serving middleware
// Available at: http://localhost:<port>/swagger/v1/swagger.json
app.UseOpenApi();
// Add web UIs to interact with the document
// Available at: http://localhost:<port>/swagger
app.UseSwaggerUi(settings =>  // UseSwaggerUI Protected by if (env.IsDevelopment())
{
    settings.Path = "/api";  // Path to Swagger UI
    settings.DocumentPath = "/api/specification.json";  // Path to OpenAPI document
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Map("/", () => Results.Redirect("/api"));

app.UseAntiforgery();
app.MapEndpoints();

app.Run();

public partial class Program { }
