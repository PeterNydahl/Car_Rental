using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Exceptions; 

public class WhitespaceInputException : Exception
{
    public override string Message =>
        "It seemed like you left a part empty! Please Try again";
}
