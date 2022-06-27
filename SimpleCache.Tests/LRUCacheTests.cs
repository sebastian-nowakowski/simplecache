namespace SimpleCache.Tests;
using SimpleCache.Core;

public class LRUCacheTests
{
    const int maxVolume = 10;
    ILRUCache<string, string> _cache;
    public LRUCacheTests(){
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

    [Theory]
    [MemberData(nameof(CacheValues))]
    public void PopulateTest(List<(string key, string value)> data)
    {
        data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));
        Assert.Equal(data.Count(), _cache.Count);
        Assert.Equal(data.Last(), _cache.Head);
        Assert.Equal(data.First(), _cache.Tail);
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public void AddNewTest(List<(string key, string value)> data){
        data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));
        _cache.AddOrUpdate("queen", "freedie mercury");
        Assert.Equal(data.Count() + 1, _cache.Count);
        Assert.Equal("queen", _cache.Head?.key);
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public void UpdateExistingTest(List<(string key, string value)> data){
        data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));

        var key = data.First().key;
        var value = "roger moore";
        _cache.AddOrUpdate(key, value);

        Assert.Equal(data.Count(), _cache.Count);
        Assert.Equal(_cache.Get(key), value);
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public void GetExistingTest(List<(string key, string value)> data){
        data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));
        data.ForEach(i => {
            Assert.Equal(_cache.Get(i.key), i.value);
        });
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public void DeleteTest(List<(string key, string value)> data){
        data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));
        var toDelete = data.First().key;
        _cache.Delete(toDelete);

        Assert.Equal(_cache.Count, data.Count() - 1);
        Assert.Null(_cache.Get(toDelete));
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public void CheckMaxVolume(List<(string key, string value)> data){
        data.ForEach(i => _cache.AddOrUpdate(i.key, i.value));

        var tail = _cache.Tail;
        if( data.Count < maxVolume ) {
            var toAdd = maxVolume - data.Count;

            // up to max volume
            for( var i = 1; i <= toAdd; ++i){
                _cache.AddOrUpdate(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
        }
        Assert.Equal(tail, _cache.Tail);
        _cache.AddOrUpdate(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        Assert.NotEqual(tail, _cache.Tail);
    }
}