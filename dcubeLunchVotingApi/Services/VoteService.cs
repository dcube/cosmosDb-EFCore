using Microsoft.EntityFrameworkCore;

namespace dcubeLunchVotingApi.Services
{
    public class VoteService : IVoteService
    {
        private readonly IDbContextFactory<LunchVotingContext> contextFactory;

        public VoteService(IDbContextFactory<LunchVotingContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<Vote> AddVote(CreateVote createVote)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var voteEntry = context.Votes.Add(new Vote
            {
                VoteId = Guid.NewGuid().ToString(),
                RestaurantId = createVote.RestaurantId,
                RestaurantName = createVote.RestaurantName,
                User = createVote.User,
                ShortDate = createVote.Date.ToShortDateString()
            });

            await context.SaveChangesAsync();

            return voteEntry.Entity;
        }

        public async Task<Vote?> GetVote(string id, string? shortDate)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var vote = shortDate == null 
                ? await context.Votes.FirstOrDefaultAsync(v => v.VoteId == id) 
                : await context.Votes.FindAsync(id, shortDate);

            if (vote == null) return null;

            await context.Entry(vote)
                    .Reference(vote => vote.Restaurant)
                    .LoadAsync();

            return vote;
        }

        public async Task<IEnumerable<Vote>> GetAllVotesForDay(string shortDate,
                                                               string? restaurantId = null)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var votesQuery = context.Votes.WithPartitionKey(shortDate);

            if (restaurantId != null)
                votesQuery = votesQuery.Where(v => v.RestaurantId == restaurantId);

            return await votesQuery.AsNoTracking().ToListAsync();
        }


    }
}
