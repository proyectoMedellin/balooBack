using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;
using SiecaAPI.Errors;
using SiecaAPI.Services;

namespace SiecaAPI.Models.Services
{
    public static class BeneficiariesServices
    {
        public static async Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary)
        {

            if(await DaoBeneficiariesFactory.GetDaoBeneficiaries()
                .ExistAsync(beneficiary.DocumentTypeId, beneficiary.DocumentNumber))
            {
                throw new ExistingBeneficiaryException("The beneficiary to create already exists");
            }


            Organization org = await OrganizationServices.GetActiveOrganization();
            beneficiary.OrganizationId = org.Id;
            
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().CreateAsync(beneficiary);
        }

        public static async Task<DtoBeneficiaries> UpdateAsync(DtoBeneficiaries beneficiary)
        {

            if (!await DaoBeneficiariesFactory.GetDaoBeneficiaries()
                .ExistAsync(beneficiary.DocumentTypeId, beneficiary.DocumentNumber))
            {
                throw new NoDataFoundException("The beneficiary to update dosen't exists");
            }

            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().UpdateAsync(beneficiary);
        }

        public static async Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetBeneficiaryParameterInfoByType(type);
        }

        public static async Task<bool> DeleteAsync(Guid id)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().DeleteAsync(id);
        }

        public static async Task<DtoBeneficiaries> GetById(Guid id)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetById(id);
        }
    }
}
