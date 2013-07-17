using System;
using System.Linq;
using System.Web;
using System.Xml;
using umbraco;
using umbraco.interfaces;
using Umbraco.Core.IO;
namespace Our.Umbraco.uGoLive.PackageActions
{
	public class AddRestExtension : IPackageAction
	{
		public string Alias()
		{
			return "ugl_AddRestExtension";
		}
		public bool Execute(string packageName, XmlNode xmlData)
		{
			bool result;
			if (GlobalSettings.get_CurrentVersion().StartsWith("4.7"))
			{
				result = true;
			}
			else
			{
				XmlNodeList xmlNodeList = xmlData.SelectNodes("//ext");
				if (xmlNodeList.Count > 0)
				{
					string text = HttpContext.Current.Server.MapPath(SystemFiles.get_RestextensionsConfig());
					XmlDocument xmlDocument = new XmlDocument
					{
						PreserveWhitespace = true
					};
					xmlDocument.Load(text);
					XmlNode xmlNode = xmlDocument.SelectSingleNode("//RestExtensions");
					if (xmlNode != null)
					{
						for (int i = 0; i < xmlNodeList.Count; i++)
						{
							XmlNode xmlNode3 = xmlNodeList[i];
							string alias = xmlNode3.Attributes["alias"].Value;
							bool flag = true;
							if (xmlNode.HasChildNodes && xmlNode.SelectNodes("//ext").Cast<XmlNode>().Any((XmlNode xmlNode2) => xmlNode2.Attributes["alias"] != null && xmlNode2.Attributes["alias"].Value == alias))
							{
								flag = false;
							}
							if (flag)
							{
								xmlNode.AppendChild(xmlDocument.ImportNode(xmlNode3, true));
							}
						}
						xmlDocument.Save(IOHelper.MapPath(text));
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}
		public bool Undo(string packageName, XmlNode xmlData)
		{
			bool result;
			if (GlobalSettings.get_CurrentVersion().StartsWith("4.7"))
			{
				result = true;
			}
			else
			{
				XmlNodeList xmlNodeList = xmlData.SelectNodes("//ext");
				if (xmlNodeList.Count > 0)
				{
					string text = HttpContext.Current.Server.MapPath(SystemFiles.get_RestextensionsConfig());
					XmlDocument xmlDocument = new XmlDocument
					{
						PreserveWhitespace = true
					};
					xmlDocument.Load(text);
					XmlNode xmlNode = xmlDocument.SelectSingleNode("//RestExtensions");
					if (xmlNode != null)
					{
						bool flag = false;
						for (int i = 0; i < xmlNodeList.Count; i++)
						{
							string alias = xmlNodeList[i].Attributes["alias"].Value;
							if (xmlNode.HasChildNodes)
							{
								foreach (XmlNode current in 
									from XmlNode oldChild in xmlNode.SelectNodes("//ext")
									where oldChild.Attributes["alias"] != null && oldChild.Attributes["alias"].Value == alias
									select oldChild)
								{
									xmlNode.RemoveChild(current);
									flag = true;
								}
							}
						}
						if (flag)
						{
							xmlDocument.Save(IOHelper.MapPath(text));
							result = true;
							return result;
						}
					}
				}
				result = false;
			}
			return result;
		}
		public XmlNode SampleXml()
		{
			throw new NotImplementedException();
		}
	}
}
