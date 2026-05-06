using VMS.EntityFramework.EntityModel;

namespace VMS.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(Staff staff, string roleName);
    }
}