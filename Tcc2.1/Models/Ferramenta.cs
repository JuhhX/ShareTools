using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public enum Disponibilidade
    {
        DISPONIVEL,
        INDISPONIVEL
    }

    public enum Condition
    {
        USADO,
        NOVO
    }
    public class Ferramenta
    {
        public byte[] ImagemFerramenta { get; set; }

        [Key]
        public int FerramentaID { get; set; }

        [Required]
        // Não usar o AllowEmptyValues
        public string NomeFerramenta { get; set; }
        public double ValorFerramenta { get; set; }
        public int VendedorID { get; set; }

        public string modelo_ferramenta { get; set; }

        public string marca_ferramenta { get; set; }

        public int cod_nacional { get; set; }

        public Disponibilidade disponibilidade { get; set; }

        public Condition utilizado { get; set; }

        public int quantidade { get; set; }

    }
}