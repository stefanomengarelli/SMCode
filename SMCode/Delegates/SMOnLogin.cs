/*  ===========================================================================
 *  
 *  File:       SMOnLogin.cs
 *  Version:    2.0.54
 *  Date:       October 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode delegate method for login event. 
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode delegate method for login event.</summary>
    public delegate bool SMOnLogin(SMLogItem _LogItem, SMUser _User, ref bool _Validate);

    /* */

}
