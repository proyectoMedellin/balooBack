using SiecaAPI.Data.Factory;
using SiecaAPI.DTO.Data;

namespace SiecaAPI.Models.Services
{
    public static class BeneficiariesServices
    {
        public static async Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().CreateAsync(beneficiary);
        }

        public static async Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type)
        {
            return await DaoBeneficiariesFactory.GetDaoBeneficiaries().GetBeneficiaryParameterInfoByType(type);
        }
    }
}
