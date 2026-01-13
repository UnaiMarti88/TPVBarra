using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TPVBarra.DTOak;

namespace TPVBarra.ApiKonexioak
{
    public class ApiEskaerak
    {
        public async Task<ErantzunaDTO> SortuEskaeraAsync(int idLogina, List<EskaeraProduktuaDTO> produktuak, int mahaiaId)
        {
            using var client = new HttpClient();

            var dto = new
            {
                ErabiltzaileId = idLogina,
                MahaiaId = mahaiaId,
                Komensalak = 4,
                Produktuak = produktuak
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7236/api/eskaerak", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO>(responseJson);
            return erantzuna;
        }
    }
}
