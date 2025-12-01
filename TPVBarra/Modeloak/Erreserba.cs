using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class Erreserba
    {
        public virtual int id { get; set; }
        public virtual Mahaia mahaia { get; set; }
        public virtual string bezeroIzena { get; set; }
        public virtual string telefonoa { get; set; }
        public virtual DateTime data { get; set; }
        public virtual string txanda { get; set; }
        public virtual int pertsonaKopurua { get; set; }
        public virtual string egoera { get; set; }

    }
}
