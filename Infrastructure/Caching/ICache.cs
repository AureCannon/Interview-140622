namespace Infrastructure.Caching
{
    public interface ICache
    {
        Task<T> GetOrCreateAsync<T>(object key, Func<Task<T>> getFromDb);
    }
}
