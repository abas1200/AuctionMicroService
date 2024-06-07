using AuctionMicroService.Core.Entities;

namespace AuctionMicroService.Core.Abstraction
{
    public interface IAuctionStore
    {
        public Task<IEnumerable<Auction>> GetAllAuctions();
    }
}
