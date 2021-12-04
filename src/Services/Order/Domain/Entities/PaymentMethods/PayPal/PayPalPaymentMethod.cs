﻿using System;

namespace Domain.Entities.PaymentMethods.PayPal;

public class PayPalPaymentMethod : PaymentMethod
{
    public string UserName { get; init; }
    public string Password { get; init; }
    
    protected override bool Validate() 
        => throw new NotImplementedException();
}