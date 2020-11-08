using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Windows;

namespace LoginPage
{
    class Email
    {
        public static void SendEmail(MailMessage emailTOsend, string applicantName, string applicantEmail)
        {
            try
            {
                //Setting up the smtp port
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;

                //Setting up a timer before showing wether the email was sent or not 
                client.Timeout = 2000;

                // email delivery method
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                emailTOsend.Subject = "Feedback";
                emailTOsend.Body = ("Dear " + applicantName + "," + "\n" + "Find attached your feedback");

                //setting up the credentials of the email used as a sender
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("staff999111@gmail.com", "Password123@");

                //Setting up the sender
                emailTOsend.From = new MailAddress("staff999111@gmail.com");


                // Setting up the recipient
                emailTOsend.To.Add(applicantEmail);
                client.Send(emailTOsend);
                MessageBox.Show("Successfuly sent");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

