using System;
namespace Our.Umbraco.uGoLive.Checks
{
	public interface ICheck
	{
		Guid Id
		{
			get;
		}
		string Name
		{
			get;
		}
		string Description
		{
			get;
		}
		string Group
		{
			get;
		}
		bool CanRectify
		{
			get;
		}
		CheckResult Check();
		RectifyResult Rectify();
	}
}
