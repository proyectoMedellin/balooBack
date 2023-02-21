using Microsoft.EntityFrameworkCore;
using SiecaAPI.Data.Interfaces;
using SiecaAPI.Data.SQLImpl.Entities;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;

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
                .Include(tc => tc.Campus)
                .ThenInclude(tc => tc.TrainingCenter)
                .ThenInclude(tc => tc.Organization)
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
                .Include(tc => tc.Campus)
                .ThenInclude(tc => tc.TrainingCenter)
                .ThenInclude(tc => tc.Organization)
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
                .Include(tc => tc.Campus)
                .ThenInclude(tc => tc.TrainingCenter)
                .ThenInclude(tc => tc.Organization)
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

        public async Task<List<DtoDevelopmentRoomGroupByYear>> GetAllGroupsByYear(
            Guid? DevRoomId, int? year, int? page, int? pageSize)
        {
            List<DtoDevelopmentRoomGroupByYear> groupList = new();

            using SqlContext context = new SqlContext();
            List<DevelopmentRoomGroupByYearEntity> grpYears;

            if (page.HasValue && pageSize.HasValue)
            {
                int skipData = page.Value > 0 ? (page.Value - 1) * pageSize.Value : 0;
                grpYears = await context
                    .DevelopmentRoomGroupByYearEntities
                    .Where(gy =>
                        ((DevRoomId.HasValue && gy.DevelopmentRoomId.Equals(DevRoomId.Value)) || !DevRoomId.HasValue) &&
                        ((year.HasValue && gy.Year.Equals(year.Value)) || !year.HasValue))
                    .Include(tc => tc.DevelopmentRoom)
                    .ThenInclude(tc => tc.Organization)
                    .Skip(skipData).Take(pageSize.Value)
                    .ToListAsync();
            }
            else
            {
                grpYears = await context
                    .DevelopmentRoomGroupByYearEntities
                    .Where(gy =>
                        ((DevRoomId.HasValue && gy.DevelopmentRoomId.Equals(DevRoomId.Value)) || !DevRoomId.HasValue) &&
                        ((year.HasValue && gy.Year.Equals(year.Value)) || !year.HasValue))
                    .Include(tc => tc.DevelopmentRoom)
                    .ThenInclude(tc => tc.Organization)
                    .ToListAsync();
            }
            foreach (DevelopmentRoomGroupByYearEntity dg in
                grpYears.Where(gy => gy.DevelopmentRoom.Enabled))
            {
                DevelopmentRoomEntity roomInfo = await context.DevelopmentRooms
                    .Where(r => r.Id.Equals(dg.DevelopmentRoomId))
                    .Include(r => r.Campus)
                    .ThenInclude(r => r.TrainingCenter)
                    .FirstAsync();

                DtoDevelopmentRoomGroupByYear groupInfo = new()
                {
                    Id = dg.Id,
                    OrganizationId = dg.OrganizationId,
                    OrganizationName = dg.Organization.Name,
                    TrainingCenterId = roomInfo.TrainingCenterId,
                    TrainingCenterCode = roomInfo.TrainingCenter.Code,
                    TrainingCenterName = roomInfo.TrainingCenter.Name,
                    CampusId = roomInfo.CampusId,
                    CampusCode = roomInfo.Campus.Code,
                    CampusName = roomInfo.Campus.Name,
                    DevelopmentRoomId = dg.DevelopmentRoomId,
                    DevelopmentRoomCode = dg.DevelopmentRoom.Code,
                    DevelopmentRoomName = dg.DevelopmentRoom.Name,
                    Year = dg.Year,
                    GroupCode = dg.GroupCode,
                    GroupName = dg.GroupName,
                    Enabled = dg.Enabled,
                    Agents = new()
                };

                //DtoDevelopmentRoomGroupAgent
                List<DevelopmentRoomGroupAgentEntity> agentsBase = await context.DevelopmentRoomGroupAgentEntities
                    .Where(drga => drga.DevelopmentRoomGroupByYearId.Equals(dg.Id))
                    .Include(drga => drga.AccessUser)
                    .ToListAsync();
                if (agentsBase != null && agentsBase.Count > 0)
                {
                    foreach (AccessUserEntity a in agentsBase.Select(a => a.AccessUser))
                    {
                        groupInfo.Agents.Add(
                            a.FirstName
                            + " "
                            + a.OtherNames
                            + " "
                            + a.LastName
                            + " "
                            + a.OtherLastName);
                    }
                }
                groupList.Add(groupInfo);
            }
            return groupList;
        }

        public async Task<bool> AssignAgentsByYear(Guid OrganizationId, Guid DevRoomId, int year, string groupCode, 
            string groupName, List<Guid> agentsIds, string createdBy)
        {
            using SqlContext context = new SqlContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                List<DevelopmentRoomGroupByYearEntity> currentAssignmentList = await context
                    .DevelopmentRoomGroupByYearEntities
                    .Where(dry => dry.DevelopmentRoomId.Equals(DevRoomId) && dry.Year.Equals(year))
                    .ToListAsync();
                
                if (currentAssignmentList != null && currentAssignmentList.Count > 0)
                {
                    DevelopmentRoomGroupByYearEntity currentAssignment = currentAssignmentList.First();
                    //se elimnan todos los datos de asignación existentes
                    //primero los agentes
                    context.RemoveRange(
                            await context.DevelopmentRoomGroupAgentEntities
                            .Where(drga => drga.DevelopmentRoomGroupByYearId.Equals(currentAssignment.Id))
                            .ToListAsync()
                        );
                    await context.SaveChangesAsync();
                    //elimino la asignación exsitente
                    context.Remove(currentAssignment);
                    await context.SaveChangesAsync();
                }


                OrganizationEntity org = await context.Organizations.Where(o => o.Id.Equals(OrganizationId)).FirstAsync();
                DevelopmentRoomEntity room = await context.DevelopmentRooms.Where(r => r.Id.Equals(DevRoomId)).FirstAsync();

                //se crea la nueva asignación
                DevelopmentRoomGroupByYearEntity mewAssignment = new()
                {
                    OrganizationId = org.Id,
                    Organization = org,
                    DevelopmentRoomId = DevRoomId,
                    DevelopmentRoom = room,
                    Year = year,
                    GroupCode = groupCode,
                    GroupName = groupName,
                    Enabled = true,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.UtcNow
                };
                context.DevelopmentRoomGroupByYearEntities.Add(mewAssignment);
                await context.SaveChangesAsync();
                foreach (Guid agent in agentsIds)
                {
                    AccessUserEntity user = await context.AccessUsers.Where(au => au.Id.Equals(agent)).FirstAsync();
                    DevelopmentRoomGroupAgentEntity newAgent = new()
                    {
                        DevelopmentRoomGroupByYearId = mewAssignment.Id,
                        DevelopmentRoomGroupByYear = mewAssignment,
                        AccessUserId = agent,
                        AccessUser = user,
                    };
                    await context.DevelopmentRoomGroupAgentEntities.AddAsync(newAgent);
                    await context.SaveChangesAsync();
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> DeleteGroupAssignment(Guid groupAssignmetId)
        {
            using SqlContext context = new SqlContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                List<DevelopmentRoomGroupByYearEntity> currentAssignmentList = await context
                    .DevelopmentRoomGroupByYearEntities
                    .Where(dry => dry.Id.Equals(groupAssignmetId))
                    .ToListAsync();

                if (currentAssignmentList != null && currentAssignmentList.Count > 0)
                {
                    DevelopmentRoomGroupByYearEntity currentAssignment = currentAssignmentList.First();
                    //se elimnan todos los datos de asignación existentes
                    //primero los agentes
                    context.RemoveRange(
                            await context.DevelopmentRoomGroupAgentEntities
                            .Where(drga => drga.DevelopmentRoomGroupByYearId.Equals(currentAssignment.Id))
                            .ToListAsync()
                        );
                    await context.SaveChangesAsync();
                    //elimino la asignación exsitente
                    context.Remove(currentAssignment);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                    return true;
                }
                return false;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}
