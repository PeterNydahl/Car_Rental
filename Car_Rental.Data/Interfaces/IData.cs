using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;
public interface IData
{
    public int NextVehicleId { get; }
    public int NextPersonId { get; }
    public int NextBookingId { get; }

    
    public void AddBooking(IBooking newBooking);
    public void AddCustomer(Customer newCustomer);
    public void AddVehicle(IVehicle vehicle);
    IEnumerable<IPerson> GetPersons();
    IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default);
    IEnumerable<IBooking> GetBookings();

    public List<T> Get<T>(Func<T, bool> expression) where T : class;
    }
