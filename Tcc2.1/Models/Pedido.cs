using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public class Pedido
    {
        [Key]
        public int numero { get; set; }

        [Required]
        public int numero_pedido { get; set; }

        public double valor_compra { get; set; }

        public int VendedorID { get; set; }

        public int CompradorID { get; set; }

        public int ferramentaID { get; set; }

        public int pedido_venda { get; set; }

        public int quantidade_ferramenta { get; set; }

        public string data_compra { get; set; }
    }
}