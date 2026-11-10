namespace Vendinha.Api.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }

    public List<Divida> Dividas { get; set; } = new List<Divida>();

    public int CalcularIdade()
    {
        var hoje = DateTime.Today;
        int idade = hoje.Year - DataNascimento.Year;

        if (DataNascimento.Date > hoje.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }

    public decimal CalcularTotalDividas()
    {
        return Dividas.Where(d => !d.Paga).Sum(d => d.Valor);
    }
}
