using DAL.Repositories;
using DAL.Repositories.InMemory;
using Interface;
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

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();