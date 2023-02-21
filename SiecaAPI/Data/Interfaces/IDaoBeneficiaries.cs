using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoBeneficiaries
    {
        public Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary);
        public Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type);
    }
}
