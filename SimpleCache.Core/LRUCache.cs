namespace SimpleCache.Core;
using System.Collections.Generic;

/// <summary>
/// Least recent cache class - holds items up to maxVolume has been reached.
/// When full discards the least recent one, and puts the new in. 
/// Thread safe.
/// <returns></returns>
public class LRUCache<T, U>: ILRUCache<T, U>
    where T: notnull
{
    Dictionary<T, LRUItem<T,U>> data;
    LinkedList<T> linkedKeys;
    int maxVolume;
    object protector;

    public LRUCache(int maxVolume){
        this.data = new Dictionary<T, LRUItem<T,U>>(maxVolume);
        this.linkedKeys = new LinkedList<T>();
        this.maxVolume = maxVolume;
        this.protector = new Object();
    }

    /// <summary>
    /// Finds key in cache, and returns stored data (key + value item)
    /// </summary>
    /// <param name="key">Key to find</param>
    /// <returns>Stored key + value entity</returns>
    private LRUItem<T, U>? FindByKey(T key){
        if (this.data.TryGetValue(key, out LRUItem<T, U>? value)){
            return value;
        }

        return null;
    }

    /// <summary>
    /// Sets item as the most recent one (first item in the linked list)
    /// </summary>
    /// <param name="item">item to set</param>
    private void SetAsRecent(LRUItem<T, U> item)
    {
        this.linkedKeys.Remove(item.Node);
        this.linkedKeys.AddFirst(item.Node);
    }

    /// <summary>
    /// Adds key with provided value to storage. Or, if key exists, updates the key value.
    /// In any case, sets item as the most recent.
    /// </summary>
    /// <param name="key">key to store</param>
    /// <param name="value">value to store</param>
    public void AddOrUpdate(T key, U value){
        lock(protector){
            var item = FindByKey(key);
            if (item != null){  // if already exists
                SetAsRecent(item); // swap to top
                item.Value = value; // and update value
            }
            else {
                if( this.data.Count() >= maxVolume ){   // if capacity too high
                    this.Delete(this.linkedKeys.Last!.Value); // delete the least recently used to push new item
                }

                data.Add(key,
                         new LRUItem<T, U>(
                            node: this.linkedKeys.AddFirst(key),
                            value: value)
                );
            }
        }
    }

    /// <summary>
    /// Gets stored value for key.
    /// Or returns default value, if key doesn't exist.
    /// </summary>
    /// <param name="key">Key to look for in cache</param>
    /// <returns>Stored value for key, or default value (default(U))</returns>
    public U? Get(T key){
        lock(protector){
            var item = FindByKey(key);
            if (item != null)
            {
                SetAsRecent(item);
                return item.Value;
            }
        }

        return default(U);
    }

    /// <summary>
    /// Gets all the values held in cache, as a collection of simple tuples (key/value)
    /// </summary>
    /// <returns>Collection of key + value tuples</returns>
    public IEnumerable<(T key, U value)> GetAll(){
        var node = linkedKeys.First;
        while ((node != null))
        {
            var item = FindByKey(node.Value);
            if( item != null) yield return item.ToKeyValue();
            node = node.Next;
        }
    }

    /// <summary>
    /// Deletes value in cache, for provided key
    /// </summary>
    /// <param name="key">Key to delete from cache</param>
    /// <returns>true if deleted, or false if key doesn't exist</returns>
    public bool Delete(T key){
        lock(protector){
            var item = FindByKey(key);
            if (item != null)
            {
                data.Remove(key);
                linkedKeys.Remove(item.Node);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Count of all items held in cache
    /// </summary>
    public long Count => this.data.Count();

    /// <summary>
    /// The most recent used item key + value
    /// </summary>
    public (T key, U value)? Head {
        get{
            var head = FindByKey(linkedKeys.First());
            if( head == null ) return null;
            return (key: head.Node.Value,
                    value: head.Value);
        }
    }

    /// <summary>
    /// The least recent used item key + value
    /// </summary>
    public (T key, U value)? Tail {
        get{
            var last = FindByKey(linkedKeys.Last());
            if( last == null ) return null;
            return (key: last.Node.Value,
                    value: last.Value);
        }
    }
}
