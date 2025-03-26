using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Cliente
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public int RevendaId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Razao { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string? Fantasia { get; set; }
        
        [Column(TypeName = "varchar(14)")]
        public string? CPF_CNPJ { get; set; }
        
        [Column(TypeName = "varchar(255)")]
        public string? Endereco { get; set; }
        
        [Column(TypeName = "varchar(255)")]
        public string? Bairro { get; set; }
        
        [Column(TypeName = "varchar(10)")]
        public string? CEP { get; set; }
        
        [Column(TypeName = "Char(2)")]
        public string? UFId { get; set; }
        
        [Column(TypeName = "varchar(10)")]
        public string? Numero { get; set; }
        public bool Bloqueado { get; set; }
        public bool Desativado { get; set; }
        
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorContrato { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorContratoBackup { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? UltimaAtualizacao { get; set; } = DateTime.MinValue;
        
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataInclusao { get; set; } = DateTime.Now;
        
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataAlteracao { get; set; } = DateTime.MinValue;
        
        public int MunicipioId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? LatitudeLongitude { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? InscricaoEstadual { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? ChaveFlexDocs { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? ChaveCSC { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? ChaveManifesto { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? WhatsApp { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Resp_Nome { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Resp_Endereco { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? Resp_Numero { get; set; }

        [Column(TypeName = "Char(2)")]
        public string? Resp_UF { get; set; }
        
        [Column(TypeName = "varchar(255)")]
        public string? Resp_Cidade { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? Resp_CEP { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string? Resp_CPF { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorTef { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorSped { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorManifesto { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorComissao { get; set; }

        public string? Vendedor { get; set; }

    }
}
