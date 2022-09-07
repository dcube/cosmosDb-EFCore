public class Restaurant
{
    public string RestaurantId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public Uri? ImageUrl { get; set; }
    public Uri? WebsiteUrl { get; set; }
    public List<string> FoodTypes { get; set; } = new();
    public int AveragePriceInEuros { get; set; }
    public int TimeToServeInMinutes { get; set; }
}