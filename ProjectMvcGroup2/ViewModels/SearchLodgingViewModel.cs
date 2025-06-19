using ProjectLibraryGroup2;

public class SearchLodgingViewModel
{
    public DateOnly? CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }
    public string? GuestEmail { get; set; }
    public bool ShowAll { get; set; }
    public List<LodgingDates> LodgingSearchResult { get; set; } = new();
}
