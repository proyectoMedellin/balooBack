using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoDevelopmentRoom
    {
        public Task<DtoDevelopmentRoom> CreateAsync(DtoDevelopmentRoom devRoom);
        public Task<DtoDevelopmentRoom> UpdateAsync(DtoDevelopmentRoom devRoom);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DtoDevelopmentRoom>> GetAllAsync(int page, int pageSize, Guid? campusId, 
            string? fCode, string? fName, bool? fEnabled);
        public Task<List<DtoDevelopmentRoom>> GetActiveByCampusAsync(Guid campusId);
        public Task<DtoDevelopmentRoom> GetByIdAsync(Guid id);
    }
}
