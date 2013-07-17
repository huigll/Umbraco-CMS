using System;
using Umbraco.Core.IO;
namespace Our.Umbraco.uGoLive.Web
{
	public class SystemPaths : SystemDirectories
	{
		public static string AppBrowsers
		{
			get
			{
				return "~/app_browsers";
			}
		}
		public static string AppCode
		{
			get
			{
				return "~/app_code";
			}
		}
		public static string AppData
		{
			get
			{
				return "~/app_data";
			}
		}
		public static string Base
		{
			get
			{
				return IOHelper.returnPath("umbracoBaseDirectory", "~/base");
			}
		}
		public static string WebConfig
		{
			get
			{
				return "~/web.config";
			}
		}
		public static string AppPlugins
		{
			get
			{
				return "~/App_Plugins";
			}
		}
		public static string Views
		{
			get
			{
				return "~/Views";
			}
		}
	}
}
