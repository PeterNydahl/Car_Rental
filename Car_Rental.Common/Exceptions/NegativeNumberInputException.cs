namespace Car_Rental.Common.Exceptions;
public class NegativeNumberInputException : Exception
{
    public override string Message =>
        "Input can't be a negative number! Please try again.";
}
