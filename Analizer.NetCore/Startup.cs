using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analizer.NetCore.Models;
using Analizer.NetCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Analizer.NetCore
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FireRiskContext>(options => options.UseSqlServer(connectionString));

            string connectionString2 = Configuration.GetConnectionString("BaseConnection");
            services.AddDbContext<HidrometDbContext>(options => options.UseSqlServer(connectionString2));

            services.AddScoped<IDbFillMeneger, DbFillMeneger>();
            services.AddScoped<IFireRiskMeneger, FireRiskMeneger>();
            services.AddScoped<IDataReaderFromBaseDb, DataReaderFromBaseDb>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        static void FillCityDb(FireRiskContext context)
        {
            if (context.Cities.FirstOrDefault() == null)
            {
                string[] cities = { "Yerevan", "Gyumri", "Vanadzor", "Ijevan", "Ashtarak", "Hrazdan", "Gavar", "Armavir", "Artashat", "Exegnadzor", "Kapan" };
                List<City> citys = new List<City>();

                foreach (string city in cities)
                {
                    citys.Add(new City
                    {
                        Name = city
                    });
                }

                context.Cities.AddRange(citys);
                context.SaveChanges();
            }
            else return;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,FireRiskContext context,HidrometDbContext hidrometContext)
        {
            //method call which we need in demo versia 
            hidrometContext.Database.Migrate();

            context.Database.Migrate();

            FillCityDb(context);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
