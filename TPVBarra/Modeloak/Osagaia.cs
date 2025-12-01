using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.Modeloak
{
    internal class Osagaia
    {
        public virtual int id { get; set; }
        public virtual string izena { get; set; }
        public virtual string unitatea { get; set; }
        public virtual double stocka { get; set; }
    }
}
