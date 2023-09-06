namespace WEB_153501_Antilevskaya.Models
{
    public class ListDemo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ListDemo(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
