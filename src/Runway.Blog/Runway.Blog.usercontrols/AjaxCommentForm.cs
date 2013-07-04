using Runway.Blog.Library;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.presentation.nodeFactory;
namespace Runway.Blog.usercontrols
{
	public class AjaxCommentForm : System.Web.UI.UserControl
	{
		private string _CommentsClosedMessage;
		protected System.Web.UI.WebControls.PlaceHolder ph_closed;
		protected System.Web.UI.WebControls.PlaceHolder ph_form;
		public string CommentsClosedMessage
		{
			get
			{
				return this._CommentsClosedMessage;
			}
			set
			{
				this._CommentsClosedMessage = value;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool flag = false;
			try
			{
				if (Node.GetCurrent().GetProperty("closeComments").Value == "1")
				{
					flag = true;
				}
			}
			catch (System.NullReferenceException)
			{
			}
			catch (System.ArgumentException)
			{
			}
			this.ph_closed.Visible = flag;
			this.ph_form.Visible = !flag;
			if (this.Page.IsPostBack)
			{
				Base.CreateComment(Node.GetCurrent().Id);
			}
		}
	}
}
