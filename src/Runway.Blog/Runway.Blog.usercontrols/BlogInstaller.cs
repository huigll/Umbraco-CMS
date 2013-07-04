using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;
using umbraco.BasePages;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;
namespace Runway.Blog.usercontrols
{
	public class BlogInstaller : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel done;
		protected System.Web.UI.WebControls.HyperLink blogLink;
		protected System.Web.UI.WebControls.Panel install;
		protected System.Web.UI.WebControls.TextBox tb_name;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox tb_description;
		protected System.Web.UI.WebControls.Button bt_create;
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		protected void saveAndPublish(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				string text = this.tb_name.Text;
				string text2 = this.tb_description.Text;
				User currentUser = UmbracoEnsuredPage.CurrentUser;
				string text3 = "RunwayHomepage";
				string text4 = "/";
				Document[] rootDocuments = Document.GetRootDocuments();
				Document[] array = rootDocuments;
				for (int i = 0; i < array.Length; i++)
				{
					Document document = array[i];
					if (document.Text.Trim() == text3.Trim() && document != null && document.ContentType != null)
					{
						document.getProperty("siteName").Value = text;
						document.getProperty("siteDescription").Value = text2;
						document.PublishWithChildrenWithResult(User.GetUser(0));
						library.UpdateDocumentCache(document.Id);
						library.RefreshContent();
						text4 = library.NiceUrl(document.Id);
						break;
					}
				}
				if (string.IsNullOrEmpty(text4))
				{
					text4 = "/";
				}
				this.install.Visible = false;
				this.done.Visible = true;
				this.blogLink.NavigateUrl = text4;
			}
		}
	}
}
