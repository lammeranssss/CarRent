namespace CarRental.API.Abstractions.Routing;

public static class ApiRoutes
{
    public const string Id = "{id:guid}";
    public static class Bookings
    {
        public const string Base = "api/bookings";
    }
    public static class Cars
    {
        public const string Base = "api/cars";
    }
    public static class Customers
    {
        public const string Base = "api/customers";
    }
    public static class Locations
    {
        public const string Base = "api/locations";
    }
    public static class Rentals
    {
        public const string Base = "api/rentals";
    }
}
