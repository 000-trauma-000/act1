using GatesApi.Models;

namespace GatesApi.Services;

public interface IAccessCodeValidator
{
    GateResponse Validate(string accessCode);
}
