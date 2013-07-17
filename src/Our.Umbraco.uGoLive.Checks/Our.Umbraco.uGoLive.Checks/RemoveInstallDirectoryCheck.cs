using Our.Umbraco.uGoLive.Attribution;
using System;
using umbraco.IO;
namespace Our.Umbraco.uGoLive.Checks
{
	[Check("46E000D5-4073-4B63-BC54-F6FB6AE295CE", "Remove install directory", "Remove the /install directory.")]
	public class RemoveInstallDirectoryCheck : AbstractPathCheck
	{
		public override string Path
		{
			get
			{
				return SystemDirectories.get_Install();
			}
		}
		public override PathComparisonType PathComparisonType
		{
			get
			{
				return PathComparisonType.ShouldNotExist;
			}
		}
	}
}
