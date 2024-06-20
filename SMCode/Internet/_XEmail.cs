/*  ===========================================================================
 *  
 *  File:       SMEmail.cs
 *  Version:    2.0.24
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode e-mail management class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mail;

namespace SMCode
{

    /* */

    /// <summary>SMCode e-mail management class.</summary>
    public class SMEmail
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set email from address.</summary>
        public string From { get; set; }

        /// <summary>Get or set email to address.</summary>
        public string To { get; set; }

        /// <summary>Get or set cc addresses.</summary>
        public List<string> Cc { get; private set; } = new List<string>();

        /// <summary>Get or set blind-mail cc addresses.</summary>
        public List<string> Bcc { get; set; } = new List<string>();

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMEmail(SMCode _SM = null)
        {
            if (_SM == null) SM = SMApplication.CurrentOrNew();
            else SM = _SM;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */


        /// <summary>Send email message via SMTP. Returns true if succeed.</summary>
        public bool Send(string _Host, string _SenderAddress, string _Addresses, string _BCC, 
            string _Subject, string _Body, int _Port, string _User, string _Password, bool _SSL, string[] _Attachments)
        {
            int i;
            bool r = false;
            System.Net.Mail.MailMessage netMail;
            System.Web.Mail.MailMessage webMail;
            SmtpClient smtp;
            if (_Addresses != null) 
            {
                try
                {
                    if (_SSL)
                    {
                        webMail = new System.Web.Mail.MailMessage();
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", _Host);
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", _Port.ToString());

                        //sendusing: cdoSendUsingPort, value 2, for sending the message using the network.
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing","2"); 

                        //smtpauthenticate: Specifies the mechanism used when authenticating 
                        //to an SMTP 
                        //service over the network. Possible values are:
                        //- cdoAnonymous, value 0. Do not authenticate.
                        //- cdoBasic, value 1. Use basic clear-text authentication. 
                        //When using this option you have to provide the user name and password 
                        //through the sendusername and sendpassword fields.
                        //- cdoNTLM, value 2. The current process security context is used to 
                        // authenticate with the service.
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                        //Use 0 for anonymous
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", _User);
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", _Password);
                        webMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl","true");
                        webMail.From = _SenderAddress;
                        webMail.To = _Addresses;
                        webMail.Subject = _Subject;
                        webMail.BodyFormat = System.Web.Mail.MailFormat.Text;
                        webMail.Body = _Body;
                        if (_Attachments != null)
                        {
                            for (i = 0; i < _Attachments.Length; i++)
                            {
                                if (XIO.FileExists(_Attachments[i])) webMail.Attachments.Add(new MailAttachment(_Attachments[i]));
                            }
                            webMail.Priority = System.Web.Mail.MailPriority.High;
                        }

                        System.Web.Mail.SmtpMail.SmtpServer = _Host+":"+_Port.ToString();
                        System.Web.Mail.SmtpMail.Send(webMail);
                    }
                    else
                    {
                        //
                        // set message
                        //
                        netMail = new System.Net.Mail.MailMessage();
                        netMail.From = new MailAddress(_SenderAddress);
                        while (_Addresses.Trim().Length > 0) netMail.To.Add(SM.Extract(ref _Addresses, ";,").Trim());
                        while (_BCC.Trim().Length > 0) netMail.Bcc.Add(SM.Extract(ref _BCC, ";,").Trim());
                        netMail.Subject = _Subject;
                        netMail.IsBodyHtml = _Body.Trim().ToUpper().StartsWith(@"<HTML") || _Body.Trim().ToUpper().StartsWith(@"<!DOCTYPE HTML");
                        netMail.Body = _Body;
                        //
                        // add attachments files
                        //
                        if (_Attachments != null)
                        {
                            for (i = 0; i < _Attachments.Length; i++)
                            {
                                if (SM.FileExists(_Attachments[i])) netMail.Attachments.Add(new Attachment(_Attachments[i]));
                            }
                        }
                        //
                        // set smtp server
                        //
                        smtp = new SmtpClient(_Host);
                        if (_Port < 1) smtp.Port = 25;
                        else smtp.Port = _Port;
                        smtp.Credentials = new System.Net.NetworkCredential(_User, _Password);
                        smtp.EnableSsl = _SSL;
                        smtp.UseDefaultCredentials = false;
                        smtp.Send(netMail);
                        //
                        // release resources
                        //
                        netMail.Dispose();
                        smtp.Dispose();
                        //
                        // end
                        //
                    }
                    r = true;
                }
                catch (Exception ex)
                {
                    r = false;
                    SM.Error(ex);
                }
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}