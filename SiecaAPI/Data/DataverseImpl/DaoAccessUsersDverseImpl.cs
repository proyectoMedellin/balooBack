﻿using SiecaAPI.Data.Interfaces;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.DataverseImpl
{
    public class DaoAccessUsersDverseImpl : IDaoAccessUsers
    {
        public async Task<DtoAccessUser> CreateAsync(DtoAccessUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(DtoAccessUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistUserByUserNamePass(string userName, string pass)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoAccessUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DtoAccessUser> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DtoAccessUser> GetByUserNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoUserRol>> GetRolesByUser(string userName)
        {

            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(DtoAccessUser user, string oldUserName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserPassword(Guid id, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserPassword(Guid? id, string password)
        {
            throw new NotImplementedException();
        }
    }
}
