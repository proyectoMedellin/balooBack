using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoAccessUsers
    {
        public Task<DtoAccessUser> CreateAsync(DtoAccessUser user);
        public Task<bool> UpdateAsync(DtoAccessUser user, string oldUserName);
        public Task<bool> DeleteAsync(DtoAccessUser user);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<DtoAccessUser> GetByIdAsync(Guid id);
        public Task<DtoAccessUser> GetByUserNameAsync(string name);
        public Task<List<DtoAccessUser>> GetAllAsync();
        public Task<bool> UpdateUserPassword(Guid? id, string password);
        public Task<bool> ExistUserByUserNamePass(string userName, string pass);
        public Task<List<DtoUserRol>> GetRolesByUser(string userName);
    }
}
