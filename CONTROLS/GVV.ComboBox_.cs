using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GVV
{
    /// <summary>Summary description for BetterComboBox.</summary>
    public class ComboBox_ : ComboBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int SWP_NOSIZE = 0x1;
        private const UInt32 WM_CTLCOLORLISTBOX = 0x0134;

        //Store the default width to perform check in UpdateDropDownWidth
        private int initialDropDownWidth = 0;

        public ComboBox_()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            initialDropDownWidth = this.DropDownWidth;
            this.HandleCreated += new EventHandler(BetterComboBox_HandleCreated);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        private void UpdateDropDownWidth()
        {
            System.Drawing.Graphics ds = this.CreateGraphics();                                         //Create a GDI+ drawing surface to measure string widths
            float maxWidth = 0;                                                                         //Float to hold largest single item width

            
            foreach (object item in this.Items)                                                         //Iterate over each item, measuring the maximum width	 of the DisplayMember strings
            {
                maxWidth = Math.Max(maxWidth, ds.MeasureString(item.ToString(), this.Font).Width);
            }
            maxWidth += 30;                                                                             //Add a buffer for some white space	 around the text
            int newWidth = (int)Decimal.Round((decimal)maxWidth, 0);                                    //round maxWidth and cast to an int
            
            if (newWidth > Screen.GetWorkingArea(this).Width)                                           //If the width is bigger than the screen, ensure	we stay within the bounds of the screen
            {
                newWidth = Screen.GetWorkingArea(this).Width;
            }
                                                                                    
            if (newWidth > initialDropDownWidth)                                                        //Only change the default width if it's smaller	than the newly calculated width
            {
                this.DropDownWidth = newWidth;
            }
            ds.Dispose();                                    //Clean up the drawing surface
        }

        #region//-- WndProc() -------------------- 
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CTLCOLORLISTBOX)
            {
                int left = this.PointToScreen(new Point(0, 0)).X;            //-- Make sure we are inbounds of the screen 

                //--  Only do this if the dropdown is going off right edge of screen ---
                if (this.DropDownWidth > Screen.PrimaryScreen.WorkingArea.Width - left)
                {
                    Rectangle comboRect = this.RectangleToScreen(this.ClientRectangle); //-- Get the current combo position and size --

                    int dropHeight     = 0;
                    int topOfDropDown  = 0;
                    int leftOfDropDown = 0;


                    for (int i = 0; (i < this.Items.Count && i < this.MaxDropDownItems); i++)   //-- Calculate dropped list height ---
                    {
                        dropHeight += this.ItemHeight;
                    }

                    //Set top position of the dropped list if 
                    //it goes off the bottom of the screen
                    if (dropHeight > Screen.PrimaryScreen.WorkingArea.Height -
                        this.PointToScreen(new Point(0, 0)).Y)
                    {
                        topOfDropDown = comboRect.Top - dropHeight - 2;
                    }
                    else
                    {
                        topOfDropDown = comboRect.Bottom;
                    }

                    //Calculate shifted left position

                    leftOfDropDown = comboRect.Left - (this.DropDownWidth -
                        (Screen.PrimaryScreen.WorkingArea.Width - left));

                    // Postioning/sizing the drop-down
                    //SetWindowPos(HWND hWnd,
                    //      HWND hWndInsertAfter,
                    //      int X,
                    //      int Y,
                    //      int cx,
                    //      int cy,
                    //      UINT uFlags);
                    //when using the SWP_NOSIZE flag, cx and cy params are ignored
                    SetWindowPos(m.LParam,
                        IntPtr.Zero,
                        leftOfDropDown,
                        topOfDropDown,
                        0,
                        0,
                        SWP_NOSIZE);
                }
            }

            base.WndProc(ref m);
        }
        #endregion//-- WndProc() -------------------- 

        #region//-- OnKeyPress() -------------------- 
        //-- Джон Коннел "Разработка элементов управления" (стр. 39) --
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            int old_SelectedIndex   = this.SelectedIndex;
            string old_text         = this.Text;
            int old_SelectionStart_ = this.SelectionStart;
            int SelectionStart_     = this.SelectionStart;    //-- Текущее значение курсора в клетке
            int SelectionLength_    = this.SelectionLength;

            if ((char)(e.KeyChar) == (char)Keys.Escape)
            {
                this.SelectedIndex = -1;
                this.Text = "";
            }
            else if ((char)(e.KeyChar) == (char)Keys.Back)
            {
                //================================================  
                //-- стоим в конце строки - удалять нечего ------
                if ((SelectionStart_ == 0) && (SelectionLength_ == 0)) goto Back_;
                    
                //-- выделена часть строки -------------
                if (SelectionLength_ > 0)
                {
                    this.Text = this.Text.Remove(SelectionStart_, SelectionLength_);
                    goto Back_;
                }
                //-- удаляем символ перед курсором ------
                this.Text = this.Text.Remove(SelectionStart_ - 1, 1);
                --SelectionStart_;
                Back_: AutoComplete(this, this.Text, ref SelectionStart_, old_text, old_SelectionStart_, old_SelectedIndex);
            }
            else
            {
                //-- выделена часть строки -------------
                if (SelectionLength_ > 0)
                {
                    this.Text = this.Text.Remove(SelectionStart_, SelectionLength_);
                }
                this.Text = this.Text.Insert(SelectionStart_, e.KeyChar.ToString());
                ++SelectionStart_;
                AutoComplete(this, this.Text, ref SelectionStart_, old_text, old_SelectionStart_, old_SelectedIndex);
            }

            this.SelectionStart = SelectionStart_;
            this.DroppedDown    = true;
            e.Handled           = true;
        }

        #endregion//-- OnKeyPress() -------------------- 

        #region//-- AutoComplete() -------------------- 
        //-- Завершение с поиском подстроки и
        //-- установкой на найденную строку ComboBox
        private void AutoComplete
            (
            GVV.ComboBox_ cb_,
            string s_,
            ref int SelectionStart_,
            string old_text_,
            int old_SelectionStart_,
            int old_SelectedIndex_
            )
        {
            int iIndex = -1;
            if (s_.Length == 0)
            {
                cb_.SelectedIndex = -1;
                cb_.Text = "";
            }
            else
            {
                iIndex = cb_.FindString(s_, -1);
                if (iIndex != -1)
                {
                    cb_.SelectedIndex = iIndex;
                    cb_.SelectionStart = SelectionStart_;
                    cb_.Text = s_;
                }
                else
                {
                    cb_.SelectedIndex = old_SelectedIndex_;
                    cb_.SelectionStart = old_SelectionStart_;
                    SelectionStart_ = old_SelectionStart_;
                    cb_.Text = old_text_;
                }
            }

        }
        #endregion//-- AutoComplete() -------------------- 

        private void BetterComboBox_HandleCreated(object sender, EventArgs e)
        {
            UpdateDropDownWidth();
        }
    }
}
