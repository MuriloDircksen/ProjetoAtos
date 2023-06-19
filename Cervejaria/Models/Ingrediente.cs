using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cervejaria.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome de preenchimento obigatório!")]
        [MaxLength(200, ErrorMessage = "Máximo de 200 caracteres")]
        [MinLength(5, ErrorMessage = "Minimo de 5 caracteres")]
        public string NomeIngrediente { get; set; }
        [Required]
        [ForeignKey("Estoque")]
        public virtual int IdEstoque { get; set; }
        [JsonIgnore]
        public Estoque? Estoque { get; set;}
        [Required(ErrorMessage = "Quantidade de ingredientes em estoque é obrigatório")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double Quantidade { get; set; }
        [Required(ErrorMessage = "Valor unitário é obrigatório")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double ValorUnidade { get; set; }
        [Required(ErrorMessage = "Valor total é obrigatório")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números são permitidos.")]
        public double ValorTotal { get; set; }
        [Required(ErrorMessage = "Campo unidade padrão preenchimento obigatório!")]
        public string Unidade { get; set; }
        [Required(ErrorMessage = "Campo tipo de ingrediente de preenchimento obigatório!")]
        [MaxLength(50, ErrorMessage = "Máximo de 50 caracteres")]
        [MinLength(3, ErrorMessage = "Minimo de 3 caracteres")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo fornecedor de preenchimento obigatório!")]
        [MaxLength(80, ErrorMessage = "Máximo de 80 caracteres")]
        [MinLength(3, ErrorMessage = "Minimo de 3 caracteres")]
        public string Fornecedor { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy", ApplyFormatInEditMode = true)]
        public DateTime Validade { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy", ApplyFormatInEditMode = true)]
        public DateTime DataEntrada { get; set;}
        [JsonIgnore]
        public ICollection<ReceitaIngrediente>? ReceitaIngredientes { get; set; }

    }
}
