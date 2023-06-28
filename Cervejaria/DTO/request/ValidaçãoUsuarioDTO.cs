using System.ComponentModel.DataAnnotations;

namespace Cervejaria.DTO.request
{
    public class ValidaçãoUsuarioDTO
    {
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [Display(Name = "Informe a Senha")]
        [StringLength(10, MinimumLength = 8)]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Informe o seu email")]
        
        public string Email { get; set; }
    }
}
