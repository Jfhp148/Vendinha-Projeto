using Microsoft.EntityFrameworkCore;
using Vendinha.Api.Data;
using Vendinha.Api.Dtos;
using Vendinha.Api.Excecoes;
using Vendinha.Api.Models;
using Vendinha.Api.Utils;

namespace Vendinha.Api.Services;

public class ClienteService
{
    private const int TamanhoPagina = 10;

    private readonly VendinhaContext _context;

    public ClienteService(VendinhaContext context)
    {
        _context = context;
    }

    public ClienteResponse Criar(ClienteRequest request)
    {
        ValidarDados(request);

        string cpf = CpfValidador.SomenteNumeros(request.Cpf);

        if (_context.Clientes.Any(c => c.Cpf == cpf))
        {
            throw new RegraNegocioException("Já existe um cliente cadastrado com esse CPF.");
        }

        var cliente = new Cliente
        {
            Nome = request.Nome.Trim(),
            Cpf = cpf,
            DataNascimento = request.DataNascimento.Value.Date,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim()
        };

        _context.Clientes.Add(cliente);
        _context.SaveChanges();

        return ParaResponse(cliente);
    }

    public ClienteResponse BuscarPorId(int id)
    {
        var cliente = BuscarCliente(id);
        return ParaResponse(cliente);
    }

    public PaginaClientesResponse Listar(string busca, int pagina)
    {
        if (pagina < 1)
        {
            pagina = 1;
        }

        var clientes = _context.Clientes
            .Include(c => c.Dividas)
            .ToList();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            clientes = clientes
                .Where(c => c.Nome.Contains(busca.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        clientes = clientes
            .OrderByDescending(c => c.CalcularTotalDividas())
            .ThenBy(c => c.Nome)
            .ToList();

        int totalClientes = clientes.Count;
        int totalPaginas = (int)Math.Ceiling(totalClientes / (double)TamanhoPagina);
        decimal somaTotalDividas = clientes.Sum(c => c.CalcularTotalDividas());

        var clientesDaPagina = clientes
            .Skip((pagina - 1) * TamanhoPagina)
            .Take(TamanhoPagina)
            .ToList();

        return new PaginaClientesResponse
        {
            Pagina = pagina,
            TamanhoPagina = TamanhoPagina,
            TotalClientes = totalClientes,
            TotalPaginas = totalPaginas,
            SomaTotalDividas = somaTotalDividas,
            Clientes = clientesDaPagina.Select(ParaResponse).ToList()
        };
    }

    public ClienteResponse Atualizar(int id, ClienteRequest request)
    {
        var cliente = BuscarCliente(id);

        ValidarDados(request);

        string cpf = CpfValidador.SomenteNumeros(request.Cpf);

        if (_context.Clientes.Any(c => c.Cpf == cpf && c.Id != id))
        {
            throw new RegraNegocioException("Já existe um cliente cadastrado com esse CPF.");
        }

        cliente.Nome = request.Nome.Trim();
        cliente.Cpf = cpf;
        cliente.DataNascimento = request.DataNascimento.Value.Date;
        cliente.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();

        _context.SaveChanges();

        return ParaResponse(cliente);
    }

    public void Excluir(int id)
    {
        var cliente = BuscarCliente(id);

        _context.Clientes.Remove(cliente);
        _context.SaveChanges();
    }

    private Cliente BuscarCliente(int id)
    {
        var cliente = _context.Clientes
            .Include(c => c.Dividas)
            .FirstOrDefault(c => c.Id == id);

        if (cliente == null)
        {
            throw new NaoEncontradoException("Cliente não encontrado.");
        }

        return cliente;
    }

    private void ValidarDados(ClienteRequest request)
    {
        if (request == null)
        {
            throw new RegraNegocioException("Os dados do cliente são obrigatórios.");
        }

        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            throw new RegraNegocioException("O nome completo é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Cpf))
        {
            throw new RegraNegocioException("O CPF é obrigatório.");
        }

        if (!CpfValidador.EhValido(request.Cpf))
        {
            throw new RegraNegocioException("O CPF informado é inválido.");
        }

        if (request.DataNascimento == null)
        {
            throw new RegraNegocioException("A data de nascimento é obrigatória.");
        }

        if (request.DataNascimento.Value.Date > DateTime.Today)
        {
            throw new RegraNegocioException("A data de nascimento não pode ser no futuro.");
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && !EmailValidador.EhValido(request.Email))
        {
            throw new RegraNegocioException("O e-mail informado é inválido.");
        }
    }

    private static ClienteResponse ParaResponse(Cliente cliente)
    {
        return new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cpf = cliente.Cpf,
            DataNascimento = cliente.DataNascimento,
            Idade = cliente.CalcularIdade(),
            Email = cliente.Email,
            TotalDividas = cliente.CalcularTotalDividas()
        };
    }
}
