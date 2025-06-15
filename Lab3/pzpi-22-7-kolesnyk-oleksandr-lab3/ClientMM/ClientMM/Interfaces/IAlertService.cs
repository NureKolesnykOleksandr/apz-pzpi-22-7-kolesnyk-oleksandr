using ClientMM.Dtos;
using Microsoft.AspNetCore.Identity;

namespace ClientMM.Interfaces
{
    public interface IAlertService
    {
        Task<IdentityResult> CreateAlert(CreateAlertDto alertDto);
    }
}
