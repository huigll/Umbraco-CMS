using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager;
using umbraco.cms.businesslogic.web;
using umbraco.uicontrols;
namespace Runway.Blog.DataTypes
{
	public class SkinBrowser : System.Web.UI.HtmlControls.HtmlInputHidden
	{
		public string PackageIdentifier
		{
			get;
			set;
		}
		public string ErrorMessage
		{
			get;
			set;
		}
		public string SuccessMessage
		{
			get;
			set;
		}
		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			this.Page.ClientScript.RegisterClientScriptInclude("skinBrowser", this.Page.ResolveUrl(GlobalSettings.Path + "/plugins/blog4umbraco/skinBrowser.js"));
			umbraco.uicontrols.helper.AddLinkToHeader("skinBrowser", this.Page.ResolveUrl(GlobalSettings.Path + "/plugins/blog4umbraco/skinBrowser.css"), this.Page);
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write(string.Format("<input type=\"hidden\" name=\"{0}\" id=\"{1}\" value=\"{2}\" />", this.UniqueID, this.ClientID, this.Value));
			if (!string.IsNullOrEmpty(this.ErrorMessage))
			{
				writer.Write("<div class=\"error\"><p>" + this.ErrorMessage + "</p></div>");
			}
			else
			{
				if (!string.IsNullOrEmpty(this.SuccessMessage))
				{
					writer.Write("<div class=\"success\"><p>" + this.SuccessMessage + "</p></div>");
				}
			}
			writer.Write(string.Format("<p>Your current theme is '{0}'</p><p><a href=\"#\" id=\"themeLoadLink_{2}\" onClick=\"loadSkins('{2}','{1}');\">Change theme (requires online access)</a></p>", this.Value, this.PackageIdentifier, this.ClientID));
			writer.Write(string.Format("<ul class=\"skinBrowser\" id=\"skinContainer_{0}\"></ul>", this.ClientID));
		}
		public static string DownloadPackage(System.Guid packageGuid)
		{
            var installer = new global::umbraco.cms.businesslogic.packager.Installer();//new Installer();
			return System.Web.HttpContext.Current.Server.MapPath(System.IO.Path.Combine(GlobalSettings.StorageDirectory, installer.Fetch(packageGuid)));
		}
		public static string ExtractPackage(string zipPath)
		{
			string text = System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.StorageDirectory) + System.IO.Path.DirectorySeparatorChar + System.Guid.NewGuid().ToString();
			System.IO.Directory.CreateDirectory(text);
			ZipInputStream zipInputStream = new ZipInputStream(System.IO.File.OpenRead(zipPath));
			ZipEntry nextEntry;
			while ((nextEntry = zipInputStream.GetNextEntry()) != null)
			{
				string directoryName = System.IO.Path.GetDirectoryName(nextEntry.Name);
				string fileName = System.IO.Path.GetFileName(nextEntry.Name);
				if (fileName != string.Empty)
				{
					if (!System.IO.Directory.Exists(text + System.IO.Path.DirectorySeparatorChar + directoryName))
					{
						System.IO.Directory.CreateDirectory(text + System.IO.Path.DirectorySeparatorChar + directoryName);
					}
					System.IO.FileStream fileStream = System.IO.File.Create(string.Concat(new object[]
					{
						text, 
						System.IO.Path.DirectorySeparatorChar, 
						directoryName, 
						System.IO.Path.DirectorySeparatorChar, 
						fileName
					}));
					byte[] array = new byte[2048];
					while (true)
					{
						int num = zipInputStream.Read(array, 0, array.Length);
						if (num <= 0)
						{
							break;
						}
						fileStream.Write(array, 0, num);
					}
					fileStream.Close();
				}
			}
			zipInputStream.Close();
			System.IO.File.Delete(zipPath);
			return text;
		}
		public static string InstallSkin(string skinPath)
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.Load(skinPath + "\\skin.xml");
			string text = "skin_" + SkinBrowser.safeFileName(SkinBrowser.getNodeValue(xmlDocument, "/skin/title"));
			string text2 = System.Web.HttpContext.Current.Server.MapPath(GlobalSettings.Path + "/../css/" + text);
			if (System.IO.Directory.Exists(text2))
			{
				System.IO.Directory.Delete(text2, true);
			}
			System.IO.Directory.Move(skinPath, text2);
			string text3 = text + "/style";
			bool flag = false;
			StyleSheet[] all = StyleSheet.GetAll();
			for (int i = 0; i < all.Length; i++)
			{
				StyleSheet styleSheet = all[i];
				if (styleSheet.Text == text3)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				StyleSheet styleSheet2 = StyleSheet.MakeNew(User.GetUser(0), text3, text3 + ".css", "");
			}
			return "/css/" + text;
		}
		private static string getNodeValue(System.Xml.XmlDocument xd, string xPath)
		{
			System.Xml.XmlNode xmlNode = xd.SelectSingleNode(xPath);
			string result;
			if (xmlNode != null && xmlNode.FirstChild != null && !string.IsNullOrEmpty(xmlNode.FirstChild.Value))
			{
				result = xmlNode.FirstChild.Value;
			}
			else
			{
				result = "";
			}
			return result;
		}
		private static string safeFileName(string skinName)
		{
			skinName = skinName.Replace(" ", "_");
			char[] array = System.IO.Path.GetInvalidFileNameChars();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				skinName = skinName.Replace(c.ToString(), "");
			}
			array = System.IO.Path.GetInvalidPathChars();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				skinName = skinName.Replace(c.ToString(), "");
			}
			return skinName;
		}
	}
}
