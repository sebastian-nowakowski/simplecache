namespace SimpleCache.Tests;
using SimpleCache.Core;
using SimpleCache.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;

public class CacheWebApiTests: BaseCacheTest
{
    CacheController _cacheController;

    public CacheWebApiTests(): base()
    {
        var logger = new Mock<ILogger<CacheController>>();
        _cacheController = new CacheController(logger.Object, _cache);
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public async Task GetTest(List<(string key, string value)> data){
        FillCache(data);
        var response = await _cacheController.Get(data.First().key) as OkObjectResult;
        Assert.NotNull(response);
        if( response != null ){
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public async Task GetListTest(List<(string key, string value)> data){
        FillCache(data);
        var response = await _cacheController.List() as OkObjectResult;
        Assert.NotNull(response);
        if( response != null ){
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            var list = response.Value as Dictionary<string, string>;
            Assert.NotNull(list);
            if( list != null ){
                Assert.Equal(data.Count(), list.Count());
            }
        }
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public async Task DeleteTest(List<(string key, string value)> data){
        FillCache(data);

        // check non existent
        var nonExistent = await _cacheController.Delete(Guid.NewGuid().ToString()) as NotFoundObjectResult;
        Assert.NotNull(nonExistent);
        if( nonExistent != null ){
            Assert.Equal(StatusCodes.Status404NotFound, nonExistent.StatusCode);
        }

        var response = await _cacheController.Delete(data.First().key) as OkObjectResult;
        if( response != null ){
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }
    }

    [Theory]
    [MemberData(nameof(CacheValues))]
    public async Task AddTest(List<(string key, string value)> data){
        FillCache(data);

        var newItem = new { key = Guid.NewGuid().ToString(), value = Guid.NewGuid().ToString() };
        var addResult = await _cacheController.Add(newItem.key, newItem.value) as OkObjectResult;
        Assert.NotNull(addResult);
        if( addResult != null ){
            Assert.Equal(StatusCodes.Status200OK, addResult.StatusCode);
            Assert.Equal(_cache.Get(newItem.key), newItem.value);
        }
    }
}