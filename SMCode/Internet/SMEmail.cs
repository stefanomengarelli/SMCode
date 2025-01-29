/*  ===========================================================================
 *  
 *  File:       SMEmail.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  E-mail management class.
 *  
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SMCodeSystem
{

    /* */

    /// <summary>E-mail management class.</summary>
    public class SMEmail
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>SMTP account.</summary>
        public SMSmtpAccount Account { get; private set; }

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
        public SMEmail(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Account = new SMSmtpAccount("", SM);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */


        /// <summary>Send email message via SMTP. Returns true if succeed.</summary>
        public bool Send(string _Host,
            string _SenderAddress, string _Addresses, string _BCC,
            string _Subject, string _Body, bool _Html, string[] _Attachments,
            string _User, string _Password, int _Port = 0, bool _SSL = false)
        {
            int i;
            bool r = false;
            MailMessage netMail;
            SmtpClient smtp;
            if (_Addresses != null)
            {
                try
                {
                    //
                    // set message
                    //
                    netMail = new MailMessage();
                    netMail.From = new MailAddress(_SenderAddress);
                    while (_Addresses.Trim().Length > 0) netMail.To.Add(SM.Extract(ref _Addresses, ";,").Trim());
                    while (_BCC.Trim().Length > 0) netMail.Bcc.Add(SM.Extract(ref _BCC, ";,").Trim());
                    netMail.Subject = _Subject;
                    netMail.IsBodyHtml = _Html;
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
                    if (_Port > 0) smtp.Port = _Port;
                    else if (_SSL) smtp.Port = 465;
                    else smtp.Port = 25;
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

        /// <summary>Send email message via SMTP with default account. Returns true if succeed.</summary>
        public bool Send(string _SenderAddress, string _Addresses, string _BCC,
            string _Subject, string _Body, bool _Html, string[] _Attachments)
        {
            return Send(Account.Host,
                _SenderAddress, _Addresses, _BCC, _Subject, _Body, _Html, _Attachments,
                Account.User, Account.Password, Account.Port, Account.SSL);
        }

        #endregion

        /* */

    }

    /* */

}