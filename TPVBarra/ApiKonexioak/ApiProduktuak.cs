using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVBarra.DTOak;
using Newtonsoft.Json;

namespace TPVBarra.ApiKonexioak
{
    internal class ApiProduktuak
    {
        public async Task<List<ProduktuaDTO>> LortuProduktuakKategoriagatik(int kategoriaId)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7236/api/ProduktuakKontrollera/kategoria/{kategoriaId}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProduktuaDTO>>(json);
        
        }
    }
}
