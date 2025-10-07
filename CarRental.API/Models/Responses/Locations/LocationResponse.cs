namespace CarRental.API.Models.Responses.Locations;

public class LocationResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int AvailableCarsCount { get; set; }
    public int TotalCarsCount { get; set; }
    public bool CanAcceptReturns { get; set; }
}
