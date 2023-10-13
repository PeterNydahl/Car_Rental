using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;

//TEST
public interface IData
{
    List<IPerson> GetPersons();
    List<IVehicle> GetVehicles(VehicleStatuses status = default);
    List<IBooking> GetBookings();
}



////RIKITG
//public interface IData
//{
//    IEnumerable<IPerson> GetPersons();
//    IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default); // så att du kan hämta de som är obokade eller bokade
//    IEnumerable<IBooking> GetBookings();

//    // I videon finns här extra metoder som inte är obligatoriska att lägga till.
//}

