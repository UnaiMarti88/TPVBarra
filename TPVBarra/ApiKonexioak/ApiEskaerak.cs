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
        public async Task<ErantzunaDTO<String>> SortuEskaeraAsync(int idLogina, List<EskaeraProduktuaDTO> produktuak, int mahaiaId)
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

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<String>>(responseJson);
            return erantzuna;
        }

        public async Task<ErantzunaDTO<EskaeraDTO>> LortuEskaerakAsync(int erabiltzaileId)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7236/api/eskaerak?erabiltzaileId={erabiltzaileId}");
            var json = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<EskaeraDTO>>(json);

            if (erantzuna?.Datuak == null)
            {
                erantzuna = new ErantzunaDTO<EskaeraDTO>
                {
                    Code = (int)response.StatusCode,
                    Message = "Errorea zerbitzarian",
                    Datuak = new List<EskaeraDTO>()
                };
            }

            return erantzuna;
        }

        public async Task<ErantzunaDTO<EskaeraProduktuaDTO>> LortuEskaeraProduktuakAsync(int eskaeraId)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7236/api/eskaerak/{eskaeraId}/produktuak");
            var json = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<EskaeraProduktuaDTO>>(json);

            if (erantzuna?.Datuak == null)
            {
                erantzuna = new ErantzunaDTO<EskaeraProduktuaDTO>
                {
                    Code = (int)response.StatusCode,
                    Message = "Errorea zerbitzarian",
                    Datuak = new List<EskaeraProduktuaDTO>()
                };
            }

            return erantzuna;
        }

    }
}
