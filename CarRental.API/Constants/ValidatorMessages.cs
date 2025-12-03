namespace CarRental.API.Constants;

public static class ValidatorMessages
{
    public static string EnumMustBeSpecifiedMessage(string propertyName)
    {
        return $"{propertyName} must be specified and cannot be Unknown.";
    }
}
