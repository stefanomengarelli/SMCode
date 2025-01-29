/*  ===========================================================================
 *  
 *  File:       SMLogItem.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode log item class.
 *
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode log item class.</summary>
    public class SMLogItem
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

        /// <summary>Get or set log date-time value.</summary>
        public DateTime DateTime { get; set; }

        /// <summary>Get or set log application name value.</summary>
        public string Application { get; set; }

        /// <summary>Get or set log application version value.</summary>
        public string Version { get; set; }

        /// <summary>Get or set user id.</summary>
        public int IdUser { get; set; }

        /// <summary>Get or set user Uid.</summary>
        public string UidUser { get; set; }

        /// <summary>Get or set log type value.</summary>
        public SMLogType LogType { get; set; }

        /// <summary>Get or set log action value.</summary>
        public string Action { get; set; }

        /// <summary>Get or set log message value.</summary>
        public string Message { get; set; }

        /// <summary>Get or set log details value.</summary>
        public string Details { get; set; }

        /// <summary>Get or set object tag.</summary>
        public object Tag { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMLogItem(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(SMLogItem _LogItem, SMCode _SM)
        {
            if (_SM == null) _SM = _LogItem.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_LogItem);
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(DateTime _Date, SMLogType _Type, string _Message, string _Details, string _Action, SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            this.DateTime = _Date;
            this.Application = SM.ExecutableName;
            this.Version = SM.Version + " - " + SM.ToStr(SM.ExecutableDate, true);
            this.IdUser = SM.User.IdUser;
            this.UidUser = SM.User.UidUser;
            this.LogType = _Type;
            this.Action = _Message;
            if (SM.Empty(_Message) && (_Type == SMLogType.Separator)) _Message = SM.LogSeparator;
            if (SM.Empty(_Message) && (_Type == SMLogType.Line)) _Message = SM.LogLine;
            this.Message = SM.Flat(_Message);
            this.Details = _Details;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMLogItem _LogItem)
        {
            this.DateTime = _LogItem.DateTime;
            this.Application = _LogItem.Application;
            this.Version = _LogItem.Version;
            this.IdUser = _LogItem.IdUser;
            this.UidUser= _LogItem.UidUser;
            this.LogType = _LogItem.LogType;
            this.Action = _LogItem.Action;
            this.Message = SM.Flat(_LogItem.Message);
            this.Details = _LogItem.Details;
            this.Tag = _LogItem.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            this.DateTime = DateTime.MinValue;
            this.Application = "";
            this.Version = "";
            this.IdUser = 0;
            this.UidUser = "";
            this.LogType = SMLogType.None;
            this.Action = "";
            this.Message = "";
            this.Details = "";
            this.Tag = null;
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMLogItem)JsonSerializer.Deserialize(_JSON, null));
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
        public string ToJSON64()
        {
            return SM.Base64Encode(ToJSON());
        }

        /// <summary>Return log item as full description string.</summary>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>Return log item as full description string.</summary>
        public string ToString(bool _FlatString)
        {
            string r = this.DateTime.ToString(@"yyyy-MM-dd HH:mm:ss.fff") + " " + SM.LogType(this.LogType)
                + @" [" + SM.Cat(this.Application.Trim(), this.Version.Trim(), " - ") + @"] " + this.Message.Trim(), s = "\r\n";
            if (_FlatString) s = "|";
            if (this.Details.Trim().Length > 0) r += s + this.Details.Replace("\n", s).Replace("\r", "");
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
