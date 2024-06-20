/*  ===========================================================================
 *  
 *  File:       SMDatasetState.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dataset states enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode dataset states enumeration.</summary>
    public enum SMDatasetState
    {
        /// <summary>Closed.</summary>
        Closed = 0,
        /// <summary>Browse.</summary>
        Browse = 1,
        /// <summary>Insert.</summary>
        Insert = 2,
        /// <summary>Edit.</summary>
        Edit = 3,
        /// <summary>Delete.</summary>
        Delete = 4,
        /// <summary>Read.</summary>
        Read = 5
    };

    /* */

}
