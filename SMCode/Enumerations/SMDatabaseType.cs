/*  ===========================================================================
 *  
 *  File:       SMDatabaseType.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode database type enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode database type enumeration.</summary>
    public enum SMDatabaseType
    {
        /// <summary>None.</summary>
        None,
        /// <summary>Microsoft Access (MDB).</summary>
        Mdb,
        /// <summary>Microsoft SQL Server.</summary>
        Sql,
        /// <summary>MySQL Server.</summary>
        MySql,
        /// <summary>DBase IV (DBF).</summary>
        Dbf,
        /// <summary>PostgreSQL.</summary>
        PostgreSQL,
        /// <summary>Microsoft Access database (ACCDB).</summary>
        Accdb
    }

    /* */

}
