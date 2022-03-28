using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tcc2._1.Models;
using Microsoft.Owin;
using Owin;

namespace Tcc2._1.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //Permissões Globais
            //Indicamos o tipo de usuário que pode acessar o sistema. Com isso, somente pessoas com essas identificações podem acessar
            //O conteúdo daquela página

            filters.Add(new AuthorizeAttribute()
            {
                Roles = "" + TipoDeUsuario.ADMINISTRADOR + "," + TipoDeUsuario.CLIENTE
            });

            //Impede o armazenamento em cache das informações da navegação
            filters.Add(new OutputCacheAttribute()
            {
                VaryByParam = "*",
                Duration = 0,
                NoStore = true
            });
        }
    }
}