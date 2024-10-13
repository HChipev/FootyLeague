namespace Data.Models.Abstractions;

public interface IBaseEntity
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}
