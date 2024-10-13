using System.ComponentModel.DataAnnotations;

namespace Data.Models.Abstractions;

public class ModelBase : IBaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}
