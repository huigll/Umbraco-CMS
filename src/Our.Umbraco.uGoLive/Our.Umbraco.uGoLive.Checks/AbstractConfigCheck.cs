using System;
using System.IO;
using System.Web;
using System.Xml;
namespace Our.Umbraco.uGoLive.Checks
{
	public abstract class AbstractConfigCheck : AbstractCheck
	{
		public abstract string FilePath
		{
			get;
		}
		public abstract string XPath
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
				return "Node <strong>{1}</strong> passed check successfully.";
			}
		}
		public virtual string CheckFailedMessage
		{
			get
			{
				return (this.ValueComparisonType == ValueComparisonType.ShouldEqual) ? "Expected value <strong>{2}</strong> for the node <strong>{1}</strong> in config <strong>{0}</strong>, but found <strong>{3}</strong>" : "Found unexpected value <strong>{2}</strong> for the node <strong>{1}</strong> in config <strong>{0}</strong>";
			}
		}
		public virtual string RectifySuccessMessage
		{
			get
			{
				return "Node <strong>{1}</strong> rectified successfully.";
			}
		}
		private string FileName
		{
			get
			{
				return Path.GetFileName(this.FilePath);
			}
		}
		private string AbsoluteFilePath
		{
			get
			{
				return HttpContext.Current.Server.MapPath(this.FilePath);
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
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.AbsoluteFilePath);
			XmlNode xmlNode = xmlDocument.SelectSingleNode(this.XPath);
			CheckResult result;
			if (xmlNode == null)
			{
				result = new CheckResult
				{
					Status = CheckResultStatus.Failed,
					Message = string.Format("Unable to find node <strong>{1}</strong> in config file <strong>{0}</strong>", this.FileName, this.XPath)
				};
			}
			else
			{
				string text = xmlNode.Value ?? xmlNode.InnerText;
				result = (((this.ValueComparisonType == ValueComparisonType.ShouldEqual && text != this.Value) || (this.ValueComparisonType == ValueComparisonType.ShouldNotEqual && text == this.Value)) ? new CheckResult
				{
					Status = CheckResultStatus.Failed,
					Message = string.Format(this.CheckFailedMessage, new object[]
					{
						this.FileName,
						this.XPath,
						this.Value,
						text
					})
				} : new CheckResult
				{
					Status = CheckResultStatus.Passed,
					Message = string.Format(this.CheckPassedMessage, new object[]
					{
						this.FileName,
						this.XPath,
						this.Value,
						text
					})
				});
			}
			return result;
		}
		public override RectifyResult Rectify()
		{
			if (this.ValueComparisonType == ValueComparisonType.ShouldNotEqual)
			{
				throw new InvalidOperationException("Cannot rectify a check with a value comparison type of ShouldNotEqual.");
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.AbsoluteFilePath);
			XmlNode xmlNode = xmlDocument.SelectSingleNode(this.XPath);
			RectifyResult result;
			if (xmlNode == null)
			{
				result = new RectifyResult
				{
					Status = RectifyResultStatus.Failed,
					Message = string.Format("Unable to find node <strong>{1}</strong> in config file <strong>{0}</strong>", this.FileName, this.XPath)
				};
			}
			else
			{
				xmlNode.Value = this.Value;
				xmlDocument.Save(this.AbsoluteFilePath);
				result = new RectifyResult
				{
					Status = RectifyResultStatus.Success,
					Message = string.Format(this.RectifySuccessMessage, this.FileName, this.XPath, this.Value)
				};
			}
			return result;
		}
	}
}
