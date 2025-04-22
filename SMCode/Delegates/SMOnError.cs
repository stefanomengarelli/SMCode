/*  ===========================================================================
 *  
 *  File:       SMOnError.cs
 *  Version:    2.0.250
 *  Date:       April 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode delegate method for error event. 
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>Delegate method for error event.</summary>
    public delegate int SMOnError(string _ErrorMessage, Exception _Exception, ref bool _Handled);

    /* */

}
