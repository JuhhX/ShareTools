using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public class MeuCarrinho
    {

        public int FerramentaID { get; set; }

        public int ClienteID { get; set; }

        public int Quantidade { get; set; }
        
        [Key]
        public int Meu_carrinho { get; set; }
    }
}