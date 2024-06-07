using AutoMapper;
using System;
using AuctionMicroService.Core.Abstraction;
using AuctionMicroService.Core.DTOs;
using AuctionMicroService.Core.Entities;
using AuctionMicroService.Core.Services.Interfaces;

namespace AuctionMicroService.Core.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IMapper _mapper;
        private readonly IAuctionStore _testStore;
        public AuctionService(IMapper mapper , IAuctionStore testStore)
        {
            _testStore = testStore;
            _mapper = mapper;
        }
        public async Task<List<AuctionDto>> GetAllAuctions()
        {
            var result = await _testStore.GetAllAuctions();
            return _mapper.Map<List<AuctionDto>>(result);
        }
    }
}
