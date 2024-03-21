/*  ===========================================================================
 *  
 *  File:       Database.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
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
