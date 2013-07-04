using System;
namespace Runway.Blog.Interfaces
{
	public interface ISpamChecker
	{
		string ProviderName
		{
			get;
			set;
		}
		bool Check(int nodeid, string UserAgent, string UserIp, string Author, string AuthorEmail, string AuthorUrl, string Content);
		void MarkAsHam(int nodeid, string Author, string AuthorEmail, string AuthorUrl, string Content);
		void MarkAsSpam(int nodeid, string Author, string AuthorEmail, string AuthorUrl, string Content);
	}
}
