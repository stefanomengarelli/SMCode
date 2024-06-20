/*  ===========================================================================
 *  
 *  File:       SMLogItem.cs
 *  Version:    2.0.0
 *  Date:       February 2024
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
        public string Application { get; set; }

        /// <summary>Get or set log date-time value.</summary>
        public DateTime Date { get; set; }

        /// <summary>Get or set log details value.</summary>
        public string Details { get; set; }

        /// <summary>Get or set log message value.</summary>
        public string Message { get; set; }

        /// <summary>Get or set log type value.</summary>
        public SMLogType Type { get; set; }

        /// <summary>Get or set log application version value.</summary>
        public string Version { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMLogItem(SMCode _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMCode.CurrentOrNew();
            else SM = _SMApplication;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(SMLogItem _LogItem, SMCode _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMCode.CurrentOrNew();
            else SM = _SMApplication;
            Assign(_LogItem);
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(DateTime _Date, SMLogType _Type, string _Message = "", string _Details = "", string _Application = "", string _Version = "", SMCode _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMCode.CurrentOrNew();
            else SM = _SMApplication;
            this.Application = _Application;
            this.Date = _Date;
            this.Details = _Details;
            if (SM.Empty(_Message) && (_Type == SMLogType.Separator))
            {
                _Message = "================================================================================";
            }
            if (SM.Empty(_Message) && (_Type == SMLogType.Line))
            {
                _Message = "--------------------------------------------------------------------------------";
            }
            this.Message = SM.Flat(_Message);
            this.Type = _Type;
            this.Version = _Version;
        }

        /// <summary>Class constructor.</summary>
        public SMLogItem(string _String, SMCode _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMCode.CurrentOrNew();
            else SM = _SMApplication;
            FromString(_String);
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
            this.Application = _LogItem.Application;
            this.Date = _LogItem.Date;
            this.Details = _LogItem.Details;
            this.Message = SM.Flat(_LogItem.Message);
            this.Type = _LogItem.Type;
            this.Version = _LogItem.Version;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            this.Application = "";
            this.Date = DateTime.MinValue;
            this.Details = "";
            this.Message = "";
            this.Type = SMLogType.None;
            this.Version = "";
        }

        /// <summary>Get log item values from comma separated string.</summary>
        public bool FromString(string _String)
        {
            Clear();
            try
            {
                this.Date = DateTime.Parse(SM.Extract(ref _String, ' ').Trim() + " " + SM.Extract(ref _String, ' ').Trim());
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            if (this.Date.Year > 2019)
            {
                this.Type = TypeFromStr(SM.Extract(ref _String, ' '));
                this.Application = SM.Extract(ref _String, ' ').Trim();
                if (this.Application.StartsWith("[")) this.Application = this.Application.Substring(1);
                this.Version = SM.Extract(ref _String, ' ').Trim();
                if (this.Version.EndsWith("]")) this.Version = this.Version.Substring(0, this.Version.Length - 1);
                if (_String.IndexOf('|') > -1)
                {
                    this.Message = SM.Extract(ref _String, '|').Trim();
                    while (_String.IndexOf('|') > -1) this.Details += SM.Extract(ref _String, '|').Trim() + "\r\n";
                    if (_String.Trim().Length > 0) this.Details += _String.Trim();
                }
                else this.Message = _String;
                return true;
            }
            else
            {
                this.Date = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>Return log item as comma separated string.</summary>
        public override string ToString()
        {
            string r = this.Date.ToString(@"yyyy-MM-dd HH:mm:ss.fff")
                + ' ' + TypeToStr(this.Type)
                + @" [" + this.Application.Trim()
                + ' ' + this.Version.Trim() + ']'
                + ' ' + this.Message.Trim();
            if (this.Details.Trim().Length > 0) r += '|' + this.Details.Replace('\n', '|').Replace("\r", "");
            return r;
        }

        /// <summary>Return 3 chars length string representing log type.</summary>
        public SMLogType TypeFromStr(string _String)
        {
            _String = _String.Trim().ToUpper();
            if (_String == @"INFO") return SMLogType.Information;
            else if (_String == @"WARN") return SMLogType.Warning;
            else if (_String == @"*ERR") return SMLogType.Error;
            else if (_String == @"!DBG") return SMLogType.Debug;
            else if (_String == @"====") return SMLogType.Separator;
            else if (_String == @"----") return SMLogType.Line;
            else return SMLogType.None;
        }

        /// <summary>Return 3 chars length string representing log type.</summary>
        public string TypeToStr(SMLogType _Type)
        {
            if (_Type == SMLogType.Information) return @"INFO";
            else if (_Type == SMLogType.Warning) return @"WARN";
            else if (_Type == SMLogType.Error) return @"*ERR";
            else if (_Type == SMLogType.Debug) return @"!DBG";
            else if (_Type == SMLogType.Separator) return @"====";
            else if (_Type == SMLogType.Line) return @"----";
            else return @"    ";
        }

        #endregion

        /* */

    }

    /* */

}
