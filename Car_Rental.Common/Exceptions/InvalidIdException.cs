using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Exceptions;
public class InvalidIdException : Exception
{
    public string Message => 
        "You have to choose a customer in the drop-down menu before booking a vehicle! Please try again.";
}
