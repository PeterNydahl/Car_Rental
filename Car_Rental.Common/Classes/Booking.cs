using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Car_Rental.Common.Classes;
public class Booking : IBooking
{
    private IVehicle vehicle;
    private IPerson customer;
    public string? RegNo { get; init; }
    public string? NameWithSsn { get; init; }
    public int KmRented { get; init; }
    public int? KmReturned { get; set; }
    public DateOnly DayRentedOut { get; init; }
    public DateOnly? DayReturned { get; set; }
    public double? Cost { get; set; }
    public VehicleStatuses Status { get; set; }

    //Konstruktor
    public Booking(IVehicle vehicle, IPerson customer, DateOnly dayRentedOut)
    {
        this.vehicle = vehicle;
        NameWithSsn = $"{customer.FirstName} {customer.LastName}({customer.Ssn})";

        RegNo = vehicle.RegNo;
        KmRented = vehicle.Odometer;
        KmReturned = null;
        DayRentedOut = dayRentedOut;
        DayReturned = null; 
        Cost = null;
        Status = vehicle.Status;
    }
    //räknar ut kostnad när bilen lämnas tillbaka
    public void ReturnVehicle(int kmReturned)
    {
        Cost = 0;
        DayReturned = DateOnly.FromDateTime(DateTime.Now);
        KmReturned = kmReturned;

        if (KmReturned == null || DayReturned == null || Cost == null) return;

        DateTime date1 = DateTime.Now;
        //Konvertera datatyp för att möjliggöra beräkning av mellanskillnad i dagar.
        DateTime date2 = DayRentedOut.ToDateTime(TimeOnly.Parse("00:00:00"));

        // Räkna ut mellanskillnad i dagar
        TimeSpan duration = (TimeSpan)(date1 - date2);
        double DifferenceInDays = duration.TotalDays;
        int RentedDays = (int)Math.Round(DifferenceInDays, 0);
        // räkna ut kostnad
        Cost = RentedDays * vehicle.CostDay + (KmReturned - KmRented) * vehicle.CostKm;
    }

}
