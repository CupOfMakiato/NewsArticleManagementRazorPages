using Repository;
using Service;

var builder = WebApplication.CreateBuilder(args);

// register service
builder.Services.AddSingleton<ISystemAccountService, SystemAccountService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<ITagService, TagService>();
builder.Services.AddSingleton<INewsArticleService, NewsArticleService>();

builder.Services.AddSingleton<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<ITagRepository, TagRepository>();
builder.Services.AddSingleton<INewsArticleRepository, NewsArticleRepository>();

// Add services to the container.

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddRazorPages();
// add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
});

builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/Authentication/Login");
});

app.Run();
