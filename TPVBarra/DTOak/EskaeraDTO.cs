using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPVBarra.DTOak
{
    public class EskaeraDTO
    {
        private string _sukaldeaEgoera;

        public int Id { get; set; }
        public string Izena { get; set; }
        public int MahaiaId { get; set; }
        public string Data { get; set; }
        [JsonIgnore]
        public string SukaldeaEgoera
        {
            get => _sukaldeaEgoera;
            set => _sukaldeaEgoera = value;
        }

        [JsonProperty("sukaldea_egoera")]
        private string SukaldeaEgoeraSnake
        {
            get => _sukaldeaEgoera;
            set => _sukaldeaEgoera = value;
        }

        [JsonProperty("sukaldeaEgoera")]
        private string SukaldeaEgoeraCamel
        {
            get => _sukaldeaEgoera;
            set => _sukaldeaEgoera = value;
        }
    }
}