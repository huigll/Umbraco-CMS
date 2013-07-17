using Our.Umbraco.uGoLive.Checks;
using System;
using System.Linq;
using System.Web.UI;
namespace Our.Umbraco.uGoLive.Web.Umbraco.Plugins.uGoLive
{
	public class Dashboard : UserControl
	{
		public object Checks
		{
			get
			{
				return 
					from x in CheckFactory.Checks
					select new
					{
						x.Id,
						x.Name,
						x.Description,
						x.CanRectify,
						x.Group
					} into x
					orderby (x.Group == "Core") ? 1 : 2, x.Group, x.Name
					group x by x.Group;
			}
		}
	}
}
