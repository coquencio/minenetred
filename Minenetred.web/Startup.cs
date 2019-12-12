using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minenetred.Web.Context;
using Minenetred.Web.Infrastructure;
using Minenetred.Web.Services;
using Minenetred.Web.Services.Implementations;
using Redmine.Library.Core;
using Redmine.Library.Services;
using Redmine.Library.Services.Implementations;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;

namespace Minenetred.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.MSSqlServer(
                  configuration.GetConnectionString("DefaultConnection"),
                  "LogEvents",
                  autoCreateSqlTable: true,
                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning
                  )
              .CreateLogger();

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
            services.AddHttpClient<Redmine.Library.Services.IIssueService, Redmine.Library.Services.Implementations.IssueService>("issueClient");

            services.AddScoped<Redmine.Library.Services.ITimeEntryService, Redmine.Library.Services.Implementations.TimeEntryService>();
            services.AddHttpClient<Redmine.Library.Services.ITimeEntryService, Redmine.Library.Services.Implementations.TimeEntryService>("timeEntryClient");

            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient<IUserService, UserService>("userClient", c => c.BaseAddress = _uri);

            services.AddScoped<Redmine.Library.Services.IActivityService, Redmine.Library.Services.Implementations.ActivityService>();
            services.AddHttpClient<Redmine.Library.Services.IActivityService, Redmine.Library.Services.Implementations.ActivityService>("activityClient");

            services.AddScoped<Redmine.Library.Services.IProjectService, Redmine.Library.Services.Implementations.ProjectService>();
            services.AddHttpClient<Redmine.Library.Services.IProjectService, Redmine.Library.Services.Implementations.ProjectService>("projectClient");

            services.AddScoped<IConnectionService, ConnectionService>();
            services.AddHttpClient<IConnectionService, ConnectionService>("connectionClient");

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

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder
                   .WithOrigins("*")
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .AllowAnyHeader()));

            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Description = "Swagger core API" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddSerilog();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Projects}/{action=GetProjectsAsync}"
                    );
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<MinenetredContext>();
            var userName = UserPrincipal.Current.EmailAddress;
            var user = context.Users.SingleOrDefaultAsync(c=>c.UserName == userName);
            if (user.Result != null)
            {
                if (!string.IsNullOrEmpty(user.Result.BaseUri))
                {
                    ClientSettings.BaseAddress = user.Result.BaseUri;
                }
            }
        }
    }
}