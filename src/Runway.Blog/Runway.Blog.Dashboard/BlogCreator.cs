using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;
using umbraco.controls;
namespace Runway.Blog.Dashboard
{
	public class BlogCreator : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox tb_name;
		protected System.Web.UI.WebControls.DropDownList dd_author;
		protected System.Web.UI.WebControls.TextBox tb_AuthorName;
		protected System.Web.UI.WebControls.TextBox tb_AuthorEmail;
		protected System.Web.UI.WebControls.PlaceHolder ph_contentPicker;
		public ContentPicker cp = new ContentPicker();
		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);
			this.ph_contentPicker.Controls.Add(this.cp);
			if (!this.Page.IsPostBack)
			{
				this.dd_author.Items.Clear();
				System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem("Create a new blog author account", "create");
				this.dd_author.Items.Add(item);
				User[] all = User.getAll();
				for (int i = 0; i < all.Length; i++)
				{
					User user = all[i];
					this.dd_author.Items.Add(new System.Web.UI.WebControls.ListItem(user.Name, user.Id.ToString()));
				}
			}
		}
		protected void blogCreateClick(object sender, System.EventArgs e)
		{
			DocumentType byAlias = DocumentType.GetByAlias("Blog");
			int parentId = (!string.IsNullOrEmpty(this.cp.Text)) ? int.Parse(this.cp.Text) : -1;
			Document document = Document.MakeNew(this.tb_name.Text, byAlias, User.GetCurrent(), parentId);
			document.getProperty("owner").Value = User.GetCurrent().Id;
			document.getProperty("blogName").Value = this.tb_name.Text;
			document.getProperty("blogDescription").Value = "The blog of " + User.GetCurrent().Name;
			document.Save();
			document.Publish(User.GetCurrent());
			library.UpdateDocumentCache(document.Id);
			base.Response.Redirect("editContent.aspx?id=" + document.Id, true);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
	}
}
