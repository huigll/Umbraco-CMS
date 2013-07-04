using Runway.Blog.Interfaces;
using System;
namespace Runway.Blog.Spam
{
	public abstract class SpamChecker : ISpamChecker
	{
		private string m_providername;
		public string ProviderName
		{
			get
			{
				return this.m_providername;
			}
			set
			{
				this.m_providername = value;
			}
		}
		public virtual bool Check(int nodeid, string UserAgent, string UserIp, string Author, string AuthorEmail, string AuthorUrl, string Content)
		{
			return false;
		}
		public virtual void MarkAsHam(int nodeid, string Author, string AuthorEmail, string AuthorUrl, string Content)
		{
		}
		public virtual void MarkAsSpam(int nodeid, string Author, string AuthorEmail, string AuthorUrl, string Content)
		{
		}
	}
}
