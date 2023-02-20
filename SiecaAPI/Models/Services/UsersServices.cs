using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Models;

namespace SiecaAPI.Services
{
    public static class UsersServices
    {
        public static async Task<AccessUser> CreateAccessUserAsync(string userName, string email,
            string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requiredPaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo)
        {
            Organization org = await OrganizationServices.GetActiveOrganization();

            DtoAccessUser user = new(userName, email, firstName, otherNames, lastName,
                otherLastName, requiredPaswordChange, createdBy, phone, documentTypeId, documentNo);
            user.OrganizationId = org.Id;
            user = await DaoAccessUserFactory.GetDaoAccessUsers().CreateAsync(user);

            if (!user.Id.HasValue) throw new InvalidOperationException("El usuario no fue creada exitosamente");

            return new AccessUser(user.Id.Value, user.OrganizationId, user.UserName, user.Email, user.FirstName,
                user.OtherNames, user.LastName, user.OtherLastName, user.DocumentTypeId, user.DocumentNo);
        }

        public static async Task<DtoAccessUser> GetUserInfo(string userName)
        {
            DtoAccessUser user = await DaoAccessUserFactory
                .GetDaoAccessUsers().GetByUserNameAsync(userName);
            return user;
        }
        public static async Task<bool> UpdateAccessUserAsync(string oldUserName, string userName, string email,
            string firstName, string? otherNames, string lastName, string? otherLastName,
            bool requiredPaswordChange, string createdBy, string? phone, Guid documentTypeId, string documentNo)
        {
            Organization org = await OrganizationServices.GetActiveOrganization();
            DtoAccessUser user = new(userName, email, firstName, otherNames, lastName,
                otherLastName, requiredPaswordChange, createdBy, phone, documentTypeId, documentNo);
            user.OrganizationId = org.Id;
            bool response = await DaoAccessUserFactory.GetDaoAccessUsers().UpdateAsync(user, oldUserName);
            if (!response) throw new InvalidOperationException("El usuario no fue creada exitosamente");

            return response;

        }
        public static async Task<bool> ChangePassword(Guid id, string password)
        {
            bool canChangePassword = await DaoAccessUserFactory
                .GetDaoAccessUsers().UpdateUserPassword(id, password);
            return canChangePassword;
        }
        public static async Task<List<DtoAccessUser>> GetAllUsersByOrganization()
        {
            List<DtoAccessUser> users = await DaoAccessUserFactory.
                GetDaoAccessUsers().GetAllAsync();
            return users;
        }
    }
}
