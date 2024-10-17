using NguyenLMFall2024RazorPages;
using Repository;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<ISystemAccountService, SystemAccountService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<ITagService, TagService>();
builder.Services.AddSingleton<INewsArticleService, NewsArticleService>();

builder.Services.AddSingleton<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<ITagRepository, TagRepository>();
builder.Services.AddSingleton<INewsArticleRepository, NewsArticleRepository>();

// Add required services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSignalR();
builder.Services.AddRazorPages();

// Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true; // Recommended for security
    options.Cookie.IsEssential = true; // Make sure the session cookie is set
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforce HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session middleware before authorization
app.UseSession();

app.UseAuthorization();

// Map the SignalR hub endpoint
app.UseEndpoints(endpoints =>
{
    // Map SignalR hub
    endpoints.MapHub<NewsHub>("/NewsHub");

    // Map Razor pages
    endpoints.MapRazorPages();

    // Redirect root URL to Login page
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("/Authentication/Login");
    });
});


app.Run();
