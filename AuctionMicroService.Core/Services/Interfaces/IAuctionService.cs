using Microsoft.AspNetCore.Mvc;
using AuctionMicroService.Core.DTOs;

namespace AuctionMicroService.Core.Services.Interfaces
{
    public interface IAuctionService
    {
        public  Task<List<AuctionDto>> GetAllAuctions();

    }
}
