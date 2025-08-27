namespace CarRental.ntier.API.Models.Requests.Locations;
public record CreateLocationRequest(
    string Name,
    string Address,
    string Phone
);
