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

        /// <summary>Control data max length.</summary>
        private int maxLen = 0;

        /// <summary>Memo field (text area) max length.</summary>
        public const int MEMO_MAX_LEN = 8192;

        /// <summary>Number field max length.</summary>
        public const int NUMBER_MAX_LEN = 18;

        /// <summary>Text field max length.</summary>
        public const int TEXT_MAX_LEN = 240;

        #endregion

        /* */

        #region Delegates

        /*  ===================================================================
         *  Delegates
         *  ===================================================================
         */

        /// <summary>Occurs when accordion control has to be rendered.</summary>
        public delegate void OnRenderAccordion(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when accordion control has to be rendered.</summary>
        public static event OnRenderAccordion RenderAccordion = null;

        /// <summary>Occurs when attachment control has to be rendered.</summary>
        public delegate void OnRenderAttachment(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when attachment control has to be rendered.</summary>
        public static event OnRenderAttachment RenderAttachment = null;

        /// <summary>Occurs when blob control has to be rendered.</summary>
        public delegate void OnRenderBlob(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when blob control has to be rendered.</summary>
        public static event OnRenderBlob RenderBlob = null;

        /// <summary>Occurs when button control has to be rendered.</summary>
        public delegate void OnRenderButton(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when button control has to be rendered.</summary>
        public static event OnRenderButton RenderButton = null;

        /// <summary>Occurs when caption control has to be rendered.</summary>
        public delegate void OnRenderCaption(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when caption control has to be rendered.</summary>
        public static event OnRenderCaption RenderCaption = null;

        /// <summary>Occurs when check control has to be rendered.</summary>
        public delegate void OnRenderCheck(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when check control has to be rendered.</summary>
        public static event OnRenderCheck RenderCheck = null;

        /// <summary>Occurs when chips control has to be rendered.</summary>
        public delegate void OnRenderChips(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when chips control has to be rendered.</summary>
        public static event OnRenderChips RenderChips = null;

        /// <summary>Occurs when combo control has to be rendered.</summary>
        public delegate void OnRenderCombo(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when combo control has to be rendered.</summary>
        public static event OnRenderCombo RenderCombo = null;

        /// <summary>Occurs when date control has to be rendered.</summary>
        public delegate void OnRenderDate(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when date control has to be rendered.</summary>
        public static event OnRenderDate RenderDate = null;

        /// <summary>Occurs when details control has to be rendered.</summary>
        public delegate void OnRenderDetails(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when details control has to be rendered.</summary>
        public static event OnRenderDetails RenderDetails = null;

        /// <summary>Occurs when end accordion control has to be rendered.</summary>
        public delegate void OnRenderEndAccordion(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when end accordion control has to be rendered.</summary>
        public static event OnRenderEndAccordion RenderEndAccordion = null;

        /// <summary>Occurs when end details control has to be rendered.</summary>
        public delegate void OnRenderEndDetails(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when end details control has to be rendered.</summary>
        public static event OnRenderEndDetails RenderEndDetails = null;

        /// <summary>Occurs when end panel control has to be rendered.</summary>
        public delegate void OnRenderEndPanel(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when end panel control has to be rendered.</summary>
        public static event OnRenderEndPanel RenderEndPanel = null;

        /// <summary>Occurs when end repeat control has to be rendered.</summary>
        public delegate void OnRenderEndRepeat(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when end repeat control has to be rendered.</summary>
        public static event OnRenderEndRepeat RenderEndRepeat = null;

        /// <summary>Occurs when end row control has to be rendered.</summary>
        public delegate void OnRenderEndRow(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when end row control has to be rendered.</summary>
        public static event OnRenderEndRow RenderEndRow = null;

        /// <summary>Occurs when end field set control has to be rendered.</summary>
        public delegate void OnRenderEndSet(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when EndSet control has to be rendered.</summary>
        public static event OnRenderEndSet RenderEndSet = null;

        /// <summary>Occurs when end tab control has to be rendered.</summary>
        public delegate void OnRenderEndTab(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when EndTab control has to be rendered.</summary>
        public static event OnRenderEndTab RenderEndTab = null;

        /// <summary>Occurs when end view control has to be rendered.</summary>
        public delegate void OnRenderEndView(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when EndView control has to be rendered.</summary>
        public static event OnRenderEndView RenderEndView = null;

        /// <summary>Occurs when field set control has to be rendered.</summary>
        public delegate void OnRenderFieldSet(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when field set control has to be rendered.</summary>
        public static event OnRenderFieldSet RenderFieldSet = null;

        /// <summary>Occurs when hidden control has to be rendered.</summary>
        public delegate void OnRenderHidden(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when hidden control has to be rendered.</summary>
        public static event OnRenderHidden RenderHidden = null;

        /// <summary>Occurs when horizontalLine control has to be rendered.</summary>
        public delegate void OnRenderHorizontalLine(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when horizontalLine control has to be rendered.</summary>
        public static event OnRenderHorizontalLine RenderHorizontalLine = null;

        /// <summary>Occurs when image control has to be rendered.</summary>
        public delegate void OnRenderImage(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when image control has to be rendered.</summary>
        public static event OnRenderImage RenderImage = null;

        /// <summary>Occurs when import control has to be rendered.</summary>
        public delegate void OnRenderImport(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when import control has to be rendered.</summary>
        public static event OnRenderImport RenderImport = null;

        /// <summary>Occurs when include control has to be rendered.</summary>
        public delegate void OnRenderInclude(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when include control has to be rendered.</summary>
        public static event OnRenderInclude RenderInclude = null;

        /// <summary>Occurs when information box control has to be rendered.</summary>
        public delegate void OnRenderInformation(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when information box control has to be rendered.</summary>
        public static event OnRenderInformation RenderInformation = null;

        /// <summary>Occurs when literal code control has to be rendered.</summary>
        public delegate void OnRenderLiteral(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when literal code control has to be rendered.</summary>
        public static event OnRenderLiteral RenderLiteral = null;

        /// <summary>Occurs when location control has to be rendered.</summary>
        public delegate void OnRenderLocation(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when location control has to be rendered.</summary>
        public static event OnRenderLocation RenderLocation = null;

        /// <summary>Occurs when memo control has to be rendered.</summary>
        public delegate void OnRenderMemo(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when memo control has to be rendered.</summary>
        public static event OnRenderMemo RenderMemo = null;

        /// <summary>Occurs when meta control has to be rendered.</summary>
        public delegate void OnRenderMeta(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when meta control has to be rendered.</summary>
        public static event OnRenderMeta RenderMeta = null;

        /// <summary>Occurs when number control has to be rendered.</summary>
        public delegate void OnRenderNumber(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when number control has to be rendered.</summary>
        public static event OnRenderNumber RenderNumber = null;

        /// <summary>Occurs when panel control has to be rendered.</summary>
        public delegate void OnRenderPanel(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when panel control has to be rendered.</summary>
        public static event OnRenderPanel RenderPanel = null;

        /// <summary>Occurs when print control has to be rendered.</summary>
        public delegate void OnRenderPrint(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when print control has to be rendered.</summary>
        public static event OnRenderPrint RenderPrint = null;

        /// <summary>Occurs when radio button control has to be rendered.</summary>
        public delegate void OnRenderRadioButton(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when radio button control has to be rendered.</summary>
        public static event OnRenderRadioButton RenderRadioButton = null;

        /// <summary>Occurs when remark control has to be rendered.</summary>
        public delegate void OnRenderRem(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when remark control has to be rendered.</summary>
        public static event OnRenderRem RenderRem = null;

        /// <summary>Occurs when repeater control has to be rendered.</summary>
        public delegate void OnRenderRepeat(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when repeater control has to be rendered.</summary>
        public static event OnRenderRepeat RenderRepeat = null;

        /// <summary>Occurs when script control has to be rendered.</summary>
        public delegate void OnRenderScript(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when script control has to be rendered.</summary>
        public static event OnRenderScript RenderScript = null;

        /// <summary>Occurs when tab control has to be rendered.</summary>
        public delegate void OnRenderTab(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when tab control has to be rendered.</summary>
        public static event OnRenderTab RenderTab = null;

        /// <summary>Occurs when text control has to be rendered.</summary>
        public delegate void OnRenderText(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when text control has to be rendered.</summary>
        public static event OnRenderText RenderText = null;

        /// <summary>Occurs when time control has to be rendered.</summary>
        public delegate void OnRenderTime(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when time control has to be rendered.</summary>
        public static event OnRenderTime RenderTime = null;

        /// <summary>Occurs when upload control has to be rendered.</summary>
        public delegate void OnRenderUpload(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when upload control has to be rendered.</summary>
        public static event OnRenderUpload RenderUpload = null;

        /// <summary>Occurs when verticalSpacing control has to be rendered.</summary>
        public delegate void OnRenderVerticalSpacing(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when verticalSpacing control has to be rendered.</summary>
        public static event OnRenderVerticalSpacing RenderVerticalSpacing = null;

        /// <summary>Occurs when view control has to be rendered.</summary>
        public delegate void OnRenderView(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when view control has to be rendered.</summary>
        public static event OnRenderView RenderView = null;

        /// <summary>Occurs when warning control has to be rendered.</summary>
        public delegate void OnRenderWarning(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when warning control has to be rendered.</summary>
        public static event OnRenderWarning RenderWarning = null;

        /// <summary>Occurs when yes/no control has to be rendered.</summary>
        public delegate void OnRenderYesNo(object _Sender, StringBuilder _Code);
        /// <summary>Occurs when yes/no control has to be rendered.</summary>
        public static event OnRenderYesNo RenderYesNo = null;

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
                    if (Type == SMWebControlType.Number) maxLen = NUMBER_MAX_LEN;
                    else if (Type == SMWebControlType.Chips) maxLen = MEMO_MAX_LEN;
                    else if (Type == SMWebControlType.Text) maxLen = TEXT_MAX_LEN;
                    else if (Type == SMWebControlType.Memo) maxLen = MEMO_MAX_LEN;
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
        public SMWebControlType Type { get; set; } = SMWebControlType.None;

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
        public SMWebControl(SMApplication _SMApplication = null, SMWebControl _ParentControl = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
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
            RenderAccordion = RenderAccordionBuiltIn;
            RenderPanel = RenderPanelBuiltIn;
        }

        #endregion

        /* */

        #region Methods - Render

        /*  ===================================================================
         *  Methods - Render
         *  ===================================================================
         */

        /// <summary>Return string containing control rendered HTML code.</summary>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            if ((Type == SMWebControlType.Accordion) && (RenderAccordion != null)) RenderAccordion(this, sb);
            else if ((Type == SMWebControlType.Attachment) && (RenderAttachment != null)) RenderAttachment(this, sb);
            else if ((Type == SMWebControlType.Blob) && (RenderBlob != null)) RenderBlob(this, sb);
            else if ((Type == SMWebControlType.Button) && (RenderButton != null)) RenderButton(this, sb);
            else if ((Type == SMWebControlType.Caption) && (RenderCaption != null)) RenderCaption(this, sb);
            else if ((Type == SMWebControlType.Check) && (RenderCheck != null)) RenderCheck(this, sb);
            else if ((Type == SMWebControlType.Chips) && (RenderChips != null)) RenderChips(this, sb);
            else if ((Type == SMWebControlType.Combo) && (RenderCombo != null)) RenderCombo(this, sb);
            else if ((Type == SMWebControlType.Date) && (RenderDate != null)) RenderDate(this, sb);
            else if ((Type == SMWebControlType.Details) && (RenderDetails != null)) RenderDetails(this, sb);
            else if ((Type == SMWebControlType.EndAccordion) && (RenderEndAccordion != null)) RenderEndAccordion(this, sb);
            else if ((Type == SMWebControlType.EndDetails) && (RenderEndDetails != null)) RenderEndDetails(this, sb);
            else if ((Type == SMWebControlType.EndPanel) && (RenderEndPanel != null)) RenderEndPanel(this, sb);
            else if ((Type == SMWebControlType.EndRepeat) && (RenderEndRepeat != null)) RenderEndRepeat(this, sb);
            else if ((Type == SMWebControlType.EndRow) && (RenderEndRow != null)) RenderEndRow(this, sb);
            else if ((Type == SMWebControlType.EndSet) && (RenderEndSet != null)) RenderEndSet(this, sb);
            else if ((Type == SMWebControlType.EndTab) && (RenderEndTab != null)) RenderEndTab(this, sb);
            else if ((Type == SMWebControlType.EndView) && (RenderEndView != null)) RenderEndView(this, sb);
            else if ((Type == SMWebControlType.FieldSet) && (RenderFieldSet != null)) RenderFieldSet(this, sb);
            else if ((Type == SMWebControlType.Hidden) && (RenderHidden != null)) RenderHidden(this, sb);
            else if ((Type == SMWebControlType.HorizontalLine) && (RenderHorizontalLine != null)) RenderHorizontalLine(this, sb);
            else if ((Type == SMWebControlType.Image) && (RenderImage != null)) RenderImage(this, sb);
            else if ((Type == SMWebControlType.Import) && (RenderImport != null)) RenderImport(this, sb);
            else if ((Type == SMWebControlType.Include) && (RenderInclude != null)) RenderInclude(this, sb);
            else if ((Type == SMWebControlType.Information) && (RenderInformation != null)) RenderInformation(this, sb);
            else if ((Type == SMWebControlType.Literal) && (RenderLiteral != null)) RenderLiteral(this, sb);
            else if ((Type == SMWebControlType.Location) && (RenderLocation != null)) RenderLocation(this, sb);
            else if ((Type == SMWebControlType.Memo) && (RenderMemo != null)) RenderMemo(this, sb);
            else if ((Type == SMWebControlType.Meta) && (RenderMeta != null)) RenderMeta(this, sb);
            else if ((Type == SMWebControlType.Number) && (RenderNumber != null)) RenderNumber(this, sb);
            else if ((Type == SMWebControlType.Panel) && (RenderPanel != null)) RenderPanel(this, sb);
            else if ((Type == SMWebControlType.Print) && (RenderPrint != null)) RenderPrint(this, sb);
            else if ((Type == SMWebControlType.RadioButton) && (RenderRadioButton != null)) RenderRadioButton(this, sb);
            else if ((Type == SMWebControlType.Rem) && (RenderRem != null)) RenderRem(this, sb);
            else if ((Type == SMWebControlType.Repeat) && (RenderRepeat != null)) RenderRepeat(this, sb);
            else if ((Type == SMWebControlType.Script) && (RenderScript != null)) RenderScript(this, sb);
            else if ((Type == SMWebControlType.Tab) && (RenderTab != null)) RenderTab(this, sb);
            else if ((Type == SMWebControlType.Text) && (RenderText != null)) RenderText(this, sb);
            else if ((Type == SMWebControlType.Time) && (RenderTime != null)) RenderTime(this, sb);
            else if ((Type == SMWebControlType.Upload) && (RenderUpload != null)) RenderUpload(this, sb);
            else if ((Type == SMWebControlType.VerticalSpacing) && (RenderVerticalSpacing != null)) RenderVerticalSpacing(this, sb);
            else if ((Type == SMWebControlType.View) && (RenderView != null)) RenderView(this, sb);
            else if ((Type == SMWebControlType.Warning) && (RenderWarning != null)) RenderWarning(this, sb);
            else if ((Type == SMWebControlType.YesNo) && (RenderYesNo != null)) RenderYesNo(this, sb);
            return sb.ToString();
        }

        /// <summary>Built in accordion control render.</summary>
        public static void RenderAccordionBuiltIn(object _Sender, StringBuilder _Code)
        {
            // 
        }

        /// <summary>Built in panel control render.</summary>
        public static void RenderPanelBuiltIn(object _Sender, StringBuilder _Code)
        {
            // 
        }

        #endregion

        /* */

    }

    /* */

}
