using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class TipoAPP
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Descricao { get; set; }


    }
}
