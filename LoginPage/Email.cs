using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;


namespace LoginPage
{
    class Email
    {
        public Email(System.Net.Mail.Attachment pdfFile)
        {
            MailMessage feedbackEmail = new MailMessage();
            feedbackEmail.Attachments.Add(pdfFile);
        }

        public static void SendEmail(string filename, string applicantName, string applicantEmail, string staffMember, string staffMemberEmail, List<string> comments, List<string> sections)
        {
            MailMessage emailTOsend = new MailMessage();

            PdfCreator pdf = new PdfCreator(filename, applicantName, applicantEmail, staffMember, staffMemberEmail, comments, sections);
            try
            {
                //Setting up the smtp port
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;

                //Setting up a timer before showing wether the email was sent or not 
                client.Timeout = 2000;

                // email delivery method
                client.DeliveryMethod = SmtpDeliveryMethod.Network;


                //setting up the credentials of the email used as a sender
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("staff999111@gmail.com", "Password123@");


                MailMessage msg = new MailMessage();
                msg = emailTOsend;
                msg.Subject = "Feedback";
                msg.Body = ("Dear " + applicantName + "," + "\n" + "Find attached your feedback");

                //Setting up the sender
                msg.From = new MailAddress("staff999111@gmail.com");


                // Setting up the recipient
                msg.To.Add(applicantEmail);
                client.Send(msg);
                MessageBox.Show("Successfuly sent");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}



