/*  ===========================================================================
 *  
 *  File:       SMDataset.cs
 *  Version:    2.0.201
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dataset component.
 *
 *  ===========================================================================
 */

using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace SMCodeSystem
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
        private readonly SMCode SM = null;

        /// <summary>Dataset database alias.</summary>
        private string alias = "";

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

        /// <summary>GUID column presence flag.</summary>
        private bool guidColumn = false;

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

        /// <summary>Occurs when the column value changes by assignment.</summary>
        public delegate void OnColumnChange(object _Sender, DataColumn _DataColumn);
        /// <summary>Occurs when the column value changes by assignment.</summary>
        public event OnColumnChange ColumnChange;

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

        /// <summary>Quick access declarations.</summary>
        public object this[string _FieldName]
        {
            get
            {
                return Field(_FieldName);
            }
            set
            {
                Assign(_FieldName, value);
            }
        }

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

        /// <summary>Return table columns collection or null.</summary>
        [Browsable(false)]
        public DataColumnCollection Columns
        {
            get
            {
                if (Table != null) return Table.Columns;
                else return null;
            }
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

        /// <summary>Get dataset instance.</summary>
        public DataSet Dataset { get; private set; }

        /// <summary>Component disposing flag.</summary>
        public bool Disposing { get; private set; }

        /// <summary>Specifies whether dataset contains no records.</summary>
        [Browsable(false)]
        public bool Empty
        {
            get { return RecordCount() < 1; }
        }

        /// <summary>Indicates whether a dataset is positioned at the last record.</summary>
        [Browsable(false)]
        public bool Eof { get; private set; } = false;

        /// <summary>Indicates if dataset has an exclusive database (not managed in databases collection).</summary>
        [Browsable(false)]
        public bool ExclusiveDatabase { get; private set; } = false;

        /// <summary>Get or set extended dataset.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Get or set extended dataset.")]
        public SMDataset ExtendedDataset { get; set; } = null;

        /// <summary>Get or set GUID column name.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        public string GuidColumn { get; set; } = "";

        /// <summary>Contains the text of the SQL statement to execute for the dataset.</summary>
        [Browsable(true)]
        [Category("SMCode")]
        [Description("Contains the text of the SQL statement to execute for the dataset.")]
        public string Query { get; set; } = "";

        /// <summary>Indicates if dataset is read-only.</summary>
        [Browsable(false)]
        public bool ReadOnly { get; private set; } = false;

        /// <summary>Specifies if dataset position is on an available record.</summary>
        [Browsable(false)]
        public bool RecordAvailable
        {
            get { return (recordIndex > -1) && (recordIndex < RecordCount()); }
        }

        /// <summary>Indicates the index of the current record in the dataset.</summary>
        [Browsable(false)]
        public int RecordIndex
        {
            get { return recordIndex; }
            set { Goto(value); }
        }

        /// <summary>Get or set record modification information columns presence flag.</summary>
        [Browsable(false)]
        public bool RecordInformationColumn { get; set; } = false;

        /// <summary>Indicates the current active row of the dataset.</summary>
        [Browsable(false)]
        public DataRow Row { get; private set; } = null;

        /// <summary>Return table rows collection or null.</summary>
        [Browsable(false)]
        public DataRowCollection Rows 
        { 
            get
            {
                if (Table != null) return Table.Rows;
                else return null;
            }
        }

        /// <summary>Indicates the current operating mode of the dataset.</summary>
        [Browsable(false)]
        public SMDatasetState State { get; private set; }

        /// <summary>Indicates the current table object open in dataset.</summary>
        [Browsable(false)]
        public DataTable Table { get; private set; }

        /// <summary>Indicates the current table name open in dataset.</summary>
        [Browsable(false)]
        public string TableName { get; private set; }

        /// <summary>Get or set SM unique identifier column name if present.</summary>
        [Browsable(false)]
        public bool UniqueIdentifierColumn { get; set; } = false;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Dataset instance constructor.</summary>
        public SMDataset(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeComponent();
            Clear();
        }

        /// <summary>Dataset instance constructor with db database connection.</summary>
        public SMDataset(SMDatabase _Database, SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeComponent();
            Clear();
            Database = _Database;
        }

        /// <summary>Dataset instance constructor with alias connection.</summary>
        public SMDataset(string _Alias, SMCode _SM, bool _ExclusiveDatabase = false)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeComponent();
            Clear();
            ExclusiveDatabase = _ExclusiveDatabase;
            if (ExclusiveDatabase)
            {
                Database = new SMDatabase(SM);
                Database.Copy(_Alias);
                alias = "";
            }
            else
            {
                alias = _Alias;
                Database = SM.Databases.Keep(alias);
            }
        }

        /// <summary>Dataset instance constructor with ds dataset connection.</summary>
        public SMDataset(SMDataset _DataSet, SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeComponent();
            Clear();
            if (_DataSet != null)
            {
                if (_DataSet.Alias.Trim().Length > 0)
                {
                    alias = _DataSet.Alias;
                    Database = SM.Databases.Keep(alias);
                }
                else Database = _DataSet.Database;
            }
        }

        /// <summary>Dataset instance constructor with container</summary>
        public SMDataset(IContainer _Container)
        {
            SM = SMCode.CurrentOrNew();
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
            Database = null;
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
                    if (Database != null)
                    {
                        if (Database.Active)
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
                if (AfterClose != null) AfterClose(this);
            }
            //
            return r;
        }

        /// <summary>Return true if field name is the name of one of any table open column.</summary>
        public bool IsField(string _FieldName)
        {
            return FieldIndex(_FieldName) > -1;
        }

        /// <summary>Return index of field name in table open columns.</summary>
        public int FieldIndex(string _FieldName)
        {
            if (_FieldName.Length < 1) return -1;
            else if (Table != null) return Table.Columns.IndexOf(_FieldName);
            else if ((Database.Type == SMDatabaseType.Mdb) && (oleReader != null)) return oleReader.GetOrdinal(_FieldName);
            else if ((Database.Type == SMDatabaseType.Dbf) && (oleReader != null)) return oleReader.GetOrdinal(_FieldName);
            else if ((Database.Type == SMDatabaseType.MySql) && (mySqlReader != null)) return mySqlReader.GetOrdinal(_FieldName);
            else if (sqlReader != null) return sqlReader.GetOrdinal(_FieldName);
            else return -1;
        }

        /// <summary>Return max length of column specified or -1 if fail (only for data tables).</summary>
        public int FieldMaxLength(string _FieldName)
        {
            int r = FieldIndex(_FieldName);
            if (r > -1)
            {
                if (Table != null) r = Table.Columns[r].MaxLength;
                else r = 0;
            }
            return r;
        }

        /// <summary>Load the recordset. Return true if succeed.</summary>
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
                    if (Database.Type == SMDatabaseType.Mdb) oleAdapter.Fill(Dataset);
                    else if (Database.Type == SMDatabaseType.Dbf) oleAdapter.Fill(Dataset);
                    else if (Database.Type == SMDatabaseType.MySql) mySqlAdapter.Fill(Dataset);
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
                            if (Database.Type == SMDatabaseType.Mdb)
                            {
                                // Insert command
                                oleAdapter.InsertCommand.CommandText = SM.SqlCommandInsert(this);
                                if (Database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(oleAdapter.InsertCommand);
                                // Update command
                                oleAdapter.UpdateCommand.CommandText = SM.SqlCommandUpdate(this);
                                if (Database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(oleAdapter.UpdateCommand);
                                // Delete command
                                oleAdapter.DeleteCommand.CommandText = SM.SqlCommandDelete(this);
                                if (Database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(oleAdapter.DeleteCommand);
                            }
                            else if (Database.Type == SMDatabaseType.Dbf)
                            {
                                // Insert command
                                oleAdapter.InsertCommand.CommandText = SM.SqlCommandInsert(this);
                                if (Database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(oleAdapter.InsertCommand);
                                // Update command
                                oleAdapter.UpdateCommand.CommandText = SM.SqlCommandUpdate(this);
                                if (Database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(oleAdapter.UpdateCommand);
                                // Delete command
                                oleAdapter.DeleteCommand.CommandText = SM.SqlCommandDelete(this);
                                if (Database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(oleAdapter.DeleteCommand);
                            }
                            else if (Database.Type == SMDatabaseType.MySql)
                            {
                                // Insert command
                                mySqlAdapter.InsertCommand.CommandText = SM.SqlCommandInsert(this);
                                if (Database.CommandTimeout > 0) mySqlAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(mySqlAdapter.InsertCommand);
                                // Update command
                                mySqlAdapter.UpdateCommand.CommandText = SM.SqlCommandUpdate(this);
                                if (Database.CommandTimeout > 0) mySqlAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(mySqlAdapter.UpdateCommand);
                                // Delete command
                                mySqlAdapter.DeleteCommand.CommandText = SM.SqlCommandDelete(this);
                                if (Database.CommandTimeout > 0) mySqlAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(mySqlAdapter.DeleteCommand);
                            }
                            else
                            {
                                // Insert command
                                sqlAdapter.InsertCommand.CommandText = SM.SqlCommandInsert(this);
                                if (Database.CommandTimeout > 0) sqlAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(sqlAdapter.InsertCommand);
                                // Update command
                                sqlAdapter.UpdateCommand.CommandText = SM.SqlCommandUpdate(this);
                                if (Database.CommandTimeout > 0) sqlAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(sqlAdapter.UpdateCommand);
                                // Delete command
                                sqlAdapter.DeleteCommand.CommandText = SM.SqlCommandDelete(this);
                                if (Database.CommandTimeout > 0) sqlAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                SMDatabase.ParametersByName(sqlAdapter.DeleteCommand);
                            }
                        }
                    }
                    ChangeState(SMDatasetState.Browse);
                    First();
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

        /// <summary>Open dataset with query specified in sqlQuery parameter. 
        /// If readOnly is true dataset will be opened in read-only mode. 
        /// Return true if succeed.</summary>
        public bool Open(string _SQLSelectionQuery = "", bool _ReadOnly = false)
        {
            bool r = false;
            UniqueIdentifierColumn = false;
            RecordInformationColumn = false;
            guidColumn = false;
            Close();
            if (OpenDatabase())
            {
                bool cancel = false;
                if (BeforeOpen != null) BeforeOpen(this, ref cancel);
                if (!cancel)
                {
                    if (SM.Empty(_SQLSelectionQuery)) _SQLSelectionQuery = Query;
                    adaptedQuery = SMDatabase.Delimiters(SM.SqlMacros(_SQLSelectionQuery, Database.Type), Database.Type);
                    if (_ReadOnly) ReadOnly = true;
                    else ReadOnly = SM.Btw(adaptedQuery.ToLower(), " from ", " on ").IndexOf("join") > -1;
                    try
                    {
                        Dataset = new DataSet() { EnforceConstraints = !ReadOnly };
                        try
                        {
                            if (Database.Type == SMDatabaseType.Mdb)
                            {
                                oleAdapter = new OleDbDataAdapter(adaptedQuery, Database.ConnectionOleDB);
                                oleBuilder = new OleDbCommandBuilder(oleAdapter);
                                oleBuilder.QuotePrefix = SMDatabase.SqlPrefix;
                                oleBuilder.QuoteSuffix = SMDatabase.SqlSuffix;
                                oleAdapter.FillSchema(Dataset, SchemaType.Source);
                                if (!ReadOnly)
                                {
                                    oleAdapter.InsertCommand = oleBuilder.GetInsertCommand();
                                    if (Database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                    oleAdapter.UpdateCommand = oleBuilder.GetUpdateCommand();
                                    if (Database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                    oleAdapter.DeleteCommand = oleBuilder.GetDeleteCommand();
                                    if (Database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                }
                            }
                            else if (Database.Type == SMDatabaseType.Dbf)
                            {
                                oleAdapter = new OleDbDataAdapter(adaptedQuery, Database.ConnectionOleDB);
                                oleBuilder = new OleDbCommandBuilder(oleAdapter);
                                oleBuilder.QuotePrefix = SMDatabase.SqlPrefix;
                                oleBuilder.QuoteSuffix = SMDatabase.SqlSuffix;
                                oleAdapter.FillSchema(Dataset, SchemaType.Source);
                                if (!ReadOnly)
                                {
                                    oleAdapter.InsertCommand = oleBuilder.GetInsertCommand();
                                    if (Database.CommandTimeout > 0) oleAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                    oleAdapter.UpdateCommand = oleBuilder.GetUpdateCommand();
                                    if (Database.CommandTimeout > 0) oleAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                    oleAdapter.DeleteCommand = oleBuilder.GetDeleteCommand();
                                    if (Database.CommandTimeout > 0) oleAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                }
                            }
                            else if (Database.Type == SMDatabaseType.MySql)
                            {
                                mySqlAdapter = new MySqlDataAdapter(adaptedQuery, Database.ConnectionMySql);
                                mySqlBuilder = new MySqlCommandBuilder(mySqlAdapter);
                                mySqlBuilder.QuotePrefix = SMDatabase.MySqlPrefix;
                                mySqlBuilder.QuoteSuffix = SMDatabase.MySqlSuffix;
                                mySqlAdapter.FillSchema(Dataset, SchemaType.Source);
                                if (!ReadOnly)
                                {
                                    mySqlAdapter.InsertCommand = mySqlBuilder.GetInsertCommand();
                                    if (Database.CommandTimeout > 0) mySqlAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                    mySqlAdapter.UpdateCommand = mySqlBuilder.GetUpdateCommand();
                                    if (Database.CommandTimeout > 0) mySqlAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                    mySqlAdapter.DeleteCommand = mySqlBuilder.GetDeleteCommand();
                                    if (Database.CommandTimeout > 0) mySqlAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                }
                            }
                            else
                            {
                                sqlAdapter = new SqlDataAdapter(adaptedQuery, Database.ConnectionSql);
                                sqlBuilder = new SqlCommandBuilder(sqlAdapter);
                                sqlBuilder.QuotePrefix = SMDatabase.SqlPrefix;
                                sqlBuilder.QuoteSuffix = SMDatabase.SqlSuffix;
                                sqlAdapter.FillSchema(Dataset, SchemaType.Source);
                                if (!ReadOnly)
                                {
                                    sqlAdapter.InsertCommand = sqlBuilder.GetInsertCommand();
                                    if (Database.CommandTimeout > 0) sqlAdapter.InsertCommand.CommandTimeout = Database.CommandTimeout;
                                    sqlAdapter.UpdateCommand = sqlBuilder.GetUpdateCommand();
                                    if (Database.CommandTimeout > 0) sqlAdapter.UpdateCommand.CommandTimeout = Database.CommandTimeout;
                                    sqlAdapter.DeleteCommand = sqlBuilder.GetDeleteCommand();
                                    if (Database.CommandTimeout > 0) sqlAdapter.DeleteCommand.CommandTimeout = Database.CommandTimeout;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SM.Error(ex.Message + " on query: " + adaptedQuery, ex);
                        }
                        //
                        if (IsField("ID"))
                        {
                            UniqueIdentifierColumn = SMDataType.IsText(Table.Columns["ID"].DataType) && (Table.Columns["ID"].MaxLength == 12);
                        }
                        //
                        if (!SM.Empty(GuidColumn))
                        {
                            if (IsField(GuidColumn)) guidColumn = SMDataType.IsGuid(Table.Columns[GuidColumn].DataType);
                        }
                        //
                        RecordInformationColumn = IsField("InsertionDate") && IsField("InsertionUser") && IsField("ModificationDate") && IsField("ModificationUser");
                        //
                        Load();
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex.Message + " on query: " + adaptedQuery, ex);
                    }
                    r = State != SMDatasetState.Closed;
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
            if (alias.Trim().Length > 0) Database = SM.Databases.Keep(alias);
            else if (Database != null) Database.Keep();
            if (Database == null) SM.Raise("SMDatabase: null database error.", false);
            else
            {
                r = Database.Active;
                if (!r)
                {
                    SM.DoEvents();
                    r = Database.Open();
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
                    if (Database.Type == SMDatabaseType.Mdb)
                    {
                        i = oleReader.GetOrdinal(_FieldName);
                        if (i > -1) return oleReader[i];
                        else return null;
                    }
                    else if (Database.Type == SMDatabaseType.Dbf)
                    {
                        i = oleReader.GetOrdinal(_FieldName);
                        if (i > -1) return oleReader[i];
                        else return null;
                    }
                    else if (Database.Type == SMDatabaseType.MySql)
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
            if (o != null) return SM.Format(o.ToString(), _FormatString);
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
        public DateTime FieldDate(string _FieldName, bool _IncludeTime = false)
        {
            object o = Field(_FieldName);
            if (o != null) return SM.ToDate(o.ToString(), SM.DateFormat, _IncludeTime);
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

        /// <summary>Start a readonly session with sqlQuery reading first record 
        /// on current database connection. Returns true if succeed.</summary>
        public bool Read(string _SqlQuery)
        {
            bool retValue = false;
            Close();
            try
            {
                if (Database != null)
                {
                    if (Database.Keep())
                    {
                        if (Database.Type == SMDatabaseType.Mdb)
                        {
                            oleCommand = new OleDbCommand(SMDatabase.Delimiters(SM.SqlMacros(_SqlQuery, Database.Type), Database.Type), Database.ConnectionOleDB);
                            oleReader = oleCommand.ExecuteReader();
                        }
                        else if (Database.Type == SMDatabaseType.Dbf)
                        {
                            oleCommand = new OleDbCommand(SMDatabase.Delimiters(SM.SqlMacros(_SqlQuery, Database.Type), Database.Type), Database.ConnectionOleDB);
                            oleReader = oleCommand.ExecuteReader();
                        }
                        else if (Database.Type == SMDatabaseType.MySql)
                        {
                            mySqlCommand = new MySqlCommand(SMDatabase.Delimiters(SM.SqlMacros(_SqlQuery, Database.Type), Database.Type), Database.ConnectionMySql);
                            mySqlReader = mySqlCommand.ExecuteReader();
                        }
                        else
                        {
                            sqlCommand = new SqlCommand(SMDatabase.Delimiters(SM.SqlMacros(_SqlQuery, Database.Type), Database.Type), Database.ConnectionSql);
                            sqlReader = sqlCommand.ExecuteReader();
                        }
                        retValue = Read();
                        if (retValue)
                        {
                            ChangeState(SMDatasetState.Read);
                            Eof = false;
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
                if (Database != null)
                {
                    if (Database.Keep())
                    {
                        if (Database.Type == SMDatabaseType.Mdb) retValue = oleReader.Read();
                        else if (Database.Type == SMDatabaseType.Dbf) retValue = oleReader.Read();
                        else if (Database.Type == SMDatabaseType.MySql) retValue = mySqlReader.Read();
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

        /*  ===================================================================
         *  Methods - Assigning
         *  ===================================================================
         */

        /// <summary>Assign value content to field named column.</summary>
        public bool Assign(string _FieldName, string _Value)
        {
            return Assign(Table.Columns.IndexOf(_FieldName), _Value);
        }

        /// <summary>Assign value content to field named column.</summary>
        public bool Assign(string _FieldName, object _Value)
        {
            return Assign(Table.Columns.IndexOf(_FieldName), _Value);
        }

        /// <summary>Assign value content to field with column index.</summary>
        public bool Assign(int _ColumnIndex, object _Value)
        {
            string s;
            DataColumn co;
            if ((Row != null) && (_ColumnIndex > -1) && (_ColumnIndex<Table.Columns.Count))
            {
                try
                {
                    co = Table.Columns[_ColumnIndex];
                    if (co.AutoIncrement) Row[_ColumnIndex] = 0;
                    else if (co.DataType == SMDataType.Boolean)
                    {
                        if (_Value == null) Row[_ColumnIndex] = false;
                        else Row[_ColumnIndex] = "1TVStvs".IndexOf((_Value.ToString().Trim() + " ")[0]) > -1;
                    }
                    else if ((co.DataType == SMDataType.Byte)
                        || (co.DataType == SMDataType.SByte))
                    {
                        if (_Value == null) Row[_ColumnIndex] = 0;
                        else Row[_ColumnIndex] = Convert.ToByte(SM.ToInt(_Value.ToString()) % 256);
                    }
                    else if (co.DataType == SMDataType.Char)
                    {
                        if (_Value == null) Row[_ColumnIndex] = '\0';
                        else Row[_ColumnIndex] = (_Value.ToString() + " ")[0];
                    }
                    else if (co.DataType == SMDataType.DateTime)
                    {
                        if (_Value == null) Row[_ColumnIndex] = DBNull.Value;
                        else if (_Value is DateTime)
                        {
                            if (SM.MinDate((DateTime)_Value)) Row[_ColumnIndex] = DBNull.Value;
                            else Row[_ColumnIndex] = (DateTime)_Value;
                        }
                        else if (_Value.ToString().Trim().Length > 0) Row[_ColumnIndex] = SM.ToDate(_Value.ToString());
                        else Row[_ColumnIndex] = DBNull.Value;
                    }
                    else if (co.DataType == SMDataType.Decimal)
                    {
                        if (_Value == null) Row[_ColumnIndex] = 0.0;
                        else Row[_ColumnIndex] = Convert.ToDecimal(SM.Val(_Value.ToString()));
                    }
                    else if (co.DataType == SMDataType.Double)
                    {
                        if (_Value == null) Row[_ColumnIndex] = 0.0d;
                        else Row[_ColumnIndex] = SM.Val(_Value.ToString());
                    }
                    else if (co.DataType == SMDataType.Single)
                    {
                        if (_Value == null) Row[_ColumnIndex] = 0.0f;
                        else Row[_ColumnIndex] = Convert.ToSingle(SM.Val(_Value.ToString()));
                    }
                    else if ((co.DataType == SMDataType.Int32)
                        || (co.DataType == SMDataType.Int16)
                        || (co.DataType == SMDataType.UInt32)
                        || (co.DataType == SMDataType.UInt16))
                    {
                        if (_Value == null) Row[_ColumnIndex] = 0;
                        else Row[_ColumnIndex] = SM.ToInt(_Value.ToString());
                    }
                    else if ((co.DataType == SMDataType.Int64)
                        || (co.DataType == SMDataType.UInt64))
                    {
                        if (_Value == null) Row[_ColumnIndex] = 0;
                        else Row[_ColumnIndex] = SM.ToLong(_Value.ToString());
                    }
                    else if (co.DataType == SMDataType.TimeSpan)
                    {
                        if (_Value == null) Row[_ColumnIndex] = DBNull.Value;
                        else if (_Value.ToString().Trim().Length > 0) Row[_ColumnIndex] = new TimeSpan(SM.ToDate(_Value.ToString()).Ticks);
                        else Row[_ColumnIndex] = DBNull.Value;
                    }
                    else if (co.DataType == SMDataType.BytesArray)
                    {
                        if (_Value == null) Row[_ColumnIndex] = DBNull.Value;
                        else Row[_ColumnIndex] = _Value;
                    }
                    else if (co.DataType == SMDataType.Guid)
                    {
                        if (_Value == null) Row[_ColumnIndex] = DBNull.Value;
                        else
                        {
                            s = SM.ToStr(_Value);
                            if (SM.Empty(s)) Row[_ColumnIndex] = DBNull.Value;
                            else Row[_ColumnIndex] = Guid.Parse(s);
                        }
                    }
                    else if (_Value == null) Row[_ColumnIndex] = "";
                    else if (co.MaxLength > 0) Row[_ColumnIndex] = SM.Mid(_Value.ToString(), 0, co.MaxLength);
                    else Row[_ColumnIndex] = _Value;
                    if (ColumnChange != null) ColumnChange(this, co);
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
            int i = Table.Columns.IndexOf(_FieldName);
            if (i > -1) return Assign(i, SM.Blank(Table.Columns[i]));
            else return false;
        }

        #endregion

        /* */

        #region Methods - Searching

        /*  ===================================================================
         *  Methods - Searching
         *  ===================================================================
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
            if ((_KeyFields != null) && (_Values != null) && (Table.Rows.Count > 0))
            {
                if (k > _Values.Length) k = _Values.Length;
                map = new int[k];
                for (i = 0; i < k; i++) map[i] = Table.Columns.IndexOf(_KeyFields[i]);
                //
                // binary search
                //
                if (_BinarySearch)
                {
                    a = 0;
                    b = Table.Rows.Count - 1;
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
                            s = SM.ToSortable(Table.Columns[map[j]], Table.Rows[a][map[j]]);
                            i = String.Compare(s, _Values[j], !Table.CaseSensitive);
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
                                s = SM.ToSortable(Table.Columns[map[j]], Table.Rows[b][map[j]]);
                                i = String.Compare(s, _Values[j], !Table.CaseSensitive);
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
                                s = SM.ToSortable(Table.Columns[map[j]], Table.Rows[c][map[j]]);
                                i = String.Compare(s, _Values[j], !Table.CaseSensitive);
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
                    b = Table.Rows.Count;
                    while ((i < b) && (r < 0))
                    {
                        q = true;
                        j = 0;
                        while (q && (j < k))
                        {
                            if (_Values[j] != SM.ToSortable(Table.Columns[map[j]], Table.Rows[i][map[j]])) q = false;
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
            if (Table.Rows.Count > 0)
            {
                if (k > _Values.Length) k = _Values.Length;
                //
                // binary search
                //
                if (_BinarySearch)
                {
                    a = 0;
                    b = Table.Rows.Count - 1;
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
                            s = SM.ToSortable(Table.Columns[_KeyFieldsIndexes[j]], Table.Rows[a][_KeyFieldsIndexes[j]]);
                            i = String.Compare(s, _Values[j], !Table.CaseSensitive);
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
                                s = SM.ToSortable(Table.Columns[_KeyFieldsIndexes[j]], Table.Rows[b][_KeyFieldsIndexes[j]]);
                                i = String.Compare(s, _Values[j], !Table.CaseSensitive);
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
                                s = SM.ToSortable(Table.Columns[_KeyFieldsIndexes[j]], Table.Rows[c][_KeyFieldsIndexes[j]]);
                                i = String.Compare(s, _Values[j], !Table.CaseSensitive);
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
                    b = Table.Rows.Count;
                    while ((i < b) && (r < 0))
                    {
                        q = true; j = 0;
                        while (q && (j < k))
                        {
                            if (_Values[j] != SM.ToSortable(Table.Columns[_KeyFieldsIndexes[j]], Table.Rows[i][_KeyFieldsIndexes[j]])) q = false;
                            else j++;
                        }
                        if (q) r = i;
                        else i++;
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

        /*  ===================================================================
         *  Methods - Editing
         *  ===================================================================
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
            else if (ReadOnly) SM.Raise("SMDataSet: cancel cannot performed on readonly dataset.", false);
            else if (!Modifying(false)) SM.Raise("SMDataSet: cancel operation can performed only on editing or appending state dataset.", false);
            else
            {
                try 
                { 
                    Table.RejectChanges();
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
                if (Dataset != null)
                {
                    if (Dataset.HasChanges())
                    {
                        if (Database.Type == SMDatabaseType.Mdb)
                        {
                            oleAdapter.InsertCommand.Connection = Database.ConnectionOleDB;
                            oleAdapter.UpdateCommand.Connection = Database.ConnectionOleDB;
                            oleAdapter.DeleteCommand.Connection = Database.ConnectionOleDB;
                            oleAdapter.Update(Dataset);
                        }
                        else if (Database.Type == SMDatabaseType.Dbf)
                        {
                            oleAdapter.InsertCommand.Connection = Database.ConnectionOleDB;
                            oleAdapter.UpdateCommand.Connection = Database.ConnectionOleDB;
                            oleAdapter.DeleteCommand.Connection = Database.ConnectionOleDB;
                            oleAdapter.Update(Dataset);
                        }
                        else if (Database.Type == SMDatabaseType.MySql)
                        {
                            mySqlAdapter.InsertCommand.Connection = Database.ConnectionMySql;
                            mySqlAdapter.UpdateCommand.Connection = Database.ConnectionMySql;
                            mySqlAdapter.DeleteCommand.Connection = Database.ConnectionMySql;
                            mySqlAdapter.Update(Dataset);
                        }
                        else
                        {
                            sqlAdapter.InsertCommand.Connection = Database.ConnectionSql;
                            sqlAdapter.UpdateCommand.Connection = Database.ConnectionSql;
                            sqlAdapter.DeleteCommand.Connection = Database.ConnectionSql;
                            sqlAdapter.Update(Dataset);
                        }
                    }
                    Table.AcceptChanges();
                    bufferCount = 0;
                    if (Table.PrimaryKey != null)
                    {
                        if (Table.PrimaryKey.Length > 0)
                        {
                            if (Database.Type == SMDatabaseType.Mdb) oleAdapter.Fill(Dataset);
                            else if (Database.Type == SMDatabaseType.Dbf) oleAdapter.Fill(Dataset);
                            else if (Database.Type == SMDatabaseType.MySql) mySqlAdapter.Fill(Dataset);
                            else sqlAdapter.Fill(Dataset);
                            Table = Dataset.Tables[0];
                        }
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
            if (bufferCount < ChangesBufferSize) return true;
            else return Commit();
        }

        /// <summary>Copy the values of corresponding fields from source dataset current row 
        /// to this dataset current row. Returns true if succeed.</summary>
        public bool CopyRow(SMDataset _SourceDataSet, string[] _ExcludeFields = null)
        {
            int i;
            bool r = false;
            if ((_SourceDataSet != null) && (Row != null) && (Table != null))
            {
                if (this.Modifying(false) && (_SourceDataSet.State != SMDatasetState.Closed))
                {
                    i = Table.Columns.Count;
                    r = true;
                    while (r && (i > 0))
                    {
                        i--;
                        if (!Table.Columns[i].AutoIncrement)
                        {
                            if (_SourceDataSet.IsField(Table.Columns[i].ColumnName)
                                && ((_ExcludeFields == null) || (SM.Find(Table.Columns[i].ColumnName, _ExcludeFields, true) < 0)))
                            {
                                r = this.Assign(i, _SourceDataSet.Field(Table.Columns[i].ColumnName));
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
            else if (ReadOnly) SM.Raise("SMDataSet: delete cannot performed on readonly dataset.", false);
            else if (!Browsing) SM.Raise("SMDataSet: delete can performed only on browsing state dataset.", false);
            else
            {
                if (Row != null)
                {
                    ChangeState(SMDatasetState.Delete);
                    Row.Delete();
                    if (Buffer()) r = true;
                    else Table.RejectChanges();
                    if ((recordIndex > -1) && (recordIndex < Table.Rows.Count))
                    {
                        Row = Table.Rows[recordIndex];
                        Bof = false;
                        Eof = false;
                        r = SkipDeletedForward();
                        if (r)
                        {
                            if (AfterDelete != null) AfterDelete(this);
                        }
                    }
                    else
                    {
                        Row = null;
                        Eof = true;
                        Bof = true;
                    }
                    ChangeState(SMDatasetState.Browse);
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
            else if (ReadOnly) SM.Raise("SMDataSet: edit cannot performed on readonly dataset.", false);
            else if (!Browsing) SM.Raise("SMDataSet: edit can performed only on browsing state dataset.", false);
            else
            {
                ChangeState(SMDatasetState.Edit);
                if (AfterEdit != null) AfterEdit(this);
                r = true;
            }
            return r;
        }

        /// <summary>Executes SQL statement passed as parameter. 
        /// Return the number of records affected or -1 if not succeed.</summary>
        public int Exec(string _SqlStatement, bool _ErrorManagement = true, bool _ExecuteScalar = false)
        {
            Close();
            return Database.Exec(_SqlStatement, _ErrorManagement, _ExecuteScalar);
        }

        /// <summary>Perform stored procedure with parameters and return @Result parameter.</summary>
        public string StoredProcedure(string _StoredProcedure, object[] _Parameters = null, bool _ErrorManagement = true)
        {
            Close();
            return Database.StoredProcedure(_StoredProcedure, _Parameters, _ErrorManagement);
        }

        /// <summary>Return true if exists record changes in the buffer.</summary>
        public bool Modified()
        {
            if (Dataset != null) return Dataset.HasChanges();
            else return false;
        }

        /// <summary>Return true if dataset state is in insert or edit mode. If force is true, 
        /// dataset AutoEdit property is setted to true and the dataset is not in edit or insert mode
        /// the function try to set the edit mode. Returns true if succeesd.</summary>
        public bool Modifying(bool _ForceEdit)
        {
            if ((State == SMDatasetState.Insert) || (State == SMDatasetState.Edit)) return true;
            else if (_ForceEdit && (State == SMDatasetState.Browse) && (Row!=null)) return Edit();
            else return false;
        }

        /// <summary>Write a modified record to the buffer. Return true if succeed.</summary>
        public bool Post()
        {
            bool r = false, abort = false;
            if (BeforePost != null) BeforePost(this, ref abort);
            //
            if (abort) SM.Raise("SMDataSet: post operation aborted.", false);
            else if (ReadOnly) SM.Raise("SMDataSet: post cannot performed on readonly dataset.", false);
            else if (!Modifying(false)) SM.Raise("SMDataSet: post can performed only on editing or appending state dataset.", false);
            else
            {
                try
                {
                    if (Row.RowState == DataRowState.Added)
                    {
                        if (RecordInformationColumn)
                        {
                            if (IsField("InsertionDate")) Row["InsertionDate"] = DateTime.Now;
                            if (IsField("InsertionUser")) Row["InsertionUser"] = SM.User.IdUser;
                        }
                    }
                    else if (Row.RowState == DataRowState.Modified)
                    {
                        if (RecordInformationColumn)
                        {
                            if (IsField("ModificationDate")) Row["ModificationDate"] = DateTime.Now;
                            if (IsField("ModificationUser")) Row["ModificationUser"] = SM.User.IdUser;
                        }
                    }
                    if (UniqueIdentifierColumn)
                    {
                        if (SM.Empty(Row["ID"], true)) Row["ID"] = SM.UniqueId();
                    }
                    if (guidColumn)
                    {
                        if (SM.Empty(Row[GuidColumn], true)) Row[GuidColumn] = SM.GUID();
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
                if ((recordIndex > -1) && (recordIndex < Table.Rows.Count))
                {
                    Row = Table.Rows[recordIndex];
                    Bof = false;
                    Eof = false;
                    r = SkipDeletedForward();
                }
                else
                {
                    Row = null;
                    Bof = true;
                    Eof = true;
                }
                ChangeState(SMDatasetState.Browse);
                if (r) if (AfterPost != null) AfterPost(this);
            }
            return r;
        }

        /// <summary>Store in the current record fields values contained on s. 
        /// If blobs is true, blob fields will be stored in temporary directory and
        /// its names will be included on strings.</summary>
        public bool RecordFromJSON(string _JSONValue, string[] _ExcludeFields = null)
        {
            int i;
            bool r = false, b;
            string c;
            SMDictionary dict = new SMDictionary(SM);
            if ((Row != null) && dict.FromJSON(_JSONValue))
            {
                if (Modifying(false))
                {
                    i = 0;
                    r = true;
                    while (i < Table.Columns.Count)
                    {
                        if (!Table.Columns[i].AutoIncrement)
                        {
                            c = Table.Columns[i].ColumnName;
                            if ((_ExcludeFields == null) || (SM.Find(c, _ExcludeFields, true) < 0))
                            {
                                b = Table.Columns[i].DataType == System.Type.GetType("System.Byte[]");
                                if (b) b = Assign(c, SM.Base64DecodeBytes(dict.ValueOf(c)));
                                else b = Assign(c, dict.ValueOf(c));
                                if (!b) r = false;
                            }
                        }
                        i++;
                    }
                }
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
