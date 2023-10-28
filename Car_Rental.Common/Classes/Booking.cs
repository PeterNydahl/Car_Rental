using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Car_Rental.Common.Classes;
public class Booking : IBooking
{
    //private IVehicle vehicle;
    //private IPerson customer;
    // testar att byta ut dessa fält mot props
    public IVehicle Vehicle { get; init; }
    public IPerson Customer { get; set; }


    public string? RegNo { get; init; }
    public string? NameWithSsn { get; init; }
    public int KmRented { get; init; }
    public int? KmReturned { get; set; }
    public DateOnly DayRentedOut { get; init; }
    public DateOnly? DayReturned { get; set; }
    public double? Cost { get; set; }
    public VehicleStatuses Status { get; set; }

    public Booking(IVehicle vehicle, IPerson customer, DateOnly dayRentedOut, VehicleStatuses status)
    {
        Vehicle = vehicle;
        Customer = customer;
        NameWithSsn = $"{Customer.FirstName} {Customer.LastName}({Customer.Ssn})";

        RegNo = Vehicle.RegNo;
        KmRented = vehicle.Odometer;
        KmReturned = null;
        DayRentedOut = dayRentedOut;
        Cost = null;
        Status = status;
        DayReturned = null;
    }

}
