using Runway.Blog.Spam;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;
using umbraco.cms.businesslogic.web;
using umbraco.DataLayer;
namespace Runway.Blog.Dashboard
{
	public class CommentModeration : System.Web.UI.UserControl
	{
        private ISqlHelper SqlHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN, false);
		protected System.Web.UI.WebControls.LinkButton btnDeleteSelected;
		protected System.Web.UI.WebControls.LinkButton btnApproved;
		protected System.Web.UI.WebControls.LinkButton btnSpam;
		protected System.Web.UI.WebControls.LinkButton btnAll;
		protected System.Web.UI.WebControls.Repeater rptComments;
		protected System.Web.UI.WebControls.Repeater rptPages;

        
		public int CurrentPage
		{
			get
			{
				object obj = this.ViewState["_CurrentPage"];
				int result;
				if (obj == null)
				{
					result = 0;
				}
				else
				{
					result = (int)obj;
				}
				return result;
			}
			set
			{
				this.ViewState["_CurrentPage"] = value;
			}
		}
		public string Filter
		{
			get
			{
				object obj = this.ViewState["Filter"];
				string result;
				if (obj == null)
				{
					result = string.Empty;
				}
				else
				{
					result = (string)obj;
				}
				return result;
			}
			set
			{
				this.ViewState["Filter"] = value;
			}
		}
		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);
			this.rptPages.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptPages_ItemCommand);
		}
		private void rptPages_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			this.CurrentPage = System.Convert.ToInt32(e.CommandArgument) - 1;
			this.BindData();
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.Filter = "where spam != 1";
				this.btnApproved.Enabled = false;
				this.BindData();
			}
		}
		private void BindData()
		{
			IRecordsReader recordsReader = this.SqlHelper.ExecuteReader(string.Format("select * from comment {0} order by created desc", this.Filter), new IParameter[0]);
			DataTable dataTable = new DataTable("Comments");
			dataTable.Columns.Add("id", System.Type.GetType("System.Int32"));
			dataTable.Columns.Add("nodeid", System.Type.GetType("System.Int32"));
			dataTable.Columns.Add("name", System.Type.GetType("System.String"));
			dataTable.Columns.Add("email", System.Type.GetType("System.String"));
			dataTable.Columns.Add("website", System.Type.GetType("System.String"));
			dataTable.Columns.Add("comment", System.Type.GetType("System.String"));
			dataTable.Columns.Add("spam", System.Type.GetType("System.Boolean"));
			dataTable.Columns.Add("ham", System.Type.GetType("System.Boolean"));
			dataTable.Columns.Add("created", System.Type.GetType("System.DateTime"));
			while (recordsReader.Read())
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["id"] = recordsReader.GetInt("id");
				dataRow["nodeid"] = recordsReader.GetInt("nodeid");
				dataRow["name"] = recordsReader.GetString("name");
				dataRow["email"] = recordsReader.GetString("email");
				dataRow["website"] = recordsReader.GetString("website");
				dataRow["comment"] = recordsReader.GetString("comment");
				dataRow["spam"] = (!recordsReader.IsNull("spam") && recordsReader.GetBoolean("spam"));
				dataRow["ham"] = (!recordsReader.IsNull("ham") && recordsReader.GetBoolean("ham"));
				dataRow["created"] = recordsReader.GetDateTime("created");
				dataTable.Rows.Add(dataRow);
			}
			System.Web.UI.WebControls.PagedDataSource pagedDataSource = new System.Web.UI.WebControls.PagedDataSource();
			pagedDataSource.DataSource = dataTable.DefaultView;
			pagedDataSource.AllowPaging = true;
			pagedDataSource.PageSize = 25;
			pagedDataSource.CurrentPageIndex = this.CurrentPage;
			this.rptComments.DataSource = pagedDataSource;
			this.rptComments.DataBind();
			if (pagedDataSource.PageCount > 1)
			{
				System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
				for (int i = 0; i < pagedDataSource.PageCount; i++)
				{
					arrayList.Add((i + 1).ToString());
				}
				this.rptPages.DataSource = arrayList;
				this.rptPages.DataBind();
			}
		}
		protected void btnDelete_Click(object sender, System.EventArgs e)
		{
			this.DeleteComment(System.Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument));
			this.BindData();
		}
		private void DeleteComment(int id)
		{
			this.SqlHelper.ExecuteNonQuery("delete from comment where id = @id", new IParameter[]
			{
				this.SqlHelper.CreateParameter("@id", id)
			});
		}
		protected void btnToggleState_Click(object sender, System.EventArgs e)
		{
			IRecordsReader recordsReader = this.SqlHelper.ExecuteReader("select * from comment where id = @id", new IParameter[]
			{
				this.SqlHelper.CreateParameter("@id", System.Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument))
			});
			recordsReader.Read();
			int @int = recordsReader.GetInt("nodeid");
			string @string = recordsReader.GetString("name");
			string string2 = recordsReader.GetString("email");
			string string3 = recordsReader.GetString("website");
			string string4 = recordsReader.GetString("comment");
			if (((System.Web.UI.WebControls.LinkButton)sender).CommandName == "True")
			{
				this.SqlHelper.ExecuteNonQuery("update comment set spam = 0, ham = 1 where id = @id", new IParameter[]
				{
					this.SqlHelper.CreateParameter("@id", System.Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument))
				});
				SpamChecker checker = Config.GetChecker();
				if (checker != null)
				{
					checker.MarkAsHam(@int, @string, string2, string3, string4);
				}
			}
			else
			{
				this.SqlHelper.ExecuteNonQuery("update comment set spam = 1 where id = @id", new IParameter[]
				{
					this.SqlHelper.CreateParameter("@id", System.Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument))
				});
				SpamChecker checker = Config.GetChecker();
				if (checker != null)
				{
					checker.MarkAsSpam(@int, @string, string2, string3, string4);
				}
			}
			this.BindData();
		}
		public string GetPageDetails(object nodeid)
		{
			int id;
			string result;
			if (int.TryParse(nodeid.ToString(), out id))
			{
				try
				{
					Document document = new Document(id);
					result = string.Format("<a href='{0}' target='_blank'>{1}</a>", library.NiceUrl(document.Id), document.Text);
					return result;
				}
				catch
				{
					result = string.Empty;
					return result;
				}
			}
			result = string.Empty;
			return result;
		}
		public string GetToggleStateText(object spam)
		{
			bool flag;
			string result;
			if (bool.TryParse(spam.ToString(), out flag))
			{
				if (flag)
				{
					result = "Mark as ham";
				}
				else
				{
					result = "Mark as spam";
				}
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		protected void btnApproved_Click(object sender, System.EventArgs e)
		{
			this.btnApproved.Enabled = false;
			this.btnSpam.Enabled = true;
			this.btnAll.Enabled = true;
			this.Filter = "where spam != 1";
			this.CurrentPage = 0;
			this.BindData();
		}
		protected void btnSpam_Click(object sender, System.EventArgs e)
		{
			this.btnApproved.Enabled = true;
			this.btnSpam.Enabled = false;
			this.btnAll.Enabled = true;
			this.Filter = "where spam = 1";
			this.CurrentPage = 0;
			this.BindData();
		}
		protected void btnAll_Click(object sender, System.EventArgs e)
		{
			this.btnApproved.Enabled = true;
			this.btnSpam.Enabled = true;
			this.btnAll.Enabled = false;
			this.Filter = "";
			this.CurrentPage = 0;
			this.BindData();
		}
		protected void btnDeleteSelected_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.RepeaterItem repeaterItem in this.rptComments.Items)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)repeaterItem.FindControl("cbSelectComment");
				if (checkBox.Checked)
				{
					this.DeleteComment(System.Convert.ToInt32(((System.Web.UI.WebControls.Label)repeaterItem.FindControl("lblID")).Text));
				}
			}
			this.BindData();
		}
		protected void rptComments_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			string commandName = e.CommandName;
			if (commandName != null)
			{
				if (commandName == "DeleteComment")
				{
					this.DeleteComment(int.Parse(e.CommandArgument.ToString()));
					this.BindData();
				}
			}
		}
	}
}
