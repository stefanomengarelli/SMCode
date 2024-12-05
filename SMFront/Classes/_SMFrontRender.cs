/*  ===========================================================================
 *  
 *  File:       SMFrontRender.cs
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

using Microsoft.Extensions.Primitives;
using SMCodeSystem;
using System.Collections.Generic;
using System.Text;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMCode form render class.</summary>
    public class SMFrontRender
	{

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMFront SM = null;

        /// <summary>Current render controls collection.</summary>
        private List<SMFrontControl> controls = null;

        /// <summary>Current code output.</summary>
        private StringBuilder output = null;

        /// <summary>Current script output.</summary>
        private StringBuilder script = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set property.</summary>
        public SMFrontForm Form { get; private set; } = null;

        /// <summary>Get last rendered output.</summary>
        public string Output { get { return output.ToString(); } }

        /// <summary>Get last rendered script.</summary>
        public string Script { get { return script.ToString(); } }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontRender(SMFrontForm _Form, SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Form = _Form;
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
            controls = new List<SMFrontControl>();
            output = new StringBuilder();
            script = new StringBuilder();
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            output.Clear();
            script.Clear();
        }

        /// <summary>Build render.</summary>
        public void Build()
        {
            RenderScripts();
            RenderControls();
        }

        /// <summary>Return script rendering.</summary>
        public void RenderControls()
        {
            output.Clear();
            if (controls.Count < 1) Select(Form.Controls.Items, Form.Controls.IndicesByViewIndex.ToArray());

            //
            // remarks begin
            //
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - Self generated controls.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");

            //
            // remarks end
            //
            output.Append("</script>\r\n");
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - End of self generated controls.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");
        }

        /// <summary>Return script rendering.</summary>
        public void RenderScripts()
        {
            script.Clear();
            if (controls.Count < 1) Select(Form.Controls.Items, Form.Controls.IndicesByViewIndex.ToArray());
            
            //
            // remarks begin
            //
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - Self generated script.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");
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
            // remarks end
            //
            output.Append("</script>\r\n");
            output.Append("<!-- --------------------------------------------------------------------------------\r\n");
            output.Append("     " + SM.ExecutableName + " - End of self generated script.\r\n");
            output.Append("     -------------------------------------------------------------------------------- -->\r\n");
        }

        /// <summary>Load controls collection to render.</summary>
        public void Select(object[] _Controls, int[] _Indexes = null)
        {
            int i;
            controls.Clear();
            if (_Controls != null)
            {
                if (_Indexes != null)
                {
                    for (i = 0; i < _Indexes.Length; i++) controls.Add((SMFrontControl)_Controls[_Indexes[i]]);
                }
                else
                {
                    for (i = 0; i < _Controls.Length; i++) controls.Add((SMFrontControl)_Controls[i]);
                }
            }
        }

        #endregion

        /* */

    }

    /* */

}
