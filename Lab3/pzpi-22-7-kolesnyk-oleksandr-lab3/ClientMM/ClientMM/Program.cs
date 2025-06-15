using ClientMM.Components;
using ClientMM.Interfaces;
using ClientMM.Services;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<StateContainer>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IRecomendationService, RecommendationService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense
    (
    "Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhOYVJzWmFZfVtgc19GaFZURGYuP1ZhSXxWdkNiXH9ZcHxQQmhVWEZ9XUs="
    );

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
