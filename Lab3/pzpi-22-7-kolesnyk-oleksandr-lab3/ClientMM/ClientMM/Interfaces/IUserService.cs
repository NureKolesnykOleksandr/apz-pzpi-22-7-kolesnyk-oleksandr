using ClientMM.Dtos;
using ClientMM.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientMM.Interfaces
{
    public interface IUserService
    {
        Task<LoginResultDto?> Register(RegisterDto registerDto);

        Task<LoginResultDto?> Login(LoginDto loginDto, string ip);
        Task<IdentityResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto);

        Task<List<User>> GetUsers();

        Task<IdentityResult> UpdateProfile(int userId, UpdateProfileDto updateProfileDto);

        Task<IdentityResult> UpdateUserOptions(int userId, UpdateUserOptionsDto updateUserOptionsDto);

        Task<IdentityResult> BanUser(int userId, string password);
        Task<IdentityResult> UnBanUser(int userId, string password);
    }
}
