using Microsoft.VisualBasic;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.DataverseImpl;
using SecurityLayerDotNetAPI.Data.SQLImpl;

namespace SecurityLayerDotNetAPI.Data
{
    public static class DaoRolPermissionFactory
    {
        public static IDaoRolPermission GetDaoRolPermission()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoRolPermissionSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoRolPermissionDverseImpl(),
                _ => throw 
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
