using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors;

namespace ДКС
{

    public class δy1_ИзменитьDlg : XtraForm
    {
        private SqlConnection                       sqlCN_;
        private GasTreeNode                         GasTreeNode_;
        private DateTime                            Date_;
        private DataRow                             R_;
        private DevExpress.XtraEditors.SimpleButton ButtonCancel;
        private DevExpress.XtraEditors.SimpleButton ButtonOk;

        #region-- Флаги                             готовности значений -------------------
        private bool флагТип                       = false;
        private bool флагНазначение                = false;
        #endregion-- Флаги                          готовности значений -------------------
        private IContainer                          components;
        private DevExpress.XtraEditors.GroupControl groupBoxГмрCurrent;
        private DevExpress.XtraEditors.GroupControl groupControl_ГМР;
        private TextEdit textEdit_ГмрИмяOld;
        private LabelControl labelControl2;
        private DevExpress.XtraEditors.GroupControl groupBox1;
        private MyCalcEdit CalcEdit_δy1;

        private DXValidationProvider dxValidationProvider1;



        #region-- КОНСТРУКТОР Гмр_ИзменитьDlg()--------------------
        public δy1_ИзменитьDlg
          (
            SqlConnection sqlCN_
           ,GasTreeNode   GasTreeNode_
           ,DateTime      Date_
          )
        {
            this.sqlCN_        = sqlCN_;
            this.GasTreeNode_  = GasTreeNode_;
            this.Date_         = Date_;

            InitializeComponent();

            this.ButtonCancel.Parent = this;
            this.ButtonOk.Parent     = this;
        }
        #endregion//-- КОНСТРУКТОР Гмр_ИзменитьDlg()--------------------

