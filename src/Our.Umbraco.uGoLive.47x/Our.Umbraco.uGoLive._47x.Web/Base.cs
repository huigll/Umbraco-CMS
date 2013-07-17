using Our.Umbraco.uGoLive.Web;
using System;
using umbraco.presentation.umbracobase;
namespace Our.Umbraco.uGoLive._47x.Web
{
	[RestExtension("uGoLive")]
	public class Base
	{
		[RestExtensionMethod(allowAll = true, returnXml = false)]
		public static string Check(string id)
		{
			return Our.Umbraco.uGoLive.Web.Base.Check(id);
		}
		[RestExtensionMethod(allowAll = true, returnXml = false)]
		public static string Rectify(string id)
		{
			return Our.Umbraco.uGoLive.Web.Base.Rectify(id);
		}
	}
}
