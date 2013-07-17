using System;
using System.IO;
using System.Web;
namespace Our.Umbraco.uGoLive.Checks
{
	public abstract class AbstractPathCheck : AbstractCheck
	{
		public abstract string Path
		{
			get;
		}
		public string PathName
		{
			get
			{
				return this.Path.Substring(this.Path.LastIndexOf('/') + 1);
			}
		}
		public string AbsolutePath
		{
			get
			{
				return HttpContext.Current.Server.MapPath(this.Path);
			}
		}
		public string PathType
		{
			get
			{
				return this.IsDirectoryPath ? "directory" : "file";
			}
		}
		public bool IsDirectoryPath
		{
			get
			{
				return System.IO.Path.GetExtension(this.Path) == "";
			}
		}
		public abstract PathComparisonType PathComparisonType
		{
			get;
		}
		public override bool CanRectify
		{
			get
			{
				return this.PathComparisonType == PathComparisonType.ShouldNotExist;
			}
		}
		public virtual string CheckPassedMessage
		{
			get
			{
				return (this.PathComparisonType == PathComparisonType.ShouldExist) ? "<strong>{1}</strong> {2} found." : "<strong>{1}</strong> {2} not found.";
			}
		}
		public virtual string CheckFailedMessage
		{
			get
			{
				return (this.PathComparisonType == PathComparisonType.ShouldExist) ? "<strong>{1}</strong> {2} not found." : "<strong>{1}</strong> {2} found.";
			}
		}
		public string RectifySuccessMessage
		{
			get
			{
				return "The '<strong>{0}</strong>' {2} has been deleted.";
			}
		}
		public string RectifyFailedMessage
		{
			get
			{
				return "The '<strong>{0}</strong>' {2} can not be delete.";
			}
		}
		public override CheckResult Check()
		{
			return ((this.PathComparisonType == PathComparisonType.ShouldExist && !this.PathExists()) || (this.PathComparisonType == PathComparisonType.ShouldNotExist && this.PathExists())) ? new CheckResult
			{
				Status = CheckResultStatus.Failed,
				Message = string.Format(this.CheckFailedMessage, this.Path, this.PathName, this.PathType)
			} : new CheckResult
			{
				Status = CheckResultStatus.Passed,
				Message = string.Format(this.CheckPassedMessage, this.Path, this.PathName, this.PathType)
			};
		}
		public override RectifyResult Rectify()
		{
			RectifyResult result;
			if (this.PathComparisonType == PathComparisonType.ShouldExist)
			{
				result = base.Rectify();
			}
			else
			{
				result = (this.DeletePath() ? new RectifyResult
				{
					Status = RectifyResultStatus.Success,
					Message = string.Format(this.RectifySuccessMessage, this.Path, this.PathName, this.PathType)
				} : new RectifyResult
				{
					Status = RectifyResultStatus.Failed,
					Message = string.Format(this.RectifyFailedMessage, this.Path, this.PathName, this.PathType)
				});
			}
			return result;
		}
		private bool PathExists()
		{
			return this.IsDirectoryPath ? Directory.Exists(this.AbsolutePath) : File.Exists(this.AbsolutePath);
		}
		private bool DeletePath()
		{
			return this.IsDirectoryPath ? this.DeleteDirectory(this.AbsolutePath) : this.DeleteFile(this.AbsolutePath);
		}
		private bool DeleteFile(string path)
		{
			bool result;
			try
			{
				File.Delete(path);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		private bool DeleteDirectory(string path)
		{
			bool result;
			try
			{
				string[] files = Directory.GetFiles(path);
				string[] directories = Directory.GetDirectories(path);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string path2 = array[i];
					File.SetAttributes(path2, FileAttributes.Normal);
					File.Delete(path2);
				}
				array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					string path3 = array[i];
					this.DeleteDirectory(path3);
				}
				Directory.Delete(path, false);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
