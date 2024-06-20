/*  ===========================================================================
 *  
 *  File:       SMParserAtomType.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode parser atom type enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode parser atom type enumeration.</summary>
    public enum SMParserAtomType
    {
        /// <summary>Variable atom.</summary>
        Variable,
        /// <summary>Value atom.</summary>
        Value,
        /// <summary>Operator atom.</summary>
        Operator,
        /// <summary>Function atom.</summary>
        Function,
        /// <summary>Result atom.</summary>
        Result,
        /// <summary>Bracket atom.</summary>
        Bracket,
        /// <summary>Comma atom.</summary>
        Comma,
        /// <summary>Error atom.</summary>
        Error,
        /// <summary>None atom.</summary>
        None
    }

    /* */

}
