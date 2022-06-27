using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SimpleCache.Core;
using System.Text;

namespace SimpleCache.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly ILogger<CacheController> _logger;
    private readonly ILRUCache<string, string> _cache;

    public CacheController(ILogger<CacheController> logger, ILRUCache<string, string> cache)
    {
        _logger = logger;
        _cache = cache;
    }

    static readonly string manifest =
    "++ Welcome to simple cache api ++\r\nEndpoints:\r\nHttpGet Get[key]: gets value at key, or 404 \r\nHttpGet List: lists all the data stored in cache \r\nHttpPost Add[key,value]: adds new cache entry \r\nHttpDelete Delete[key]: deletes cache at key, or 404";

    [HttpGet]
    public IActionResult Index()
    {
        return Ok(manifest);
    }

    [HttpGet("get/{key}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string key)
    {
        var value = await Task.Run(() => _cache.Get(key));
        if( value == null ) return NotFound();
        return Ok(value);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List(){
        var data = await Task.Run(
                            () => _cache.GetAll()
                                        .Select(i =>
                                            new { Key = i.key, Value = i.value })
                                        .ToList());
        return Ok(data);
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(string key, string value){
        await Task.Run(() => _cache.AddOrUpdate(key, value));
        return Ok($"Key '{key}' has been added");
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string key) {
        var deleted = await Task.Run(() => _cache.Delete(key));
        if( deleted ) return Ok($"Key '{key}' has been deleted");
        return NotFound("No key to delete");
    }
}
