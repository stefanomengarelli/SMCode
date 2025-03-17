/*  ===========================================================================
 *  
 *  File:       Errors.cs
 *  Version:    2.0.221
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
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

        /// <summary>Get or set error verbose flag.</summary>
        public bool ErrorLog { get; set; }

        /// <summary>Get or set last error message.</summary>
        public string ErrorMessage { get; set; }

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
            ErrorLog = false;
            ErrorMessage = "";
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
        /// <param name="_Error">The error message to set.</param>
        /// <param name="_Exception">The exception associated with the error.</param>
        public void Error(string _Error, Exception _Exception)
        {
            ErrorMessage = _Error;
            Exception = _Exception;
            if (OnError != null) OnError(ErrorMessage, Exception);
            if (ErrorLog) Log(SMLogType.Error, ErrorMessage, ExceptionMessage, "");
        }

        /// <summary>Set last error exception.</summary>
        /// <param name="_Exception">The exception to set.</param>
        /// <param name="_Message">The error message to set (optional).</param>
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
            if (ErrorLog) Log(SMLogType.Error, ErrorMessage, ExceptionMessage, "");
        }

        /// <summary>Return error message after prefix and including exception if specified.</summary>
        /// <param name="_Prefix">The prefix to add to the error message.</param>
        /// <param name="_IncludeException">Whether to include the exception message.</param>
        /// <returns>The formatted error message.</returns>
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
        /// <param name="_ErrorMessage">The error message to set.</param>
        /// <param name="_RaiseException">Whether to throw the exception.</param>
        public void Raise(string _ErrorMessage, bool _RaiseException)
        {
            ErrorMessage = _ErrorMessage;
            Exception = new Exception(_ErrorMessage);
            if (_RaiseException) throw Exception;
        }

        /// <summary>Set last error and throw exception if specified.</summary>
        /// <param name="_Exception">The exception to set.</param>
        /// <param name="_RaiseException">Whether to throw the exception.</param>
        public void Raise(Exception _Exception, bool _RaiseException)
        {
            ErrorMessage = _Exception.Message;
            Exception = _Exception;
            if (_RaiseException) throw Exception;
        }

        /// <summary>Get stack trace.</summary>
        /// <returns>The current stack trace.</returns>
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
