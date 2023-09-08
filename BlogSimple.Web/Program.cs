using BlogSimple.Model.Models;
using BlogSimple.Model.Services;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Web.BusinessManager;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings;
using BlogSimple.Web.Settings.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Gets all the settings from appsettings.json and maps them to DatabaseSettings class. 
builder.Services.Configure<BlogSimpleDatabaseSettings>(
    builder.Configuration.GetSection(nameof(BlogSimpleDatabaseSettings)));

// Ties the interface class with the class.
// Hence provides the instance of the class (which contains database settings) whenever an instance of the interface is required.
builder.Services.AddSingleton<IBlogSimpleDatabaseSettings>(e =>
    e.GetRequiredService<IOptions<BlogSimpleDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(e =>
    new MongoClient(builder.Configuration.GetValue<string>("BlogSimpleDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentReplyService, CommentReplyService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<IHomeBusinessManager, HomeBusinessManager>();
builder.Services.AddScoped<IBlogBusinessManager, BlogBusinessManager>();
builder.Services.AddScoped<IAccountBusinessManager, AccountBusinessManager>();

builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));

builder.Services.AddIdentity<User, ApplicationRole>()
    .AddMongoDbStores<User, ApplicationRole, Guid>(
    builder.Configuration.GetValue<string>("BlogSimpleDatabaseSettings:ConnectionString"),
    builder.Configuration.GetValue<string>("BlogSimpleDatabaseSettings:DatabaseName"))
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false);

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
