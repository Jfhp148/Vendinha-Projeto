using System.Text.RegularExpressions;

namespace Vendinha.Api.Utils;

public static class EmailValidador
{
    public static bool EhValido(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
