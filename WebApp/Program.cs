using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Helper;
using WebApp.Model;
using WebApp.Repository;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddDbContext<DataContext>(options => options.UseSqlite($"Data Source=final_project.sqlite3", o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddDbContext<DataContext>(options => options
    .UseMySql(
        builder.Configuration.GetConnectionString("DatabaseConnection"),
        new MariaDbServerVersion(new Version(12, 0, 2))
    )
);
// builder.Services.AddDbContext<DataContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<DataContext>();
builder.Services.AddScoped<CommentRepo>();
builder.Services.AddScoped<FriendRepo>();
builder.Services.AddScoped<PostRepo>();
builder.Services.AddScoped<UserRepo>();

builder.Services.AddRazorPages(options => { options.Conventions.AuthorizeFolder("/"); });
builder.Services.AddScoped<SeedDatabase>(); // Remove Later

WebApplication app = builder.Build();
// TODO: SEEDING DB HERE to prevent stack overflow error
using (IServiceScope scope = app.Services.CreateScope())
{
    DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
    SeedDatabase seedDatabase = scope.ServiceProvider.GetRequiredService<SeedDatabase>();
    seedDatabase.FakeData().Wait();
    if (!context.Users.Any()) throw new Exception("Unable to Seed Database");
}


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

app.Run();