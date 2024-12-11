/*  ------------------------------------------------------------------------
 *  
 *  File:       XPopAccount.cs
 *  Version:    1.0.0
 *  Date:       March 2021
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@microrun.it
 *  
 *  Copyright (C) 2022 by Stefano Mengarelli - All rights reserved - Use, permission and restrictions under license.
 *
 *  Microrun XRad POP account class.
 *
 *  Dependencies: XError, XIni, XTag
 *
 *  ------------------------------------------------------------------------
 */

using System;

namespace XRadSharp
{

    /* */

    /// <summary>Microrun XRad POP account class.</summary>
    public class XPopAccount
    {

        /* */

        #region Declarations

        /*  --------------------------------------------------------------------
         *  Declarations
         *  --------------------------------------------------------------------
         */

        /// <summary>INI section.</summary>
        private const string iniSection = "POP_ACCOUNT";

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Class constructor.</summary>
        public XPopAccount()
        {
            Clear(); 
        }

        /// <summary>Class constructor.</summary>
        public XPopAccount(XPopAccount _PopAccount)
        {
            Assign(_PopAccount);
        }

        /// <summary>Class constructor.</summary>
        public XPopAccount(string _IniFileName)
        {
            Clear();
            Load(_IniFileName);
        }

        #endregion

        /* */

        #region Properties

        /*  --------------------------------------------------------------------
         *  Properties
         *  --------------------------------------------------------------------
         */

        /// <summary>Indicate if POP account is not defined.</summary>
        public bool Empty
        {
            get { return (Host.Trim().Length<1) || (User.Trim().Length<1) || (Password.Trim().Length<1); }
        }

        /// <summary>Get or set POP host name.</summary>
        public string Host { get; set; }

        /// <summary>Get or set POP user name.</summary>
        public string User { get; set; }

        /// <summary>Get or set POP password.</summary>
        public string Password { get; set; }

        /// <summary>Get or set POP email.</summary>
        public string Email { get; set; }

        /// <summary>Get or set POP port number.</summary>
        public int Port { get; set; }

        /// <summary>Get or set POP SSL activation flag.</summary>
        public bool SSL { get; set; }

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Assign item properties from another.</summary>
        public void Assign(XPopAccount _PopAccount)
        {
            Host = _PopAccount.Host;
            User = _PopAccount.User;
            Password = _PopAccount.Password;
            Email = _PopAccount.Email;
            Port = _PopAccount.Port;
            SSL = _PopAccount.SSL;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Host = "";
            User = "";
            Password = "";
            Email = "";
            Port = 0;
            SSL = false;
        }

        /// <summary>Set instance value from tagged string.</summary>
        public bool FromString(string _TaggedString)
        {
            bool r = false;
            _TaggedString = XTag.Extract(_TaggedString, "xpopaccount", "\t");
            if (_TaggedString.Length > 0)
            {
                Host = XTag.Get(_TaggedString, "host", "");
                User = XTag.Get(_TaggedString, "user", "");
                Password = XTag.GetHexMask(_TaggedString, "password", "");
                Email = XTag.Get(_TaggedString, "email", "");
                Port = XTag.GetInt(_TaggedString, "port", 21);
                SSL = XTag.GetBool(_TaggedString, "ssl", false);
                r = true;
            }
            else Clear();
            return r;
        }

        /// <summary>Load item from ini file. Return true if succeed.</summary>
        public bool Load(string _IniFileName)
        {
            try
            {
                Clear();
                XIni ini = new XIni(_IniFileName);
                Host = ini.ReadString(iniSection, "HOST", Host);
                User = ini.ReadString(iniSection, "USER", User);
                Password = ini.ReadHexMask(iniSection, "PASS", Password);
                Email = ini.ReadString(iniSection, "EMAIL", Email);
                Port = ini.ReadInteger(iniSection, "PORT", Port);
                SSL = ini.ReadBool(iniSection, "SSL", SSL);
                return true;
            }
            catch (Exception ex)
            {
                XError.Internal(ex);
                return false;
            }
        }

        /// <summary>Save item to ini file. Return true if succeed.</summary>
        public bool Save(string _IniFileName)
        {
            try
            {
                XIni ini = new XIni(_IniFileName);
                ini.WriteString(iniSection, "HOST", Host);
                ini.WriteString(iniSection, "USER", User);
                ini.WriteHexMask(iniSection, "PASS", Password);
                ini.WriteString(iniSection, "EMAIL", Email);
                ini.WriteInteger(iniSection, "PORT", Port);
                ini.WriteBool(iniSection, "SSL", SSL);
                return ini.Save();
            }
            catch (Exception ex)
            {
                XError.Internal(ex);
                return false;
            }
        }

        /// <summary>Return tagged string containing instance value.</summary>
        public override string ToString()
        {
            string r = XTag.Set("", "host", Host);
            r = XTag.Set(r, "user", User);
            r = XTag.SetHexMask(r, "password", Password);
            r = XTag.Set(r, "email", Email);
            r = XTag.SetInt(r, "port", Port);
            r = XTag.SetBool(r, "ssl", SSL);
            r = XTag.Embedd(r, "xpopaccount", "\t");
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
