
using SecurityLayerDotNetAPI.Data;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.Models;

namespace SecurityLayerDotNetAPI.Services
{
    public static class OrganizationServices
    {
        public static async Task<Organization> CreateOrganizationAsync(string orgName, string creator)
        {
            DtoOrganization org = new(orgName, creator);
            org = await  DaoOrganizationFactory.GetDaoOrganizations().CreateAsync(org);
            
            if (org.Id == Guid.Empty) 
                throw new InvalidOperationException("La organización no fue creada exitosamente");

            return new Organization(org.Id.Value, org.Name);
           
        }
        public static async Task<Organization> GetActiveOrganization()
        {
            var orgResult = await DaoOrganizationFactory.GetDaoOrganizations().GetAllEnabledAsync();
            DtoOrganization org = orgResult.First();
            if (org.Id.HasValue)
            {
                return new Organization(org.Id.Value, org.Name);
            }
            throw new InvalidOperationException("No organization Found");
            
        }
    }
}
