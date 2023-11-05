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

    //TODO: ta bort
    //string[] _vehicleTypes = Enum.GetNames(typeof(VehicleTypes));

    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(v => v.Id) + 1;
    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(p => p.Id) + 1;
    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(b => b.Id) + 1;

    public CollectionData() => SeedData(); //TODO: Exakt vad gör denna metod? public COllectionData() => SeedData();

    // Metod som lägger till data till listorna
    void SeedData()
    {
        //adding cutomers
        _persons.Add(new Customer() { Id = NextPersonId, Ssn = 171010, LastName = "Monk", FirstName = "Thelonius" });
        _persons.Add(new Customer() { Id = NextPersonId, Ssn = 240927, LastName = "Powell", FirstName = "Bud" });

        //adding vehicles
        _vehicles.Add(new Motorcycle(NextVehicleId, "MNO234", "Yamaha", 30000, 0.5, Enum.GetName(typeof(VehicleTypes), 4) ,50, VehicleStatuses.Available)); 
        _vehicles.Add(new Car(NextVehicleId, "ABC123", "Volvo", 10000, 1, Enum.GetName(typeof(VehicleTypes), 2), 200, VehicleStatuses.Available));
        _vehicles.Add(new Car(NextVehicleId, "DEF456", "Saab", 20000, 1, Enum.GetName(typeof(VehicleTypes), 1), 100, VehicleStatuses.Available));
        _vehicles.Add(new Car(NextVehicleId, "GHI789", "Tesla", 1000, 3, Enum.GetName(typeof(VehicleTypes), 1), 100, VehicleStatuses.Available)); 
        _vehicles.Add(new Car(NextVehicleId, "JKL012", "Jeep", 5000, 1.5, Enum.GetName(typeof(VehicleTypes), 3), 300, VehicleStatuses.Available)); 
    }

    #region ********* METODER *********

    #region Metoder som lägger till data
    public void AddBooking(IBooking newBooking)
    {
        _bookings.Add(newBooking);
    }
    public void AddCustomer(Customer newCustomer)
    {
        _persons.Add(newCustomer);
    }
    public void AddVehicle(IVehicle newVehicle)
    {
        _vehicles.Add(newVehicle);
    }
    #endregion

    #region Metoder som returnerar data
    public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _vehicles;
    public IEnumerable<IBooking> GetBookings() => _bookings;
    //TODO ta bort: public string[] GetVehicleTypes() => _vehicleTypes;   
    #endregion

    #endregion REGION METODER ENDS
}