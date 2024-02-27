using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using TravelOrganization2.Model.Dto.Common;

namespace TravelOrganization2.Model.Services.Email
{
    public class EmailRepository
    {
        public async Task<ResultDto> SendEmail(string email,string subject,string body,bool ishtml = false)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var credentials = new NetworkCredential()
                    {
                        UserName = "mahery811@gmail.com",
                        Password = "68C8B55F22DD273C137C0A11062F44888BD8"
                    };
                    client.Credentials = credentials;
                    client.Host = "smtp.elasticemail.com";
                    client.Port = 2525;
                    client.EnableSsl = true;

                    using (var message = new MailMessage())
                    {
                        message.To.Add(new MailAddress(email));
                        message.From = new MailAddress("mahery811@gmail.com");
                        message.Subject = subject;  
                        message.Body = body;    
                        message.IsBodyHtml = ishtml;
                        client.Send(message);   
                    }

                    await Task.CompletedTask;

                }

                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "Email has been sent"
                };
            }
            catch (Exception e)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}
