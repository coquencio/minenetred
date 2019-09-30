using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minenetred.web.Context;
using Minenetred.web.Infrastructure;
using Redmine.library.Services;
using Redmine.library.Services.Implementations;
using System.DirectoryServices.AccountManagement;
using Minenetred.web.Services;
using Minenetred.web.Services.Implementations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Minenetred.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string _secretEncrytionKey;
        private HttpClient _client;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _secretEncrytionKey = Configuration["EncryptionKey"];
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://dev.unosquare.com/redmine/");

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region Library Services
            services.AddScoped<Redmine.library.Services.IIssueService>(s => new Redmine.library.Services.Implementations.IssueService(_client));
            services.AddScoped<Redmine.library.Services.ITimeEntryService>(s => new Redmine.library.Services.Implementations.TimeEntryService(_client));
            services.AddScoped<IUserService>(s => new UserService(_client));
            services.AddScoped<Redmine.library.Services.IActivityService>(s => new Redmine.library.Services.Implementations.ActivityService(_client));
            services.AddScoped<Redmine.library.Services.IProjectService>(s => new Redmine.library.Services.Implementations.ProjectService(_client));
            services.AddScoped<IEncryptionService>(s => new EncryptionService(_secretEncrytionKey));
            #endregion
            #region Project services
            services.AddScoped<IUsersManagementService, UsersManagementService>();
            services.AddScoped<Services.IProjectService, Services.Implementations.ProjectService>();
            services.AddScoped<Services.ITimeEntryService, Services.Implementations.TimeEntryService>();
            services.AddScoped<Services.IIssueService, Services.Implementations.IssueService>();
            services.AddScoped<Services.IActivityService, Services.Implementations.ActivityService>();
            #endregion
            var mappingConfig = new MapperConfiguration(mc =>
           {
               mc.AddProfile(new MappingProfile());
           });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<MinenetredContext>(
                o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           services.AddSwaggerGen(c=> {
                c.SwaggerDoc("v1", new Info {Title = "My API", Description="Swagger core API"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}"
                    );
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}