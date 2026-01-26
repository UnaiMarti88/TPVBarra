using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class Eskaera
    {
        public virtual int id { get; set; }
        public virtual Mahaia mahaia { get; set; }
        public virtual Erabiltzailea erabiltzailea { get; set; }
        public virtual int komentsalak { get; set; }
        public virtual string egoera { get; set; }
        public virtual string sukaldeaEgoera { get; set; }
        public virtual DateTime sortzeData { get; set; } = DateTime.Now;
        public virtual DateTime itxieraData { get; set; }
        public virtual Erreserba erreserba { get; set; }
    }
}
