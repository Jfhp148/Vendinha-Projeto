using Microsoft.AspNetCore.Mvc;
using Vendinha.Api.Dtos;
using Vendinha.Api.Services;

namespace Vendinha.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;
    private readonly DividaService _dividaService;

    public ClientesController(ClienteService clienteService, DividaService dividaService)
    {
        _clienteService = clienteService;
        _dividaService = dividaService;
    }

    [HttpGet]
    public IActionResult Listar([FromQuery] string busca, [FromQuery] int pagina = 1)
    {
        var resultado = _clienteService.Listar(busca, pagina);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var cliente = _clienteService.BuscarPorId(id);
        return Ok(cliente);
    }

    [HttpPost]
    public IActionResult Criar([FromBody] ClienteRequest request)
    {
        var cliente = _clienteService.Criar(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, [FromBody] ClienteRequest request)
    {
        var cliente = _clienteService.Atualizar(id, request);
        return Ok(cliente);
    }

    [HttpDelete("{id}")]
    public IActionResult Excluir(int id)
    {
        _clienteService.Excluir(id);
        return NoContent();
    }

    [HttpGet("{clienteId}/dividas")]
    public IActionResult ListarDividas(int clienteId)
    {
        var dividas = _dividaService.ListarPorCliente(clienteId);
        return Ok(dividas);
    }

    [HttpPost("{clienteId}/dividas")]
    public IActionResult CriarDivida(int clienteId, [FromBody] DividaRequest request)
    {
        var divida = _dividaService.Criar(clienteId, request);
        return Created($"/api/clientes/{clienteId}/dividas", divida);
    }
}
