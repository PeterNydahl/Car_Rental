using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;
public interface IData
{
    public int NextVehicleId { get; }
    public int NextPersonId { get; }
    public int NextBookingId { get; }
    public List<T> Get<T>(Func<T, bool> expression) where T : class;
    public T Single<T>(Func<T, bool> expression) where T : class;
    public void Add<T>(T item) where T : class;
    public string[] GetVehicleTypes() => Enum.GetNames(typeof(VehicleTypes));
    public string[] GetVehicleBrands() => Enum.GetNames(typeof(VehicleBrands));

}
