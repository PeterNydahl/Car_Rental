namespace Car_Rental.Common.Exceptions 
{
    public class RegNoAlreadyExistsException : Exception
    {
        public override string Message =>
            "Registration number already exists. Please try again.";
    }
}
