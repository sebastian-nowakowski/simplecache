namespace SimpleCache.Tests;
using SimpleCache.Core;
using SimpleCache.WebApi.Controllers;

public class CacheWebApiTests
{
    const int maxVolume = 10;
    ILRUCache<string, string> _cache;
    public CacheWebApiTests()
    {
        _cache = new LRUCache<string, string>(maxVolume);
    }

    
}