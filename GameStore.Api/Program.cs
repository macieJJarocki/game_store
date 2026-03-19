using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGameEndpoints();

app.MigrateDb();
int[] x = [1, 23, 3];
Console.WriteLine(x);
app.Run();
// https://youtu.be/YbRe4iIVYJk?t=9684
