using MailKit.Net.Smtp;
using MimeKit;
namespace AdminPanel.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email,string subject,string message)
        {
            var emailMsg = new MimeMessage();
            //откуда мы высылаем
            emailMsg.From.Add(new MailboxAddress("Administration Bilimkana American","alanhao@mail.ru"));
            //куда мы отправляем
            emailMsg.To.Add(new MailboxAddress("",email));
            //примочки
            emailMsg.Subject = subject;
            //текстовка 
            emailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            //очистка памяти неуправляемого кода при неудачной попытке подключения с исключением
            using (var client = new SmtpClient())
            {
                //true-поддерживание писем
                await client.ConnectAsync("smtp.gmail.com",465,true);
                // имя на которое будут отправлять
                await client.AuthenticateAsync("alanhao@mail.ru", "ilosallsgbeujqdy");
                //отправляем само письмо
                await client.SendAsync(emailMsg);
                //сессию прервали
                await client.DisconnectAsync(true);
            }
        }
    }
}
