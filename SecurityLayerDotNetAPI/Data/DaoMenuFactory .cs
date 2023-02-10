using Microsoft.VisualBasic;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.DataverseImpl;
using SecurityLayerDotNetAPI.Data.SQLImpl;

namespace SecurityLayerDotNetAPI.Data
{
    public static class DaoMenuFactory
    {
        public static IDaoMenu GetDaoMenu()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoMenuSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoMenuDverseImpl(),
                _ => throw
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
