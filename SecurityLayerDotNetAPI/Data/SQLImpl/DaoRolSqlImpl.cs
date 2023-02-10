using Microsoft.EntityFrameworkCore;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.SQLImpl.Entities;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.Errors;

namespace SecurityLayerDotNetAPI.Data.SQLImpl
{
    public class DaoRolSqlImpl : IDaoRoles
    {
        public async Task<DtoRol> CreateAsync(DtoRol rol)
        {
            using SqlContext context = new SqlContext();
            if (rol.OrganizationId == Guid.Empty)
                throw new MissingArgumentsException("Empty organization ID");

            OrganizationEntity org = await context.Organizations
                .Where(o => o.Id == rol.OrganizationId).FirstAsync();
            if (org != null)
            {
                RolEntity newRol = new(org, rol.Name,rol.Description,rol.NewAccessUserDefaultRol,
                    rol.Enabled, rol.CreatedBy, DateTime.Now, null, null);
                await context.Roles.AddAsync(newRol);
                await context.SaveChangesAsync();
                rol.Id = newRol.Id;
                return rol;
            }
            throw new NoDataFoundException("No organization found");
        }

        public async Task<bool> DeleteAsync(DtoRol rol)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DtoRol>> GetAllAsync()
        {
            using SqlContext context = new SqlContext();
            List<RolEntity> roles = await context.Roles.Where(o => o.Enabled).ToListAsync();
            List<DtoRol> dtoRoles = new();
            foreach (RolEntity doc in roles)
            {
                dtoRoles.Add(new DtoRol(doc.Id, doc.OrganizationId, doc.Name, doc.Description,
                    doc.NewAccessUserDefaultRol, doc.Enabled, doc.CreatedBy, doc.CreatedOn,
                    doc.ModifiedBy, doc.ModifiedOn));
            }
            return dtoRoles;
        }

        public async Task<DtoRol> GetByIdAsync(Guid id)
        {
            using SqlContext context = new SqlContext();
            RolEntity rol = await context.Roles.Where(r => r.Id.Equals(id)).FirstAsync();
            return new DtoRol(rol.Id, rol.OrganizationId, rol.Name, rol.Description, rol.NewAccessUserDefaultRol, rol.Enabled,
                rol.CreatedBy, rol.CreatedOn, rol.ModifiedBy, rol.ModifiedOn);
        }

        public async Task<DtoRol> GetByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task<DtoRol> UpdateAsync(DtoRol rol)
        {
            using SqlContext context = new SqlContext();
            if (rol.OrganizationId == Guid.Empty)
                throw new MissingArgumentsException("Empty organization ID");
        
            OrganizationEntity org = await context.Organizations
                .Where(o => o.Id == rol.OrganizationId).FirstAsync();

            RolEntity updateRol = await context.Roles.Where(r => r.Id.Equals(rol.Id)).FirstAsync();

            if (org != null && updateRol.Id != Guid.Empty)
            {
                updateRol.OrganizationId = org.Id;
                updateRol.Name = rol.Name;
                updateRol.Description = rol.Description;
                updateRol.NewAccessUserDefaultRol = rol.NewAccessUserDefaultRol;
                updateRol.Enabled = rol.Enabled;
                updateRol.ModifiedBy = rol.ModifiedBy;
                updateRol.ModifiedOn = DateTime.Now;

                context.Update(updateRol);
                await context.SaveChangesAsync();

                return rol;
            }
            throw new NoDataFoundException("No organization or valid rol founnd");
        }
    }
}
