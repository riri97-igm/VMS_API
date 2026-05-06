//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VMS.EntityFramework.EntityModel;

//namespace VMS.DataAccess.Interface
//{
//    public interface ILoginRepository
//    {
//        Task<Login> GetByUserNameAndPasswordAsync(string userName, string password);
//    }
//}


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
