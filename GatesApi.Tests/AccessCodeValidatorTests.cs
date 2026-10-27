using GatesApi.Services;

namespace GatesApi.Tests;

public class AccessCodeValidatorTests
{
    private readonly AccessCodeValidator _validator = new();

    [Theory]
    [InlineData("Dragon7!")]
    [InlineData("Qwerty9@")]
    [InlineData("Abcdefgh1jKl2#3$")]
    public void ValidCodeIsAccepted(string code)
    {
        var result = _validator.Validate(code);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Equal("Код принят. Ворота открываются.", result.Message);
    }

    [Fact]
    public void ExampleFromSpecReturnsExpectedErrors()
    {
        var result = _validator.Validate("dragon123");

        Assert.False(result.IsValid);
        Assert.Equal(new[]
        {
            "Нет заглавной латинской буквы.",
            "Нет специального символа.",
            "Обнаружена последовательность из трёх или более подряд идущих цифр."
        }, result.Errors);
        Assert.Equal("Код отклонён.", result.Message);
    }

    [Theory]
    [InlineData("Ab1!xY9", "Длина кода должна быть от 8 до 16 символов.")]
    [InlineData("Abcdefgh1jKl2#3$9", "Длина кода должна быть от 8 до 16 символов.")]
    [InlineData("DRAGON7@", "Нет строчной латинской буквы.")]
    [InlineData("Dragon!$#", "Нет цифры.")]
    [InlineData("DragonX24", "Нет специального символа.")]
    [InlineData("Dragon 7!", "Код не должен содержать пробелов.")]
    [InlineData("Drrr7!x9", "Обнаружены три одинаковых символа подряд.")]
    [InlineData("Gate123!", "Обнаружена последовательность из трёх или более подряд идущих цифр.")]
    [InlineData("1Dragon7!", "Первый символ должен быть буквой.")]
    [InlineData("Dragon7!a", "Последний символ должен быть цифрой или специальным символом.")]
    public void SingleViolationIsReported(string code, string expected)
    {
        var result = _validator.Validate(code);

        Assert.False(result.IsValid);
        Assert.Equal(new[] { expected }, result.Errors);
    }
}
