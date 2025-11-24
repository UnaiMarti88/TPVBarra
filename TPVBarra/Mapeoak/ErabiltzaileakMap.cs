using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolaNHibernate.Modeloak;
using FluentNHibernate.Mapping;

namespace TPVBarra.Mapeoak
{
    internal class ErabiltzaileakMap
    {
        public ErabiltzaileakMap()
        {
            Table("ERABILTZAILEAK");
            Id(x => x.Id).Column("ID").GeneratedBy.Identity();
            Map(x => x.Izena).Column("izena").Lengh(45);
            Map(x => x.Pasahitza).Column("pasahitza").Lengh(45);
        }
    }
}
