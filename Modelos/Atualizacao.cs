using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public partial class Atualizacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataVersao { get; set; }
        [Required]
        public string? Versao { get; set; }
        public string? CNPJ { get; set; }
        public string? LeiaMe { get; set; }
        public string? Script { get; set; }
        public string? Pacote { get; set; }
        public string? PathDescompactar { get; set; }
        [Required]
        public int TipoAtualizacaoId { get; set; }
        [Required]
        public string? Backup { get; set; }
        [Required]
        public string? Ativo { get; set; }
    }
}
