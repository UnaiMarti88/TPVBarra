using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPVBarra.DTOak;
using Newtonsoft.Json;
using System.Net.Http;

namespace TPVBarra.ApiKonexioak
{
    public class ApiKategoriak
    {
        internal async Task<List<KategoriaDTO>> LortuKategoriak()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://192.168.1.10:5093/api/Kategoria");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var kategoriak = JsonConvert.DeserializeObject<List<KategoriaDTO>>(json);

            return kategoriak;
        }
    }
}
