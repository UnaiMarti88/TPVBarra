using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

            var response = await client.GetAsync("http://192.168.1.10:5093/api/mahaiak/libre");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<MahaiaDTO>();
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<MahaiaDTO>>(json);

            if (erantzuna == null || erantzuna.Datuak == null)
                return new List<MahaiaDTO>();

            return erantzuna.Datuak;
        }
    }
}
