using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Mapping;
using DataAccessLayer.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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



//repository adding
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<UrlService>();
builder.Services.AddScoped<UrlShorteningService>();
//</>


//Authorization authentication
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddRoles<IdentityRole>()
    .AddApiEndpoints();

builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, AppDbContext>>();

//builder.Services.AddAuthorization();
//builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
//    .AddBearerToken(IdentityConstants.BearerScheme);




//</>


var app = builder.Build();

//add seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeed.SeedAsync(userManager, roleManager);
}
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



//</>
app.MapGet("api/{code}", async (string code, AppDbContext _context) =>
{
    var shortenedUrl = await _context.Urls.FirstOrDefaultAsync(x => x.Code == code);
    if ( shortenedUrl == null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(shortenedUrl.OriginalUrl);
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Swagger adding
app.UseSwagger();
app.UseSwaggerUI();
//</>

app.Run();
