/*  ===========================================================================
 *  
 *  File:       SMFrontControl.cs
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
using System.Collections.Generic;
using System.Text;

namespace SMFront
{

    /* */

    /// <summary>SMFront web control base class.</summary>
    public partial class SMFrontControl
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Memo field (text area) max length.</summary>
        public const int MEMO_MAX_LEN = 16384;

        /// <summary>Number field max length.</summary>
        public const int NUMBER_MAX_LEN = 18;

        /// <summary>Text field max length.</summary>
        public const int TEXT_MAX_LEN = 254;

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        /// <summary>Control data max length.</summary>
        private int maxLen = 0;

        #endregion

        /* */

        #region Delegates

        /*  ===================================================================
         *  Delegates
         *  ===================================================================
         */

        /// <summary>Occurs when control has to be rendered.</summary>
        public delegate void OnRenderControl(object _Sender, StringBuilder _Code, SMFrontControlType _ControlType);
        /// <summary>Occurs when control has to be rendered.</summary>
        public event OnRenderControl RenderControl = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get control arguments.</summary>
        public SMDictionary Arguments { get; private set; } = null;

        /// <summary>Get or set calculate script.</summary>
        public string Calculate { get; set; } = "";

        /// <summary>Get or set value changed flag.</summary>
        public bool Changed { get; set; } = false;

        /// <summary>Get or set CSS class for control.</summary>
        public string Class { get; set; } = "";

        /// <summary>Get or set enable script.</summary>
        public string Enable { get; set; } = "";

        /// <summary>Get or set control table related field name.</summary>
        public string Field { get; set; } = "";

        /// <summary>Get or set control API related field name.</summary>
        public string FieldAPI { get; set; } = "";

        /// <summary>Get or set control data format.</summary>
        public string Format { get; set; } = "";

        /// <summary>Get or set grid columns.</summary>
        public int GridColumns { get; set; } = 0;

        /// <summary>Get or set control id.</summary>
        public int Id { get; set; } = 0;

        /// <summary>Get or set control leave event script.</summary>
        public string Leave { get; set; } = "";

        /// <summary>Get or set field content maximum length. If value is lower than zero
        /// or greather than max length, default value will be assumed.</summary>
        public int Length
        {
            get { return maxLen; }
            set
            {
                maxLen = value;
                if ((maxLen < 0) || (maxLen > MEMO_MAX_LEN))
                {
                    if (Type == SMFrontControlType.Number) maxLen = NUMBER_MAX_LEN;
                    else if (Type == SMFrontControlType.Chips) maxLen = MEMO_MAX_LEN;
                    else if (Type == SMFrontControlType.Text) maxLen = TEXT_MAX_LEN;
                    else if (Type == SMFrontControlType.Memo) maxLen = MEMO_MAX_LEN;
                    else maxLen = 0;
                }
            }
        }

        /// <summary>Get or set control name.</summary>
        public string Name { get; set; } = "";

        /// <summary>Get control options.</summary>
        public SMDictionary Options { get; private set; } = null;

        /// <summary>Get or set control order value.</summary>
        public int Order { get; set; } = 0;

        /// <summary>Get or set parent object instance.</summary>
        public object Parent { get; set; } = null;

        /// <summary>Get or set value required flag.</summary>
        public bool Required { get; set; } = false;

        /// <summary>Get or set control text.</summary>
        public string Text { get; set; } = "";

        /// <summary>Get or set parent control type.</summary>
        public SMFrontControlType Type { get; set; } = SMFrontControlType.None;

        /// <summary>Get or set control value.</summary>
        public List<string> Values { get; set; } = null;

        /// <summary>Get or set validate script.</summary>
        public string Validate { get; set; } = "";

        /// <summary>Get or set control view order value.</summary>
        public int ViewOrder { get; set; } = 0;

        /// <summary>Get or set visibility script.</summary>
        public string Visible { get; set; } = "";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontControl(object _ParentControl = null, SMApplication _SMApplication = null)
        {
            if (_SMApplication == null)
            {
                if (_ParentControl == null) SM = SMApplication.CurrentOrNew();
                else if (_ParentControl is SMFrontControl)
                {
                    if (((SMFrontControl)_ParentControl).SM == null) SM = SMApplication.CurrentOrNew();
                    else SM = ((SMFrontControl)_ParentControl).SM;
                }
                else SM = SMApplication.CurrentOrNew();
            }
            else SM = _SMApplication;
            Parent = _ParentControl;
            InitializeControl();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize control.</summary>
        private void InitializeControl()
        {
            Arguments = new SMDictionary(SM);
            Options = new SMDictionary(SM);
            //
            RenderControl = RenderControlBuiltIn;
        }

        /// <summary>Return string containing control rendered HTML code.</summary>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            if (RenderControl != null) RenderControl(this, sb, Type);
            return sb.ToString();
        }

        /// <summary>Built in accordion control render.</summary>
        public void RenderControlBuiltIn(object _Sender, StringBuilder _Code, SMFrontControlType _ControlType)
        {
            if (_ControlType == SMFrontControlType.Accordion) RenderControlBuiltIn_Accordion(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Attachment) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Blob) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Button) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Caption) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Check) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Chips) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Date) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Details) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndAccordion) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndDetails) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndPanel) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndRepeat) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndRow) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndSet) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndTab) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.EndView) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.FieldSet) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Hidden) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.HorizontalLine) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Image) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Import) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Include) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Information) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Literal) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Location) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Memo) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Meta) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Number) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Panel) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Print) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.RadioButton) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Remark) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Repeat) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Script) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Select) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Tab) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Text) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Time) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Upload) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.VerticalSpacing) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.View) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.Warning) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
            else if (_ControlType == SMFrontControlType.YesNo) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
        }

        #endregion

        /* */

    }

    /* */

}
