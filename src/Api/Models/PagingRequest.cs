/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-06 17:13:41
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-06 17:13:56
 */

namespace Api.Models; // ajusta al namespace de tu proyecto

public sealed record PagingRequest(int Limit = 50, int Page = 1);
