using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.property;
using umbraco.cms.businesslogic.web;
namespace Runway.Blog.EventHandlers
{
    public class Autoping : umbraco.businesslogic.ApplicationStartupHandler// ApplicationBase
	{
		public Autoping()
		{
			Document.AfterPublish += new Document.PublishEventHandler(this.Document_AfterPublish);
		}
		private void Document_AfterPublish(Document sender, PublishEventArgs e)
		{
			if (sender.ContentType.Alias == "umbBlogPost")
			{
				string valueRecursively = this.GetValueRecursively("pingServices", sender.Id);
				if (!string.IsNullOrEmpty(valueRecursively))
				{
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					try
					{
						xmlDocument.LoadXml(valueRecursively);
						string valueRecursively2 = this.GetValueRecursively("blogName", sender.Id);
						string text = library.NiceUrlFullPath(sender.Id);
						string str = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();
						if (!UmbracoSettings.UseDomainPrefixes)
						{
							text = "http://" + str + text;
						}
						foreach (System.Xml.XmlNode xmlNode in xmlDocument.SelectNodes("//link [@type = 'external']"))
						{
							string value = xmlNode.Attributes["link"].Value;
							this.PingService(value, valueRecursively2, text);
						}
					}
					catch
					{
					}
				}
			}
		}
		private string GetValueRecursively(string alias, int nodeId)
		{
			Document document = new Document(nodeId);
			Property property = document.getProperty(alias);
			string result;
			if (property != null && !string.IsNullOrEmpty(property.Value.ToString()))
			{
				result = property.Value.ToString();
			}
			else
			{
				if (document.Level > 1)
				{
					result = this.GetValueRecursively(alias, document.Parent.Id);
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}
		private void PingService(string ping, string name, string url)
		{
			try
			{
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ping);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "text/xml";
				using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
				{
					using (System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(requestStream, System.Text.Encoding.UTF8))
					{
						xmlTextWriter.WriteStartDocument();
						xmlTextWriter.WriteStartElement("methodCall");
						xmlTextWriter.WriteElementString("methodName", "weblogUpdates.ping");
						xmlTextWriter.WriteStartElement("params");
						xmlTextWriter.WriteStartElement("param");
						xmlTextWriter.WriteElementString("value", name);
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteStartElement("param");
						xmlTextWriter.WriteElementString("value", url);
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndDocument();
					}
				}
			}
			catch (System.Exception ex)
			{
				Log.Add(LogTypes.Debug, -1, "pinger: url: " + ping + " exception:" + ex.ToString());
			}
		}
	}
}
