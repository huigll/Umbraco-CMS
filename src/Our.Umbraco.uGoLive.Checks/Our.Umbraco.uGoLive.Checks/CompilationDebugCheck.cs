using Our.Umbraco.uGoLive.Attribution;
using Our.Umbraco.uGoLive.Web;
using System;
namespace Our.Umbraco.uGoLive.Checks
{
	[Check("61214FF3-FC57-4B31-B5CF-1D095C977D6D", "Debug Compilation Mode", "Leaving debug compilation mode enabled can severely slow down a website and take up more memory on the server.")]
	public class CompilationDebugCheck : AbstractConfigCheck
	{
		public override string FilePath
		{
			get
			{
				return SystemPaths.WebConfig;
			}
		}
		public override string XPath
		{
			get
			{
				return "/configuration/system.web/compilation/@debug";
			}
		}
		public override ValueComparisonType ValueComparisonType
		{
			get
			{
				return ValueComparisonType.ShouldEqual;
			}
		}
		public override string Value
		{
			get
			{
				return bool.FalseString.ToLower();
			}
		}
		public override string CheckPassedMessage
		{
			get
			{
				return "Debug compilation mode is disabled.";
			}
		}
		public override string CheckFailedMessage
		{
			get
			{
				return "Debug compilation mode is currently enabled. It is recommended to disable this setting before go live.";
			}
		}
		public override string RectifySuccessMessage
		{
			get
			{
				return "Debug compulation mode successfully disabled.";
			}
		}
	}
}
