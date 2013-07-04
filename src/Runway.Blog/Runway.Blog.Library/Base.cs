using Runway.Blog.Spam;
using System;
using System.Text.RegularExpressions;
using System.Web;
using umbraco;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using umbraco.DataLayer;
namespace Runway.Blog.Library
{
	public class Base
	{
		public static int CreateComment(int parentId)
		{
			System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
			string text = request["email"];
			string text2 = request["comment"];
			string text3 = request["author"];
			string text4 = request["url"];
			int result;
			try
			{
				if (!string.IsNullOrEmpty(text) && Base.isValidEmail(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
				{
					Document document = new Document(parentId);
					if (document != null && document.ContentType.Alias == "umbBlogPost")
					{
						CMSNode cMSNode = new CMSNode(document.Id);
						while (cMSNode.Level > 1)
						{
							cMSNode = cMSNode.Parent;
						}
						int id = cMSNode.Id;
						bool flag = false;
						SpamChecker checker = Config.GetChecker();
						if (checker != null)
						{
							flag = checker.Check(parentId, request.UserAgent, request.UserHostAddress, text3, text, text4, text2);
						}
						ISqlHelper sqlHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN,false);
						sqlHelper.ExecuteNonQuery("insert into Comment(mainid,nodeid,name,email,website,comment,spam,created) \r\n                    values(@mainid,@nodeid,@name,@email,@website,@comment,@spam,@created)", new IParameter[]
						{
							sqlHelper.CreateParameter("@mainid", id), 
							sqlHelper.CreateParameter("@nodeid", document.Id), 
							sqlHelper.CreateParameter("@name", text3), 
							sqlHelper.CreateParameter("@email", text), 
							sqlHelper.CreateParameter("@website", text4), 
							sqlHelper.CreateParameter("@comment", text2), 
							sqlHelper.CreateParameter("@spam", flag), 
							sqlHelper.CreateParameter("@created", System.DateTime.Now)
						});
						result = 1;
						return result;
					}
				}
				result = 0;
			}
			catch (System.Exception)
			{
				result = 0;
			}
			return result;
		}
		public static string GetGravatarImage(string email, int size)
		{
			string result;
			if (Base.isValidEmail(email))
			{
				result = string.Format("http://www.gravatar.com/avatar/{0}?s={1}", library.md5(email), size.ToString());
			}
			else
			{
				result = "";
			}
			return result;
		}
		private static bool isValidEmail(string email)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
			return !string.IsNullOrEmpty(email) && regex.IsMatch(email);
		}
	}
}
