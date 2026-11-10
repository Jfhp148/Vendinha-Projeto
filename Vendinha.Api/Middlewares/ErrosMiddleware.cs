using Vendinha.Api.Excecoes;

namespace Vendinha.Api.Middlewares;

public class ErrosMiddleware
{
    private readonly RequestDelegate _next;

    public ErrosMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NaoEncontradoException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { erro = ex.Message });
        }
        catch (RegraNegocioException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { erro = ex.Message });
        }
    }
}
