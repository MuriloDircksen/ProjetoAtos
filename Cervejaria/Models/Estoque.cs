using System.ComponentModel.DataAnnotations;

namespace Cervejaria.Models
{
    public class Estoque
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [MaxLength(60, ErrorMessage = "Máximo de 60 caracteres")]
        [MinLength(3, ErrorMessage = "Minimo de 3 caracteres")]
        public string NomeEstoque { get; set; }
    }
}
