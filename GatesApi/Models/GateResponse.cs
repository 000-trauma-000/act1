namespace GatesApi.Models;

public record GateResponse(bool IsValid, List<string> Errors, string Message);
