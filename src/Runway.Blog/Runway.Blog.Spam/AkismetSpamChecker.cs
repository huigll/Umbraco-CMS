using Joel.Net;
using System;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.presentation.nodeFactory;
namespace Runway.Blog.Spam
{
	public class AkismetSpamChecker : SpamChecker
	{
		public AkismetSpamChecker()
		{
			base.ProviderName = "Akismet";
		}
		public override bool Check(int nodeid, string UserAgent, string UserIp, string Author, string AuthorEmail, string AuthorUrl, string Content)
		{
			Akismet akismet = this.Initialize(nodeid);
			return akismet != null && akismet.CommentCheck(new AkismetComment
			{
				UserAgent = UserAgent, 
				UserIp = UserIp, 
				CommentType = "comment", 
				CommentAuthor = Author, 
				CommentAuthorEmail = AuthorEmail, 
				CommentAuthorUrl = AuthorUrl, 
				CommentContent = Content
			});
		}
		public override void MarkAsHam(int nodeid, string Author, string AuthorEmail, string AuthorUrl, string Content)
		{
			Akismet akismet = this.Initialize(nodeid);
			if (akismet != null)
			{
				akismet.SubmitHam(new AkismetComment
				{
					CommentType = "comment", 
					CommentAuthor = Author, 
					CommentAuthorEmail = AuthorEmail, 
					CommentAuthorUrl = AuthorUrl, 
					CommentContent = Content
				});
			}
		}
		public override void MarkAsSpam(int nodeid, string Author, string AuthorEmail, string AuthorUrl, string Content)
		{
			Akismet akismet = this.Initialize(nodeid);
			if (akismet != null)
			{
				akismet.SubmitSpam(new AkismetComment
				{
					CommentType = "comment", 
					CommentAuthor = Author, 
					CommentAuthorEmail = AuthorEmail, 
					CommentAuthorUrl = AuthorUrl, 
					CommentContent = Content
				});
			}
		}
		private Akismet Initialize(int nodeid)
		{
			Node node = new Node(nodeid);
			while (node.NodeTypeAlias != "Blog")
			{
				node = node.Parent;
			}
			Akismet result;
			if (node.GetProperty("akismetAPIKey").Value != string.Empty)
			{
				Akismet akismet = new Akismet(node.GetProperty("akismetAPIKey").Value, "http://" + System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + node.Url, "Blog4Umbraco2");
				if (!akismet.VerifyKey())
				{
					Log.Add(LogTypes.Error, -1, "Akismet Key could not be verified, please check if you have a valid Akismet API Key");
					result = null;
				}
				else
				{
					result = akismet;
				}
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
