/*  ===========================================================================
 *  
 *  File:       SMFtpAccount.cs
 *  Version:    2.0.124
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  FTP account class.
 *  
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>FTP account class.</summary>
    public class SMFtpAccount
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>INI section.</summary>
        private const string iniSection = "FTP_ACCOUNT";

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFtpAccount(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear(); 
        }

        /// <summary>Class constructor.</summary>
        public SMFtpAccount(SMFtpAccount _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMFtpAccount(string _IniFileName, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
            Load(_IniFileName);
        }

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
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

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign item properties from another.</summary>
        public void Assign(SMFtpAccount _FtpAccount)
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

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMFtpAccount)JsonSerializer.Deserialize(_JSON, null));
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Assign property from JSON64 serialization.</summary>
        public bool FromJSON64(string _JSON64)
        {
            return FromJSON(SM.Base64Decode(_JSON64));
        }

        /// <summary>Load item from ini file. Return true if succeed.</summary>
        public bool Load(string _IniFileName)
        {
            try
            {
                Clear();
                SMIni ini = new SMIni(_IniFileName, SM);
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
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Save item to ini file. Return true if succeed.</summary>
        public bool Save(string _IniFileName)
        {
            try
            {
                SMIni ini = new SMIni(_IniFileName);
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
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Return JSON serialization of instance.</summary>
        public string ToJSON()
        {
            try
            {
                return JsonSerializer.Serialize(this);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return "";
            }
        }

        /// <summary>Return JSON64 serialization of instance.</summary>
        public string ToJSON64(SMCode _SM = null)
        {
            return SM.Base64Encode(ToJSON());
        }

        /// <summary>Return string containing instance value.</summary>
        public override string ToString()
        {
            return ToJSON();
        }

        #endregion

        /* */

    }

    /* */

}
