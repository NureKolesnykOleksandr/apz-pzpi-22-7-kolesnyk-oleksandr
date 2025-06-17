using Microsoft.AspNetCore.Identity;
using ServerMM.Dtos;
using ServerMM.Models;

namespace ServerMM.Interfaces
{
    public interface IUserRepository
    {
        Task<LoginResultDto?> Register(RegisterDto registerDto);

        Task<LoginResultDto?> Login(LoginDto loginDto, string ip);
        Task<IdentityResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto);

        Task<List<User>> GetUsers();

        Task<IdentityResult> UpdateProfile(int userId, UpdateProfileDto updateProfileDto);

        Task<IdentityResult> UpdateUserOptions(int userId, UpdateUserOptionsDto updateUserOptionsDto);

        Task<IdentityResult> BanUser(int userId);
        Task<IdentityResult> UnBanUser(int userId);

        Task<bool> IfAdmin(string password);

    }
}
