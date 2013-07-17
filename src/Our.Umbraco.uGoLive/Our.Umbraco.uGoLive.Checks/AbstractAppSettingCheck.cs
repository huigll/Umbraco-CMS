using System;
using System.Configuration;
using System.Web.Configuration;
namespace Our.Umbraco.uGoLive.Checks
{
	public abstract class AbstractAppSettingCheck : AbstractCheck
	{
		public abstract string Key
		{
			get;
		}
		public abstract string Value
		{
			get;
		}
		public abstract ValueComparisonType ValueComparisonType
		{
			get;
		}
		public virtual string CheckPassedMessage
		{
			get
			{
				return "AppSetting <strong>{0}</strong> passed check successfully.";
			}
		}
		public virtual string CheckFailedMessage
		{
			get
			{
				return (this.ValueComparisonType == ValueComparisonType.ShouldEqual) ? "Expected value <strong>{1}</strong> for the AppSetting <strong>{0}</strong>, but found <strong>{2}</strong>" : "Unexpected value <strong>{1}</strong> for the AppSetting <strong>{0}</strong>";
			}
		}
		public virtual string RectifySuccessMessage
		{
			get
			{
				return "AppSetting <strong>{0}</strong> rectified successfully.";
			}
		}
		public override bool CanRectify
		{
			get
			{
				return this.ValueComparisonType == ValueComparisonType.ShouldEqual;
			}
		}
		public override CheckResult Check()
		{
			string text = WebConfigurationManager.AppSettings[this.Key];
			return ((this.ValueComparisonType == ValueComparisonType.ShouldEqual && text != this.Value) || (this.ValueComparisonType == ValueComparisonType.ShouldNotEqual && text == this.Value)) ? new CheckResult
			{
				Status = CheckResultStatus.Failed,
				Message = string.Format(this.CheckFailedMessage, this.Key, this.Value, text)
			} : new CheckResult
			{
				Status = CheckResultStatus.Passed,
				Message = string.Format(this.CheckPassedMessage, this.Key, this.Value, text)
			};
		}
		public override RectifyResult Rectify()
		{
			if (this.ValueComparisonType == ValueComparisonType.ShouldNotEqual)
			{
				throw new InvalidOperationException("Cannot rectify a check with a value comparison type of ShouldNotEqual.");
			}
			Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
			configuration.AppSettings.Settings[this.Key].Value = this.Value;
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return new RectifyResult
			{
				Status = RectifyResultStatus.Success,
				Message = string.Format(this.RectifySuccessMessage, this.Key, this.Value)
			};
		}
	}
}
