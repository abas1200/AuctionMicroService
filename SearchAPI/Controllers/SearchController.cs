using AuctionMicroService.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : Controller
    {
        IRequestClient<GetAllAuctionsRequest> _client;

        public SearchController(IRequestClient<GetAllAuctionsRequest> client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuctionsBySearch()
        {
            var response = await _client.GetResponse<GetAllAuctions>(new GetAllAuctionsRequest());

            return Ok(response.Message);
        }


    }
}
