namespace SiecaAPI.Commons
{
    public class PrConstants
    {
        //motores DB
        public const string DBMS_SQL = "SQL";
        public const string DBMS_DATAVERSE = "DATAVERSE";
        public const string DBMS_WRONG_PARAM_ERROR = "Parametro DBMS no establecido correctamente, " +
                "los valores permitidos son: " + DBMS_SQL + ", " + DBMS_DATAVERSE;


        //permisos
        public const string PERMISSION_TYPE_MENU = "Menu";
        public const string PERMISSION_TYPE_BUTTON = "Button";
        public const string PERMISSION_TYPE_FIELD = "Field";

        public enum PERMISSION_TYPES {
            Menu, Button, Field
        }

        //seguridad
        public const int USERDATAPARAMSCOUNT = 3;
        public const int USERNAMEPOS = 0;
        public const int USERPASSPOS = 1;
        public const int USERLOGINDATEPOS = 2;
        public const string USERDATASEPARATOR = "||";
        public const int USERLOGINMAXVERIFICATIONTIME = 10000;
    }
}
