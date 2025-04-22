/*  ===========================================================================
 *  
 *  File:       SMOnLog.cs
 *  Version:    2.0.250
 *  Date:       April 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode delegate method for log event. 
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode delegate method for log event.</summary>
    public delegate bool SMOnLog(SMLogItem _LogItem, ref bool _Handled);

    /* */

}
