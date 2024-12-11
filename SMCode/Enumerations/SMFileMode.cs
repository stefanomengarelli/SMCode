/*  ===========================================================================
 *  
 *  File:       SMFileMode.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode file mode enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode file mode enumeration.</summary>
    public enum SMFileMode
    {
        /// <summary>None.</summary>
        None = -1,
        /// <summary>Open read mode.</summary>
        Read = 0,
        /// <summary>Open write mode.</summary>
        Write = 1,
        /// <summary>Open append mode.</summary>
        Append = 2,
        /// <summary>Random access (read/write) mode.</summary>
        RandomAccess = 3
    }

    /* */

}
