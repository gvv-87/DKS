using System;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars;


namespace ДКС
{
    public partial class W_Main : DevExpress.XtraEditors.XtraForm
  {
    string skinMask     = "Шкурка: ";
    int    projectIndex = 0;
    Cursor currentCursor;
    public ПаспортФирмы ПаспортФирмы;

    #region-- GET/SET -------------------------------------------
    public DevExpress.XtraBars.Docking.DockPanel DockPanel_Output  { get { return dockPanel_Output; } }
    public DevExpress.XtraEditors.MemoEdit       TextBox1          { get { return this.textBox1; } }
    public DevExpress.XtraEditors.MemoEdit       TextBox2          { get { return this.textBox2; } }
    #endregion-- GET/SET -------------------------------------------

    #region-- КОНСТРУКТОР W_Main() -------------------------------------------
    public W_Main()
    {
        DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitForm1));
        
        ПаспортФирмы  = new ПаспортФирмы(GLOBAL.SqlConn);
        ПаспортФирмы.ЧитатьПаспортФирмы();
        this.Text     = ПаспортФирмы.ФирмаИмя + " [ " + ПаспортФирмы.Месяц + "\\" + ПаспортФирмы.Год + " ]";

        InitializeComponent();
      
        xtraPropertyGrid1.PropertyGrid.AutoGenerateRows = true;
    }
    #endregion-- КОНСТРУКТОР W_Main() -------------------------------------------

    #region//-- CalendarSetDateApp()-----------------------------------------------------------
    private void CalendarSetDateApp()
    {
      //WinCalendar WinCalendar_ = new WinCalendar
      //                              (
      //                               ПаспортФирмы.Год,
      //                               ПаспортФирмы.Месяц,
      //                               ПаспортФирмы.Число
      //                               );

      //WinCalendar_.ShowDialog(this);
      ПаспортФирмы.Год = DateTime.Now.Year;
      ПаспортФирмы.Месяц = DateTime.Now.Month;
      ПаспортФирмы.Число = DateTime.Now.Day;
      ПаспортФирмы.ОбновитьПаспортФирмы();
    }
    #endregion//-- CalendarSetDateApp()-----------------------------------------------------------

    #region== Skins ===========================================
    #region-- void InitSkins() -------------------------------------------
    void InitSkins()
    {
      barManager1.ForceInitialize();
      if (barManager1.GetController().PaintStyleName == "Skin")
      {
        //DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2013");
        iPaintStyle.Caption = skinMask + DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName;
        iPaintStyle.Hint    = iPaintStyle.Caption;
      }
      foreach (DevExpress.Skins.SkinContainer cnt in DevExpress.Skins.SkinManager.Default.Skins)
      {
        BarButtonItem item = new BarButtonItem(barManager1, skinMask + cnt.SkinName);
        item.Name          = "bi" + cnt.SkinName;
        item.Id            = barManager1.GetNewItemId();
        iPaintStyle.AddItem(item);
        item.ItemClick += new ItemClickEventHandler(OnSkinClick);
      }
    }
    #endregion-- void InitSkins() -------------------------------------------

    #region-- void OnSkinClick(...) -------------------------------------------
    void OnSkinClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      string skinName                                                = e.Item.Caption.Replace(skinMask, "");
      DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skinName);
      barManager1.GetController().PaintStyleName                     = "Шкурка";
      //solutionExplorer1.barManager1.GetController().PaintStyleName = "Skin";
      iPaintStyle.Caption                                            = e.Item.Caption;
      iPaintStyle.Hint                                               = iPaintStyle.Caption;
      iPaintStyle.ImageIndex                                         = -1;
    }
    #endregion-- void OnSkinClick(...) -------------------------------------------
    #endregion== Skins ===========================================

    #region-- void InitDockMode() -------------------------------------------
    void InitDockMode()
    {
      Array arr = Enum.GetValues(typeof(DevExpress.XtraBars.Docking.Helpers.DockMode));
      foreach (object mode in arr)
        repositoryItemImageComboBox1.Items.Add(new ImageComboBoxItem(mode.ToString(), mode, -1));
      beiDockMode.EditValue = dockManager1.DockMode;
    }
    #endregion-- void InitDockMode() -------------------------------------------

    private void beiDockMode_EditValueChanged(object sender, System.EventArgs e)
    {
      dockManager1.DockMode = (DevExpress.XtraBars.Docking.Helpers.DockMode)beiDockMode.EditValue;
    }


    #region-- void InitFrmMain() -------------------------------------------
    private void InitFrmMain()
    {
      dockPanel_FindResults.HideImmediately();
      dockPanel_TaskList.HideImmediately();
      dockPanel_Output.HideImmediately();
      dockPanel_Properties.HideImmediately();

      AddControls(this, comboBox1);
      comboBox1.SelectedIndex = 0;
      comboBox2.SelectedIndex = 0;
      textBox1.ContextMenu    = textBox2.ContextMenu = new ContextMenu();
      this.dockPanel_Explorer.Controls.Add(new GasExplorer(ref GLOBAL.SqlConn, this.ПаспортФирмы));

      DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

      CalendarSetDateApp();

    }
    #endregion-- void InitFrmMain() -------------------------------------------

    #region-- void AddNewForm(string s) -------------------------------------------
    private void AddNewForm(string s)
    {
      Form newForm = new Form();
      newForm.MdiParent = this;

      newForm.Text                = s;
      //newForm.MinimumSize.Width = 20;   
      RichTextBox tb              = new RichTextBox();
      tb.Dock                     = DockStyle.Fill;
      tb.BorderStyle              = BorderStyle.None;
      tb.SelectionChanged        += new EventHandler(tb_SelectionChanged);
      barManager1.SetPopupContextMenu(tb, popupMenu2);
      newForm.Controls.Add(tb);
      newForm.Show();
    }
    #endregion-- void AddNewForm(string s) -------------------------------------------

    private RichTextBox ActiveRTB
    {
      get
      {
        if (this.ActiveMdiChild != null) return this.ActiveMdiChild.Controls[0] as RichTextBox;
        return null;
      }
    }

    private void InitEdit()
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null)
      {
        iCut.Enabled   = iCopy.Enabled = rtb.SelectedText != "";
        iPaste.Enabled = rtb.CanPaste(DataFormats.GetFormat(0));
        iUndo.Enabled  = rtb.CanUndo;
        iRedo.Enabled  = rtb.CanRedo;
      }
      else
      {
        iCut.Enabled = iCopy.Enabled = iPaste.Enabled = iUndo.Enabled = iRedo.Enabled = false;
      }
    }

    private void tb_SelectionChanged(object sender, EventArgs e)
    {
      InitEdit();
    }

    private void AddControls(Control container, DevExpress.XtraEditors.ComboBoxEdit cb)
    {
      foreach (object obj in container.Controls)
      {
        cb.Properties.Items.Add(obj);
        if (obj is Control) AddControls(obj as Control, cb);
      }
    }

    private void repositoryItemComboBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter && eFind.EditValue != null)
        repositoryItemComboBox1.Items.Add(eFind.EditValue.ToString());
    }

    private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      if (comboBox2.SelectedIndex == 0) textBox2.Text = "";
      else textBox2.Text = "";
    }

    private void textBox2_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      e.Handled = true;
    }

    private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      xtraPropertyGrid1.PropertyGrid.SelectedObject = comboBox1.SelectedItem;
    }

    private void ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      projectIndex++;
      AddNewForm(e.Item.Hint + projectIndex.ToString());
    }

    private void iCascade_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      this.LayoutMdi(MdiLayout.Cascade);
    }

    private void iHorizontal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      this.LayoutMdi(MdiLayout.TileHorizontal);
    }

    private void iVertical_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      this.LayoutMdi(MdiLayout.TileVertical);
    }

    private void iAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      W_About dlg = new W_About("");
      dlg.ShowDialog();
    }

    private void frmMain_MdiChildActivate(object sender, System.EventArgs e)
    {
      InitEdit();
    }

    private void iCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null) rtb.Cut();
    }

    private void iCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null) rtb.Copy();
    }

    private void iPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null) rtb.Paste();
    }

    private void iSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null)
      {
        rtb.SelectAll();
        rtb.Focus();
      }
    }

    private void iUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null) rtb.Undo();
      InitEdit();
    }

    private void iRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      RichTextBox rtb = ActiveRTB;
      if (rtb != null) rtb.Redo();
      InitEdit();
    }

    private void iSolutionExplorer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      dockPanel_Explorer.Show();
    }

    private void iProperties_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      dockPanel_Properties.Show();
    }

    private void iTaskList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      dockPanel_TaskList.Show();
    }

    private void iFindResults_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) { dockPanel_FindResults.Show();}
    private void BarButtomItem_Output_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) { dockPanel_Output.Show();}
   
     
    

    private void iToolbox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      dockPanel6.Show();
    }

    private void solutionExplorer1_PropertiesItemClick(object sender, System.EventArgs e)
    {
      dockPanel_Properties.Show();
    }

    private void iSaveLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      SaveFileDialog dlg = new SaveFileDialog();
      dlg.Filter = "XML files (*.xml)|*.xml";
      dlg.Title = "Save Layout";
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        Refresh(true);
        barManager1.SaveToXml(dlg.FileName);
        Refresh(false);
      }
    }
    private void iLoadLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      OpenFileDialog dlg   = new OpenFileDialog();
      dlg.Filter           = "XML files (*.xml)|*.xml|All files|*.*";
      dlg.Title            = "Restore Layout";
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        Refresh(true);
        try   { barManager1.RestoreFromXml(dlg.FileName);}
        catch { }
        Refresh(false);
      }
    }

    private void Refresh(bool isWait)
    {
      if (isWait)
      {
        currentCursor  = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
      }
      else
        Cursor.Current = currentCursor;
      this.Refresh();
    }

    private void iExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      this.Close();
    }

    private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      iStatus2.Caption = "'" + e.Item.Caption + "' has been clicked";
    }

    private void ips_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      barManager1.GetController().PaintStyleName = e.Item.Description;
      InitPaintStyle(e.Item);
      barManager1.GetController().ResetStyleDefaults();
      DevExpress.LookAndFeel.UserLookAndFeel.Default.SetDefaultStyle();
    }

    private void InitPaintStyle(DevExpress.XtraBars.BarItem item)
    {
      if (item == null) return;
      iPaintStyle.ImageIndex  = item.ImageIndex;
      iPaintStyle.Caption     = item.Caption;
      iPaintStyle.Hint        = item.Description;
      //solutionExplorer1.barManager1.GetController().PaintStyleName = barManager1.GetController().PaintStyleName;
      //solutionExplorer1.barManager1.GetController().ResetStyleDefaults();
    }

    private void ips_Init()
    {
        BarItem item = null;
        for (int i = 0; i < barManager1.Items.Count; i++)
        {
            if (barManager1.Items[i].Description == barManager1.GetController().PaintStyleName) item = barManager1.Items[i];
        }
        InitPaintStyle(item);
    }

    void InitTabbedMDI()
    {
      xtraTabbedMdiManager1.MdiParent = biTabbedMDI.Down ? this : null;
      iCascade.Enabled                = iHorizontal.Enabled = iVertical.Enabled = !biTabbedMDI.Down;
    }
    private void biTabbedMDI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      InitTabbedMDI();
    }

    private void frmMain_Load(object sender, System.EventArgs e)
    {
      ips_Init();
      InitSkins();
      InitTabbedMDI();
      InitDockMode();
      BeginInvoke(new MethodInvoker(InitFrmMain));
      dockPanel_Explorer.Show();

    }

    private void xtraTabbedMdiManager1_SelectedPageChanged(object sender, EventArgs e)
    {
      //   MessageBox.Show("Закладка поменялась");
      //     if (this.xtraTabbedMdiManager1.SelectedPage.Text == "Проводки IFRS")
      {

        //      MessageBox.Show("IFRS Tab !");
        //           this.xtraTabbedMdiManager1.SelectedPage.MdiChild.Update();
      }
    }

    private void barButtonItemCloseAllWindows_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (GLOBAL.gMainForm.MdiChildren.Length > 0)
        {

            Form[] f = GLOBAL.gMainForm.MdiChildren;
            foreach (Form fr_ in f)
            {
                fr_.Close();
            }
        }

        dockPanel_Explorer.Close();
        dockPanel_Properties.Close();
        dockPanel_FindResults.Close();
        dockPanel_TaskList.Close();
        dockPanel_Output.Close();
        dockPanel_Properties.Close();

    }

    private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
    {

    }
  }
}
