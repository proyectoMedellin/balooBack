using SecurityLayerDotNetAPI.Data;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.Models;

namespace SecurityLayerDotNetAPI.Services
{
    public static class PermissionServices
    {

        public static async Task<List<DtoPermission>> GetAllPermissionAsync()
        {
            List<DtoPermission> Permissions = await DaoPermissionFactory
                .GetDaoPermission().GetAllAsync();
            return Permissions;
        }
        public static async Task<List<DtoPermission>> GetAllPermissionByUserNameAsync(string userName)
        {
            List<DtoPermission> Permissions = await DaoPermissionFactory
                .GetDaoPermission().GetAllByUserNameAsync(userName);
            return Permissions;
        }
    }
}
