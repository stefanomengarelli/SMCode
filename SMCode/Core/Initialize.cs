/*  ===========================================================================
 *  
 *  File:       Initialize.cs
 *  Version:    1.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode core class: initialization.
 *  
 *  ===========================================================================
 */

using System.Diagnostics;

namespace SMCode
{

    /* */

    /// <summary>SM core class: initialization.</summary>
    public partial class SM
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

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

        /// <summary>OEM id.</summary>
        public string OEM { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance values with custom OEM identifier.</summary>
        public SM(string[] _Arguments = null, string _OEM = "")
        {
            if (!Initialized && !Initializing)
            {
                Initializing = true;
                //
                // Preliminary initializations
                //
                Arguments = _Arguments;
                InternalPassword = @"Mng5Fn$5MC0d3=R4d";
                OEM = _OEM;
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
                // Ini settings
                //
                try
                {
                    // SMIni ini = new SMIni("");
                    // XDatabase.ClientMode = ini.ReadBool("SETUP", "CLIENT_MODE", false);
                    // DataPath = ini.ReadString("SETUP", "DATA_PATH", SM.DataPath);
                    // XLocalization.Language = (XLanguage)ini.ReadInteger("SETUP", "LANGUAGE", (int)XLocalization.Language);
                    // XDatabase.DefaultCommandTimeout = ini.ReadInteger("DATABASE_SETTINGS", "DEFAULT_COMMAND_TIMEOUT", 0);
                    // XDatabase.DefaultConnectionTimeout = ini.ReadInteger("DATABASE_SETTINGS", "DEFAULT_CONNECTION_TIMEOUT", 30);
                    // XDatabase.DefaultFetchDelay = ini.ReadInteger("DATABASE_SETTINGS", "DEFAULT_FETCH_DELAY", 330);
                    // ErrorVerbose = ini.ReadBool("ERROR_SETTINGS", "VERBOSE", IsDebugger());
                }
                catch (Exception ex)
                {
                    // Error(ex);
                }
                ////
                //// Resources
                ////
                //Resources = new XResources("Resources.zip");
                //
                // Cleaning operation and maintenance
                //
                //WipeTemp();
                //

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

    }

    /* */

}
