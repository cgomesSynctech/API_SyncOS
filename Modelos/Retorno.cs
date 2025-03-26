using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class Retorno
    {
        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public DateTime Baixa { get; set; }
        public int CodigoOcorrencia { get; set; }
        public int BoletoId { get; set; }
        public int SacadoId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ValorPago { get; set; }

    }
}
