/*  ===========================================================================
 *  
 *  File:       SMLogItem.cs
 *  Version:    2.0.54
 *  Date:       October 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
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

        /// <summary>Get or set log application name value.</summary>
        public string About { get; set; }

        /// <summary>Get or set log date-time value.</summary>
        public DateTime DateTime { get; set; }

        /// <summary>Get or set log details value.</summary>
        public string Details { get; set; }

        /// <summary>Get or set log message value.</summary>
        public string Message { get; set; }

        /// <summary>Get or set log type value.</summary>
        public SMLogType LogType { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMLogItem(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(SMLogItem _LogItem, SMCode _SM = null)
        {
            if (_SM == null) _SM = _LogItem.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_LogItem);
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(DateTime _Date, SMLogType _Type, string _Message = "", string _Details = "", SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            this.About = SM.About();
            this.DateTime = _Date;
            this.Details = _Details;
            if (SM.Empty(_Message) && (_Type == SMLogType.Separator)) _Message = SM.LogSeparator;
            if (SM.Empty(_Message) && (_Type == SMLogType.Line)) _Message = SM.LogLine;
            this.Message = SM.Flat(_Message);
            this.LogType = _Type;
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
            this.About = _LogItem.About;
            this.DateTime = _LogItem.DateTime;
            this.Details = _LogItem.Details;
            this.Message = SM.Flat(_LogItem.Message);
            this.LogType = _LogItem.LogType;
        }

        /// <summary>Return log item as full description string.</summary>
        public string AsString(bool _FlatString = true)
        {
            string r = this.DateTime.ToString(@"yyyy-MM-dd HH:mm:ss.fff") + " " + SM.LogType(this.LogType)
                + @" [" + this.About.Trim() + @"] " + this.Message.Trim(), s = "\r\n";
            if (_FlatString) s = "|";
            if (this.Details.Trim().Length > 0) r += s + this.Details.Replace("\n", s).Replace("\r", "");
            return r;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            this.About = "";
            this.DateTime = DateTime.MinValue;
            this.Details = "";
            this.Message = "";
            this.LogType = SMLogType.None;
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

        #endregion

        /* */

    }

    /* */

}
