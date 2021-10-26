﻿using System;

namespace Messages
{
    public static class Models
    {
        public record Address
        {
            public string City { get; init; }
            public string Country { get; init; }
            public int? Number { get; init; }
            public string State { get; init; }
            public string Street { get; init; }
            public string ZipCode { get; init; }
        }
        
        public record CreditCard
        {
            public DateOnly Expiration { get; init; }
            public string HolderName { get; init; }
            public string Number { get; init; }
            public string SecurityNumber { get; init; }
        }
        
        public record Item
        {
            public Guid CatalogItemId { get; init; }
            public string ProductName { get; init; }
            public decimal UnitPrice { get; init; }
            public int Quantity { get; init; }
            public string PictureUrl { get; init; }
        }
    }
}