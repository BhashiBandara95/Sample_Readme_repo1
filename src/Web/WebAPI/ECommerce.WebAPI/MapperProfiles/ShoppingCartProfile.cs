﻿using AutoMapper;
using ECommerce.Contracts.Common;
using ECommerce.Contracts.ShoppingCart;
using ECommerce.WebAPI.DataTransferObjects.ShoppingCarts;

namespace ECommerce.WebAPI.MapperProfiles;

public class ShoppingCartProfile : Profile
{
    public ShoppingCartProfile()
    {
        CreateMap<Requests.AddCartItem, Models.ShoppingCartItem>();

        CreateMap<Models.ShoppingCartItem, Outputs.ShoppingCartItem>();

        CreateMap<Responses.ShoppingCart, Outputs.ShoppingCart>();
        CreateMap<Responses.ShoppingCartItem, Outputs.ShoppingCartItem>();
        CreateMap<Responses.ShoppingCartItems, Outputs.ShoppingCartItems>();
    }
}