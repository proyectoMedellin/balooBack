﻿using SiecaAPI.DTO.Data;

namespace SiecaAPI.Data.Interfaces
{
    public interface IDaoBeneficiaries
    {
        public Task<DtoBeneficiaries> CreateAsync(DtoBeneficiaries beneficiary);
        public Task<DtoBeneficiaries> UpdateAsync(DtoBeneficiaries beneficiary);
        public Task<DtoBeneficiaries> UpdatePhotoUrl(Guid beneficiaryId, string url);
        public Task<bool> ExistAsync(Guid docmentTypeId, string documentNumber);
        public Task<List<DtoBeneficiaries>> GetAllAsync(int? year, Guid? trainingCenterId, 
            Guid? campusId, Guid? developmentRoomId, string? documentNumber, 
            string? name, bool? fEnabled, int? page, int? pageSize);
        public Task<DtoBeneficiaries> GetById(Guid id);
        public Task<List<DtoBeneficiariesParameters>> GetBeneficiaryParameterInfoByType(string type);
        public Task<bool> DeleteAsync(Guid id);
    }
}
