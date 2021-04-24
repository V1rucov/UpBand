using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace upband{
    public interface IEmailService {
        public MailAddress from { get; set; }
        public MailAddress to { get; set; }
        public void Send(string to, string header, string body);
        public Task SendAsync(string to, string header, string body);
    }
    public class EmailService : IEmailService
    {
        public MailAddress from { get; set; }
        public MailAddress to { get; set; }
        public EmailService(){
            from = new MailAddress("retrocoderteam@gmail.com");
        }
        public void Send(string to,string header, string body){
            this.to = new MailAddress(to);
            using(MailMessage message = new MailMessage(from,this.to))
            using(SmtpClient smtp = new SmtpClient("smtp.gmail.com",587))
            {
                message.Subject = header;
                message.Body = body;
                message.IsBodyHtml = true;

                smtp.EnableSsl=true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("retrocoderteam@gmail.com","popka2278");
                smtp.Send(message);
            }
        }
        public async Task SendAsync(string to, string header, string body) {
            await Task.Run(()=>{
                Send(to,header,body);
            });
        }
    }
}