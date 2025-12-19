using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPVBarra.DTOak
{
    internal class ProduktuaDTO
    {
        public int id { get; set; }
        public string izena { get; set; }
        public decimal prezioa { get; set; }
        public int kategoria_id { get; set; }
        public int stock_aktuala { get; set; }
    }
}
