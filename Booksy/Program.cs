using DAL.Repositories;
using Interface;
using ServiceLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Repositories (DAL)
builder.Services.AddScoped<IBookRepository>(_ => new BookRepository(connectionString));
builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddScoped<IReviewRepository>(_ => new ReviewRepository(connectionString));

// Services (ServiceLibrary)
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