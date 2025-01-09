/*  ===========================================================================
 *  
 *  File:       SMPopAccount.cs
 *  Version:    2.0.124
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  POP account class.
 *  
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>POP account class.</summary>
    public class SMPopAccount
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>INI section.</summary>
        private const string iniSection = "POP_ACCOUNT";

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
        public SMPopAccount(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear(); 
        }

        /// <summary>Class constructor.</summary>
        public SMPopAccount(SMPopAccount _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMPopAccount(string _IniFileName, SMCode _SM = null)
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

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign item properties from another.</summary>
        public void Assign(SMPopAccount _PopAccount)
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

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMPopAccount)JsonSerializer.Deserialize(_JSON, null));
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
                SMIni ini = new SMIni(_IniFileName);
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
                ini.WriteString(iniSection, "EMAIL", Email);
                ini.WriteInteger(iniSection, "PORT", Port);
                ini.WriteBool(iniSection, "SSL", SSL);
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
