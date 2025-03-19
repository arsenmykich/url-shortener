using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Swagger adding
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//</>

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});



//Authorization authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
//    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();
//</>

//repository adding
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<UrlService>();
//</>

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//
app.MapIdentityApi<User>();
app.MapPost("/logout", async (SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok(new { message = "Logged out successfully" });
});
//
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Swagger adding
app.UseSwagger();
app.UseSwaggerUI();
//</>

app.Run();
