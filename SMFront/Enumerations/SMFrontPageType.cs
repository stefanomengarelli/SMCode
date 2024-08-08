/*  ===========================================================================
 *  
 *  File:       SMFrontPageType.cs
 *  Version:    2.0.25
 *  Date:       June 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront page type enumeration.
 *
 *  ===========================================================================
 */

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront page type enumeration.</summary>
    public enum SMFrontPageType
    {
        /// <summary>None.</summary>
        None = -1,
        /// <summary>Record browse page.</summary>
        Browse = 0,
        /// <summary>Crud page.</summary>
        Crud = 1,
        /// <summary>Document flow page.</summary>
        Flow = 2
    }

    /* */

}
