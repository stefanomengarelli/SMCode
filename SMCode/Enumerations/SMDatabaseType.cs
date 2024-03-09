/*  ===========================================================================
 *  
 *  File:       SMDatabaseType.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode database type enumeration.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode database type enumeration.</summary>
    public enum SMDatabaseType
    {
        /// <summary>None.</summary>
        None = -1,
        /// <summary>Microsoft Access (MDB).</summary>
        Access = 0,
        /// <summary>Microsoft SQL Server.</summary>
        Sql = 1,
        /// <summary>Microsoft SQL Server Express.</summary>
        SqlExpress = 2,
        /// <summary>MySQL Server.</summary>
        MySql = 3,
        /// <summary>DBase IV.</summary>
        DBase4 = 4
    }

    /* */

}
