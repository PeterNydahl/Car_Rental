using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Collections.Generic;

namespace Car_Rental.Business.Classes;

public class BookingProcessor
{
    //******************** fält/egenskaper ********************
    readonly IData _db;

    List<IVehicle> _vehicles = new();
    List<IBooking> _bookedVehicles = new();
    List<IPerson> _customers = new();

    // ******************** konstruktor ********************
    public BookingProcessor(IData db)
    {
        _db = db;
        _vehicles.AddRange(_db.GetVehicles());
        _customers.AddRange(_db.GetPersons());

        // skapa nya bokningar genom att anropa NewBooking
        NewBooking("GHI789", 240927); // tesla, Bud
        NewBooking("JKL012", 171010); // jeep, Monk
        ReturnVehicle("JKL012", 6000);
    }

    // ******************** metoder ********************

    // tar in lista med <IPerson> och returnerar lista med <Customer>
    public IEnumerable<Customer> GetCustomers()
    {
        List<Customer> customerList = new List<Customer>();

        foreach (var c in _db.GetPersons())
            customerList.Add((Customer)c);

        IEnumerable<Customer> newCustomerList = customerList;
        return newCustomerList;
    }

    // returnerar samtliga fordon
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _vehicles;

    
    
    // skapar en ny bokning
    public void NewBooking(string regNr, int ssn)
    {
        Customer customer = (Customer)_customers.FirstOrDefault(c => c.Ssn == ssn);
        _bookedVehicles.Add(new Booking(_vehicles.First(v => v.RegNo == regNr), customer, new(2023, 10, 10), VehicleStatuses.Open));

        // i _vehicles ändra status till Booked för fordonet
        IVehicle? updateVehicle = _vehicles.Find(v => v.RegNo == regNr);
        if (updateVehicle is not null)
            updateVehicle.Status = VehicleStatuses.Booked;
        else throw new Exception();
    }

    // (lämna tillbaka fordon) - gör uträkning och ändrar status
    public void ReturnVehicle(string regNr, int kmReturned)
    {
        // leta upp fordonets som bokningen gäller
        IVehicle? vehicle = _vehicles.Find(v => v.RegNo == regNr);
        // ändra status och odometer 
        vehicle.Status = VehicleStatuses.Available;
        vehicle.Odometer = kmReturned;

        //leta upp bokningen som ska avslutas
        IBooking? booking = _bookedVehicles.Find(bv => bv.RegNo == regNr);
        //ändra status till Closed
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

    public IEnumerable<IBooking> GetBookings() => _bookedVehicles;
        
}