        #region-- Dispose( bool disposing )--------------------
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
        #endregion//-- Dispose( bool disposing )--------------------

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(δy1_ИзменитьDlg));
            this.ButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOk = new DevExpress.XtraEditors.SimpleButton();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
            this.groupControl_ГМР = new DevExpress.XtraEditors.GroupControl();
            this.groupBox1 = new DevExpress.XtraEditors.GroupControl();
            this.CalcEdit_δy1 = new ДКС.MyCalcEdit();
            this.groupBoxГмрCurrent = new DevExpress.XtraEditors.GroupControl();
            this.textEdit_ГмрИмяOld = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_ГМР)).BeginInit();
            this.groupControl_ГМР.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CalcEdit_δy1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxГмрCurrent)).BeginInit();
            this.groupBoxГмрCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_ГмрИмяOld.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonCancel.Appearance.ForeColor = System.Drawing.Color.Red;
            this.ButtonCancel.Appearance.Options.UseFont = true;
            this.ButtonCancel.Appearance.Options.UseForeColor = true;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(382, 55);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(117, 32);
            this.ButtonCancel.TabIndex = 0;
            this.ButtonCancel.Text = "Не сохранять";
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOk
            // 
            this.ButtonOk.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonOk.Appearance.ForeColor = System.Drawing.Color.Green;
            this.ButtonOk.Appearance.Options.UseFont = true;
            this.ButtonOk.Appearance.Options.UseForeColor = true;
            this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonOk.Location = new System.Drawing.Point(382, 12);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(117, 32);
            this.ButtonOk.TabIndex = 1;
            this.ButtonOk.Text = "Сохранить";
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // groupControl_ГМР
            // 
            this.groupControl_ГМР.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl_ГМР.AppearanceCaption.ForeColor = System.Drawing.Color.Gray;
            this.groupControl_ГМР.AppearanceCaption.Options.UseFont = true;
            this.groupControl_ГМР.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl_ГМР.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl_ГМР.Controls.Add(this.groupBox1);
            this.groupControl_ГМР.Controls.Add(this.groupBoxГмрCurrent);
            this.groupControl_ГМР.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl_ГМР.Location = new System.Drawing.Point(0, 0);
            this.groupControl_ГМР.Name = "groupControl_ГМР";
            this.groupControl_ГМР.Size = new System.Drawing.Size(360, 96);
            this.groupControl_ГМР.TabIndex = 25;
            this.groupControl_ГМР.Text = "δy1:";
            // 
            // groupBox1
            // 
            this.groupBox1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.AppearanceCaption.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.AppearanceCaption.Options.UseFont = true;
            this.groupBox1.AppearanceCaption.Options.UseForeColor = true;
            this.groupBox1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupBox1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupBox1.Controls.Add(this.CalcEdit_δy1);
            this.groupBox1.Location = new System.Drawing.Point(217, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 69);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.Text = "Новые значения:";
            // 
            // CalcEdit_δy1
            // 
            this.CalcEdit_δy1.Location = new System.Drawing.Point(5, 38);
            this.CalcEdit_δy1.Name = "CalcEdit_δy1";
            this.CalcEdit_δy1.Properties.Appearance.BorderColor = System.Drawing.Color.Silver;
            this.CalcEdit_δy1.Properties.Appearance.Options.UseBorderColor = true;
            this.CalcEdit_δy1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.CalcEdit_δy1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.CalcEdit_δy1.Properties.DisplayFormat.FormatString = "#0.######";
            this.CalcEdit_δy1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcEdit_δy1.Properties.EditFormat.FormatString = "##.######";
            this.CalcEdit_δy1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CalcEdit_δy1.Properties.Mask.EditMask = "\\d{1,2}(\\R.\\d{0,6})?";
            this.CalcEdit_δy1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.CalcEdit_δy1.Properties.MaxLength = 10;
            this.CalcEdit_δy1.Properties.NullText = "<null>";
            this.CalcEdit_δy1.Properties.NullValuePrompt = "-";
            this.CalcEdit_δy1.Properties.NullValuePromptShowForEmptyValue = true;
            this.CalcEdit_δy1.Properties.ShowCloseButton = true;
            this.CalcEdit_δy1.Size = new System.Drawing.Size(122, 20);
            this.CalcEdit_δy1.TabIndex = 4;
            // 
            // groupBoxГмрCurrent
            // 
            this.groupBoxГмрCurrent.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.groupBoxГмрCurrent.Appearance.Options.UseForeColor = true;
            this.groupBoxГмрCurrent.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxГмрCurrent.AppearanceCaption.ForeColor = System.Drawing.Color.Gray;
            this.groupBoxГмрCurrent.AppearanceCaption.Options.UseFont = true;
            this.groupBoxГмрCurrent.AppearanceCaption.Options.UseForeColor = true;
            this.groupBoxГмрCurrent.AppearanceCaption.Options.UseTextOptions = true;
            this.groupBoxГмрCurrent.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupBoxГмрCurrent.Controls.Add(this.textEdit_ГмрИмяOld);
            this.groupBoxГмрCurrent.Controls.Add(this.labelControl2);
            this.groupBoxГмрCurrent.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxГмрCurrent.Location = new System.Drawing.Point(2, 23);
            this.groupBoxГмрCurrent.Name = "groupBoxГмрCurrent";
            this.groupBoxГмрCurrent.Size = new System.Drawing.Size(209, 71);
            this.groupBoxГмрCurrent.TabIndex = 12;
            this.groupBoxГмрCurrent.Text = "Текущие значения:";
            // 
            // textEdit_ГмрИмяOld
            // 
            this.textEdit_ГмрИмяOld.Enabled = false;
            this.textEdit_ГмрИмяOld.Location = new System.Drawing.Point(106, 38);
            this.textEdit_ГмрИмяOld.Name = "textEdit_ГмрИмяOld";
            this.textEdit_ГмрИмяOld.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textEdit_ГмрИмяOld.Properties.Appearance.Options.UseFont = true;
            this.textEdit_ГмрИмяOld.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textEdit_ГмрИмяOld.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Gray;
            this.textEdit_ГмрИмяOld.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.textEdit_ГмрИмяOld.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.textEdit_ГмрИмяOld.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textEdit_ГмрИмяOld.Size = new System.Drawing.Size(87, 20);
            this.textEdit_ГмрИмяOld.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl2.Location = new System.Drawing.Point(11, 40);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(88, 16);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Точность δy1";
            // 
            // δy1_ИзменитьDlg
            // 
            this.AcceptButton = this.ButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(508, 96);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.groupControl_ГМР);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "δy1_ИзменитьDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ГМР: изменить реквизиты";
            this.Load += new System.EventHandler(this.δy1_ИзменитьDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_ГМР)).EndInit();
            this.groupControl_ГМР.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CalcEdit_δy1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxГмрCurrent)).EndInit();
            this.groupBoxГмрCurrent.ResumeLayout(false);
            this.groupBoxГмрCurrent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_ГмрИмяOld.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        #region-- Гмр_ИзменитьDlg_Load() -----------------
        private void δy1_ИзменитьDlg_Load(object sender, System.EventArgs e)
        {
            InitFieldValues();

            InitValidationRules();

            dxValidationProvider1.ValidationMode = ValidationMode.Auto;
            dxValidationProvider1.Validate();
        }
        #endregion//-- Гмр_ИзменитьDlg_Load() -----------------

        #region-- ButtonOk_Click() -----------------
        private void ButtonOk_Click(object sender, System.EventArgs e)
        {
            if (dxValidationProvider1.GetInvalidControls().Count > 0)
            {
                MessageBox.Show("Не все поля заполнены !", "Контроль", MessageBoxButtons.OK);
                return;
            };

            //int RowCount1_ =
            //              Т_ГМРы.ГМРы_UPDATE_Имя
            //              (
            //                sqlCN_
            //              , (int)R_["ГмрId"]
            //              , (string)textEdit_ГмрИмя.EditValue
            //              );


            //if (RowCount1_ == 1)
            //{
            //    GasTreeNode_.Text = (string)textEdit_ГмрИмя.EditValue;
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //    return;
            //}
            //else
            //{
            //    return;
            //}


        }
        #endregion-- ButtonOk_Click() -----------------

        #region-- ButtonCancel_Click() -----------------
        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }
        #endregion-- ButtonCancel_Click() -----------------


        #region-- InitValidationRules() -----------------
        private void InitValidationRules()
        {
            // <notEmptyTextEdit> 
            ConditionValidationRule notEmptyValidationRule = new ConditionValidationRule();
            notEmptyValidationRule.ConditionOperator = ConditionOperator.IsNotBlank;
            notEmptyValidationRule.ErrorText = "Не заполнено поле";
            // </notEmptyTextEdit>


            ConditionValidationRule notEqualsValidationRule = new ConditionValidationRule();
            notEqualsValidationRule.ConditionOperator = ConditionOperator.NotEquals;
            notEqualsValidationRule.Value1 = null;
            notEqualsValidationRule.ErrorText = "Please choose a value";
            notEqualsValidationRule.ErrorType = ErrorType.Critical;

            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule customValidationRule = new ConditionValidationRule();
            customValidationRule.ErrorText = "Please enter a valid person name";
            customValidationRule.ErrorType = ErrorType.Warning;

            dxValidationProvider1.SetValidationRule(this.CalcEdit_δy1, notEmptyValidationRule);

        }
        #endregion//-- InitValidationRules() --------------------------------------------

        #region//-- DataToEditor() --------------------------------------------
        private void InitFieldValues()
        {
            R_ = Т_ГМРы.ГМРы_SELECT_BY_ГмрIdDate
                (
                sqlCN_
                , (int)GasTreeNode_.УЗЕЛ_ID
                , Date_
                );

            if (R_ == null)
            {
                return;
            }

            this.textEdit_ГмрИмяOld.EditValue = R_["δy1"];
            this.CalcEdit_δy1.EditValue    = R_["δy1"];
        }
        #endregion//-- DataToEditor() --------------------------------------------


    }
}
