using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cervejaria.Models
{
    public class ReceitaIngrediente
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Receita")]
        public virtual int IdReceita { get; set; }
        [JsonIgnore]
        public Receita? Receita { get; set; }
        [Required]
        [ForeignKey("Ingrediente")]
        public virtual int IdIngrediente { get; set; }
        [JsonIgnore]
        public Ingrediente? Ingrediente { get; set; }
        [Required(ErrorMessage = "Quantidade de ingredientes em estoque é obrigatório")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double QuantidadeDeIngrediente { get; set; }
    }
}
