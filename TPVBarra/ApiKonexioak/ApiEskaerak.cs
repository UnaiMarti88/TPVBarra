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
        public async Task<ErantzunaDTO<String>> SortuEskaeraAsync(int idLogina, List<EskaeraProduktuaDTO> produktuak, int mahaiaId, int komentsalak)
        {
            using var client = new HttpClient();

            var dto = new
            {
                ErabiltzaileId = idLogina,
                MahaiaId = mahaiaId,
                Komensalak = komentsalak,
                SukaldeaEgoera = "zain",
                Produktuak = produktuak
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7236/api/eskaerak", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<String>>(responseJson);
            return erantzuna;
        }

        public async Task<int> LortuMahaiKapasitateaAsync(int mahaiaId)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync(
                $"https://localhost:7236/api/eskaerak/mahaiak/{mahaiaId}/kapazitatea"
            );

            var responseJson = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<int>>(responseJson);

            if (erantzuna == null)
                throw new Exception("Errorea: erantzuna hutsik");

            if (erantzuna.Code != 200)
                throw new Exception(erantzuna.Message);

            if (erantzuna.Datuak == null || !erantzuna.Datuak.Any())
                throw new Exception("Mahaiak ez du kapazitaterik");

            return erantzuna.Datuak.First();
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

        public async Task<ErantzunaDTO<EskaeraLortuDTO>> LortuEskaeraProduktuakAsync(int eskaeraId)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7236/api/eskaerak/{eskaeraId}/produktuak");
            var json = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<EskaeraLortuDTO>>(json);

            if (erantzuna?.Datuak == null)
            {
                erantzuna = new ErantzunaDTO<EskaeraLortuDTO>
                {
                    Code = (int)response.StatusCode,
                    Message = "Errorea zerbitzarian",
                    Datuak = new List<EskaeraLortuDTO>()
                };
            }

            return erantzuna;
        }

        public async Task<ErantzunaDTO<string>> EzabatuEskaeraAsync(int eskaeraId)
        {
            using var client = new HttpClient();

            try
            {
                var response = await client.DeleteAsync($"https://localhost:7236/api/eskaerak/{eskaeraId}");
                var json = await response.Content.ReadAsStringAsync();

                var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<string>>(json);

                if (erantzuna == null)
                {
                    erantzuna = new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Errorea zerbitzarian",
                        Datuak = new List<string>()
                    };
                }

                return erantzuna;
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<string>()
                };
            }
        }

        public async Task<ErantzunaDTO<string>> EguneratuEskaeraAsync(int eskaeraId, List<EskaeraProduktuaEditatuDTO> produktuak)
        {
            using var client = new HttpClient();

            if (produktuak == null || !produktuak.Any())
            {
                return new ErantzunaDTO<string>
                {
                    Code = 400,
                    Message = "Ez duzu produkturik bidali",
                    Datuak = new List<string>()
                };
            }

            var json = JsonConvert.SerializeObject(produktuak);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PutAsync($"https://localhost:7236/api/eskaerak/{eskaeraId}", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseJson))
                {
                    return new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Ez dago erantzunik",
                        Datuak = new List<string>()
                    };
                }

                var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<string>>(responseJson);

                if (erantzuna == null)
                {
                    erantzuna = new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Errorea zerbitzarian",
                        Datuak = new List<string>()
                    };
                }

                if (erantzuna.Datuak == null)
                    erantzuna.Datuak = new List<string>();

                return erantzuna;
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<string>()
                };
            }
        }

        public async Task<ErantzunaDTO<string>> OrdainduEskaeraAsync(int eskaeraId)
        {
            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsync(
                    $"https://localhost:7236/api/eskaerak/{eskaeraId}/ordainduEskaera",
                    null
                );

                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Ez da erantzunik jaso",
                        Datuak = new List<string>()
                    };
                }

                var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<string>>(json);

                if (erantzuna == null)
                {
                    return new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Errorea zerbitzarian",
                        Datuak = new List<string>()
                    };
                }

                return erantzuna;
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = "Errorea ordaintzera bidaltzean: " + ex.Message,
                    Datuak = new List<string>()
                };
            }
        }

        public async Task<ErantzunaDTO<string>> SortuFakturaAsync(int eskaeraId)
        {
            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsync(
                    $"https://localhost:7236/api/eskaerak/{eskaeraId}/sortuFaktura",
                    null
                );

                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json))
                    return new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Ez du erantzunik jaso zerbitzaritik",
                        Datuak = new List<string>()
                    };

                var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<string>>(json);

                if (erantzuna == null)
                    return new ErantzunaDTO<string>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Arazoa zerbitzariarekin",
                        Datuak = new List<string>()
                    };

                return erantzuna;
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = "Arazoa faktura sortzean: " + ex.Message,
                    Datuak = new List<string>()
                };
            }
        }

        public async Task<ErantzunaDTO<EskaeraDTO>> LortuEskaerakOrdaintzekoAsync()
        {
            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync("https://localhost:7236/api/eskaerak/ordainketa-pendiente");
                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json))
                    return new ErantzunaDTO<EskaeraDTO>
                    {
                        Code = (int)response.StatusCode,
                        Message = "Ez da zerbitzariaren erantzunik jaso",
                        Datuak = new List<EskaeraDTO>()
                    };

                var erantzuna = JsonConvert.DeserializeObject<ErantzunaDTO<EskaeraDTO>>(json);

                return erantzuna ?? new ErantzunaDTO<EskaeraDTO>
                {
                    Code = (int)response.StatusCode,
                    Message = "Arazoa zerbitzarian",
                    Datuak = new List<EskaeraDTO>()
                };
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<EskaeraDTO>
                {
                    Code = 500,
                    Message = "Arazoa eskaerak jasotzean: " + ex.Message,
                    Datuak = new List<EskaeraDTO>()
                };
            }
        }

    }
}
