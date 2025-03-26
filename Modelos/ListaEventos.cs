using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos
{
    [NotMapped]
    public class ListaEventos
    {
        public int Id { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? NomePagador { get; set; }
        public string? CpfCnpj { get; set; }
        public DateTime Vencimento { get; set; }
        public DateTime Pagamento { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Oscilacao { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorPago { get; set; }
        public string? Status { get; set; }
        public string? Doc { get; set; }
        public string? Carteira { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorSped { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorTef { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorBackup { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorManifesto { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorComissao { get; set; }
        public DateTime Data { get; set; }
        public string? Arquivo { get; set; }
        public string? StatusEvento { get; set; }
        public string? TipoEvento { get; set; }
    }
}
