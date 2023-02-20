using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.DTO.Response;
using SiecaAPI.Services;
using System.ComponentModel;

namespace SiecaAPI.Models.Services
{
    public static class TrainingCenterServices
    {
        public static async Task<DtoTrainingCenter> CreateAsync(DtoTrainingCenter center)
        {
            Organization organization = await OrganizationServices.GetActiveOrganization();
            center.OrganizationId = organization.Id;
            center.OrganizationName = organization.Name;

            return await DaoTrainingCenterFactory.GetDaoTrainingCenter().CreateAsync(center);
        }

        public static async Task<DtoTrainingCenter> UpdateAsync(DtoTrainingCenter center)
        {
            return await DaoTrainingCenterFactory.GetDaoTrainingCenter().UpdateAsync(center);
        }

        public static async Task<List<DtoTrainingCenter>> GetAllAsync(int page, int pageSize, string? fCode,
            string? fName, bool? fEnabled)
        {
            return await DaoTrainingCenterFactory.GetDaoTrainingCenter().GetAllAsync(page, pageSize, 
                fCode, fName, fEnabled);
        }

        public static async Task<DtoTrainingCenter> GetByIdAsync(Guid id)
        {
            return await DaoTrainingCenterFactory.GetDaoTrainingCenter().GetByIdAsync(id);
        }

        public static async Task<bool> DeleteAsync(Guid id)
        {
            return await DaoTrainingCenterFactory.GetDaoTrainingCenter().DeleteByIdAsync(id);
        }
    }
}
