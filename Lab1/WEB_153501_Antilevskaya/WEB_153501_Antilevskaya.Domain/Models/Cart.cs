using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Domain.Models;
public class Cart
{
    public Dictionary<int, CartItem> CartItems { get; set; } = new();

    public virtual void AddToCart(Exhibit exhibit)
    {
        if (CartItems.ContainsKey(exhibit.Id))
        {
            CartItems[exhibit.Id].Amount++;
        }
        else
        {
            CartItems[exhibit.Id] = new CartItem
            {
                Exhibit = exhibit,
                Amount = 1
            };
        }
    }

    public virtual void RemoveItems(int id)
    {
        if (CartItems.ContainsKey(id)) 
        { 
            CartItems.Remove(id);
        }
    }

    public virtual void ClearAll()
    {
        CartItems.Clear();
    }

    public int Count { get => CartItems.Sum(item => item.Value.Amount); }

    public int TotalPrice { get => CartItems.Sum(item => item.Value.Exhibit.Price * item.Value.Amount);  }
}
