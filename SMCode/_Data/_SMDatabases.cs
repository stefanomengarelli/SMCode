/*  ------------------------------------------------------------------------
 *  
 *  File:       SMDatabases.cs
 *  Version:    6.10.0
 *  Date:       January 2020
 *  Author:     SM  
 *  E-mail:     -
 *  
 *  Copyright (c) 2018-2020 all rights reserved.
 *
 *  Application databases collection class.
 *
 *  ------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;

namespace SMNetCode
{

    /* */

    /// <summary>Application databases collection class.</summary>
    public class SMDatabases
    {

        /* */

        #region Declarations

        /*  --------------------------------------------------------------------
         *  Declarations
         *  --------------------------------------------------------------------
         */

        /// <summary>Database items list.</summary>
        private List<SMDatabase> items = new List<SMDatabase>();

        /// <summary>Last connection string.</summary>
        private string lastConnectionString = "";

        /// <summary>Last connection time.</summary>
        private DateTime lastConnectionTime = DateTime.MinValue;

        /// <summary>Last connection fail.</summary>
        private DateTime lastConnectionFail = DateTime.MinValue;

        /// <summary>Connection timeout in seconds.</summary>
        private int connectionTimeout = 30;

        /// <summary>Command timeout in seconds.</summary>
        private int commandTimeout = 0;

        /// <summary>Deferred data load delay in milliseconds.</summary>
        private int dataLoadDelay = 330;

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>SMDatabases constructor.</summary>
        public SMDatabases()
        {
            SM.Initialize();
            Clear();
        }

        #endregion

        /* */

        #region Properties

        /*  --------------------------------------------------------------------
         *  Properties
         *  --------------------------------------------------------------------
         */

        /// <summary>Get or set database command default timeout in seconds.</summary>
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        /// <summary>Get or set database connection default timeout in seconds.</summary>
        public int ConnectionTimeout
        {
            get { return connectionTimeout; }
            set { connectionTimeout = value; }
        }

        /// <summary>Get or set database default deferred data load delay interval in milliseconds.</summary>
        public int DataLoadDelay
        {
            get { return dataLoadDelay; }
            set { dataLoadDelay = value; }
        }

        /// <summary>Get database items.</summary>
        public List<SMDatabase> Items
        {
            get { return items; }
        }

        /// <summary>Get or set last database connection fail time.</summary>
        public DateTime LastConnectionFail
        {
            get { return lastConnectionFail; }
            set { lastConnectionFail = value; }
        }

        /// <summary>Get or set last database connection string.</summary>
        public string LastConnectionString
        {
            get { return lastConnectionString; }
            set { lastConnectionString = value; }
        }

        /// <summary>Get or set last database successful connection time.</summary>
        public DateTime LastConnectionTime
        {
            get { return lastConnectionTime; }
            set { lastConnectionTime = value; }
        }

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Add database with aliasName to list and load parameters.
        /// Returns list index of database or -1 if fails.</summary>
        public int Add(string _Alias, bool _CreateAliasIfNotExists)
        {
            int r;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.Add(\"" + _Alias + "\"," + SM.Iif(_CreateAliasIfNotExists, "true", "false") + ")");
            r = Add(_Alias, SMDatabaseType.Access, "", "", "", "", "", "", _CreateAliasIfNotExists);
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Add database with aliasName to list and load parameters. 
        /// If specified create alias on ini file if not exists.
        /// Returns list index of database or -1 if fails.</summary>
        public int Add(string _Alias, SMDatabaseType _DatabaseType, string _DatabaseHost, string _DatabaseName, 
            string _Template, string _Path, string _User, string _Password, bool _CreateAliasIfNotExists)
        {
            int i, r = -1;
            SMDatabase db;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.Add(\"" + _Alias + "\", \"" + _DatabaseType.ToString() + "\", \""
                + _DatabaseHost + "\", \"" + _DatabaseName + "\", \"" + _Template + "\", \"" + _Path + "\", \"" + _User + "\", \""
                + _Password + "\", " + SM.Iif(_CreateAliasIfNotExists, "true", "false") + ")");
            try
            {
                _Alias = _Alias.Trim().ToUpper();
                if (_Alias.Length > 0)
                {
                    if (_CreateAliasIfNotExists) SM.CreateAlias("", _Alias, _DatabaseType, _DatabaseHost, _DatabaseName, _Template, _Path, _User, _Password);
                    i = Find(_Alias);
                    if (i < 0)
                    {
                        db = new SMDatabase();
                        if (db.Load(_Alias))
                        {
                            items.Add(db);
                            r = items.Count - 1;
                        }
                        else
                        {
                            db.Dispose();
                            r = -1;
                        }
                    }
                    else r = i;
                }
                else r = -1;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = -1;
            }
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Return a string array containing alias ids.</summary>
        public string[] AliasArray()
        {
            int i;
            string[] r = new string[items.Count];
            for (i = 0; i < items.Count; i++) r[i] = items[i].Alias;
            return r;
        }

        /// <summary>Close and dispose all databases items.</summary>
        public bool Clear()
        {
            int i;
            bool r = false;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.Clear()");
            if (Close(""))
            {
                for (i = 0; i < items.Count; i++)
                {
                    if (items[i] != null)
                    {
                        items[i].Close();
                        items[i].Dispose();
                    }
                }
                items.Clear();
                r = true;
            }
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Close database with aliasName or all if aliasName is empty.
        /// Returns true if succeed.</summary>
        public bool Close(string _Alias)
        {
            int i;
            bool r = true;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.Close(\"" + _Alias + "\")");
            _Alias = _Alias.Trim().ToUpper();
            if (_Alias.Length > 0)
            {
                i = Find(_Alias);
                if (i > -1) r = items[i].Close();
                else r = false;
            }
            else r = CloseAll();
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Close all application databases. Returns true if succeed.</summary>
        public bool CloseAll()
        {
            int i;
            bool r = true;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.CloseAll()");
            for (i = 0; i < items.Count; i++) if (!items[i].Close()) r = false;
            SM.MemoryRelease(true);
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Returns index of aliasName or -1 if not found.</summary>
        public int Find(string _Alias)
        {
            int i = 0, r = -1;
            _Alias = _Alias.Trim().ToUpper();
            if (_Alias.Length > 0)
            {
                while ((i < items.Count) && (r < 0))
                {
                    if (items[i].Alias.ToUpper() == _Alias) r = i;
                    i++;
                }
            }
            return r;
        }

        /// <summary>Keep database with aliasName active. 
        /// Returns SMDatabase instance if succeed otherwise null.</summary>
        public SMDatabase Keep(string _Alias)
        {
            int i;
            SMDatabase r = null;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.Keep(\"" + _Alias + "\")");
            _Alias = _Alias.Trim().ToUpper();
            if (_Alias.Length > 0)
            {
                i = Add(_Alias, false);
                if (i > -1) 
                {
                    items[i].Keep();
                    r = items[i];
                }
            }
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Load databases settings.</summary>
        public void LoadSettings()
        {
            SMIniFile ini;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.LoadSettings()");
            ini = new SMIniFile("");
            connectionTimeout = ini.ReadInteger("DBSETUP", "CONNECTION_TIMEOUT", 30);
            commandTimeout = ini.ReadInteger("DBSETUP", "CONNECTION_TIMEOUT", 0);
            dataLoadDelay = ini.ReadInteger("DBSETUP", "DATA_LOAD_DELAY", 330);
            SM.ErrorTracePop();
        }

        /// <summary>Open database with alias. Returns true if succeed.</summary>
        public bool Open(string _Alias)
        {
            int i;
            bool r = false;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.Open(\"" + _Alias + "\")");
            _Alias = _Alias.Trim().ToUpper();
            if (_Alias.Length > 0)
            {
                i = Add(_Alias, false);
                if (i > -1) r = items[i].Open();
            }
            SM.ErrorTracePop();
            return r;
        }

        /// <summary>Save databases settings.</summary>
        public void SaveSettings()
        {
            SMIniFile ini;
            SM.ErrorTracePush("SMDatabases.cs", "SMDatabases.SaveSettings()");
            ini = new SMIniFile("");
            ini.ReadInteger("DBSETUP", "CONNECTION_TIMEOUT", connectionTimeout);
            ini.ReadInteger("DBSETUP", "CONNECTION_TIMEOUT", commandTimeout);
            ini.ReadInteger("DBSETUP", "DATA_LOAD_DELAY", dataLoadDelay);
            ini.Save();
            SM.ErrorTracePop();
        }

        #endregion

        /* */

    }

    /* */

}
