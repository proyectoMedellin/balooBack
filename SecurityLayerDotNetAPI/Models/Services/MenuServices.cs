using SecurityLayerDotNetAPI.Data;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.Models;

namespace SecurityLayerDotNetAPI.Services
{
    public static class MenuServices
    {

        public static async Task<List<DtoMenu>> GetAllMenuItemasAsync(string user)
        {

            List<DtoMenu> menuItems = await DaoMenuFactory.
                GetDaoMenu().GetItems(user);
            return menuItems;
        }
    }
}
