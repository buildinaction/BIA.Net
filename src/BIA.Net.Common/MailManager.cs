using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace BIA.Net.Common
{
    public static class MailManager
    {
        private static string MailFrom { get { return ConfigurationManager.AppSettings["MailFrom"]; } }

        private static string SmtpHost { get { return ConfigurationManager.AppSettings["SmtpHost"]; } }

        private static int SmtpPort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]); } }

        private static string SmtpUserName { get { return ConfigurationManager.AppSettings["SmtpUserName"]; } }

        private static string SmtpUserPassword { get { return ConfigurationManager.AppSettings["SmtpUserPassword"]; } }

        private static void FillMailCollection(List<string> sEmails, MailAddressCollection emails)
        {
            if (sEmails != null && emails != null)
            {
                foreach (string sEmail in sEmails)
                {
                    if (!string.IsNullOrEmpty(sEmail))
                    {
                        MailAddress mailAdress = new MailAddress(sEmail);
                        emails.Add(mailAdress);
                    }
                }
            }
        }

        public static CustomMailMessage CreateMailMessage(List<string> to, List<string> copy, string subject, string body, bool isBodyHtml)
        {
            CustomMailMessage mailToSend = new CustomMailMessage();

            mailToSend.From = new MailAddress(MailFrom);
            FillMailCollection(to, mailToSend.To);
            FillMailCollection(copy, mailToSend.CC);
            mailToSend.Subject = subject;
            mailToSend.Body = body;
            mailToSend.IsBodyHtml = isBodyHtml;

            return mailToSend;
        }

        public static bool SendMail(List<string> to, List<string> copy, string subject, string body, bool isBodyHtml)
        {
            using (SmtpClient smtpC = new SmtpClient(SmtpHost, SmtpPort))
            {
                if (!string.IsNullOrWhiteSpace(SmtpUserName) || !string.IsNullOrWhiteSpace(SmtpUserPassword))
                {
                    smtpC.Credentials = new NetworkCredential(SmtpUserName, SmtpUserPassword);
                }
                else
                {
                    smtpC.UseDefaultCredentials = true;
                }

                using (CustomMailMessage mailToSend = new CustomMailMessage())
                {
                    mailToSend.From = new MailAddress(MailFrom);
                    FillMailCollection(to, mailToSend.To);
                    FillMailCollection(copy, mailToSend.CC);
                    mailToSend.Subject = subject;
                    mailToSend.Body = body;
                    mailToSend.IsBodyHtml = isBodyHtml;

                    List<CustomMailMessage> mailMessage = new List<CustomMailMessage>();
                    mailMessage.Add(mailToSend);
                    SendMails(ref mailMessage);
                    return mailMessage.First().IsSuccess;
                }
            }
        }

        public static void SendMails(ref List<CustomMailMessage> mailMessages)
        {
            if (mailMessages != null && mailMessages.Count > 0)
            {
                using (SmtpClient smtpC = new SmtpClient(SmtpHost, SmtpPort))
                {
                    if (!string.IsNullOrWhiteSpace(SmtpUserName) || !string.IsNullOrWhiteSpace(SmtpUserPassword))
                    {
                        smtpC.Credentials = new NetworkCredential(SmtpUserName, SmtpUserPassword);
                    }
                    else
                    {
                        smtpC.UseDefaultCredentials = true;
                    }

                    foreach (CustomMailMessage mailMessage in mailMessages)
                    {
                        if (mailMessage != null)
                        {
                            if (mailMessage.To != null && mailMessage.To.Count > 0)
                            {
                                try
                                {
                                    smtpC.Send(mailMessage);
                                    mailMessage.IsSuccess = true;
                                }
                                catch (SmtpException e)
                                {
                                    mailMessage.IsSuccess = false;
                                }
                            }

                            //mailMessage.Dispose();
                        }
                    }
                }
            }
        }
    }
}
