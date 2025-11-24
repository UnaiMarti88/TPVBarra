using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra
{

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Tool.hbm2ddl;
    //using ConsolaNHibernate.Mapeoak;
    using TPVBarra.Mapeoak;

    internal class NHibernateHelper
    {

        public static class NHibernateHelperra
        {
            private static ISessionFactory _sessionFactory;

            public static ISessionFactory SessionFactory
            {
                get
                {
                    if (_sessionFactory == null)
                        InitializeSessionFactory();
                    return _sessionFactory;
                }
            }

            private static void InitializeSessionFactory()
            {
                _sessionFactory = Fluently.Configure()
                    .Database(
                        MySQLConfiguration.Standard
                            .ConnectionString(cs => cs
                                .Server("localhost")
                                .Database("hibernateprobak")
                                .Username("root")
                                .Password("1MG2024")
                            )
                    )
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<ErabiltzaileakMap>();
                    })
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true)) // Ez dezan sortu taula exekuzio bakoitzeko
                    .BuildSessionFactory();
            }

            public static ISession OpenSession()
            {
                return SessionFactory.OpenSession();
            }
        }
    }

