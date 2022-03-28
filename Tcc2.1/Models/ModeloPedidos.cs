using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public class ModeloPedidos
    {
        public int numero_pedido { get; set; }
        public string nome_comprador { get; set; }

        public string nome_ferramenta { get; set; }

        public int quantidade_ferramenta { get; set; }

        public double valor_compra { get; set; } 

        public int parcelas { get; set; }

        public string forma_pagamento { get; set; }
    }
}