using AuctionMicroService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionMicroService.Core.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Auction> Auctions { get; set; }
}
