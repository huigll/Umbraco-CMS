using Our.Umbraco.uGoLive.Attribution;
using Our.Umbraco.uGoLive.Web;
using System;
using System.Web.Configuration;
namespace Our.Umbraco.uGoLive.Checks
{
	[Check("4090C0A1-2C52-4124-92DD-F028FD066A64", "Custom Errors", "Leaving custom errors off will display a complete stack trace to your visitors if an exception occurs.")]
	public class CustomErrorsCheck : AbstractConfigCheck
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
				return "/configuration/system.web/customErrors/@mode";
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
				return CustomErrorsMode.RemoteOnly.ToString();
			}
		}
		public override string CheckPassedMessage
		{
			get
			{
				return "Custom errors are enabled.";
			}
		}
		public override string CheckFailedMessage
		{
			get
			{
				return "Custom errors are currently disabled. It is recommended to enable this setting before go live.";
			}
		}
		public override string RectifySuccessMessage
		{
			get
			{
				return "Custom errors successfully enabled.";
			}
		}
	}
}
