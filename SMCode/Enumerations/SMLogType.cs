/*  ===========================================================================
 *  
 *  File:       SMLogType.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode log type enumeration.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode log type enumeration.</summary>
    public enum SMLogType
    {
        /// <summary>None.</summary>
        None = 0,
        /// <summary>Information.</summary>
        Information = 1,
        /// <summary>Warning.</summary>
        Warning = 2,
        /// <summary>Error.</summary>
        Error = 3,
        /// <summary>Debug error.</summary>
        Debug = 4
    }

    /* */

}
