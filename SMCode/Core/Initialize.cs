/*  ===========================================================================
 *  
 *  File:       Initialize.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: initialization.
 *  
 *  ===========================================================================
 */

using System;
using System.Diagnostics;

namespace SMCodeSystem
{

    /* */

    /// <summary>SM application class: initialization.</summary>
    public partial class SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last application instance created.</summary>
        public static SMCode SM { get; set; } = null;

        /// <summary>Get or set application passed arguments array (parameters).</summary>
        public string[] Arguments { get; set; } = null;

        /// <summary>Get application passed arguments array (parameters) count.</summary>
        public int ArgumentsCount
        {
            get
            {
                if (Arguments == null) return 0;
                else return Arguments.Length;
            }
        }

        /// <summary>SMCode core class initialized flag.</summary>
        public bool Initialized { get; private set; }

        /// <summary>SMCode core class initializing flag.</summary>
        public bool Initializing { get; private set; }

        /// <summary>Generic internal password.</summary>
        public string InternalPassword { get; set; }

        /// <summary>Parametri di configurazione dell'applicazione.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>OEM id.</summary>
        public string OEM { get; set; }

        /// <summary>.NET platform.</summary>
        public SMPlatform Platform { get; private set; } = SMPlatform.Unknown;

        /// <summary>Session UID.</summary>
        public string SessionUID { get; private set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance values with custom OEM identifier.</summary>
        public SMCode(string[] _Arguments = null, string _OEM = "", string _InternalPassword = "")
        {
            if (!Initialized && !Initializing)
            {
                Initializing = true;
                SM = this;
                //
                // Preliminary initializations
                //
                Arguments = _Arguments;
                if (_InternalPassword == "") InternalPassword = @"Mng5Fn$5MC0d3=R4d";
                else InternalPassword = _InternalPassword;
                OEM = _OEM;
                SessionUID = GUID();
                Parameters = new SMDictionary(this);
                //
                // Detect .NET platform
                //
#if NET47_OR_GREATER
                Platform = SMPlatform.NetFramework47;
#elif NET46_OR_GREATER
                Platform = SMPlatform.NetFramework46;
#elif NET45_OR_GREATER
                Platform = SMPlatform.NetFramework45;
#elif NET7_0_OR_GREATER
                Platform = SMPlatform.Net7;
#elif NET6_0_OR_GREATER
                Platform = SMPlatform.Net6
#elif NET5_0_OR_GREATER
                Platform = SMPlatform.Net5
#endif
                //
                // Core classes initializations
                //
                InitializeStrings();
                InitializeMath();
                InitializeErrors();
                InitializeDate();
                InitializeIO();
                InitializeSystem();
                InitializePath();
                InitializeUniqueId();
                //InitializeCSV();

                //
                // Log initialization
                //
                DefaultLogFilePath = Combine(ApplicationPath, ExecutableName, "log");

                //
                // Ini settings
                //
                try
                {
                    SMIni ini = new SMIni("", this);
                    // XDatabase.ClientMode = ini.ReadBool("SETUP", "CLIENT_MODE", false);
                    DataPath = ini.ReadString("SETUP", "DATA_PATH", DataPath);
                    // XLocalization.Language = (XLanguage)ini.ReadInteger("SETUP", "LANGUAGE", (int)XLocalization.Language);
                    // XDatabase.DefaultCommandTimeout = ini.ReadInteger("DATABASE_SETTINGS", "DEFAULT_COMMAND_TIMEOUT", 0);
                    // XDatabase.DefaultConnectionTimeout = ini.ReadInteger("DATABASE_SETTINGS", "DEFAULT_CONNECTION_TIMEOUT", 30);
                    // XDatabase.DefaultFetchDelay = ini.ReadInteger("DATABASE_SETTINGS", "DEFAULT_FETCH_DELAY", 330);
                    ErrorVerbose = ini.ReadBool("ERROR", "VERBOSE", IsDebugger());
                }
                catch (Exception ex)
                {
                    Error(ex);
                }

                ////
                //// Resources
                ////
                //Resources = new XResources("Resources.zip");
                
                //
                // Cleaning operation and maintenance
                //
                WipeTemp();

                //
                // Custom initialization
                //
                if (OnInitialize != null) OnInitialize(this, Arguments);

                //
                // End of initialization
                //
                Initializing = false;
                Initialized = true;
            }
        }

        #endregion

        /* */

        #region Events

        /*  ===================================================================
         *  Events
         *  ===================================================================
         */

        /// <summary>On initialize event.</summary>
        public SMOnEvent OnInitialize = null;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return argument by index or empty string if not found.</summary>
        public string Argument(int _ArgumentIndex)
        {
            if (Arguments == null) return "";
            else if ((_ArgumentIndex > -1) && (_ArgumentIndex < Arguments.Length))
            {
                return Arguments[_ArgumentIndex];
            }
            else return "";
        }

        /// <summary>Return true if debugger attached.</summary>
        public bool IsDebugger()
        {
            return Debugger.IsAttached;
        }

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Return current instance of SMApplication or new if not found.</summary>
        public static SMCode CurrentOrNew()
        {
            if (SM == null) return new SMCode();
            else return SM;
        }

        #endregion

        /* */

    }

    /* */

}
