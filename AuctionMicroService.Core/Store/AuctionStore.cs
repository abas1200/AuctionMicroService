using Microsoft.EntityFrameworkCore;
using AuctionMicroService.Core.Abstraction;
using AuctionMicroService.Core.Data;
using AuctionMicroService.Core.DTOs;
using AuctionMicroService.Core.Entities;

namespace AuctionMicroService.Core.Store
{
    public class AuctionStore: IAuctionStore
    {
        private readonly AuctionDbContext _dbContext;

        public AuctionStore(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctions()
        {
            var result = await _dbContext.Auctions
           .Include(x => x.Item)
           .OrderBy(x => x.Item.Make)
           .ToListAsync();

            return result; 
        }

    }
}
