/*  ===========================================================================
 *  
 *  File:       Logger.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: logger.
 *
 *  ===========================================================================
 */

using System;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: logger.</summary>
    public partial class SMCode
    {

        /* */

        #region Delegates and events

        /*  ===================================================================
         *  Delegates and events
         *  ===================================================================
         */

        /// <summary>Occurs when log function called.</summary>
        public event SMOnLog OnLogEvent = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set default log file path.</summary>
        public string DefaultLogFilePath { get; set; } = "";

        /// <summary>Last log.</summary>
        public SMLogItem LastLog { get; private set; } = null;

        /// <summary>Get or set log database alias.</summary>
        public string LogDBAlias { get; set; } = "";

        /// <summary>Get or set log max file size.</summary>
        public long LogFileMaxSize { get; set; } = 8192000;

        /// <summary>Get or set log max history files.</summary>
        public int LogFileMaxHistory { get; set; } = 32;

        /// <summary>Get or set log separator.</summary>
        public string LogSeparator { get; set; } = "================================================================================";

        /// <summary>Get or set log line.</summary>
        public string LogLine { get; set; } = "--------------------------------------------------------------------------------";

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(DateTime _Date, SMLogType _LogType, string _Message = "", string _Details = "", string _LogFile = "")
        {
            SMDataset ds;
            if (this.Initialized)
            {
                if (!Empty(_Message) 
                    || (_LogType == SMLogType.Separator) 
                    || (_LogType == SMLogType.Line))
                {
                    if (Empty(_LogFile)) _LogFile = DefaultLogFilePath;
                    LastLog.Date = _Date;
                    LastLog.Type = _LogType;
                    if ((LastLog.Type == SMLogType.Separator) && (_Message == "")) LastLog.Message = LogSeparator;
                    else if ((LastLog.Type == SMLogType.Line) && (_Message == "")) LastLog.Message = LogLine;
                    else LastLog.Message = _Message;
                    LastLog.Details = _Details;
                    LastLog.Application = ExecutableName;
                    LastLog.Version = Version;
                    if (!SM.Empty(_LogFile))
                    {
                        if (FileExists(_LogFile))
                        {
                            if (FileSize(_LogFile) > LogFileMaxSize)
                            {
                                if (FileHistory(_LogFile, LogFileMaxHistory)) FileDelete(_LogFile);
                            }
                        }
                        LastLog.Wrote = AppendString(_LogFile, LastLog.ToString() + "\r\n", TextEncoding, FileRetries);
                    }
                    if (IsDebugger())
                    {
                        Output(_Message.Trim());
                        if (!Empty(_Details)) Output(_Details.Trim());
                    }
                    if (!SM.Empty(LogDBAlias))
                    {
                        ds = new SMDataset(LogDBAlias, this);
                        if (ds.Open("SELECT * FROM sm_logs WHERE IdLog<0"))
                        {
                            if (ds.Append())
                            {
                                ds["UidLog"] = SM.GUID();
                                ds["DateTime"] = LastLog.Date;
                                ds["IdUser"] = User.Id;
                                ds["UidUser"] = User.Uid;
                                ds["Type"] = LogTypeToStr(LastLog.Type);
                                ds["Message"] = LastLog.Message;
                                ds["Details"] = LastLog.Details;
                                if (!ds.Post()) ds.Cancel();
                            }
                            ds.Close();
                        }
                        ds.Dispose();
                    }
                    if (OnLogEvent != null) OnLogEvent(LastLog);
                    return LastLog.Wrote;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogType _LogType, string _Message = "", string _Details = "", string _LogFile = "")
        {
            return Log(DateTime.Now, _LogType, _Message,_Details, _LogFile);
        }

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogItem _Item, string _LogFile = "")
        {
            return Log(_Item.Date, _Item.Type, _Item.Message, _Item.Details, _LogFile);
        }

        /// <summary>Return 3 chars length string representing log type.</summary>
        public SMLogType LogTypeFromStr(string _String)
        {
            _String = _String.Trim().ToUpper();
            if (_String == @"INFO") return SMLogType.Information;
            else if (_String == @"!WRN") return SMLogType.Warning;
            else if (_String == @"*ERR") return SMLogType.Error;
            else if (_String == @"$DBG") return SMLogType.Debug;
            else if (_String == @"====") return SMLogType.Separator;
            else if (_String == @"----") return SMLogType.Line;
            else if (_String == @"LOGN") return SMLogType.Login;
            else if (_String == @"EVNT") return SMLogType.Event;
            else if (_String == @"ACTN") return SMLogType.Action;
            else return SMLogType.None;
        }

        /// <summary>Return 3 chars length string representing log type.</summary>
        public string LogTypeToStr(SMLogType _Type)
        {
            if (_Type == SMLogType.Information) return @"INFO";
            else if (_Type == SMLogType.Warning) return @"!WRN";
            else if (_Type == SMLogType.Error) return @"*ERR";
            else if (_Type == SMLogType.Debug) return @"$DBG";
            else if (_Type == SMLogType.Separator) return @"====";
            else if (_Type == SMLogType.Line) return @"----";
            else if (_Type == SMLogType.Login) return @"LOGN";
            else if (_Type == SMLogType.Event) return @"EVNT";
            else if (_Type == SMLogType.Action) return @"ACTN";
            else return @"    ";
        }

        #endregion

        /* */

    }

    /* */

}
