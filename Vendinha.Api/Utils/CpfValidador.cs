namespace Vendinha.Api.Utils;

public static class CpfValidador
{
    public static string SomenteNumeros(string cpf)
    {
        if (cpf == null)
        {
            return "";
        }

        return new string(cpf.Where(char.IsDigit).ToArray());
    }

    public static bool EhValido(string cpf)
    {
        string numeros = SomenteNumeros(cpf);

        if (numeros.Length != 11)
        {
            return false;
        }

        if (numeros.Distinct().Count() == 1)
        {
            return false;
        }

        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += (numeros[i] - '0') * (10 - i);
        }
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (digito1 != numeros[9] - '0')
        {
            return false;
        }

        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += (numeros[i] - '0') * (11 - i);
        }
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return digito2 == numeros[10] - '0';
    }
}
