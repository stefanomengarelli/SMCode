/*  ===========================================================================
 *  
 *  File:       SMDataset.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dataset component.
 *
 *  ===========================================================================
 */

using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace SMCode
{

    /* */

    /// <summary>Dataset component.</summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SMDataset : Component
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SMApplication instance.</summary>
        private readonly SMApplication SM = null;

        /// <summary>Component disposing flag.</summary>
        private bool disposing = false;

        /// <summary>Dataset database alias.</summary>
        private string alias = "";
        /// <summary>Dataset database instance.</summary>
        private SMDatabase database = null;

        /// <summary>Current record index.</summary>
        private int recordIndex = -1;
        /// <summary>Record changes buffer count.</summary>
        private int bufferCount = 0;
        /// <summary>Adapted query string.</summary>
        private string adaptedQuery = "";

        /// <summary>OleDB command instance.</summary>
        private OleDbCommand oleCommand = null;
        /// <summary>OleDB data reader instance.</summary>
        private OleDbDataReader oleReader = null;
        /// <summary>OleDB data adapter instance.</summary>
        private OleDbDataAdapter oleAdapter = null;
        /// <summary>OleDB command builder instance.</summary>
        private OleDbCommandBuilder oleBuilder = null;

        /// <summary>SQL command instance.</summary>
        private SqlCommand sqlCommand = null;
        /// <summary>SQL data reader instance.</summary>
        private SqlDataReader sqlReader = null;
        /// <summary>SQL data adapter instance.</summary>
        private SqlDataAdapter sqlAdapter = null;
        /// <summary>SQL command builder instance.</summary>
        private SqlCommandBuilder sqlBuilder = null;

        /// <summary>MySQL command instance.</summary>
        private MySqlCommand mySqlCommand = null;
        /// <summary>MySQL data reader instance.</summary>
        private MySqlDataReader mySqlReader = null;
        /// <summary>MySQL data adapter instance.</summary>
        private MySqlDataAdapter mySqlAdapter = null;
        /// <summary>MySQL command builder instance.</summary>
        private MySqlCommandBuilder mySqlBuilder = null;

        #endregion

        /* */

        #region Delegates and events

        /*  ===================================================================
         *  Delegates and events
         *  ===================================================================
         */

        /// <summary>Occurs after dataset completes a request to cancel modifications to the active record.</summary>
        public delegate void OnAfterCancel(object _Sender);
        /// <summary>Occurs after dataset completes a request to cancel modifications to the active record.</summary>
        public event OnAfterCancel AfterCancel;

        /// <summary>Occurs after dataset close query.</summary>
        public delegate void OnAfterClose(object _Sender);
        /// <summary>Occurs after dataset close query.</summary>
        public event OnAfterClose AfterClose;

        /// <summary>Occurs after dataset deletes a record.</summary>
        public delegate void OnAfterDelete(object _Sender);
        /// <summary>Occurs after dataset deletes a record.</summary>
        public event OnAfterDelete AfterDelete;

        /// <summary>Occurs after dataset starts editing a record.</summary>
        public delegate void OnAfterEdit(object _Sender);
        /// <summary>Occurs after dataset starts editing a record.</summary>
        public event OnAfterEdit AfterEdit;

        /// <summary>Occurs after dataset inserts a new record.</summary>
        public delegate void OnAfterInsert(object _Sender);
        /// <summary>Occurs after dataset inserts a new record.</summary>
        public event OnAfterInsert AfterInsert;
        
        /// <summary>Occurs after dataset completes opening.</summary>
        public delegate void OnAfterOpen(object _Sender);
        /// <summary>Occurs after dataset completes opening.</summary>
        public event OnAfterOpen AfterOpen;

        /// <summary>Occurs after dataset posts modifications to the active record.</summary>
        public delegate void OnAfterPost(object _Sender);
        /// <summary>Occurs after dataset posts modifications to the active record.</summary>
        public event OnAfterPost AfterPost;

        /// <summary>Occurs before dataset executes a request to cancel changes to the active record.</summary>
        public delegate void OnBeforeCancel(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset executes a request to cancel changes to the active record.</summary>
        public event OnBeforeCancel BeforeCancel;

        /// <summary>Occurs before dataset close query.</summary>
        public delegate void OnBeforeClose(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset close query.</summary>
        public event OnBeforeClose BeforeClose;

        /// <summary>Occurs before dataset attempts to delete the active record.</summary>
        public delegate void OnBeforeDelete(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset attempts to delete the active record.</summary>
        public event OnBeforeDelete BeforeDelete;

        /// <summary>Occurs before dataset enters edit mode for the active record.</summary>
        public delegate void OnBeforeEdit(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset enters edit mode for the active record.</summary>
        public event OnBeforeEdit BeforeEdit;

        /// <summary>Occurs before dataset enters insert mode.</summary>
        public delegate void OnBeforeInsert(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset enters insert mode.</summary>
        public event OnBeforeInsert BeforeInsert;

        /// <summary>Occurs before dataset executes a request to open query.</summary>
        public delegate void OnBeforeOpen(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset executes a request to open query.</summary>
        public event OnBeforeOpen BeforeOpen;

        /// <summary>Occurs before dataset posts modifications to the active record.</summary>
        public delegate void OnBeforePost(object _Sender, ref bool _AbortOperation);
        /// <summary>Occurs before dataset posts modifications to the active record.</summary>
        public event OnBeforePost BeforePost;

        /// <summary>Occurs when the record index changes.</summary>
        public delegate void OnRecordChange(object _Sender);
        /// <summary>Occurs when the record index changes.</summary>
        public event OnRecordChange RecordChange;

        /// <summary>Occurs when the state of dataset changes.</summary>
        public delegate void OnStateChange(object _Sender);
        /// <summary>Occurs when the state of dataset changes.</summary>
        public event OnStateChange StateChange;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Specifies whether or not dataset is open.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies whether or not dataset is open.")]
        public bool Active
        {
            get { return State != SMDatasetState.Closed; }
            set 
            { 
                if (value) Open(); 
                else Close();  
            }
        }

        /// <summary>Specifies database alias name to use with dataset.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies database alias name to use with dataset.")]
        public string Alias
        {
            get { return alias; }
            set { alias = value.Trim().ToUpper(); }
        }

        /// <summary>Specifies whether or not dataset is in insert state.</summary>
        [Browsable(false)]
        public bool Appending
        {
            get { return State == SMDatasetState.Insert; }
        }

        /// <summary>Indicates whether the first record in the dataset is active.</summary>
        [Browsable(false)]
        public bool Bof { get; private set; } = false;

        /// <summary>Specifies whether or not dataset is in browse state.</summary>
        [Browsable(false)]
        public bool Browsing
        {
            get { return State == SMDatasetState.Browse; }
        }

        /// <summary>Indicates how many changes can buffered before writes them to the database.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Indicates how many changes can buffered before writes them to the database.")]
        public int ChangesBufferSize { get; set; } = 1;

        /// <summary>Specifies whether or not dataset is closed.</summary>
        [Browsable(false)]
        public bool Closed
        {
            get { return State == SMDatasetState.Closed; }
        }

        /// <summary>Specifies the database connection component to use.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Specifies the database connection component to use.")]
        public SMDatabase Database { get; set; } = null;

        /// <summary>Specifies whether or not dataset is in edit state.</summary>
        [Browsable(false)]
        public bool Editing
        {
            get { return State == SMDatasetState.Edit; }
        }

        /// <summary>Specifies whether dataset contains no records.</summary>
        [Browsable(false)]
        public bool Empty
        {
            get { return RecordCount() < 1; }
        }

        /// <summary>Indicates whether a dataset is positioned at the last record.</summary>
        [Browsable(false)]
        public bool Eof { get; private set; } = false;

        /// <summary>Get dataset instance.</summary>
        public DataSet Dataset { get; private set; }

        /// <summary>Get or set extended dataset.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Get or set extended dataset.")]
        public SMDataset ExtendedDataset { get; set; } = null;

        /// <summary>Contains the text of the SQL statement to execute for the dataset.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Contains the text of the SQL statement to execute for the dataset.")]
        public string Query { get; set; } = "";

        /// <summary>Indicates if dataset is read-only.</summary>
        [Browsable(false)]
        public bool ReadOnly { get; private set; } = false;

        /// <summary>Indicates the index of the current record in the dataset.</summary>
        [Browsable(false)]
        public int RecordIndex
        {
            get { return recordIndex; }
            set { Goto(value); }
        }

        /// <summary>Specifies if dataset position is on an available record.</summary>
        [Browsable(false)]
        public bool RecordAvailable
        {
            get { return (recordIndex > -1) && (recordIndex < RecordCount()); }
        }

        /// <summary>Indicates the current active row of the dataset.</summary>
        [Browsable(false)]
        public DataRow Row { get; private set; } = null;

        /// <summary>Indicates the current operating mode of the dataset.</summary>
        [Browsable(false)]
        public SMDatasetState State { get; private set; }

        /// <summary>Indicates the current table object open in dataset.</summary>
        [Browsable(false)]
        public DataTable Table { get; private set; }

        /// <summary>Indicates the current table name open in dataset.</summary>
        [Browsable(false)]
        public string TableName { get; private set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Dataset instance constructor.</summary>
        public SMDataset(SMApplication _SMApplication)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            InitializeComponent();
            Clear();
        }

        /// <summary>Dataset instance constructor with db database connection.</summary>
        public SMDataset(SMApplication _SMApplication, SMDatabase _Database)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            InitializeComponent();
            Clear();
            database = _Database;
        }

        /// <summary>Dataset instance constructor with alias connection.</summary>
        public SMDataset(SMApplication _SMApplication, string _Alias)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            InitializeComponent();
            Clear();
            alias = _Alias;
            database = SM.Databases.Keep(alias);
        }

        /// <summary>Dataset instance constructor with ds dataset connection.</summary>
        public SMDataset(SMApplication _SMApplication, SMDataset _DataSet)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            InitializeComponent();
            Clear();
            if (_DataSet != null)
            {
                if (_DataSet.Alias.Trim().Length > 0)
                {
                    alias = _DataSet.Alias;
                    database = SM.Databases.Keep(alias);
                }
                else database = _DataSet.Database;
            }
        }

        /// <summary>Dataset instance constructor with container</summary>
        public SMDataset(IContainer _Container)
        {
            SM = SMApplication.Application;
            _Container.Add(this);
            InitializeComponent();
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Set current dataset state to newState.</summary>
        public void ChangeState(SMDatasetState _State)
        {
            if (State != _State)
            {
                State = _State;
                if (StateChange != null) StateChange(this);
            }
        }

        /// <summary>Initialize and reset dataset variables.</summary>
        public void Clear()
        {
            database = null;
            alias = "";
            //
            Bof = true;
            Eof = true;
            State = SMDatasetState.Closed;
            recordIndex = -1;
            ReadOnly = true;
            bufferCount = 0;
            ChangesBufferSize = 1;
            Query = "";
            adaptedQuery = "";
            //
            oleCommand = null;
            oleReader = null;
            oleAdapter = null;
            oleBuilder = null;
            //
            sqlCommand = null;
            sqlReader = null;
            sqlAdapter = null;
            sqlBuilder = null;
            //
            mySqlCommand = null;
            mySqlReader = null;
            mySqlAdapter = null;
            mySqlBuilder = null;
            //
            Dataset = null;
            Table = null;
            Row = null;
            TableName = "";
            //
            ExtendedDataset = null;
        }

        /// <summary>Close dataset. Return true if succeed.</summary>
        public bool Close()
        {
            bool r = false, cancel = false;
            //
            if (BeforeClose != null) BeforeClose(this, ref cancel);
            if (!cancel)
            {
                try
                {
                    if (database != null)
                    {
                        if (database.Active)
                        {
                            if (bufferCount > 0) Commit();
                            //
                            if (oleReader != null) 
                            { 
                                oleReader.Close(); 
                                oleReader.Dispose(); 
                            }
                            if (sqlReader != null) 
                            { 
                                sqlReader.Close(); 
                                sqlReader.Dispose(); 
                            }
                            if (mySqlReader != null) 
                            { 
                                mySqlReader.Close(); 
                                mySqlReader.Dispose(); 
                            }
                            //
                            if (oleCommand != null) oleCommand.Dispose();
                            if (sqlCommand != null) sqlCommand.Dispose();
                            if (mySqlCommand != null) mySqlCommand.Dispose();
                            //
                            if (oleBuilder != null) oleBuilder.Dispose();
                            if (sqlBuilder != null) sqlBuilder.Dispose();
                            if (mySqlBuilder != null) mySqlBuilder.Dispose();
                            //
                            if (Dataset != null) Dataset.Dispose();
                            //
                            if (oleAdapter != null) oleAdapter.Dispose();
                            if (sqlAdapter != null) sqlAdapter.Dispose();
                            if (mySqlAdapter != null) mySqlAdapter.Dispose();
                        }
                    }
                    r = true;
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                }
                //
                ChangeState(SMDatasetState.Closed);
                //
                oleReader = null;
                sqlReader = null;
                mySqlReader = null;
                //
                oleCommand = null;
                sqlCommand = null;
                mySqlCommand = null;
                //
                oleBuilder = null;
                sqlBuilder = null;
                mySqlBuilder = null;
                //
                Dataset = null;
                //
                oleAdapter = null;
                sqlAdapter = null;
                mySqlAdapter = null;
                //
                Table = null;
                Row = null;
                Eof = true;
                Bof = true;
                //
                LinkedDatasetClose();
                //
                if (AfterClose != null) AfterClose(this);
            }
            //
            return r;
        }

        /// <summary>Return true if fieldName is the name of one of any table open field.</summary>
        public bool IsField(string _FieldName)
        {
            if (_FieldName.Length < 1) return false;
            else if (Table != null) return Table.Columns.IndexOf(_FieldName) > -1;
            else if ((database.Type == SMDatabaseType.Mdb) && (oleReader != null)) return oleReader.GetOrdinal(_FieldName) > -1;
            else if ((database.Type == SMDatabaseType.Dbf) && (oleReader != null)) return oleReader.GetOrdinal(_FieldName) > -1;
            else if ((database.Type == SMDatabaseType.MySql) && (mySqlReader != null)) return mySqlReader.GetOrdinal(_FieldName) > -1;
            else if (sqlReader != null) return sqlReader.GetOrdinal(_FieldName) > -1;
            else return false;
        }

        /// <summary>*TODO* Load the recordset. Return true if succeed.</summary>
        public bool Load()
        {
            Table = null;
            Row = null;
            TableName = "";
            try
            {
                if (OpenDatabase())
                {
                    Dataset.Clear();
                    if (database.Type == SMDatabaseType.Mdb) oleAdapter.Fill(Dataset);
                    else if (database.Type == SMDatabaseType.Dbf) oleAdapter.Fill(Dataset);
                    else if (database.Type == SMDatabaseType.MySql) mySqlAdapter.Fill(Dataset);
                    else sqlAdapter.Fill(Dataset);
                    Table = Dataset.Tables[0];
                    TableName = SM.BtwU(adaptedQuery + " ", " from ", " ").Trim();
                    if (TableName.Length > 2)
                    {
                        if (((TableName[0] == SMDatabase.MySqlPrefix[0]) && (TableName[TableName.Length - 1] == SMDatabase.MySqlSuffix[0]))
                            || ((TableName[0] == SMDatabase.SqlPrefix[0]) && (TableName[TableName.Length - 1] == SMDatabase.SqlSuffix[0])))
                        {
                            TableName = TableName.Substring(1, TableName.Length - 2);
                        }
                    }
                    if (Table.Columns.IndexOf("ID") > -1)
                    {
                        if (!ReadOnly)
                        {
                            if (database.Type == SMDatabaseType.Mdb)
                            {
                                // Insert command
                                oleAdapter.InsertCommand.CommandText = SM.SqlInsertCommandString(this);
                                if (database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(oleAdapter.InsertCommand);
                                // Update command
                                oleAdapter.UpdateCommand.CommandText = SM.SqlUpdateCommandString(this);
                                if (database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(oleAdapter.UpdateCommand);
                                // Delete command
                                oleAdapter.DeleteCommand.CommandText = SM.SqlDeleteCommandString(this);
                                if (database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(oleAdapter.DeleteCommand);
                            }
                            else if (database.Type == SMDatabaseType.DBase4)
                            {
                                // Insert command
                                oleAdapter.InsertCommand.CommandText = SM.SqlInsertCommandString(this);
                                if (database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(oleAdapter.InsertCommand);
                                // Update command
                                oleAdapter.UpdateCommand.CommandText = SM.SqlUpdateCommandString(this);
                                if (database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(oleAdapter.UpdateCommand);
                                // Delete command
                                oleAdapter.DeleteCommand.CommandText = SM.SqlDeleteCommandString(this);
                                if (database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(oleAdapter.DeleteCommand);
                            }
                            else if (database.Type == SMDatabaseType.MySql)
                            {
                                // Insert command
                                mySqlAdapter.InsertCommand.CommandText = SM.SqlInsertCommandString(this);
                                if (database.CommandTimeout > 0) mySqlAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(mySqlAdapter.InsertCommand);
                                // Update command
                                mySqlAdapter.UpdateCommand.CommandText = SM.SqlUpdateCommandString(this);
                                if (database.CommandTimeout > 0) mySqlAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(mySqlAdapter.UpdateCommand);
                                // Delete command
                                mySqlAdapter.DeleteCommand.CommandText = SM.SqlDeleteCommandString(this);
                                if (database.CommandTimeout > 0) mySqlAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(mySqlAdapter.DeleteCommand);
                            }
                            else
                            {
                                // Insert command
                                sqlAdapter.InsertCommand.CommandText = SM.SqlInsertCommandString(this);
                                if (database.CommandTimeout > 0) sqlAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(sqlAdapter.InsertCommand);
                                // Update command
                                sqlAdapter.UpdateCommand.CommandText = SM.SqlUpdateCommandString(this);
                                if (database.CommandTimeout > 0) sqlAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(sqlAdapter.UpdateCommand);
                                // Delete command
                                sqlAdapter.DeleteCommand.CommandText = SM.SqlDeleteCommandString(this);
                                if (database.CommandTimeout > 0) sqlAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                SM.SqlParametersByName(sqlAdapter.DeleteCommand);
                            }
                        }
                    }
                    ChangeState(SMDatasetState.Browse);
                    First();
                    //
                    LinkedDatasetOpen();
                    //
                    if (AfterOpen != null) AfterOpen(this);
                }
            }
            catch (Exception ex)
            {
                Close();
                SM.Error(ex);
            }
            //
            return State == SMDatasetState.Browse;
        }

        /// <summary>*TODO* Open dataset with query specified in sqlQuery parameter. 
        /// If readOnly is true dataset will be opened in read-only mode. 
        /// Return true if succeed.</summary>
        public bool Open(string _SQLSelectionQuery = "", bool _ReadOnly = false)
        {
            bool r = false;
            Close();
            if (OpenDatabase())
            {
                bool cancel = false;
                if (BeforeOpen != null) BeforeOpen(this, ref cancel);
                if (!cancel)
                {
                    if (SM.Empty(_SQLSelectionQuery)) _SQLSelectionQuery = Query;
                    adaptedQuery = SM.SqlDelimiters(SM.SqlMacros(_SQLSelectionQuery, database.Type), database.Type);
                    if (_ReadOnly) readOnly = true;
                    else readOnly = SM.Btw(adaptedQuery.ToLower(), " from ", " on ").IndexOf("join") > -1;
                    try
                    {
                        Dataset = new DataSet();
                        try
                        {
                            if (database.Type == SMDatabaseType.Mdb)
                            {
                                oleAdapter = new OleDbDataAdapter(adaptedQuery, database.OleDB);
                                oleBuilder = new OleDbCommandBuilder(oleAdapter);
                                oleBuilder.QuotePrefix = SM.DatabaseSqlPrefix;
                                oleBuilder.QuoteSuffix = SM.DatabaseSqlSuffix;
                                oleAdapter.FillSchema(dataset, SchemaType.Source);
                                if (!readOnly)
                                {
                                    oleAdapter.InsertCommand = oleBuilder.GetInsertCommand();
                                    if (database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                    oleAdapter.UpdateCommand = oleBuilder.GetUpdateCommand();
                                    if (database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                    oleAdapter.DeleteCommand = oleBuilder.GetDeleteCommand();
                                    if (database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                }
                            }
                            else if (database.Type == SMDatabaseType.DBase4)
                            {
                                oleAdapter = new OleDbDataAdapter(adaptedQuery, database.OleDB);
                                oleBuilder = new OleDbCommandBuilder(oleAdapter);
                                oleBuilder.QuotePrefix = SM.DatabaseSqlPrefix;
                                oleBuilder.QuoteSuffix = SM.DatabaseSqlSuffix;
                                oleAdapter.FillSchema(dataset, SchemaType.Source);
                                if (!readOnly)
                                {
                                    oleAdapter.InsertCommand = oleBuilder.GetInsertCommand();
                                    if (database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                    oleAdapter.UpdateCommand = oleBuilder.GetUpdateCommand();
                                    if (database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                    oleAdapter.DeleteCommand = oleBuilder.GetDeleteCommand();
                                    if (database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                }
                            }
                            else if (database.Type == SMDatabaseType.MySql)
                            {
                                mySqlAdapter = new MySqlDataAdapter(adaptedQuery, database.MySqlDB);
                                mySqlBuilder = new MySqlCommandBuilder(mySqlAdapter);
                                mySqlBuilder.QuotePrefix = SM.DatabaseMySqlPrefix;
                                mySqlBuilder.QuoteSuffix = SM.DatabaseMySqlSuffix;
                                mySqlAdapter.FillSchema(dataset, SchemaType.Source);
                                if (!readOnly)
                                {
                                    mySqlAdapter.InsertCommand = mySqlBuilder.GetInsertCommand();
                                    if (database.CommandTimeout > 0) mySqlAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                    mySqlAdapter.UpdateCommand = mySqlBuilder.GetUpdateCommand();
                                    if (database.CommandTimeout > 0) mySqlAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                    mySqlAdapter.DeleteCommand = mySqlBuilder.GetDeleteCommand();
                                    if (database.CommandTimeout > 0) mySqlAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                }
                            }
                            else
                            {
                                sqlAdapter = new SqlDataAdapter(adaptedQuery, database.SqlDB);
                                sqlBuilder = new SqlCommandBuilder(sqlAdapter);
                                sqlBuilder.QuotePrefix = SM.DatabaseSqlPrefix;
                                sqlBuilder.QuoteSuffix = SM.DatabaseSqlSuffix;
                                sqlAdapter.FillSchema(dataset, SchemaType.Source);
                                if (!readOnly)
                                {
                                    sqlAdapter.InsertCommand = sqlBuilder.GetInsertCommand();
                                    if (database.CommandTimeout > 0) sqlAdapter.InsertCommand.CommandTimeout = database.CommandTimeout;
                                    sqlAdapter.UpdateCommand = sqlBuilder.GetUpdateCommand();
                                    if (database.CommandTimeout > 0) sqlAdapter.UpdateCommand.CommandTimeout = database.CommandTimeout;
                                    sqlAdapter.DeleteCommand = sqlBuilder.GetDeleteCommand();
                                    if (database.CommandTimeout > 0) sqlAdapter.DeleteCommand.CommandTimeout = database.CommandTimeout;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SM.Error(ex.Message + " on query: " + adaptedQuery, ex);
                        }
                        Load();
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex.Message + " on query: " + adaptedQuery, ex);
                    }
                    r = state != SMDatasetState.Closed;
                }
                else
                {
                    SM.Raise("SMDataset: dataset open cancelled.", false);
                    r = false;
                }
            }
            return r;
        }

        /// <summary>Open dataset with query specified in sqlQuery parameter. Return true if succeed
        /// and if the table contains at least one record.</summary>
        public bool OpenAtLeast(string _SQLSelectionQuery)
        {
            if (Open(_SQLSelectionQuery))
            {
                if (Eof)
                {
                    Close();
                    return false;
                }
                else return true;
            }
            else return true;
        }

        /// <summary>Keep dataset database connection with database Alias property 
        /// or Database property. Return true if succeed.</summary>
        public bool OpenDatabase()
        {
            bool r = false;
            if (alias.Trim().Length > 0) database = SM.Databases.Keep(alias);
            else if (database != null) database.Keep();
            if (database == null) SM.Raise("SMDatabase: null database error.", false);
            else
            {
                r = database.Active;
                if (!r)
                {
                    SM.DoEvents();
                    r = database.Open();
                }
                if (!r) SM.Raise("SMDatabase: error opening database.", false);
            }
            return r;
        }

        #endregion

        /* */

        #region Methods - Fields

        /*  ===================================================================
         *  Methods - Fields
         *  ===================================================================
         */

        /// <summary>Return object related to field of current active record.</summary>
        public object Field(string _FieldName)
        {
            int i;
            try
            {
                if (State == SMDatasetState.Read)
                {
                    if (database.Type == SMDatabaseType.Mdb)
                    {
                        i = oleReader.GetOrdinal(_FieldName);
                        if (i > -1) return oleReader[i];
                        else return null;
                    }
                    else if (database.Type == SMDatabaseType.Dbf)
                    {
                        i = oleReader.GetOrdinal(_FieldName);
                        if (i > -1) return oleReader[i];
                        else return null;
                    }
                    else if (database.Type == SMDatabaseType.MySql)
                    {
                        i = mySqlReader.GetOrdinal(_FieldName);
                        if (i > -1) return mySqlReader[i];
                        else return null;
                    }
                    else
                    {
                        i = sqlReader.GetOrdinal(_FieldName);
                        if (i > -1) return sqlReader[i];
                        else return null;
                    }
                }
                else if (Row != null)
                {
                    if ((Row.RowState != DataRowState.Deleted) && (Row.RowState != DataRowState.Detached)) return Row[_FieldName];
                    else return null;
                }
                else return null;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return null;
            }
        }

        /// <summary>Return char related to field value of current active record.</summary>
        public char FieldChar(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return (o.ToString().Trim()+" ")[0];
            else return ' ';
        }

        /// <summary>Return string related to field value of current active record.</summary>
        public string FieldStr(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return o.ToString();
            else return "";
        }

        /// <summary>Return string related to field value of current active record
        /// and formatted with format specifications.</summary>
        public string FieldStr(string _FieldName, string _FormatString)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToStr(o.ToString(), _FormatString);
            else return "";
        }

        /// <summary>Return integer related to field value of current active record.</summary>
        public int FieldInt(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToInt(o.ToString());
            else return 0;
        }

        /// <summary>Return long integer related to field value of current active record.</summary>
        public long FieldLong(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToLong(o.ToString());
            else return 0;
        }

        /// <summary>Return double related to field value of current active record.</summary>
        public double FieldDouble(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToDouble(o.ToString());
            else return 0.0d;
        }

        /// <summary>Return date related to field value of current active record.</summary>
        public DateTime FieldDate(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToDate(o.ToString(), SM.DateFormat, false);
            else return DateTime.MinValue;
        }

        /// <summary>Return datetime related to field value of current active record.</summary>
        public DateTime FieldDateTime(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToDate(o.ToString(), SM.DateFormat, true);
            else return DateTime.MinValue;
        }

        /// <summary>Return time related to field value of current active record.</summary>
        public DateTime FieldTime(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToTime(o.ToString());
            else return DateTime.MinValue;
        }

        /// <summary>Return bool related to field value of current active record.</summary>
        public bool FieldBool(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != null) 
            {
                if (o is bool) return (bool)o;
                else return SM.ToBool(o.ToString());
            }
            else return false;
        }

        /// <summary>Return byte array with blob content of field of current active record.</summary>
        public byte[] FieldBlob(string _FieldName)
        {
            object o = Field(_FieldName);
            if (o != DBNull.Value) return (byte[])Field(_FieldName);
            else return null;
        }

        /// <summary>Load blob content of field from file. Return blob size or -1 if fail.</summary>
        public int FieldLoad(string _FieldName, string _FileName)
        {
            int r = -1;
            byte[] b;
            try
            {
                if (SM.FileExists(_FileName))
                {
                    b = SM.FileLoad(_FileName);
                    if (Assign(_FieldName, b))
                    {
                        if (b != null) r = b.Length;
                        else r = 0;
                    }
                }
                else r = -1;
            }
            catch (Exception ex)
            {
                r = -1;
                SM.Error(ex);
            }
            return r;
        }

        /// <summary>Save blob content of field which name is fieldName of current active record 
        /// in to file fileName. Return blob size or -1 if fail.</summary>
        public int FieldSave(string _FieldName, string _FileName)
        {
            int r = -1;
            byte[] b;
            object o;
            FileStream fs;
            BinaryWriter bw;
            try
            {
                o = Field(_FieldName);
                if (o != DBNull.Value)
                {
                    b = (byte[])Field(_FieldName);
                    fs = new FileStream(_FileName, System.IO.FileMode.Create);
                    bw = new BinaryWriter(fs);
                    try 
                    { 
                        bw.Write(b);
                        r = b.Length;
                    }
                    catch (Exception ex)
                    {
                        r = -1;
                        SM.Error(ex);
                    }
                    bw.Close();
                    fs.Close();
                    fs.Dispose();
                }
                else r = 0;
            }
            catch (Exception ex)
            {
                r = -1;
                SM.Error(ex);
            }
            return r;
        }

        #endregion

        /* */

        #region Methods - Browsing

        /*  ===================================================================
         *  Methods - Browsing
         *  ===================================================================
         */

        /// <summary>Return true if current record is deleted or detached.</summary>
        public bool DataReady()
        {
            if ((State != SMDatasetState.Closed) && (Row != null))
            {
                return (Row.RowState != DataRowState.Deleted) && (Row.RowState != DataRowState.Detached);
            }
            else return false;
        }

        /// <summary>Return true if current record is deleted or detached.</summary>
        public bool Deleted()
        {
            if (Row != null)
            {
                return (Row.RowState == DataRowState.Deleted) || (Row.RowState == DataRowState.Detached);
            }
            else return true;
        }

        /// <summary>Moves to the first record in the dataset. Return true if succeed.</summary>
        public bool First()
        {
            int oldRecordIndex = recordIndex;
            recordIndex = -1;
            Row = null;
            Bof = true;
            Eof = true;
            if (Table != null)
            {
                if (Table.Rows != null)
                {
                    if (Table.Rows.Count > 0)
                    {
                        recordIndex = 0;
                        Row = Table.Rows[recordIndex];
                        Eof = false;
                        Bof = !SkipDeletedForward();
                    }
                }
            }
            if ((RecordChange != null) && (recordIndex != oldRecordIndex)) RecordChange(this);
            return !Eof && (recordIndex > -1);
        }

        /// <summary>Moves to the record with recordIndex position in the dataset. Return true if succeed.</summary>
        public bool Goto(int _RecordIndex)
        {
            int oldRecordIndex = recordIndex;
            if (Table != null)
            {
                if (Table.Rows != null)
                {
                    if ((_RecordIndex > -1) && (_RecordIndex < Table.Rows.Count))
                    {
                        recordIndex = _RecordIndex;
                        Row = Table.Rows[recordIndex];
                        Bof = false;
                        Eof = false;
                        SkipDeletedForward();
                    }
                }
            }
            if ((RecordChange != null) && (recordIndex != oldRecordIndex)) RecordChange(this);
            return !Eof && (recordIndex == _RecordIndex);
        }

        /// <summary>Moves to the last record in the dataset. Return true if succeed.</summary>
        public bool Last()
        {
            int oldRecordIndex = recordIndex;
            recordIndex = -1;
            Row = null;
            Bof = true;
            Eof = true;
            if (Table != null)
            {
                if (Table.Rows != null)
                {
                    if (Table.Rows.Count > 0)
                    {
                        recordIndex = Table.Rows.Count - 1;
                        Row = Table.Rows[recordIndex];
                        Bof = false;
                        Eof = false;
                        SkipDeletedBackward();
                    }
                }
            }
            if ((RecordChange != null) && (recordIndex != oldRecordIndex)) RecordChange(this);
            return !Bof && (recordIndex > -1);
        }

        /// <summary>Moves to the next record in the dataset. Return true if succeed.</summary>
        public bool Next()
        {
            int oldRecordIndex = recordIndex;
            if (State == SMDatasetState.Read) Read();
            else if (Table != null)
            {
                if (Table.Rows != null)
                {
                    recordIndex++;
                    if (recordIndex < Table.Rows.Count)
                    {
                        Row = Table.Rows[recordIndex];
                        Bof = false;
                        Eof = false;
                        SkipDeletedForward();
                    }
                    else Eof = true;
                }
            }
            if ((RecordChange != null) && (recordIndex != oldRecordIndex)) RecordChange(this);
            return !Eof && (recordIndex > oldRecordIndex);
        }

        /// <summary>Moves to the previous record in the dataset. Return true if succeed.</summary>
        public bool Previous()
        {
            int oldRecordIndex = recordIndex;
            if ((State != SMDatasetState.Read) && (Table != null))
            {
                if (Table.Rows != null)
                {
                    if (recordIndex > 0)
                    {
                        recordIndex--;
                        if (recordIndex < Table.Rows.Count)
                        {
                            Row = Table.Rows[recordIndex];
                            Bof = false;
                            Eof = false;
                            SkipDeletedBackward();
                        }
                        else Eof = true;
                    }
                    else Bof = true;
                }
            }
            if ((RecordChange != null) && (recordIndex != oldRecordIndex)) RecordChange(this);
            return !Eof && (recordIndex < oldRecordIndex);
        }

        /// <summary>*TODO* Start a readonly session with sqlQuery reading first record 
        /// on current database connection. Returns true if succeed.</summary>
        public bool Read(string _SqlQuery)
        {
            bool retValue = false;
            Close();
            try
            {
                if (database != null)
                {
                    if (database.Keep())
                    {
                        if (database.Type == SMDatabaseType.Mdb)
                        {
                            oleCommand = new OleDbCommand(SM.SqlDelimiters(SM.SqlMacros(_SqlQuery, database.Type), database.Type), database.OleDB);
                            oleReader = oleCommand.ExecuteReader();
                        }
                        else if (database.Type == SMDatabaseType.DBase4)
                        {
                            oleCommand = new OleDbCommand(SM.SqlDelimiters(SM.SqlMacros(_SqlQuery, database.Type), database.Type), database.OleDB);
                            oleReader = oleCommand.ExecuteReader();
                        }
                        else if (database.Type == SMDatabaseType.MySql)
                        {
                            mySqlCommand = new MySqlCommand(SM.SqlDelimiters(SM.SqlMacros(_SqlQuery, database.Type), database.Type), database.MySqlDB);
                            mySqlReader = mySqlCommand.ExecuteReader();
                        }
                        else
                        {
                            sqlCommand = new SqlCommand(SM.SqlDelimiters(SM.SqlMacros(_SqlQuery, database.Type), database.Type), database.SqlDB);
                            sqlReader = sqlCommand.ExecuteReader();
                        }
                        retValue = Read();
                        if (retValue)
                        {
                            ChangeState(SMDatasetState.Read);
                            eof = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            return retValue;
        }

        /// <summary>Continue readonly session with sqlQuery reading next record 
        /// on current database connection. Returns true if succeed.</summary>
        public bool Read()
        {
            bool retValue = false;
            try
            {
                if (database != null)
                {
                    if (database.Keep())
                    {
                        if (database.Type == SMDatabaseType.Mdb) retValue = oleReader.Read();
                        else if (database.Type == SMDatabaseType.Dbf) retValue = oleReader.Read();
                        else if (database.Type == SMDatabaseType.MySql) retValue = mySqlReader.Read();
                        else retValue = sqlReader.Read();
                        if (retValue) Bof = false;
                        else Eof = true;
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            return retValue;
        }

        /// <summary>Indicates the total number of records associated with the dataset.</summary>
        public int RecordCount()
        {
            if (Table != null)
            {
                if (Table.Rows != null) return Table.Rows.Count;
                else return -1;
            }
            return -1;
        }

        /// <summary>Returns a JSON string representing values of current record fields. 
        /// If blobs is true, blob fields will be stored with base 64 encoding.</summary>
        public string RecordToJSON(bool _IncludeBlobs)
        {
            int i;
            string c;
            SMDictionary dict = new SMDictionary(SM);
            if (Row != null)
            {
                for (i=0; i < Table.Columns.Count; i++)
                {
                    c = Table.Columns[i].ColumnName;
                    if (Table.Columns[i].DataType == System.Type.GetType("System.Byte[]"))
                    {
                        if (_IncludeBlobs) dict.Add(c, SM.Base64EncodeBytes(FieldBlob(c)));
                    }
                    else dict.Add(c, FieldStr(c));
                }
            }
            return dict.ToJSON();
        }

        /// <summary>Returns a JSON string representing values of current record fields
        /// specified in array. Blob fields will be stored with base 64 encoding.</summary>
        public string RecordToJSON(string[] _FieldNames = null)
        {
            int i, j;
            string c;
            SMDictionary dict = new SMDictionary(SM);
            if ((_FieldNames != null) && (Row != null))
            {
                for (i = 0; i < _FieldNames.Length; i++)
                {
                    j = Table.Columns.IndexOf(_FieldNames[i]);
                    if (j > -1)
                    {
                        c = Table.Columns[j].ColumnName;
                        if (Table.Columns[j].DataType == System.Type.GetType("System.Byte[]"))
                        {
                            dict.Add(c, SM.Base64EncodeBytes(FieldBlob(c)));
                        }
                        else dict.Add(c, FieldStr(c));
                    }
                    i++;
                }
            }
            return dict.ToJSON();
        }

        /// <summary>Skip backward all deleted or detached records from the current index. Return true if not BOF.</summary>
        public bool SkipDeletedBackward()
        {
            if (Row != null)
            {
                while (!Bof && ((Row.RowState == DataRowState.Deleted) || (Row.RowState == DataRowState.Detached)))
                {
                    if (recordIndex > 0)
                    {
                        recordIndex--;
                        Row = Table.Rows[recordIndex];
                    }
                    else
                    {
                        Bof = true;
                        Row = null;
                    }
                }
            }
            return !Bof;
        }

        /// <summary>Skip forward all deleted or detached records from the current index. Return true if not EOF.</summary>
        public bool SkipDeletedForward()
        {
            if (Row != null)
            {
                while (!Eof && ((Row.RowState == DataRowState.Deleted) || (Row.RowState == DataRowState.Detached)))
                {
                    if (recordIndex < Table.Rows.Count - 1)
                    {
                        recordIndex++;
                        Row = Table.Rows[recordIndex];
                    }
                    else
                    {
                        Eof = true;
                        Row = null;
                    }
                }
            }
            return !Eof;
        }

        #endregion

        /* */

        #region Methods - Assigning

        /*********************************************************************
           Methods - Assigning
         *********************************************************************/

        /// <summary>Assign value content to field named column.</summary>
        public bool Assign(string _FieldName, string _Value)
        {
            return Assign(table.Columns.IndexOf(_FieldName), _Value);
        }

        /// <summary>Assign value content to field named column.</summary>
        public bool Assign(string _FieldName, object _Value)
        {
            return Assign(table.Columns.IndexOf(_FieldName), _Value);
        }

        /// <summary>Assign value content to field with column index.</summary>
        public bool Assign(int _ColumnIndex, object _Value)
        {
            DataColumn co;
            if ((row != null) && (_ColumnIndex > -1))
            {
                try
                {
                    co = table.Columns[_ColumnIndex];
                    if (co.AutoIncrement) row[_ColumnIndex] = 0;
                    else if (co.DataType == System.Type.GetType("System.Boolean"))
                    {
                        if (_Value == null) row[_ColumnIndex] = false;
                        else row[_ColumnIndex] = "1TVStvs".IndexOf((_Value.ToString().Trim() + " ")[0]) > -1;
                    }
                    else if ((co.DataType == System.Type.GetType("System.Byte"))
                        || (co.DataType == System.Type.GetType("System.SByte")))
                    {
                        if (_Value == null) row[_ColumnIndex] = 0;
                        else row[_ColumnIndex] = Convert.ToByte(SM.StrToInt(_Value.ToString()) % 256);
                    }
                    else if (co.DataType == System.Type.GetType("System.Char"))
                    {
                        if (_Value == null) row[_ColumnIndex] = '\0';
                        else row[_ColumnIndex] = (_Value.ToString() + " ")[0];
                    }
                    else if (co.DataType == System.Type.GetType("System.DateTime"))
                    {
                        if (_Value == null) row[_ColumnIndex] = DBNull.Value;
                        else if (_Value is DateTime) 
                        {
                            if (SM.DateMin((DateTime)_Value)) row[_ColumnIndex] = DBNull.Value;
                            else row[_ColumnIndex] = (DateTime)_Value;
                        }
                        else if (_Value.ToString().Trim().Length > 0) row[_ColumnIndex] = SM.CToDT(_Value.ToString());
                        else row[_ColumnIndex] = DBNull.Value;
                    }
                    else if ((co.DataType == System.Type.GetType("System.Decimal"))
                        || (co.DataType == System.Type.GetType("System.Double"))
                        || (co.DataType == System.Type.GetType("System.Single")))
                    {
                        if (_Value == null) row[_ColumnIndex] = 0.0;
                        else row[_ColumnIndex] = SM.StrVal(_Value.ToString());
                    }
                    else if ((co.DataType == System.Type.GetType("System.Int32"))
                        || (co.DataType == System.Type.GetType("System.Int16"))
                        || (co.DataType == System.Type.GetType("System.UInt32"))
                        || (co.DataType == System.Type.GetType("System.UInt16")))
                    {
                        if (_Value == null) row[_ColumnIndex] = 0;
                        else row[_ColumnIndex] = SM.StrToInt(_Value.ToString());
                    }
                    else if ((co.DataType == System.Type.GetType("System.Int64"))
                        || (co.DataType == System.Type.GetType("System.UInt64")))
                    {
                        if (_Value == null) row[_ColumnIndex] = 0;
                        else row[_ColumnIndex] = SM.StrToLong(_Value.ToString());
                    }
                    else if (co.DataType == System.Type.GetType("System.TimeSpan"))
                    {
                        if (_Value == null) row[_ColumnIndex] = DBNull.Value;
                        else if (_Value.ToString().Trim().Length > 0) row[_ColumnIndex] = new TimeSpan(SM.CToDT(_Value.ToString()).Ticks);
                        else row[_ColumnIndex] = DBNull.Value;
                    }
                    else if (co.DataType == System.Type.GetType("System.Byte[]"))
                    {
                        if (_Value == null) row[_ColumnIndex] = DBNull.Value;
                        else row[_ColumnIndex] = _Value;
                    }
                    else if (_Value == null) row[_ColumnIndex] = "";
                    else if (co.MaxLength > 0) row[_ColumnIndex] = SM.Mid(_Value.ToString(), 0, co.MaxLength);
                    else row[_ColumnIndex] = _Value;
                    if (dsAutoBind) ReadBindings(co.ColumnName);
                    return true;
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    return false;
                }
            }
            else
            {
                SM.Raise("Unknown field or invalid field index", false);
                return false;
            }
        }

        /// <summary>Assign blank content to field named column.</summary>
        public bool Assign(string _FieldName)
        {
            int i = table.Columns.IndexOf(_FieldName);
            if (i > -1) return Assign(i, SM.Blank(table.Columns[i]));
            else return false;
        }

        /// <summary>Assign fields from source dataset. fields is an array with field 
        /// names couple "target field name","source field name". Returns true if succeed.</summary>
        public bool Assign(SMDataSet _SourceDataSet, string[] _FieldsList)
        {
            bool r = true;
            int i = 0;
            while (r && (i < _FieldsList.Length - 2))
            {
                r = Assign(_FieldsList[i], Field(_FieldsList[i + 1]));
                i += 2;
            }
            return r;
        }

        /// <summary>Assign image img content to field named column with format.</summary>
        public bool Assign(string _FieldName, Image _Image, System.Drawing.Imaging.ImageFormat _ImageFormat)
        {
            bool r = false;
            Bitmap bmp;
            MemoryStream ms;
            try
            {
                if (_Image != null)
                {
                    bmp = new Bitmap(_Image);
                    ms = new MemoryStream();
                    try
                    {
                        bmp.Save(ms, _ImageFormat);
                        r = Assign(_FieldName, ms.ToArray());
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex);
                        r = false;
                    }
                    bmp.Dispose();
                    ms.Close();
                    ms.Dispose();
                }
                else r = Assign(_FieldName, null);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = false;
            }
            return r;
        }

        /// <summary>Assign new unique ID to record, matching type.</summary>
        public bool AssignNewID()
        {
            if (dsUniqueID == SMUniqueIDType.Standard) return Assign("ID", SM.UniqueID());
            else if (dsUniqueID == SMUniqueIDType.Extended) return Assign("ID", SM.UniqueXID());
            else return false;
        }

        #endregion

        /* */

        #region Methods - Searching

        /*  --------------------------------------------------------------------
         *  Methods - Searching
         *  --------------------------------------------------------------------
         */

        /// <summary>Return index of row with keys field equal to values, that must 
        /// be FieldSort() formatted, or -1 if not found. Binary if true enable 
        /// binary search for keys sequence ordered recordset.</summary>
        public int Find(string[] _KeyFields, string[] _Values, bool _BinarySearch)
        {
            bool q;
            string s;
            int[] map;
            int r = -1, k = _KeyFields.Length, i = 0, j, a, b, c;
            if ((_KeyFields != null) && (_Values != null) && (table.Rows.Count > 0))
            {
                if (k > _Values.Length) k = _Values.Length;
                map = new int[k];
                for (i = 0; i < k; i++) map[i] = table.Columns.IndexOf(_KeyFields[i]);
                //
                // binary search
                //
                if (_BinarySearch)
                {
                    a = 0;
                    b = table.Rows.Count - 1;
                    //
                    // search loop
                    //
                    while ((r < 0) && (a <= b))
                    {
                        //
                        // test first element
                        //
                        i = 0;
                        j = 0;
                        while ((i == 0) && (j < k))
                        {
                            s = SM.FieldSort(table.Columns[map[j]], table.Rows[a][map[j]]);
                            i = String.Compare(s, _Values[j], !table.CaseSensitive);
                            j++;
                        }
                        if (i == 0) r = a;
                        //
                        // test last element
                        //
                        if (r < 0)
                        {
                            i = 0;
                            j = 0;
                            while ((i == 0) && (j < k))
                            {
                                s = SM.FieldSort(table.Columns[map[j]], table.Rows[b][map[j]]);
                                i = String.Compare(s, _Values[j], !table.CaseSensitive);
                                j++;
                            }
                            if (i == 0) r = b;
                        }
                        //
                        // test middle element
                        //
                        if (r < 0)
                        {
                            c = (a + b) / 2;
                            i = 0;
                            j = 0;
                            while ((i == 0) && (j < k))
                            {
                                s = SM.FieldSort(table.Columns[map[j]], table.Rows[c][map[j]]);
                                i = String.Compare(s, _Values[j], !table.CaseSensitive);
                                j++;
                            }
                            if (i > 0) b = c - 1;
                            else if (i < 0) a = c + 1;
                            else r = c;
                        }
                    }
                }
                //
                // sequential search
                //
                else
                {
                    b = table.Rows.Count;
                    while ((i < b) && (r < 0))
                    {
                        q = true;
                        j = 0;
                        while (q && (j < k))
                        {
                            if (_Values[j] != SM.FieldSort(table.Columns[map[j]], table.Rows[i][map[j]])) q = false;
                            else j++;
                        }
                        if (q) r = i;
                        else i++;
                    }
                }
            }
            return r;
        }

        /// <summary>Return index of row with key fields corresponding to map indexes field 
        /// equal to values, that must be FieldSort() formatted, or -1 if not found. 
        /// Binary if true enable binary search for keys sequence ordered recordset.</summary>
        public int Find(int[] _KeyFieldsIndexes, string[] _Values, bool _BinarySearch)
        {
            bool q;
            string s;
            int r = -1, k = _KeyFieldsIndexes.Length, i = 0, j, a, b, c;
            if (table.Rows.Count > 0)
            {
                if (k > _Values.Length) k = _Values.Length;
                //
                // binary search
                //
                if (_BinarySearch)
                {
                    a = 0;
                    b = table.Rows.Count - 1;
                    //
                    // search loop
                    //
                    while ((r < 0) && (a <= b))
                    {
                        //
                        // test first element
                        //
                        i = 0;
                        j = 0;
                        while ((i == 0) && (j < k))
                        {
                            s = SM.FieldSort(table.Columns[_KeyFieldsIndexes[j]], table.Rows[a][_KeyFieldsIndexes[j]]);
                            i = String.Compare(s, _Values[j], !table.CaseSensitive);
                            j++;
                        }
                        if (i == 0) r = a;
                        //
                        // test last element
                        //
                        if (r < 0)
                        {
                            i = 0;
                            j = 0;
                            while ((i == 0) && (j < k))
                            {
                                s = SM.FieldSort(table.Columns[_KeyFieldsIndexes[j]], table.Rows[b][_KeyFieldsIndexes[j]]);
                                i = String.Compare(s, _Values[j], !table.CaseSensitive);
                                j++;
                            }
                            if (i == 0) r = b;
                        }
                        //
                        // test middle element
                        //
                        if (r < 0)
                        {
                            c = (a + b) / 2;
                            i = 0;
                            j = 0;
                            while ((i == 0) && (j < k))
                            {
                                s = SM.FieldSort(table.Columns[_KeyFieldsIndexes[j]], table.Rows[c][_KeyFieldsIndexes[j]]);
                                i = String.Compare(s, _Values[j], !table.CaseSensitive);
                                j++;
                            }
                            if (i > 0) b = c - 1;
                            else if (i < 0) a = c + 1;
                            else r = c;
                        }
                    }
                }
                //
                // sequential search
                //
                else
                {
                    b = table.Rows.Count;
                    while ((i < b) && (r < 0))
                    {
                        q = true; j = 0;
                        while (q && (j < k))
                        {
                            if (_Values[j] != SM.FieldSort(table.Columns[_KeyFieldsIndexes[j]], table.Rows[i][_KeyFieldsIndexes[j]])) q = false;
                            else j++;
                        }
                        if (q) r = i;
                        else i++;
                    }
                }
            }
            return r;
        }

        /// <summary>Return value greater than zero if opened dataset table contains another record 
        /// except current with same value of tested field. A value lesser than zero indicate error.</summary>
        public int FindDouble(string _UniqueFieldName)
        {
            int r = -1, i;
            string value;
            SMDataSet ds;
            if (this.Active)
            {
                if (this.UniqueID != SMUniqueIDType.None)
                {
                    try
                    {
                        i = this.Table.Columns.IndexOf(_UniqueFieldName);
                        if (i > -1)
                        {
                            if (SM.FieldIsDate(this.Table.Columns[i])) value = SM.QuotedDate(this.FieldDateTime(_UniqueFieldName), this.Database.Type);
                            else if (SM.FieldIsNumeric(this.Table.Columns[i])) value = this.FieldStr(_UniqueFieldName);
                            else if (SM.FieldIsBoolean(this.Table.Columns[i])) value = SM.Iif(this.FieldBool(_UniqueFieldName), "TRUE", "FALSE");
                            else value = SM.QuotedStr(this.FieldStr(_UniqueFieldName));
                            ds = new SMDataSet(this.Database);
                            if (ds.OpenAtLeast("SELECT [ID],[" + _UniqueFieldName + "] FROM [" + this.TableName
                                + "] WHERE ([" + _UniqueFieldName + "]=" + value + ")AND([ID]<>" + SM.QuotedStr(this.FieldStr("ID")) + ")"))
                            {
                                r = ds.RecordCount();
                                ds.Close();
                            }
                            ds.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex);
                        r = -1;
                    }
                }
            }
            return r;
        }

        /// <summary>Seek and go to row index with keys field equal to values, that must 
        /// be FieldSort() formatted, or -1 if not found or cannot go to row index. Binary if true enable 
        /// binary search for keys sequence ordered recordset.</summary>
        public int Seek(string[] _KeyFields, string[] _Values, bool _BinarySearch)
        {
            int r = Find(_KeyFields, _Values, _BinarySearch);
            if (r>-1)
            {
                if (!Goto(r)) r = -1;
            }
            return r;
        }

        /// <summary>Seek and go to row index with key fields corresponding to map indexes field 
        /// equal to values, that must be FieldSort() formatted, or -1 if not found or cannot go 
        /// to row index. Binary if true enable binary search for keys sequence ordered recordset.</summary>
        public int Seek(int[] _KeyFieldsIndexes, string[] _Values, bool _BinarySearch)
        {
            int r = Find(_KeyFieldsIndexes, _Values, _BinarySearch);
            if (r > -1)
            {
                if (!Goto(r)) r = -1;
            }
            return r;
        }

        #endregion 

        /* */

        #region Methods - Editing

        /*  --------------------------------------------------------------------
         *  Methods - Editing
         *  --------------------------------------------------------------------
         */

        /// <summary>Adds a new, empty record to the dataset. Return true if succeed.</summary>
        public bool Append()
        {
            int i;
            bool cancel = false;
            DataRow row;
            if (ReadOnly)
            {
                SM.Raise("SMDataSet: append cannot performed on readonly dataset.", false);
                return false;
            }
            else if (!Browsing)
            {
                SM.Raise("SMDataSet: append can performed only on browsing state dataset.", false);
                return false;
            }
            else
            {
                if (BeforeInsert != null) BeforeInsert(this, ref cancel);
                if (cancel)
                {
                    SM.Raise("SMDataSet: append operation cancelled.", false);
                    return false;
                }
                else
                {
                    try
                    {
                        row = Table.NewRow();
                        for (i = 0; i < Table.Columns.Count; i++)
                        {
                            if (!Table.Columns[i].AutoIncrement) row[i] = SM.Blank(Table.Columns[i]);
                        }
                        Table.Rows.Add(row);
                        recordIndex = Table.Rows.Count - 1;
                        Row = Table.Rows[recordIndex];
                        Bof = false;
                        Eof = false;
                        ChangeState(SMDatasetState.Insert);
                        if (AfterInsert != null) AfterInsert(this);
                        if (RecordChange != null) RecordChange(this);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex);
                        return false;
                    }
                }
            }
        }

        /// <summary>Cancels not yet posted modifications to the active record. Return true if succeed.</summary>
        public bool Cancel()
        {
            bool r = false, abort=false;
            if (BeforeCancel != null) BeforeCancel(this, ref abort);
            if (abort) SM.Raise("SMDataSet: cancel operation aborted.", false);
            else if (readOnly) SM.Raise("SMDataSet: cancel cannot performed on readonly dataset.", false);
            else if (!Modifying(false)) SM.Raise("SMDataSet: cancel operation can performed only on editing or appending state dataset.", false);
            else
            {
                try 
                { 
                    table.RejectChanges();
                    r = true;
                }
                catch (Exception ex) 
                { 
                    SM.Error(ex); 
                }
                if (r)
                {
                    ChangeState(SMDatasetState.Browse);
                    Goto(recordIndex);
                    //
                    LinkedDatasetCancel();
                    //
                    if (AfterCancel != null) AfterCancel(this);
                }
            }
            return r;
        }

        /// <summary>Writes a dataset buffered updates to the database. Return true if succeed.</summary>
        public bool Commit()
        {
            try
            {
                if (dataset != null)
                {
                    if (dataset.HasChanges())
                    {
                        if (database.Type == SMDatabaseType.Access)
                        {
                            oleAdapter.InsertCommand.Connection = database.OleDB;
                            oleAdapter.UpdateCommand.Connection = database.OleDB;
                            oleAdapter.DeleteCommand.Connection = database.OleDB;
                            oleAdapter.Update(dataset);
                        }
                        else if (database.Type == SMDatabaseType.DBase4)
                        {
                            oleAdapter.InsertCommand.Connection = database.OleDB;
                            oleAdapter.UpdateCommand.Connection = database.OleDB;
                            oleAdapter.DeleteCommand.Connection = database.OleDB;
                            oleAdapter.Update(dataset);
                        }
                        else if (database.Type == SMDatabaseType.MySql)
                        {
                            mySqlAdapter.InsertCommand.Connection = database.MySqlDB;
                            mySqlAdapter.UpdateCommand.Connection = database.MySqlDB;
                            mySqlAdapter.DeleteCommand.Connection = database.MySqlDB;
                            mySqlAdapter.Update(dataset);
                        }
                        else
                        {
                            sqlAdapter.InsertCommand.Connection = database.SqlDB;
                            sqlAdapter.UpdateCommand.Connection = database.SqlDB;
                            sqlAdapter.DeleteCommand.Connection = database.SqlDB;
                            sqlAdapter.Update(dataset);
                        }
                    }
                    table.AcceptChanges();
                    bufferCount = 0;
                    if (this.dsUniqueID != SMUniqueIDType.None)
                    {
                        if (database.Type == SMDatabaseType.Access) oleAdapter.Fill(dataset);
                        else if (database.Type == SMDatabaseType.DBase4) oleAdapter.Fill(dataset);
                        else if (database.Type == SMDatabaseType.MySql) mySqlAdapter.Fill(dataset);
                        else sqlAdapter.Fill(dataset);
                        table = dataset.Tables[0];
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex) 
            { 
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>If dataset updates buffers is full, writes changes to database. Return true if succeed.</summary>
        public bool Buffer()
        {
            bufferCount++;
            if (bufferCount < bufferSize) return true;
            else return Commit();
        }

        /// <summary>Copy the values of corresponding fields from source dataset current row 
        /// to this dataset current row. Returns true if succeed.</summary>
        public bool CopyRow(SMDataSet _SourceDataSet)
        {
            int i;
            bool r = false;
            if ((_SourceDataSet != null) && (row != null) && (table != null))
            {
                if (this.Modifying(false) && (_SourceDataSet.State != SMDatasetState.Closed))
                {
                    i = table.Columns.Count;
                    r = true;
                    while (r && (i > 0))
                    {
                        i--;
                        if (!table.Columns[i].AutoIncrement)
                        {
                            if (_SourceDataSet.IsField(table.Columns[i].ColumnName))
                            {
                                r = this.Assign(i, _SourceDataSet.Field(table.Columns[i].ColumnName));
                            }
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>Deletes the active record in the dataset. Return true if succeed.</summary>
        public bool Delete()
        {
            bool r = false, abort = false;
            if (BeforeDelete != null) BeforeDelete(this, ref abort);
            if (abort) SM.Raise("SMDataSet: delete operation aborted.", false);
            else if (readOnly) SM.Raise("SMDataSet: delete cannot performed on readonly dataset.", false);
            else if (!Browsing) SM.Raise("SMDataSet: delete can performed only on browsing state dataset.", false);
            else
            {
                dsLinkedOldValue = "";
                if (row != null)
                {
                    if ((extendedDataset != null) && (dsLinkedTable.Length > 0)) 
                    {
                        if (dsLinkedSourceField.Length > 0) dsLinkedOldValue = this.FieldStr(dsLinkedSourceField);
                    }
                    ChangeState(SMDatasetState.Delete);
                    row.Delete();
                    if (Buffer()) r = true;
                    else table.RejectChanges();
                    if ((recordIndex > -1) && (recordIndex < table.Rows.Count))
                    {
                        row = table.Rows[recordIndex];
                        bof = false;
                        eof = false;
                        r = SkipDeletedForward();
                    }
                    else
                    {
                        row = null;
                        eof = true;
                        bof = true;
                    }
                    ChangeState(SMDatasetState.Browse);
                    if (r)
                    {
                        LinkedDatasetDelete();
                        //
                        if (AfterDelete != null) AfterDelete(this);
                    }
                }
            }
            return r;
        }

        /// <summary>Enables data editing of dataset. Return true if succeed.</summary>
        public bool Edit()
        {
            bool r = false, abort = false;
            if (BeforeEdit != null) BeforeEdit(this, ref abort);
            if (abort) SM.Raise("SMDataSet: edit operation aborted.", false);
            else if (readOnly) SM.Raise("SMDataSet: edit cannot performed on readonly dataset.", false);
            else if (!Browsing) SM.Raise("SMDataSet: edit can performed only on browsing state dataset.", false);
            else
            {
                ChangeState(SMDatasetState.Edit);
                //
                LinkedDatasetEdit();
                //
                if (AfterEdit != null) AfterEdit(this);
                r = true;
            }
            return r;
        }

        /// <summary>Executes SQL statement passed as parameter. 
        /// Return the number of records affected or -1 if not succeed.</summary>
        public int Exec(string _SqlStatement)
        {
            int r = 0;
            Close();
            if (OpenDatabase())
            {
                _SqlStatement = SM.SqlDelimiters(SM.SqlMacros(_SqlStatement, database.Type), database.Type);
                try
                {
                    if (database.Type == SMDatabaseType.Access)
                    {
                        OleDbCommand cmd = new OleDbCommand(_SqlStatement, database.OleDB);
                        r = cmd.ExecuteNonQuery();
                    }
                    else if (database.Type == SMDatabaseType.DBase4)
                    {
                        OleDbCommand cmd = new OleDbCommand(_SqlStatement, database.OleDB);
                        r = cmd.ExecuteNonQuery();
                    }
                    else if (database.Type == SMDatabaseType.MySql)
                    {
                        MySqlCommand cmd = new MySqlCommand(_SqlStatement, database.MySqlDB);
                        r = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand(_SqlStatement, database.SqlDB);
                        r = cmd.ExecuteNonQuery();
                    }
                    if (r < 0) r = 0;
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    SM.ErrorMessage += SM.CR + SM.CR + "*** SQL STATEMENT ***" + SM.CR + _SqlStatement;
                    r = -1;
                }
            }
            else r = -1;
            return r;
        }

        /// <summary>Return true if exists record changes in the buffer.</summary>
        public bool Modified()
        {
            if (dataset != null) return dataset.HasChanges();
            else return false;
        }

        /// <summary>Return true if dataset state is in insert or edit mode. If force is true, 
        /// dataset AutoEdit property is setted to true and the dataset is not in edit or insert mode
        /// the function try to set the edit mode. Returns true if succeesd.</summary>
        public bool Modifying(bool _ForceEdit)
        {
            SMTitleBar tb;
            bool canEdit = true;
            if (dsAutoEdit)
            {
                tb = SM.TitleBarFind(dsBindingParent);
                if (tb != null)
                {
                    if (tb.FormRegister) canEdit = tb.UserAllows.CanEdit;
                }
            }
            if ((state == SMDatasetState.Insert) || (state == SMDatasetState.Edit)) return true;
            else if (_ForceEdit && canEdit && (state == SMDatasetState.Browse) && (row!=null)) return Edit();
            else return false;
        }

        /// <summary>Write a modified record to the buffer. Return true if succeed.</summary>
        public bool Post()
        {
            bool r = false, abort = false;
            if (BeforePost != null) BeforePost(this, ref abort);
            //
            if (!abort) abort = !LinkedDatasetPost();
            //
            if (abort) SM.Raise("SMDataSet: post operation aborted.", false);
            else if (readOnly) SM.Raise("SMDataSet: post cannot performed on readonly dataset.", false);
            else if (!Modifying(false)) SM.Raise("SMDataSet: post can performed only on editing or appending state dataset.", false);
            else
            {
                if (dsAutoBind) WriteBindings();
                try
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        if (dsInfoInsert) row["sysInsert"] = DateTime.Now;
                        if (dsInfoDate) row["sysDate"] = DateTime.Now;
                        if (dsInfoUser)
                        {
                            if (SM.Users.Logged && (row["sysUser"].ToString().Trim().ToLower() != "factory"))
                            {
                                row["sysUser"] = SM.Users.Current.User;
                            }
                        }
                    }
                    else if (row.RowState == DataRowState.Modified)
                    {
                        if (dsInfoDate) row["sysDate"] = DateTime.Now;
                        if (dsInfoUser)
                        {
                            if (SM.Users.Logged && (row["sysUser"].ToString().Trim().ToLower() != "factory"))
                            {
                                row["sysUser"] = SM.Users.Current.User;
                            }
                        }
                    }
                    r = Buffer();
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    r = false;
                }
            }
            if (r)
            {
                if ((recordIndex > -1) && (recordIndex < table.Rows.Count))
                {
                    row = table.Rows[recordIndex];
                    bof = false;
                    eof = false;
                    r = SkipDeletedForward();
                }
                else
                {
                    row = null;
                    eof = true;
                    bof = true;
                }
                ChangeState(SMDatasetState.Browse);
                if (r) if (AfterPost != null) AfterPost(this);
            }
            return r;
        }

        /// <summary>Store in the current record fields values contained on clipboard. 
        /// If blobs is true, blob fields will be stored in temporary directory and
        /// its names will be included on strings.</summary>
        public bool RecordFromClipboard(bool _IncludeBlobs)
        {
            return RecordFromString(Clipboard.GetText(), _IncludeBlobs);
        }

        /// <summary>Store in the current record fields values contained on s. 
        /// If blobs is true, blob fields will be stored in temporary directory and
        /// its names will be included on strings.</summary>
        public bool RecordFromString(string _TaggedString, bool _IncludeBlobs)
        {
            int i;
            bool r = false, b;
            string c, tmp;
            if (row != null)
            {
                if (Modifying(false))
                {
                    i = 0;
                    while (i < table.Columns.Count)
                    {
                        if (!table.Columns[i].AutoIncrement)
                        {
                            c = table.Columns[i].ColumnName;
                            b = table.Columns[i].DataType == System.Type.GetType("System.Byte[]");
                            if (b)
                            {
                                if (_IncludeBlobs)
                                {
                                    tmp = SM.TempPath(SM.TagGet(_TaggedString, c));
                                    if (SM.FileExists(tmp)) Assign(c, SM.FileLoad(tmp));
                                }
                            }
                            else Assign(c, SM.TagGet(_TaggedString, c));
                        }
                        i++;
                    }
                    r = true;
                }
            }
            return r;
        }

        /// <summary>Store in the current record fields values contained on sReturns a string representing values of current record fields. 
        /// If blobs is true, blob fields will be stored in temporary directory and
        /// its names will be included on strings.</summary>
        public bool RecordFromString(string _TaggedString, bool _IncludeBlobs, string[] _ExcludeFieldNames, bool _BackwardCompatibility)
        {
            bool r = false;
            if (row != null)
            {
                if (Modifying(false))
                {
                    int i = 0, j, h = 0;
                    bool b, q;
                    string c, tmp;
                    if (_ExcludeFieldNames != null) h = _ExcludeFieldNames.Length;
                    while (i < table.Columns.Count)
                    {
                        if (!table.Columns[i].AutoIncrement)
                        {
                            c = table.Columns[i].ColumnName;
                            q = table.Columns[i].DataType == System.Type.GetType("System.Byte[]");
                            b = true;
                            j = 0;
                            while (b && (j < h)) if (c == _ExcludeFieldNames[j]) b = false; else j++;
                            if (b)
                            {
                                if (q)
                                {
                                    if (_IncludeBlobs)
                                    {
                                        if (_BackwardCompatibility) tmp = SM.TempPath(SM.TagGetBackwardCompatible(_TaggedString, c));
                                        else tmp = SM.TempPath(SM.TagGet(_TaggedString, c));
                                        if (SM.FileExists(tmp)) Assign(c, SM.FileLoad(tmp));
                                    }
                                }
                                else if (_BackwardCompatibility) Assign(c, SM.TagGetBackwardCompatible(_TaggedString, c));
                                else Assign(c, SM.TagGet(_TaggedString, c));
                            }
                        }
                        i++;
                    }
                    r = true;
                }
            }
            return r;
        }

        #endregion

        /* */

        #region Methods - Bindings

        /*  --------------------------------------------------------------------
         *  Methods - Bindings
         *  --------------------------------------------------------------------
         */

        /// <summary>Get current form bindings controls and set states.</summary>
        public void GetBindings()
        {
            if (dsAutoBind && !dsBinding)
            {
                dsBinding = true;
                dsBindings.Clear();
                if (dsBindingParent == null) dsBindingParent = dsBindingForm;
                GetBindings(dsBindingParent, true);
                dsBinding = false;
            }
        }

        /// <summary>Get bindings controls and set states.</summary>
        public void GetBindings(Control _Control)
        {
            if (dsAutoBind && !dsBinding)
            {
                dsBinding = true;
                dsBindings.Clear();
                dsBindingParent = _Control;
                GetBindings(dsBindingParent, true);
                dsBinding = false;
            }
        }

        /// <summary>Get binding controls and set states, recursive if specified.</summary>
        public void GetBindings(Control _Control, bool _Recursive)
        {
            int i;
            bool b;
            if (_Control != null)
            {
                b = false;
                if (_Control is SMTextBox) b = ((SMTextBox)_Control).Dataset == this;
                else if (_Control is SMComboBox) b = ((SMComboBox)_Control).Dataset == this;
                else if (_Control is SMButton) b = ((SMButton)_Control).Dataset == this;
                else if (_Control is SMLabel) b = ((SMLabel)_Control).Dataset == this;
                else if (_Control is SMProgressBar) b = ((SMProgressBar)_Control).Dataset == this;
                else if (_Control is SMCheckBox) b = ((SMCheckBox)_Control).Dataset == this;
                else if (_Control is SMRadioButton) b = ((SMRadioButton)_Control).Dataset == this;
                else if (_Control is SMPictureBox) b = ((SMPictureBox)_Control).Dataset == this;
                else if (_Control is SMRating) b = ((SMRating)_Control).Dataset == this;
                else if (_Control is SMTrackBar) b = ((SMTrackBar)_Control).Dataset == this;
                else if (_Control is SMSlider) b = ((SMSlider)_Control).Dataset == this;
                else if (_Control is SMRichTextBox) b = ((SMRichTextBox)_Control).Dataset == this;
                else if (_Control is SMListView) b = ((SMListView)_Control).Dataset == this;
                else if (_Control is SMNumericUpDown) b = ((SMNumericUpDown)_Control).Dataset == this;
                if (b)
                {
                    dsBindings.Add(_Control);
                    if (_Control is SMTextBox) ((SMTextBox)_Control).SetState();
                    else if (_Control is SMComboBox) ((SMComboBox)_Control).SetState();
                    else if (_Control is SMButton) ((SMButton)_Control).SetState();
                    else if (_Control is SMCheckBox) ((SMCheckBox)_Control).SetState();
                    else if (_Control is SMRadioButton) ((SMRadioButton)_Control).SetState();
                    else if (_Control is SMRating) ((SMRating)_Control).SetState();
                    else if (_Control is SMTrackBar) ((SMTrackBar)_Control).SetState();
                    else if (_Control is SMSlider) ((SMSlider)_Control).SetState();
                    else if (_Control is SMRichTextBox) ((SMRichTextBox)_Control).SetState();
                    else if (_Control is SMNumericUpDown) ((SMNumericUpDown)_Control).SetState();
                }
                if (_Recursive)
                {
                    if (_Control is SMUserControlPanel)
                    {
                        for (i = 0; i < ((SMUserControlPanel)_Control).ControlsCount; i++)
                        {
                            GetBindings(((SMUserControlPanel)_Control).Items[i], true);
                        }
                    }
                    else
                    {
                        for (i = 0; i < _Control.Controls.Count; i++) GetBindings(_Control.Controls[i], true);
                    }
                }
            }
        }

        /// <summary>Read controls values from binding data.</summary>
        public void ReadBindings()
        {
            int i;
            if (!dsBinding)
            {
                dsBinding = true;
                for (i = 0; i < dsBindings.Count; i++) ReadBindings(dsBindings[i]);
                dsBinding = false;
            }
        }

        /// <summary>Read controls values with field from binding data.</summary>
        public void ReadBindings(string _FieldName)
        {
            int i;
            bool b;
            Control c;
            if (!dsBinding)
            {
                dsBinding = true;
                for (i = 0; i < dsBindings.Count; i++)
                {
                    b = false;
                    c = dsBindings[i];
                    if (c is SMTextBox) b = ((SMTextBox)c).FieldName == _FieldName;
                    else if (c is SMComboBox) b = ((SMComboBox)c).FieldName == _FieldName;
                    else if (c is SMButton) b = ((SMButton)c).FieldName == _FieldName;
                    else if (c is SMLabel) b = ((SMLabel)c).FieldName == _FieldName;
                    else if (c is SMProgressBar) b = ((SMProgressBar)c).FieldName == _FieldName;
                    else if (c is SMCheckBox) b = ((SMCheckBox)c).FieldName == _FieldName;
                    else if (c is SMRadioButton) b = ((SMRadioButton)c).FieldName == _FieldName;
                    else if (c is SMPictureBox) b = ((SMPictureBox)c).FieldName == _FieldName;
                    else if (c is SMRating) b = ((SMRating)c).FieldName == _FieldName;
                    else if (c is SMTrackBar) b = ((SMTrackBar)c).FieldName == _FieldName;
                    else if (c is SMSlider) b = ((SMSlider)c).FieldName == _FieldName;
                    else if (c is SMRichTextBox) b = ((SMRichTextBox)c).FieldName == _FieldName;
                    else if (c is SMListView) b = ((SMListView)c).FieldName == _FieldName;
                    else if (c is SMNumericUpDown) b = ((SMNumericUpDown)c).FieldName == _FieldName;
                    if (b) ReadBindings(c);
                }
                dsBinding = false;
            }
        }

        /// <summary>Read control value from binding data.</summary>
        public void ReadBindings(Control _Control)
        {
            if (_Control != null)
            {
                if (_Control is SMTextBox) ((SMTextBox)_Control).ReadBinding();
                else if (_Control is SMComboBox) ((SMComboBox)_Control).ReadBinding();
                else if (_Control is SMButton) ((SMButton)_Control).ReadBinding();
                else if (_Control is SMLabel) ((SMLabel)_Control).ReadBinding();
                else if (_Control is SMProgressBar) ((SMProgressBar)_Control).ReadBinding();
                else if (_Control is SMCheckBox) ((SMCheckBox)_Control).ReadBinding();
                else if (_Control is SMRadioButton) ((SMRadioButton)_Control).ReadBinding();
                else if (_Control is SMPictureBox) ((SMPictureBox)_Control).ReadBinding();
                else if (_Control is SMRating) ((SMRating)_Control).ReadBinding();
                else if (_Control is SMTrackBar) ((SMTrackBar)_Control).ReadBinding();
                else if (_Control is SMSlider) ((SMSlider)_Control).ReadBinding();
                else if (_Control is SMRichTextBox) ((SMRichTextBox)_Control).ReadBinding();
                else if (_Control is SMListView) ((SMListView)_Control).ReadBinding();
                else if (_Control is SMNumericUpDown) ((SMNumericUpDown)_Control).ReadBinding();
            }
        }

        /// <summary>Set binding controls and set states, recursive if specified and
        /// considering all starting by string control or all if parameter is empty.
        /// Call GetBindings() method for binding controls acquiring.</summary>
        public void SetBindings(Control _Control, bool _Recursive, string _StartWith)
        {
            int i;
            if (_Control != null)
            {
                _StartWith = _StartWith.Trim();
                if ((_StartWith.Length < 1) || _Control.Name.StartsWith(_StartWith))
                {
                    if (_Control is SMTextBox) ((SMTextBox)_Control).Dataset = this;
                    else if (_Control is SMComboBox) ((SMComboBox)_Control).Dataset = this;
                    else if (_Control is SMButton) ((SMButton)_Control).Dataset = this;
                    else if (_Control is SMLabel) ((SMLabel)_Control).Dataset = this;
                    else if (_Control is SMProgressBar) ((SMProgressBar)_Control).Dataset = this;
                    else if (_Control is SMCheckBox) ((SMCheckBox)_Control).Dataset = this;
                    else if (_Control is SMRadioButton) ((SMRadioButton)_Control).Dataset = this;
                    else if (_Control is SMPictureBox) ((SMPictureBox)_Control).Dataset = this;
                    else if (_Control is SMRating) ((SMRating)_Control).Dataset = this;
                    else if (_Control is SMTrackBar) ((SMTrackBar)_Control).Dataset = this;
                    else if (_Control is SMSlider) ((SMSlider)_Control).Dataset = this;
                    else if (_Control is SMRichTextBox) ((SMRichTextBox)_Control).Dataset = this;
                    else if (_Control is SMListView) ((SMListView)_Control).Dataset = this;
                    else if (_Control is SMNumericUpDown) ((SMNumericUpDown)_Control).Dataset = this;
                }
                if (_Recursive)
                {
                    if (_Control is SMUserControlPanel)
                    {
                        for (i = 0; i < ((SMUserControlPanel)_Control).ControlsCount; i++)
                        {
                            SetBindings(((SMUserControlPanel)_Control).Items[i], true, _StartWith);
                        }
                    }
                    else
                    {
                        for (i = 0; i < _Control.Controls.Count; i++) SetBindings(_Control.Controls[i], true, _StartWith);
                    }
                }
            }
        }

        /// <summary>Write controls values to binding data.</summary>
        public void WriteBindings()
        {
            int i;
            if (!dsBinding)
            {
                dsBinding = true;
                for (i = 0; i < dsBindings.Count; i++) WriteBindings(dsBindings[i]);
                dsBinding = false;
            }
        }

        /// <summary>Write control value to binding data.</summary>
        public void WriteBindings(Control _Control)
        {
            if (_Control != null)
            {
                if (_Control is SMTextBox) ((SMTextBox)_Control).WriteBinding();
                else if (_Control is SMComboBox) ((SMComboBox)_Control).WriteBinding();
                else if (_Control is SMButton) ((SMButton)_Control).WriteBinding();
                else if (_Control is SMCheckBox) ((SMCheckBox)_Control).WriteBinding();
                else if (_Control is SMRadioButton) ((SMRadioButton)_Control).WriteBinding();
                else if (_Control is SMPictureBox) ((SMPictureBox)_Control).WriteBinding();
                else if (_Control is SMRating) ((SMRating)_Control).WriteBinding();
                else if (_Control is SMTrackBar) ((SMTrackBar)_Control).WriteBinding();
                else if (_Control is SMSlider) ((SMSlider)_Control).WriteBinding();
                else if (_Control is SMRichTextBox) ((SMRichTextBox)_Control).WriteBinding();
                else if (_Control is SMListView) ((SMListView)_Control).WriteBinding();
                else if (_Control is SMNumericUpDown) ((SMNumericUpDown)_Control).WriteBinding();
            }
        }

        #endregion

        /* */

        #region Methods - Form data load

        /*  --------------------------------------------------------------------
         *  Methods - Form data load
         *  --------------------------------------------------------------------
         */

        /// <summary>Form data load timer initialization.</summary>
        private bool FormDataLoadTimerInit()
        {
            int i;
            if (disposing) return false;
            else if (dsFormDataLoadTimer == null)
            {
                try
                {
                    dsFormDataLoadTimer = new Timer();
                    dsFormDataLoadTimer.Enabled = false;
                    i = SM.Databases.DataLoadDelay;
                    if ((i > 0) && (i < 600000)) dsFormDataLoadTimer.Interval = i;
                    else dsFormDataLoadTimer.Interval = 330;
                    dsFormDataLoadTimer.Tick += FormDataLoadTimerTick;
                    return true;
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    return false;
                }
            }
            else return true;
        }

        /// <summary>Form data load timer tick.</summary>
        private void FormDataLoadTimerTick(object _Sender, EventArgs _EventArgs)
        {
            if (!disposing && (dsFormDataLoadTimer != null))
            {
                dsFormDataLoadTimer.Enabled = false;
                if (!disposing && (FormDataLoad != null)) FormDataLoad(this);
            }
        }

        /// <summary>Load data with mode.</summary>
        public void LoadFormData(SMFormDataLoadMode _FormDataLoadMode)
        {
            if (FormDataLoadTimerInit())
            {
                if (!disposing && (dsFormDataLoadTimer != null))
                {
                    dsFormDataLoadTimer.Enabled = false;
                    if (_FormDataLoadMode == SMFormDataLoadMode.Now)
                    {
                        if (!disposing && (FormDataLoad != null)) FormDataLoad(this);
                    }
                    else if (!disposing && (_FormDataLoadMode == SMFormDataLoadMode.Delayed)) dsFormDataLoadTimer.Enabled = true;
                }
            }
        }

        #endregion

        /* */

        #region Methods - Linked table

        /*  --------------------------------------------------------------------
         *  Methods - Linked dataset
         *  --------------------------------------------------------------------
         */

        /// <summary>Return true if linked dataset.</summary>
        public bool IsLinkedDataset()
        {
            if (ExtendedDataset == null) return false;
            else return dsLinkedTable.Length > 0;
        }

        /// <summary>Append linked dataset.</summary>
        public bool LinkedDatasetAppend()
        {
            bool r = true;
            string id;
            if (IsLinkedDataset()) 
            {
                if ((dsLinkedSourceField.Length > 0) && (dsLinkedTargetField.Length > 0))
                {
                    if (extendedDataset.Active)
                    {
                        id = this.FieldStr(dsLinkedSourceField);
                        if (id.Trim().Length > 0)
                        {
                            if (extendedDataset.Append())
                            {
                                if (!extendedDataset.Assign(dsLinkedTargetField, id))
                                {
                                    extendedDataset.Cancel();
                                    r = false;
                                    SM.Raise("SMDataSet: linked dataset assign failed.", false);
                                }
                            }
                            else
                            {
                                r = false;
                                SM.Raise("SMDataSet: linked dataset append failed.", false);
                            }
                        }
                    }
                    else
                    {
                        r = false;
                        SM.Raise("SMDataSet: linked dataset not active.", false);
                    }
                }
                else
                {
                    r = false;
                    SM.Raise("SMDataSet: missing linked fields.", false);
                }
            }
            return r;
        }

        /// <summary>Cancel linked dataset.</summary>
        public bool LinkedDatasetCancel()
        {
            bool r = true;
            if (IsLinkedDataset()) 
            {
                if (extendedDataset.Modifying(false))
                {
                    if (!extendedDataset.Cancel())
                    {
                        SM.Raise("SMDataSet: linked dataset cancel failed.", false);
                        r = false;
                    }
                }
            }
            return r;
        }

        /// <summary>Close linked dataset.</summary>
        public bool LinkedDatasetClose()
        {
            bool r = true;
            if (IsLinkedDataset()) 
            {
                r = extendedDataset.Close();
            }
            return r;
        }

        /// <summary>Delete current record of linked dataset.</summary>
        public bool LinkedDatasetDelete()
        {
            bool r = true;
            if (IsLinkedDataset()) 
            {
                if ((dsLinkedSourceField.Length > 0) && (dsLinkedTargetField.Length > 0))
                {
                    if (extendedDataset.Active)
                    {
                        if (extendedDataset.FieldStr(dsLinkedTargetField) == dsLinkedOldValue)
                        {
                            if (!extendedDataset.Delete())
                            {
                                r = false;
                                SM.Raise("SMDataSet: delete error on linked dataset.", false);
                            }
                        }
                        else
                        {
                            r = false;
                            SM.Raise("SMDataSet: linked dataset misaligned.", false);
                        }
                    }
                    else
                    {
                        r = false;
                        SM.Raise("SMDataSet: linked dataset not active.", false);
                    }
                }
                else
                {
                    r = false;
                    SM.Raise("SMDataSet: missing linked fields.", false);
                }
            }
            dsLinkedOldValue = "";
            return r;
        }

        /// <summary>Edit current record of linked dataset.</summary>
        public bool LinkedDatasetEdit()
        {
            bool r = true;
            string id;
            if (IsLinkedDataset()) 
            {
                if ((dsLinkedSourceField.Length > 0) && (dsLinkedTargetField.Length > 0))
                {
                    if (extendedDataset.Active)
                    {
                        id = this.FieldStr(dsLinkedSourceField);
                        if (id.Trim().Length > 0)
                        {
                            if (extendedDataset.FieldStr(dsLinkedTargetField) == id)
                            {
                                if (!extendedDataset.Modifying(true))
                                {
                                    r = false;
                                    SM.Raise("SMDataSet: edit error on linked dataset.", false);
                                }
                            }
                            else
                            {
                                r = false;
                                SM.Raise("SMDataSet: linked dataset misaligned.", false);
                            }
                        }
                    }
                    else
                    {
                        r = false;
                        SM.Raise("SMDataSet: linked dataset not active.", false);
                    }
                }
                else
                {
                    r = false;
                    SM.Raise("SMDataSet: missing linked fields.", false);
                }
            }
            return r;
        }

        /// <summary>Open linked dataset.</summary>
        public bool LinkedDatasetOpen()
        {
            bool r = true;
            string sql, id;
            if (IsLinkedDataset()) 
            {
                if ((dsLinkedSourceField.Length > 0) && (dsLinkedTargetField.Length > 0))
                {
                    id = this.FieldStr(dsLinkedSourceField);
                    sql = "SELECT * FROM [" + dsLinkedTable + "] WHERE [" + dsLinkedTargetField + "]=" + SM.QuotedStr(id);
                    if (extendedDataset.Open(sql))
                    {
                        if (extendedDataset.Eof)
                        {
                            if (id.Trim().Length > 0)
                            {
                                if (extendedDataset.Append())
                                {
                                    extendedDataset.Assign(dsLinkedTargetField, this.FieldStr(dsLinkedSourceField));
                                    if (!extendedDataset.Post())
                                    {
                                        extendedDataset.Cancel();
                                        r = false;
                                        SM.Raise("SMDataSet: error posting record on linked dataset.", false);
                                    }
                                }
                                else
                                {
                                    r = false;
                                    SM.Raise("SMDataSet: error adding record on linked dataset.", false);
                                }
                            }
                        }
                    }
                    else
                    {
                        r = false;
                        SM.Raise("SMDataSet: error opening linked dataset.", false);
                    }
                }
                else
                {
                    r = false;
                    SM.Raise("SMDataSet: missing linked fields.", false);
                }
            }
            return r;
        }

        /// <summary>Post linked dataset.</summary>
        public bool LinkedDatasetPost()
        {
            bool r = true;
            string id;
            if (IsLinkedDataset())
            {
                if ((dsLinkedSourceField.Length > 0) && (dsLinkedTargetField.Length > 0))
                {
                    if (extendedDataset.Active)
                    {
                        id = this.FieldStr(dsLinkedSourceField);
                        if (id.Trim().Length > 0)
                        {
                            if (extendedDataset.FieldStr(dsLinkedTargetField) == id)
                            {
                                if (extendedDataset.Modifying(false))
                                {
                                    if (!extendedDataset.Post())
                                    {
                                        r = false;
                                        SM.Raise("SMDataSet: error posting record on linked dataset.", false);
                                    }
                                }
                                else
                                {
                                    r = false;
                                    SM.Raise("SMDataSet: linked dataset is not in editing state.", false);
                                }
                            }
                            else
                            {
                                r = false;
                                SM.Raise("SMDataSet: linked dataset misaligned.", false);
                            }
                        }
                    }
                    else
                    {
                        r = false;
                        SM.Raise("SMDataSet: linked dataset not active.", false);
                    }
                }
                else
                {
                    r = false;
                    SM.Raise("SMDataSet: missing linked fields.", false);
                }
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
