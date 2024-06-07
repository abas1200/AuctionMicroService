using MassTransit;
using System.Net.Security;
using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuctionMicroService.Config
{
    public static class ServiceCollectionExtensions
    { 
        internal static IServiceCollection ConfigRabbitMqHost(this IServiceCollection services, IConfiguration configuration)
        {
           services.AddOptions<RabbitMqTransportOptions>().Bind(configuration.GetSection("RabbitMqHostSettings"));
           services.AddOptions<RabbitMqSslOptions>().Bind(configuration.GetSection("RabbitMqSslSettings"));
           return services;
        }

        internal static void CreateRabbitMqBus(this IRabbitMqBusFactoryConfigurator busFactory,
            IBusRegistrationContext context)
        {
            var option = context.GetRequiredService<IOptions<RabbitMqTransportOptions>>();
            var hostSetting = option.Value;
            busFactory.Host(hostSetting.Host, hostSetting.Port, hostSetting.VHost, config =>
            {
                config.Username(hostSetting.User);
                config.Password(hostSetting.Pass);

                if (hostSetting.UseSsl)
                {
                    var sslOptions = context.GetRequiredService<IOptions<RabbitMqSslOptions>>().Value;
                    config.UseSsl(s =>
                    {
                        //if (sslOptions.ServerName.IsNotNullOrEmpty())
                        //    s.ServerName = sslOptions.ServerName;
                        //if (sslOptions.CertPath.IsNotNullOrEmpty())
                        //    s.CertificatePath = sslOptions.CertPath;
                        //if (sslOptions.CertPassphrase.IsNotNullOrEmpty())
                        //    s.CertificatePassphrase = sslOptions.CertPassphrase;

                        s.UseCertificateAsAuthenticationIdentity = sslOptions.CertIdentity;

                        //todo: needs to move to appsettings.
                        s.Protocol = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                        if (sslOptions.Trust)
                        {
                            s.AllowPolicyErrors(SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors
                                | SslPolicyErrors.RemoteCertificateNotAvailable);
                        }
                    });
                }
            });
        }

  public static IServiceCollection AddTestMassTransit(this IServiceCollection services
   , IConfiguration configurationManager
   , Action<IBusRegistrationConfigurator> configure
   , Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>?
       rabbitMqBusFactoryConfigurator = null)
        {
            services.ConfigRabbitMqHost(configurationManager);

            services.AddMassTransit(c =>
            {
                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.CreateRabbitMqBus(context);


                    if (rabbitMqBusFactoryConfigurator != null)
                        rabbitMqBusFactoryConfigurator(cfg, context);
                    cfg.ConfigureEndpoints(context);
                });
                c.SetKebabCaseEndpointNameFormatter();
                configure(c);
            });

            services.AddOptions<MassTransitHostOptions>()
                 .Configure(options =>
                 {
                     options.WaitUntilStarted = true;
                 });

            return services;
        }


    }
}
