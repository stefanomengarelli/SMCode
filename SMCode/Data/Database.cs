/*  ===========================================================================
 *  
 *  File:       Database.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: database.
 *
 *  ===========================================================================
 */

using System.Data;
using System.Text;

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: database.</summary>
    public partial class SMApplication
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Database connections collection.</summary>
        public SMDatabases Databases { get; set; } = null;

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
            else return DBNull.Value;
        }

        /// <summary>Return string containing SQL parameterized syntax for dataset DS delete command.
        /// Require use of unique IDs.</summary>
        public string DeleteCommandString(SMDataset _Dataset)
        {
            return "DELETE FROM " + QuoteField(_Dataset.TableName, _Dataset.Database.Type)
                + " WHERE " + QuoteField("ID", _Dataset.Database.Type) + "=@ID";
        }

        /// <summary>Return string containing SQL parameterized syntax for dataset insert command.
        /// Require use of unique IDs.</summary>
        public string InsertCommandString(SMDataset _Dataset)
        {
            string q = "";
            StringBuilder r = new StringBuilder(), v = new StringBuilder();
            r.Append("INSERT INTO ");
            r.Append(QuoteField(_Dataset.TableName, _Dataset.Database.Type));
            r.Append(" (");
            for (int i = 0; i < _Dataset.Table.Columns.Count; i++)
            {
                if (!_Dataset.Table.Columns[i].AutoIncrement)
                {
                    r.Append(q);
                    r.Append(QuoteField(_Dataset.Table.Columns[i].ColumnName, _Dataset.Database.Type));
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

        /// <summary>Return dateValue with dbType database SQL syntax delimiters.</summary>
        public string Quote(DateTime _Value, SMDatabaseType _DatabaseType)
        {
            if (_DatabaseType == SMDatabaseType.Sql)
            {
                // ISO 8601 
                return Quote(Zeroes(_Value.Year, 4) + "-" + Zeroes(_Value.Month, 2) + "-" + Zeroes(_Value.Day, 2)
                       + "T" + Zeroes(_Value.Hour, 2) + ":" + Zeroes(_Value.Minute, 2) + ":" + Zeroes(_Value.Second, 2));
            }
            else if (_DatabaseType == SMDatabaseType.MySql)
            {
                return Quote(Zeroes(_Value.Year, 4) + "-" + Zeroes(_Value.Month, 2) + "-" + Zeroes(_Value.Day, 2)
                       + " " + Zeroes(_Value.Hour, 2) + ":" + Zeroes(_Value.Minute, 2) + ":" + Zeroes(_Value.Second, 2));
            }
            else return "#" + Zeroes(_Value.Month, 2) + "-" + Zeroes(_Value.Day, 2) + "-" + Zeroes(_Value.Year, 4)
                       + " " + Zeroes(_Value.Hour, 2) + "." + Zeroes(_Value.Minute, 2) + "." + Zeroes(_Value.Second, 2) + "#";
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
                q = Quote(ToDate(a), _DatabaseType);
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
                _SQLStatement = _SQLStatement.Replace(m, Quote(DateTime.Now, _DatabaseType));
            }
            //
            // ritorna il risultato
            //
            return _SQLStatement;
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

        /// <summary>Return field name with type database SQL syntax delimiters.</summary>
        public string QuoteField(string _FieldName, SMDatabaseType _DatabaseType)
        {
            _FieldName = UnquoteField(_FieldName);
            if ((_DatabaseType == SMDatabaseType.Mdb) || (_DatabaseType == SMDatabaseType.Sql))
            {
                return SMDatabase.SqlPrefix + _FieldName + SMDatabase.SqlSuffix;
            }
            else if (_DatabaseType == SMDatabaseType.MySql)
            {
                return SMDatabase.MySqlPrefix + _FieldName + SMDatabase.MySqlSuffix;
            }
            else return _FieldName;
        }

        /// <summary>Return field name without database SQL syntax delimiters.</summary>
        public string UnquoteField(string _FieldName)
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

        /// <summary>Return string containing SQL parameterized syntax for dataset update command.
        /// Require use of unique IDs.</summary>
        public string UpdateCommandString(SMDataset _Dataset)
        {
            string q = "";
            StringBuilder r = new StringBuilder();
            r.Append("UPDATE ");
            r.Append(QuoteField(_Dataset.TableName, _Dataset.Database.Type));
            r.Append(" SET ");
            for (int i = 0; i < _Dataset.Table.Columns.Count; i++)
            {
                if (!_Dataset.Table.Columns[i].AutoIncrement)
                {
                    r.Append(q);
                    r.Append(QuoteField(_Dataset.Table.Columns[i].ColumnName, _Dataset.Database.Type));
                    r.Append(@"=@");
                    r.Append(_Dataset.Table.Columns[i].ColumnName);
                    q = ",";
                }
            }
            r.Append(" WHERE ");
            r.Append(QuoteField("ID", _Dataset.Database.Type));
            r.Append(@"=@ID");
            return r.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
