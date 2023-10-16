using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Collections.Generic;

namespace Car_Rental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _db;

    public BookingProcessor(IData db) => _db = db;

    public IEnumerable<Customer> GetCustomers()
    {
        List<Customer> customerList = new List<Customer>();
       
        foreach (var c in _db.GetPersons())
            customerList.Add((Customer)c);

        IEnumerable <Customer> newCustomerList = customerList;
        return newCustomerList;
    }
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _db.GetVehicles();
   
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
