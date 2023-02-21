using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoBeneficiaries
    {
        public Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary);
        public Task<bool> ExistAsync(Guid docmentTypeId, string documentNumber);
        public Task<DtoBeneficiaries> GetById(Guid id);
        public Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type);
        public Task<bool> DeleteAsync(Guid id);
    }
}
