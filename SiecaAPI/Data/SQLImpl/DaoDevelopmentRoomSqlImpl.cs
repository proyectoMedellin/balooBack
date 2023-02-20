using Microsoft.EntityFrameworkCore;
using SiecaAPI.Controllers;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using System.Drawing.Printing;
using System.Linq;

namespace SiecaAPI.Data.SQLImpl
{
    public class DaoDevelopmentRoomSqlImpl : IDaoDevelopmentRoom
    {
        public async Task<DtoDevelopmentRoom> CreateAsync(DtoDevelopmentRoom devRoom)
        {
            using SqlContext context = new();
            OrganizationEntity org = await context.Organizations
                    .Where(o => o.Id == devRoom.OrganizationId).FirstAsync();

            TrainingCenterEntity tc = await context.TrainingCenters
                .Where(t => t.Id.Equals(devRoom.TrainingCenterId)).FirstAsync();

            CampusEntity camp = await context.Campuses
                .Where(cp => cp.Id.Equals(devRoom.CampusId)).FirstAsync();

            if (org == null || tc == null || camp == null)
                throw new NoDataFoundException("Organization or training center or campus not found");

            DevelopmentRoomEntity newRoom = new()
            {
                OrganizationId = devRoom.OrganizationId,
                Organization = org,
                TrainingCenterId = devRoom.TrainingCenterId,
                TrainingCenter = tc,
                CampusId = devRoom.CampusId,
                Campus = camp,
                Code = devRoom.Code.ToUpper(),
                Name = devRoom.Name.ToUpper(),
                IntegrationCode = devRoom.IntegrationCode,
                DahuaChannelCode = devRoom.DahuaChannelCode,
                Enabled = devRoom.Enabled,
                CreatedBy = devRoom.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            await context.DevelopmentRooms.AddAsync(newRoom);
            await context.SaveChangesAsync();

            devRoom.Id = newRoom.Id;
            devRoom.OrganizationId = newRoom.OrganizationId;
            devRoom.OrganizationName = org.Name;
            devRoom.TrainingCenterId = newRoom.TrainingCenterId;
            devRoom.TrainingCenterName = tc.Name;
            devRoom.CampusId = newRoom.CampusId;
            devRoom.CampusCode = camp.Code;
            devRoom.CampusName = camp.Name;

            return devRoom;
        }

        public async Task<DtoDevelopmentRoom> UpdateAsync(DtoDevelopmentRoom devRoom)
        {
            using SqlContext context = new();
            DevelopmentRoomEntity oldRoom = await context.DevelopmentRooms
                .Where(c => c.Id.Equals(devRoom.Id)).FirstAsync();

            if (oldRoom != null)
            {
                if (!devRoom.CampusId.Equals(oldRoom.CampusId))
                {
                    CampusEntity newCamp = await context.Campuses
                        .Where(t => t.Id.Equals(devRoom.CampusId)).FirstAsync();
                    if(newCamp == null) throw new NoDataFoundException("The campus dosent exists");

                    oldRoom.CampusId = devRoom.CampusId;
                    oldRoom.Campus = newCamp;
                }

                oldRoom.Code = devRoom.Code.ToUpper();
                oldRoom.Name = devRoom.Name.ToUpper();
                oldRoom.IntegrationCode = devRoom.IntegrationCode;
                oldRoom.DahuaChannelCode = devRoom.DahuaChannelCode;
                oldRoom.Enabled = devRoom.Enabled;
                oldRoom.ModifiedBy = devRoom.ModifiedBy;
                oldRoom.ModifiedOn = DateTime.Now;
                await context.SaveChangesAsync();

                devRoom.OrganizationId = oldRoom.OrganizationId;
                devRoom.OrganizationName = oldRoom.Organization.Name;
                devRoom.TrainingCenterId = oldRoom.TrainingCenterId;
                devRoom.TrainingCenterName = oldRoom.TrainingCenter.Name;
                devRoom.CampusId = oldRoom.CampusId;
                devRoom.CampusCode = oldRoom.Campus.Code;
                devRoom.CampusName = oldRoom.Campus.Name;
            }
            else
            {
                throw new NoDataFoundException("The entity to update was not found");
            }
            return devRoom;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            try {
                using SqlContext context = new();

                var roomToDelete = await context.DevelopmentRooms
                    .Where(tc => tc.Id.Equals(id)).FirstAsync();
                if (roomToDelete == null)
                {
                    throw new NoDataFoundException("The entity to delete was not found");
                }

                context.Remove(roomToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<DtoDevelopmentRoom>> GetAllAsync(int page, int pageSize, Guid? campusId,
            string? fCode, string? fName, bool? fEnabled)
        {
            List<DtoDevelopmentRoom> devRooms = new();

            using SqlContext context = new();
            int skipData = page > 0 ? (page - 1) * pageSize: 0;

            List<DevelopmentRoomEntity> rooms = await context.DevelopmentRooms
                .Where( tc =>
                    (!campusId.HasValue || campusId.Value.Equals(tc.CampusId)) &&
                    (string.IsNullOrEmpty(fCode) || (!string.IsNullOrEmpty(fCode) && tc.Code.Contains(fCode.ToLower()))) &&
                    (string.IsNullOrEmpty(fName) || (!string.IsNullOrEmpty(fName) && tc.Name.Contains(fName.ToLower()))) &&
                    (!fEnabled.HasValue || (fEnabled.HasValue && tc.Enabled == fEnabled.Value))
                )
                .Include(tc => tc.TrainingCenter)
                .Include(tc => tc.Campus)
                .Skip(skipData).Take(pageSize).ToListAsync();

            foreach (DevelopmentRoomEntity tc in rooms)
            {
                devRooms.Add(new DtoDevelopmentRoom() { 
                    Id = tc.Id,
                    OrganizationId = tc.OrganizationId,
                    OrganizationName = tc.Organization.Name,
                    TrainingCenterId = tc.TrainingCenterId,
                    TrainingCenterCode = tc.TrainingCenter.Code,
                    TrainingCenterName = tc.TrainingCenter.Name,
                    CampusId = tc.CampusId,
                    CampusCode = tc.Campus.Code,
                    CampusName = tc.Campus.Name,
                    Code = tc.Code,
                    Name = tc.Name,
                    IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                    DahuaChannelCode = tc.DahuaChannelCode,
                    Enabled = tc.Enabled
                });
            }
            return devRooms;
        }

        public async Task<List<DtoDevelopmentRoom>> GetActiveByCampusAsync(Guid campusId)
        {
            List<DtoDevelopmentRoom> devRooms = new();

            using SqlContext context = new();
            List<DevelopmentRoomEntity> rooms = await context.DevelopmentRooms
                .Where(tc => tc.CampusId.Equals(campusId))
                .Include(tc => tc.TrainingCenter)
                .Include(tc => tc.Campus)
                .ToListAsync();

            foreach (DevelopmentRoomEntity tc in rooms)
            {
                devRooms.Add(new DtoDevelopmentRoom()
                {
                    Id = tc.Id,
                    OrganizationId = tc.OrganizationId,
                    OrganizationName = tc.Organization.Name,
                    TrainingCenterId = tc.TrainingCenterId,
                    TrainingCenterCode = tc.TrainingCenter.Code,
                    TrainingCenterName = tc.TrainingCenter.Name,
                    CampusId = tc.CampusId,
                    CampusCode = tc.Campus.Code,
                    CampusName = tc.Campus.Name,
                    Code = tc.Code,
                    Name = tc.Name,
                    IntegrationCode = !string.IsNullOrEmpty(tc.IntegrationCode) ? tc.IntegrationCode : String.Empty,
                    DahuaChannelCode = tc.DahuaChannelCode,
                    Enabled = tc.Enabled
                });
            }
            return devRooms;
        }

        public async Task<DtoDevelopmentRoom> GetByIdAsync(Guid id)
        {
            using SqlContext context = new();

            DevelopmentRoomEntity dr = await context.DevelopmentRooms
                .Where(dr => dr.Id.Equals(id))
                .Include(dr => dr.Organization)
                .Include(dr => dr.TrainingCenter)
                .Include(dr => dr.Campus)
                .FirstAsync();

            return new DtoDevelopmentRoom()
            {
                Id = dr.Id,
                OrganizationId = dr.OrganizationId,
                OrganizationName = dr.Organization.Name,
                TrainingCenterId = dr.TrainingCenterId,
                TrainingCenterCode = dr.TrainingCenter.Code,
                TrainingCenterName = dr.TrainingCenter.Name,
                CampusId = dr.CampusId,
                CampusCode = dr.Campus.Code,
                CampusName = dr.Campus.Name,
                Code = dr.Code,
                Name = dr.Name,
                IntegrationCode = !string.IsNullOrEmpty(dr.IntegrationCode) ? dr.IntegrationCode : String.Empty,
                DahuaChannelCode = dr.DahuaChannelCode,
                Enabled = dr.Enabled
            };
        }
    }
}
