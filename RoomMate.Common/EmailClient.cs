﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Configuration;

namespace RoomMate.Common
{
    public class EmailClient
    {
        private MailAddress domainEmailAdress = new MailAddress("materoom2019@gmail.com", "Room Mate");
        private MailAddress userEmailAddress = null; 
        private string domainEmailPassword;
        private string subjectEmail;
        private string contentEmail;
        private SmtpClient smtpClient;
        
        public EmailClient()
        {
            var passwordEmailFromWebConfig = ConfigurationManager.AppSettings["MAIL_PASSWORD"];
            domainEmailPassword = passwordEmailFromWebConfig;

            smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(domainEmailAdress.Address, domainEmailPassword)
            };
        }
        public bool SendVerifyAccountCode(string _userEmailAddress, string requestLink)
        {
            subjectEmail = "Welcome in RoomMate!";
            contentEmail = "<br><br>Twoje konto zostało poprawnie założone." +
                             "W celu aktywowania konta wejdź w poniższy link." +
                             "<br><br><a href='" + requestLink + "'>Aktywuj konto</a>";
            
            return SendEmail(_userEmailAddress);
        }
        public bool SendResetPasswordCode(string _userEmailAddress, string requestLink)
        {
            subjectEmail = "Zresetuj hasło";
            contentEmail = "<br><br>W celu zresetowania hasła kliknij na niżej zamieszczony link." +
                             "Jeśli to nie Ty chciałeś zresetować hasło proszę o zignorowanie wiadomości." +
                             "<br><br><a href='" + requestLink + "'>Resetuj hasło</a>";

            return SendEmail(_userEmailAddress);
        }
        public bool SendEmail(string _userEmailAddress)
        {
            
            userEmailAddress = new MailAddress(_userEmailAddress);
            bool sendEmailWasSuccessful = false;

            using (var message = new MailMessage(domainEmailAdress, userEmailAddress)
            {
                Subject = subjectEmail,
                Body = contentEmail,
                IsBodyHtml = true
            })

            try 
            { 
                smtpClient.Send(message);
                sendEmailWasSuccessful = true;
            }
            catch(SmtpException ex) 
            { 
               System.Diagnostics.Debug.WriteLine("Mail cannot be sent: "+ ex.Message);
            }
            return sendEmailWasSuccessful;

        }

    }
}