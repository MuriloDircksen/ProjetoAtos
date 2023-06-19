using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Cervejaria.Models
{
    public class Receita
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [MaxLength(200, ErrorMessage = "Máximo de 200 caracteres")]
        [MinLength(10, ErrorMessage = "Minimo de 10 caracteres")]
        public string NomeReceita { get; set; }
        [Required(ErrorMessage = "Campo responsável de preenchimento obigatório!")]
        [MaxLength(200, ErrorMessage = "Máximo de 200 caracteres")]
        [MinLength(10, ErrorMessage = "Minimo de 10 caracteres")]
        public string Responsavel { get; set; }
        [Required(ErrorMessage = "Campo estilo de preenchimento obigatório!")]
        [MaxLength(80, ErrorMessage = "Máximo de 80 caracteres")]
        [MinLength(10, ErrorMessage = "Minimo de 10 caracteres")]
        public string Estilo { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy", ApplyFormatInEditMode = true)]
        public DateTime UltimaAtualizacao { get; set; }
        [Required(ErrorMessage = "Orçamento é obrigatório")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double Orcamento { get; set; }
        [Required(ErrorMessage = "Volume é obrigatório")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double VolumeReceita { get; set; }
        [JsonIgnore]
        public ICollection<Producao>? Producao { get; set; }
        [JsonIgnore]
        public ICollection<ReceitaIngrediente>? ReceitaIngredientes { get; set; }
    }
}
