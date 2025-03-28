
using elraee.IRepository;
using elraee.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace elraee.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //dataBase configuration
            builder.Services.AddDbContext<context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
            });

            //identity configuration

            builder.Services.AddIdentity<applicationUser, IdentityRole<int>>(option =>
            {
                //min 6 digits with 1 uppercase at least

                option.Password.RequiredLength = 6;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<context>();

            //register services for injection
            
            builder.Services.AddScoped<ICategoryRepo,categoryRepo>();
            builder.Services.AddScoped<IProductRepo, productRepo>();
            builder.Services.AddScoped<IImageAsStringRepo, ImageAsStringRepo>();
            builder.Services.AddScoped<IRelativeRepo, RelativeRepo>();
            builder.Services.AddScoped<IContactInfoRepo, ContactInfoRepo>();
            builder.Services.AddScoped<IphoneNumsRepo, phoneNumsRepo>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); 
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
