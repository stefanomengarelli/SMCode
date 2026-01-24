/*  ===========================================================================
 *  
 *  File:       SMStringMatch.cs
 *  Version:    2.0.321
 *  Date:       January 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode log type enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode string matching enumeration.</summary>
    public enum SMStringMatch
    {
        /// <summary>String must be equal to second string.</summary>
        Equal = 0,
        /// <summary>String must start by second string.</summary>
        Start = 1,
        /// <summary>String must end by second string.</summary>
        End = 2,
        /// <summary>String must contains second string.</summary>
        Contains = 3
    }

    /* */

}
