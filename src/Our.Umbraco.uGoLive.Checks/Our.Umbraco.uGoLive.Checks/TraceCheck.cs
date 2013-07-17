using Our.Umbraco.uGoLive.Attribution;
using Our.Umbraco.uGoLive.Web;
using System;
namespace Our.Umbraco.uGoLive.Checks
{
	[Check("9BED6EF4-A7F3-457A-8935-B64E9AA8BAB3", "Trace Mode", "Leaving trace mode enabled can make valuable information about your system available to hackers.")]
	public class TraceCheck : AbstractConfigCheck
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
				return "/configuration/system.web/trace/@enabled";
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
				return "Trace mode is disabled.";
			}
		}
		public override string CheckFailedMessage
		{
			get
			{
				return "Trace mode is currently enabled. It is recommended to disable this setting before go live.";
			}
		}
		public override string RectifySuccessMessage
		{
			get
			{
				return "Trace mode successfully disabled.";
			}
		}
	}
}
