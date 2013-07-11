using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.ObjectResolution;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Tests.TestHelpers;
using umbraco.interfaces;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;


namespace Umbraco.Tests.BootManagers
{
    [TestFixture]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass()]
    public class CoreBootManagerTests : BaseUmbracoApplicationTest
    {

        private TestApp _testApp;

        [SetUp]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _testApp = new TestApp();
        }

        [TearDown]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanup]
        public override void TearDown()
        {
            base.TearDown();

            _testApp = null;
            
            //ApplicationEventsResolver.Reset();
            //SqlSyntaxProvidersResolver.Reset();
        }

        protected override void FreezeResolution()
        {
            //don't freeze resolution, we'll do that in the boot manager
        }

        /// <summary>
        /// test application using a CoreBootManager instance to boot
        /// </summary>
        public class TestApp : UmbracoApplicationBase
        {
            protected override IBootManager GetBootManager()
            {
                return new TestBootManager(this);
            }
        }

        /// <summary>
        /// Test boot manager to add a custom application event handler
        /// </summary>
        public class TestBootManager : CoreBootManager
        {
            public TestBootManager(UmbracoApplicationBase umbracoApplication)
                : base(umbracoApplication)
            {
            }

            protected override void InitializeApplicationEventsResolver()
            {
                //create an empty resolver so we can add our own custom ones (don't type find)
                ApplicationEventsResolver.Current = new ApplicationEventsResolver(new Type[]
                    {
                        typeof(LegacyStartupHandler),
                        typeof(TestApplicationEventHandler)
                    })
                    {
                        CanResolveBeforeFrozen = true
                    };
            }

            protected override void InitializeResolvers()
            {
                //Do nothing as we don't want to initialize all resolvers in this test
                //We only include this resolver to not cause trouble for the database context
                SqlSyntaxProvidersResolver.Current = new SqlSyntaxProvidersResolver(
                    PluginManager.Current.ResolveSqlSyntaxProviders())
                                                         {
                                                             CanResolveBeforeFrozen = true
                                                         };
            }
        }

        /// <summary>
        /// Test legacy startup handler
        /// </summary>
        public class LegacyStartupHandler : IApplicationStartupHandler
        {
            public static bool Initialized = false;

            public LegacyStartupHandler()
            {
                Initialized = true;
            }
        }

        /// <summary>
        /// test event handler
        /// </summary>
        public class TestApplicationEventHandler : IApplicationEventHandler
        {
            public static bool Initialized = false;
            public static bool Starting = false;
            public static bool Started = false;

            public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
            {
                Initialized = true;
            }

            public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
            {
                Starting = true;
            }

            public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
            {
                Started = true;
            }
        }

        [Test]
        public void Handle_IApplicationEventHandler_Objects_Outside_Web_Context()
        {
            _testApp.StartApplication(_testApp, new EventArgs());

            Assert.IsTrue(TestApplicationEventHandler.Initialized);
            Assert.IsTrue(TestApplicationEventHandler.Starting);
            Assert.IsTrue(TestApplicationEventHandler.Started);
        }

        [Test]
        public void Ensure_Legacy_Startup_Handlers_Not_Started_Until_Complete()
        {
            EventHandler starting = (sender, args) =>
                {
                    Assert.IsTrue(TestApplicationEventHandler.Initialized);
                    Assert.IsTrue(TestApplicationEventHandler.Starting);
                    Assert.IsFalse(LegacyStartupHandler.Initialized);
                };
            EventHandler started = (sender, args) =>
                {
                    Assert.IsTrue(TestApplicationEventHandler.Started);
                    Assert.IsTrue(LegacyStartupHandler.Initialized);
                };
            TestApp.ApplicationStarting += starting;
            TestApp.ApplicationStarted += started;

            _testApp.StartApplication(_testApp, new EventArgs());

            TestApp.ApplicationStarting -= starting;
            TestApp.ApplicationStarting -= started;

        }
        [Test]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        public void MyTest()
        {
            RepositoryFactory factor = new RepositoryFactory();
            PetaPocoUnitOfWorkProvider pp = new PetaPocoUnitOfWorkProvider();
            SqlSyntaxProvidersResolver.Current = new SqlSyntaxProvidersResolver(
              new List<Type> { typeof(MySqlSyntaxProvider), typeof(SqlCeSyntaxProvider), typeof(SqlServerSyntaxProvider) }) { CanResolveBeforeFrozen = true };
            SqlSyntaxContext.SqlSyntaxProvider = new MySqlSyntaxProvider();
            var content = factor.CreateContentRepository(pp.GetUnitOfWork());
            var aaa =content.Get(1064);
        }
    }
}
