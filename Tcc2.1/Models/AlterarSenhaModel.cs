using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public class AlterarSenhaModel
    {
        public string Email { get; set; }

        public string EmailWpp { get; set; }

        public string NumeroWpp { get; set; }

        public string CodEnviado { get; set;  }

        public string CodInserido { get; set; }

        public string SenhaNova { get; set; }

        public string ConfirmarSenhaNova { get; set; }
    }
}