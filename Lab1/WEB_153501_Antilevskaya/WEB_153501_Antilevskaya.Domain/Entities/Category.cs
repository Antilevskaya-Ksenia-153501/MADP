﻿namespace WEB_153501_Antilevskaya.Domain.Entities;
public class Category
{
    int Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public List<Exhibit> Exhibits { get; set; }
}
