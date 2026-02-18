/*  ===========================================================================
 *  
 *  File:       Database.cs
 *  Version:    2.0.322
 *  Date:       February 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: database.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: database.</summary>
    public partial class SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Database write log flag.</summary>
        public bool DatabaseLog { get; set; } = false;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return correct blank value for data column type.</summary>
        public object Blank(DataColumn _DataColumn)
        {
            if (_DataColumn.AutoIncrement) return 0;
            else if ((_DataColumn.MaxLength == UniqueIdLength) && (_DataColumn.ColumnName == "ID")) return UniqueId();
            else if (_DataColumn.DataType == System.Type.GetType("System.Boolean")) return false;
            else if (_DataColumn.DataType == System.Type.GetType("System.Byte")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.Char")) return ' ';
            else if (_DataColumn.DataType == System.Type.GetType("System.DateTime")) return DBNull.Value;
            else if (_DataColumn.DataType == System.Type.GetType("System.Decimal")) return 0.0d;
            else if (_DataColumn.DataType == System.Type.GetType("System.Double")) return 0.0d;
            else if (_DataColumn.DataType == System.Type.GetType("System.Int16")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.Int32")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.Int64")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.SByte")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.Single")) return 0.0f;
            else if (_DataColumn.DataType == System.Type.GetType("System.String")) return "";
            else if (_DataColumn.DataType == System.Type.GetType("System.TimeSpan")) return DBNull.Value;
            else if (_DataColumn.DataType == System.Type.GetType("System.UInt16")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.UInt32")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.UInt64")) return 0;
            else if (_DataColumn.DataType == System.Type.GetType("System.Guid")) return GUID();
            else return DBNull.Value;
        }

        /// <summary>Return date value for SQL database type expressions.</summary>
        public string Quote(DateTime _Value, SMDatabaseType _DatabaseType, bool _ReturnNullOnMinValue = true)
        {
            if (_DatabaseType == SMDatabaseType.Sql)
            {
                // ISO 8601 
                if (_ReturnNullOnMinValue && (_Value <= DateTime.MinValue)) return "NULL";
                else return Quote(Zeroes(_Value.Year, 4) + "-" + Zeroes(_Value.Month, 2) + "-" + Zeroes(_Value.Day, 2)
                       + "T" + Zeroes(_Value.Hour, 2) + ":" + Zeroes(_Value.Minute, 2) + ":" + Zeroes(_Value.Second, 2));
            }
            else if (_DatabaseType == SMDatabaseType.MySql)
            {
                if (_ReturnNullOnMinValue && (_Value <= DateTime.MinValue)) return "NULL";
                else return Quote(Zeroes(_Value.Year, 4) + "-" + Zeroes(_Value.Month, 2) + "-" + Zeroes(_Value.Day, 2)
                       + " " + Zeroes(_Value.Hour, 2) + ":" + Zeroes(_Value.Minute, 2) + ":" + Zeroes(_Value.Second, 2));
            }
            else if (_ReturnNullOnMinValue && (_Value <= DateTime.MinValue)) return "NULL";
            else return "#" + Zeroes(_Value.Month, 2) + "-" + Zeroes(_Value.Day, 2) + "-" + Zeroes(_Value.Year, 4)
                       + " " + Zeroes(_Value.Hour, 2) + "." + Zeroes(_Value.Minute, 2) + "." + Zeroes(_Value.Second, 2) + "#";
        }

        /// <summary>Return integer value as constant for SQL expressions.</summary>
        public string Quote(int _Value)
        {
            return _Value.ToString();
        }

        /// <summary>Return double value as constant for SQL expressions.</summary>
        public string Quote(double _Value)
        {
            return _Value.ToString("###############0.############").Replace(",", ".");
        }

        /// <summary>Return decimal value as constant for SQL expressions.</summary>
        public string Quote(decimal _Value)
        {
            return _Value.ToString("###############0.############").Replace(",", ".");
        }

        /// <summary>Return database identifier name with type database SQL syntax delimiters.</summary>
        public string QuoteIdentifier(string _IdentifierName, SMDatabaseType _DatabaseType)
        {
            _IdentifierName = UnquoteIdentifier(_IdentifierName);
            if ((_DatabaseType == SMDatabaseType.Accdb) || (_DatabaseType == SMDatabaseType.Mdb) || (_DatabaseType == SMDatabaseType.Sql))
            {
                return SMDatabase.SqlPrefix + _IdentifierName + SMDatabase.SqlSuffix;
            }
            else if (_DatabaseType == SMDatabaseType.MySql)
            {
                return SMDatabase.MySqlPrefix + _IdentifierName + SMDatabase.MySqlSuffix;
            }
            else return _IdentifierName;
        }

        /// <summary>Return database identifier with type database SQL syntax delimiters.</summary>
        public string QuoteIdentifier(string[] _IdentifiersNames, SMDatabaseType _DatabaseType)
        {
            int i = 0;
            string r = "";
            if (_IdentifiersNames != null)
            {
                while (i < _IdentifiersNames.Length)
                {
                    if (i > 0) r += ",";
                    r += QuoteIdentifier(_IdentifiersNames[i], _DatabaseType);
                    i++;
                }
            }
            return r;
        }

        /// <summary>Return string containing SQL parameterized syntax for dataset DS delete command.
        /// Require use of unique IDs.</summary>
        public string SqlCommandDelete(SMDataset _Dataset)
        {
            return "DELETE FROM " + QuoteIdentifier(_Dataset.TableName, _Dataset.Database.Type)
                + " WHERE " + QuoteIdentifier("ID", _Dataset.Database.Type) + "=@ID";
        }

        /// <summary>Return string containing SQL parameterized syntax for dataset insert command.
        /// Require use of unique IDs.</summary>
        public string SqlCommandInsert(SMDataset _Dataset)
        {
            string q = "";
            StringBuilder r = new StringBuilder(), v = new StringBuilder();
            r.Append("INSERT INTO ");
            r.Append(QuoteIdentifier(_Dataset.TableName, _Dataset.Database.Type));
            r.Append(" (");
            for (int i = 0; i < _Dataset.Table.Columns.Count; i++)
            {
                if (!_Dataset.Table.Columns[i].AutoIncrement)
                {
                    r.Append(q);
                    r.Append(QuoteIdentifier(_Dataset.Table.Columns[i].ColumnName, _Dataset.Database.Type));
                    v.Append(q);
                    v.Append('@');
                    v.Append(_Dataset.Table.Columns[i].ColumnName);
                    q = ",";
                }
            }
            r.Append(") VALUES(");
            r.Append(v.ToString());
            r.Append(')');
            return r.ToString();
        }

        /// <summary>Return string containing SQL parameterized syntax for dataset update command.
        /// Require use of unique IDs.</summary>
        public string SqlCommandUpdate(SMDataset _Dataset)
        {
            string q = "";
            StringBuilder r = new StringBuilder();
            r.Append("UPDATE ");
            r.Append(QuoteIdentifier(_Dataset.TableName, _Dataset.Database.Type));
            r.Append(" SET ");
            for (int i = 0; i < _Dataset.Table.Columns.Count; i++)
            {
                if (!_Dataset.Table.Columns[i].AutoIncrement)
                {
                    r.Append(q);
                    r.Append(QuoteIdentifier(_Dataset.Table.Columns[i].ColumnName, _Dataset.Database.Type));
                    r.Append(@"=@");
                    r.Append(_Dataset.Table.Columns[i].ColumnName);
                    q = ",";
                }
            }
            r.Append(" WHERE ");
            r.Append(QuoteIdentifier("ID", _Dataset.Database.Type));
            r.Append(@"=@ID");
            return r.ToString();
        }

        /// <summary>Executes SQL statement passed on database with alias. Is statement start by SELECT
        /// function will return integer value of result of first column of first row 
        /// else will return the number of records affected or -1 if not succeed.</summary>
        public int SqlExec(string _SqlStatement, string _Alias = "MAIN", bool _ErrorManagement = true, bool _ExecuteScalar = false)
        {
            SMDatabase db;
            if (!Empty(_Alias))
            {
                db = Databases.Keep(_Alias);
                if (db != null) return db.Exec(_SqlStatement, _ErrorManagement, _ExecuteScalar);
                else return -1;
            }
            else return -1;
        }

        /// <summary>Return optimized SQL field list, if one of field is * return *.</summary>
        public string SqlFields(string _FieldList)
        {
            int i;
            string r = "", c;
            List<string> f = new List<string>();
            while (_FieldList.Trim().Length > 0)
            {
                c = Extract(ref _FieldList, ",; ").Trim();
                if (c == "*")
                {
                    r = "*";
                    _FieldList = "";
                }
                else if ((c.Length > 0) && (f.IndexOf(c) < 0)) f.Add(c);
            }
            if (r == "")
            {
                for (i = 0; i < f.Count; i++)
                {
                    if (i > 0) r += ",";
                    r += f[i];
                }
            }
            return r;
        }

        /// <summary>Return INSERT INTO statement with table and fields and values (with delimiters) passed.</summary>
        public string SqlInsert(string _TableName, string[] _FieldValueArray)
        {
            int i = 0;
            bool b = false;
            StringBuilder r = new StringBuilder("INSERT INTO " + _TableName + " ("), f = new StringBuilder(), v = new StringBuilder();
            while (i < _FieldValueArray.Length - 1)
            {
                if (!Empty(_FieldValueArray[i]) && !Empty(_FieldValueArray[i + 1]))
                {
                    if (b)
                    {
                        f.Append(',');
                        v.Append(',');
                    }
                    f.Append(_FieldValueArray[i]);
                    v.Append(_FieldValueArray[i + 1]);
                    b = true;
                }
                i += 2;
            }
            r.Append(f.ToString());
            r.Append(") VALUES (");
            r.Append(v.ToString());
            r.Append(')');
            return r.ToString();
        }

        /// <summary>Return string with SQL expression for INSTR of expr, below database type.</summary>
        public string SqlInstr(string _SQLExpression, SMDatabaseType _DatabaseType)
        {
            if (_SQLExpression.Trim().Length > 0)
            {
                if (_DatabaseType == SMDatabaseType.MySql) return "INSTR(" + _SQLExpression.Trim() + ")";
                else if (_DatabaseType == SMDatabaseType.Sql) return "CHARINDEX(" + _SQLExpression.Trim() + ")";
                else return "Instr(" + _SQLExpression + ")";
            }
            else return "";
        }

        /// <summary>Return string with SQL expression for ISNULL, below database type.</summary>
        public string SqlIsNull(string _SQLExpression, SMDatabaseType _DatabaseType)
        {
            if (_SQLExpression.Trim().Length > 0)
            {
                if (_DatabaseType == SMDatabaseType.MySql) return "(" + _SQLExpression.Trim() + ") IS NULL";
                else if (_DatabaseType == SMDatabaseType.Sql) return "(" + _SQLExpression.Trim() + ") IS NULL";
                else return "IsNull(" + _SQLExpression + ")";
            }
            else return "";
        }

        /// <summary>Return string with SQL expression for LOWERCASE, below database type.</summary>
        public string SqlLower(string _SQLExpression, SMDatabaseType _DatabaseType)
        {
            if (_SQLExpression.Trim().Length > 0)
            {
                if (_DatabaseType == SMDatabaseType.MySql) return "LCASE(" + _SQLExpression.Trim() + ")";
                else if (_DatabaseType == SMDatabaseType.Sql) return "LOWER(" + _SQLExpression.Trim() + ")";
                else return "LCase(" + _SQLExpression + ")";
            }
            else return "";
        }

        /// <summary>Return string replacing SM SQL Macros with corresponding database SQL statement.
        /// Macros: SM_ISNULL(expr), SM_UPPER(expr), SM_LOWER(expr), SM_TRIM(expr), SM_INSTR(expr,substring),
        /// SM_DATETIME(date), SM_USER(), SM_NOW().</summary>
        public string SqlMacros(string _SQLStatement, SMDatabaseType _DatabaseType)
        {
            string a, m, q;
            //
            // macro SM_ISNULL()
            //
            m = "SM_ISNULL";
            a = BtwU(_SQLStatement, m + "(", ")").Trim();
            while (a.Length > 0)
            {
                q = SqlIsNull(a, _DatabaseType);
                _SQLStatement = _SQLStatement.Replace(m + "(" + a + ")", "(" + q + ")");
                a = Btw(_SQLStatement, m + "(", ")").Trim();
            }
            //
            // macro SM_UPPER()
            //
            m = "SM_UPPER";
            a = BtwU(_SQLStatement, m + "(", ")").Trim();
            while (a.Length > 0)
            {
                q = SqlUpper(a, _DatabaseType);
                _SQLStatement = _SQLStatement.Replace(m + "(" + a + ")", "(" + q + ")");
                a = Btw(_SQLStatement, m + "(", ")").Trim();
            }
            //
            // macro SM_LOWER()
            //
            m = "SM_LOWER";
            a = BtwU(_SQLStatement, m + "(", ")").Trim();
            while (a.Length > 0)
            {
                q = SqlLower(a, _DatabaseType);
                _SQLStatement = _SQLStatement.Replace(m + "(" + a + ")", "(" + q + ")");
                a = Btw(_SQLStatement, m + "(", ")").Trim();
            }
            //
            // macro SM_TRIM()
            //
            m = "SM_TRIM";
            a = BtwU(_SQLStatement, m + "(", ")").Trim();
            while (a.Length > 0)
            {
                q = SqlTrim(a, _DatabaseType);
                _SQLStatement = _SQLStatement.Replace(m + "(" + a + ")", "(" + q + ")");
                a = Btw(_SQLStatement, m + "(", ")").Trim();
            }
            //
            // macro SM_INSTR()
            //
            m = "SM_INSTR";
            a = BtwU(_SQLStatement, m + "(", ")").Trim();
            while (a.Length > 0)
            {
                q = SqlInstr(a, _DatabaseType);
                _SQLStatement = _SQLStatement.Replace(m + "(" + a + ")", "(" + q + ")");
                a = Btw(_SQLStatement, m + "(", ")").Trim();
            }
            //
            // macro SM_DATETIME()
            //
            m = "SM_DATETIME";
            a = BtwU(_SQLStatement, m + "(", ")").Trim();
            while (a.Length > 0)
            {
                q = Quote(ToDate(a), _DatabaseType, false);
                _SQLStatement = _SQLStatement.Replace(m + "(" + a + ")", "(" + q + ")");
                a = Btw(_SQLStatement, m + "(", ")").Trim();
            }
            //
            // macro SM_SYSUSER()
            //
            m = "SM_USER()";
            while (_SQLStatement.IndexOf(m) > -1)
            {
                _SQLStatement = _SQLStatement.Replace(m, Quote("???"));
            }
            //
            // macro SM_NOW()
            //
            m = "SM_NOW()";
            while (_SQLStatement.IndexOf(m) > -1)
            {
                _SQLStatement = _SQLStatement.Replace(m, Quote(DateTime.Now, _DatabaseType, false));
            }
            //
            // ritorna il risultato
            //
            return _SQLStatement;
        }

        /// <summary>Return result field value of table first record with field greather than value related to database alias.</summary>
        public string SqlNext(SMCode _SM, string _Alias, string _TableName, string _OrderColumn, string _Value, string _ResultColumn = "", string _IdColumn = "ID", bool _ExclusiveDatabase = false)
        {
            string r = "", sql;
            SMDataset ds;
            if (_Alias.Trim().Length < 1) _Alias = "MAIN";
            if (Empty(_ResultColumn)) _ResultColumn = _OrderColumn;
            try
            {
                ds = new SMDataset(_Alias, SM, _ExclusiveDatabase);
                sql = $"SELECT {FixList($"{_IdColumn},{_OrderColumn},{_ResultColumn}")} FROM {_TableName}"
                    + $" WHERE {_OrderColumn}>{Quote(_Value)} ORDER BY {_OrderColumn}";
                if (ds.Open(sql))
                {
                    if (!ds.Eof) r = ds.FieldStr(_ResultColumn);
                }
                ds.Close();
                ds.Dispose();
            }
            catch (Exception ex)
            {
                Error(ex);
                r = "";
            }
            return r;
        }

        /// <summary>Return not delete expression by deleted column name.</summary>
        public string SqlNotDeleted(string _DeletedColumn = "Deleted", string _TableName = "", string _NotDeletedExpr = "0")
        {
            if (Empty(_DeletedColumn)) return "";
            else
            {
                if (!Empty(_TableName)) _DeletedColumn = $"{_TableName}.{_DeletedColumn}";
                return $"(({_DeletedColumn} IS NULL)OR({_DeletedColumn}={_NotDeletedExpr}))";
            }
        }

        /// <summary>Return result field value of table first record with field less than value related to database alias.</summary>
        public string SqlPrior(SMCode _SM, string _Alias, string _TableName, string _OrderColumn, string _Value, string _ResultColumn = "", string _IdColumn = "ID", bool _ExclusiveDatabase = false)
        {
            string r = "";
            SMDataset ds;
            if (_Alias.Trim().Length < 1) _Alias = "MAIN";
            if (Empty(_ResultColumn)) _ResultColumn = _OrderColumn;
            try
            {
                ds = new SMDataset(_Alias, SM, _ExclusiveDatabase);
                if (ds.Open("SELECT " + FixList(_IdColumn + "," + _OrderColumn + "," + _ResultColumn)
                    + " FROM " + _TableName
                    + " WHERE " + _OrderColumn + "<" + Quote(_Value)
                    + " ORDER BY " + _OrderColumn + " DESC"))
                {
                    if (!ds.Eof) r = ds.FieldStr(_ResultColumn);
                }
                ds.Close();
                ds.Dispose();
            }
            catch (Exception ex)
            {
                Error(ex);
                r = "";
            }
            return r;
        }

        /// <summary>Return list of type specified containing result of query passed.</summary>
        public List<T> SqlQuery<T>(string _SQLExpression, string _Alias = "MAIN", bool _EsclusiveConnection = true) where T : new()
        {
            bool loop = true;
            List<T> rslt = null;
            SMDataset ds;
            T item;
            ds = SqlQuery(_SQLExpression, _Alias, _EsclusiveConnection);
            if (ds != null)
            {
                rslt = new List<T>();
                while (!ds.Eof && loop)
                {
                    item = new T();
                    loop = ds.RecordToObject(item);
                    rslt.Add(item);
                    ds.Next();
                }
                ds.Close();
                ds.Dispose();
                if (!loop)
                {
                    rslt = null;
                    Error(new Exception($"Error on item assign {ExceptionMessage}"));
                }
            }
            return rslt;
        }

        /// <summary>Return dataset with result of query passed. Dataset is open and ready to use. Return null if error occurred.</summary>
        public SMDataset SqlQuery(string _SQLExpression, string _Alias = "MAIN", bool _ExclusiveConnection = false)
        {
            SMDataset ds = null;
            if (Empty(_Alias)) _Alias = "MAIN";
            try
            {
                ds = new SMDataset(_Alias, this, _ExclusiveConnection);
                if (!ds.Open(_SQLExpression))
                {
                    Error(new Exception($"Can't open query {_SQLExpression}"));
                    ds.Close();
                    ds.Dispose();
                    ds = null;
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                if (ds != null)
                {
                    ds.Close();
                    ds.Dispose();
                    ds = null;
                }
            }
            return ds;
        }

        /// <summary>Return table name from SQL selection statement.</summary>
        public string SqlTable(string _SqlSelectStatement)
        {
            return UnquoteIdentifier(BtwU(_SqlSelectStatement + " ", " from ", " ").Trim()).Trim();
        }

        /// <summary>Return string with SQL expression for TRIM, below database type.</summary>
        public string SqlTrim(string _SQLExpression, SMDatabaseType _DatabaseType)
        {
            if (_SQLExpression.Trim().Length > 0)
            {
                if (_DatabaseType == SMDatabaseType.MySql) return "TRIM(" + _SQLExpression.Trim() + ")";
                else if (_DatabaseType == SMDatabaseType.Sql) return "LTRIM(RTRIM(" + _SQLExpression.Trim() + "))";
                else return "Trim(" + _SQLExpression + ")";
            }
            else return "";
        }

        /// <summary>Update table with item passed, using table name and alias. Return -1 if error occurred, 
        /// 0 if no record changed, 1 for record modified, 2 for record append, 3 for soft deletion, 4 for hard deletion.</summary>
        public int SqlUpdate<T>(T _Item, string _Alias = "MAIN", bool _UpdateOnlyChanged = true, bool _ErrorManagement = true, bool _CloseDatabase = true, string _DeletedProperty = "Deleted")
        {
            int i, rslt = -1;
            bool deletion = false;
            object vo;
            string tb, vl, guid;
            string[] pk;
            StringBuilder expr = new StringBuilder();
            Type type, ty;
            SMDataset ds;
            SMDatabase db;
            if (_Item != null)
            {
                if (!Empty(_Alias))
                {
                    // get item type
                    type = _Item.GetType();
                    // test if item changed or deleted
                    deletion = ToBool(type.GetProperty(_DeletedProperty).GetValue(_Item));
                    guid = ToStr(type.GetProperty("_GuidColumn").GetValue(_Item)).Trim();
                    if (!_UpdateOnlyChanged || deletion
                        || ToBool(type.GetProperty("_IsChanged").GetValue(_Item)))
                    {
                        // get table name 
                        tb = ToStr(type.GetProperty("_TableName").GetValue(_Item));
                        if (!Empty(tb))
                        {
                            // get primary key
                            pk = (string[])type.GetProperty("_PrimaryKey").GetValue(_Item);
                            if (pk != null)
                            {
                                // build SQL expression for primary key
                                for (i = 0; i < pk.Length; i++)
                                {
                                    vl = "";
                                    ty = type.GetProperty(pk[i]).PropertyType;
                                    if (ty.IsGenericType && (ty.GetGenericTypeDefinition() == typeof(Nullable<>)))
                                    {
                                        ty = ty.GetGenericArguments()[0];
                                    }
                                    vo = type.GetProperty(pk[i]).GetValue(_Item);
                                    if (vo == null)
                                    {
                                        vl = " IS NULL";
                                    }
                                    else if (SMDataType.IsText(ty))
                                    {
                                        vl = '='+Quote(ToStr(vo));
                                    }
                                    else if (SMDataType.IsNumeric(ty))
                                    {
                                        vl = '=' + ToStr(vo).Replace(DecimalSeparator, '.');
                                    }
                                    else if (SMDataType.IsDate(ty))
                                    {
                                        vl = '=' + Quote(ToStr(ToDate(vo), SMDateFormat.iso8601, true));
                                    }
                                    else if (SMDataType.IsBoolean(ty))
                                    {
                                        vl = '=' + ToBool(ToBool(ToStr(vo)));
                                    }
                                    else if (SMDataType.IsGuid(ty))
                                    {
                                        vl = '=' + Quote(((Guid)vo).ToString());
                                    }
                                    if (vl != "")
                                    {
                                        if (expr.Length > 0) expr.Append(" AND ");
                                        expr.Append('(');
                                        expr.Append(pk[i]);
                                        expr.Append(vl);
                                        expr.Append(')');
                                    }
                                }
                                // open database
                                db = Databases.Keep(_Alias);
                                if (db != null)
                                {
                                    // open dataset
                                    ds = new SMDataset(db, this);
                                    ds.GuidColumn = guid;
                                    if (ds.Open($"SELECT * FROM {tb} WHERE {expr.ToString()}"))
                                    {
                                        rslt = 0;
                                        // test hard deletion case
                                        if (deletion && !ds.IsField(_DeletedProperty))
                                        {
                                            if (!ds.Eof)
                                            {
                                                if (ds.Delete()) rslt = 4; // hard deletion
                                                else
                                                {
                                                    rslt = -1;
                                                    if (_ErrorManagement) Error(new Exception($"Can't delete record on table {Quote(tb)}."));
                                                }
                                            }
                                        }
                                        // test append or edit case
                                        else if (ds.Eof)
                                        {
                                            if (!deletion)
                                            {
                                                if (ds.Append()) rslt = 2; // append record
                                                else
                                                {
                                                    if (_ErrorManagement) Error(new Exception($"Can't append record on table {Quote(tb)}."));
                                                    rslt = -1;
                                                }
                                            }
                                        }
                                        else if (ds.Edit())
                                        {
                                            if (deletion) rslt = 3; // soft deletion
                                            else rslt = 1; // edit record
                                        }
                                        else
                                        {
                                            if (_ErrorManagement) Error(new Exception($"Can't edit record on table {Quote(tb)}."));
                                            rslt = -1;
                                        }
                                        // assign item to dataset row
                                        if (rslt > 0)
                                        {
                                            if (ds.RecordFromObject(_Item))
                                            {
                                                if (deletion && (rslt == 3))
                                                {
                                                    if (ds.IsField(SMDataset.DeletionDateColumn)) ds.Assign(SMDataset.DeletionDateColumn, DateTime.Now);
                                                    if ((User != null) && ds.IsField(SMDataset.DeletionUserColumn)) ds.Assign(SMDataset.DeletionUserColumn, User.IdUser);
                                                }
                                                if (ds.Post())
                                                {
                                                    if ((rslt == 2) && (guid.Length > 0))
                                                    {
                                                        if (ds.Open($"SELECT * FROM {tb} WHERE {guid}={Quote(ds.FieldStr(guid))}"))
                                                        {
                                                            if (!ds.Eof) ds.RecordToObject(_Item);
                                                        }
                                                        else if (_ErrorManagement) Error(new Exception($"Can't open record on table {Quote(tb)} with GUID {guid}."));
                                                    }
                                                }
                                                else
                                                {
                                                    rslt = -1;
                                                    if (_ErrorManagement) Error(new Exception($"Can't post record on table {Quote(tb)}."));
                                                    ds.Cancel();
                                                }
                                            }
                                            else
                                            {
                                                rslt = -1;
                                                if (_ErrorManagement) Error(new Exception($"Can't assign record on table {Quote(tb)}."));
                                                ds.Cancel();
                                            }
                                        }
                                    }
                                    else if (_ErrorManagement) Error(new Exception($"Can't open table {Quote(tb)}."));
                                    ds.Close();
                                    ds.Dispose();
                                    if (_CloseDatabase) db.Close();
                                }
                                else if (_ErrorManagement) Error(new Exception("Can't keep database alias."));
                            }
                            else if (_ErrorManagement) Error(new Exception("Primary key not defined for item."));
                        }
                        else if (_ErrorManagement) Error(new Exception("Table name not defined for item."));
                    }
                    else rslt = 0; // no change, no update, no delete
                }
                else if (_ErrorManagement) Error(new Exception("Missing database alias."));
            }
            else if (_ErrorManagement) Error(new Exception("Missing item or item is null."));
            return rslt;
        }

        /// <summary>Update table with items passed, using table name and alias.</summary>
        public int SqlUpdate<T>(List<T> _Items, string _Alias = "MAIN", bool _UpdateOnlyChanged = true, bool _ErrorManagement = true, bool _CloseDatabase = true)
        {
            int i, r;
            int rslt = -1;
            if (!Empty(_Alias) && (_Items != null))
            {
                i = 0;
                rslt = 0;
                while (i < _Items.Count)
                {
                    r = SqlUpdate(_Items[i], _Alias, _UpdateOnlyChanged, _ErrorManagement, false);
                    if (r > 0) rslt++;
                    if ((r == 3) || (r == 4)) _Items.RemoveAt(i);
                    else i++;
                }
                if (_CloseDatabase) Databases.Close(_Alias);
            }
            return rslt;
        }

        /// <summary>Return string with SQL expression for UPPERCASE, below database type.</summary>
        public string SqlUpper(string _SQLExpression, SMDatabaseType _DatabaseType)
        {
            if (_SQLExpression.Trim().Length > 0)
            {
                if (_DatabaseType == SMDatabaseType.MySql) return "UCASE(" + _SQLExpression.Trim() + ")";
                else if (_DatabaseType == SMDatabaseType.Sql) return "UPPER(" + _SQLExpression.Trim() + ")";
                else return "UCase(" + _SQLExpression + ")";
            }
            else return "";
        }

        /// <summary>Executes SQL stored procedure passed with parameters on database with alias.</summary>
        public string SqlStoredProcedure(string _StoredProcedure, object[] _Parameters=null, string _Alias = "MAIN", bool _ErrorManagement = true, bool _CloseDatabase = true)
        {
            string rslt = "KO:Missing alias.";
            SMDatabase db;
            if (!Empty(_Alias))
            {
                db = Databases.Keep(_Alias);
                if (db != null)
                {
                    rslt = db.StoredProcedure(_StoredProcedure, _Parameters, _ErrorManagement);
                    if (_CloseDatabase) db.Close();
                }
                else rslt = "KO:Can't keep database alias.";
            }
            return rslt;
        }

        /// <summary>Return field name without database SQL syntax delimiters.</summary>
        public string UnquoteIdentifier(string _FieldName)
        {
            _FieldName = _FieldName.Trim();
            if (_FieldName.StartsWith("[")
                || _FieldName.StartsWith(SMDatabase.SqlPrefix)
                || _FieldName.StartsWith(SMDatabase.MySqlPrefix))
            {
                if (_FieldName.Length > 1) _FieldName = _FieldName.Substring(1);
                else _FieldName = "";
            }
            if (_FieldName.EndsWith("]")
                || _FieldName.EndsWith(SMDatabase.SqlSuffix)
                || _FieldName.EndsWith(SMDatabase.MySqlSuffix))
            {
                if (_FieldName.Length > 1) _FieldName = _FieldName.Substring(0, _FieldName.Length - 1);
                else _FieldName = "";
            }
            return _FieldName.Trim();
        }

        #endregion

        /* */

    }

    /* */

}
