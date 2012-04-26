using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace EPRacing.Web.Setup
{

    public class SetDependencyResolver : IBootstrapTask
    {
        public void Execute()
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }


        public object GetService(Type serviceType)
        {
            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return _container.TryGetInstance(serviceType);
            }
            return _container.GetInstance(serviceType);
        }


        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances<object>()
                .Where(s => s.GetType() == serviceType);
        }

        private readonly IContainer _container;
    }
    public class BootstrapStructureMap
    {
        public IContainer Setup()
        {
            ObjectFactory.Configure(x =>
            {
                x.AddRegistry<RavenRegistry>();
                x.AddRegistry<BootstrapperRegistry>();
            });

            return ObjectFactory.Container;
        }
    }

    public class BootstrapperRegistry : Registry
    {
        public BootstrapperRegistry()
        {
            Scan(scanner =>
                     {
                         scanner.TheCallingAssembly();
                         scanner.AddAllTypesOf<IBootstrapTask>();
                     });
        }
    }

    public class RavenRegistry : Registry
    {
        public RavenRegistry()
        {
            ObjectFactory.Configure(x => x.For<IRavenSessionFactoryBuilder>().Singleton()
                                             .Use<RavenSessionFactoryBuilder>());

            ObjectFactory.Configure(x => x.For<IDocumentSession>()
                                             .HybridHttpOrThreadLocalScoped()
                                             .AddInstances(inst => inst.ConstructedBy
                                                                       (context => context.GetInstance<IRavenSessionFactoryBuilder>()
                                                                                       .GetSessionFactory().CreateSession())));
        }
    }
    public interface IBootstrapTask
    {
        void Execute();
    }

    public class RavenSessionFactoryBuilder : IRavenSessionFactoryBuilder
    {
        private IRavenSessionFactory _ravenSessionFactory;

        public IRavenSessionFactory GetSessionFactory()
        {
            return _ravenSessionFactory ?? (_ravenSessionFactory = CreateSessionFactory());
        }

        private static IRavenSessionFactory CreateSessionFactory()
        {
            Debug.Write("IRavenSessionFactory Created");
            return new RavenSessionFactory(new DocumentStore { Url = "http://localhost:8080" });
        }
    }

    public interface IRavenSessionFactoryBuilder
    {
        IRavenSessionFactory GetSessionFactory();
    }

    public class RavenSessionFactory : IRavenSessionFactory
    {
        private readonly IDocumentStore _documentStore;

        public RavenSessionFactory(IDocumentStore documentStore)
        {
            if (_documentStore == null)
            {
                _documentStore = documentStore;
                _documentStore.Initialize();
            }
        }

        public IDocumentSession CreateSession()
        {
            Debug.Write("IDocumentSession Created");
            return _documentStore.OpenSession();
        }
    }
    public interface IRavenSessionFactory
    {
        IDocumentSession CreateSession();
    }
}