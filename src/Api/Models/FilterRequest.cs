/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-06 17:14:13
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-06 17:27:37
 */


namespace Api.Models;


public sealed class FilterRequest
{
    public string? Estado { get; init; }
    public DateTime? Desde { get; init; }
    public DateTime? Hasta { get; init; }
}

