using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WEB_153501_Antilevskaya.Domain.Entities;
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public List<Exhibit> Exhibits { get; set; }
}
