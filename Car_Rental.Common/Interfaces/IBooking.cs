using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;

public interface IBooking
{
    public string? RegNo { get; init; }
    public string? NameWithSsn { get; init; }
    public int KmRented { get; init; }
    public int? KmReturned { get; set; }
    public DateOnly DayRentedOut { get; init; }
    public DateOnly? DayReturned { get; set; }
    public double? Cost { get; set; }
    public VehicleStatuses Status{ get; set; }


    // metod som räknar ut km och kostnad
    public void ReturnVehicle(int kmReturned);

}


