using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
//using QAlbum;
using DevExpress.XtraEditors;


namespace ДКС
{
    public class SQLPhotosCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        private SqlConnection SqlConn_;
        private int           ГАЗОбъект_ID_;
        private int           ГАЗТипОбъекта_ID_;
        private bool          FlagUpdateNodes_ = false;


        #region//-- ПЕРЕМЕННЫЕ ФОРМЫ  ---------------------
        private QAlbum.ScalablePictureBox pictureBox;
        private System.Windows.Forms.TreeView TreePhotos;

        private System.Windows.Forms.OpenFileDialog FileOpenDlg;
        private System.Windows.Forms.MenuItem mnuAddPhoto;
        private System.Windows.Forms.MenuItem mnuRenameAlbum;
        private System.Windows.Forms.MenuItem mnuDeleteAlbum;
        private System.Windows.Forms.ContextMenu mnuPhotoPopup;
        private System.Windows.Forms.MenuItem mnuRenamePhoto;
        private System.Windows.Forms.MenuItem mnuDeletePhoto;
        private System.Windows.Forms.ContextMenu mnuAlbumPopup;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.MenuItem mnuAlbumEdit;
        private System.Windows.Forms.MenuItem mnuPhotoEdit;
        private System.Windows.Forms.ContextMenu mnuImage;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit_barEditItem_DateДУ;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private ImageList imageList1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_AddAlbum;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_EditDescription;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_DelAlbumPhoto;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_PrintPhoto;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_AddPhoto;
        private SplitContainerControl splitContainerControl1;
        private IContainer components;
        #endregion//-- ПЕРЕМЕННЫЕ ФОРМЫ  ---------------------

        #region--// FlagFillData -- get/set -------------------------------------------
        public bool FlagUpdateNodes
        {
            set
            {
                FlagUpdateNodes_ = value;
                LoadTree();
            }
        }
        #endregion--// FlagFillData -- get/set -------------------------------------------

        #region--// GET/SET -------------------------------------------
        public int ГАЗОбъект_ID { set { ГАЗОбъект_ID_ = value; } }
        public int ГАЗТипОбъекта_ID { set { ГАЗТипОбъекта_ID_ = value; } }
        #endregion--// GET/SET -------------------------------------------


        #region//--	КОНСТРУКТОР SQLPhotosCtrl()  ---------------------
        public SQLPhotosCtrl(SqlConnection SqlConn_)
        {
            this.SqlConn_ = SqlConn_;
            InitializeComponent();
        }
        #endregion//-- КОНСТРУКТОР SQLPhotosCtrl()  ---------------------

        #region//-- protected override void Dispose( bool disposing )  ---------------------
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
        #endregion//-- protected override void Dispose( bool disposing )  ---------------------


