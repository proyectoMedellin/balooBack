using Microsoft.EntityFrameworkCore;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.SQLImpl.Entities;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.Errors;

namespace SecurityLayerDotNetAPI.Data.SQLImpl
{
    public class DaoAccessUserSqlImpl : IDaoAccessUsers
    {
        public async Task<DtoAccessUser> CreateAsync(DtoAccessUser user)
        {
            using SqlContext context = new SqlContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                if (user.OrganizationId == Guid.Empty) 
                    throw new MissingArgumentsException("Empty organization ID");

                OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id == user.OrganizationId).FirstAsync();
                if (org != null)
                {
                    AccessUserEntity accessUser = new(org, user.UserName, user.Email, 
                        user.FirstName, user.OtherNames, user.LastName, user.OtherLastName, 
                        user.RequirePaswordChange, true, user.CreatedBy, DateTime.Now, 
                        null, null, user.Phone, user.DocumentTypeId, user.DocumentNo);

                    await context.AccessUsers.AddAsync(accessUser);
     

                    user.Id = accessUser.Id;

                    string tmpPassword = SecurityTools.GeneratePassword();

                    AccessUserPasswordEntity password = new(user.Id.Value, tmpPassword, user.CreatedBy, 
                        DateTime.Now, null, null);

                    await context.AccessUsersPassword.AddAsync(password);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                    return user;
                }
                throw new NoDataFoundException("No organization found");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(DtoAccessUser org)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistUserByUserNamePass(string userName, string pass)
        {
            bool result = false;

            if (string.IsNullOrEmpty(userName))
                throw new MissingArgumentsException("Empty user name");

            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();

            if (user == null) throw new NoDataFoundException("No user found");

            AccessUserPasswordEntity passInfo = await context.AccessUsersPassword
                .Where(p => p.AccessUserId == user.Id).FirstAsync();

            if (passInfo == null) throw new NoDataFoundException("No pass found for user");

            result = passInfo.Password == pass;

            return result;
        }

        public async Task<List<DtoAccessUser>> GetAllAsync()
        {
            using SqlContext context = new SqlContext();
            OrganizationEntity org = await context.Organizations.FirstAsync();
            List<AccessUserEntity> users = await context.AccessUsers.Where(u => u.OrganizationId == org.Id).ToListAsync();
            List<DtoAccessUser> dtoAccessUser = new();
            foreach (AccessUserEntity user in users)
            {
                dtoAccessUser.Add(new DtoAccessUser(user.UserName, user.Email, user.FirstName, user.OtherNames,
                    user.LastName,user.OtherLastName,user.RequirePaswordChange,
                    user.CreatedBy,user.Phone,user.DocumentTypeId,user.DocumentNo));
            }
            return dtoAccessUser;
        }

        public async Task<DtoAccessUser> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DtoAccessUser> GetByUserNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new MissingArgumentsException("Empty user name");
            
            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == name).FirstAsync();

            return new DtoAccessUser(user.Id, user.UserName, user.Email, user.FirstName, user.OtherNames,
                user.LastName, user.OtherLastName, user.RequirePaswordChange, user.CreatedBy,
                user.Phone, user.DocumentTypeId, user.DocumentNo);
        }

        public async Task<List<DtoUserRol>> GetRolesByUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new MissingArgumentsException("Empty user name");

            using SqlContext context = new SqlContext();
            AccessUserEntity user = await context.AccessUsers
                .Where(u => u.UserName == userName).FirstAsync();

            //si el usuario existe, se considera un usuario interno, de lo contrario
            //se obtendrá la información de roles como un usuario externo
            List<AccessUserRolEntity> userRoles = user != null && user.Id != Guid.Empty ?
                    await context.AccessUserRoles.Where(aur => aur.AccessUserId == user.Id).ToListAsync() :
                    await context.AccessUserRoles.Where(aur => aur.AccessUserExternalId == userName).ToListAsync();

            List<DtoUserRol> roles = new List<DtoUserRol>();
            foreach (AccessUserRolEntity aur in userRoles)
            {
                RolEntity rol = await context.Roles.Where(r => r.Id.Equals(aur.RolId)).FirstAsync();
                roles.Add(new DtoUserRol()
                {
                    OrganizationId = aur.OrganizationId,
                    RolId = aur.RolId,
                    RolName = rol.Name
                });
            }
            return roles;
        }

        public async Task<bool> UpdateAsync(DtoAccessUser user, string oldUserName)
        {
            using SqlContext context = new SqlContext();
            try
            {
                if (user.OrganizationId == Guid.Empty)
                    throw new MissingArgumentsException("Empty organization ID");

                OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id == user.OrganizationId).FirstAsync();
                if (org != null)
                {
                    DtoAccessUser UserOld = await GetByUserNameAsync(oldUserName);
                    AccessUserEntity accessUser = await context.AccessUsers.FindAsync(UserOld.Id);
                    if (accessUser != null)
                    {
                        accessUser.OrganizationId = user.OrganizationId;
                        accessUser.UserName = user.UserName;
                        accessUser.Email = user.Email;
                        accessUser.FirstName = user.FirstName;
                        accessUser.OtherNames = user.OtherNames;
                        accessUser.LastName = user.LastName;
                        accessUser.OtherLastName = user.OtherLastName;
                        accessUser.ModifiedOn = DateTime.Now;
                        accessUser.Phone = user.Phone;
                        accessUser.DocumentTypeId= user.DocumentTypeId;
                        accessUser.DocumentNo = user.DocumentNo;
                        await context.SaveChangesAsync();
                    }
                    return true;
                }
                throw new NoDataFoundException("No organization found");
            }
            catch
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateUserPassword(Guid? id, string password)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
                throw new MissingArgumentsException("Empty user name");

            using SqlContext context = new SqlContext();
            AccessUserPasswordEntity accessUser = await context.AccessUsersPassword.Where(u => u.AccessUserId == id).FirstAsync();
            accessUser.Password = password;
            context.Update(accessUser);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
