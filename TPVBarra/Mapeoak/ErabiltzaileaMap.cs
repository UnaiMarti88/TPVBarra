using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVBarra.Modeloak;
using FluentNHibernate.Mapping;

namespace TPVBarra.Mapeoak
{
    internal class ErabiltzaileaMap : ClassMap<Erabiltzailea>
    {
        public ErabiltzaileaMap()
        {
            Table("erabiltzaileak");
            Id(x => x.id).Column("ID").GeneratedBy.Identity();
            Map(x => x.izena).Column("izena").Length(45);
            Map(x => x.emiala).Column("email").Length(100);
            Map(x => x.pasahitza).Column("pasahitza").Length(45);
            References(x => x.rola).Column("rola_id").Not.Nullable();
            Map(x => x.ezabatua).Column("ezabatua");
        }
    }
}
