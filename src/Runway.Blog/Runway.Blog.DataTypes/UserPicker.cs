using System;
using System.Web.UI.WebControls;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
namespace Runway.Blog.DataTypes
{
	public class UserPicker : AbstractDataEditor
	{
		private System.Web.UI.WebControls.DropDownList m_control = new System.Web.UI.WebControls.DropDownList();
		public override System.Guid Id
		{
			get
			{
				return new System.Guid("e66af4a0-e8b4-11de-8a39-0800200c9a66");
			}
		}
		public override string DataTypeName
		{
			get
			{
				return "Userpicker";
			}
		}
		public UserPicker()
		{
			base.RenderControl = this.m_control;
			this.m_control.Init += new System.EventHandler(this.m_control_Init);
			base.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);
		}
		private void m_control_Init(object sender, System.EventArgs e)
		{
			this.m_control.Items.Clear();
			User[] all = User.getAll();
			for (int i = 0; i < all.Length; i++)
			{
				User user = all[i];
				this.m_control.Items.Add(new System.Web.UI.WebControls.ListItem(user.Name, user.Id.ToString()));
			}
			this.m_control.SelectedValue = ((base.Data.Value != null) ? base.Data.Value.ToString() : "");
		}
		private void DataEditorControl_OnSave(System.EventArgs e)
		{
			base.Data.Value = this.m_control.SelectedValue;
		}
	}
}
