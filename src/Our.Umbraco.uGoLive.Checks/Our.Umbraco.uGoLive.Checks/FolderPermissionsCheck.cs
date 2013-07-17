using Our.Umbraco.uGoLive.Attribution;
using Our.Umbraco.uGoLive.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using umbraco.IO;
namespace Our.Umbraco.uGoLive.Checks
{
	[Check("172D4043-B93E-4E1F-B410-316986C8D210", "Folder Permissions", "Incorrect folder permissions may cause Umbraco to fail unexpectedly.")]
	public class FolderPermissionsCheck : AbstractCheck
	{
		private readonly List<string> _incorrectPermissionPaths;
		public FolderPermissionsCheck()
		{
			this._incorrectPermissionPaths = new List<string>();
		}
		public override CheckResult Check()
		{
			bool[] source = new bool[]
			{
				this.CanWriteToPath(SystemDirectories.get_Root()),
				this.CanWriteToPath(SystemPaths.AppBrowsers),
				this.CanWriteToPath(SystemPaths.AppCode),
				this.CanWriteToPath(SystemPaths.AppPlugins),
				this.CanWriteToPath(SystemDirectories.get_Data()),
				this.CanWriteToPath(SystemDirectories.get_Bin()),
				this.CanWriteToPath(SystemDirectories.get_Config()),
				this.CanWriteToPath(SystemDirectories.get_Css()),
				this.CanWriteToPath(SystemDirectories.get_Data()),
				this.CanWriteToPath(SystemDirectories.get_Masterpages()),
				this.CanWriteToPath(SystemDirectories.get_Media()),
				this.CanWriteToPath(SystemDirectories.get_Packages()),
				this.CanWriteToPath(SystemDirectories.get_Preview()),
				this.CanWriteToPath(SystemDirectories.get_Scripts()),
				this.CanWriteToPath(SystemDirectories.get_Umbraco()),
				this.CanWriteToPath(SystemDirectories.get_Usercontrols()),
				this.CanWriteToPath(SystemDirectories.get_Xslt()),
				this.CanWriteToPath(SystemPaths.Views),
				this.CanWriteToFile(SystemPaths.WebConfig)
			};
			CheckResult result;
			if (source.All((bool x) => x))
			{
				result = new CheckResult
				{
					Status = CheckResultStatus.Passed,
					Message = "Folder permissions set correctly."
				};
			}
			else
			{
				if (source.Any((bool x) => x))
				{
					result = new CheckResult
					{
						Status = CheckResultStatus.Indeterminate,
						Message = string.Format("Not all folder permissions are set correctly: {0}", string.Join(", ", this._incorrectPermissionPaths.ToArray())).TrimStart(new char[]
						{
							','
						})
					};
				}
				else
				{
					result = new CheckResult
					{
						Status = CheckResultStatus.Failed,
						Message = string.Format("Folder permissions are not set correctly. For paths: {0}", string.Join(", ", this._incorrectPermissionPaths.ToArray())).TrimStart(new char[]
						{
							','
						})
					};
				}
			}
			return result;
		}
		private bool CanWriteToPath(string path)
		{
			bool result;
			try
			{
				if (string.IsNullOrEmpty(path))
				{
					path = "~/";
				}
				string path2 = string.Concat(new object[]
				{
					HttpContext.Current.Server.MapPath(path).TrimEnd(new char[]
					{
						'\\'
					}),
					"\\",
					Guid.Empty,
					".tmp"
				});
				using (FileStream fileStream = File.Create(path2))
				{
					for (byte b = 0; b < 10; b += 1)
					{
						fileStream.WriteByte(b);
					}
				}
				File.Delete(path2);
				result = true;
			}
			catch (IOException)
			{
				this._incorrectPermissionPaths.Add(path);
				result = false;
			}
			return result;
		}
		private bool CanWriteToFile(string file)
		{
			bool result;
			try
			{
				string text = HttpContext.Current.Server.MapPath(file);
				string text2 = text + ".tmp";
				File.Copy(text, text2);
				File.Copy(text2, text, true);
				File.Delete(text2);
				result = true;
			}
			catch (IOException)
			{
				result = false;
			}
			return result;
		}
	}
}
