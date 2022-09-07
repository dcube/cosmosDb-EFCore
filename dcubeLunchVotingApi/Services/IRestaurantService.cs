namespace dcubeLunchVotingApi.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsForFoodType(string foodType);
    }
}
