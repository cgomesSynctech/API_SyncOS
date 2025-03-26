namespace Modelos
{
    public class EventoReport
    {
        public int Id { get; set; }
        public int BoletoId { get; set; }
        public DateTime Data { get; set; }
        public string Arquivo { get; set; }
        public string Status { get; set; }
        public string TipoEvento { get; set; }
    }
}
