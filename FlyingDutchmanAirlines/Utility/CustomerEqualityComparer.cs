﻿using System.Security.Cryptography;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.Utility;

internal class CustomerEqualityComparer : EqualityComparer<Customer> 
{
    public override int GetHashCode(Customer obj)
    {
        int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue / 2);
        return (obj.CustomerId + obj.Name.Length + randomNumber).GetHashCode();
    }

    public override bool Equals(Customer x, Customer y)
    {
        return x.CustomerId == y.CustomerId
               && x.Name == y.Name;
    }
}

