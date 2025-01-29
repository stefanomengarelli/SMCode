/*  ===========================================================================
 *  
 *  File:       Logger.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
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
        public string LogAlias { get; set; } = "";

        /// <summary>Get or set log max file size.</summary>
        public long LogFileMaxSize { get; set; } = 8192000;

        /// <summary>Get or set log max history files.</summary>
        public int LogFileMaxHistory { get; set; } = 32;

        /// <summary>Get or set log separator.</summary>
        public string LogSeparator { get; set; } = "================================================================================";

        /// <summary>Get or set log line.</summary>
        public string LogLine { get; set; } = "--------------------------------------------------------------------------------";

        /// <summary>Get or set cache db table name.</summary>
        public string TableName { get; set; } = SMDefaults.LogsTableName;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(DateTime _Date, SMLogType _LogType, string _Message = "", string _Details = "", string _Action = "", string _LogFile = "")
        {
            SMLogItem logItem = new SMLogItem(this);
            logItem.DateTime = _Date;
            logItem.Application = ExecutableName;
            logItem.Version = Cat(Version, ToStr(ExecutableDate, true), " - ");
            logItem.IdUser = User.IdUser;
            logItem.UidUser = User.UidUser; 
            logItem.LogType = _LogType;
            logItem.Action = _Action;
            logItem.Message = _Message;
            logItem.Details = _Details; 
            return Log(logItem, _LogFile);
        }

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogType _LogType, string _Message = "", string _Details = "", string _Action = "", string _LogFile = "")
        {
            return Log(DateTime.Now, _LogType, _Message,_Details, _Action, _LogFile);
        }

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogItem _LogItem, string _LogFile = "")
        {
            bool rslt = false;
            SMDatabase db;
            SMDataset ds;
            if (this.Initialized && (_LogItem != null))
            {
                if (!Empty(_LogItem.Message)
                    || (_LogItem.LogType == SMLogType.Separator)
                    || (_LogItem.LogType == SMLogType.Line))
                {
                    try
                    {
                        if (Empty(_LogFile)) _LogFile = DefaultLogFilePath;
                        //
                        LastLog.Assign(_LogItem);
                        if ((LastLog.LogType == SMLogType.Separator) && Empty(LastLog.Message)) LastLog.Message = LogSeparator;
                        else if ((LastLog.LogType == SMLogType.Line) && Empty(LastLog.Message)) LastLog.Message = LogLine;
                        //
                        rslt = !Empty(LastLog.Message);
                        if (rslt)
                        {
                            if (FileExists(_LogFile))
                            {
                                if (FileSize(_LogFile) > LogFileMaxSize)
                                {
                                    if (FileHistory(_LogFile, LogFileMaxHistory)) FileDelete(_LogFile);
                                }
                            }
                            rslt = AppendString(_LogFile, LastLog.ToString() + "\r\n", TextEncoding, FileRetries);
                            if (IsDebugger())
                            {
                                Output(LastLog.ToString().Replace("|", "\r\n"));
                            }
                            if (!Empty(LogAlias))
                            {
                                db = new SMDatabase(this);
                                db.ParametersFrom(LogAlias);
                                ds = new SMDataset(db, this);
                                if (ds.Open($"SELECT * FROM {TableName} WHERE (IdLog<0)"))
                                {
                                    if (ds.Append())
                                    {
                                        ds["UidLog"] = GUID();
                                        ds["DateTime"] = LastLog.DateTime;
                                        ds["Application"] = LastLog.Application;
                                        ds["Version"] = LastLog.Version;
                                        ds["IdUser"] = User.IdUser;
                                        ds["UidUser"] = User.UidUser;
                                        ds["LogType"] = LogType(LastLog.LogType);
                                        ds["Action"] = LastLog.Action;
                                        ds["Message"] = LastLog.Message;
                                        ds["Details"] = LastLog.Details;
                                        if (!ds.Post())
                                        {
                                            ds.Cancel();
                                            rslt = false;
                                        }
                                    }
                                    ds.Close();
                                }
                                db.Close();
                                ds.Dispose();
                                db.Dispose();
                            }
                            if (OnLogEvent != null) OnLogEvent(LastLog);
                        }
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        rslt = false;
                    }
                }
            }
            return rslt;
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
