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

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: logger.</summary>
    public partial class SMApplication
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set default log file path.</summary>
        public string DefaultLogFilePath { get; set; } = "";

        /// <summary>Get or set log max file size.</summary>
        public long LogFileMaxSize { get; set; } = 8192000;

        /// <summary>Get or set log max history files.</summary>
        public int LogFileMaxHistory { get; set; } = 32;

        /// <summary>Get or set log line separator.</summary>
        public string LogSeparator { get; set; } = "================================================================================";

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Write log on log file, log file path is empty write log on default application log file.</summary>
        public bool Log(SMLogType _LogType, string _Message = "", string _Details = "", string _LogFile = "")
        {
            bool r = false;
            if (this.Initialized)
            {
                if (Empty(_LogFile)) _LogFile = DefaultLogFilePath;
                SMLogItem item = new SMLogItem(DateTime.Now, _LogType, _Message, _Details, ExecutableName, Version, this);
                if (FileExists(_LogFile))
                {
                    if (FileSize(_LogFile) > LogFileMaxSize)
                    {
                        if (FileHistory(_LogFile, LogFileMaxHistory)) FileDelete(_LogFile);
                    }
                }
                AppendString(_LogFile, item.ToString() + "\r\n", TextEncoding, FileRetries);
                if (!Empty(_Message) && IsDebugger())
                {
                    Output(_Message.Trim());
                    if (!Empty(_Details)) Output(_Details.Trim());
                }
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
