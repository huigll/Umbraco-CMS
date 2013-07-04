#if DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Joel.Net {

    [TestFixture()]
    public class AkismetTest {

        const string key = "8b8d8d8c375d";
        const string blog = "http://your.url.here.com";

        Akismet api;

        [SetUp]
        public void Setup() {
             api = new Akismet(key, blog, null);
        }

        [Test()]
        public void Verification() {
            Assert.IsTrue(api.VerifyKey(), "VerifyKey() returned 'False' when 'True' was expected.");
        }

        [Test()]
        public void SpamTest() {
            
            AkismetComment comment = new AkismetComment();
            comment.Blog = blog;
            comment.UserIp = "147.202.45.202";
            comment.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
            comment.CommentContent = "A friend of mine told me about this place. I'm just wondering if this thing works.   You check this posts everyday incase <a href=\"http://someone.finderinn.com\">find someone</a> needs some help?  I think this is a great job! Really nice place http://someone.finderinn.com here.   I found a lot of interesting stuff all around.  I enjoy beeing here and i'll come back soon. Many greetings.";
            comment.CommentType = "blog";
            comment.CommentAuthor = "someone";
            comment.CommentAuthorEmail = "backthismailtojerry@fastmail.fm";
            comment.CommentAuthorUrl = "http://someone.finderinn.com";

            bool result = api.CommentCheck(comment);

            Assert.IsTrue(result, "API was expected to return 'True' when 'False' was returned instead.");
        }

        [Test()]
        public void NonSpamTest() {
            
            AkismetComment comment = new AkismetComment();
            comment.Blog = blog;
            comment.UserIp = "127.0.0.1";
            comment.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            comment.CommentContent = "Hey, I'm testing out the Akismet API!";
            comment.CommentType = "blog";
            comment.CommentAuthor = "Joel";
            comment.CommentAuthorEmail = "";
            comment.CommentAuthorUrl = "";

            bool result = api.CommentCheck(comment);

            Assert.IsFalse(result, "API was expected to return 'False' when 'True' was returned instead.");
        }

        [Test(), Ignore("Pollution")]
        public void SubmitSpam() {

            AkismetComment comment = new AkismetComment();
            comment.Blog = blog;
            comment.UserIp = "147.202.45.202";
            comment.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
            comment.CommentContent = "A friend of mine told me about this place. I'm just wondering if this thing works.   You check this posts everyday incase <a href=\"http://someone.finderinn.com\">find someone</a> needs some help?  I think this is a great job! Really nice place http://someone.finderinn.com here.   I found a lot of interesting stuff all around.  I enjoy beeing here and i'll come back soon. Many greetings.";
            comment.CommentType = "blog";
            comment.CommentAuthor = "someone";
            comment.CommentAuthorEmail = "backthismailtojerry@fastmail.fm";
            comment.CommentAuthorUrl = "http://someone.finderinn.com";

            api.SubmitSpam(comment);
        }

        [Test(), Ignore("Pollution")]
        public void SubmitHam() {

            AkismetComment comment = new AkismetComment();
            comment.Blog = blog;
            comment.UserIp = "127.0.0.1";
            comment.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            comment.CommentContent = "Hey, I'm testing out the Akismet API!";
            comment.CommentType = "blog";
            comment.CommentAuthor = "Joel";
            comment.CommentAuthorEmail = "";
            comment.CommentAuthorUrl = "";

            api.SubmitHam(comment);
        }
    }
}
#endif