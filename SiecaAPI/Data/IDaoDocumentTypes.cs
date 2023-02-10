﻿using SecurityLayerDotNetAPI.DTO.Data;

namespace SecurityLayerDotNetAPI.Data
{
    public interface IDaoDocumentTypes
    {
        public Task<DTODocumentType> CreateAsync(DTODocumentType documentType);
        public Task<DTODocumentType> UpdateAsync(DTODocumentType documentType);
        public Task<bool> DeleteAsync(DTODocumentType documentType);
        public Task<bool> DeleteByIdAsync(Guid id);
        public Task<List<DTODocumentType>> GetAllAsync();
    }
}
