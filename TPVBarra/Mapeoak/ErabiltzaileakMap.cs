using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVBarra.Modeloak;
using FluentNHibernate.Mapping;

namespace TPVBarra.Mapeoak
{
    internal class ErabiltzaileakMap : ClassMap<Erabiltzailea>
    {
        public ErabiltzaileakMap()
        {
            Table("erabiltzaileak");
            Id(x => x.Id).Column("ID").GeneratedBy.Identity();
            Map(x => x.Izena).Column("izena").Length(45);
            Map(x => x.Pasahitza).Column("pasahitza").Length(45);
        }
    }
}
