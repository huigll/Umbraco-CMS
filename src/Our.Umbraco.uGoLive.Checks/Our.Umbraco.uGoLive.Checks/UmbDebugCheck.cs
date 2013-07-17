using Our.Umbraco.uGoLive.Attribution;
using System;
namespace Our.Umbraco.uGoLive.Checks
{
	[Check("4448738F-F5EE-4B42-A627-E685ED8FEDF4", "Umbraco Debug Mode", "Leaving umbraco debug mode enabled can make valuable information about your system available to hackers.")]
	public class UmbDebugCheck : AbstractAppSettingCheck
	{
		public override string Key
		{
			get
			{
				return "umbracoDebugMode";
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
				return "Umbraco debug mode is disabled.";
			}
		}
		public override string CheckFailedMessage
		{
			get
			{
				return "Umbraco debug mode is currently enabled. It is recommended to disable this setting before go live.";
			}
		}
		public override string RectifySuccessMessage
		{
			get
			{
				return "Umbraco debug mode successfully disabled.";
			}
		}
	}
}
