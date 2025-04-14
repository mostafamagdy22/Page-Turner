using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Repositories;
using PageTurner.Repositories.Interfaces;
using PageTurner.Services;
using PageTurner.Services.Interfaces;
using PageTurner.Services.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
	.Get<EmailConfiguration>();


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IBookRepository,BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => 
{
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
	options.Password.RequireDigit = false;
	options.Password.RequireNonAlphanumeric = false;
})
	 .AddEntityFrameworkStores<ApplicationDbContext>()
	 .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	string[] roles = { "Admin", "User" };
	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
}

app.Run();
