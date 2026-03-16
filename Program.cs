using EIKON.UI.Data.Mock;
using DevExpress.AspNetCore.Reporting;
using DevExpress.Blazor.Reporting;
using DevExpress.Security.Resources;
using DevExpress.Xpo;
using DevExpress.XtraReports.Web.Extensions;
using EIKON.UI.Datos;
using EIKON.UI.Interfaces;
using EIKON.UI.Repositories;
using EIKON.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// MVC y JSON
// -----------------------------
builder.Services.AddMvc()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = null);

// -----------------------------
// Servicios básicos
// -----------------------------
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<EikonDataServices>();
builder.Services.AddScoped(typeof(IMockService<>), typeof(MockService<>));
builder.Services.AddScoped<AuthenticationService>();

builder.Services.Configure<AppApi>(
    builder.Configuration.GetSection("AppApi").GetSection("ApiBaseUrl")
);

builder.Configuration.AddJsonFile("appsettings.json");
var apibaseurl = builder.Configuration["AppApi:ApiBaseUrl"];

builder.Services.AddScoped<EikonDataRepository>();

builder.Services.AddScoped(sp =>
{
    return new HttpClient { BaseAddress = new Uri(apibaseurl) };
});

// -----------------------------
// Blazor Server
// -----------------------------
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => options.DetailedErrors = true);

// -----------------------------
// DevExpress Blazor
// -----------------------------
builder.Services.AddDevExpressBlazor();

// Opcional (DevExpress ya usa Bootstrap 5 por defecto)
// builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(opt =>
// {
//     opt.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
// });

// -----------------------------
// REPORTING
// -----------------------------
builder.Services.AddDevExpressServerSideBlazorReportViewer();
builder.Services.AddDevExpressBlazorReporting();

//builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();

//builder.Services.ConfigureReportingServices(configurator =>
//{
//    configurator.ConfigureReportDesigner(designer =>
//    {
//        designer.RegisterDataSourceWizardJsonConnectionStorage<CustomDataSourceWizardJsonDataConnectionStorage>(true);
//    });

//    configurator.ConfigureWebDocumentViewer(viewer =>
//    {
//        viewer.UseCachedReportSourceBuilder();
//        viewer.RegisterJsonDataConnectionProviderFactory<CustomJsonDataConnectionProviderFactory>();
//        viewer.RegisterConnectionProviderFactory<CustomSqlDataConnectionProviderFactory>();
//    });

//    configurator.UseAsyncEngine();
//});

//builder.Services.AddDbContext<ReportDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("ReportsDataConnectionString"))
//);

// -----------------------------
// Sesión
// -----------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(400);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// -----------------------------
// Inicializar DB de reportes
// -----------------------------
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
//    db.InitializeDatabase();
//}

// -----------------------------
// Acceso a recursos DevExpress
// -----------------------------
var contentDir = new DirectoryInfo(
    Path.Combine(app.Environment.ContentRootPath, "..", "Content")
).FullName;

var allowRule = DirectoryAccessRule.Allow(contentDir);
AccessSettings.ReportingSpecificResources.TrySetRules(allowRule, UrlAccessRule.Allow());

// -----------------------------
// Middleware DevExpress
// -----------------------------
app.UseDevExpressBlazorReporting();
app.UseReporting(options =>
{
    options.UserDesignerOptions.DataBindingMode =
        DevExpress.XtraReports.UI.DataBindingMode.Expressions;
});

// -----------------------------
// Pipeline HTTP
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.MapControllers();
app.UseStaticFiles();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Ajustes de datos
string contentPath = app.Environment.ContentRootPath;
AppDomain.CurrentDomain.SetData("DataDirectory", contentPath);
AppDomain.CurrentDomain.SetData("DXResourceDirectory", contentPath);

app.Run();
