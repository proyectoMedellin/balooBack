using Microsoft.VisualBasic;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.DataverseImpl;
using SecurityLayerDotNetAPI.Data.SQLImpl;

namespace SecurityLayerDotNetAPI.Data
{
    public static class DaoRolFactory
    {
        public static IDaoRoles GetDaoRoles()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoRolSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoRolDverseImpl(),
                _ => throw 
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
