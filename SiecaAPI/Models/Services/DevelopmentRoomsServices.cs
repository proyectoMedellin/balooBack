using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Services;

namespace SiecaAPI.Models.Services
{
    public static class DevelopmentRoomsServices
    {
        public static async Task<DtoDevelopmentRoom> CreateAsync(DtoDevelopmentRoom room)
        {
            if(string.IsNullOrEmpty(room.Code) || string.IsNullOrEmpty(room.Name))
                throw new MissingFieldException("The fields code and name cant be empty");

            DtoCampus cmp = await CampusesServices.GetByIdAsync(room.CampusId);
            DtoTrainingCenter tc = await TrainingCenterServices.GetByIdAsync(cmp.TrainingCenterId);
            DtoOrganization org = await OrganizationServices.GetOrganizationById(tc.OrganizationId);

            if (tc.Id == Guid.Empty || !org.Id.HasValue)
            {
                throw new NoDataFoundException("the parent records couldn´t be found");
            }

            room.OrganizationId = org.Id.Value;
            room.TrainingCenterId = tc.Id;
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().CreateAsync(room);
        }

        public static async Task<DtoDevelopmentRoom> UpdateAsync(DtoDevelopmentRoom room)
        {
            if (string.IsNullOrEmpty(room.Code) || string.IsNullOrEmpty(room.Name))
                throw new MissingFieldException("The fields code and name cant be empty");

            DtoCampus cmp = await CampusesServices.GetByIdAsync(room.CampusId);
            DtoTrainingCenter tc = await TrainingCenterServices.GetByIdAsync(cmp.TrainingCenterId);
            DtoOrganization org = await OrganizationServices.GetOrganizationById(tc.OrganizationId);

            if (tc.Id == Guid.Empty || !org.Id.HasValue)
            {
                throw new NoDataFoundException("the parent records couldn´t be found");
            }

            room.OrganizationId = org.Id.Value;
            room.TrainingCenterId = tc.Id;
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().UpdateAsync(room);
        }

        public static async Task<List<DtoDevelopmentRoom>> GetAllAsync(int page, int pageSize, 
            Guid? campusId, string? fCode, string? fName, bool? fEnabled)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetAllAsync(page, pageSize,
                campusId, fCode, fName, fEnabled);
        }

        public static async Task<List<DtoDevelopmentRoom>> GetEnableDevRoomsByCampus(Guid campusId)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetActiveByCampusAsync(campusId);
        }

        public static async Task<DtoDevelopmentRoom> GetByIdAsync(Guid campusId)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().GetByIdAsync(campusId);
        }

        public static async Task<bool> DeleteAsync(Guid id)
        {
            return await DaoDevelopmentRoomFactory.GetDaoDevelopment().DeleteByIdAsync(id);
        }
    }
}
