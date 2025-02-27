/*  ===========================================================================
 *  
 *  File:       SMEmail.cs
 *  Version:    2.0.124
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
        public SMEmail(SMCode _SM = null)
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
            string _From, string _To, string _CC, string _BCC,
            string _Subject, string _Body, bool? _Html = null, string[] _Attachments = null,
            string _User = "", string _Password = "", int _Port = 0, bool _SSL = false)
        {
            int i;
            bool r = false;
            MailMessage netMail;
            SmtpClient smtp;
            if (_To != null)
            {
                try
                {
                    //
                    // set message
                    //
                    netMail = new MailMessage();
                    netMail.From = new MailAddress(_From);
                    while (_To.Trim().Length > 0) netMail.To.Add(SM.Extract(ref _To, ";,").Trim());
                    while (_CC.Trim().Length > 0) netMail.CC.Add(SM.Extract(ref _CC, ";,").Trim());
                    while (_BCC.Trim().Length > 0) netMail.Bcc.Add(SM.Extract(ref _BCC, ";,").Trim());
                    netMail.Subject = _Subject;
                    if (_Html != null) netMail.IsBodyHtml = _Html.Value;
                    else netMail.IsBodyHtml = _Body.IndexOf("</") > -1;
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
                    if (!SM.Empty(_User))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(_User, _Password);
                        smtp.UseDefaultCredentials = false;
                    }
                    smtp.EnableSsl = _SSL;
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
        public bool Send(string _From, string _To, string _CC, string _BCC,
            string _Subject, string _Body, bool? _Html = null, string[] _Attachments = null)
        {
            return Send(Account.Host,
                _From, _To, _CC, _BCC, _Subject, _Body, _Html, _Attachments,
                Account.User, Account.Password, Account.Port, Account.SSL);
        }

        #endregion

        /* */

    }

    /* */

}