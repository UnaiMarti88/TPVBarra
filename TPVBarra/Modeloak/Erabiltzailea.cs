using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class Erabiltzailea
    {
        public virtual int id { get; set; }
        public virtual string erabiltzailea { get; set; }
        public virtual string emaila { get; set; }
        public virtual string pasahitza { get; set; }
        public virtual Rola rola { get; set; }
        public virtual bool ezabatua { get; set; }
        public virtual bool aktibatuta { get; set; }

    }
}
