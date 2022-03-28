using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public class CompraModel
    {
        public string cvv_cartao { get; set; }

        [Required(ErrorMessage="Número do cartão é obrigatório")]
        public string num_cartao { get; set; }

        [Required(ErrorMessage="O nome impresso no  cartão é obrigatório")]
        public string nome_cartao { get; set; }

        [Required(ErrorMessage="A validade do cartão é obrigatória")]
        public string validade_cartao { get; set; }

        [Required(ErrorMessage="A quantidade de parcelas é obrigatório")]
        public int parcelas_compra { get; set; }


    }
}