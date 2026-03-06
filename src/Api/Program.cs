/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-04 18:09:10
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-06 09:37:34
 */
using Demo;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Data.SqlClient;
using Npgsql;
using Dapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Clear();
    options.SerializerOptions.TypeInfoResolverChain.Add(ApiJsonContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(new DefaultJsonTypeInfoResolver());
});

var app = builder.Build();
var config = builder.Configuration;


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => Results.Ok(new {
    service = "API .NET + SQL Server + PostgreSQL",
    version = "v1",
    timestamp = DateTimeOffset.UtcNow
}));

// Probar conexión a PostgreSQL (SELECT 1 y listar todos)
app.MapGet("/postgres/test", async () =>
{
    try
    {
        await using var conn = new NpgsqlConnection(config.GetConnectionString("Postgres"));
        await conn.OpenAsync();

        var one = await conn.ExecuteScalarAsync<int>("SELECT 1");
        var todos = await conn.QueryAsync("SELECT  * from tracking_info_3 ti  limit 10000");

        return Results.Ok(new {
            status = "ok",
            test = one,
            sample = todos
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/dto", () =>
    Results.Ok(new MiRespuesta("123", "ok", DateTimeOffset.UtcNow))
);

app.MapGet("/anon", () =>
    // Esto funciona gracias al fallback (DefaultJsonTypeInfoResolver)
    Results.Ok(new { id = "123", estado = "ok", fecha = DateTimeOffset.UtcNow })
);

app.Run();