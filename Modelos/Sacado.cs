using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos
{
    public class Sacado
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nome { get; set; }
        public string? Fantasia { get; set; }
        public string? CpfCnpj { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? CEP { get; set; }
        public string? UF { get; set; }
        public string? NomeAvalista { get; set; }
        public string? CpfCnpjAvalista { get; set; }
        public bool? IsBloqueado { get; set; }
        public bool? IsDesativado { get; set; }
        public int? CentroReceitaId { get; set; }
        public double? ValorContrato { get; set; }

        [NotMapped]
        public DateTime? Vencimento { get; set; }
        [NotMapped]
        public decimal? Valor { get; set; }


        [NotMapped]
        public int IdEmpresa { get; set; }

        [NotMapped]
        [ForeignKey("IdEmpresa")]
        public CentroReceita? CentroReceita { get; set; }
    }
}