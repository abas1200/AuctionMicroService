using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using AuctionMicroService.Core.Abstraction;
using AuctionMicroService.Core.Services;
using AuctionMicroService.Core.Services.Interfaces;
using AuctionMicroService.Core.Store;

namespace AuctionMicroService
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IAuctionStore, AuctionStore>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, ConfigurationManager configuration)

        {

            var identitySettings = configuration.GetSection("IdentitySettings");
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {


                    options.Authority = identitySettings.GetValue<string>("Authority");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        NameClaimType = "username"
                    };

                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", identitySettings.GetValue<string>("Scope"));
                });
            });

            return services;
        }



    }
}
