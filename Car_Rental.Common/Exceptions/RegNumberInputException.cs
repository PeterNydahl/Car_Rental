namespace Car_Rental.Common.Exceptions;
public class RegNumberInputException : Exception
{
    public override string Message =>
        "Registration number must consist of 3 letters followed by 3 digits! PLease try again.";    
}

