
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modelos
{
        public class BoletoReport
        {
            public int Id { get; set; }
            public string? NumeroDocumento { get; set; }
            public string? NomePagador { get; set; }
            public string? CpfCnpj { get; set; }
            public DateTime? Emissao { get; set; }
            public DateTime? Vencimento { get; set; }
            public DateTime? Pagamento { get; set; }
            [Column(TypeName = "decimal(10,2)")]
            public decimal Valor { get; set; }
            [Column(TypeName = "decimal(10,2)")]
            public decimal Oscilacao { get; set; }
            [Column(TypeName = "decimal(10,2)")]
            public decimal ValorPago { get; set; }
            public int StatusId { get; set; }
            public int SacadoId { get; set; }
            public string? Vendedor{ get; set; }
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

        }
}