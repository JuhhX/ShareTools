using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tcc2._1.Models;

namespace Tcc2._1.DAL
{
    public class ToolsInitializer : System.Data.Entity.DropCreateDatabaseAlways<ToolsContext>
    {
        protected override void Seed(ToolsContext context)
        {
            // para url da imagem local não colocar "~/Content" e sim "/Content"
           var ferramenta = new Ferramenta
            {
                FerramentaID = 1,
                NomeFerramenta = "Martelo",
                ValorFerramenta = 30.50,
                ImagemFerramenta = null,
                VendedorID = 1,
                modelo_ferramenta = "Modelo tal",
                marca_ferramenta = "Tal marca",
                cod_nacional = 1234,
                disponibilidade = Disponibilidade.DISPONIVEL,
                utilizado = Condition.NOVO,
                quantidade = 6
            };
            var ferramenta2 = new Ferramenta
            {
                FerramentaID = 2,
                NomeFerramenta = "Chave de fenda",
                ValorFerramenta = 15.00,
                ImagemFerramenta = null,
                VendedorID = 1,
                modelo_ferramenta = "Modelo tal",
                marca_ferramenta = "Tal marca",
                cod_nacional = 1357,
                disponibilidade = Disponibilidade.DISPONIVEL,
                utilizado = Condition.NOVO,
                quantidade = 3
            };
            var ferramenta3 = new Ferramenta
            {
                FerramentaID = 3,
                NomeFerramenta = "Machado",
                ValorFerramenta = 80.00,
                ImagemFerramenta = null,
                VendedorID = 1,
                modelo_ferramenta = "Modelo tal",
                marca_ferramenta = "Tal marca",
                cod_nacional = 4321,
                disponibilidade = Disponibilidade.DISPONIVEL,
                utilizado = Condition.NOVO,
                quantidade = 0
            };
            var ferramenta4 = new Ferramenta
            {
                FerramentaID = 4,
                NomeFerramenta = "Chave de boca",
                ValorFerramenta = 25.00,
                ImagemFerramenta = null,
                VendedorID = 1,
                modelo_ferramenta = "Modelo tal",
                marca_ferramenta = "Tal marca",
                cod_nacional = 9273,
                disponibilidade = Disponibilidade.DISPONIVEL,
                utilizado = Condition.NOVO,
                quantidade = 5
            };

            var Administrador = new Usuario
            {
                UserID = 1,
                nome_User = "Júlio César",
                sobrenome_User = "Silva",
                ImagemPerfil = null,
                cpf_User = "12345678901",
                email_User = "julio95978@gmail.com",
                telefone_User = "12345678",
                senha = Usuario.GerarHash("12345"),
                Rua = "Rua",
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                //data_nascimento = "01/11/2002",
                tipo = TipoDeUsuario.ADMINISTRADOR,
                num_casa = 110,
                complemento = 0
            };

            var ferramenta_auxiliar = new Ferramenta
            {
                FerramentaID = 5,
                NomeFerramenta = "Marreta",
                ValorFerramenta = 75.00,
                ImagemFerramenta = null,
                VendedorID = 2,
                modelo_ferramenta = "Modelo x",
                marca_ferramenta = "Marca x",
                cod_nacional = 2915,
                disponibilidade = Disponibilidade.DISPONIVEL,
                utilizado = Condition.USADO,
                quantidade = 4
            };

            var auxiliar = new Usuario
            {
                UserID = 2,
                nome_User = "C",
                sobrenome_User = "S",
                ImagemPerfil = null,
                cpf_User = "23456789010",
                email_User = "cesar@gmail.com",
                telefone_User = "11110000",
                senha = Usuario.GerarHash("1"),
                Rua = "Rua",
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                tipo = TipoDeUsuario.CLIENTE,
                num_casa = 110,
                complemento = 0
            };

            context.Ferramenta.Add(ferramenta_auxiliar);
            context.Usuario.Add(auxiliar);

            context.Ferramenta.Add(ferramenta);
            context.Ferramenta.Add(ferramenta2);
            context.Ferramenta.Add(ferramenta3);
            context.Ferramenta.Add(ferramenta4);
            context.Usuario.Add(Administrador);
            context.SaveChanges();
        }
    }
}