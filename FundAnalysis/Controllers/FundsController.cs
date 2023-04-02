using FundAnalysis.API.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundsController : ControllerBase
    {
        private readonly IFundRepository _fundRepository;
        public FundsController(IFundRepository fundRepository)
        {
            _fundRepository = fundRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetRelatedFundProfits(string fundCode)
        {
            try
            {
                var profitsWithMessages = await _fundRepository.GetProfitsWithMessagesWithKnownPeriods(fundCode);
                var fundNotFoundMessage = profitsWithMessages.FirstOrDefault(x => x.Contains("Fund is not found for"));

                if (fundNotFoundMessage != null)
                    return BadRequest(fundNotFoundMessage);
                return Ok(profitsWithMessages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
