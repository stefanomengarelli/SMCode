/*  ===========================================================================
 *  
 *  File:       Initialize.cs
 *  Version:    2.4.2
 *  Date:       October 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
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

    /// <summary>SM application class.</summary>
    public partial class SMCode
    {

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
                if (Empty(RootPath)) RootPath = ExecutablePath;
                DefaultPath = RootPath;
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
            StringBuilder rslt = new StringBuilder();
            rslt.Append(ExecutableName.Trim());
            if (rslt.Length < 1) rslt.Append("?Unknown");
            if (!Empty(Version))
            {
                rslt.Append(" [Version ");
                rslt.Append(Version);
                if (ExecutableDate > DateTime.MinValue)
                {
                    rslt.Append(" - ");
                    rslt.Append(ToStr(ExecutableDate, false));
                    rslt.Append(' ');
                    rslt.Append(ToTime(ExecutableDate, false, true));
                    rslt.Append(']');
                }
            }
            return rslt.ToString();
        }

        /// <summary>Return file about string with version and date.</summary>
        public string About(string _FilePath)
        {
            string version;
            DateTime date;
            FileVersionInfo info;
            StringBuilder rslt = new StringBuilder();
            if (FileExists(_FilePath))
            {
                rslt.Append(FileNameWithoutExt(_FilePath.Trim()));
                try
                {
                    info = FileVersionInfo.GetVersionInfo(_FilePath);
                    date = FileDate(_FilePath);
                    version = info.FileVersion;
                    if (!Empty(version))
                    {
                        rslt.Append(" [Version ");
                        rslt.Append(version);
                        if (date > DateTime.MinValue)
                        {
                            rslt.Append(" - ");
                            rslt.Append(ToStr(date, false));
                            rslt.Append(' ');
                            rslt.Append(ToTime(date, false, true));
                            rslt.Append(']');
                        }
                    }
                }
                catch
                {
                    // nop
                }
            }
            return rslt.ToString();
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

        /// <summary>Return text string with start by current language with format ln:text, replacing
        /// if specified values with format %%i%% where i is value index. If language not found will
        /// be returned first instance.</summary>
        public string T(string[] _Texts, string[] _Values = null)
        {
            int i = 0;
            string rslt = null, first = null, s;
            if (_Texts != null)
            {
                while ((rslt == null) && (i < _Texts.Length))
                {
                    s = _Texts[i];
                    if (s != null)
                    {
                        if (s.Length > 3)
                        {
                            if (s[2] == ':')
                            {
                                if (s.Substring(0, 2).Trim().ToLower() == language) rslt = s.Substring(3);
                                else if (first == null) first = s.Substring(3);
                            }
                        }
                    }
                    i++;
                }
            }
            if (rslt == null)
            {
                if (first == null) rslt = "";
                else rslt = first;
            }
            if (_Values != null) rslt = ParseMacro(rslt, _Values);
            return rslt;
        }

        /// <summary>Return text string with start by current language with format ln:text|ln2:text2, 
        /// replacing if specified values with format %%i%% where i is value index. If language not found will
        /// be returned first instance.</summary>
        public string T(string _Text, string[] _Values = null)
        {
            if (_Text == null) return "";
            else return T(_Text.Split('|'), _Values);
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

        /// <summary>Return object by key from static settings or null if not found.</summary>
        public static object GetStaticSettings(string _Key)
        {
            try
            {
                if (StaticSettings.ContainsKey(_Key)) return StaticSettings[_Key];
                else return null;
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>Set static settings with key and object.</summary>
        public static object SetStaticSettings(string _Key, object _Object)
        {
            if (StaticSettings.ContainsKey(_Key)) StaticSettings[_Key] = _Object;
            else StaticSettings.Add(_Key, _Object);
            return _Object;
        }

        #endregion

        /* */

    }

    /* */

}
