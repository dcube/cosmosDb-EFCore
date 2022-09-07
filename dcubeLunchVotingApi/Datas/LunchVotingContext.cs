using Microsoft.EntityFrameworkCore;

public class LunchVotingContext : DbContext
{
    public LunchVotingContext(DbContextOptions<LunchVotingContext> options)
        : base(options)
    {

    }

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vote>()
            .ToContainer(nameof(Vote))
            .HasNoDiscriminator()
            .HasDefaultTimeToLive(60*60*24*30)
            .HasPartitionKey(vote => vote.ShortDate)
            .HasKey(vote => new { vote.VoteId, vote.ShortDate });

        modelBuilder.Entity<Restaurant>()
            .ToContainer(nameof(Restaurant))
            .HasNoDiscriminator()
            .UseETagConcurrency()
            .HasPartitionKey(restaurant => restaurant.RestaurantId)
            .HasKey(restaurant => restaurant.RestaurantId);

        modelBuilder.Entity<Restaurant>()
            .HasData(new Restaurant[] { 
                new Restaurant { 
                    RestaurantId = "1",
                    Name = "Bistronomique",
                    Address = "1 rue de Paris, 75014 Paris",
                    FoodTypes = new List<string> { "Bistro" }, 
                    AveragePriceInEuros = 15,
                    TimeToServeInMinutes = 60
                },
                new Restaurant { 
                    RestaurantId = "2", 
                    Name = "XL Burger", 
                    Address = "2 rue de Paris, 75014 Paris",
                    FoodTypes = new List<string> { "Burger" },
                    AveragePriceInEuros = 11,
                    TimeToServeInMinutes = 20 
                },
                new Restaurant { 
                    RestaurantId = "3", 
                    Name = "Le Fast",
                    Address = "3 rue de Paris, 75014 Paris",
                    FoodTypes = new List<string> { "Burger", "Bistro", "Française" }, 
                    AveragePriceInEuros = 9,
                    TimeToServeInMinutes = 25 
                },
            });
    }
}