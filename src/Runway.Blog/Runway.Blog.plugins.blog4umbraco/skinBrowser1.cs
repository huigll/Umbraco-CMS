using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using umbraco;
namespace Runway.Blog.plugins.blog4umbraco
{
	public class skinBrowser1 : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Literal skinLists;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string requestUriString = string.Format("http://packages.umbraco.org/skins/{0}/getskinsasxml", helper.Request("package"));
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
			try
			{
				System.Net.WebResponse response = webRequest.GetResponse();
				System.IO.Stream responseStream = response.GetResponseStream();
				System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(responseStream);
				xmlDocument.Load(reader);
				response.Close();
				responseStream.Close();
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				foreach (System.Xml.XmlNode xmlNode in xmlDocument.SelectNodes("//skin"))
				{
					stringBuilder.Append(string.Format("<li id=\"{0}\"><a href=\"#\" onClick=\"chooseSkin('{0}')\"><img src=\"http://packages.umbraco.org{1}\" alt=\"{2}\" /></a><div class=\"skinBrowserInfo\"><h4>{2}</h4><p>{3}<br /><small>Author: {4}</small></p><p><a href=\"#\" onClick=\"chooseSkin('{0}')\">Choose this skin</a></p></div><br style=\"clear: both;\" /></li>", new object[]
					{
						xmlNode.SelectSingleNode("package").FirstChild.Value, 
						xmlNode.SelectSingleNode("thumbnail").FirstChild.Value, 
						xmlNode.SelectSingleNode("title").FirstChild.Value, 
						xmlNode.SelectSingleNode("description").FirstChild.Value, 
						xmlNode.SelectSingleNode("author/name").FirstChild.Value
					}));
				}
				this.skinLists.Text = stringBuilder.ToString();
			}
			catch (System.Exception ex)
			{
				throw new System.Exception(string.Format("Error reading skins: {0}", ex.ToString()));
			}
		}
	}
}
