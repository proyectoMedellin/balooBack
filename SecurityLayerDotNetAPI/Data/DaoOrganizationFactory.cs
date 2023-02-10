using Microsoft.VisualBasic;
using SecurityLayerDotNetAPI.Commons;
using SecurityLayerDotNetAPI.Data.DataverseImpl;
using SecurityLayerDotNetAPI.Data.SQLImpl;

namespace SecurityLayerDotNetAPI.Data
{
    public static class DaoOrganizationFactory
    {
        public static IDaoOrganizations GetDaoOrganizations()
        {
            return AppParamsTools.GetEnvironmentVariable("DBMS") switch
            {
                PrConstants.DBMS_SQL => new DaoOrganizationSqlImpl(),
                PrConstants.DBMS_DATAVERSE => new DaoOrganizationsDverseImpl(),
                _ => throw 
                new Exception(PrConstants.DBMS_WRONG_PARAM_ERROR),
            };
        }
    }
}
