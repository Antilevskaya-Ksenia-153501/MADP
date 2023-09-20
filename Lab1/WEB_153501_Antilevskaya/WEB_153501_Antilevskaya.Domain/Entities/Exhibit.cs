namespace WEB_153501_Antilevskaya.Domain.Entities;
public class Exhibit
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Category? Category { get; set; }
    public int Price { get; set; }
    public string Image { get; set; }
}
