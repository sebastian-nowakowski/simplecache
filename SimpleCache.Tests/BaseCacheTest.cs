namespace SimpleCache.Tests;
using SimpleCache.Core;

public class BaseCacheTest
{
    protected const int maxVolume = 10;
    protected ILRUCache<string, string> _cache;
    public BaseCacheTest(){
        _cache = new LRUCache<string, string>(maxVolume);
    }

    public static IEnumerable<object[]> CacheValues() {
        yield return new object[] {
            new List<(string key, string value)>(){
                (key: "actor", value: "daniel craig"),
                (key: "singer", value: "bruce dickinson"),
                (key: "comedian", value: "john cleese"),
                (key: "actress", value: "emily blunt"),
                (key: "monster", value: "nessie"),
                (key: "fellowship", value: "frodo, sam, merry, pippin!")
            }
        };
    }

    protected void FillCache(List<(string key, string value)> data){
        if( data != null ){
            data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));
        }
    }
}
