using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(Tcc2._1.OwinStartup))]

namespace Tcc2._1
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Para obter mais informações sobre como configurar seu aplicativo, visite https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Home/Login"),
                //É bom colocar um nome distinto no cookie para ele n dar problema com o Anti forgen token
                CookieName = "UsuarioCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(10)
            });
            
        }
    }
}
