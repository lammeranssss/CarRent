namespace CarRental.ntier.BLL.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
