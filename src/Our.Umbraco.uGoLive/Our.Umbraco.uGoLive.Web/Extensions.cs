using Our.Umbraco.uGoLive.Checks;
using Our.Umbraco.uGoLive.Web.Converters;
using System;
using System.Web.Script.Serialization;
namespace Our.Umbraco.uGoLive.Web
{
	public static class Extensions
	{
		public static string ToJsonString(this object obj)
		{
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.RegisterConverters(new JavaScriptConverter[]
			{
				new EnumConverter<CheckResultStatus>(),
				new EnumConverter<RectifyResultStatus>()
			});
			return javaScriptSerializer.Serialize(obj);
		}
	}
}
