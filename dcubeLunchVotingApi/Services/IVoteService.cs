namespace dcubeLunchVotingApi.Services
{
    public interface IVoteService
    {
        Task<Vote> AddVote(CreateVote createVote);
        Task<Vote?> GetVote(string id, string? shortDate);
        Task<IEnumerable<Vote>> GetAllVotesForDay(string shortDate,
                                                  string? restaurantId = null);
    }
}
