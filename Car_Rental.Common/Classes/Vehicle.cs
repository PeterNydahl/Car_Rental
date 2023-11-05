﻿using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Vehicle : IVehicle
{
    public int Id { get; init; }
    public string? RegNo { get; init; }
    public string Brand { get; init; }
    public int Odometer { get; set; }
    public double CostKm { get; init; }
    public string VehicleType { get; init; }
    public int CostDay { get; set; }
    public VehicleStatuses Status { get; set; }
}
