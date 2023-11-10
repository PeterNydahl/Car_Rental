namespace Car_Rental.Common.Exceptions;
public class InputValueTooBigException : Exception
{
    public override string Message =>
        "The input value was to big. Please try again.";
}
