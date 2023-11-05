using Car_Rental.Common.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Car_Rental.Common.Extensions;
public static class VehicleExtensions
{
    ///Duration(this DateTime StartDate, DateTime EndDate)
    public static TimeSpan Duration(this DateTime StartDate, DateTime EndDate)
    {
        return (TimeSpan)(EndDate - StartDate);
    }
}

//DateTime date1 = DateTime.Now;
////Konvertera datatyp för att möjliggöra beräkning av mellanskillnad i dagar.
//DateTime date2 = booking.DayRentedOut.ToDateTime(TimeOnly.Parse("00:00:00"));

//// Räkna ut mellanskillnad i dagar
//TimeSpan duration = (TimeSpan)(date1 - date2);