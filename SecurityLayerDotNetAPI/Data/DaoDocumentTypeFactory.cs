using Microsoft.VisualBasic;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.DataverseImpl;
using SecurityLayerDotNetAPI.Data.SQLImpl;
namespace SecurityLayerDotNetAPI.Data
{
    public static class DaoDocumentTypeFactory
    {
        public static IDaoDocumentTypes GetDaoDocumenTypes()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoDocumentTypeSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoDocumentTypeDverseImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
