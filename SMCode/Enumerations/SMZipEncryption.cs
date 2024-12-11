/*  ===========================================================================
 *  
 *  File:       SMZipEncryption.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2023 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode zip encryption mode enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode zip encryption mode enumeration.</summary>
    public enum SMZipEncryption
    {
        /// <summary>Default encryption.</summary>
        Default,
        /// <summary>AES 128 bit encryption.</summary>
        AES128,
        /// <summary>AES 256 bit encryption.</summary>
        AES256
    };

    /* */

}
