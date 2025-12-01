using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class Produktua
    {
        public virtual int id { get; set; }
        public virtual string izena { get; set; }
        public virtual Kategoria kategoria { get; set; }
        public virtual double prezioa { get; set; }
        public virtual int stocka { get; set; }
    }
}
