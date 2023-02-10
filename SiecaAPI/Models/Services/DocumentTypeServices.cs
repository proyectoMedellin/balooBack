using SecurityLayerDotNetAPI.Data;
using SecurityLayerDotNetAPI.DTO.Data;
using SecurityLayerDotNetAPI.Models;

namespace SecurityLayerDotNetAPI.Services
{
    public static class DocumentTypeServices
    {

        public static async Task<List<DTODocumentType>> GetAllDocumentTypeAsync()
        {
            
            List<DTODocumentType> documentTypes = await DaoDocumentTypeFactory
                .GetDaoDocumenTypes().GetAllAsync();
            return documentTypes;
        }
    }
}
