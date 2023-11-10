namespace Car_Rental.Common.Exceptions 
{
    public class RegNoAlreadyExistsException : Exception
    {
        public override string Message =>
            "The registration number already exists. Please try again.";
    }
}
