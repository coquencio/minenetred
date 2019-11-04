using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minenetred.Web.Context;
using Minenetred.Web.Infrastructure;
using Minenetred.Web.Services;
using Minenetred.Web.Services.Implementations;
using Redmine.Library.Core;
using Redmine.Library.Services;
using Redmine.Library.Services.Implementations;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net.Http;

namespace Minenetred.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string _secretEncrytionKey;
        private Uri _uri;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _secretEncrytionKey = Configuration["EncryptionKey"];
            _uri = new Uri("https://dev.unosquare.com/redmine/");
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region Library Services
            services.AddSingleton<IUriHelper, UriHelper>();
            services.AddSingleton<ISerializerHelper, SerializerHelper>();

            services.AddScoped<Redmine.Library.Services.IIssueService, Redmine.Library.Services.Implementations.IssueService>();
            services.AddHttpClient<Redmine.Library.Services.IIssueService, Redmine.Library.Services.Implementations.IssueService>("issueClient", c=> c.BaseAddress = _uri);

            services.AddScoped<Redmine.Library.Services.ITimeEntryService, Redmine.Library.Services.Implementations.TimeEntryService>();
            services.AddHttpClient<Redmine.Library.Services.ITimeEntryService, Redmine.Library.Services.Implementations.TimeEntryService>("timeEntryClient", c => c.BaseAddress = _uri);

            //services.AddScoped<IUserService>(s => new UserService(_client));
            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient<IUserService, UserService>("userClient", c => c.BaseAddress = _uri);

            services.AddScoped<Redmine.Library.Services.IActivityService, Redmine.Library.Services.Implementations.ActivityService>();
            services.AddHttpClient<Redmine.Library.Services.IActivityService, Redmine.Library.Services.Implementations.ActivityService>("activityClient", c => c.BaseAddress = _uri);

            services.AddScoped<Redmine.Library.Services.IProjectService, Redmine.Library.Services.Implementations.ProjectService>();
            services.AddHttpClient<Redmine.Library.Services.IProjectService, Redmine.Library.Services.Implementations.ProjectService>("projectClient", c => c.BaseAddress = _uri);

            services.AddScoped<IEncryptionService>(s => new EncryptionService(_secretEncrytionKey));

            #endregion Library Services

            #region Project services

            services.AddScoped<IUsersManagementService, UsersManagementService>();
            services.AddScoped<Services.IProjectService, Services.Implementations.ProjectService>();
            services.AddScoped<Services.ITimeEntryService, Services.Implementations.TimeEntryService>();
            services.AddScoped<Services.IIssueService, Services.Implementations.IssueService>();
            services.AddScoped<Services.IActivityService, Services.Implementations.ActivityService>();
            services.AddScoped<IPopulateSelectorService, PopulateSelectorService>();
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
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Description = "Swagger core API" }));
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}