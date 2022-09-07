using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace dcubeLunchVotingApi.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IDbContextFactory<LunchVotingContext> contextFactory;

        public RestaurantService(IDbContextFactory<LunchVotingContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsForFoodType(string foodType)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var sql = "SELECT * FROM c " +
                        "WHERE EXISTS (" +
                        "SELECT VALUE FoodType " +
                        "FROM FoodType IN c.FoodTypes " +
                        "WHERE FoodType={0})";

            var restaurants = await context.Restaurants
                .FromSqlRaw(sql, foodType)
                .ToListAsync();

            return restaurants;
        }

        public async Task ExecuteCosmosClientRequest()
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var container = context.Database.GetCosmosClient()
                .GetContainer("LunchVotingDb", "Restaurant");

            await container.Scripts.ExecuteStoredProcedureAsync<string>(
                storedProcedureId: "myStoredProcedure",
                partitionKey: new PartitionKey("partitionKey"),
                parameters: null);
        }
    }
}
