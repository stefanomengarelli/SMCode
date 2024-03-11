/*  ===========================================================================
 *  
 *  File:       SMDatabase.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode database component.
 *
 *  ===========================================================================
 */

using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace SMCode
{

    /* */

    /// <summary>SMCode database component.</summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SMDatabase : Component
    {

        /* */

        #region Declarations

        /*  --------------------------------------------------------------------
         *  Declarations
         *  --------------------------------------------------------------------
         */

        /// <summary>SM session instance.</summary>
        private readonly SM SM = null;

        /// <summary>Database OLE DB connection.</summary>
        private OleDbConnection dbOle;
        /// <summary>Database SQL connection.</summary>
        private SqlConnection dbSql;
        /// <summary>Database MySQL connection.</summary>
        private MySqlConnection dbMySql;

        /// <summary>Database alias.</summary>
        private string dbAlias;
        /// <summary>Database host.</summary>
        private string dbHost;
        /// <summary>Database name.</summary>
        private string dbName;
        /// <summary>Database template file id without extension.</summary>
        private string dbTemplate;
        /// <summary>Database password.</summary>
        private string dbPassword;
        /// <summary>Database path.</summary>
        private string dbPath;
        /// <summary>Database type.</summary>
        private SMDatabaseType dbType;
        /// <summary>Database username.</summary>
        private string dbUser;
        /// <summary>Database version.</summary>
        private string dbVersion;

        /// <summary>Database connection timeout in seconds.</summary>
        private int dbConnectionTimeout;
        /// <summary>Database command timeout in seconds.</summary>
        private int dbCommandTimeout;

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Database instance constructor.</summary>
        public SMDatabase(SM _SM)
        {
            SM = _SM;
            InitializeComponent();
            Clear();
        }

        /// <summary>Database instance constructor with container.</summary>
        public SMDatabase(IContainer _Container)
        {
            SM = SM._SM;
            _Container.Add(this);
            InitializeComponent();
            Clear();
        }

        #endregion

        /* */

        #region Properties

        /*  --------------------------------------------------------------------
         *  Properties
         *  --------------------------------------------------------------------
         */

        /// <summary>Specifies whether or not database is open.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies whether or not database is open.")]
        public bool Active
        {
            get
            {
                if ((dbType == SMDatabaseType.SqlExpress) || (dbType == SMDatabaseType.Sql))
                {
                    if (dbSql != null) return dbSql.State == ConnectionState.Open;
                    else return false;
                }
                else if (dbType == SMDatabaseType.MySql)
                {
                    if (dbMySql != null) return dbMySql.State == ConnectionState.Open;
                    else return false;
                }
                else
                {
                    if (dbOle != null) return dbOle.State == ConnectionState.Open;
                    else return false;
                }
            }
            set 
            { 
                if (value) Open(); 
                else Close(); 
            }
        }

        /// <summary>Specifies database alias name to use.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database alias name to use.")]
        public string Alias
        {
            get { return dbAlias; }
            set { dbAlias = value.Trim().ToUpper(); }
        }

        /// <summary>Specifies database command timeout.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database command timeout.")]
        public int CommandTimeout
        {
            get { return dbCommandTimeout; }
            set { dbCommandTimeout = value; }
        }

        /// <summary>Specifies database connection timeout in seconds.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database connection timeout in seconds.")]
        public int ConnectionTimeout
        {
            get { return dbConnectionTimeout; }
            set { dbConnectionTimeout = value; }
        }

        /// <summary>Specifies default database name.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies default database name.")]
        public string Database
        {
            get { return dbName; }
            set { dbName = value; }
        }

        /// <summary>Specifies database templates file id without extension.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database templates file id without extension.")]
        public string Template
        {
            get { return dbTemplate; }
            set { dbTemplate = value; }
        }

        /// <summary>Specifies database host name or address.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database host name or address.")]
        public string Host
        {
            get { return dbHost; }
            set { dbHost = value; }
        }

        /// <summary>Indicated used connection form MySql database type.</summary>
        [BrowsableAttribute(false)]
        public MySqlConnection MySqlDB
        {
            get { return dbMySql; }
            set { dbMySql = value; }
        }

        /// <summary>Indicated used connection form OleDB database type.</summary>
        [BrowsableAttribute(false)]
        public OleDbConnection OleDB
        {
            get { return dbOle; }
            set { dbOle = value; }
        }

        /// <summary>Specifies database password.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database password.")]
        public string Password
        {
            get { return dbPassword; }
            set { dbPassword = value; }
        }

        /// <summary>Specifies database path.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database path.")]
        public string Path
        {
            get { return dbPath; }
            set { dbPath = value; }
        }

        /// <summary>Indicated used connection form Sql database type.</summary>
        [BrowsableAttribute(false)]
        public SqlConnection SqlDB
        {
            get { return dbSql; }
            set { dbSql = value; }
        }

        /// <summary>Specifies database type.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database type.")]
        public SMDatabaseType Type
        {
            get { return dbType; }
            set { dbType = value; }
        }

        /// <summary>Specifies database user name.</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database user name.")]
        public string User
        {
            get { return dbUser; }
            set { dbUser = value; }
        }

        /// <summary>Specifies database version (after LoadSchema() call).</summary>
        [BrowsableAttribute(true)]
        [CategoryAttribute("SMNetCode")]
        [DescriptionAttribute("Specifies database version (after LoadSchema() call).")]
        public string Version
        {
            get { return dbVersion; }
        }

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Reset database variables.</summary>
        public void Clear()
        {
            dbOle = null;
            dbSql = null;
            dbMySql = null;
            //
            dbAlias = "";
            dbHost = "";
            dbName = "";
            dbTemplate = "";
            dbPassword = "";
            dbPath = "";
            dbType = SMDatabaseType.Access;
            dbUser = "";
            dbVersion = "";
            // 
            dbConnectionTimeout = SM.ToInt(SM.ReadIni("","DATABASE","CONNECTION_TIMEOUT","120"));
            dbCommandTimeout = SM.ToInt(SM.ReadIni("", "DATABASE", "COMMAND_TIMEOUT", "120"));
        }

        /// <summary>Close database connection. Returns true if succeed.</summary>
        public bool Close()
        {
            dbVersion = "";
            //
            // close dbOle
            //
            try
            {
                if (dbOle != null)
                {
                    if (dbOle.State != ConnectionState.Closed) dbOle.Close();
                    dbOle.Dispose();
                    dbOle = null;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            //
            // close dbSql
            //
            try
            {
                if (dbSql != null)
                {
                    if (dbSql.State != ConnectionState.Closed) dbSql.Close();
                    dbSql.Dispose();
                    dbSql = null;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            //
            // close dbMySql
            //
            try
            {
                if (dbMySql != null)
                {
                    if (dbMySql.State != ConnectionState.Closed) dbMySql.Close();
                    dbMySql.Dispose();
                    dbMySql = null;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            //
            // return
            //
            return !this.Active;
        }

        /// <summary>Return connection string for current settings.</summary>
        public string ConnectionString()
        {
            string r = "", s, v, q = "";
            int i = (int)dbType;
            if ((i > -1) && (i < SM.DatabaseConnectionStrings.Length))
            {
                s = SM.DatabaseConnectionStrings[i];
                s = s.Replace("%%DBHOST%%", dbHost).Replace("%%DBNAME%%", dbName).Replace("%%DBPATH%%", dbPath).Replace("%%DBTEMPLATE%%", dbTemplate);
                s = s.Replace("%%DBUSER%%", dbUser).Replace("%%PASSWORD%%", dbPassword).Replace("%%FILENAME%%", MdbPath());
                s = s.Replace("%%TIMEOUT%%", dbConnectionTimeout.ToString());
                while (s.Trim().Length > 0)
                {
                    v = SM.Extract(ref s, ";").Trim();
                    if (!v.EndsWith("=")) r += q + v;
                    if (q == "") q = "; ";
                }
            }
            return r;
        }

        /// <summary>Copy template database to file. Return true if succeed.</summary>
        public bool CopyTemplate(string _FileName)
        {
            return SM.FileCopy(this.TemplatePath(), _FileName);
        }

        /// <summary>Create a temporary local database from template. 
        /// Returns true if succeed.</summary>
        public bool CreateTemporary()
        {
            string s = TemplatePath(), t = TemporaryPath();
            if (s.Length > 0)
            {
                SM.FileDelete(t);
                return SM.FileCopy(s, t);
            }
            else return false;
        }

        /// <summary>Return standard dbf path for current settings.</summary>
        public string DbfPath()
        {
            return SM.Combine(dbPath, dbName, "dbf");
        }

        /// <summary>Test if database connection is active otherwise try to open it. Returns true if succeed.</summary>
        public bool Keep()
        {
            if (this.Active) return true;
            else return Open();
        }

        /// <summary>Load database parameters related to alias from application INI file. Returns true if succeed.</summary>
        public bool Load(string _Alias)
        {
            return Load("", _Alias, true);
        }

        /// <summary>Load database parameters related to alias from fileName INI file. Returns true if succeed.</summary>
        public bool Load(string _FileName, string _Alias, bool _AssignAliasName)
        {
            bool r = false;
            string s;
            SMIniFile ini;
            SM.ErrorTracePush("SMDatabase.cs", "SMDatabase.Load(\"" + _FileName + "\",\"" + _Alias + "\")");
            if (Close())
            {
                _Alias = _Alias.Trim().ToUpper();
                if ((_Alias.Length > 0) && (SM.Language != SMLanguage.Auto))
                {
                    ini = new SMIniFile(_FileName);
                    s = "DATABASE " + _Alias;
                    ini.WriteDefault = true;
                    dbAlias = "";
                    dbType = TypeFromString(ini.ReadString(s, "TYPE", TypeToString(SMDatabaseType.Access)));
                    dbHost = ini.ReadString(s, "HOST", "localhost");
                    dbName = ini.ReadString(s, "NAME", SM.ExecName);
                    dbTemplate = ini.ReadString(s, "TEMPLATE", SM.ExecName);
                    dbPath = SM.Macro(ini.ReadString(s, "PATH", SM.DataDir));
                    dbUser = ini.ReadString(s, "USER", "");
                    dbPassword = ini.ReadMasked(s, "PASS", SM.DatabaseAccessPassword);
                    if ((dbType == SMDatabaseType.Access) && (dbPassword.Trim().Length < 1)) dbPassword = SM.DatabaseAccessPassword;
                    if (_AssignAliasName) dbAlias = _Alias;
                    ini.Save();
                    r = true;
                }
                else r = false;
            }
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Return standard mdb path for current settings.</summary>
        public string MdbPath()
        {
            return SM.Combine(dbPath, dbName, "mdb");
        }

        /// <summary>Close and reopen database connection with actual parameters. Returns true if succeed.</summary>
        public bool Open()
        {
            bool r = false;
            Form w = null;
            string f, s, t;
            SM.ErrorTracePush("SMDatabase.cs", "SMDatabase.Open()");
            if (SM.Language != SMLanguage.Auto)
            {
                Close();
                if (dbAlias.Trim().Length > 0) r = Load(dbAlias);
                else r = true;
                if (r)
                {
                    r = false;
                    s = ConnectionString();
                    if ((s == SM.Databases.LastConnectionString) && (DateTime.Now < SM.Databases.LastConnectionFail.AddSeconds(5)))
                    {
                        SM.Raise(SM.T("@SM|E' stata ritentata una connessione ad un database che era precedentemente fallita prima del tempo minimo di attesa."), false);
                    }
                    else
                    {
                        SM.Databases.LastConnectionString = s;
                        SM.Databases.LastConnectionFail = DateTime.MinValue;
                        try
                        {
                            if (dbType == SMDatabaseType.SqlExpress)
                            {
                                dbSql = new SqlConnection(s);
                                w = SM.ProgressDialog(SM.T("@SM|Apertura database"),
                                    SM.T("@SM|Connessione con il database in corso..."),
                                    SM.T("@SM|Attendere la connessione al database. L'applicazione proverà a connettersi per un periodo massimo di %%0%% secondi.").Replace("%%0%%", dbSql.ConnectionTimeout.ToString()),
                                    SMProgressBarMode.None,
                                    false,
                                    SM.ThemeImage("icons/ui_icon_48_dbopen"));
                                dbSql.Open();
                                SM.ProgressDialog(ref w);
                            }
                            else if (dbType == SMDatabaseType.Sql)
                            {
                                dbSql = new SqlConnection(s);
                                w = SM.ProgressDialog(SM.T("@SM|Apertura database"),
                                    SM.T("@SM|Connessione con il database in corso..."),
                                    SM.T("@SM|Attendere la connessione al database. L'applicazione proverà a connettersi per un periodo massimo di %%0%% secondi.").Replace("%%0%%", dbSql.ConnectionTimeout.ToString()),
                                    SMProgressBarMode.None,
                                    false,
                                    SM.ThemeImage("icons/ui_icon_48_dbopen"));
                                dbSql.Open();
                                SM.ProgressDialog(ref w);
                            }
                            else if (dbType == SMDatabaseType.MySql)
                            {
                                dbMySql = new MySqlConnection(s);
                                w = SM.ProgressDialog(SM.T("@SM|Apertura database"),
                                    SM.T("@SM|Connessione con il database in corso..."),
                                    SM.T("@SM|Attendere la connessione al database. L'applicazione proverà a connettersi per un periodo massimo di %%0%% secondi.").Replace("%%0%%", dbMySql.ConnectionTimeout.ToString()),
                                    SMProgressBarMode.None,
                                    false,
                                    SM.ThemeImage("icons/ui_icon_48_dbopen"));
                                dbMySql.Open();
                                SM.ProgressDialog(ref w);
                            }
                            else if (dbType == SMDatabaseType.DBase4)
                            {
                                f = DbfPath();
                                if (SM.FileExists(f))
                                {
                                    dbOle = new OleDbConnection(s);
                                    dbOle.Open();
                                }
                            }
                            else
                            {
                                f = MdbPath();
                                if (!SM.DatabaseClientMode && SM.DirectoryExists(SM.ExtractFilePath(f)))
                                {
                                    if (!SM.FileExists(f))
                                    {
                                        t = TemplatePath();
                                        if (t.Length > 0)
                                        {
                                            if (t.ToLower().Trim() != f.ToLower().Trim())
                                            {
                                                if (SM.DirectoryExists(SM.ExtractFilePath(f)))
                                                {
                                                    w = SM.ProgressDialog(SM.T("@SM|Configurazione applicazione"),
                                                        SM.T("@SM|Installazione database in corso...|icons/ui_icon_48_setup"),
                                                        SMProgressBarMode.Continuos, false);
                                                    SM.FileCopy(t, f, SM.IORetryTimes);
                                                    SM.WaitSecsDoEvents(0.5);
                                                    SM.ProgressDialog(ref w);
                                                }
                                                else SM.ErrorDialog(SM.T("@SM|Impossibile accedere al percorso dei dati."));
                                            }
                                        }
                                    }
                                }
                                if (SM.FileExists(f))
                                {
                                    dbOle = new OleDbConnection(s);
                                    dbOle.Open();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SM.Databases.LastConnectionFail = DateTime.Now;
                            SM.ProgressDialog(w);
                            SM.Error(ex);
                        }
                        r = this.Active;
                        if (!r) this.Close();
                    }
                }
                else SM.Raise(SM.T("@SM|Errore durante la lettura dei parametri dell'alias."), false);
            }
            else SM.Raise(SM.T("@SM|Non è stato inizializzato il linguaggio di default della libreria."), false);
            //
            SM.ErrorTracePop();
            //
            return r;
        }

        /// <summary>Open database connection with parameters stored for aliasName.
        /// Returns true if succeed.</summary>
        public bool Open(string _Alias)
        {
            if (Load(_Alias)) return Open();
            else return false;
        }

        /// <summary>Open database connection with database type, host name, database name, user name and password parameters.
        /// Returns true if succeed. </summary>
        public bool Open(SMDatabaseType _DatabaseType, 
            string _DatabaseHost, string _DatabaseName, string _DatabaseTemplate, 
            string _DatabasePath, string _UserName, string _Password)
        {
            bool r;
            dbType = _DatabaseType;
            dbHost = _DatabaseHost;
            dbName = _DatabaseName;
            dbTemplate = _DatabaseTemplate;
            dbPath = _DatabasePath;
            dbUser = _UserName;
            dbPassword = _Password;
            r = Open();
            return r;
        }

        /// <summary>Open DBase IV DBF database connection with file fileName.
        /// Returns true if succeed.</summary>
        public bool OpenDbf(string _FileName)
        {
            if (_FileName.Trim().Length > 0)
            {
                return Open(
                    SMDatabaseType.DBase4, 
                    "localhost",
                    SM.FileNameWithoutExt(_FileName), "",
                    SM.FilePath(_FileName), 
                    "", 
                    ""
                    );
            }
            else return false;
        }

        /// <summary>Open Microsoft Access® MDB database connection with file fileName.
        /// Returns true if succeed.</summary>
        public bool OpenMdb(string _FileName)
        {
            bool r = false;
            SM.ErrorTracePush("SMDatabase.cs", "SMDatabase.OpenMdb(\"" + _FileName + "\")");
            if (_FileName.Trim().Length > 0)
            {
                r = Open(SMDatabaseType.Access, "localhost", SM.ExtractFileNoExt(_FileName), "",
                    SM.ExtractFilePath(_FileName), "", SM.DatabaseAccessPassword);
            }
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Save database connection parameters to application INI file with alias.
        /// Returns true if succeed.</summary>
        public bool Save(string _Alias)
        {
            bool r;
            SM.ErrorTracePush("SMDatabase.cs", "SMDatabase.Save(\"" + _Alias + "\")");
            r = Save("", _Alias);
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Save database connection parameters to application INI file for alias.
        /// Returns true if succeed.</summary>
        public bool Save(string _FileName, string _Alias)
        {
            bool b = false;
            string s;
            SMIniFile ini;
            SM.ErrorTracePush("SMDatabase.cs", "SMDatabase.Save(\"" + _FileName + "\", \"" + _Alias + "\")");
            SM.Error("");
            _Alias = _Alias.Trim().ToUpper();
            if ((SM.Language != SMLanguage.Auto) && (_Alias.Length > 0))
            {
                ini = new SMIniFile(_FileName);
                s = "DATABASE " + _Alias;
                ini.WriteString(s, "TYPE", TypeToString(dbType));
                if (dbType == SMDatabaseType.Access)
                {
                    if (dbHost.Trim().Length < 1) dbHost = "localhost";
                    if (dbName.Trim().Length < 1) dbName = SM.ExecName;
                    if (dbPath.Trim().Length < 1) dbPath = SM.DataDir;
                }
                ini.WriteString(s, "HOST", dbHost);
                ini.WriteString(s, "NAME", dbName);
                ini.WriteString(s, "TEMPLATE", dbTemplate);
                ini.WriteString(s, "PATH", dbPath);
                ini.WriteString(s, "USER", dbUser);
                ini.WriteMasked(s, "PASS", dbPassword);
                dbAlias = _Alias;
                b = ini.Save();
                if (!b) SM.ErrorMessage = "Error writing configuration to " + SM.ApplicationIniFile();
            }
            SM.ErrorTracePop();
            return b;
        }

        /// <summary>Return a string list of data schema items tables names, if schema is not already loaded will be loaded,
        /// if record tagged values is setted to true returns list of items records tagged values.</summary>
        public List<string> SchemaList(bool _RecordTaggedValues)
        {
            if (dbSchema.Items.Count<1) LoadSchema();
            return dbSchema.List(_RecordTaggedValues);
        }

        /// <summary>Returns return MDB file path represent local database template.</summary>
        public string TemplatePath()
        {
            if (dbTemplate.Trim().Length > 0) return SM.LibPath("Database", SM.FixPath("", dbTemplate, "mdb"));
            else return "";
        }

        /// <summary>Returns MDB file path represent local database temporary template.</summary>
        public string TemporaryPath()
        {
            return SM.FixPath(SM.TempPath(), dbName, "mdb");
        }

        /// <summary>Ritorna il tipo di database relativo alla stringa passata.</summary>
        public SMDatabaseType TypeFromString(string _DatabaseType)
        {
            _DatabaseType = _DatabaseType.Trim().ToUpper();
            if ((_DatabaseType == "ACCESS") || (_DatabaseType == "0")) return SMDatabaseType.Access;
            else if ((_DatabaseType == "SQLSVR") || (_DatabaseType == "1") || (_DatabaseType == "2")) return SMDatabaseType.Sql;
            else if ((_DatabaseType == "MYSQL") || (_DatabaseType == "3")) return SMDatabaseType.MySql;
            else if ((_DatabaseType == "DBASE4") || (_DatabaseType == "4")) return SMDatabaseType.DBase4;
            else return SMDatabaseType.None;
        }

        /// <summary>Ritorna la stringa relativa al tipo di database passato.</summary>
        public string TypeToString(SMDatabaseType _DatabaseType)
        {
            if (_DatabaseType == SMDatabaseType.Access) return "ACCESS";
            else if (_DatabaseType == SMDatabaseType.Sql) return "SQLSVR";
            else if (_DatabaseType == SMDatabaseType.SqlExpress) return "SQLEXP";
            else if (_DatabaseType == SMDatabaseType.MySql) return "MYSQL";
            else if (_DatabaseType == SMDatabaseType.DBase4) return "DBASE4";
            else return "";
        }

        /// <summary>Erase database connection parameters on application INI file for alias.
        /// Returns true if succeed.</summary>
        public bool Wipe(string _FileName, string _Alias)
        {
            bool b = false;
            string s;
            SMIni ini;
            SM.Error();
            _Alias = _Alias.Trim().ToUpper();
            if ((SM.Language != SMLanguage.Auto) && (_Alias.Length > 0))
            {
                ini = new SMIniFile(_FileName);
                s = "DATABASE " + _Alias;
                ini.WriteString(s, "TYPE", TypeToString(SMDatabaseType.None));
                ini.WriteString(s, "HOST", "");
                ini.WriteString(s, "NAME", "");
                ini.WriteString(s, "TEMPLATE", "");
                ini.WriteString(s, "PATH", "");
                ini.WriteString(s, "USER", "");
                ini.WriteString(s, "PASS", "");
                b = ini.Save();
                if (!b) SM.ErrorMessage = "Error wiping configuration to " + SM.ApplicationIniFile();
            }
            return b;
        }

        #endregion

        /* */
    
    }

    /* */

}
