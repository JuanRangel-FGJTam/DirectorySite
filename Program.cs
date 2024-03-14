using DirectorySite.Data;
using DirectorySite.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJwtAuthentication( builder.Configuration );
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication";
});
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient( "DirectoryAPI", o => o.BaseAddress = new Uri(builder.Configuration.GetValue<string>("DirectoryAPI")!) );
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CatalogService>();

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
app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.Run();
