using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.DataLayer;
using umbraco.interfaces;
namespace Runway.Blog.DataTypes
{
	public class SkinBrowserConfig : System.Web.UI.WebControls.Panel, IDataPrevalue
	{
		private BaseDataType m_datatype;
		private System.Web.UI.WebControls.DropDownList m_dbType;
		private System.Web.UI.WebControls.TextBox m_packageId;
		public System.Web.UI.Control Editor
		{
			get
			{
				return this;
			}
		}
		public static ISqlHelper SqlHelper
		{
			get
			{
				return Application.SqlHelper;
			}
		}
		public string Configuration
		{
			get
			{
				string result;
				try
				{
					result = SkinBrowserConfig.SqlHelper.ExecuteScalar<string>("select value from cmsDataTypePreValues where datatypenodeid = @datatypenodeid", new IParameter[]
					{
						SkinBrowserConfig.SqlHelper.CreateParameter("@datatypenodeid", this.m_datatype.DataTypeDefinitionId)
					});
				}
				catch
				{
					result = "";
				}
				return result;
			}
		}
		public void Save()
		{
			this.m_datatype.DBType = (DBTypes)System.Enum.Parse(typeof(DBTypes), this.m_dbType.SelectedValue, true);
			IParameter[] parameters = new IParameter[]
			{
				SkinBrowserConfig.SqlHelper.CreateParameter("@value", this.m_packageId.Text), 
				SkinBrowserConfig.SqlHelper.CreateParameter("@dtdefid", this.m_datatype.DataTypeDefinitionId)
			};
			SkinBrowserConfig.SqlHelper.ExecuteNonQuery("delete from cmsDataTypePreValues where datatypenodeid = @dtdefid", parameters);
			SkinBrowserConfig.SqlHelper.ExecuteNonQuery("insert into cmsDataTypePreValues (datatypenodeid,[value],sortorder,alias) values (@dtdefid,@value,0,'')", parameters);
		}
		public SkinBrowserConfig(BaseDataType dataType)
		{
			this.m_datatype = dataType;
			this.setupChildControls();
		}
		public void setupChildControls()
		{
			this.m_dbType = new System.Web.UI.WebControls.DropDownList();
			this.m_dbType.ID = "dbtype";
			this.m_dbType.Items.Add(DBTypes.Date.ToString());
			this.m_dbType.Items.Add(DBTypes.Integer.ToString());
			this.m_dbType.Items.Add(DBTypes.Ntext.ToString());
			this.m_dbType.Items.Add(DBTypes.Nvarchar.ToString());
			this.m_packageId = new System.Web.UI.WebControls.TextBox();
			this.m_packageId.ID = "packageId";
			this.Controls.Add(this.m_dbType);
			this.Controls.Add(this.m_packageId);
		}
		protected override void OnLoad(System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				string[] array = this.Configuration.Split("|".ToCharArray());
				if (array.Length > 0)
				{
					this.m_packageId.Text = array[0];
				}
				this.m_dbType.SelectedValue = this.m_datatype.DBType.ToString();
			}
			base.OnLoad(e);
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.WriteLine("<table>");
			writer.WriteLine("<tr><th>" + ui.Text("editdatatype", "dataBaseDatatype") + ":</th><td>");
			this.m_dbType.RenderControl(writer);
			writer.Write("</td></tr>");
			writer.Write("<tr><th>Skin package identifier (name):</th><td>");
			this.m_packageId.RenderControl(writer);
			writer.Write("</td></tr>");
			writer.Write("</table>");
		}
	}
}
