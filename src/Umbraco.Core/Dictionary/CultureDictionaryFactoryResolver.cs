﻿using Umbraco.Core.ObjectResolution;

namespace Umbraco.Core.Dictionary
{
	/// <summary>
	/// Resolves the current CultureDictionaryFactory
	/// </summary>
	internal sealed class CultureDictionaryFactoryResolver : SingleObjectResolverBase<CultureDictionaryFactoryResolver, ICultureDictionaryFactory>
	{
		internal CultureDictionaryFactoryResolver(ICultureDictionaryFactory factory)
			: base(factory)
		{
		}

		/// <summary>
		/// Can be used by developers at runtime to set their ICultureDictionaryFactory at app startup
		/// </summary>
		/// <param name="factory"></param>
		public void SetContentStore(ICultureDictionaryFactory factory)
		{
			Value = factory;
		}

		/// <summary>
		/// Returns the ICultureDictionaryFactory
		/// </summary>
		public ICultureDictionaryFactory Factory
		{
			get { return Value; }
		}
	}
}