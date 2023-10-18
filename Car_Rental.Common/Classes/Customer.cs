using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Customer : IPerson
{
    public int Ssn { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }
}
