namespace Car_Rental.Common.Exceptions;
public class DropdownMenuException : Exception
{
    public override string Message => 
        "You have to choose an option in the drop-down menu! Please try again.";
}
