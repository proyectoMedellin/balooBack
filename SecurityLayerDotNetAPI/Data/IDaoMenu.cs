using SecurityLayerDotNetAPI.DTO.Data;

namespace SecurityLayerDotNetAPI.Data
{
    public interface IDaoMenu
    {
        public Task<DtoMenu> CreateAsync(DtoMenu menu);
        public Task<DtoMenu> UpdateAsync(DtoMenu menu);
        public Task<List<DtoMenu>> GetItems(string user);
        public Task<bool> DeleteAsync(DtoMenu menu);
        public Task<bool> DeleteByIdAsync(Guid id);
    }
}
