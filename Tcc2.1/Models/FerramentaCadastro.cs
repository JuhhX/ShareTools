using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public enum coditionCadastro
    {
        USADO,
        NOVO
    }
    public class FerramentaCadastro
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImagemFerramenta { get; set; }

        [Required(ErrorMessage = "Nome da ferramenta é obrigatório")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        // Não usar o AllowEmptyValues
        public string NomeFerramenta { get; set; }
        [Required(ErrorMessage = "Valor da ferramenta é obrigatório")]
        public double ValorFerramenta { get; set; }
        [Required(ErrorMessage = "O modelo da ferramenta é obrigatório")]
        [StringLength(30, ErrorMessage = "Insira no máximo 30 caracteres")]
        public string modelo_ferramenta { get; set; }

        [Required(ErrorMessage = "A marca da ferramenta é obrigatória")]
        [StringLength(30, ErrorMessage = "Insira no máximo 30 caracteres")]
        public string marca_ferramenta { get; set; }

        //[StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public int cod_nacional { get; set; }

        [Required(ErrorMessage = "Informe \"NOVO\" ou \"USADO\"")]
        public coditionCadastro utilizado { get; set; }
        [Required(ErrorMessage = "A quantidade é obrigatória")]
        public int quantidade { get; set; }
    }
}