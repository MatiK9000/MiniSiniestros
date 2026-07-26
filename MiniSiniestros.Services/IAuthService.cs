using MiniSiniestros.Dto;

namespace MiniSiniestros.Services;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
}