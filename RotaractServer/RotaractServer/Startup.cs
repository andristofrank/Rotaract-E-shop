using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using RotaractServer.Authorization;
using RotaractServer.Models.DAO_Models;
using RotaractServer.Profiles;
using RotaractServer.Services;
using PathString = Microsoft.Owin.PathString;

namespace RotaractServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable CORS (cross origin resource sharing) for making request using browser from different domains
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

           

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,

                //The Path For generating the Toekn
                TokenEndpointPath = new PathString("/auth/login"),

                

                //Setting the Token Expired Time (24 hours)
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                //MyAuthorizationServerProvider class will validate the user credentials
                Provider = new RotaractAuthorizationServerProvider()
            };

            //Token Generations
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);
        }


        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddResponseCaching();
            services.AddMvc();
            services.AddScoped<IRotaractRepository, RotaractRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("RotaractCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:4200/")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            //services.AddResponseCaching(options =>
            //{
            //    options.MaximumBodySize = 1024;
            //    options.UseCaseSensitivePaths = true;
            //});
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCaching();
            app.UseCors();

            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromSeconds(10)
            //        };
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "Accept-Encoding" };

            //    await next();
            //});

            app.UseMvc();
        }

    }
}