using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Motorcycle : IVehicle
{
    public string? RegNo { get; init; }
    public string? Brand { get; init; }
    public int Odometer { get; set; }

    public double CostKm { get; init; }

    public VehicleTypes VehicleType { get; init; }

    public int CostDay { get; set; }

    public VehicleStatuses Status { get; set; }

    public Motorcycle(string RegNo, string Brand, int Odometer, double CostKm, VehicleTypes VehicleType, int CostDay, VehicleStatuses Status)
    {
        this.RegNo = RegNo;
        this.Brand = Brand;
        this.Odometer = Odometer;
        this.CostKm = CostKm;
        this.VehicleType = VehicleType;
        this.CostDay = CostDay;
        this.Status = Status;
    }
}
