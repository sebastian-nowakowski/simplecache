using SimpleCache.Core;

var builder = WebApplication.CreateBuilder(args);

// set singleton lrucache object for depedency injection
builder.Services
       .AddSingleton<ILRUCache<string, string>>(
            cache => new LRUCache<string, string>(
                    // try parsing and passing config value, otherwise use the defaults
                    int.TryParse(builder.Configuration["LRUCapacity"], out int capacity) ? capacity : 10
            )
       );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
