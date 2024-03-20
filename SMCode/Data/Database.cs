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

        #endregion

        /* */

    }

    /* */

}
