using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class ProduktuOsagaia
    {
        public virtual int id { get; set; }
        public virtual Produktua produktua { get; set; }
        public virtual Osagaia osagaia { get; set; }
        public virtual double kantitatea { get; set; }
    }
}
