using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHibernate;
using OnepMini.ETP;
using OnepMini.ETP.OnePlannerMigrations.Framework;
using OnepMini.OrmNhib.DBInterface;
using OnepMini.OrmNhib.Initializer;

namespace OnepMini
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
            services.AddControllers();

            services.AddSingleton<INHibernateInitializer, NHibernateInitializer>();
            services.AddSingleton(typeof(ISessionFactory), serviceProvider =>
            {
                var nhibernateInitializer = serviceProvider.GetService<INHibernateInitializer>();
                return nhibernateInitializer.GetSessionFactory();
            });

            services.AddSingleton<INetworkDBStorageInitializer, NetworkDBStorageInitializer>();
            services.AddSingleton<INHMigrationRunner, NHMigrationRunner>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            INetworkDBStorageInitializer networkDBStorageInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            networkDBStorageInitializer.RunMigrations();
        }
    }
}
