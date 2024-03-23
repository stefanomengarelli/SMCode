/*  ===========================================================================
 *  
 *  File:       SMWebControl.cs
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

using SMCode;
using System.Text;

namespace SMFront
{

    /* */

    /// <summary>SMFront web control base class.</summary>
    public partial class SMWebControl
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get control arguments.</summary>
        public SMDictionary Arguments { get; private set; } = null;

        /// <summary>Get or set control order value.</summary>
        public int Order { get; set; } = 0;

        /// <summary>Get or set parent control instance.</summary>
        public SMWebControl Parent { get; set; } = null;

        /// <summary>Get or set parent control type.</summary>
        public SMWebControlType Type { get; set; } = SMWebControlType.None;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMWebControl(SMApplication _SMApplication = null, SMWebControl _ParentControl = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Parent = _ParentControl;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return string containing control rendered HTML code.</summary>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            if (Type == SMWebControlType.Panel) RenderPanel(sb);
            //else if (Type == SMWebControlType.Text) RenderText(sb);
            //else if (Type == SMWebControlType.Memo) RenderMemo(sb);
            //else if (Type == SMWebControlType.Number) RenderNumber(sb);
            //else if (Type == SMWebControlType.Date) RenderDate(sb);
            //else if (Type == SMWebControlType.Line) RenderLine(sb);
            return sb.ToString();
        }

        public virtual void RenderPanel(StringBuilder _Code)
        {
            // 
        }

        #endregion

        /* */

    }

    /* */

}
