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
        //TODO ändra till getnames så att du får strängar
        //VehicleTypes = (VehicleTypes[])Enum.GetValues(typeof(VehicleTypes));
        //VehicleTypes = (VehicleTypes[])Enum.GetValues(typeof(VehicleTypes));

        // skapa nya bokningar genom att anropa NewBooking
        NewBooking("GHI789", 240927); // tesla, Bud
        NewBooking("JKL012", 171010); // jeep, Monk
        ReturnVehicle("JKL012", 6000);
    }
    #endregion

    #region ************* METODER *************

    #region Metoder som lägger till kund, fordon
    // Metod som lägger till ny kund
    public void AddCustomer(int ssn, string lastName, string firstName)
    {
        _db.AddCustomer(new Customer() { Ssn = ssn, LastName = lastName, FirstName = firstName });
    }
    //Metod som lägger till ny fordon
    public void AddVehicle(string regNo, string brand, int odometer, double costKm, string vehicleType, int costDay)
    {
        if (vehicleType == "Motorcycle")
        {
            _db.AddVehicle(new Motorcycle(regNo.ToUpper(), brand, odometer, costKm, vehicleType, costDay, VehicleStatuses.Available));
        }
        _db.AddVehicle(new Car(regNo.ToUpper(), brand, odometer, costKm, vehicleType, costDay, VehicleStatuses.Available));
    }
    #endregion

    #region Metoder som sköter bokningar
    public void NewBooking(string regNr, int ssn)
    {
        // skapar en bokning
        Customer customer = (Customer)_db.GetPersons().First(c => c.Ssn == ssn);
        _db.AddBooking(new Booking(_db.GetVehicles().First(v => v.RegNo == regNr), customer, new(2023, 10, 10), VehicleStatuses.Open));

        // Ändrar status till Booked för fordonet i Vehicle-lista
        IVehicle updateVehicle = _db.GetVehicles().First(v => v.RegNo == regNr);
        updateVehicle.Status = VehicleStatuses.Booked;
    }

    // (lämna tillbaka fordon) - gör uträkning och ändrar status
    public void ReturnVehicle(string regNr, int kmReturned)
    {
        // leta upp fordonets som bokningen gäller, ändra status och odometer
        IVehicle? vehicle = _db.GetVehicles().First(v => v.RegNo == regNr); 
        vehicle.Status = VehicleStatuses.Available;
        vehicle.Odometer = kmReturned;

        //leta upp bokningen som ska avslutas och ändra status
        IBooking? booking = _db.GetBookings().First(bv => bv.RegNo == regNr);
        booking.Status = VehicleStatuses.Closed;

        // Gör uträkning
        booking.Cost = 0;
        booking.DayReturned = DateOnly.FromDateTime(DateTime.Now);
        booking.KmReturned = kmReturned;

        if (booking.KmReturned == null || booking.DayReturned == null || booking.Cost == null) return;

        DateTime date1 = DateTime.Now;
        //Konvertera datatyp för att möjliggöra beräkning av mellanskillnad i dagar.
        DateTime date2 = booking.DayRentedOut.ToDateTime(TimeOnly.Parse("00:00:00"));

        // Räkna ut mellanskillnad i dagar
        TimeSpan duration = (TimeSpan)(date1 - date2);
        double DifferenceInDays = duration.TotalDays;
        int RentedDays = (int)Math.Round(DifferenceInDays, 0);
        booking.Cost = RentedDays * vehicle.CostDay + (booking.KmReturned - booking.KmRented) * vehicle.CostKm;
    }
    #endregion

    #region Metoder som returnerar
    // returnerar kunder 
    public IEnumerable<Customer> GetCustomers() => _db.GetPersons().Cast<Customer>();

    // returnerar fordon
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _db.GetVehicles();

    // returnerar bokningar
    public IEnumerable<IBooking> GetBookings() => _db.GetBookings().OrderBy(b => b.RegNo);

    //returnerar enums (vehicle types)
    public string[] GetVehicleTypes() => _db.GetVehicleTypes();
    #endregion

    #endregion METODER REGION ENDS
}
