using DAL.Repositories;
using DAL.Repositories.InMemory;
using Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Met USE_IN_MEMORY=true draait de app op een in-memory database, zonder SQL Server.
// Handig om de app ergens te tonen waar de Fontys-database niet bereikbaar is.
// Standaard (false) gebruikt de app de echte SQL Server uit de connectionstring.
var useInMemory = builder.Configuration.GetValue<bool>("USE_IN_MEMORY");

if (useInMemory)
{
    // Singleton: alle requests delen dezelfde data, zodat toegevoegde boeken
    // en reviews zichtbaar blijven tot de app herstart.
    builder.Services.AddSingleton<IAuthorRepository, InMemoryAuthorRepository>();
    builder.Services.AddSingleton<IGenreRepository, InMemoryGenreRepository>();
    builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
    builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
    builder.Services.AddSingleton<IReviewRepository, InMemoryReviewRepository>();
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    builder.Services.AddScoped<IAuthorRepository>(_ => new AuthorRepository(connectionString));
    builder.Services.AddScoped<IGenreRepository>(_ => new GenreRepository(connectionString));
    builder.Services.AddScoped<IBookRepository>(_ => new BookRepository(connectionString));
    builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
    builder.Services.AddScoped<IReviewRepository>(_ => new ReviewRepository(connectionString));
}

// Services (ServiceLibrary)
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReviewService>();

// FR-11/FR-12: inloggen via een cookie. Niet ingelogd? Dan stuurt de app door
// naar de loginpagina; uitloggen verwijdert de cookie.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// In de in-memory modus is de database leeg na een herstart. We maken daarom
// een vaste gebruiker aan via de service, zodat het wachtwoord netjes gehasht
// wordt en je direct kunt inloggen om de app te tonen.
if (useInMemory)
{
    using var scope = app.Services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    if (userService.Login("leyla@booksy.nl", "geheim123") == null)
    {
        userService.Register("Leyla", "leyla@booksy.nl", "geheim123");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();