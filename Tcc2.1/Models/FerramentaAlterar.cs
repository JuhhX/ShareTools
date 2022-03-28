using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tcc2._1.Models;

namespace Tcc2._1.Models
{
    public class FerramentaAlterar
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImagemFerramenta { get; set; }

        [Required]
        // Não usar o AllowEmptyValues
        public string NomeAnterior { get; set; }
        public string NomeFerramenta { get; set; }
        public double ValorFerramenta { get; set; }

        public string modelo_ferramenta { get; set; }

        public string marca_ferramenta { get; set; }

        public int cod_nacional { get; set; }

        public Condition utilizado { get; set; }

        public int quantidade { get; set; }
    }
}