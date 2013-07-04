using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using umbraco;
using umbraco.DataLayer;
namespace Runway.Blog
{
	public class BlogLibrary
	{
		public static string Dictionary(string text, string fallback)
		{
			string text2 = text.Trim().TrimStart(new char[]
			{
				'#'
			});
			string dictionaryItem = library.GetDictionaryItem(text2);
			string result;
			if (dictionaryItem.Length > 0)
			{
				result = dictionaryItem;
			}
			else
			{
				if (!string.IsNullOrEmpty(fallback))
				{
					result = fallback;
				}
				else
				{
					result = text2;
				}
			}
			return result;
		}
		public static string getGravatar(string email, int size, string defaultImage)
		{
			System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(System.Text.Encoding.ASCII.GetBytes(email));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			stringBuilder2.Append("http://www.gravatar.com/avatar.php?");
			stringBuilder2.Append("gravatar_id=" + stringBuilder.ToString());
			stringBuilder2.Append("&amp;rating=G");
			stringBuilder2.Append("&amp;size=" + size.ToString());
			stringBuilder2.Append("&amp;default=");
			stringBuilder2.Append(System.Web.HttpContext.Current.Server.UrlEncode(defaultImage));
			return stringBuilder2.ToString();
		}
		public static System.Xml.XPath.XPathNodeIterator GetComments()
		{
			ISqlHelper sqlHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN,false);
			IRecordsReader rr = sqlHelper.ExecuteReader("select * from comment where spam != 1", new IParameter[0]);
			return BlogLibrary.CommentsToXml(rr);
		}
		public static System.Xml.XPath.XPathNodeIterator GetCommentsForBlog(int id)
		{
			ISqlHelper sqlHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN,false);
			IRecordsReader rr = sqlHelper.ExecuteReader("select * from comment where mainid = @mainid and spam != 1", new IParameter[]
			{
				sqlHelper.CreateParameter("@mainid", id)
			});
			return BlogLibrary.CommentsToXml(rr);
		}
		public static System.Xml.XPath.XPathNodeIterator GetCommentsForPost(int id)
		{
			ISqlHelper sqlHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN,false);
			IRecordsReader rr = sqlHelper.ExecuteReader("select * from comment where nodeid = @nodeid and spam != 1", new IParameter[]
			{
				sqlHelper.CreateParameter("@nodeid", id)
			});
			return BlogLibrary.CommentsToXml(rr);
		}
		private static System.Xml.XPath.XPathNodeIterator CommentsToXml(IRecordsReader rr)
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Xml.XmlNode xmlNode = xmlHelper.addTextNode(xmlDocument, "comments", "");
			while (rr.Read())
			{
				System.Xml.XmlNode xmlNode2 = xmlDocument.CreateElement("comment");
				System.Xml.XmlAttributeCollection arg_4F_0 = xmlNode2.Attributes;
				System.Xml.XmlDocument arg_4A_0 = xmlDocument;
				string arg_4A_1 = "id";
				int @int = rr.GetInt("id");
				arg_4F_0.Append(xmlHelper.addAttribute(arg_4A_0, arg_4A_1, @int.ToString()));
				System.Xml.XmlAttributeCollection arg_7A_0 = xmlNode2.Attributes;
				System.Xml.XmlDocument arg_75_0 = xmlDocument;
				string arg_75_1 = "nodeid";
				@int = rr.GetInt("nodeid");
				arg_7A_0.Append(xmlHelper.addAttribute(arg_75_0, arg_75_1, @int.ToString()));
				xmlNode2.Attributes.Append(xmlHelper.addAttribute(xmlDocument, "created", rr.GetDateTime("created").ToString()));
				xmlNode2.AppendChild(xmlHelper.addCDataNode(xmlDocument, "name", rr.GetString("name")));
				xmlNode2.AppendChild(xmlHelper.addCDataNode(xmlDocument, "email", rr.GetString("email")));
				xmlNode2.AppendChild(xmlHelper.addCDataNode(xmlDocument, "website", rr.GetString("website")));
				xmlNode2.AppendChild(xmlHelper.addCDataNode(xmlDocument, "message", rr.GetString("comment")));
				xmlNode.AppendChild(xmlNode2);
			}
			xmlDocument.AppendChild(xmlNode);
			rr.Close();
			return xmlDocument.CreateNavigator().Select(".");
		}
		public static int CountSeparatedStringMatches(string input1, string separator1, string input2, string separator2)
		{
			string[] array = input1.Split(new string[]
			{
				separator1
			}, System.StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = input2.Split(new string[]
			{
				separator2
			}, System.StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			string[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				string a = array3[i];
				string[] array4 = array2;
				for (int j = 0; j < array4.Length; j++)
				{
					string b = array4[j];
					if (a == b)
					{
						num++;
					}
				}
			}
			return num;
		}
		private static System.Xml.XPath.XPathNodeIterator GetIteratorFromString(string xmlstring)
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xmlstring);
			System.Xml.XPath.XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
			return xPathNavigator.Select("/");
		}
		public static System.Xml.XPath.XPathNodeIterator GetSkin(string cssFilePath)
		{
			System.Xml.XPath.XPathNodeIterator result;
			try
			{
				System.Web.HttpServerUtility server = System.Web.HttpContext.Current.Server;
				string path = server.MapPath(cssFilePath);
				if (!System.IO.File.Exists(path))
				{
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.LoadXml(string.Format("<error>Could not find the CSS file '{0}'</error>", cssFilePath));
					result = xmlDocument.CreateNavigator().Select("/");
				}
				else
				{
					string text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "skin.xml");
					if (!System.IO.File.Exists(text))
					{
						System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
						xmlDocument.LoadXml(string.Format("<error>Could not find the skin file '{0}'</error>", text));
						result = xmlDocument.CreateNavigator().Select("/");
					}
					else
					{
						System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
						xmlDocument.Load(text);
						result = xmlDocument.CreateNavigator().Select("/skin");
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.LoadXml(string.Format("<error><message>{0}</message><stackTrace>{1}</stackTrace></error>", ex.Message, ex.StackTrace));
				result = xmlDocument.CreateNavigator().Select("/");
			}
			return result;
		}
	}
}
