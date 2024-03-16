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
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;

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

        /// <summary>SMApplication instance.</summary>
        private readonly SMApplication SM = null;

        /// <summary>Database command timeout in seconds.</summary>
        private int commandTimeout;

        /// <summary>Database OLE DB connection.</summary>
        private OleDbConnection connectionOleDB;
        /// <summary>Database SQL connection.</summary>
        private SqlConnection connectionSql;
        /// <summary>Database MySQL connection.</summary>
        private MySqlConnection connectionMySql;

        /// <summary>Database connection string.</summary>
        private string connectionString;
        /// <summary>Database connection timeout in seconds.</summary>
        private int connectionTimeout;

        /// <summary>Database alias.</summary>
        private string alias;
        /// <summary>Database host.</summary>
        private string host;
        /// <summary>Database name.</summary>
        private string database;
        /// <summary>Database password.</summary>
        private string password;
        /// <summary>Database path.</summary>
        private string path;
        /// <summary>Database type.</summary>
        private SMDatabaseType type;
        /// <summary>Database username.</summary>
        private string user;

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Database instance constructor.</summary>
        public SMDatabase(SMApplication _SM)
        {
            SM = _SM;
            InitializeComponent();
            Clear();
        }

        /// <summary>Database instance constructor with container.</summary>
        public SMDatabase(IContainer _Container)
        {
            SM = SMApplication.Application;
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
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies whether or not database is open.")]
        public bool Active
        {
            get
            {
                if ((type == SMDatabaseType.SqlExpress) || (type == SMDatabaseType.Sql))
                {
                    if (connectionSql != null) return connectionSql.State == ConnectionState.Open;
                    else return false;
                }
                else if (type == SMDatabaseType.MySql)
                {
                    if (connectionMySql != null) return connectionMySql.State == ConnectionState.Open;
                    else return false;
                }
                else
                {
                    if (connectionOleDB != null) return connectionOleDB.State == ConnectionState.Open;
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
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database alias name to use.")]
        public string Alias
        {
            get { return alias; }
            set { alias = value.Trim().ToUpper(); }
        }

        /// <summary>Specifies database command timeout.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database command timeout.")]
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        /// <summary>Indicated used connection form MySql database type.</summary>
        [Browsable(false)]
        public MySqlConnection ConnectionMySql
        {
            get { return connectionMySql; }
            set { connectionMySql = value; }
        }

        /// <summary>Indicated used connection form OleDB database type.</summary>
        [Browsable(false)]
        public OleDbConnection ConnectionOleDB
        {
            get { return connectionOleDB; }
            set { connectionOleDB = value; }
        }

        /// <summary>Indicated used connection form Sql database type.</summary>
        [Browsable(false)]
        public SqlConnection ConnectionSql
        {
            get { return connectionSql; }
            set { connectionSql = value; }
        }

        /// <summary>Get or set database connection string.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Get or set database connection string.")]
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>Specifies database connection timeout in seconds.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database connection timeout in seconds.")]
        public int ConnectionTimeout
        {
            get { return connectionTimeout; }
            set { connectionTimeout = value; }
        }

        /// <summary>Specifies default database name.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies default database name.")]
        public string Database
        {
            get { return database; }
            set { database = value.Trim(); }
        }

        /// <summary>Specifies database host name or address.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database host name or address.")]
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>Specifies database password.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database password.")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>Specifies database path.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database path.")]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>Specifies database type.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database type.")]
        public SMDatabaseType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>Specifies database user name.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database user name.")]
        public string User
        {
            get { return user; }
            set { user = value; }
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
            alias = "";
            commandTimeout = SM.ToInt(SM.ReadIni("", "DATABASE", "COMMAND_TIMEOUT", "120"));
            connectionMySql = null;
            connectionOleDB = null;
            connectionSql = null;
            connectionString = "";
            connectionTimeout = SM.ToInt(SM.ReadIni("", "DATABASE", "CONNECTION_TIMEOUT", "120"));
            database = "";
            host = "";
            password = "";
            path = "";
            type = SMDatabaseType.Mdb;
            user = "";
        }

        /// <summary>Close database connection. Returns true if succeed.</summary>
        public bool Close()
        {
            //
            // Close OleDB connection
            //
            try
            {
                if (connectionOleDB != null)
                {
                    if (connectionOleDB.State != ConnectionState.Closed) connectionOleDB.Close();
                    connectionOleDB.Dispose();
                    connectionOleDB = null;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            //
            // Close SQL connection
            //
            try
            {
                if (connectionSql != null)
                {
                    if (connectionSql.State != ConnectionState.Closed) connectionSql.Close();
                    connectionSql.Dispose();
                    connectionSql = null;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            //
            // Close mySQL connection
            //
            try
            {
                if (connectionMySql != null)
                {
                    if (connectionMySql.State != ConnectionState.Closed) connectionMySql.Close();
                    connectionMySql.Dispose();
                    connectionMySql = null;
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

        /// <summary>Return connection string.</summary>
        public string GetConnectionString()
        {
            if (connectionString.Trim().Length > 0) return connectionString.Trim();
            else if (type == SMDatabaseType.Mdb) return "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=%%MDBPATH%%; Jet OLEDB:Database Password=%%PASSWORD%%";
            else if (type == SMDatabaseType.Sql) return "Data Source=%%HOST%%; Initial Catalog=%%DATABASE%%; User Id=%%USER%%; Password=%%PASSWORD%%; Connection Timeout=%%TIMEOUT%%; Encrypt=True; TrustServerCertificate=True;";
            else if (type == SMDatabaseType.MySql) return "Persist Security Info=False; Database=%%DATABASE%%; Data Source=%%HOST%%; Connect Timeout=%%TIMEOUT%%; User Id=%%USER%%; Password=%%PASSWORD%%;";
            else if (type == SMDatabaseType.Dbf) return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=%%PATH%%;Extended Properties=dBASE IV;User ID=Admin;Password=;";
            else return "";
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
            if (Close())
            {
                _Alias = _Alias.Trim().ToUpper();
                if ((_Alias.Length > 0) && (SM.Language != SMLanguage.Auto))
                {
                    ini = new SMIniFile(_FileName);
                    s = "DATABASE " + _Alias;
                    ini.WriteDefault = true;
                    alias = "";
                    type = TypeFromString(ini.ReadString(s, "TYPE", TypeToString(SMDatabaseType.Mdb)));
                    host = ini.ReadString(s, "HOST", "localhost");
                    database = ini.ReadString(s, "NAME", SM.ExecName);
                    dbTemplate = ini.ReadString(s, "TEMPLATE", SM.ExecName);
                    path = SM.Macro(ini.ReadString(s, "PATH", SM.DataDir));
                    user = ini.ReadString(s, "USER", "");
                    password = ini.ReadMasked(s, "PASS", SM.DatabaseAccessPassword);
                    if ((type == SMDatabaseType.Mdb) && (password.Trim().Length < 1)) password = SM.DatabaseAccessPassword;
                    if (_AssignAliasName) alias = _Alias;
                    ini.Save();
                    r = true;
                }
                else r = false;
            }
            return r;
        }

        /// <summary>Return connection string with valued macro.</summary>
        public string MacroConnectionString(string _ConnectionString)
        {
            return  _ConnectionString.Trim()
                .Replace("%%HOST%%", host)
                .Replace("%%DATABASE%%", database)
                .Replace("%%PATH%%", path)
                .Replace("%%USER%%", user)
                .Replace("%%PASSWORD%%", password)
                .Replace("%%MDBPATH%%", SM.Combine(path, database, "mdb"))
                .Replace("%%TIMEOUT%%", connectionTimeout.ToString());
        }

        /// <summary>Close and reopen database connection with actual parameters. Returns true if succeed.</summary>
        public bool Open()
        {
            bool r = false;
            string f, connStr, t;
            Close();
            if (alias.Trim().Length > 0) r = Load(alias);
            else r = true;
            if (r)
            {
                r = false;
                connStr = GetConnectionString();
                if ((connStr == SM.Databases.LastConnectionString) && (DateTime.Now < SM.Databases.LastConnectionFail.AddSeconds(5)))
                {
                    SM.Raise(SM.T("@SM|E' stata ritentata una connessione ad un database che era precedentemente fallita prima del tempo minimo di attesa."), false);
                }
                else
                {
                    SM.Databases.LastConnectionString = connStr;
                    SM.Databases.LastConnectionFail = DateTime.MinValue;
                    try
                    {
                        if (type == SMDatabaseType.SqlExpress)
                        {
                            connectionSql = new SqlConnection(connStr);
                            w = SM.ProgressDialog(SM.T("@SM|Apertura database"),
                                SM.T("@SM|Connessione con il database in corso..."),
                                SM.T("@SM|Attendere la connessione al database. L'applicazione proverà a connettersi per un periodo massimo di %%0%% secondi.").Replace("%%0%%", connectionSql.ConnectionTimeout.ToString()),
                                SMProgressBarMode.None,
                                false,
                                SM.ThemeImage("icons/ui_icon_48_dbopen"));
                            connectionSql.Open();
                            SM.ProgressDialog(ref w);
                        }
                        else if (type == SMDatabaseType.Sql)
                        {
                            connectionSql = new SqlConnection(connStr);
                            w = SM.ProgressDialog(SM.T("@SM|Apertura database"),
                                SM.T("@SM|Connessione con il database in corso..."),
                                SM.T("@SM|Attendere la connessione al database. L'applicazione proverà a connettersi per un periodo massimo di %%0%% secondi.").Replace("%%0%%", connectionSql.ConnectionTimeout.ToString()),
                                SMProgressBarMode.None,
                                false,
                                SM.ThemeImage("icons/ui_icon_48_dbopen"));
                            connectionSql.Open();
                            SM.ProgressDialog(ref w);
                        }
                        else if (type == SMDatabaseType.MySql)
                        {
                            connectionMySql = new MySqlConnection(connStr);
                            w = SM.ProgressDialog(SM.T("@SM|Apertura database"),
                                SM.T("@SM|Connessione con il database in corso..."),
                                SM.T("@SM|Attendere la connessione al database. L'applicazione proverà a connettersi per un periodo massimo di %%0%% secondi.").Replace("%%0%%", connectionMySql.ConnectionTimeout.ToString()),
                                SMProgressBarMode.None,
                                false,
                                SM.ThemeImage("icons/ui_icon_48_dbopen"));
                            connectionMySql.Open();
                            SM.ProgressDialog(ref w);
                        }
                        else if (type == SMDatabaseType.Dbf)
                        {
                            f = SM.Combine(path, database, "dbf");
                            if (SM.FileExists(f))
                            {
                                connectionOleDB = new OleDbConnection(connStr);
                                connectionOleDB.Open();
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
                                connectionOleDB = new OleDbConnection(connStr);
                                connectionOleDB.Open();
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
            type = _DatabaseType;
            host = _DatabaseHost;
            database = _DatabaseName;
            path = _DatabasePath;
            user = _UserName;
            password = _Password;
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
                    SMDatabaseType.Dbf, 
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
            if (_FileName.Trim().Length > 0)
            {
                r = Open(SMDatabaseType.Mdb,"localhost", 
                    SM.FileNameWithoutExt(_FileName), "",
                    SM.FilePath(_FileName), "", password);
            }
            return r;
        }

        /// <summary>Return database type from string value.</summary>
        public SMDatabaseType TypeFromString(string _DatabaseType)
        {
            _DatabaseType = _DatabaseType.Trim().ToUpper();
            if (_DatabaseType == "MDB") return SMDatabaseType.Mdb;
            else if (_DatabaseType == "SQL") return SMDatabaseType.Sql;
            else if (_DatabaseType == "MYSQL") return SMDatabaseType.MySql;
            else if (_DatabaseType == "DBF") return SMDatabaseType.Dbf;
            else return SMDatabaseType.None;
        }

        /// <summary>Return string corresponding to database type.</summary>
        public string TypeToString(SMDatabaseType _DatabaseType)
        {
            if (_DatabaseType == SMDatabaseType.Mdb) return "MDB";
            else if (_DatabaseType == SMDatabaseType.Sql) return "SQL";
            else if (_DatabaseType == SMDatabaseType.MySql) return "MYSQL";
            else if (_DatabaseType == SMDatabaseType.Dbf) return "DBF";
            else return "";
        }

        #endregion

        /* */
    
    }

    /* */

}