        #region//-- LoadTree()  ---------------------
        /// <summary>
        /// This method queries the database to obtain all album and photos
        /// then inserts the records into the tree  control
        /// </summary>
        private void LoadTree()
        {
            pictureBox.Picture = null;  //-- Clear the image

            try
            {
                TreePhotos.Nodes.Clear();												//-- Start with clear tree
                if (SqlConn_.State == ConnectionState.Closed) SqlConn_.Open();			//-- If the connection is closed, open it.
                if (SqlConn_.State == ConnectionState.Open)								//-- Just to be sure make sure it was opened 
                {
                    SqlCommand sqlCmd = new SqlCommand("ГАЗФотоальбомы_SELECT_by_ГАЗОбъектТип", SqlConn_);	//-- Create the stored proc command to retrieve the records
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    #region//-- ГАЗОбъект_ID ------------------------------------
                    sqlCmd.Parameters.Add(new SqlParameter("@ГАЗОбъект_ID", SqlDbType.Int));
                    sqlCmd.Parameters["@ГАЗОбъект_ID"].Value = ГАЗОбъект_ID_;
                    #endregion//-- ГАЗОбъект_ID ------------------------------------

                    #region//-- ГАЗТипОбъекта_ID ------------------------------------
                    sqlCmd.Parameters.Add(new SqlParameter("@ГАЗТипОбъекта_ID", SqlDbType.Int));
                    sqlCmd.Parameters["@ГАЗТипОбъекта_ID"].Value = ГАЗТипОбъекта_ID_;
                    #endregion//-- ГАЗТипОбъекта_ID ------------------------------------

                    SqlDataReader sqlPhotoReader = sqlCmd.ExecuteReader();	//-- Now execute the command
                    int nAlbumID = 0;						                //-- Keep track of the current album
                    int nAlbumIndex = -1;						            //-- Keep track of the tree index

                    while (sqlPhotoReader.Read())							//-- Keep reading all the records returned --
                    {
                        if (nAlbumID != (int)sqlPhotoReader["Фотоальбом_ID"])		//-- Check if a new album needs to be added to the tree	--
                        {
                            //-- Insert a new album	--
                            InsertAlbum(sqlPhotoReader["Фотоальбом_Имя"].ToString(), (int)sqlPhotoReader["Фотоальбом_ID"]);
                            nAlbumID = (int)sqlPhotoReader["Фотоальбом_ID"];
                            nAlbumIndex++;
                        }
                        //-- Insert the photo in the tree under the current album
                        if (sqlPhotoReader["Фото_ID"] != DBNull.Value)
                            InsertPhoto(nAlbumIndex, sqlPhotoReader["Фото_Описание"].ToString(), (int)sqlPhotoReader["Фото_ID"]);
                    }

                    sqlPhotoReader.Close();									//-- We are finished with this for now so close it
                    TreePhotos.ExpandAll();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion//-- LoadTree()  ---------------------

        #region//-- InsertAlbum()  ---------------------
        /// <summary>This method will inset an album into the tree</summary>
        /// <param name="strName">Album name</param>
        /// <param name="nAlbum">DB index of album</param>
        private void InsertAlbum(string @Фотоальбом_Имя, int @Фотоальбом_ID)
        {
            try
            {
                TreeNode node = new TreeNode(@Фотоальбом_Имя);					//-- Create a new treenode
                node.Tag      = new TreeItem(NodeType.Album, @Фотоальбом_ID);	//-- Create new treeitem to store the info about this album
                TreePhotos.Nodes.Add(node);										//-- Add the АвоТипDataRow into the tree
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        #endregion//-- InsertAlbum()  ---------------------

        #region//-- InsertPhoto()  ---------------------
        /// <summary>This method will insert a photo into the tree under the specified album</summary>
        /// <param name="nAlbum">Tree index of album</param>
        /// <param name="strName">Name of photo</param>
        /// <param name="nPhoto">DB index of photo</param>
        private void InsertPhoto(int @Фотоальбом_Index, string @Фото_Описание, int @Фото_ID)
        {
            try
            {
                TreeNode node   = new TreeNode(@Фото_Описание);				//-- Create a new treenode
                node.Tag        = new TreeItem(NodeType.Photo, @Фото_ID); ;	//-- Create new treeitem to store the info about this photo
                node.ImageIndex = 7;
                TreePhotos.Nodes[@Фотоальбом_Index].Nodes.Add(node);		//-- Add the АвоТипDataRow to the tree under the specified album
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        #endregion//-- InsertPhoto()  ---------------------

        #region//-- UpdateZoom()  ---  Update zoom factor ------------------
        private void UpdateZoom()
        {
        }
        #endregion//-- UpdateZoom()  ---  Update zoom factor ------------------

        #region//-- InsertImage()  ---  Update zoom factor ------------------
        /// <summary>This method will insert a phot into the database under the designated album</summary>
        /// <param name="buffer">The images bytes</param>
        /// <param name="strName">Name of the photo</param>
        /// <param name="nAlbum">DB index of album</param>
        private void InsertImage(ref byte[] buffer, string strName, int Фотоальбом_ID_)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("ГАЗФото_INSERT", SqlConn_);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                param.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add("@Фото_Описание", SqlDbType.VarChar).Value = strName;
                cmd.Parameters.Add("@Фото", SqlDbType.Image).Value = buffer;
                cmd.Parameters.Add("@Фотоальбом_ID", SqlDbType.Int).Value = Фотоальбом_ID_;

                cmd.ExecuteNonQuery();

                int nID = (int)cmd.Parameters["RETURN_VALUE"].Value;
                TreeNode node = new TreeNode(strName);
                node.Tag = new TreeItem(NodeType.Photo, nID); ;

                // Get the index of the album we are adding to
                // and insert the new photo АвоТипDataRow
                nID = TreePhotos.SelectedNode.Index;
                TreePhotos.Nodes[nID].Nodes.Add(node);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        #endregion//-- InsertImage()  ---  Update zoom factor ------------------

        #region== Main menu handlers ===========================================
        private void barButtonItem_AddAlbum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            NewAlbum();
        }

        #region//-- OnNewAlbum() -- New album menu handler -----------------
        private void OnNewAlbum(object sender, System.EventArgs e)
        {
            NewAlbum();
        }
        #endregion//-- OnNewAlbum() -- New album menu handler -----------------

        #region//-- NewAlbum() -------------------
        private void NewAlbum()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("ГАЗФотоАльбомы_INSERT", SqlConn_);				//-- Create a stored procedure command
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);	//-- Add the return value parameter
                param.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add("@Фотоальбом_Имя", SqlDbType.VarChar).Value = "Новый фотоальбом " + DateTime.Now.Date;			//-- Add the name parameter and set the value
                cmd.Parameters.Add("@ГАЗТипОбъекта_ID", SqlDbType.Int).Value   = ГАЗТипОбъекта_ID_;
                cmd.Parameters.Add("@ГАЗОбъект_ID", SqlDbType.Int).Value       = ГАЗОбъект_ID_;
                cmd.ExecuteNonQuery();														//-- Execute the command

                int nID = (int)cmd.Parameters["RETURN_VALUE"].Value;						//-- The return value is the index of the newly added album

                // Create a new tree АвоТипDataRow and add it to the treeview
                TreeNode node           = new TreeNode("Новый альбом " + DateTime.Now.Date.ToLongDateString());
                node.Tag                = new TreeItem(NodeType.Album, nID);
                TreePhotos.Nodes.Add(node);
                TreePhotos.SelectedNode = node;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion//-- NewAlbum() -------------------

        #endregion== Main menu handlers ===========================================

        #region== Treeview handlers	 =====================
        #region//-- OnMouseUp() -- Tree control mouse up handler -----------------
        private void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)					//-- We are only interested in right mouse clicks
            {
                TreeNode node = TreePhotos.GetNodeAt(e.X, e.Y);	//-- Attempt to get the АвоТипDataRow the mouse clicked on
                if (node != null)
                {
                    TreePhotos.SelectedNode = node;				//-- Select the tree item
                    TreeItem item = (TreeItem)node.Tag;
                    //-- Check what type of АвоТипDataRow was clicked and display the context menu for that type
                    if (NodeType.Album == item.Type) mnuAlbumPopup.Show(TreePhotos, new Point(e.X, e.Y));
                    else if (NodeType.Photo == item.Type) mnuPhotoPopup.Show(TreePhotos, new Point(e.X, e.Y));
                }
            }
        }
        #endregion//-- OnMouseUp() -------------------

        #region//-- OnAfterSelect() -- Tree control select handler -----------------
        private void OnAfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            try
            {
                Cursor curs_       = this.Cursor;
                this.Cursor        = Cursors.WaitCursor;
                TreeItem item      = (TreeItem)e.Node.Tag;		//-- Retrieve the item info for this АвоТипDataRow
                if (NodeType.Album == item.Type)				//-- If the selected item is an album...
                {
                    pictureBox.Picture = null;					//-- Clear the image
                    this.Cursor = curs_;
                    return;
                }
                ShowPhoto(item);								//-- ...otherwise it is a photo
                TreePhotos.Select();
                this.Cursor = curs_;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion//-- OnAfterSelect() -------------------

        #region//-- OnAfterLabelEdit() -------------------
        private void OnAfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            if (e.Label == null) { e.CancelEdit = true; return; }
            if (e.Label.Length == 0) { e.CancelEdit = true; return; }

            try
            {
                TreeItem item = (TreeItem)TreePhotos.SelectedNode.Tag;	//-- Retrieve item from the selected АвоТипDataRow

                string strCmd;										   //-- Check what type has been edited and create a command string to update name
                if (NodeType.Album == item.Type)
                    strCmd = String.Format("UPDATE ГАЗФотоальбомы SET Фотоальбом_Имя = '{0}' WHERE Фотоальбом_ID = {1}", e.Label, item.Id);
                else
                    strCmd = String.Format("UPDATEГАЗФото SET Фото_Описание = '{0}' WHERE Фото_ID = {1}", e.Label, item.Id);

                SqlCommand cmd = new SqlCommand(strCmd, SqlConn_);  //-- Create a SqlCommand and execute it
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion//-- OnAfterLabelEdit() -------------------

        #region//-- OnKeyUp() -------------------
        /// <summary>Treeview key up handler</summary>
        private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Handle the usual rename key
            if (Keys.F2 == e.KeyCode) TreePhotos.SelectedNode.BeginEdit();
        }
        #endregion//-- OnKeyUp() -------------------

        #endregion== Treeview handlers	 =====================

        #region== Context menu handlers	 =====================

        #region//-- OnRename() -- Album rename menu handler -----------------
        private void OnRename(object sender, System.EventArgs e)
        {
            TreePhotos.SelectedNode.BeginEdit();	   //-- Not much to do, just let the АвоТипDataRow handle itself
        }
        #endregion//-- OnRename() -- Album rename menu handler -----------------

        #region//-- OnAddPhoto() -- Add Photo menu item handler -----------------
        private void OnAddPhoto(object sender, System.EventArgs e)
        {
            AddPhoto();
        }
        #endregion//-- OnAddPhoto() -- Add Photo menu item handler -----------------

        #region//-- barButtonItem_AddPhoto_ItemClick() -- Add Photo barButtonItem handler -----------------
        private void barButtonItem_AddPhoto_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddPhoto();
        }
        #endregion//-- barButtonItem_AddPhoto_ItemClick() -- Add Photo barButtonItem handler -----------------

        #region//-- AddPhoto() -- Add Photo -----------------
        private void AddPhoto()
        {
            if (DialogResult.OK == FileOpenDlg.ShowDialog())			  // Show the file open dialog
            {
                if (TreePhotos.Nodes.Count == 0)
                {
                 MessageBox.Show(this, "Заведите хотя-бы один альбом!", "Альбом", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                 return;
                }             
                TreeItem item = (TreeItem)TreePhotos.SelectedNode.Tag;	 // Retrieve the treeitem for the selected parent АвоТипDataRow
                if (item.Type == NodeType.Photo)
                {
                    TreePhotos.SelectedNode = TreePhotos.SelectedNode.Parent;
                    item = (TreeItem)TreePhotos.SelectedNode.Tag;
                }

                foreach (string file in FileOpenDlg.FileNames)			 // We allow multiple selections so loop through each one
                {
                    // Create a new stream to load this photo into
                    System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    byte[] buffer = new byte[stream.Length];	// Create a buffer to hold the stream bytes
                    stream.Read(buffer, 0, (int)stream.Length);	// Read the bytes from this stream
                    stream.Close();								// Now we can close the stream

                    string strName = System.IO.Path.GetFileNameWithoutExtension(file);	// Extract out the name of the file an use it for the name of the photo
                    InsertImage(ref buffer, strName, item.Id);							// Insert the image into the database and add it to the tree
                    buffer = null;
                }
                TreePhotos.SelectedNode = TreePhotos.SelectedNode.LastNode;				// Select the last child АвоТипDataRow under this album
            }
        }
        #endregion//-- AddPhoto() -- Add Photo  -----------------

        #region//-- OnDelete() -- Delete menu handler -----------------
        private void OnDelete(object sender, System.EventArgs e)
        {
            DeleteAlbumOrPhoto();
        }
        #endregion//-- OnDelete() -- Delete menu handler -----------------

        #region//-- DeleteAlbumOrPhoto() -------------------
        private void DeleteAlbumOrPhoto()
        {
            try
            {
                TreeItem item = (TreeItem)TreePhotos.SelectedNode.Tag;		//-- Retrieve the item from the selected АвоТипDataRow
                int nIndex = TreePhotos.SelectedNode.Index;				//-- Get the index of the selected АвоТипDataRow

                if (MessageBox.Show("Удалить " + TreePhotos.SelectedNode.Text + " ?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;


                // Check the type of АвоТипDataRow selected and create a command sting
                string strCmd;
                if (NodeType.Album == item.Type)
                {
                    if (TreePhotos.SelectedNode.Nodes.Count == 0)
                        strCmd = String.Format("DELETE FROM ГАЗФотоальбомы WHERE Фотоальбом_ID = {0}", item.Id);
                    else
                    {
                        MessageBox.Show("В альбоме есть фото. Можно удалить только пустой альбом");
                        return;
                    }
                }
                else strCmd = String.Format("DELETE FROM ГАЗФото WHERE Фото_ID = {0}", item.Id);

                SqlCommand cmd = new SqlCommand(strCmd, SqlConn_);
                cmd.ExecuteNonQuery();

                TreeNode node = null;
                switch (item.Type)		   												//-- Check the type of item that was deleted
                {
                    case NodeType.Photo:
                        node = TreePhotos.SelectedNode.NextNode;		//-- Get the next АвоТипDataRow or the previous АвоТипDataRow if there is none
                        if (null == node) node = TreePhotos.SelectedNode.PrevNode;
                        if (null == node) node = TreePhotos.SelectedNode.Parent;		//-- Check if there was a previous АвоТипDataRow, if not we need to get the parent
                        TreePhotos.Nodes[TreePhotos.SelectedNode.Parent.Index].Nodes.RemoveAt(nIndex);	//-- Remove the selected АвоТипDataRow from the tree
                        break;

                    case NodeType.Album:
                        node = TreePhotos.SelectedNode.NextNode;
                        if (null == node) node = TreePhotos.SelectedNode.PrevNode;
                        pictureBox.Picture = null;					//-- This was an ablum АвоТипDataRow so remove any photos displayed
                        TreePhotos.Nodes.RemoveAt(nIndex);			//-- Remove the selected АвоТипDataRow from the tree
                        break;
                }
                if (null != node) TreePhotos.SelectedNode = node;	//-- Now select the АвоТипDataRow

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion//-- DeleteAlbumOrPhoto() -----------------

        #endregion== Context menu handlers	 =====================

        #region//-- ShowPhoto()  ---------------------
        private void ShowPhoto(TreeItem item)
        {
            try
            {
                string strCmd = String.Format("SELECT Фото FROM ГАЗФото WHERE Фото_ID = {0}", item.Id);
                SqlCommand cmd = new SqlCommand(strCmd, SqlConn_);

                byte[] b = (byte[])cmd.ExecuteScalar();
                if (b.Length > 0)
                {
                    //-- Open a stream for the image and write the bytes into it
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(b, true);
                    stream.Write(b, 0, b.Length);
                    try
                    {
                        this.pictureBox.Picture = Image.FromStream(stream);
                        this.ActiveControl = this.pictureBox.PictureBox;
                    }
                    catch (Exception ex) { MessageBox.Show(this, ex.Message); }

                    stream.Close();						// Close the stream and delete the temp file
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion//-- ShowPhoto()  ---------------------

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLPhotosCtrl));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            this.TreePhotos = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.mnuAlbumPopup = new System.Windows.Forms.ContextMenu();
            this.mnuAddPhoto = new System.Windows.Forms.MenuItem();
            this.mnuAlbumEdit = new System.Windows.Forms.MenuItem();
            this.mnuRenameAlbum = new System.Windows.Forms.MenuItem();
            this.mnuDeleteAlbum = new System.Windows.Forms.MenuItem();
            this.mnuImage = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.FileOpenDlg = new System.Windows.Forms.OpenFileDialog();
            this.mnuPhotoPopup = new System.Windows.Forms.ContextMenu();
            this.mnuRenamePhoto = new System.Windows.Forms.MenuItem();
            this.mnuPhotoEdit = new System.Windows.Forms.MenuItem();
            this.mnuDeletePhoto = new System.Windows.Forms.MenuItem();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.pictureBox = new QAlbum.ScalablePictureBox();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem_AddAlbum = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_EditDescription = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_DelAlbumPhoto = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_PrintPhoto = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_AddPhoto = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemDateEdit_barEditItem_DateДУ = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_barEditItem_DateДУ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_barEditItem_DateДУ.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreePhotos
            // 
            this.TreePhotos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TreePhotos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreePhotos.ImageIndex = 3;
            this.TreePhotos.ImageList = this.imageList1;
            this.TreePhotos.LabelEdit = true;
            this.TreePhotos.Location = new System.Drawing.Point(0, 0);
            this.TreePhotos.Margin = new System.Windows.Forms.Padding(0);
            this.TreePhotos.Name = "TreePhotos";
            this.TreePhotos.SelectedImageIndex = 5;
            this.TreePhotos.Size = new System.Drawing.Size(154, 497);
            this.TreePhotos.TabIndex = 0;
            this.TreePhotos.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreePhotos_BeforeLabelEdit);
            this.TreePhotos.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnAfterLabelEdit);
            this.TreePhotos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            this.TreePhotos.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.TreePhotos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "add.ico");
            this.imageList1.Images.SetKeyName(1, "Удалить.ico");
            this.imageList1.Images.SetKeyName(2, "Справочники_закрыто.ico");
            this.imageList1.Images.SetKeyName(3, "Справочники.ico");
            this.imageList1.Images.SetKeyName(4, "Принтер.ico");
            this.imageList1.Images.SetKeyName(5, "edit.ico");
            this.imageList1.Images.SetKeyName(6, "export.ico");
            this.imageList1.Images.SetKeyName(7, "Node_0.ico");
            // 
            // mnuAlbumPopup
            // 
            this.mnuAlbumPopup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.mnuAddPhoto,
			this.mnuAlbumEdit,
			this.mnuRenameAlbum,
			this.mnuDeleteAlbum});
            // 
            // mnuAddPhoto
            // 
            this.mnuAddPhoto.Index = 0;
            this.mnuAddPhoto.Text = "Add Photo";
            this.mnuAddPhoto.Click += new System.EventHandler(this.OnAddPhoto);
            // 
            // mnuAlbumEdit
            // 
            this.mnuAlbumEdit.Index = 1;
            this.mnuAlbumEdit.Text = "Edit Description";
            // 
            // mnuRenameAlbum
            // 
            this.mnuRenameAlbum.Index = 2;
            this.mnuRenameAlbum.Text = "Rename";
            this.mnuRenameAlbum.Click += new System.EventHandler(this.OnRename);
            // 
            // mnuDeleteAlbum
            // 
            this.mnuDeleteAlbum.Index = 3;
            this.mnuDeleteAlbum.Text = "Delete";
            this.mnuDeleteAlbum.Click += new System.EventHandler(this.OnDelete);
            // 
            // mnuImage
            // 
            this.mnuImage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem1,
			this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Rotate Left 90";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Rotate Right 90";
            // 
            // FileOpenDlg
            // 
            this.FileOpenDlg.Filter = "Gif files|*.gif|Jpeg Files|*.jpg;*.jpeg|Bmp Files|*.bmp";
            this.FileOpenDlg.Multiselect = true;
            this.FileOpenDlg.Title = "Select Photos";
            // 
            // mnuPhotoPopup
            // 
            this.mnuPhotoPopup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.mnuRenamePhoto,
			this.mnuPhotoEdit,
			this.mnuDeletePhoto});
            // 
            // mnuRenamePhoto
            // 
            this.mnuRenamePhoto.Index = 0;
            this.mnuRenamePhoto.Text = "Rename";
            this.mnuRenamePhoto.Click += new System.EventHandler(this.OnRename);
            // 
            // mnuPhotoEdit
            // 
            this.mnuPhotoEdit.Index = 1;
            this.mnuPhotoEdit.Text = "Edit Description";
            // 
            // mnuDeletePhoto
            // 
            this.mnuDeletePhoto.Index = 2;
            this.mnuDeletePhoto.Text = "Delete";
            this.mnuDeletePhoto.Click += new System.EventHandler(this.OnDelete);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(625, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 10);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Visible = false;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.ContextMenu = this.mnuImage;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(314, 497);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
			this.bar2,
			this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageList1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
			this.barButtonItem_AddAlbum,
			this.barButtonItem_EditDescription,
			this.barButtonItem_DelAlbumPhoto,
			this.barButtonItem_PrintPhoto,
			this.barButtonItem_AddPhoto});
            this.barManager1.MaxItemId = 9;
            this.barManager1.MdiMenuMergeStyle = DevExpress.XtraBars.BarMdiMenuMergeStyle.Never;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
			this.repositoryItemDateEdit_barEditItem_DateДУ});
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
			new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_AddAlbum),
			new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_EditDescription),
			new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_DelAlbumPhoto),
			new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_PrintPhoto),
			new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_AddPhoto)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DisableClose = true;
            this.bar2.OptionsBar.DisableCustomization = true;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barButtonItem_AddAlbum
            // 
            this.barButtonItem_AddAlbum.Caption = "Добавить альбом";
            this.barButtonItem_AddAlbum.Id = 1;
            this.barButtonItem_AddAlbum.ImageIndex = 0;
            this.barButtonItem_AddAlbum.Name = "barButtonItem_AddAlbum";
            toolTipTitleItem1.Text = "Добавить альбом";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.barButtonItem_AddAlbum.SuperTip = superToolTip1;
            this.barButtonItem_AddAlbum.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_AddAlbum_ItemClick);
            // 
            // barButtonItem_EditDescription
            // 
            this.barButtonItem_EditDescription.Caption = "Редактировать название";
            this.barButtonItem_EditDescription.Id = 2;
            this.barButtonItem_EditDescription.ImageIndex = 5;
            this.barButtonItem_EditDescription.Name = "barButtonItem_EditDescription";
            toolTipTitleItem2.Text = "Редактировать название";
            superToolTip2.Items.Add(toolTipTitleItem2);
            this.barButtonItem_EditDescription.SuperTip = superToolTip2;
            this.barButtonItem_EditDescription.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_EditDescription_ItemClick);
            // 
            // barButtonItem_DelAlbumPhoto
            // 
            this.barButtonItem_DelAlbumPhoto.Caption = "Удалить";
            this.barButtonItem_DelAlbumPhoto.Id = 3;
            this.barButtonItem_DelAlbumPhoto.ImageIndex = 1;
            this.barButtonItem_DelAlbumPhoto.Name = "barButtonItem_DelAlbumPhoto";
            toolTipTitleItem3.Text = "Удалить альбом/фото";
            superToolTip3.Items.Add(toolTipTitleItem3);
            this.barButtonItem_DelAlbumPhoto.SuperTip = superToolTip3;
            this.barButtonItem_DelAlbumPhoto.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_DelAlbumPhoto_ItemClick);
            // 
            // barButtonItem_PrintPhoto
            // 
            this.barButtonItem_PrintPhoto.Caption = "Печать фото";
            this.barButtonItem_PrintPhoto.Id = 4;
            this.barButtonItem_PrintPhoto.ImageIndex = 4;
            this.barButtonItem_PrintPhoto.Name = "barButtonItem_PrintPhoto";
            toolTipTitleItem4.Text = "Печать фото";
            superToolTip4.Items.Add(toolTipTitleItem4);
            this.barButtonItem_PrintPhoto.SuperTip = superToolTip4;
            // 
            // barButtonItem_AddPhoto
            // 
            this.barButtonItem_AddPhoto.Caption = "Загрузить файл фото";
            this.barButtonItem_AddPhoto.Id = 5;
            this.barButtonItem_AddPhoto.ImageIndex = 6;
            this.barButtonItem_AddPhoto.Name = "barButtonItem_AddPhoto";
            toolTipTitleItem5.Text = "Загрузить фото из файла";
            superToolTip5.Items.Add(toolTipTitleItem5);
            this.barButtonItem_AddPhoto.SuperTip = superToolTip5;
            this.barButtonItem_AddPhoto.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_AddPhoto_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            this.bar3.Visible = false;
            // 
            // repositoryItemDateEdit_barEditItem_DateДУ
            // 
            this.repositoryItemDateEdit_barEditItem_DateДУ.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.repositoryItemDateEdit_barEditItem_DateДУ.Appearance.ForeColor = System.Drawing.Color.DeepPink;
            this.repositoryItemDateEdit_barEditItem_DateДУ.Appearance.Options.UseFont = true;
            this.repositoryItemDateEdit_barEditItem_DateДУ.Appearance.Options.UseForeColor = true;
            this.repositoryItemDateEdit_barEditItem_DateДУ.AutoHeight = false;
            this.repositoryItemDateEdit_barEditItem_DateДУ.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_barEditItem_DateДУ.DisplayFormat.FormatString = "D";
            this.repositoryItemDateEdit_barEditItem_DateДУ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit_barEditItem_DateДУ.Name = "repositoryItemDateEdit_barEditItem_DateДУ";
            this.repositoryItemDateEdit_barEditItem_DateДУ.VistaEditTime = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemDateEdit_barEditItem_DateДУ.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
			new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 26);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.TreePhotos);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(474, 497);
            this.splitContainerControl1.SplitterPosition = 154;
            this.splitContainerControl1.TabIndex = 8;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // SQLPhotosCtrl
            // 
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "CtrlSQLPhotos";
            this.Size = new System.Drawing.Size(474, 545);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_barEditItem_DateДУ.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_barEditItem_DateДУ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region//-- TreePhotos_BeforeLabelEdit() -------------------------=
        private void TreePhotos_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            //e.Label = e.Node.Text;
        }
        #endregion//-- TreePhotos_BeforeLabelEdit() -------------------------

        private void barButtonItem_EditDescription_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreePhotos.SelectedNode.BeginEdit();
        }

        private void barButtonItem_DelAlbumPhoto_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteAlbumOrPhoto();
        }
    }
}
