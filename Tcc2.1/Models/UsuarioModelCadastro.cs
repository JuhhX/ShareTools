using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tcc2._1.Models
{
    public class UsuarioModelCadastro
    {

        public string telefone_usuario { get; set; }

        //public string data_nascimento_usuario { get; set; }

        [StringLength(20, ErrorMessage = "Insira no máximo 20 caracteres")]
        public string sobrenome_usuario { get; set; }

        public string complemento_casa { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImagemPerfil { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public string nome_usuario { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "Insira no máximo 11 caracteres")]
        public string cpf_usuario { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public string email_usuario { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [StringLength(16, ErrorMessage = "Insira no máximo 16 caracteres")]
        public string senha { get; set; }

        [Required(ErrorMessage = "Confirme a senha")]
        [StringLength(16, ErrorMessage = "Insira no máximo 16 caracteres")]
        public string confirmar_senha { get; set; }

        [Required(ErrorMessage = "Nome da Rua é obrigatório")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Nome do Bairro é obrigatório")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Nome da Cidade é obrigatório")]
        [StringLength(40, ErrorMessage = "Insira no máximo 40 caracteres")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Nome do Estado é obrigatório")]
        [StringLength(2, ErrorMessage = "Insira no máximo 2 caracteres")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Número da casa é obrigatório")]
        public int numero_casa { get; set; }

    }
}