using System;

namespace ДКС
{
    partial class W_Main
    {

        public void OutputString(string s_)
        {
            this.TextBox2.Text += DateTime.Now.ToString() + s_ +"\r\n";
            this.TextBox2.Refresh();
            this.dockPanel_Output.ShowSliding();
        }

    }
}