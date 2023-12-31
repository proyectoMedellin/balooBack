﻿namespace SiecaAPI.Commons
{
    public class PrConstants
    {
        //motores DB
        public const string DBMS_SQL = "SQL";
        public const string DBMS_DATAVERSE = "DATAVERSE";
        public const string DBMS_WRONG_PARAM_ERROR = "Parametro DBMS no establecido correctamente, " +
                "los valores permitidos son: " + DBMS_SQL + ", " + DBMS_DATAVERSE;


        //permisos
        public const string ROL_NAME = "SUPER_ADMINISTRADOR";
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
        public const int USERLOGINMAXVERIFICATIONTIME = 30000;

        //DSSPRO - emociones
        public const string REQUEST_TYPE_POST = "POST";
        public const string REQUEST_TYPE_PUT = "PUT";
        public const int EMOTION_NOT_FOUND = -1;
        public const int EMOTION_SMILE = 0;
        public const int EMOTION_ANGRY = 1;
        public const int EMOTION_SAD = 2;
        public const int EMOTION_DISGUSTED = 3;
        public const int EMOTION_SCARED = 4;
        public const int EMOTION_SURPRISED = 5;
        public const int EMOTION_NORMAL = 6;
        public const int EMOTION_LAUGH = 7;
        public const int EMOTION_HAPPY = 8;
        public const int EMOTION_CONFUSED = 9;
        public const int EMOTION_SCREAM = 10;

        public static string GetEmotionName(int emotionId) {
            string response = emotionId switch
            {
                EMOTION_SMILE => "Alegría",
                EMOTION_ANGRY => "Enfado",
                EMOTION_SAD => "Tristeza",
                EMOTION_DISGUSTED => "Enfado",
                EMOTION_SCARED => "Miedo",
                EMOTION_SURPRISED => "Sorpresa",
                EMOTION_NORMAL => "Calma y control",
                EMOTION_LAUGH => "Alegría",
                EMOTION_HAPPY => "Alegría",
                EMOTION_CONFUSED => "Anticipación",
                EMOTION_SCREAM => "Miedo",
                _ => "NO_IDENTIFICADO",
            };
            return response;
        }
    }
}
