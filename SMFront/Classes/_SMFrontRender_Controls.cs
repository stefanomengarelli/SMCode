/*  ===========================================================================
 *  
 *  File:       SMFrontRender_Controls.cs
 *  Version:    2.0.95
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront form render class.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMCode form render class.</summary>
    public partial class SMFrontRender
	{

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Build controls code on output string builder.</summary>
        public string Controls()
        {
            //
            // initialize
            //
            int i;
            List<string> macros = new List<string>();
            StringBuilder output = new StringBuilder();
            SMFrontControl control;

            //
            // remarks begin
            //
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - Self generated controls.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");

            //
            // controls
            //
            for (i = 0; i < Form.Controls.Count; i++)
            {
                control = Form.Controls[Form.Controls.IndicesByViewIndex[i]];
                output.Append(Templates.Get("ctrl_" + control.ControlType.ToLower() + ".html", macros.ToArray()));
            }

            //
            // remarks end
            //
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - End of self generated controls.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");

            //
            // return
            //
            return output.ToString(); 
        }

        #endregion

        /* */

    }

    /* */

}
