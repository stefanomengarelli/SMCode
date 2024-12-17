/*  ===========================================================================
 *  
 *  File:       Errors.cs
 *  Version:    2.0.112
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: errors.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: errors.</summary>
    public partial class SMCode
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>On error customizable event.</summary>
        public SMOnError OnError = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last error message.</summary>
        public string ErrorMessage { get; set; }

        /// <summary>Error write log flag.</summary>
        public bool ErrorWriteLog = true;

        /// <summary>Get or set error verbose flag.</summary>
        public bool ErrorVerbose { get; set; }

        /// <summary>Get or set last exception.</summary>
        public Exception Exception { get; set; }

        /// <summary>Get or set last exception message as string.</summary>
        public string ExceptionMessage 
        { 
            get
            {
                if (Exception == null) return "";
                else return Exception.Message;
            }
        }

        /// <summary>Return true if last error instance contains error.</summary>
        public bool IsError
        {
            get { return (ErrorMessage.Trim().Length > 0) || (Exception != null); }
        }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize error management class environment.</summary>
        public void InitializeErrors()
        {
            ErrorMessage = "";
            ErrorVerbose = false;
            Exception = null;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Clear last error.</summary>
        public void Error()
        {
            ErrorMessage = "";
            Exception = null;
        }

        /// <summary>Set last error.</summary>
        public void Error(string _Error, Exception _Exception)
        {
            ErrorMessage = _Error;
            Exception = _Exception;
            if (OnError != null) OnError(ErrorMessage, Exception);
            if (ErrorWriteLog)
            {
                ErrorWriteLog = false;
                Log(SMLogType.Error, ErrorMessage, ExceptionMessage, "");
                ErrorWriteLog = true;
            }
        }

        /// <summary>Set last error exception.</summary>
        public void Error(Exception _Exception, string _Message = null)
        {
            if (_Message == null)
            {
                if (_Exception == null) ErrorMessage = "";
                else if (_Exception.Message == null) ErrorMessage = "";
                else ErrorMessage = _Exception.Message;
            }
            else ErrorMessage = _Message;
            Exception = _Exception;
            if (OnError != null) OnError(ErrorMessage, Exception);
            if (ErrorWriteLog)
            {
                ErrorWriteLog = false;
                Log(SMLogType.Error, ErrorMessage, ExceptionMessage, "");
                ErrorWriteLog = true;
            }
        }

        /// <summary>Return error message after prefix and including exception if specified.</summary>
        public string ErrorStr(string _Prefix = "", bool _IncludeException = false)
        {
            if (_Prefix == null) _Prefix = "";
            else _Prefix = _Prefix.Trim();
            if (Empty(ErrorMessage)) _Prefix = Cat(_Prefix, "Nessun errore rilevato.", " - ");
            else _Prefix = Cat(_Prefix, ErrorMessage, " - ");
            if (_IncludeException && (Exception != null))
            {
                _Prefix = Cat(_Prefix, Exception.Message, " - ");
            }
            return _Prefix;
        }

        /// <summary>Set last error and throw exception if specified.</summary>
        public void Raise(string _ErrorMessage, bool _RaiseException)
        {
            ErrorMessage = _ErrorMessage;
            Exception = new Exception(_ErrorMessage);
            if (_RaiseException) throw Exception;
        }

        /// <summary>Set last error and throw exception if specified.</summary>
        public void Raise(Exception _Exception, bool _RaiseException)
        {
            ErrorMessage = _Exception.Message;
            Exception = _Exception;
            if (_RaiseException) throw Exception;
        }

        /// <summary>Get stack trace.</summary>
        public string StackTrace()
        {
            try
            {
                if (System.Environment.StackTrace == null) return "";
                else return System.Environment.StackTrace;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        /* */

    }

    /* */

}
