using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using DevExpress.XtraEditors.Controls;

namespace ДКС
{
    public sealed class MainModule
	{
		public delegate void ShowStatusDelegate(string text);
		public static DialogResult      DialogResultLogin;
		public static W_Main           GetMainForm() {	return ((W_Main) GLOBAL.gMainForm); }
		
		public static Application       gApp;
		public const string             gConfigFileName = "ДКС.exe.config";
		
		static W_Login                 gLoginForm;
		
		
		// ===============================================================
		// ТОЧКА ВХОДА В ПРОГРАММУ
		// ===============================================================
		[STAThread]
		static public void Main(string[] args)
		{
            Thread.CurrentThread.CurrentUICulture                  = new CultureInfo("ru");
            Localizer.Active                                       = Localizer.CreateDefaultLocalizer();
            System.Threading.Thread.CurrentThread.CurrentCulture   =  new System.Globalization.CultureInfo("ru");//-- The following line provides localization for data formats.
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");//-- The following line provides localization for the application's user interface.
            //-- Note that the above code should be added BEFORE calling the Run() method.  
            
            Utils.WriteToLog("\r\n" + new String( '=',50) + "\r\n" + "ЗАПУСК" + "\r\n" + "Версия проекта = " + Application.ProductVersion + "\r\n" + new String( '=',50), false);
			
            
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
			
			if (! System.IO.File.Exists(gConfigFileName))
			{
				Utils.WriteToLog("\r\n Отсутствует конфигурационный файл (" + gConfigFileName + ")", false);
				MessageBox.Show("Отсутствует конфигурационный файл (" + gConfigFileName + ")");
				return;
			}
            
            //-- Обработка любых входящих аргументов
            GLOBAL.FlagGasTreeNodeActions = true;
            for (int i = 0; i < args.Length; i++)
            {
                //if (args[0].Trim().ToUpper() == "ВНИИГАЗ") GLOBAL.FlagGasTreeNodeActions = true;
                break;
			}
            
            GLOBAL.gHandleSqlException             = Utils.HandleSqlException;
			Utils.CustomExceptionHandler eh        = new Utils.CustomExceptionHandler();
			Application.ThreadException           += new System.Threading.ThreadExceptionEventHandler(eh.OnThreadException);
			GLOBAL.gThreadExceptionHandler         = eh.ThreadExceptionHandler;
			gLoginForm                             = new W_Login();
			DialogResultLogin                      = gLoginForm.ShowDialog();
			
			if (DialogResultLogin == DialogResult.Cancel) {	Utils.WriteToLog("\r\n Не удалось войти по Login:", false); return; }

			GLOBAL.gMainForm = new W_Main();
			Application.Run(GLOBAL.gMainForm);
			Utils.WriteToLog("\r\n" + new String('=',50) + "\r\n" + "ШТАТНОЕ ЗАВЕРШЕНИЕ ПРОГРАММЫ" + "\r\n" + "Версия проекта = " + Application.ProductVersion + "\r\n" + new String('=',50), false);
		}
		
	}
	
}
