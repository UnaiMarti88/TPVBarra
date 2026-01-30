using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TPVBarra.DTOak;
using TPVBarra.Modeloak;

namespace TPVBarra.ApiKonexioak
{
    internal class ApiLogina
    {
        private static readonly HttpClient client;

        static ApiLogina()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            client = new HttpClient(handler);
        }

        public async Task<Erabiltzailea?> LoginAsync(string erabiltzailea, string pasahitza)
        {
            var body = new
            {
                erabiltzailea,
                pasahitza
            };

            String json = JsonSerializer.Serialize(body);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.PostAsync("http://192.168.1.10:5093/api/Logina", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            String jsonResponse = await response.Content.ReadAsStringAsync();

            var erantzuna = JsonSerializer.Deserialize<ErantzunaDTO<Erabiltzailea>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return erantzuna?.Datuak?.FirstOrDefault();
        }
    }
}