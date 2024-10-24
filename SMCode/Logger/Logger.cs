/*  ===========================================================================
 *  
 *  File:       Logger.cs
 *  Version:    2.0.54
 *  Date:       October 2024
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

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: logger.</summary>
    public partial class SMCode
    {

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
            bool rslt = false;
            SMDataset ds;
            if (this.Initialized)
            {
                if (!Empty(_Message) 
                    || (_LogType == SMLogType.Separator) 
                    || (_LogType == SMLogType.Line))
                {
                    if (Empty(_LogFile)) _LogFile = DefaultLogFilePath;
                    LastLog.DateTime = _Date;
                    LastLog.LogType = _LogType;
                    if ((LastLog.LogType == SMLogType.Separator) && SM.Empty(_Message)) LastLog.Message = LogSeparator;
                    else if ((LastLog.LogType == SMLogType.Line) && SM.Empty(_Message)) LastLog.Message = LogLine;
                    else LastLog.Message = _Message;
                    LastLog.Details = _Details;
                    LastLog.About = SM.About();
                    rslt = SM.Empty(LastLog);
                    if (!rslt)
                    {
                        if (FileExists(_LogFile))
                        {
                            if (FileSize(_LogFile) > LogFileMaxSize)
                            {
                                if (FileHistory(_LogFile, LogFileMaxHistory)) FileDelete(_LogFile);
                            }
                        }
                        rslt = AppendString(_LogFile, LastLog.AsString() + "\r\n", TextEncoding, FileRetries);
                    }
                    if (IsDebugger())
                    {
                        Output(LastLog.AsString().Replace("|", "\r\n"));
                    }
                    if (!SM.Empty(LogDBAlias))
                    {
                        rslt = false;
                        ds = new SMDataset(LogDBAlias, this);
                        if (ds.Open("SELECT * FROM sm_logs WHERE IdLog<0"))
                        {
                            if (ds.Append())
                            {
                                ds["UidLog"] = SM.GUID();
                                ds["DateTime"] = LastLog.DateTime;
                                ds["About"] = LastLog.About;
                                ds["IdUser"] = User.IdUser;
                                ds["UidUser"] = User.UidUser;
                                ds["LogType"] = LogType(LastLog.LogType);
                                ds["Message"] = LastLog.Message;
                                ds["Details"] = LastLog.Details;
                                rslt = ds.Post();
                                if (!rslt) ds.Cancel();
                            }
                            ds.Close();
                        }
                        ds.Dispose();
                    }
                    if (OnLogEvent != null) OnLogEvent(LastLog);
                }
            }
            return rslt;
        }

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogType _LogType, string _Message = "", string _Details = "", string _LogFile = "")
        {
            return Log(DateTime.Now, _LogType, _Message,_Details, _LogFile);
        }

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogItem _Item, string _LogFile = "")
        {
            return Log(_Item.DateTime, _Item.LogType, _Item.Message, _Item.Details, _LogFile);
        }

        /// <summary>Return 3 chars length string representing log type.</summary>
        public SMLogType LogType(string _String)
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
        public string LogType(SMLogType _LogType)
        {
            if (_LogType == SMLogType.Information) return @"INFO";
            else if (_LogType == SMLogType.Warning) return @"!WRN";
            else if (_LogType == SMLogType.Error) return @"*ERR";
            else if (_LogType == SMLogType.Debug) return @"$DBG";
            else if (_LogType == SMLogType.Separator) return @"====";
            else if (_LogType == SMLogType.Line) return @"----";
            else if (_LogType == SMLogType.Login) return @"LOGN";
            else if (_LogType == SMLogType.Event) return @"EVNT";
            else if (_LogType == SMLogType.Action) return @"ACTN";
            else return @"    ";
        }

        #endregion

        /* */

    }

    /* */

}
