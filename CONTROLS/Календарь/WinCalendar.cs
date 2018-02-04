using System;
using System.Windows.Forms;

namespace ДКС
{
    public class WinCalendar : System.Windows.Forms.Form
    {
        protected DateTime DateBegin_;
        protected DateTime DateEnd_;
        protected DateTime ДатаПриложения_;

        public DateTime ДатаПриложения  { set { ДатаПриложения_  = value; }  get { return ДатаПриложения_; }}
        public DateTime DateBegin       { set { DateBegin_       = value; }  get { return DateBegin_; }}
        public DateTime DateEnd         { set { DateEnd_         = value; }  get { return DateEnd_; }}

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Button        buttonOk;
        private System.ComponentModel.Container    components = null;

        public WinCalendar(int yyyy_, int mm_, int dd_)
        {
            InitializeComponent();
            SetMonth(this.monthCalendar1, yyyy_, mm_, dd_);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the Код_Районы editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinCalendar));
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            resources.ApplyResources(this.monthCalendar1, "monthCalendar1");
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.ShowTodayCircle = false;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.BackColor = System.Drawing.Color.Snow;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.ForeColor = System.Drawing.Color.ForestGreen;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // WinCalendar
            // 
            this.AcceptButton = this.buttonOk;
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.LightGreen;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.monthCalendar1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinCalendar";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.WinCalendar_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WinCalendar_KeyDown);
            this.ResumeLayout(false);

        }
        #endregion

        private void monthCalendar1_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            ДатаПриложения_ = e.Start;

            //Ret_Date        = e.Start;
            //e.Start           = this.monthCalendar1.SelectionStart;

        }

        private void WinCalendar_Load(object sender, System.EventArgs e)
        {
            ДатаПриложения_ = this.monthCalendar1.SelectionStart;
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            ДатаПриложения_ = this.monthCalendar1.SelectionStart;
            DateBegin       = this.monthCalendar1.SelectionStart;
            DateEnd         = this.monthCalendar1.SelectionEnd;
            DialogResult    = DialogResult.OK;
            this.Close();
        }

        private void WinCalendar_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        #region//-- public static void SetMonth()-----------------------
        public static void SetMonth( System.Windows.Forms.MonthCalendar monthCalendar_, int yyyy_, int mm_, int dd_)
        {
            //monthCalendar_.MinDate             = new System.DateTime(yyyy_, mm_, 1, 0, 0, 0, 0);
            //monthCalendar_.MaxDate             = LastDayOfMonthOfYear(yyyy_, mm_);
            monthCalendar_.MaxSelectionCount   = 1;
            monthCalendar_.SelectionStart      = new System.DateTime(yyyy_, mm_, dd_, 0, 0, 0, 0);
        }
        #endregion//-- public static void SetMonth()-----------------------

        #region//-- public static DateTime LastDayOfMonthOfYear()-----------------------
        public static DateTime LastDayOfMonthOfYear(int yyyy_, int mm_)
        {
            DateTime LastDayOfMonth = new DateTime(3000, 01, 01);
            switch (mm_)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: LastDayOfMonth = new System.DateTime(yyyy_, mm_, 31, 0, 0, 0, 0); break;

                case 4:
                case 6:
                case 9:
                case 11: LastDayOfMonth = new System.DateTime(yyyy_, mm_, 30, 0, 0, 0, 0); break;

                case 2: int f_dd_ = ((int)yyyy_ % 4 == 0) ? 29 : 28;
                        LastDayOfMonth = new System.DateTime(yyyy_, mm_, f_dd_, 0, 0, 0, 0);
                        break;

            }

            return LastDayOfMonth;
        }
        #endregion//-- public static DateTime LastDayOfMonthOfYear()-----------------------







    }
}
