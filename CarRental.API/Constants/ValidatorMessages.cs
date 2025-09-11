namespace CarRental.API.Constants;

public static class ValidatorMessages
{
    public static string Required(string fieldName) =>
        $"{fieldName} is required.";
}
