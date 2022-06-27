#!/bin/bash
target="DEBUG"
ASPNETCORE_ENVIRONMENT=Development

echo **** Build! ****
dotnet build --configuration $target

echo **** Publish! ****
dotnet publish --configuration $target

echo **** Run! ****
cd SimpleCache.WebApi/bin/Debug/net6.0/publish
dotnet SimpleCache.WebApi.dll --urls=http://0.0.0.0:5001 &

xdg-open "http://localhost:5001/cache" &