namespace Vendinha.Api.Dtos;

public class ClienteRequest
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string Email { get; set; }
}
