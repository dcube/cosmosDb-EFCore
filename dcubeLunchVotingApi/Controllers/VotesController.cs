using dcubeLunchVotingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace dcubeLunchVotingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService voteService;

        public VotesController(IVoteService voteService)
        {
            this.voteService = voteService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Vote), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string shortDate,
                                             [FromQuery] string? restaurantId)
        {
            IEnumerable<Vote> votes = await voteService.GetAllVotesForDay(
                shortDate,
                restaurantId);

            return Ok(votes);
        }

        [HttpGet]
        [Route("{voteId}", Name = "GetVoteById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Vote), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] string voteId, [FromQuery] string? shortDate)
        {
            Vote? vote = await voteService.GetVote(voteId, shortDate);

            return vote == null ? NotFound() : Ok(vote);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateVote([FromBody] CreateVote createVote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Vote vote = await voteService.AddVote(createVote);

            return CreatedAtRoute("GetVoteById", new { voteId = vote.VoteId }, vote);
        }
    }
}
