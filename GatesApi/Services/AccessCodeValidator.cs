using GatesApi.Models;

namespace GatesApi.Services;

public class AccessCodeValidator : IAccessCodeValidator
{
    private static readonly char[] SpecialSymbols = { '!', '@', '#', '$', '%' };

    public GateResponse Validate(string accessCode)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(accessCode))
        {
            errors.Add("Код доступа отсутствует или пуст.");
            return new GateResponse(false, errors, "Код отклонён.");
        }

        if (accessCode.Length < 8 || accessCode.Length > 16)
            errors.Add("Длина кода должна быть от 8 до 16 символов.");

        if (!accessCode.Any(c => c >= 'A' && c <= 'Z'))
            errors.Add("Нет заглавной латинской буквы.");

        if (!accessCode.Any(c => c >= 'a' && c <= 'z'))
            errors.Add("Нет строчной латинской буквы.");

        if (!accessCode.Any(c => c >= '0' && c <= '9'))
            errors.Add("Нет цифры.");

        if (!accessCode.Any(SpecialSymbols.Contains))
            errors.Add("Нет специального символа.");

        if (accessCode.Contains(' '))
            errors.Add("Код не должен содержать пробелов.");

        if (HasRepeats(accessCode))
            errors.Add("Обнаружены три одинаковых символа подряд.");

        if (HasDigitRun(accessCode))
            errors.Add("Обнаружена последовательность из трёх или более подряд идущих цифр.");

        if (!IsLetter(accessCode[0]))
            errors.Add("Первый символ должен быть буквой.");

        var last = accessCode[^1];
        if (!(last >= '0' && last <= '9') && !SpecialSymbols.Contains(last))
            errors.Add("Последний символ должен быть цифрой или специальным символом.");

        return new GateResponse(
            errors.Count == 0,
            errors,
            errors.Count == 0 ? "Код принят. Ворота открываются." : "Код отклонён.");
    }

    private static bool IsLetter(char c) =>
        (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');

    private static bool HasRepeats(string code)
    {
        for (var i = 0; i < code.Length - 2; i++)
        {
            if (code[i] == code[i + 1] && code[i + 1] == code[i + 2])
                return true;
        }

        return false;
    }

    private static bool HasDigitRun(string code)
    {
        var count = 0;

        foreach (var c in code)
        {
            if (c >= '0' && c <= '9')
            {
                count++;
                if (count >= 3)
                    return true;
            }
            else
            {
                count = 0;
            }
        }

        return false;
    }
}
