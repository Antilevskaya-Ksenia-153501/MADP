using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Domain.Models;
public class CartItem
{
    public Exhibit Exhibit { get; set; }
    public int Amount { get; set; }

}
