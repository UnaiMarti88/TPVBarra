using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPVBarra.DTOak;

namespace TPVBarra.ApiKonexioak
{
    internal class ApiMahaiak
    {
        public async Task<List<MahaiaDTO>> LortuMahaiLibreAsync()
        {
            using var client = new HttpClient();

            var response = await client.GetAsync("https://localhost:7236/api/mahaiak/libre");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<MahaiaDTO>>(json);

            if (erantzuna == null || erantzuna.Datuak == null)
                return new List<MahaiaDTO>();

            return erantzuna.Datuak;
        }
    }
}
