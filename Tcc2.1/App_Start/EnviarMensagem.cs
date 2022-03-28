using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Tcc2._1.App_Start
{
    public class EnviarMensagem
    {
        public static void enviar_Email(string Assunto, string Mensagem, string Destino = "suporte@gmail.com")
        {
            MailMessage mail = new MailMessage();

            //Quem vai enviar
            mail.From = new MailAddress("Email@gmail.com");

            //Para quem?
            mail.CC.Add(Destino);
            //Assunto
            mail.Subject = Assunto;
            //Utiliza caracteres UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            //É um código HTML
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            //Mensagem em HTML
            mail.Body = "<p>" + Mensagem + "</p>";
            //Prioridade do Email
            mail.Priority = MailPriority.High;

            //Conexão com o Gmail
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

            //Conectar a conta Gmail
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("Email@gmail.com", "SenhaDaConta");

            smtpClient.EnableSsl = true;

            //Enviar Email
            smtpClient.Send(mail);
        }

    }
}