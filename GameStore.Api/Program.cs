using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// configuration for request pipeline
var app = builder.Build();

app.mapGameEndpoints();

app.Run();

