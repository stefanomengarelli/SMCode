/*  ===========================================================================
 *  
 *  File:       SMControlResizeMode.cs
 *  Version:    2.0.45
 *  Date:       September 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMDesk UI control resize mode enumeration.
 *  
 *  ===========================================================================
 */

namespace SMDeskSystem
{

    /* */

    /// <summary>SMDesk UI control resize mode enumeration.</summary>
    public enum SMControlResizeMode
    {
        /// <summary>No resize.</summary>
        None = 0,
        /// <summary>Resize S-E.</summary>
        SizeSE,
        /// <summary>Resize N.</summary>
        SizeN,
        /// <summary>Resize E.</summary>
        SizeE,
        /// <summary>Resize S.</summary>
        SizeS,
        /// <summary>Resize W.</summary>
        SizeW,
        /// <summary>Move.</summary>
        Move
    }

    /* */

}
