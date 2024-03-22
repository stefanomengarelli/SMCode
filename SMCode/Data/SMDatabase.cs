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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace SMCode
{

    /* */

    /// <summary>SMCode database component.</summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SMDatabase : Component
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
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

        /// <summary>Last connection fail.</summary>
        private DateTime lastConnectionFail = DateTime.MinValue;
        /// <summary>Last connection string.</summary>
        private string lastConnectionString = "";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Database instance constructor.</summary>
        public SMDatabase(SMApplication _SMApplication)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
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

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Specifies whether or not database is open.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies whether or not database is open.")]
        public bool Active
        {
            get
            {
                if (type == SMDatabaseType.Sql)
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

        /*  ===================================================================
         *  Methods
         *  ===================================================================
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

        /// <summary>Load database parameters related to alias from fileName INI file. Returns true if succeed.</summary>
        public bool Load(string _Alias = "MAIN", string _FileName = "")
        {
            string section;
            SMIni ini;
            if (Close())
            {
                alias = _Alias.Trim().ToUpper();
                if (alias.Length > 0)
                {
                    ini = SM.NewIni(_FileName);
                    section = "DATABASE " + alias;
                    commandTimeout = ini.ReadInteger(section, "COMMAND_TIMEOUT", commandTimeout);
                    connectionString = ini.ReadString(section, "CONNECTION_STRING", connectionString);
                    connectionTimeout = ini.ReadInteger(section, "CONNECTION_TIMEOUT", connectionTimeout);
                    database = ini.ReadString(section, "DATABASE", database);
                    host = ini.ReadString(section, "HOST", host);
                    password = ini.ReadHexMask(section, "PASSWORD", password);
                    path = ini.ReadString(section, "PATH", path);
                    type = TypeFromString(ini.ReadString(section, "TYPE", TypeToString(type)));
                    user = ini.ReadString(section, "USER", user);
                    return ini.Save();
                }
                else return false;
            }
            else return false;
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

        /// <summary>Close and reopen database connection with alias, .mdb or .dbf file specified. Returns true if succeed.</summary>
        public bool Open(string _Alias = "")
        {
            bool r = true;
            string fileName, connStr;
            Close();
            if (_Alias.Trim().Length > 0)
            {
                if (_Alias.ToLower().EndsWith(".mdb")) return Open(SMDatabaseType.Mdb, "localhost", SM.FileNameWithoutExt(_Alias), "", SM.FilePath(_Alias), "", password);
                else if (_Alias.ToLower().EndsWith(".dbf")) return Open(SMDatabaseType.Dbf, "localhost", SM.FileNameWithoutExt(_Alias), "", SM.FilePath(_Alias), "", "");
                else r = Load(_Alias);
            }
            if (r)
            {
                r = false;
                connStr = GetConnectionString();
                if ((connStr == lastConnectionString) && (DateTime.Now < lastConnectionFail.AddSeconds(5)))
                {
                    lastConnectionString = connStr;
                    lastConnectionFail = DateTime.MinValue;
                    try
                    {
                        if (type == SMDatabaseType.Sql)
                        {
                            connectionSql = new SqlConnection(connStr);
                            connectionSql.Open();
                        }
                        else if (type == SMDatabaseType.MySql)
                        {
                            connectionMySql = new MySqlConnection(connStr);
                            connectionMySql.Open();
                        }
                        else if (type == SMDatabaseType.Dbf)
                        {
                            fileName = SM.Combine(path, database, "dbf");
                            if (SM.FileExists(fileName))
                            {
                                connectionOleDB = new OleDbConnection(connStr);
                                connectionOleDB.Open();
                            }
                        }
                        else
                        {
                            fileName = SM.Combine(path, database, "mdb");
                            if (SM.FileExists(fileName))
                            {
                                connectionOleDB = new OleDbConnection(connStr);
                                connectionOleDB.Open();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex);
                        lastConnectionFail = DateTime.Now;
                    }
                    r = this.Active;
                    if (!r) this.Close();
                }
                else SM.Raise("A connection to a database that had previously failed was retried before the minimum wait time.", false);
            }
            else SM.Raise("Error reading alias parameters.", false);
            return r;
        }

        /// <summary>Open database connection with database type, host name, database name, user name and password parameters.
        /// Returns true if succeed. </summary>
        public bool Open(SMDatabaseType _Type, 
            string _Host, 
            string _Database,
            string _ConnectionString,
            string _Path, 
            string _User, 
            string _Password)
        {
            type = _Type;
            host = _Host;
            database = _Database;
            connectionString = _ConnectionString;
            path = _Path;
            user = _User;
            password = _Password;
            return Open();
        }

        /// <summary>Save database parameters related to alias to fileName INI file. Returns true if succeed.</summary>
        public bool Save(string _Alias = "", string _FileName = "")
        {
            string section;
            SMIni ini;
            if (_Alias.Trim().Length < 1) _Alias = alias;
            if (alias.Trim().Length > 0)
            {
                ini = SM.NewIni(_FileName);
                section = "DATABASE " + alias;
                ini.WriteInteger(section, "COMMAND_TIMEOUT", commandTimeout);
                ini.WriteString(section, "CONNECTION_STRING", connectionString);
                ini.WriteInteger(section, "CONNECTION_TIMEOUT", connectionTimeout);
                ini.WriteString(section, "DATABASE", database);
                ini.WriteString(section, "HOST", host);
                ini.WriteHexMask(section, "PASSWORD", password);
                ini.WriteString(section, "PATH", path);
                ini.WriteString(section, "TYPE", TypeToString(type));
                ini.WriteString(section, "USER", user);
                return ini.Save();
            }
            else return false;
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

        #region Static Properties

        /*  ===================================================================
         *  Static Properties
         *  ===================================================================
         */

        /// <summary>Specifies database alias name to use.</summary>
        public static bool ClientMode { get; set; } = false;

        /// <summary>Specifies database alias name to use.</summary>
        public static string MySqlPrefix { get; set; } = "`";

        /// <summary>Specifies database alias name to use.</summary>
        public static string MySqlSuffix { get; set; } = "`";

        /// <summary>Specifies database alias name to use.</summary>
        public static string SqlPrefix { get; set; } = "[";

        /// <summary>Specifies database alias name to use.</summary>
        public static string SqlSuffix { get; set; } = "]";

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Compact MDB database file specified in file name parameter. Password must be setted 
        /// to security password to access database file or "" if not necessary. Return true if succeed.</summary>
        public static bool CompactMdb(string _FileName, string _Password)
        {
            bool r = false;
            object jro;
            object[] par;
            SMApplication SM = SMApplication.CurrentOrNew();
            string tmp = SM.Combine(SM.FilePath(_FileName), SM.FileNameWithoutExt(_FileName) + "_tmp", "mdb");
            string bkp = SM.Combine(SM.FilePath(_FileName), SM.FileNameWithoutExt(_FileName) + "_bkp", "mdb");
            string src = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + _FileName;
            string tgt = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + tmp + "; Jet OLEDB:Engine Type=5";
            if (!SM.Empty(_Password))
            {
                tgt += "; Jet OLEDB:Database Password = " + _Password;
                src += "; Jet OLEDB:Database Password = " + _Password;
            }
            try
            {
                SM.FileDelete(tmp);
                jro = Activator.CreateInstance(System.Type.GetTypeFromProgID("JRO.JetEngine"));
                par = new object[] { src, tgt };
                jro.GetType().InvokeMember("CompactDatabase", System.Reflection.BindingFlags.InvokeMethod, null, jro, par);
                SM.FileDelete(bkp);
                if (SM.FileMove(_FileName, bkp)) r = SM.FileMove(tmp, _FileName);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(jro);
                jro = null;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = false;
            }
            return r;
        }

        /// <summary>Return string containing sql statement with [ ] delimiters 
        /// turned in to ty type database delimiters.</summary>
        public static string Delimiters(string _SQLStatement, SMDatabaseType _DatabaseType)
        {
            int i;
            bool b;
            char c, c1, c2;
            StringBuilder r;
            if (_DatabaseType == SMDatabaseType.MySql)
            {
                b = false;
                r = new StringBuilder();
                c1 = MySqlPrefix[0];
                c2 = MySqlSuffix[0];
                for (i = 0; i < _SQLStatement.Length; i++)
                {
                    c = _SQLStatement[i];
                    if (c == '\'') b = !b;
                    else if (!b)
                    {
                        if (c == SqlPrefix[0]) c = c1;
                        else if (c == SqlSuffix[0]) c = c2;
                    }
                    r.Append(c);
                }
                return r.ToString();
            }
            else return _SQLStatement;
        }

        /// <summary>Try to create lock database file.</summary>
        public static bool Lock()
        {
            SMApplication SM = SMApplication.CurrentOrNew();
            if (!Locked())
            {
                return SM.SaveString(LockPath(),
                    SM.ExecutableName + ";" + SM.ToStr(DateTime.Now, SMDateFormat.iso8601, true)
                    + ";" + SM.Machine() + ";" + SM.User());
            }
            else return false;
        }

        /// <summary>Return true if database lock file exists.</summary>
        public static bool Locked()
        {
            SMApplication SM = SMApplication.CurrentOrNew();
            string f = LockPath();
            if (SM.FileExists(f)) return SM.FileDate(f) > DateTime.Now.AddHours(-4);
            else return false;
        }

        /// <summary>Return database lock file full path.</summary>
        public static string LockPath()
        {
            SMApplication SM = SMApplication.CurrentOrNew();
            return SM.Combine(SM.DataPath, SM.ExecutableName, "lck");
        }

        /// <summary>Set OleDb command names with char @ followed by field name wich related parameter (SourceColumn).</summary>
        public static void ParametersByName(OleDbCommand _OleDbCommand)
        {
            int i;
            string s;
            for (i = 0; i < _OleDbCommand.Parameters.Count; i++)
            {
                s = "@" + _OleDbCommand.Parameters[i].SourceColumn;
                if (_OleDbCommand.Parameters.IndexOf(s) < 0) _OleDbCommand.Parameters[i].ParameterName = s;
            }
        }

        /// <summary>Set SQL command names with char @ followed by field name wich related parameter (SourceColumn).</summary>
        public static void ParametersByName(SqlCommand _SqlCommand)
        {
            int i;
            string s;
            for (i = 0; i < _SqlCommand.Parameters.Count; i++)
            {
                s = "@" + _SqlCommand.Parameters[i].SourceColumn;
                if (_SqlCommand.Parameters.IndexOf(s) < 0) _SqlCommand.Parameters[i].ParameterName = s;
            }
        }

        /// <summary>Set MySQL command names with char @ followed by field name wich related parameter (SourceColumn).</summary>
        public static void ParametersByName(MySqlCommand _MySqlCommand)
        {
            int i;
            string s;
            for (i = 0; i < _MySqlCommand.Parameters.Count; i++)
            {
                s = "@" + _MySqlCommand.Parameters[i].SourceColumn;
                if (_MySqlCommand.Parameters.IndexOf(s) < 0) _MySqlCommand.Parameters[i].ParameterName = s;
            }
        }

        /// <summary>Try to delete database lock file.</summary>
        public static bool Unlock()
        {
            SMApplication SM = SMApplication.CurrentOrNew();
            SM.FileDelete(LockPath());
            return !Locked();
        }
        #endregion

        /* */

    }

    /* */

}
