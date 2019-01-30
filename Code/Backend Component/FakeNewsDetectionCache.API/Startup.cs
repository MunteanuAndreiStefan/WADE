using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeNewsDetectionCache.API.Swagger;
using FakeNewsDetectionCache.Authentication;
using FakeNewsDetectionCache.Authentication.Authorization;
using FakeNewsDetectionCache.Authentication.Authorization.Handlers;
using FakeNewsDetectionCache.Authentication.Authorization.Requirements;
using FakeNewsDetectionCache.Entities;
using FakeNewsDetectionCache.Entities.Models;
using FakeNewsDetectionCache.Interfaces;
using FakeNewsDetectionCache.Service;
using FakeNewsDetectionCache.Service.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace FakeNewsDetectionCache.API
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

            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            ConfigureAuthentication(services);

            ConfigureAuthorization(services);


            ConfigureContext(services);

            ConfigureEntitiesServices(services);


            // Register the Swagger generator, defining 1 or more Swagger documents
            ConfigureSwagger(services);

            services.Configure<ProcessingUnitConfig>(Configuration.GetSection("ProcessingUnitConfig"));

        }

        public void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            })
                    .AddApiKeySupport(options => { });

            services.AddScoped<IGetApiKeyQuery, InMemoryGetApiKeyQuery>();
        }

        public void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.OnlyExtension, policy => policy.Requirements.Add(new OnlyExtensionRequirement()));
                options.AddPolicy(Policies.OnlyDevelopers, policy => policy.Requirements.Add(new OnlyDevelopersRequirement()));
                options.AddPolicy(Policies.OnlyProcessingService, policy => policy.Requirements.Add(new OnlyProcessingServiceRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, OnlyExtensionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, OnlyDevelopersAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, OnlyProcessingServiceAuthorizationHandler>();
        }

        public void ConfigureContext(IServiceCollection services)
        {
            //var connection = @"Server=localhost\SQLEXPRESS;Database=FakeNewsDetectionCache;Trusted_Connection=True;";
            var connection =
            Microsoft
                .Extensions
                .Configuration
                .ConfigurationExtensions
                .GetConnectionString(this.Configuration, "FakeNewsModelContainer");


            services.AddDbContext<Repository>(options => options.UseSqlServer(connection)
                                                        .UseLoggerFactory(new LoggerFactory().AddFile("Logs/ts-{Date}.txt"))
                                                        .EnableSensitiveDataLogging()
                                                        );

            //services.AddDbContext<Repository>(options => options.UseInMemoryDatabase("fakedb")
            //.UseLoggerFactory(new LoggerFactory().AddFile("LogsInMemory/ts-{Date}.txt"))
            //                                            .EnableSensitiveDataLogging()
            //                                            );
        }

        public void ConfigureEntitiesServices(IServiceCollection services)
        {
            services.AddScoped<ITwitterUserService, TwitterUserService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IDataToRdfService, DataToRdfService>();
        }


        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.OperationFilter<AddRequiredHeaderParameter>();
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fake News Detection Cache API V1");
                //c.RoutePrefix = string.Empty;
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}
