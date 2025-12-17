/*  ===========================================================================
 *  
 *  File:       Errors.cs
 *  Version:    2.0.250
 *  Date:       April 2025
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
using System.Text;

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

        /// <summary>Get error history string builder.</summary>
        public StringBuilder ErrorHistory { get; private set; } = new StringBuilder();

        /// <summary>Get or set error history enabled.</summary>
        public virtual bool ErrorHistoryEnabled { get; set; } = false;

        /// <summary>Get or set error verbose flag.</summary>
        public virtual bool ErrorLog { get; set; } = true;

        /// <summary>Get or set last error message.</summary>
        public virtual string ErrorMessage { get; set; } = "";

        /// <summary>Get or set last exception.</summary>
        public virtual Exception Exception { get; set; } = null;

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

        /// <summary>Get or set error throw exception flag.</summary>
        public virtual bool ThrowException { get; set; } = false;

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
        /// <param name="_IgnoreThrowException">If setted ignore ThrowException property.</param>
        public void Error(string _Error, Exception _Exception, bool _IgnoreThrowException = false)
        {
            bool handled = false;
            ErrorMessage = _Error;
            Exception = _Exception;
            if (OnError != null) OnError(ErrorMessage, Exception, ref handled);
            if (!handled)
            {
                if (ErrorLog)
                {
                    Log(SMLogType.Error, ErrorMessage, ExceptionMessages(Exception), "");
                    if (ErrorHistoryEnabled) ErrorHistory.Append(LastLog);
                }
                if (!_IgnoreThrowException && ThrowException)
                {
                    if (Exception == null) throw new Exception(ErrorMessage);
                    else throw Exception;
                }
            }
        }

        /// <summary>Set last error exception.</summary>
        /// <param name="_Exception">The exception to set.</param>
        /// <param name="_Message">The error message to set (optional).</param>
        /// <param name="_IgnoreThrowException">If setted ignore ThrowException property.</param>
        public void Error(Exception _Exception, string _Message = null, bool _IgnoreThrowException = false)
        {
            bool handled = false;
            if (_Message == null)
            {
                if (_Exception == null) ErrorMessage = "";
                else if (_Exception.Message == null) ErrorMessage = "";
                else ErrorMessage = _Exception.Message;
            }
            else ErrorMessage = _Message;
            Exception = _Exception;
            if (OnError != null) OnError(ErrorMessage, Exception, ref handled);
            if (!handled)
            {
                if (ErrorLog)
                {
                    Log(SMLogType.Error, ErrorMessage, ExceptionMessages(Exception), "");
                    if (ErrorHistoryEnabled) ErrorHistory.Append(LastLog);
                }
                if (!_IgnoreThrowException && ThrowException)
                {
                    if (Exception == null) throw new Exception(ErrorMessage);
                    else throw Exception;
                }
            }
        }

        /// <summary>Return history of errors, and delete it if specified.</summary>
        public string ErrorHistoryOutput(bool _DeleteHistory = false)
        {
            string rslt = ErrorHistory.ToString();
            if (_DeleteHistory) ErrorHistory.Clear();
            return rslt;
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

        /// <summary>Return exception messages recursively on inner exceptions.</summary>
        public string ExceptionMessages(Exception _Exception, bool _Recursive = true)
        {
            StringBuilder rslt = new StringBuilder();
            if (Exception!=null)
            {
                rslt.AppendLine(_Exception.Message);
                if (_Recursive && (_Exception.InnerException!=null))
                {
                    rslt.Append("InnerException: ");
                    rslt.AppendLine(ExceptionMessages(_Exception.InnerException, true));
                }
            }
            return rslt.ToString();
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
