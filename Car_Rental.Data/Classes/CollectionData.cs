using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(v => v.Id) + 1;
    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(p => p.Id) + 1;
    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(b => b.Id) + 1;

    public CollectionData() => SeedData();

    // Lägger till data till listorna
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

    //Returnerar lista

    public T Single<T>(Func<T, bool> expression) where T : class
    {
        var propList = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance) 
            .FirstOrDefault(x => x.FieldType == typeof(List<T>) && x.IsInitOnly) ?? throw new InvalidOperationException("Invalid datatype"); 
       
        var propListContent = propList.GetValue(this) ?? throw new Exception("List content was null!");
        var lista = ((List<T>)propListContent).AsQueryable();

        if (expression == null)
            throw new Exception("No item was found!");
        else
            return lista.Single(expression);
    }
    public List<T> Get<T>(Func<T, bool> expression) where T : class
    {
        // Sök reda på "blueprinten" av en egenskap(här en lista) av datatypen T 
        var propList = GetType() // Kollar "Blueprint", vad objektet har för egenskaper och metoder
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance) //GetFields sorterar ut egenskaperna. BindingFlags = flaggar villkor för det som ska hämtas ut
            .FirstOrDefault(x => x.FieldType == typeof(List<T>) && x.IsInitOnly) //filtrera ut den lista som är av datatypen “T” samt är initierad 
            ?? throw new InvalidOperationException("Invalid datatype"); // Om ingen lista finns av angiven datatyp - kasta ett felmeddelande
        // Hämta listans data
        var propListContent = propList.GetValue(this) //sparar värdet i listan i ny variabel. I parametern anges objektet som datat hämtas ifrån, i detta fall det objekt vi befinner oss i, därav "this"
            ?? throw new Exception("List content was null!");
        var returLista = ((List<T>)propListContent).AsQueryable(); // AsQuerable möjliggör användning linq (datat hämtas allterftersom det filtreras) 

        if (expression == null) return returLista.ToList(); // returnera hela listan om sökresultatet är null
        return returLista.Where(expression).ToList(); // returnera listan filtrerad lambdauttrycket i metdens parameter
    }

    // Lägger till i lista
    public void Add<T>(T item) where T : class
    {
        //TODO: ta bort kommentarer
        var propList = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(x => x.FieldType == typeof(List<T>) && x.IsInitOnly) ?? throw new InvalidOperationException("Invalid datatype");
        var propListContent = propList.GetValue(this) ?? throw new Exception("List content was null!");
        var qList = ((List<T>)propListContent).AsQueryable();
        var list = qList.ToList();
        
        list.Add(item); //uppdaterar temporär lista
        propList.SetValue(this, list); //uppdaterar den "riktiga" listan
    }
    #endregion

        #endregion REGION METODER ENDS

        // TODO: ta bort kod 
        public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _vehicles;
    public IEnumerable<IBooking> GetBookings() => _bookings;
}