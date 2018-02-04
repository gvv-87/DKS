using DevExpress.XtraEditors;

namespace ДКС
{
    public class MyCalcEdit : CalcEdit
	{
		public MyCalcEdit()
		{
			//Properties.Mask.EditMask = @"\d+m?";
			//Properties.Mask.MaskType = MaskType.RegEx;
		}

		protected override DevExpress.XtraEditors.Popup.PopupBaseForm CreatePopupForm()
		{
			return new MyPopupCalcEditForm(this);
		}
	}

	public class MyPopupCalcEditForm : DevExpress.XtraEditors.Popup.PopupCalcEditForm
	{
		public MyPopupCalcEditForm(PopupBaseEdit edit) : base(edit)
		{
			this.fShowOkButton		= true;
		}


	}

}
