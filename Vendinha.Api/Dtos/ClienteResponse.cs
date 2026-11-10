namespace Vendinha.Api.Dtos;

public class ClienteResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateTime DataNascimento { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public decimal TotalDividas { get; set; }
}
