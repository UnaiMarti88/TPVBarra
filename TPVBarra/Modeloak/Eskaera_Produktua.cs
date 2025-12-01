using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class Eskaera_Produktua
    {
        public virtual int id { get; set; }
        public virtual Eskaera eskaera { get; set; }
        public virtual Produktua produktua { get; set; }
        public virtual int kantitatea { get; set; }
        public virtual double prezioa { get; set; }
        public virtual double guztira { get; set; }
    }
}
