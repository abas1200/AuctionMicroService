using MassTransit;
using AuctionMicroService.Consumers;

namespace AuctionMicroService;

/// <summary>
/// Extesion class to register consumers with MassTransit
/// </summary>
public static partial class BusRegistrationExtensions
{
    /// <summary>
    /// Method to register consumer
    /// </summary>
    /// <param name="busConfigurator"></param>
    public static void RegisterConsumers(this IBusRegistrationConfigurator busConfigurator)
    {
        
        busConfigurator.AddConsumer<GetAllAuctionsConsumer>();
    }
}