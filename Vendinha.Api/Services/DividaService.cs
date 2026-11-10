using Vendinha.Api.Data;
using Vendinha.Api.Dtos;
using Vendinha.Api.Excecoes;
using Vendinha.Api.Models;

namespace Vendinha.Api.Services;

public class DividaService
{
    private readonly VendinhaContext _context;

    public DividaService(VendinhaContext context)
    {
        _context = context;
    }

    public DividaResponse Criar(int clienteId, DividaRequest request)
    {
        if (!_context.Clientes.Any(c => c.Id == clienteId))
        {
            throw new NaoEncontradoException("Cliente não encontrado.");
        }

        if (request == null || request.Valor <= 0)
        {
            throw new RegraNegocioException("O valor da dívida deve ser maior que zero.");
        }

        if (_context.Dividas.Any(d => d.ClienteId == clienteId && !d.Paga))
        {
            throw new RegraNegocioException("Esse cliente já possui uma dívida em aberto.");
        }

        var divida = new Divida
        {
            ClienteId = clienteId,
            Valor = Math.Round(request.Valor, 2),
            Paga = false,
            DataCriacao = DateTime.Now
        };

        _context.Dividas.Add(divida);
        _context.SaveChanges();

        return ParaResponse(divida);
    }

    public List<DividaResponse> ListarPorCliente(int clienteId)
    {
        if (!_context.Clientes.Any(c => c.Id == clienteId))
        {
            throw new NaoEncontradoException("Cliente não encontrado.");
        }

        return _context.Dividas
            .Where(d => d.ClienteId == clienteId)
            .OrderByDescending(d => d.DataCriacao)
            .ToList()
            .Select(ParaResponse)
            .ToList();
    }

    public DividaResponse Pagar(int id)
    {
        var divida = _context.Dividas.FirstOrDefault(d => d.Id == id);

        if (divida == null)
        {
            throw new NaoEncontradoException("Dívida não encontrada.");
        }

        if (divida.Paga)
        {
            throw new RegraNegocioException("Essa dívida já foi paga.");
        }

        divida.Paga = true;
        divida.DataPagamento = DateTime.Now;

        _context.SaveChanges();

        return ParaResponse(divida);
    }

    private static DividaResponse ParaResponse(Divida divida)
    {
        return new DividaResponse
        {
            Id = divida.Id,
            ClienteId = divida.ClienteId,
            Valor = divida.Valor,
            Paga = divida.Paga,
            DataCriacao = divida.DataCriacao,
            DataPagamento = divida.DataPagamento
        };
    }
}
