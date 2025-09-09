namespace CarRental.API.Abstractions.Routing;

public static class ApiRoutes
{
    public const string ById = "/{id:guid}";

    public static class Bookings
    {
        public const string Base = "api/bookings";
        public const string ById = Base + ApiRoutes.ById;
    }

    public static class Cars
    {
        public const string Base = "api/cars";
        public const string ById = Base + ApiRoutes.ById;
    }

    public static class Customers
    {
        public const string Base = "api/customers";
        public const string ById = Base + ApiRoutes.ById;
    }

    public static class Locations
    {
        public const string Base = "api/locations";
        public const string ById = Base + ApiRoutes.ById;
    }

    public static class Rentals
    {
        public const string Base = "api/rentals";
        public const string ById = Base + ApiRoutes.ById;
    }
}
