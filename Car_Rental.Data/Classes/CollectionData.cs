﻿using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Car_Rental.Data.Classes;

// TEST
public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();

    // TODO mata in i konstruktorn via SeedData istället
    public CollectionData() 
    {
        //adding cutomers
        _persons.Add(new Customer() { Ssn = 12345, LastName = "Thelonius", FirstName = "Monk" });
        _persons.Add(new Customer() { Ssn = 54321, LastName = "Powell", FirstName = "Bud" });

        //adding vehicles
        _vehicles.Add(new Car("ABC123", "Volvo", 10000, 1, (VehicleTypes)1, 200, (VehicleStatuses)2)); // 2 = Available
        _vehicles.Add(new Car("DEF456", "Saab", 20000, 1, (VehicleTypes)0, 100, (VehicleStatuses)2));
        _vehicles.Add(new Car("GHI789", "Tesla", 1000, 3, (VehicleTypes)0, 100, (VehicleStatuses)1)); // 2 = Available
        _vehicles.Add(new Car("JKL012", "Jeep", 5000, 1.5, (VehicleTypes)2, 300, (VehicleStatuses)2)); // 2 = Available
        _vehicles.Add(new Car("MNO234", "Yamaha", 30000, 0.5, (VehicleTypes)3, 50, (VehicleStatuses)2)); // 2 = Available
    }
    public List<IPerson> GetPersons() => _persons;
    public List<IVehicle> GetVehicles(VehicleStatuses status = default) // TODO: lägg till funk så att du kan filtrera vilka bilar du vill komma åt
    {
        return _vehicles;
    }
}


// RIKTIG
//public class CollectionData :IData
//{
//    //Dessa är privata, man kommer inte åt dem utifrån utan vi är tvugna att gå via metoderna. 
//    readonly List<IPerson> _persons = new List<IPerson>();
//    readonly List<IVehicle> _vehicles = new List<IVehicle>();
//    readonly List<IBooking> _bookings = new List<IBooking>();

//    public CollectionData() => SeedData(); //Lägger till data till listorna

//    void SeedData() //...

//    IEnumerable<IPerson> GetPersons();
//    IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default); // så att du kan hämta de som är obokade eller bokade
//    IEnumerable<IBooking> GetBookings();
//}
