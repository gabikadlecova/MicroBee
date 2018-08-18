using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MicroBee.Web.Abstraction;
using MicroBee.Web.Context;
using MicroBee.Web.Models;
using MicroBee.Web.Services;

using Microsoft.EntityFrameworkCore.SqlServer;

namespace MicroBee.Web
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

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<MicroBeeDbContext>()
				.AddDefaultTokenProviders();

			//Db context
			string connection = Configuration.GetConnectionString("MicroBeeDatabase");
			services.AddDbContext<MicroBeeDbContext>(options => options.UseSqlServer(connection));
			
			//Repositories

			//Services
			services.AddTransient<IMicroItemService, MicroItemService>();
			services.AddTransient<IMicroItemRepository, MicroItemRepository>();

			services.AddMvc();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MicroBeeDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseHttpsRedirection();
			app.UseAuthentication();

            app.UseMvc();

			context.Database.Migrate();	
        }
    }
}
