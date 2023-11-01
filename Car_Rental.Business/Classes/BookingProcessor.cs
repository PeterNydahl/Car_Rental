using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;

namespace Car_Rental.Business.Classes;

public class BookingProcessor
{
    #region ************* FÄLT & EGENSKAPER *************

    readonly IData _db;

    //public VehicleTypes[] VehicleTypes { get; init; }

    #endregion

    #region ************* KONSTRUKTOR *************
    public BookingProcessor(IData db)
    {
        _db = db;
        
        NewBooking(1, 1); // tesla, Bud
        NewBooking(5, 2); // jeep, Monk
        ReturnVehicle(5, 2, 6000);
    }
    #endregion

    #region ************* METODER *************

    #region Metoder som lägger till kund, fordon
    // Metod som lägger till ny kund
    public void AddCustomer(int ssn, string lastName, string firstName)
    {
        _db.AddCustomer(new Customer() { Id = _db.NextPersonId, Ssn = ssn, LastName = lastName, FirstName = firstName });
    }

    //Metod som lägger till nytt fordon
    public void AddVehicle(string regNo, string brand, int odometer, double costKm, string vehicleType, int costDay)
    {
        if (vehicleType == "Motorcycle")
        {
            _db.AddVehicle(new Motorcycle(_db.NextVehicleId, regNo.ToUpper(), brand, odometer, costKm, vehicleType, costDay, VehicleStatuses.Available));
        }
        _db.AddVehicle(new Car(_db.NextVehicleId, regNo.ToUpper(), brand, odometer, costKm, vehicleType, costDay, VehicleStatuses.Available));
    }
    #endregion

    #region Metoder som sköter bokningar
    // TODO : gör om parametrar till id för fordon och kund
    public void NewBooking(int vehicleId, int customerId)
    {
        try
        {
        // skapar en bokning
        Customer customer = (Customer)_db.GetPersons().First(c => c.Id == customerId);
        _db.AddBooking(new Booking(_db.NextBookingId, _db.GetVehicles().First(v => v.Id == vehicleId), customer, new(2023, 10, 30), VehicleStatuses.Open));

        // Ändrar status till Booked för fordonet i Vehicle-lista
        IVehicle updateVehicle = _db.GetVehicles().First(v => v.Id == vehicleId);
        updateVehicle.Status = VehicleStatuses.Booked;
        }
        catch
        {
            return;
        }
        
    }

    //TODO odometern fel när jag lämnar tillbaka bilen
    // (lämna tillbaka fordon) - gör uträkning och ändrar status
    public void ReturnVehicle(int vehicleId, int bookingId, int distance)
    {
        // leta upp fordonets som bokningen gäller, ändra status och odometer
        IVehicle? vehicle = _db.GetVehicles().First(v => v.Id == vehicleId); 
        vehicle.Status = VehicleStatuses.Available;
        vehicle.Odometer += distance;

        //leta upp bokningen som ska avslutas och ändra status
        IBooking? booking = _db.GetBookings().First(b => b.Id == bookingId);
        booking.Status = VehicleStatuses.Closed;

        // Gör uträkning
        booking.Cost = 0;
        booking.DayReturned = DateOnly.FromDateTime(DateTime.Now);
        booking.Distance = distance;

        //TODO ta ev bort denna konstiga kontroll
        if (booking.DayReturned == null || booking.Cost == null) return;

        DateTime date1 = DateTime.Now;
        //Konvertera datatyp för att möjliggöra beräkning av mellanskillnad i dagar.
        DateTime date2 = booking.DayRentedOut.ToDateTime(TimeOnly.Parse("00:00:00"));

        // Räkna ut mellanskillnad i dagar
        TimeSpan duration = (TimeSpan)(date1 - date2);
        double DifferenceInDays = duration.TotalDays;
        int RentedDays = (int)Math.Round(DifferenceInDays, 0);
        booking.Cost = RentedDays * vehicle.CostDay + (booking.Distance * vehicle.CostKm);
    }

   
    #endregion

    #region Metoder som returnerar
    // returnerar kunder 
    public IEnumerable<Customer> GetCustomers() => _db.GetPersons().Cast<Customer>();

    // returnerar fordon
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _db.GetVehicles();

    // returnerar bokning, bokningar
    public IBooking GetBooking(string regNo) => _db.GetBookings().First(b => b.RegNo.Equals(regNo) && b.Status == VehicleStatuses.Open);
    
    public IEnumerable<IBooking> GetBookings() => _db.GetBookings();

    //returnerar enums (vehicle types)
    public string[] GetVehicleTypes() => _db.GetVehicleTypes();
    #endregion

    #endregion METODER REGION ENDS
}
