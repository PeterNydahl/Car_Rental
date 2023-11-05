namespace Car_Rental.Common.Exceptions;

public class InvalidSsnException : Exception
{
    public override string Message => "SSN has to be 6 digits! Please try again.";
}
