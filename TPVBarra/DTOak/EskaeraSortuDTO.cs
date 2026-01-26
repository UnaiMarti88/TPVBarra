namespace TPVBarra.DTOak
{
    public class EskaeraSortuDTO
    {
        public int ErabiltzaileId { get; set; }
        public int MahaiaId { get; set; }
        public int Komensalak { get; set; }
        public string SukaldeaEgoera { get; set; } = "zain";

        public List<EskaeraProduktuaDTO> Produktuak { get; set; } = new List<EskaeraProduktuaDTO>();
    }
}
