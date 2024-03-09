using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SingleSignOn;
using SSO.Api.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Connection") ?? throw new InvalidOperationException("Connection string 'Connection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

//openID
var conStr = builder.Configuration.GetConnectionString("Connection");
var assembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddIdentityServer()
    .AddConfigurationStore(
        options => options.ConfigureDbContext =
        config => config.UseSqlServer(conStr, opt => opt.MigrationsAssembly(assembly))
    )
    .AddOperationalStore(
        options => options.ConfigureDbContext =
        config => config.UseSqlServer(conStr, opt => opt.MigrationsAssembly(assembly))
    )
    .AddInMemoryApiResources(IdentityConfig.ApiResources)
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
    .AddInMemoryClients(IdentityConfig.Clients)
    .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
    .AddDeveloperSigningCredential();

//cookie ayarlarý
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "IdentityServer.Cookie";
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app tarafýnda olmasý gereken sýra
app.UseIdentityServer();
app.UseHttpsRedirection();
app.UseStaticFiles();

//cookie ayarlarý
app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.Always
});


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello World!");
});

app.Run();
