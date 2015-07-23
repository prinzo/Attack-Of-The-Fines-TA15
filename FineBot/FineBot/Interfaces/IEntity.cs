namespace FineBot.Interfaces
{
    public interface IEntity<TIdentifier>
    {
         TIdentifier Id { get; set; }
    }
}