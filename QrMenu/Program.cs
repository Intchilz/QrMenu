using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrMenu.Components;
using QrMenu.Data;
using QrMenu.Models;
using QrMenu.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// PostgreSQL / Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Models
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Authentication / Authorization
builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

// Services
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentTenant, CurrentTenant>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(
        scope.ServiceProvider,
        app.Configuration);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Login Endpoint
app.MapPost("/account/login", async (
    HttpContext httpContext,
    SignInManager<ApplicationUser> signInManager) =>
{
    var form = await httpContext.Request.ReadFormAsync();

    var email = form["Email"].ToString();
    var password = form["Password"].ToString();

    var rememberMe =
        form["RememberMe"].ToString().Equals(
            "true",
            StringComparison.OrdinalIgnoreCase);

    var returnUrl = form["ReturnUrl"].ToString();

    if (string.IsNullOrWhiteSpace(returnUrl) ||
        !returnUrl.StartsWith("/"))
    {
        returnUrl = "/";
    }

    if (string.IsNullOrWhiteSpace(email) ||
        string.IsNullOrWhiteSpace(password))
    {
        return Results.Redirect("/login?error=invalid");
    }

    var user = await signInManager.UserManager
        .FindByEmailAsync(email);

    if (user == null ||
        !user.IsActive ||
        user.IsDeleted)
    {
        return Results.Redirect("/login?error=invalid");
    }

    var result = await signInManager.PasswordSignInAsync(
        user,
        password,
        rememberMe,
        lockoutOnFailure: true);

    if (result.Succeeded)
    {
        return Results.Redirect(returnUrl);
    }

    if (result.IsLockedOut)
    {
        return Results.Redirect("/login?error=locked");
    }

    return Results.Redirect("/login?error=invalid");
});

app.Run();