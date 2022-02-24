﻿using System;
using System.Collections.Generic;
using ECommerce.Abstractions.Messages.Queries.Responses;
using ECommerce.Contracts.Common;

namespace ECommerce.Contracts.ShoppingCart;

public static class Responses
{
    public record ShoppingCart : Response
    {
        public IEnumerable<Models.ShoppingCartItem> Items { get; init; }
        public IEnumerable<Models.IPaymentMethod> PaymentMethods { get; init; }
        public Guid CustomerId { get; init; }
        public decimal Total { get; init; }
        public Guid Id { get; init; }
        public bool IsDeleted { get; init; }
    }

    public record ShoppingCartItem : Response
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
        public string PictureUrl { get; init; }
    }

    public record ShoppingCartItems : ResponsePagedResult<Models.ShoppingCartItem>;
}