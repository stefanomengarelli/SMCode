/*  ===========================================================================
 *  
 *  File:       Initialize.cs
 *  Version:    2.0.56
 *  Date:       October 2024
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
using System.Globalization;
using System.Threading;

namespace SMCodeSystem
{

    /* */

    /// <summary>SM application class: initialization.</summary>
    public partial class SMCode
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Application selected language.</summary>
        private string language = "en";

        #endregion

        /* */

        #region Delegates and events

        /*  ===================================================================
         *  Delegates and events
         *  ===================================================================
         */

        /// <summary>Occurs when log event succeed.</summary>
        public event SMOnLog OnLogEvent = null;

        /// <summary>Occurs when login event succeed.</summary>
        public event SMOnLogin OnLoginEvent = null;

        #endregion

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

        /// <summary>Get or set client mode.</summary>
        public bool ClientMode { get; set; } = false;

        /// <summary>Application current culture.</summary>
        private CultureInfo Culture { get; set; } = null;

        /// <summary>Database connections collection.</summary>
        public SMDatabases Databases { get; private set; } = null;

        /// <summary>Get or set demonstration mode flag.</summary>
        public bool Demo { get; set; } = false;

        /// <summary>SMCode core class initialized flag.</summary>
        public bool Initialized { get; private set; } = false;

        /// <summary>SMCode core class initializing flag.</summary>
        public bool Initializing { get; private set; } = false;

        /// <summary>Generic internal password.</summary>
        public string InternalPassword { get; set; } = "";

        /// <summary>Get or set application selected language.</summary>
        public string Language
        {
            get { return language; }
            set { InitializeLanguage(value); }
        }

        /// <summary>Main database alias (default: MAIN).</summary>
        public string MainAlias { get; set; } = "MAIN";

        /// <summary>Application configuration parameters.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>OEM id.</summary>
        public string OEM { get; set; } = "";

        /// <summary>.NET environment.</summary>
        public SMEnvironment Environment { get; private set; } = SMEnvironment.Unknown;

        /// <summary>Session UID.</summary>
        public string SessionUID { get; private set; } = "";

        /// <summary>Get or set test mode flag.</summary>
        public bool Test { get; set; } = false;

        /// <summary>Current user.</summary>
        public SMUser User { get; private set; } = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance values with custom OEM identifier.</summary>
        public SMCode(string[] _Arguments = null, string _OEM = "", string _InternalPassword = "", string _ApplicationPath = "", bool _WriteLogs = true)
        {
            if (!Initialized && !Initializing)
            {
                Initializing = true;
                SM = this;
                Databases = new SMDatabases(SM);
                User = new SMUser(SM);
                //
                // Preliminary initializations
                //
                Arguments = _Arguments;
                if (Empty(_InternalPassword)) InternalPassword = @"Mng5Fn$5MC0d3=R4d";
                else InternalPassword = _InternalPassword;
                OEM = _OEM;
                SessionUID = GUID();
                Parameters = new SMDictionary(this);
                //
                // Detect .NET platform
                //
#if NET47_OR_GREATER
                Environment = SMEnvironment.NetFramework47;
#elif NET46_OR_GREATER
                Platform = SMEnvironment.NetFramework46;
#elif NET45_OR_GREATER
                Platform = SMEnvironment.NetFramework45;
#elif NET8_0_OR_GREATER
                Environment = SMEnvironment.Net8;
#elif NET7_0_OR_GREATER
                Environment = SMEnvironment.Net7;
#elif NET6_0_OR_GREATER
                Platform = SMEnvironment.Net6
#elif NET5_0_OR_GREATER
                Platform = SMEnvironment.Net5
#endif

                //
                // Get culture language
                // 
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                if (currentCulture != null)
                {
                    if (currentCulture.Name.ToLower().StartsWith("it-")) language = "it";
                }

                //
                // Logger
                //
                this.LastLog = new SMLogItem(this);

                //
                // Core classes initializations
                //
                InitializeStrings();
                InitializeMath();
                InitializeErrors();
                InitializeLanguage();
                InitializeDate();
                InitializeIO();
                InitializeSystem();
                InitializePath();
                if (!SM.Empty(_ApplicationPath)) ApplicationPath = _ApplicationPath;
                InitializeUniqueId();
                InitializeCSV();

                //
                // Ini settings
                //
                try
                {
                    SMIni ini = new SMIni("", this);
                    ini.WriteDefault = true;
                    ClientMode = ini.ReadBool("SETUP", "CLIENT_MODE", ClientMode);
                    DataPath = ini.ReadString("SETUP", "DATA_PATH", DataPath);
                    InitializeLanguage(ini.ReadString("SETUP", "LANGUAGE", language));
                    Databases.DefaultCommandTimeout = ini.ReadInteger("SETUP", "COMMAND_TIMEOUT", 30);
                    Databases.DefaultConnectionTimeout = ini.ReadInteger("SETUP", "CONNECTION_TIMEOUT", 60);
                    ErrorVerbose = ini.ReadBool("SETUP", "VERBOSE", IsDebugger());
                    ini.Save();
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
                if (_WriteLogs)
                {
                    Log(SMLogType.Separator);
                    Log(SMLogType.Information, "SMCode initialized.");
                }
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

        /// <summary>Return executable about string with version and date.</summary>
        public string About()
        {
            string rslt = ExecutableName.Trim();
            if (SM.Empty(rslt)) rslt = "?Unknown";
            if (!SM.Empty(Version)) rslt += " - V " + Version;
            if (ExecutableDate > DateTime.MinValue) rslt += " - " + SM.ToStr(ExecutableDate, true);
            return rslt;
        }

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

        /// <summary>Initialize selected language environment.</summary>
        private void InitializeLanguage(string _Language = null)
        {
            if (_Language != null)
            {
                _Language = _Language.Trim().ToLower();
                if (language != _Language)
                {
                    language = _Language;
                    _Language = null;
                }
            }
            if (_Language == null)
            {
                if (language == "it")
                {
                    DateFormat = SMDateFormat.ddmmyyyy;
                    DateSeparator = '/';
                    DecimalSeparator = ',';
                    ThousandSeparator = '.';
                    TimeSeparator = ':';
                    DaysNames = new string[] { "Luned�", "Marted�", "Mercoled�", "Gioved�", "Venerd�", "Sabato", "Domenica" };
                    DaysShortNames = new string[] { "Lun", "Mar", "Mer", "Gio", "Ven", "Sab", "Dom" };
                    MonthsNames = new string[] { "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre" };
                    MonthsShortNames = new string[] { "Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov", "Dic" };
                }
                else if (language == "fr")
                {
                    DateFormat = SMDateFormat.ddmmyyyy;
                    DateSeparator = '/';
                    DecimalSeparator = ',';
                    ThousandSeparator = '.';
                    TimeSeparator = ':';
                    DaysNames = new string[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
                    DaysShortNames = new string[] { "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim" };
                    MonthsNames = new string[] { "Janvier", "F�vrier", "Mars", "Avril", "Mai", "Juin", "Juillet", "Ao�t", "Septembre", "Octobre", "Novembre", "D�cembre" };
                    MonthsShortNames = new string[] { "Jan", "Fev", "Mar", "Avr", "Mai", "Juin", "Juil", "Aout", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (language == "de")
                {
                    DateFormat = SMDateFormat.ddmmyyyy;
                    DateSeparator = '/';
                    DecimalSeparator = ',';
                    ThousandSeparator = '.';
                    TimeSeparator = ':';
                    DaysNames = new string[] { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };
                    DaysShortNames = new string[] { "Mon", "Die", "Mit", "Don", "Fre", "Sam", "Son" };
                    MonthsNames = new string[] { "Januar", "Februar", "M�rz", "April", "Kann", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
                    MonthsShortNames = new string[] { "Jan", "Feb", "Mar", "Apr", "Kan", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez" };
                }
                else
                {
                    language = "en";
                    DateFormat = SMDateFormat.mmddyyyy;
                    DateSeparator = '-';
                    DecimalSeparator = '.';
                    ThousandSeparator = ',';
                    TimeSeparator = ':';
                    DaysNames = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                    DaysShortNames = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                    MonthsNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                    MonthsShortNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                Culture = new CultureInfo(language);
                Thread.CurrentThread.CurrentUICulture = Culture;
            }
        }

        /// <summary>Return true if debugger attached.</summary>
        public bool IsDebugger()
        {
            return Debugger.IsAttached;
        }

        /// <summary>Perform user login with user-id and password. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Login(string _UserId, string _Password)
        {
            return User.LoadByCredentials(_UserId, _Password);
        }

        /// <summary>Perform user login with id. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoginById(int _Id, string _Details = "")
        {
            return User.LoadById(_Id, _Details);
        }

        /// <summary>Perform user login by tax code. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoginByTaxCode(string _TaxCode, string _Details = "")
        {
            return User.Load("SELECT * FROM " + SMDefaults.UsersTableName + " WHERE (TaxCode=" + SM.Quote(_TaxCode) + ")AND" + SM.SqlNotDeleted(), _Details);
        }

        /// <summary>Perform user login with uid. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoginByUid(string _Uid, string _Details = "")
        {
            return User.LoadByUid(_Uid, _Details);
        }

        /// <summary>Perform login event if defined.</summary>
        public bool LoginEvent(SMLogItem _LogItem, SMUser _User)
        {
            bool rslt = true;
            if (OnLoginEvent != null) OnLoginEvent(_LogItem, _User, ref rslt);
            if (rslt) SM.Log(_LogItem);
            else SM.Log(SMLogType.Error, "Unauthorized user.", "", "");
            return rslt;
        }

        /// <summary>Return text string with start by current language with format ln:text, replacing
        /// if specified values with format %%i%% where i is value index. If language not found will
        /// be returned first instance.</summary>
        public string T(string[] _Texts, string[] _Values = null)
        {
            int i = 0;
            string r = null, first = null, s;
            if (_Texts != null)
            {
                while ((r == null) && (i < _Texts.Length))
                {
                    s = _Texts[i];
                    if (s != null)
                    {
                        if (s.Length > 3)
                        {
                            if (s[2] == ':')
                            {
                                if (s.Substring(0, 2).Trim().ToLower() == language) r = s.Substring(3);
                                else if (first == null) first = s.Substring(3);
                            }
                        }
                    }
                    i++;
                }
            }
            if (r == null)
            {
                if (first == null) r = "";
                else r = first;
            }
            if (_Values != null) r = MacrosIndexes(r, _Values);
            return r;
        }

        /// <summary>Return application title with argument and test/demo indicator.
        /// It is possibile specify argument separator and test/demo prefix and suffix.</summary>
        public string Title(string _Title = null, string _Argument = null, string _Separator=" - ", string _Prefix = " (", string _Suffix = ")")
        {
            string s = "";
            if (_Title == null) _Title = ExecutableName;
            else _Title = _Title.Trim();
            if (!SM.Empty(_Argument)) _Title = SM.Cat(_Title, _Argument.Trim(), _Separator);
            if (Test) s = SM.Cat(s, "TEST", ", ");
            if (Demo) s = SM.Cat(s, "DEMO", ", ");
            if (s.Trim().Length > 0) _Title += _Prefix + s.Trim() + _Suffix;
            return _Title.Trim();
        }

        /// <summary>Return test/demo indicator. It is possibile specify prefix and suffix.</summary>
        public string TestDemo(string _Prefix = " (", string _Suffix = ")")
        {
            string s = "";
            if (Test) s = SM.Cat(s, "TEST", ", ");
            if (Demo) s = SM.Cat(s, "DEMO", ", ");
            if (s.Trim().Length > 0) return _Prefix + s.Trim() + _Suffix;
            else return "";
        }

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Return current instance of SMApplication or new if not found.</summary>
        public static SMCode CurrentOrNew(SMCode _SM = null, string[] _Arguments = null, string _OEM = "", string _InternalPassword = "", string _ApplicationPath = "")
        {
            if (_SM != null) SM = _SM;
            else if (SM == null) SM = new SMCode(_Arguments, _OEM, _InternalPassword, _ApplicationPath);
            return SM;
        }

        #endregion

        /* */

    }

    /* */

}