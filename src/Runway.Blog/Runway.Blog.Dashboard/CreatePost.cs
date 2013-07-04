using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;
using umbraco.BasePages;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.propertytype;
using umbraco.cms.businesslogic.web;
using umbraco.DataLayer;
using umbraco.DataLayer.SqlHelpers.SqlServer;
using umbraco.editorControls.tinyMCE3;
using umbraco.interfaces;
using umbraco.uicontrols;
namespace Runway.Blog.Dashboard
{
	public class CreatePost : System.Web.UI.UserControl
	{
		private System.Collections.ArrayList _dataFields = new System.Collections.ArrayList();
		private int _parentId = -1;
		private TinyMCE _tinymce = null;
		protected System.Web.UI.WebControls.Panel blogpostCreator;
		protected Pane Pane1;
		protected System.Web.UI.WebControls.TextBox title;
		protected System.Web.UI.WebControls.PlaceHolder blogpostControls;
		protected System.Web.UI.WebControls.CheckBox publish;
		protected System.Web.UI.WebControls.Button createPost;
		protected System.Web.UI.WebControls.Panel blogpostCreatorNoBlog;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this._tinymce != null)
			{
				TabView dashboardPanel = this.GetDashboardPanel();
				if (this._tinymce.config["umbraco_toolbar_id"] != null)
				{
					this._tinymce.config.Remove("umbraco_toolbar_id");
				}
				this._tinymce.config.Add("umbraco_toolbar_id", dashboardPanel.ClientID + "_tab01layer_menu_sl");
			}
		}
		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);
			DocumentType blogPostDocumentType = this.GetBlogPostDocumentType();
			this.SetBlogRoot();
			if (this._parentId <= 0)
			{
				this.blogpostCreatorNoBlog.Visible = true;
				this.blogpostCreator.Visible = false;
			}
			else
			{
				ContentType.TabI tabI = blogPostDocumentType.getVirtualTabs[0];
				PropertyType[] propertyTypes = tabI.PropertyTypes;
				for (int i = 0; i < propertyTypes.Length; i++)
				{
					PropertyType propertyType = propertyTypes[i];
					IDataType dataType = propertyType.DataTypeDefinition.DataType;
					if (dataType.Id == new System.Guid("{5E9B75AE-FACE-41c8-B47E-5F4B0FD82F83}"))
					{
						this._tinymce = (TinyMCE)dataType.DataEditor;
					}
					dataType.DataEditor.Editor.ID = propertyType.Alias;
					this._dataFields.Add(dataType);
					IData data = propertyType.DataTypeDefinition.DataType.Data;
					this.blogpostControls.Controls.Add(new System.Web.UI.LiteralControl(string.Concat(new string[]
					{
						"<p><label for=\"", 
						dataType.DataEditor.Editor.ClientID, 
						"\"><strong>", 
						propertyType.Name, 
						"</strong></label>"
					})));
					this.blogpostControls.Controls.Add(dataType.DataEditor.Editor);
					this.blogpostControls.Controls.Add(new System.Web.UI.LiteralControl("</p>"));
				}
			}
		}
		private TabView GetDashboardPanel()
		{
			return (TabView)CreatePost.FindControlRecursive(this.Page.Master, "dashboardTabs");
		}
		public static System.Web.UI.Control FindControlRecursive(System.Web.UI.Control Root, string Id)
		{
			System.Web.UI.Control result;
			if (Root.ID == Id)
			{
				result = Root;
			}
			else
			{
				foreach (System.Web.UI.Control root in Root.Controls)
				{
					System.Web.UI.Control control = CreatePost.FindControlRecursive(root, Id);
					if (control != null)
					{
						result = control;
						return result;
					}
				}
				result = null;
			}
			return result;
		}
		private void SetBlogRoot()
		{
			if (this._parentId == -1)
			{
				string text = "select{0}id, path from umbracoNode inner join cmsContent on cmsContent.nodeId = umbracoNode.id inner join cmsContentType on cmsContentType.nodeId = cmsContent.contentType where cmsContentType.alias = '{1}' and path not like '%-20%' and path like '%{2}%' order by path{3}";
				string text2 = (UmbracoEnsuredPage.CurrentUser.StartNodeId != -1) ? ("," + UmbracoEnsuredPage.CurrentUser.StartNodeId + ",") : (UmbracoEnsuredPage.CurrentUser.StartNodeId + ",");
				if (umbraco.BusinessLogic.Application.SqlHelper.GetType() == typeof(SqlServerHelper))
				{
					text = string.Format(text, new object[]
					{
						" TOP 1 ", 
						this.GetBlogDocumentType().Alias, 
						text2, 
						""
					});
				}
				else
				{
					text = string.Format(text, new object[]
					{
						" ", 
						this.GetBlogDocumentType().Alias, 
						text2, 
						" LIMIT 0,1"
					});
				}
				this._parentId = umbraco.BusinessLogic.Application.SqlHelper.ExecuteScalar<int>(text, new IParameter[0]);
			}
		}
		private DocumentType GetBlogPostDocumentType()
		{
			return DocumentType.GetByAlias("umbBlogPost");
		}
		private DocumentType GetBlogDocumentType()
		{
			return DocumentType.GetByAlias("umbBlog");
		}
		protected void createPost_Click(object sender, System.EventArgs e)
		{
			this.SetBlogRoot();
			Document document = Document.MakeNew(this.title.Text, this.GetBlogPostDocumentType(), UmbracoEnsuredPage.CurrentUser, this._parentId);
			foreach (IDataType dataType in this._dataFields)
			{
				DefaultData defaultData = (DefaultData)dataType.Data;
				dataType.Data.PropertyId = defaultData.NodeId;
				dataType.DataEditor.Save();
			}
			if (this.publish.Checked)
			{
				document.Publish(UmbracoEnsuredPage.CurrentUser);
				library.UpdateDocumentCache(document.Id);
			}
			base.Response.Redirect("editContent.aspx?id=" + document.Id, true);
		}
		private void SetRequestId(string id)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = base.Request.QueryString;
			nameValueCollection = (System.Collections.Specialized.NameValueCollection)base.Request.GetType().GetField("_queryString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(base.Request);
			System.Reflection.PropertyInfo property = nameValueCollection.GetType().GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			property.SetValue(nameValueCollection, false, null);
			nameValueCollection["id"] = id;
			property.SetValue(nameValueCollection, true, null);
		}
	}
}
