using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    public CollectionData() => SeedData(); 

    // Metod som lägger till data till listorna
    void SeedData()
    {
        //adding cutomers
        _persons.Add(new Customer() { Ssn = 12345, LastName = "Thelonius", FirstName = "Monk" });
        _persons.Add(new Customer() { Ssn = 98765, LastName = "Powell", FirstName = "Bud" });

        //adding vehicles
        _vehicles.Add(new Motorcycle("MNO234", "Yamaha", 30000, 0.5, (VehicleTypes)3, 50, VehicleStatuses.Available)); 
        _vehicles.Add(new Car("ABC123", "Volvo", 10000, 1, (VehicleTypes)1, 200, VehicleStatuses.Available));
        _vehicles.Add(new Car("DEF456", "Saab", 20000, 1, (VehicleTypes)0, 100, VehicleStatuses.Available));
        _vehicles.Add(new Car("GHI789", "Tesla", 1000, 3, (VehicleTypes)0, 100, VehicleStatuses.Available)); 
        _vehicles.Add(new Car("JKL012", "Jeep", 5000, 1.5, (VehicleTypes)2, 300, VehicleStatuses.Available)); 
    }

    public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _vehicles;
    public IEnumerable<IBooking> GetBookings() => _bookings;

}