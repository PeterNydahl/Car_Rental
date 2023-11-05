using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Motorcycle : Vehicle, IVehicle
{
    public Motorcycle(int id, string RegNo, string Brand, int Odometer, double CostKm, string VehicleType, int CostDay, VehicleStatuses Status) : 
        base(id, RegNo, Brand, Odometer, CostKm, VehicleType, CostDay, Status) { }
}
