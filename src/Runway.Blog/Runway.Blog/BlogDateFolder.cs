using System;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using umbraco.presentation.nodeFactory;
namespace Runway.Blog
{
	public class BlogDateFolder : umbraco.businesslogic.ApplicationStartupHandler// ApplicationBase
	{
		public BlogDateFolder()
		{
			Document.New += new Document.NewEventHandler(this.Document_New);
			Document.BeforePublish += new Document.PublishEventHandler(this.Document_BeforePublish);
		}
		private void Document_New(Document sender, NewEventArgs e)
		{
			if (sender.ContentType.Alias == "umbBlogPost")
			{
				if (sender.getProperty("PostDate") != null)
				{
					sender.getProperty("PostDate").Value = sender.CreateDateTime.Date;
				}
			}
		}
		private void Document_BeforePublish(Document sender, PublishEventArgs e)
		{
			if (sender.ContentType.Alias == "umbBlogPost")
			{
				Log.Add(LogTypes.Debug, sender.User, sender.Id, string.Format("Start Before Publish Event for Blog Post {0}", sender.Id));
				if (sender.getProperty("PostDate") != null)
				{
					if (sender.Parent != null)
					{
						try
						{
							DocumentVersionList[] versions = sender.GetVersions();
							bool flag = true;
							System.DateTime d = System.Convert.ToDateTime(sender.getProperty("PostDate").Value);
							if (versions.Length > 1)
							{
								System.Guid version = versions[versions.Length - 2].Version;
								Document document = new Document(sender.Id, version);
								System.DateTime d2 = System.Convert.ToDateTime(document.getProperty("PostDate").Value);
								flag = (d != d2);
							}
							if (flag)
							{
								string[] array = new string[3];
								string[] arg_10D_0 = array;
								int arg_10D_1 = 0;
								int num = d.Year;
								arg_10D_0[arg_10D_1] = num.ToString();
								string[] arg_121_0 = array;
								int arg_121_1 = 1;
								num = d.Month;
								arg_121_0[arg_121_1] = num.ToString();
								string[] arg_135_0 = array;
								int arg_135_1 = 2;
								num = d.Day;
								arg_135_0[arg_135_1] = num.ToString();
								string[] array2 = array;
								if (array2.Length == 3)
								{
									Node node = new Node(sender.Parent.Id);
									while (node != null && node.NodeTypeAlias != "umbBlog")
									{
										if (node.Parent != null)
										{
											node = new Node(node.Parent.Id);
										}
										else
										{
											node = null;
										}
									}
									if (node != null)
									{
										Node node2 = null;
										foreach (Node node3 in node.Children)
										{
											if (node3.Name == array2[0])
											{
												node2 = new Node(node3.Id);
												Document document2 = new Document(node3.Id);
												break;
											}
										}
										if (node2 == null)
										{
											Document document2 = Document.MakeNew(array2[0], DocumentType.GetByAlias("DateFolder"), sender.User, node.Id);
											document2.getProperty("umbracoNaviHide").Value = 1;
											document2.Publish(sender.User);
											library.UpdateDocumentCache(document2.Id);
											node2 = new Node(document2.Id);
										}
										Node node4 = null;
										foreach (Node node3 in node2.Children)
										{
											if (node3.Name == array2[1])
											{
												node4 = new Node(node3.Id);
												break;
											}
										}
										if (node4 == null)
										{
											Document document3 = Document.MakeNew(array2[1], DocumentType.GetByAlias("DateFolder"), sender.User, node2.Id);
											document3.getProperty("umbracoNaviHide").Value = 1;
											document3.Publish(sender.User);
											library.UpdateDocumentCache(document3.Id);
											node4 = new Node(document3.Id);
										}
										Document document4 = null;
										foreach (Node node3 in node4.Children)
										{
											if (node3.Name == array2[2])
											{
												document4 = new Document(node3.Id);
												break;
											}
										}
										if (document4 == null)
										{
											document4 = Document.MakeNew(array2[2], DocumentType.GetByAlias("DateFolder"), sender.User, node4.Id);
											document4.getProperty("umbracoNaviHide").Value = 1;
											document4.Publish(sender.User);
											library.UpdateDocumentCache(document4.Id);
										}
										if (sender.Parent.Id != document4.Id)
										{
											sender.Move(document4.Id);
											Log.Add(LogTypes.Debug, sender.User, sender.Id, string.Format("Move Required for BlogPost {0} for PostDate {1}.  Moved Under Node {2}", sender.Id, d.ToShortDateString(), document4.Id));
										}
									}
									else
									{
										Log.Add(LogTypes.Debug, sender.User, sender.Id, string.Format("Unable to determine top of Blog for BlogPost {0} while attempting to move to new Post Date", sender.Id));
									}
								}
							}
						}
						catch (System.Exception ex)
						{
							Log.Add(LogTypes.Debug, sender.User, sender.Id, string.Format("Error while Finding Blog Folders for BlogPost {0} while trying to move to new Post Date.  Error: {1}", sender.Id, ex.Message));
						}
						library.RefreshContent();
					}
				}
			}
		}
	}
}
