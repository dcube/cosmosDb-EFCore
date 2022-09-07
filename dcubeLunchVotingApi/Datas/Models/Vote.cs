public class Vote
{
    public string VoteId { get; set; } = null!;
    public string RestaurantName { get; set; } = null!;
    public Restaurant? Restaurant { get; set; }
    public string RestaurantId { get; set; } = null!;
    public string ShortDate { get; set; } = null!;
    public string User { get; set; } = null!;
}
