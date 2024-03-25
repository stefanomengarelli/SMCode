/*  ===========================================================================
 *  
 *  File:       Errors.cs
 *  Version:    2.0.0
 *  Date:       February 2024
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

using System.Diagnostics;

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: errors.</summary>
    public partial class SMApplication
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
            Output((ToStr(DateTime.Now, DateFormat, true) + " - " + ExecutableName + " - " + ErrorMessage + " " + ExceptionMessage).Trim());
        }

        /// <summary>Set last error exception.</summary>
        public void Error(Exception _Exception)
        {

            if (_Exception == null) ErrorMessage = "";
            else if (_Exception.Message == null) ErrorMessage = "";
            else ErrorMessage = _Exception.Message;
            Exception = _Exception;
            if (OnError != null) OnError(ErrorMessage, Exception);
            Output((ToStr(DateTime.Now, DateFormat, true) + " - " + ExecutableName + " - " + ErrorMessage + " " + ExceptionMessage).Trim());
        }

        /// <summary>Se si è in modalità di debug scrive il messaggio passato
        /// sulla finestra di output.</summary>
        public void Output(string _Message)
        {
            if (IsDebugger()) Debug.WriteLine(_Message);
        }

        /// <summary>Set last error and throw exception if specified.</summary>
        public void Raise(string _ErrorMessage, bool _RaiseException)
        {
            ErrorMessage = _ErrorMessage;
            Exception = new Exception(_ErrorMessage);
            if (_RaiseException) throw Exception;
        }

        /// <summary>Get stack trace.</summary>
        public string StackTrace()
        {
            try
            {
                if (Environment.StackTrace == null) return "";
                else return Environment.StackTrace;
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
