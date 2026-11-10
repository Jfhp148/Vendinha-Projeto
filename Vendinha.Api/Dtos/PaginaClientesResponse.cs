namespace Vendinha.Api.Dtos;

public class PaginaClientesResponse
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalClientes { get; set; }
    public int TotalPaginas { get; set; }
    public decimal SomaTotalDividas { get; set; }
    public List<ClienteResponse> Clientes { get; set; }
}
