using System;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.media;
using umbraco.cms.businesslogic.web;
using umbraco.presentation.channels.businesslogic;
namespace Runway.Blog.EventHandlers
{
    public class AutoChannelCreator : umbraco.businesslogic.ApplicationStartupHandler
	{
		private void Document_AfterPublish(Document sender, PublishEventArgs e)
		{
			if (sender.ContentType.Alias == "umbBlog")
			{
				if (sender.getProperty("owner") != null && !string.IsNullOrEmpty(sender.getProperty("owner").Value.ToString()))
				{
					int num = -1;
					System.Web.HttpContext.Current.Response.Write("meh " + sender.getProperty("owner").Value.ToString());
					if (int.TryParse(sender.getProperty("owner").Value.ToString(), out num))
					{
						if (num >= 0)
						{
							User user = User.GetUser(num);
							if (user != null && !user.Disabled)
							{
								Channel channel = null;
								try
								{
									channel = new Channel(user.Id);
								}
								catch
								{
									channel = null;
								}
								if (channel == null)
								{
									channel = new Channel();
									channel.Name = sender.Text;
									channel.User = user;
									channel = new Channel();
									channel.DocumentTypeAlias = "umbBlogPost";
									channel.FieldCategoriesAlias = "tags";
									channel.FieldDescriptionAlias = "bodyText";
									channel.FieldExcerptAlias = "";
									channel.FullTree = true;
									channel.StartNode = sender.Id;
									Media media = Media.MakeNew(user.Name + "'s blog photos", MediaType.GetByAlias("Folder"), user, user.StartMediaId);
									media.Save();
									channel.ImageSupport = true;
									channel.MediaFolder = media.Id;
									channel.MediaTypeAlias = "Image";
									channel.MediaTypeFileProperty = "umbracoFile";
									channel.Save();
								}
							}
						}
					}
				}
			}
		}
	}
}
