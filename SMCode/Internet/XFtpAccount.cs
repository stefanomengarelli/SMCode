/*  ------------------------------------------------------------------------
 *  
 *  File:       XFtpAccount.cs
 *  Version:    1.0.0
 *  Date:       March 2021
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@microrun.it
 *  
 *  Copyright (C) 2022 by Stefano Mengarelli - All rights reserved - Use, permission and restrictions under license.
 *
 *  Microrun XRad FTP account class.
 *  
 *  Dependencies: XError, XIni, XTag
 *
 *  ------------------------------------------------------------------------
 */

using System;

namespace XRadSharp
{

    /* */

    /// <summary>Microrun XRad FTP account class.</summary>
    public class XFtpAccount
    {

        /* */

        #region Declarations

        /*  --------------------------------------------------------------------
         *  Declarations
         *  --------------------------------------------------------------------
         */

        /// <summary>INI section.</summary>
        private const string iniSection = "FTP_ACCOUNT";

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Class constructor.</summary>
        public XFtpAccount()
        {
            Clear(); 
        }

        /// <summary>Class constructor.</summary>
        public XFtpAccount(XFtpAccount _FtpAccount)
        {
            Assign(_FtpAccount);
        }

        /// <summary>Class constructor.</summary>
        public XFtpAccount(string _IniFileName)
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

        /// <summary>Indicate if FTP account is not defined.</summary>
        public bool Empty
        {
            get { return (Host.Trim().Length<1) || (User.Trim().Length<1) || (Password.Trim().Length<1); }
        }

        /// <summary>Get or set FTP host name.</summary>
        public string Host { get; set; }

        /// <summary>Get or set FTP user name.</summary>
        public string User { get; set; }

        /// <summary>Get or set FTP password.</summary>
        public string Password { get; set; }

        /// <summary>Get or set FTP port number.</summary>
        public int Port { get; set; }

        /// <summary>Get or set FTP SSL activation flag.</summary>
        public bool SSL { get; set; }

        /// <summary>Get or set FTP passive mode flag.</summary>
        public bool Passive { get; set; }

        /// <summary>Get or set FTP binary mode flag.</summary>
        public bool Binary { get; set; }

        /// <summary>Get or set FTP keep alive mode flag.</summary>
        public bool KeepAlive { get; set; }

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Assign item properties from another.</summary>
        public void Assign(XFtpAccount _FtpAccount)
        {
            Host = _FtpAccount.Host;
            User = _FtpAccount.User;
            Password = _FtpAccount.Password;
            Port = _FtpAccount.Port;
            SSL = _FtpAccount.SSL;
            Passive = _FtpAccount.Passive;
            Binary = _FtpAccount.Binary;
            KeepAlive = _FtpAccount.KeepAlive;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Host = "";
            User = "";
            Password = "";
            Port = 0;
            SSL = false;
            Passive = true;
            Binary = true;
            KeepAlive = true;
        }

        /// <summary>Set instance value from tagged string.</summary>
        public bool FromString(string _TaggedString)
        {
            bool r = false;
            _TaggedString = XTag.Extract(_TaggedString, "xftpaccount", "\t");
            if (_TaggedString.Length > 0)
            {
                Host = XTag.Get(_TaggedString, "host", "");
                User = XTag.Get(_TaggedString, "user", "");
                Password = XTag.GetHexMask(_TaggedString, "password", "");
                Port = XTag.GetInt(_TaggedString, "port", 21);
                SSL = XTag.GetBool(_TaggedString, "ssl", false);
                Passive = XTag.GetBool(_TaggedString, "passive", true);
                Binary = XTag.GetBool(_TaggedString, "binary", true);
                KeepAlive = XTag.GetBool(_TaggedString, "keep_alive", false);
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
                Port = ini.ReadInteger(iniSection, "PORT", Port);
                SSL = ini.ReadBool(iniSection, "SSL", SSL);
                Passive = ini.ReadBool(iniSection, "PASSIVE", Passive);
                Binary = ini.ReadBool(iniSection, "BINARY", Binary);
                KeepAlive = ini.ReadBool(iniSection, "KEEP_ALIVE", KeepAlive);
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
                ini.WriteInteger(iniSection, "PORT", Port);
                ini.WriteBool(iniSection, "SSL", SSL);
                ini.WriteBool(iniSection, "PASSIVE", Passive);
                ini.WriteBool(iniSection, "BINARY", Binary);
                ini.WriteBool(iniSection, "KEEP_ALIVE", KeepAlive);
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
            r = XTag.SetInt(r, "port", Port);
            r = XTag.SetBool(r, "ssl", SSL);
            r = XTag.SetBool(r, "passive", Passive);
            r = XTag.SetBool(r, "binary", Binary);
            r = XTag.SetBool(r, "keep_alive", KeepAlive);
            r = XTag.Embedd(r, "xftpaccount", "\t");
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
