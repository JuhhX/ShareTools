using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Tcc2._1.Models
{
    public enum TipoDeUsuario
    {
        CLIENTE,
        ADMINISTRADOR
    }
    public class Usuario
    {
        //public string data_nascimento { get; set; }

        public byte[] ImagemPerfil { get; set; }

        public int complemento { get; set; }

        [StringLength(20)]
        public string telefone_User { get; set; }

        [StringLength(30)]
        public string sobrenome_User { get; set; }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Informe seu nome")]
        [StringLength(40)]
        public string nome_User { get; set; }
        
        [Required(ErrorMessage = "Informe seu CPF")]
        [StringLength(14)]
        public string cpf_User { get; set; }
        [Required(ErrorMessage = "Informe um e-mail")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string email_User { get; set; }

        // A senha vai ser alterada pelo Hash para maior segurança
        [Required]
        [StringLength(50)]
        public string senha { get; set; }

        [Required]
        public string Rua { get; set; }

        [Required]
        public string Bairro { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public int num_casa { get; set; }

        public TipoDeUsuario tipo { get; set; }

        //Gera um código com a senha para manter segurança
        public static string GerarHash(string senha)
        {
            using(MD5 md5Hash = MD5.Create())
            {
                //Transforma a senha em Bytes
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));
                //Pega os bytes e transforma em string
                StringBuilder sBuilder = new StringBuilder();

                for(int i = 0; i < data.Length; i++)
                {
                    //cada byte se transforma em dois caracteres hexa(0 à 9) e (A à F)
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}