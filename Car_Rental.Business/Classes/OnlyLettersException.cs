﻿namespace Car_Rental.Business.Classes;
public class OnlyLettersException : Exception
{
    public override string Message =>
        "Name input must contain only letters! Please Try again.";
}
