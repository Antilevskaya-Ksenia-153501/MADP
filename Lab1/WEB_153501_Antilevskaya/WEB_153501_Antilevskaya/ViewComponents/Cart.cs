using System;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Antilevskaya.ViewComponents;
public class Cart:ViewComponent
{
    private readonly WEB_153501_Antilevskaya.Domain.Models.Cart _cart;
    public Cart(WEB_153501_Antilevskaya.Domain.Models.Cart cart)
    {
        _cart = cart;   
    }
    public string Invoke()
    {
        return _cart.TotalPrice.ToString() + "$(" + _cart.Count.ToString() + ")";
    }
}
