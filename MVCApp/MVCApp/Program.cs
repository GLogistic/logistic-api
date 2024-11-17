using DbAccess.Context;
using MVCApp.Extensions;
using MVCApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });

builder.Services.ConfigureContext(builder);

builder.Services.ConfigureIdentity();
builder.Services.AddAuthorization();

builder.Services.JwtConfigure(builder);

builder.Services.ConfigureScopedDependencies();

builder.Services.ConfigureMapper();

builder.Services.ConfigureCacheProfiles();

var app = builder.Build();

app.MigrateDatabase<LogisticContext>();

app.UseMiddleware<DbSeederMiddleware>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "/",
    defaults: new { controller = "Home", action = "Index" });

app.Run();
