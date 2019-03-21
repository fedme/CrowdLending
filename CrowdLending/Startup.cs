using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using CrowdLending.Database;
using CrowdLending.Filters;
using CrowdLending.Models;
using CrowdLending.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Validation;

namespace CrowdLending
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
            // DB Context TODO: Change to real database
            services.AddDbContext<DefaultDbContext>(options =>
            {
                options.UseInMemoryDatabase("debugdb");
                options.UseOpenIddict<Guid>();
            });

            // CORS TODO: change policy in production
            services.AddCors(options => options.AddPolicy("DebugPolicy",
                policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            // Add OpenIddict auth server
            AddOpenIddict(services);

            // Custom services
            services.AddScoped<IProjectService, DefaultProjectService>();
            services.AddScoped<IUserService, DefaultUserService>();
            services.AddScoped<IInvestmentService, DefaultInvestmentService>();

            // Base MVC services
            services.AddMvc(options =>
                {
                    options.Filters.Add<JsonExceptionFilter>(); // Output json exceptions (instead of HTML ones)
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddRouting(options => options.LowercaseUrls = true);

            // ASP.Net Core Identity
            AddIdentityServices(services);

            // Add Swagger support
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "CrowdLending API";
                    document.Info.Description = "An example of ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "Federico Meini",
                        Email = "federico.meini@gmail.com",
                        Url = "https://github.com/fedme"
                    };
                };
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
                app.UseHsts();
            }

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();

            // Add CORS
            app.UseCors("DebugPolicy");
            

            // Add authentication
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static void AddIdentityServices(IServiceCollection services)
        {
            // Configure ASP.NET Core Identity
            var builder = services.AddIdentityCore<UserEntity>();
            builder = new IdentityBuilder(builder.UserType, typeof(UserRoleEntity), builder.Services);

            builder.AddRoles<UserRoleEntity>()
                .AddEntityFrameworkStores<DefaultDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<UserEntity>>();
        }

        private static void AddOpenIddict(IServiceCollection services)
        {
            // Add OpenIddict services
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DefaultDbContext>()
                        .ReplaceDefaultEntities<Guid>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();

                    options.EnableTokenEndpoint("/token");

                    options.AllowPasswordFlow();
                    options.AcceptAnonymousClients();

                })
                .AddValidation();

            // ASP.NET Core Identity should use the same claim names as OpenIddict
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });
        }
    }
}
