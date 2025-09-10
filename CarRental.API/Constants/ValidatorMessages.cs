namespace CarRental.API.Constants;

public static class ValidatorMessages
{
    public static string Required(string fieldName) =>
        $"{fieldName} is required.";

    public static string CannotBeEmpty(string fieldName) =>
        $"{fieldName} cannot be empty.";

    public static string MustBeValid(string fieldName) =>
        $"Invalid {fieldName.ToLower()} value.";

    public static string Length(string fieldName, int min, int max) =>
        $"{fieldName} must be between {min} and {max} characters.";

    public static string PositiveValue(string fieldName) =>
        $"{fieldName} must be a positive value.";

    public static string YearRange(string fieldName, int min, int max) =>
        $"{fieldName} must be between {min} and {max}.";
}
