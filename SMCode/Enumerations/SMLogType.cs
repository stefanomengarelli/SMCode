/*  ===========================================================================
 *  
 *  File:       SMLogType.cs
 *  Version:    2.0.50
 *  Date:       October 2024
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

namespace SMCodeSystem
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
        Debug = 4,
        /// <summary>Separator.</summary>
        Separator = 5,
        /// <summary>Line.</summary>
        Line = 6,
        /// <summary>Login.</summary>
        Login = 7,
        /// <summary>Event.</summary>
        Event = 8,
        /// <summary>Action.</summary>
        Action = 9
    }

    /* */

}
