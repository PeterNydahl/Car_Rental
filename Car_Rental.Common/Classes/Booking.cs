using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Car_Rental.Common.Classes;
public class Booking : IBooking
{
    //private IVehicle vehicle;
    //private IPerson customer;
    // testar att byta ut dessa fält mot props
    public IVehicle vehicle { get; init; }
    public IPerson customer { get; set; }


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
        this.vehicle = vehicle;
        NameWithSsn = $"{customer.FirstName} {customer.LastName}({customer.Ssn})";

        RegNo = vehicle.RegNo;
        KmRented = vehicle.Odometer;
        KmReturned = null;
        DayRentedOut = dayRentedOut;
        Cost = null;
        Status = status;
        DayReturned = null;
    }

}
