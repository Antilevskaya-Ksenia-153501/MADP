using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WEB_153501_Antilevskaya.Domain.Entities;
public class Exhibit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public int Price { get; set; }
    public string? Image { get; set; }
}
