using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SCIR.Email
{
    public class EnvioEmail
    {
        public static bool SendMail(Usuario usuario, string text, Requerimento requerimento = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(usuario.Email))
                {
                    // Estancia da Classe de Mensagem
                    MailMessage _mailMessage = new MailMessage();
                    // Remetente
                    _mailMessage.From = new MailAddress("No.Replay.SCIR@gmail.com");

                    // Destinatario seta no metodo abaixo

                    //Contrói o MailMessage
                    _mailMessage.CC.Add(usuario.Email);
                    _mailMessage.Subject = "Auditoria SCIR - IFSC";
                    _mailMessage.IsBodyHtml = true;
                    _mailMessage.Body = $@"Olá, { usuario.Nome }! </br></br>
                                       Teve movimentação no requerimento:  { requerimento.Protocolo }
                                       <br><br>
                                       Auditoria: {DateTime.Now.ToString("dd/MM/yyyy")}<br>
                                       {text}
                                       <br><br>
                                       Sistema de controle interno de requerimento IFSC.";


                    //CONFIGURAÇÃO COM PORTA
                    SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

                    //CONFIGURAÇÃO SEM PORTA
                    // SmtpClient _smtpClient = new SmtpClient(UtilRsource.ConfigSmtp);

                    // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
                    _smtpClient.UseDefaultCredentials = false;
                    _smtpClient.Credentials = new NetworkCredential("No.Replay.SCIR@gmail.com", "emailscir");

                    _smtpClient.EnableSsl = true;

                    _smtpClient.Send(_mailMessage);

                    return true;
                }

                return false;
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}