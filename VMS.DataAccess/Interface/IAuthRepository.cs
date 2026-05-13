using VMS.Model.DTOs.Auth;

namespace VMS.DataAccess.Interface
{
    public interface IAuthRepository
    {
        Task<(bool Success, string RoleName, VMS.EntityFramework.EntityModel.Staff? Staff)> LoginAsync(LoginRequestDTO dto);
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequestDTO dto);
        Task<bool> ChangePasswordAsync(int staffId, string currentPassword, string newPassword);
    }
}
