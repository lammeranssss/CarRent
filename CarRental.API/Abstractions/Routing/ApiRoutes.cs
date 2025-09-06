namespace CarRental.API.Abstractions.Routing;

public static class ApiRoutes
{
    public const string Base = "api/[controller]";
    public const string GetById = "{id:guid}";
    public const string Update = "{id:guid}";
    public const string Delete = "{id:guid}";
}
