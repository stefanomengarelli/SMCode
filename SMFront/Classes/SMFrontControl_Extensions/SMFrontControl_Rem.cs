/*  ===========================================================================
 *  
 *  File:       SMFrontControl_Rem.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront web control base class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System.Text;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront web control base class.</summary>
    public partial class SMFrontControl
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Built in remark control render.</summary>
        public void RenderControlBuiltIn_Remark(object _Sender, StringBuilder _Code, SMFrontControlType _ControlType)
        {
            _Code.Append("\t<!-- " + Text + " -->\r\n");
        }

        #endregion

        /* */

    }

    /* */

}
