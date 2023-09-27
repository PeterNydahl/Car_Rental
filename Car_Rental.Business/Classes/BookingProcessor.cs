using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Business.Classes;

public class BookingProcessor //har inget interface eftersom vi inte ska byta ut den mot något annat
{
    private readonly IData _db;

    public BookingProcessor(IData db) => _db = db;

    public List<IPerson> GetPersons() => _db.GetPersons(); //OBS! i beskrivningen står det Customer och inte IPerson
    public List<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        return _db.GetVehicles();
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
