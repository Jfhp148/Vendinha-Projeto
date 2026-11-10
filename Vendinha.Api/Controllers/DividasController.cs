using Microsoft.AspNetCore.Mvc;
using Vendinha.Api.Services;

namespace Vendinha.Api.Controllers;

[ApiController]
[Route("api/dividas")]
public class DividasController : ControllerBase
{
    private readonly DividaService _dividaService;

    public DividasController(DividaService dividaService)
    {
        _dividaService = dividaService;
    }

    [HttpPut("{id}/pagar")]
    public IActionResult Pagar(int id)
    {
        var divida = _dividaService.Pagar(id);
        return Ok(divida);
    }
}
