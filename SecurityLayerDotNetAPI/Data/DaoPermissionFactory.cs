using Microsoft.VisualBasic;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.DataverseImpl;
using SecurityLayerDotNetAPI.Data.SQLImpl;

namespace SecurityLayerDotNetAPI.Data
{
    public static class DaoPermissionFactory
    {
        public static IDaoPermissions GetDaoPermission()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoPermissionSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoPermissionDverseImpl(),
                _ => throw 
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
