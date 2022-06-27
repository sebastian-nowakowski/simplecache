namespace SimpleCache.Core;

public interface ILRUCache<T,U>
    where T: notnull
{
    void AddOrUpdate(T key, U value);

    bool Delete(T key);

    U? Get(T key);

    IEnumerable<(T key, U value)> GetAll();

    (T key, U value)? Head { get; }

    (T key, U value)? Tail { get; }

    long Count { get; }
}
