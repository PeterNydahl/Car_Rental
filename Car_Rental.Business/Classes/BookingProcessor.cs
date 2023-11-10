using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Exceptions;
using Car_Rental.Common.Extensions;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using Car_Rental.Data.Classes;

using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Car_Rental.Business.Classes;

public class BookingProcessor
{
    #region ************* FÄLT & EGENSKAPER *************

    readonly IData _db;
    public bool IsBookingProcessing { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
    public bool ShowErrorMessage { get; set; }

    //public VehicleTypes[] VehicleTypes { get; init; }

    #endregion

    #region ************* KONSTRUKTOR *************
    public BookingProcessor(IData db)
    {
        _db = db;
        
        //TODO: Ta bort hårdkådade bokningar?
        //NewBooking(1, 1); // tesla, Bud
        //NewBooking(5, 2); // jeep, Monk
        //ReturnVehicle(5, 2, 6000);
    }
    #endregion

    #region ************* METODER *************

    #region Metoder som lägger till kund, fordon
    // Metod som lägger till ny kund
    public void AddCustomer(int ssn, string lastName, string firstName)
    {
        ShowErrorMessage = false;
        try
        {
            if (ssn.ToString().Length != 6)
                throw new InvalidSsnException();
            else if (lastName == string.Empty || firstName == string.Empty)
                throw new WhitespaceInputException();
            else if (!DoesInputContainOnlyLetters(lastName) || !DoesInputContainOnlyLetters(firstName))
                throw new OnlyLettersException();
 
                _db.Add<IPerson>(new Customer() { Id = _db.NextPersonId, Ssn = ssn, LastName = lastName, FirstName = firstName });
        }
        catch (Exception fel)
        {
            ErrorMessage = fel.Message;
            ShowErrorMessage = true;
        }
    }

    //Metod som lägger till nytt fordon
    public void AddVehicle(string regNo, string brand, int odometer, double costKm, string vehicleType, int costDay)
    {
        ShowErrorMessage = false;

        try
        {
            // felhantering av input
            if (!IsRegNoInputCorrect(regNo))
                throw new RegNumberInputException();

            else if (regNo == string.Empty)
                throw new WhitespaceInputException();

            else if (!GetVehicleBrands().Any(b => b == brand))
                throw new DropdownMenuException();

            else if (odometer < 0 || costKm < 0 || costDay < 0)
                throw new NegativeNumberInputException();

            else if (!GetVehicleTypes().Any(b => b == vehicleType))
                throw new DropdownMenuException();

            else if (odometer > int.MaxValue || costKm > double.MaxValue || costDay > int.MaxValue)
                throw new InputValueTooBigException();

            else if (_db.Get<IVehicle>(v => true).Any(v => v.RegNo == regNo))
                throw new RegNoAlreadyExistsException();

            // skapar nytt fordonsobjekt
            if (vehicleType == Enum.GetName(typeof(VehicleTypes), 1)) //Om det vallda fordonet är en motorcykel
            {
                _db.AddVehicle(new Motorcycle(_db.NextVehicleId, regNo.ToUpper(), brand, odometer, costKm, vehicleType, costDay, VehicleStatuses.Available));
            }
            _db.AddVehicle(new Car(_db.NextVehicleId, regNo.ToUpper(), brand, odometer, costKm, vehicleType, costDay, VehicleStatuses.Available));
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            ShowErrorMessage = true;
        }
    }
    #endregion

    #region Metoder som sköter bokningar

    //Asynkrona metoder som hämtar kund och fordon till bokning
    public async Task<Customer> GetCustomerAsync(int customerId) =>
    await Task.Run(() => (Customer)_db.Single<IPerson>(c => c.Id == customerId));

    public async Task<IVehicle> GetVehicleAsync(int vehicleId) =>
        await Task.Run(() => _db.Single<IVehicle>(v => v.Id == vehicleId));

    //Asynkron metod som skapar ny bokning
    public async Task NewBookingAsync(int vehicleId, int customerId)
    {
        ShowErrorMessage = false;
        
        try
        {
            if (customerId <= 0)
            {
                throw new DropdownMenuException();
            }
            IsBookingProcessing = true;

            await Task.Delay(1000);
            Customer customer = await GetCustomerAsync(customerId);
            // skapar en bokning
            _db.AddBooking(new Booking(_db.NextBookingId, _db.GetVehicles().First(v => v.Id == vehicleId), customer, new(2023, 11, 01), VehicleStatuses.Open));

            // hämtar fordon
            IVehicle updateVehicle = await GetVehicleAsync(vehicleId);
            // Ändrar status för fordonet i Vehicle-lista
            updateVehicle.Status = VehicleStatuses.Booked;

            IsBookingProcessing = false;
        }
        catch (DropdownMenuException iv)
        {
            ErrorMessage = iv.Message;
            ShowErrorMessage = true;
            return;
        }
    }
    public void ReturnVehicle(int vehicleId, int bookingId, int distance)
    {
        // leta upp fordonets som bokningen gäller, ändra status och odometer
        IVehicle? vehicle = _db.Single<IVehicle>(v => v.Id == vehicleId); 
        vehicle.Status = VehicleStatuses.Available;
        vehicle.Odometer += distance;

        //leta upp bokningen som ska avslutas och ändra status
        IBooking? booking = _db.Single<IBooking>(b => b.Id == bookingId);
        booking.Status = VehicleStatuses.Closed;

        // Gör uträkningar
        booking.Cost = 0;
        booking.DayReturned = DateOnly.FromDateTime(DateTime.Now);
        booking.Distance = distance;

        DateTime date2 = DateTime.Now;
        //Konvertera datatyp för att möjliggöra beräkning av mellanskillnad i dagar.
        DateTime date1 = booking.DayRentedOut.ToDateTime(TimeOnly.Parse("00:00:00"));

        // Extensionmetod ("Duration") som räknar ut mellanskillnad i dagar
        double DifferenceInDays = date1.Duration(date2).TotalDays;

        int RentedDays = (int)Math.Round(DifferenceInDays, 0);
        booking.Cost = RentedDays * vehicle.CostDay + (booking.Distance * vehicle.CostKm);
    }
    #endregion

    #region Metoder som hämtar och returnerar data

    // returnerar kunder
    public IEnumerable<Customer> GetCustomers() => _db.Get<IPerson>(x => true).Cast<Customer>();

    // returnerar fordon 
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _db.Get<IVehicle>(x => true);

    // returnerar bokningar
    public IEnumerable<IBooking> GetBookings() => _db.Get<IBooking>(x => true);


    // returnerar bokning
    public IBooking GetBooking(string regNo) => _db.Single<IBooking>(b => b.RegNo.Equals(regNo) && b.Status == VehicleStatuses.Open);


    //returnerar enums (vehicle types)
    public string[] GetVehicleTypes() => Enum.GetNames(typeof(VehicleTypes));
    public string[] GetVehicleBrands() => Enum.GetNames(typeof(VehicleBrands));

    #endregion

    #region Metoder för felhantering
    public bool DoesInputContainOnlyLetters(string userInput)
    {
        // uttryck (reguljärt) som matchar uteslutande bokstäver (stora eller små).
        Regex onlyLetters = new Regex("^[a-zA-Z]+$");

        // kontrollerar om strängen från argumentet endast innehåller bokstäver. Returnerar true eller false
        return onlyLetters.IsMatch(userInput);
    }
    static bool IsRegNoInputCorrect(string userInput)
    {
        // Skapar ett reguljärt uttryck för 3 bokstäver följt av 3 siffror
        string pattern = "^[A-Za-z]{3}\\d{3}$";

        // Skapar ett Regex-objekt som tar in mönstret. 
        Regex regex = new Regex(pattern);

        //Jämför och returnera. 
        return regex.IsMatch(userInput);
    }
    #endregion

    #endregion METODER REGION ENDS
}
