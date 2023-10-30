using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;
public interface IData
{
    public void AddBooking(IBooking newBooking);
    public void AddCustomer(Customer newCustomer);
    IEnumerable<IPerson> GetPersons();
    IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default);
    IEnumerable<IBooking> GetBookings();
    public string[] GetVehicleTypes();
}
