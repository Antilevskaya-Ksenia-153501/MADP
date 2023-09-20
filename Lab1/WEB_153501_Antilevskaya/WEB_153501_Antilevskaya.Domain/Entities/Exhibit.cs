using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153501_Antilevskaya.Domain.Entities;

public class Exhibit
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Category? Category { get; set; }
    public int Price { get; set; }
    public string Image { get; set; }
}
