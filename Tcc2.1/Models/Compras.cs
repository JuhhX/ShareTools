using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public enum Formas_Pagamento
    {
        PIX,
        BOLETO,
        CREDITO
    }
    public class Compras
    {
        public string num_Cartao { get; set; }

        public string nome_Cartao { get; set; }

        public string validade_Cartao { get; set; }

        [Key]
        public int numero { get; set; }

        [Required]
        public int pedidosID { get; set; }

        public double valorCompra { get; set; }

        public int parcelas_Compra { get; set; }

        public string Data_Compra { get; set; }

        public int clienteID { get; set; }

        public int VendedorID { get; set; }

        public int pedido_venda { get; set; }

        public int quantidade_ferramenta { get; set; }

        public int ferramentaID { get; set; }

        public Formas_Pagamento forma_pagamento { get; set; }
    }
}