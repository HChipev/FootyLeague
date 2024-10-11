namespace FootyLeague.Data.Models.Abstractions;

public interface IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
