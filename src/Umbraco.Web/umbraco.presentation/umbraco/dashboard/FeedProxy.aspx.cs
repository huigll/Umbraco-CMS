﻿using Umbraco.Core.Logging;

namespace dashboardUtilities
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using umbraco;
    using umbraco.BasePages;
    using umbraco.BusinessLogic;
    
    public partial class FeedProxy : UmbracoEnsuredPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            return;
            try
            {
                if (Request.QueryString.AllKeys.Contains("url") && Request.QueryString["url"] != null)
                {
                    var url = Request.QueryString["url"];
                    if (!string.IsNullOrWhiteSpace(url) && !url.StartsWith("/"))
                    {
                        Uri requestUri;
                        if (Uri.TryCreate(url, UriKind.Absolute, out requestUri))
                        {
                            var feedProxyXml = Umbraco.Core.XmlHelper.OpenAsXmlDocument(Umbraco.Core.IO.IOHelper.MapPath(Umbraco.Core.IO.SystemFiles.FeedProxyConfig));
                            if (feedProxyXml != null && feedProxyXml.SelectSingleNode(string.Concat("//allow[@host = '", requestUri.Host, "']")) != null)
                            {
                                using (var client = new WebClient())
                                {
                                    var response = client.DownloadString(requestUri);

                                    if (!string.IsNullOrEmpty(response))
                                    {
                                        Response.Clear();
                                        Response.ContentType = Request.QueryString["type"] ?? MediaTypeNames.Text.Xml;
                                        Response.Write(response);
                                    }
                                }
                            }
                            else
                            {
                                LogHelper.Debug<FeedProxy>(string.Format("Access to unallowed feedproxy attempted: {0}", requestUri));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<FeedProxy>("Exception occurred", ex);
            }
        }
    }
}