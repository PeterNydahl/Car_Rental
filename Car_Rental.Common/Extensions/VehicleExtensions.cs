using Car_Rental.Common.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Car_Rental.Common.Extensions;
public static class VehicleExtensions
{
    public static TimeSpan Duration(this DateTime StartDate, DateTime EndDate)
        => (TimeSpan)(EndDate - StartDate);
}