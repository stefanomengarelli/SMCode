/*  ===========================================================================
 *  
 *  File:       SMFrontRender_Scripts.cs
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

        /// <summary>Build scripts code on output string builder.</summary>
        public string Scripts()
        {
            int i;
            SMFrontControl control;
            SMFrontControls controls = Form.Controls;
            //
            // initialize
            //
            StringBuilder output = new StringBuilder();

            //
            // remarks begin
            //
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - Self generated script.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");

            //
            // includes
            //
            for (i = 0; i < Form.Controls.Count; i++)
            {
                control = Form.Controls[Form.Controls.IndicesByViewIndex[i]];
                if (control.ControlType == SMFrontControlType.Include)
                {
                    output.Append("<script src=\"" + control.Text + "\" type=\"text/javascript\" ></script>\r\n");
                }
            }

            //
            // code block start
            //
            output.Append("<script type=\"text/javascript\">>\r\n");

            //
            // boot script
            //
            output.Append("\r\n");
            output.Append("$(document).ready(function () {\r\n");
            output.Append("  if (typeof $_On_Ready === 'function') $_On_Ready();\r\n");
            output.Append("  $(\"#smLoader\").addClass(\"sm-hidden\");\r\n");
            output.Append("});\r\n");

            //
            // initialization
            //

            //
            // code block start
            //
            output.Append("</script>\r\n");

            //
            // remarks end
            //
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - End of self generated script.\r\n");
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
