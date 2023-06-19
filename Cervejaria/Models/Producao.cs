using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cervejaria.Models
{
    public class Producao
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Receita")]        
        public virtual int ReceitaId { get; set; }
        [JsonIgnore]
        public Receita? Receita { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double VolumeApronte { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [MaxLength(200, ErrorMessage = "Máximo de 200 caracteres")]
        [MinLength(10, ErrorMessage = "Minimo de 10 caracteres")]
        public string Responsavel { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy", ApplyFormatInEditMode = true)]
        public DateTime DataProducao { get; set; }
    }
}
