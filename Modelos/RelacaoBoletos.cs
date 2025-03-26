﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos
{
    public class RelacaoBoletos
    {
        public int Id { get; set; }
        public string? Pagador_Nome { get; set; }
        public string? PagadorCpfCnpj { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Vencimento { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PercentualMulta { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MoraDia { get; set; }
        public DateTime Baixa { get; set; }
        public DateTime Pagamento { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorPago { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? NossoNumero { get; set; }
        public string? Status_Descricao { get; set; }
        public string? Carteira_Descricao { get; set; }
        public string? Doc { get; set; }
        public int SacadoId { get; set; }
        public int StatusId { get; set; }
        public int CarteiraId { get; set; }
        public int CentroReceitaId { get; set; }
    }
}
