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
        public virtual string izena { get; set; }
        public virtual string emiala { get; set; }
        public virtual string pasahitza { get; set; }
        public virtual Rola rola { get; set; }
        public virtual bool ezabatua { get; set; }

        public Erabiltzailea() { }

        public Erabiltzailea(string izena, string pasahitza) { 
            this.izena = izena;
            this.pasahitza = pasahitza;
        }

        public Erabiltzailea(int id, string izena, string emaila, string pasahitza, Rola rola, bool ezabatua) {
            this.id = id;
            this.izena = izena;
            this.emiala = emaila;
            this.pasahitza = pasahitza;
            this.rola = rola;
            this.ezabatua = ezabatua;
        }
    }
}
