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

        /// <summary>Current render output.</summary>
        private StringBuilder output = null;

        /// <summary>Render templates engine.</summary>
        private SMTemplates templates = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set property.</summary>
        public SMFrontForm Form { get; set; } = null;

        /// <summary>Get last rendered output.</summary>
        public string Output { get { return output.ToString(); } }

        /// <summary>Get render templates engine.</summary>
        public SMTemplates Templates { get { return templates; } }

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
            templates = new SMTemplates(SM);
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
            Form = null;
            templates.Clear();
            output.Clear();
        }

        /// <summary>Load controls collection to render.</summary>
        public void Select(object[] _Controls, int[] _Indexes = null)
        {
            int i;
            controls.Clear();
            if (_Controls != null)
            {
                if (_Indexes!=null)
                {
                    for (i = 0; i < _Indexes.Length; i++) controls.Add((SMFrontControl)_Controls[_Indexes[i]]);
                }
                else
                {
                    for (i = 0; i < _Controls.Length; i++) controls.Add((SMFrontControl)_Controls[i]);
                }
            }
        }

        /// <summary>Return controls rendering.</summary>
        public string Controls()
        {
            int i = 0;
            output.Clear();
            if (controls.Count < 1) Select(Form.Controls.Items, Form.Controls.IndicesByViewIndex.ToArray());
            while (i < controls.Count)
            {

                i++;
            }
            return output.ToString();
        }

        /// <summary>Return script rendering.</summary>
        public string Scripts()
        {
            int i = 0;
            output.Clear();
            if (controls.Count < 1) Select(Form.Controls.Items, Form.Controls.IndicesByViewIndex.ToArray());
            while (i < controls.Count)
            {

                i++;
            }
            return output.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
