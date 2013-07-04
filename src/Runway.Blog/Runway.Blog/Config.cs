using Runway.Blog.Spam;
using System;
using System.Reflection;
using System.Web;
using System.Xml;
using umbraco;
namespace Runway.Blog
{
	public class Config
	{
		public static SpamChecker GetChecker()
		{
			SpamChecker result = null;
			try
			{
				string assemblyFile = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}/..{1}.dll", GlobalSettings.Path, Config.GetProviderAssembly()));
				System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyFile);
				result = (SpamChecker)System.Activator.CreateInstance(assembly.GetType(Config.GetProviderType()));
			}
			catch (System.Exception var_3_48)
			{
			}
			return result;
		}
		public static string GetProviderAssembly()
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/plugins/blog4umbraco/SpamChecker.config"));
			System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("/SpamChecker");
			return xmlNode.Attributes["assembly"].Value;
		}
		public static string GetProviderType()
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/plugins/blog4umbraco/SpamChecker.config"));
			System.Xml.XmlNode xmlNode = xmlDocument.SelectSingleNode("/SpamChecker");
			return xmlNode.Attributes["type"].Value;
		}
	}
}
