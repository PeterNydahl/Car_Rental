namespace Car_Rental.Common.Exceptions;
public class SsnAlreadyExistsException : Exception
{
    public override string Message =>
        "SSN (Social Security Number) already exists! Please try again.";
}
