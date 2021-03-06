﻿using System;
using System.Linq;
using System.Web.UI;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Macros;
using Umbraco.Core.ObjectResolution;
using Umbraco.Tests.TestHelpers;
using umbraco.interfaces;

namespace Umbraco.Tests.Resolvers
{
	[TestFixture]
	public class MacroFieldEditorsResolverTests
	{
		[SetUp]
		public void Initialize()
		{
            TestHelper.SetupLog4NetForTests();

            MacroFieldEditorsResolver.Reset();


			// this ensures it's reset
			PluginManager.Current = new PluginManager(false);

			// for testing, we'll specify which assemblies are scanned for the PluginTypeResolver
			PluginManager.Current.AssembliesToScan = new[]
				{
					this.GetType().Assembly
				};
		}

		[TearDown]
		public void TearDown()
		{
            MacroFieldEditorsResolver.Reset();
            PluginManager.Current = null;
		}

        // NOTE
        // ManyResolverTests ensure that we'll get our actions back and MacroFieldEditorsResolver works,
        // so all we're testing here is that plugin manager _does_ find our macro control types
        // which should be ensured by PlugingManagerTests anyway, so this is useless?
        [Test]
		public void FindAllTypes()
		{
            MacroFieldEditorsResolver.Current = new MacroFieldEditorsResolver(
                () => PluginManager.Current.ResolveMacroRenderings());

            Resolution.Freeze();
            
            var types = MacroFieldEditorsResolver.Current.MacroControlTypes;
			Assert.AreEqual(2, types.Count());

            // order is unspecified, but both must be there
            bool hasType1 = types.ElementAt(0) == typeof(ControlMacroRendering) || types.ElementAt(1) == typeof(ControlMacroRendering);
            bool hasType2 = types.ElementAt(0) == typeof(NonControlMacroRendering) || types.ElementAt(1) == typeof(NonControlMacroRendering);
            Assert.IsTrue(hasType1);
            Assert.IsTrue(hasType2);
        }

		#region Classes for tests

		public class ControlMacroRendering : Control, IMacroGuiRendering
		{
			public string Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public bool ShowCaption
			{
				get { throw new NotImplementedException(); }
			}
		}

		public class NonControlMacroRendering : IMacroGuiRendering
		{
			public string Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public bool ShowCaption
			{
				get { throw new NotImplementedException(); }
			}
		}

        #endregion
	}
}