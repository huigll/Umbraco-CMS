using System;
using System.Web;
using System.Xml;
using umbraco;
using umbraco.interfaces;
namespace Runway.Blog.Installer
{
	public class addRestExtension : IPackageAction
	{
		public bool Execute(string packageName, System.Xml.XmlNode xmlData)
		{
			System.Xml.XmlNodeList xmlNodeList = xmlData.SelectNodes("//ext");
			bool result;
			if (xmlNodeList.Count > 0)
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.PreserveWhitespace = true;
				xmlDocument = xmlHelper.OpenAsXmlDocument("/config/restExtensions.config");
				System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("//RestExtensions");
				if (xmlNode != null)
				{
					for (int i = 0; i < xmlNodeList.Count; i++)
					{
						System.Xml.XmlNode xmlNode2 = xmlNodeList[i];
						string value = xmlNode2.Attributes["alias"].Value;
						bool flag = true;
						if (xmlNode.HasChildNodes)
						{
							foreach (System.Xml.XmlNode xmlNode3 in xmlNode.SelectNodes("//ext"))
							{
								if (xmlNode3.Attributes["alias"] != null && xmlNode3.Attributes["alias"].Value == value)
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							xmlNode.AppendChild(xmlDocument.ImportNode(xmlNode2, true));
						}
					}
					xmlDocument.Save(System.Web.HttpContext.Current.Server.MapPath("/config/restExtensions.config"));
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public string Alias()
		{
			return "blog_addRestExtension";
		}
		public bool Undo(string packageName, System.Xml.XmlNode xmlData)
		{
			System.Xml.XmlNodeList xmlNodeList = xmlData.SelectNodes("//ext");
			bool result;
			if (xmlNodeList.Count > 0)
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.PreserveWhitespace = true;
				xmlDocument = xmlHelper.OpenAsXmlDocument("/config/restExtensions.config");
				System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("//RestExtensions");
				if (xmlNode != null)
				{
					bool flag = false;
					for (int i = 0; i < xmlNodeList.Count; i++)
					{
						System.Xml.XmlNode xmlNode2 = xmlNodeList[i];
						string value = xmlNode2.Attributes["alias"].Value;
						if (xmlNode.HasChildNodes)
						{
							foreach (System.Xml.XmlNode xmlNode3 in xmlNode.SelectNodes("//ext"))
							{
								if (xmlNode3.Attributes["alias"] != null && xmlNode3.Attributes["alias"].Value == value)
								{
									xmlNode.RemoveChild(xmlNode3);
									flag = true;
								}
							}
						}
					}
					if (flag)
					{
						xmlDocument.Save(System.Web.HttpContext.Current.Server.MapPath("/config/restExtensions.config"));
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}
		public System.Xml.XmlNode SampleXml()
		{
			throw new System.NotImplementedException();
		}
	}
}
