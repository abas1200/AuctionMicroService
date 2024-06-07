using AuctionMicroService.Contracts;
using MassTransit;
using MassTransit.Transports;
using AuctionMicroService.Core.Services.Interfaces;

namespace AuctionMicroService.Consumers
{
    public class GetAllAuctionsConsumer : IConsumer<GetAllAuctionsRequest>
    {
        private readonly IAuctionService _auctionService;
        public GetAllAuctionsConsumer(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        public async Task Consume(ConsumeContext<GetAllAuctionsRequest> context)
        {
            var auctions = await _auctionService.GetAllAuctions();
            await context.RespondAsync<GetAllAuctions>(auctions);
        }
    }
}
