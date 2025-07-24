/*  ===========================================================================
 *  
 *  File:       Initialize.cs
 *  Version:    2.0.262
 *  Date:       May 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: initialization.
 *  
 *  ===========================================================================
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
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

        /// <summary>Deploy environment.</summary>
        private string deploy = "";

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
        public virtual bool ClientMode { get; set; } = false;

        /// <summary>Application current culture.</summary>
        private CultureInfo Culture { get; set; } = null;

        /// <summary>Database connections collection.</summary>
        public SMDatabases Databases { get; private set; } = null;

        /// <summary>Get or set demonstration mode flag.</summary>
        public virtual bool Demo { get; set; } = false;

        /// <summary>Get or set deploy environment.</summary>
        public string Deploy
        {
            get { return deploy; }
            set
            {
                deploy = ToStr(value).Trim().ToLower();
                if (deploy.StartsWith("dev")) deploy = "Development";
                else if (deploy.StartsWith("prod")) deploy = "Production";
                else if (Debugger.IsAttached) deploy = "Development";
                else deploy = "Production";
            }
        }

        /// <summary>Get or set main INI configuration write defaults flag.</summary>
        public virtual bool IniDefaults { get; set; } = true;

        /// <summary>Get or set main INI settings configuration flag.</summary>
        public virtual bool IniSettings { get; set; } = true;

        /// <summary>SMCode core class initialized flag.</summary>
        public bool Initialized { get; private set; } = false;

        /// <summary>SMCode core class initializing flag.</summary>
        public bool Initializing { get; private set; } = false;

        /// <summary>Get injections collection.</summary>
        public SMInjections Injections { get; private set; } = null;

        /// <summary>Generic internal password.</summary>
        public virtual string InternalPassword { get; set; } = "";

        /// <summary>Get or set application selected language.</summary>
        public string Language
        {
            get { return language; }
            set { InitializeLanguage(value); }
        }

        /// <summary>Main database alias (default: MAIN).</summary>
        public virtual string MainAlias { get; set; } = "MAIN";

        /// <summary>Application configuration parameters.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>Application resources manager.</summary>
        public SMResources Resources { get; private set; } = null;

        /// <summary>OEM id.</summary>
        public virtual string OEM { get; set; } = "";

        /// <summary>.NET environment.</summary>
        public SMEnvironment Environment { get; private set; } = SMEnvironment.Unknown;

        /// <summary>Session UID.</summary>
        public virtual string SessionUID { get; set; } = "";

        /// <summary>Get or set test mode flag.</summary>
        public virtual bool Test { get; set; } = false;

        /// <summary>Current user.</summary>
        public SMUser User { get; private set; } = null;

        /// <summary>Get or set user extend table.</summary>
        public virtual string UserExtendTable { get; set; } = null;

        /// <summary>Get or set user extend table used id field name (default: IdUser).</summary>
        public virtual string UserExtendTableIdUserFieldName{ get; set; } = "IdUser";

        /// <summary>Get or set initialization wipe temporary file flag.</summary>
        public virtual bool WipeTemporaryFiles { get; set; } = true;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance values with custom OEM identifier.</summary>
        public SMCode(string[] _Arguments = null, bool? _DefaultOutputFiles = null, string _OEM = "", string _InternalPassword = "", string _ApplicationPath = "")
        {
            if (!Initialized && !Initializing)
            {
                Initializing = true;

                if (_DefaultOutputFiles.HasValue)
                {
                    AutoCreatePath = _DefaultOutputFiles.Value;
                    IniDefaults = _DefaultOutputFiles.Value;
                    IniSettings = _DefaultOutputFiles.Value;
                    LogToConsole = _DefaultOutputFiles.Value;
                    LogToDatabase = _DefaultOutputFiles.Value;
                    LogToFile = _DefaultOutputFiles.Value;
                    WipeTemporaryFiles = _DefaultOutputFiles.Value;
                }

                Databases = new SMDatabases(this);
                User = new SMUser(this);
                //
                // Preliminary initializations
                //
                Arguments = _Arguments;
                if (Empty(_InternalPassword))
                {
                    if (Empty(InternalPassword))
                    {
                        InternalPassword = @"Mng5Fn$5MC0d3=R4d";
                    }
                }
                else InternalPassword = _InternalPassword;
                OEM = _OEM;
                SessionUID = GUID();
                Parameters = new SMDictionary(this);
                Injections = new SMInjections(this);
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
                // Deploy environment
                //
                Deploy = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                
                //
                // Logger
                //
                this.LastLog = new SMLogItem(this);

                //
                // Core classes initializations
                //
                InitializeLanguage();
                InitializeDate();
                InitializePath();
                if (!Empty(_ApplicationPath)) ApplicationPath = _ApplicationPath;
                if (Empty(RootPath)) RootPath = _ApplicationPath;
                InitializeCustom();

                //
                // Ini settings
                //
                if (IniSettings)
                {
                    try
                    {
                        SMIni ini = new SMIni("", this);
                        ini.WriteDefault = IniDefaults;
                        ClientMode = ini.ReadBool("SETUP", "CLIENT_MODE", ClientMode);
                        DataPath = ini.ReadString("SETUP", "DATA_PATH", DataPath);
                        InitializeLanguage(ini.ReadString("SETUP", "LANGUAGE", language));
                        Databases.DefaultCommandTimeout = ini.ReadInteger("SETUP", "COMMAND_TIMEOUT", 30);
                        Databases.DefaultConnectionTimeout = ini.ReadInteger("SETUP", "CONNECTION_TIMEOUT", 60);
                        ErrorLog = ini.ReadBool("SETUP", "ERRORLOG", IsDebugger());
                        CSVDelimiter = (ini.ReadString("CSV", "DELIMITER", CSVDelimiter + "").Trim() + ';')[0];
                        CSVSeparator = (ini.ReadString("CSV", "SEPARATOR", CSVSeparator + "").Trim() + '"')[0];
                        if (IniDefaults) ini.Save();
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                    }
                }

                //
                // Resources
                //
                Resources = new SMResources(this);

                //
                // Cleaning operation and maintenance
                //
                if (WipeTemporaryFiles) WipeTemp();

                //
                // End of initialization
                //
                Initializing = false;
                Initialized = true;

                //
                // Log initialization
                //
                Log(SMLogType.Separator);
                Log(SMLogType.Information, "SMCode initialized.");
                //
                // Set last static instance
                //
                SM = this;
            }
        }

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
            if (Empty(rslt)) rslt = "?Unknown";
            if (!Empty(Version)) rslt += " - V " + Version;
            if (ExecutableDate > DateTime.MinValue) rslt += " - " + ToStr(ExecutableDate, true);
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

        /// <summary>Returns string with common environment properties and values.</summary>
        public string EnvironmentDump()
        {
            StringBuilder r = new StringBuilder();
            r.AppendLine($"AutoCreatePath: {AutoCreatePath}");
            r.AppendLine($"ApplicationPath: {ApplicationPath}");
            r.AppendLine($"ClientMode: {ClientMode}");
            r.AppendLine($"CommonPath: {CommonPath}");
            r.AppendLine($"CSVDelimiter: {CSVDelimiter}");
            r.AppendLine($"CSVSeparator: {CSVSeparator}");
            r.AppendLine($"DatabaseLog: {DatabaseLog}");
            r.AppendLine($"DataPath: {DataPath}");
            r.AppendLine($"DateFormat: {DateFormat}");
            r.AppendLine($"DateSeparator: {DateSeparator}");
            r.AppendLine($"DecimalSeparator: {DecimalSeparator}");
            r.AppendLine($"Databases.DefaultCommandTimeout: {Databases.DefaultCommandTimeout}");
            r.AppendLine($"Databases.DefaultConnectionTimeout: {Databases.DefaultConnectionTimeout}");
            r.AppendLine($"DefaultLogFilePath: {DefaultLogFilePath}");
            r.AppendLine($"DesktopPath: {DesktopPath}");
            r.AppendLine($"DocumentsPath: {DocumentsPath}");
            r.AppendLine($"ErrorHistoryEnabled: {ErrorHistoryEnabled}");
            r.AppendLine($"ErrorLog: {ErrorLog}");
            r.AppendLine($"ExecutableDate: {ExecutableDate}");
            r.AppendLine($"ExecutableName: {ExecutableName}");
            r.AppendLine($"ExecutablePath: {ExecutablePath}");
            r.AppendLine($"IniDefaults: {IniDefaults}");
            r.AppendLine($"IniSettings: {IniSettings}");
            r.AppendLine($"Language: {language}");
            r.AppendLine($"LogAlias: {LogAlias}");
            r.AppendLine($"LogToConsole: {LogToConsole}");
            r.AppendLine($"LogToDatabase: {LogToDatabase}");
            r.AppendLine($"LogToFile: {LogToFile}");
            r.AppendLine($"TempPath: {TempPath}");
            r.AppendLine($"RootPath: {RootPath}");
            r.AppendLine($"ThousandSeparator: {ThousandSeparator}");
            r.AppendLine($"ThrowException: {ThrowException}");
            r.AppendLine($"TimeSeparator: {TimeSeparator}");
            r.AppendLine($"UserDocumentsPath: {UserDocumentsPath}");
            r.AppendLine($"Version: {Version}");
            r.AppendLine($"WipeTemporaryFiles: {WipeTemporaryFiles}");
            return r.ToString();
        }

        /// <summary>Virtual method to initialize custom values.</summary>
        public virtual void InitializeCustom()
        {
            // nop
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
                    DaysNames = new string[] { "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato", "Domenica" };
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
                    MonthsNames = new string[] { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" };
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
                    MonthsNames = new string[] { "Januar", "Februar", "März", "April", "Kann", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
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
            return User.Load($"SELECT * FROM {SMDefaults.UsersTableName} WHERE (TaxCode={Quote(_TaxCode)})AND{SqlNotDeleted()}", _Details);
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
            if (rslt) Log(_LogItem);
            else Log(SMLogType.Error, "Unauthorized user.", "", "");
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
            if (_Values != null) r = Macros(r, _Values);
            return r;
        }

        /// <summary>Return application title with argument and test/demo indicator.
        /// It is possibile specify argument separator and test/demo prefix and suffix.</summary>
        public string Title(string _Title = null, string _Argument = null, string _Separator=" - ", string _Prefix = " (", string _Suffix = ")")
        {
            string s = "";
            if (_Title == null) _Title = ExecutableName;
            else _Title = _Title.Trim();
            if (!Empty(_Argument)) _Title = Cat(_Title, _Argument.Trim(), _Separator);
            if (Test) s = Cat(s, "TEST", ", ");
            if (Demo) s = Cat(s, "DEMO", ", ");
            if (s.Trim().Length > 0) _Title += _Prefix + s.Trim() + _Suffix;
            return _Title.Trim();
        }

        /// <summary>Return test/demo indicator. It is possibile specify prefix and suffix.</summary>
        public string TestDemo(string _Prefix = " (", string _Suffix = ")")
        {
            string s = "";
            if (Test) s = Cat(s, "TEST", ", ");
            if (Demo) s = Cat(s, "DEMO", ", ");
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
        public static SMCode CurrentOrNew(SMCode _SM = null, string[] _Arguments = null, bool? _DefaultOutputFiles = null, string _OEM = "", string _InternalPassword = "", string _ApplicationPath = "")
        {
            if (_SM != null) SM = _SM;
            else if (SM == null) SM = new SMCode(_Arguments, _DefaultOutputFiles, _OEM, _InternalPassword, _ApplicationPath);
            return SM;
        }

        #endregion

        /* */

    }

    /* */

}
