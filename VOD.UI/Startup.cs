using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels.UI;
using VOD.Common.Entities;
using VOD.Database.Contexts;
using VOD.Database.Migrations;
using VOD.Database.Services;

namespace VOD.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Change IdentityUser to VODUser
            //Change ApplicationDbContext to VODContext
            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<VODUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<VODContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IUIReadService, UIReadService>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Video, VideoDTO>();

                cfg.CreateMap<Instructor, InstructorDTO>()
                     .ForMember(dest => dest.InstructorName,
                     src => src.MapFrom(s => s.Name))
                     .ForMember(dest => dest.InstructorDescription,
                     src => src.MapFrom(s => s.Description))
                     .ForMember(dest => dest.InstructorAvatar,
                     src => src.MapFrom(s => s.Thumbnail));

                cfg.CreateMap<Download, DownloadDTO>()
                    .ForMember(dest => dest.DownloadUrl,
                        src => src.MapFrom(s => s.Url))
                    .ForMember(dest => dest.DownloadTitle,
                        src => src.MapFrom(s => s.Title));

                cfg.CreateMap<Course, CourseDTO>()
                     .ForMember(dest => dest.CourseId, src =>
                     src.MapFrom(s => s.Id))
                     .ForMember(dest => dest.CourseTitle,
                     src => src.MapFrom(s => s.Title))
                     .ForMember(dest => dest.CourseDescription,
                     src => src.MapFrom(s => s.Description))
                     .ForMember(dest => dest.MarqueeImageUrl,
                     src => src.MapFrom(s => s.MarqueeImageUrl))
                     .ForMember(dest => dest.CourseImageUrl,
                     src => src.MapFrom(s => s.ImageUrl));

                cfg.CreateMap<Module, ModuleDTO>()
                    .ForMember(dest => dest.ModuleTitle,
                    src => src.MapFrom(s => s.Title));

            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);


            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VODContext db)
        {

            //app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //above... VODContext db injected to the method

            //Uncomment to reecreate the database. ALL DATA WILL BE LOST !
            //DbInitializer.RecreateDatabase(db);
            //Uncomment to seed the database
            //DbInitializer.Initialize(db);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
