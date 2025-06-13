using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// here we are registering the GameStoreContext service to the ASP.Net via DP
var ConnectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(ConnectionString);

// configuration for request pipeline
var app = builder.Build();

app.mapGameEndpoints();
app.MigrateDb();

app.Run();

