using System.Globalization;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Infrastructure.Data;
using AlphaVisa.Web.Options;
using AlphaVisa.Web.Services;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<ISharedLocalizer, SharedLocalizer>();
builder.Services.ConfigureOptions<EmailOptionsSetup>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseExceptionHandler("/Error");
    // app.UseHttpsRedirection(); let proxy handle this operation
}

app.UseHealthChecks("/health");
app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);
app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToAreaPage("/NotFound", "Identity");

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

public partial class Program { }
