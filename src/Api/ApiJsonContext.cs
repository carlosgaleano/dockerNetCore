/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-05 13:31:27
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-05 16:36:55
 */
using System.Text.Json.Serialization;

namespace Demo;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(MiRespuesta))]
[JsonSerializable(typeof(Dictionary<string, object?>))]
public partial class ApiJsonContext : JsonSerializerContext { }

public record MiRespuesta(string Id, string Estado, DateTimeOffset Fecha);