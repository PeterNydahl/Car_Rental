using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Collections.Generic;

namespace Car_Rental.Business.Classes;

public class BookingProcessor //har inget interface eftersom vi inte ska byta ut den mot något annat
{
    private readonly IData _db;

    // Konstruktor
    public BookingProcessor(IData db) => _db = db;

    // Metoder
    public IEnumerable<IPerson> GetPersons() => _db.GetPersons(); //OBS! i beskrivningen står det Customer och inte IPerson
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _db.GetVehicles();
    
    //ToDo: linqmetod istället nedan
    public IEnumerable<IBooking> GetBookings()
    {
        foreach (IBooking b in _db.GetBookings())
        {
            if (b.Status == VehicleStatuses.Booked) 
            {
                b.Status = VehicleStatuses.Open; continue;
            }
            else if (b.Status == VehicleStatuses.Available)
                b.Status = VehicleStatuses.Closed;
                b.ReturnVehicle(6000);
        }
        return _db.GetBookings();
    }
}

//internal class BookingProcessor //har inget interface eftersom vi inte ska byta ut den mot något annat
//{
//    private readonly IData _db;

//    public BookingProcessor(IData db) => _db = db;

//    // det är i nedanstående metoder som du kan lägg till nödvändig logik
//    public IEnumerable<Customer> GetCustomers(); //...
//    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default); // så att du kan hämta de som är obokade eller bokade
//    public IEnumerable<IBooking> GetBookings(); //...
//}
