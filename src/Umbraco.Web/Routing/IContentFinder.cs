﻿namespace Umbraco.Web.Routing
{
	/// <summary>
	/// Provides a method to try to find and assign an Umbraco document to a <c>PublishedContentRequest</c>.
	/// </summary>
	public interface IContentFinder
    {
	    /// <summary>
	    /// Tries to find and assign an Umbraco document to a <c>PublishedContentRequest</c>.
	    /// </summary>
        /// <param name="contentRequest">The <c>PublishedContentRequest</c>.</param>
	    /// <returns>A value indicating whether an Umbraco document was found and assigned.</returns>
	    /// <remarks>Optionally, can also assign the template or anything else on the document request, although that is not required.</remarks>
	    bool TryFindContent(PublishedContentRequest contentRequest);
    }
}