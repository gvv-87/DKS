using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace ДКС
{
    public sealed class GLOBAL
    {
        public static readonly DateTime DateEndDb = new DateTime(3000, 01, 01); //-- конечная дата в системе и БД
        public static SqlConnection SqlConn;
        //public static clsActionHandler        gDefaultActionHandler   = new clsActionHandler();
        //public static stUserInfo              gUserInfo               = new stUserInfo();

        //-- Инициализируются приложением --------
        public static Form gMainForm;
        public static bool FlagGasTreeNodeActions = false; //-- Стас - отмена InsDelUpdate для узлов : Гмр, Дкс, КЦех
        //public static clsConfig               gConfig;

        public delegate bool dlgHandleSqlException(System.Data.SqlClient.SqlException e);
        public static dlgHandleSqlException gHandleSqlException;

        //public delegate void    dlgDSAfterThreadStartHandler(clsForm form, clsDataSet sender, System.Threading.Thread thread, clsDataSet.enmContext context);
        //public static           dlgDSAfterThreadStartHandler gDSAfterThreadStartHandler;

        public delegate void dlgThreadExceptionHandler(Exception e, StackTrace st);
        public static dlgThreadExceptionHandler gThreadExceptionHandler;


        //-- Также нужно инициализировать:  ----------
        // clsForm.smBuildFrame - Опционально

        //Globals.gUserInfo.UserID            = user_id;
        //Globals.gUserInfo.CompanyName       = company;
        //Globals.gUserInfo.UserDisplayName   = username;
        //Globals.gUserInfo.UserName          = login;
    
    
    
    public static string КинаНеБудет = "Кина не будет - киньщик заболел !";
    
    
    }

}
