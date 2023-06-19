using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cervejaria.Models
{
    [Index(nameof(Cnpj), IsUnique = true)]
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [MaxLength(200, ErrorMessage = "Máximo de 200 caracteres")]
        [MinLength(10, ErrorMessage = "Minimo de 10 caracteres")]
        [Display(Name = "Nome do Usuário")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [Display(Name = "Informe a Senha")]
        [StringLength(10, MinimumLength = 4)]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        public string NomeEmpresa { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [MinLength(14, ErrorMessage = "Somente números, dando 14 caracteres")]
        [MaxLength(14, ErrorMessage = "Somente números, dando 14 caracteres")]
        public string Cnpj { get; set; }
        [Required(ErrorMessage = "Informe o seu email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        public string Email { get; set; }

    }
}
