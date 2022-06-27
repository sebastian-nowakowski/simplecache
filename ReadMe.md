# Simple cache

## Solution break down
* SimpleCache.Core: contains all the project business logic
* SimpleCache.WebApi: contains the WebApi project
* SimpleCache.Tests: contains all the tests on both .Core and .WebApi

## Compiling & Running SimpleCache.WebApi
Solution has been created & compiled under linux.
To run WebApi, simply open the terminal in the solution root folder, 
and run *./run.sh* (script compiles the solution, publishes files, runs Kestrel server, and opens browser window with webApi url).

## SimpleCache.WebApi Url & Endpoints
* Default url address for SimpleCache.WebApi is *http://localhost:5001/cache*.
* WebApi handles following endpoints:
    - *http://localhost:5001/cache/get/{key}*: 
        + [HttpGet]
        + Return status codes: 404, 200
        + Gets the item associated with the key. If the item isn't present: 404.
    - *http://localhost:5001/cache/list*: 
        + [HttpGet]
        + Return status codes: 200
        + Gets all the items stored in the cache
    - *http://localhost:5001/cache/add/{key}/{value}*: 
        + [HttpPost]
        + Return status codes: 200
        + Creates new item in the cache, or updates the existing one. 
    - *http://localhost:5001/cache/delete/{key}*: 
        + [HttpDelete]
        + Return status codes: 404, 200
        + Deletes the item associated with the key from the cache. If the item isn't present: 404.

## Project references
* https://docs.microsoft.com/pl-pl/dotnet/ - dotnet docs
* https://www.c-sharpcorner.com/article/fast-and-clean-o1-lru-cache-implementation/ - lru cache implementation base