using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Tcc2._1.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Informe nome de usuário ou e-mail.")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite a senha.")]
        [StringLength(16, ErrorMessage = "Insira no máximo 16 caracteres")]
        public string Senha { get; set; }
    }
}