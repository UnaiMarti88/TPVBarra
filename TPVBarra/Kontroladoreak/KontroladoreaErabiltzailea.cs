using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using TPVBarra.Modeloak;

namespace TPVBarra.Kontroladoreak
{
    internal class KontroladoreaErabiltzailea
    {

        private readonly ISessionFactory _sessionFactory;
        public KontroladoreaErabiltzailea(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public Erabiltzailea saioaHasi(String izena, String pasahitza)
        {

            using (var session = _sessionFactory.OpenSession())
            {
                var erabiltzailea = session.Query<Erabiltzailea>()
                    .FirstOrDefault(e => e.erabiltzailea == izena && e.pasahitza == pasahitza);
                return erabiltzailea;
            }
        }
    }
}
