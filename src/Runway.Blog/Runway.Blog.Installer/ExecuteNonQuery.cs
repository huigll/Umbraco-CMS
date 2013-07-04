using System;
using System.Xml;
using umbraco;
using umbraco.DataLayer;
using umbraco.interfaces;
namespace Runway.Blog.Installer
{
	public class ExecuteNonQuery : IPackageAction
	{
		public string Alias()
		{
			return "blog_executeNonQuery";
		}
		public bool Execute(string packageName, System.Xml.XmlNode xmlData)
		{
			string innerText = xmlData.SelectSingleNode("//Sqlserver").InnerText;
			if (GlobalSettings.DbDSN.ToLower().Contains("datalayer=mysql"))
			{
				innerText = xmlData.SelectSingleNode("//MySQL").InnerText;
			}
			else
			{
				if (GlobalSettings.DbDSN.ToLower().Contains("vistadb,vistadb"))
				{
					innerText = xmlData.SelectSingleNode("//VistaDB").InnerText;
				}
			}
			ISqlHelper sqlHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN);
			sqlHelper.ExecuteNonQuery(innerText, new IParameter[0]);
			return true;
		}
		public System.Xml.XmlNode SampleXml()
		{
			throw new System.NotImplementedException();
		}
		public bool Undo(string packageName, System.Xml.XmlNode xmlData)
		{
			throw new System.NotImplementedException();
		}
	}
}
