namespace Backend.Web.Core.Backend.Interfaces
{
    public interface IRepository<out TRepository>
    {
        TRepository Repository { get; }
    }
}
