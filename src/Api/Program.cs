/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-04 18:09:10
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-06 18:08:02
 */
using Demo;
using Api.Models;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Data.SqlClient;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
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


// POST: body = { "limit": 100, "page": 2 }
app.MapPost("/postgres/test", async (IConfiguration config, PagingRequest req) =>
{
    var limit  = (req.Limit is > 0 and <= 1000) ? req.Limit : 50;
    var page   = (req.Page  > 0) ? req.Page : 1;
    var offset = (page - 1) * limit;

    await using var conn = new NpgsqlConnection(config.GetConnectionString("Postgres"));
    await conn.OpenAsync();

    var total = await conn.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM tracking_info_3");
    var data  = await conn.QueryAsync(
        @"SELECT * FROM tracking_info_3 ORDER BY id_despacho LIMIT @Limit OFFSET @Offset",
        new { Limit = limit, Offset = offset });

    return Results.Ok(new {
        page, limit, total,
        totalPages = (int)Math.Ceiling(total / (double)limit),
        data
    });
});

app.MapGet("/sqlserver/test", async (IConfiguration config) =>
{
    try
    {
        using var conn = new SqlConnection(config.GetConnectionString("SqlServer"));
        await conn.OpenAsync();

        var one = await conn.ExecuteScalarAsync<int>("SELECT 1");
        var todos = await conn.QueryAsync("SELECT * FROM vw_get_data_tracking_new  ORDER BY 1 DESC");

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

// Probar conexión a PostgreSQL (SELECT 1 y listar todos)
app.MapGet("/postgres/test", async () =>
{
    try
    {
        await using var conn = new NpgsqlConnection(config.GetConnectionString("Postgres"));
        await conn.OpenAsync();

        var one = await conn.ExecuteScalarAsync<int>("SELECT 1");
        //var todos = await conn.QueryAsync("SELECT  * from tracking_info_3 ti  limit 10000");
        
        var compiler = new PostgresCompiler();

        var db = new QueryFactory(conn, compiler);

        var todos = await db.Query("tracking_info_3")
                            .Select("id_despacho","ode","numero_guia","cliente","direccion","fecha_creacion","fecha_inicio","transporte","observacion","comentario","fecha_estado","estado","estado_penultimo","fecha_penultimo_estado","estado_penultimo_1","fecha_ante_penultimo_estado","quarto_estado","fecha_quarto_estado","quinto_estado","fecha_quinto_estado","sexto_estado","fecha_sexto_estado","septimo_estado","fecha_septimo_estado","octavo_estado","fecha_octavo_estado","peso")
                            .Limit(50)
                            .GetAsync();

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