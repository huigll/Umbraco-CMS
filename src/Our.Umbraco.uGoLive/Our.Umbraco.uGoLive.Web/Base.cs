using Our.Umbraco.uGoLive.Checks;
using System;
using System.Linq;
using umbraco;
using umbraco.BusinessLogic;
namespace Our.Umbraco.uGoLive.Web
{
	public class Base
	{
		public static string Check(string id)
		{
			bool arg_41_0;
			if (helper.GetCurrentUmbracoUser() != null)
			{
				arg_41_0 = helper.GetCurrentUmbracoUser().Applications.Any((Application x) => x.get_alias() == "developer");
			}
			else
			{
				arg_41_0 = false;
			}
			string result;
			if (!arg_41_0)
			{
				result = new CheckResult
				{
					Status = CheckResultStatus.Failed,
					Message = "User is not authorized perform checks."
				}.ToJsonString();
			}
			else
			{
				Guid checkId = new Guid(id);
				ICheck check = CheckFactory.Checks.SingleOrDefault((ICheck x) => x.Id == checkId);
				if (check == null)
				{
					result = new CheckResult
					{
						Status = CheckResultStatus.Failed,
						Message = string.Format("Unabled to find check with id '{0}'.", id)
					}.ToJsonString();
				}
				else
				{
					try
					{
						result = check.Check().ToJsonString();
					}
					catch (Exception ex)
					{
						result = new CheckResult
						{
							Status = CheckResultStatus.Failed,
							Message = ex.Message
						}.ToJsonString();
					}
				}
			}
			return result;
		}
		public static string Rectify(string id)
		{
			bool arg_41_0;
			if (helper.GetCurrentUmbracoUser() != null)
			{
				arg_41_0 = helper.GetCurrentUmbracoUser().get_Applications().Any((Application x) => x.get_alias() == "developer");
			}
			else
			{
				arg_41_0 = false;
			}
			string result;
			if (!arg_41_0)
			{
				result = new RectifyResult
				{
					Status = RectifyResultStatus.Failed,
					Message = "User is not authorized to perform checks."
				}.ToJsonString();
			}
			else
			{
				Guid checkId = new Guid(id);
				ICheck check = CheckFactory.Checks.SingleOrDefault((ICheck x) => x.Id == checkId);
				if (check == null)
				{
					result = new RectifyResult
					{
						Status = RectifyResultStatus.Failed,
						Message = string.Format("Unabled to find check with id '{0}'", id)
					}.ToJsonString();
				}
				else
				{
					if (!check.CanRectify)
					{
						result = new RectifyResult
						{
							Status = RectifyResultStatus.Failed,
							Message = string.Format("The check '{0}' can not be automatically rectified (check id: '{1}')", check.Name, id)
						}.ToJsonString();
					}
					else
					{
						try
						{
							result = check.Rectify().ToJsonString();
						}
						catch (Exception ex)
						{
							result = new RectifyResult
							{
								Status = RectifyResultStatus.Failed,
								Message = ex.Message
							}.ToJsonString();
						}
					}
				}
			}
			return result;
		}
	}
}
