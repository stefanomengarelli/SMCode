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
using System;
using System.Data;
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
        private SMApplication SM = null;

        /// <summary>Control data max length.</summary>
        private int length = 0;

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

        /// <summary>Get control parameters.</summary>
        public SMDictionary Parameters { get; private set; } = new SMDictionary();

        /// <summary>Get or set calculate script.</summary>
        public string CalculateScript { get; set; } = "";

        /// <summary>Get or set value changed flag.</summary>
        public bool Changed { get; set; } = false;

        /// <summary>Get or set CSS class for control.</summary>
        public string Class { get; set; } = "";

        /// <summary>Get or set enable script.</summary>
        public string EnableScript { get; set; } = "";

        /// <summary>Get or set control table related field name.</summary>
        public string Field { get; set; } = "";

        /// <summary>Get or set control related export field name.</summary>
        public string FieldExport { get; set; } = "";

        /// <summary>Get or set control related import field name.</summary>
        public string FieldImport { get; set; } = "";

        /// <summary>Get or set control data format.</summary>
        public string Format { get; set; } = "";

        /// <summary>Get or set grid columns.</summary>
        public int GridColumns { get; set; } = 0;

        /// <summary>Get or set control id.</summary>
        public int Id { get; set; } = 0;

        /// <summary>Get or set control leave event script.</summary>
        public string LeaveScript { get; set; } = "";

        /// <summary>Get or set field content maximum length. If value is lower than zero
        /// or greather than max length, default value will be assumed.</summary>
        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                if ((length < 0) || (length > MEMO_MAX_LEN))
                {
                    if (Type == SMFrontControlType.Number) length = NUMBER_MAX_LEN;
                    else if (Type == SMFrontControlType.Chips) length = MEMO_MAX_LEN;
                    else if (Type == SMFrontControlType.Text) length = TEXT_MAX_LEN;
                    else if (Type == SMFrontControlType.Memo) length = MEMO_MAX_LEN;
                    else length = 0;
                }
            }
        }

        /// <summary>Get or set control name.</summary>
        public string Name { get; set; } = "";

        /// <summary>Get control options.</summary>
        public string Options { get; private set; } = "";

        /// <summary>Get or set control order value.</summary>
        public int Order { get; set; } = 0;

        /// <summary>Parent object.</summary>
        public object Parent { get; set; } = null;

        /// <summary>Get or set value required flag.</summary>
        public bool Required { get; set; } = false;

        /// <summary>Get or set control text.</summary>
        public string Text { get; set; } = "";

        /// <summary>Get or set parent control type.</summary>
        public SMFrontControlType Type { get; set; } = SMFrontControlType.None;

        /// <summary>Get or set validate script.</summary>
        public string ValidateScript { get; set; } = "";

        /// <summary>Get or set visibility script.</summary>
        public string VisibleScript { get; set; } = "";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontControl(SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            InitializeControl();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControl(SMFrontControl _Control, SMApplication _SMApplication = null)
        {
            if (_SMApplication == null)
            {
                if (_Control.SM == null) SM = SMApplication.CurrentOrNew();
                else SM = _Control.SM;
            }
            else SM = _SMApplication;
            InitializeControl();
            Assign(_Control);
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControl(SMDataset _Dataset, SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            InitializeControl();
            Read(_Dataset);
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControl(int _Id, string _Name, SMFrontControlType _Type, string _Text, string _Field, int _Length, string _Format,
            bool _Required = false, int _GridColumns = 0, string _Class = "", SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            InitializeControl();
            Id = _Id;
            Name = _Name;
            Type = _Type;
            Text = _Text;
            Field = _Field;
            Length = _Length;
            Format = _Format;
            Required = _Required;
            GridColumns = _GridColumns;
            Class = _Class;
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
            Clear();
            RenderControl = RenderControlBuiltIn;
        }

        /// <summary>Clear control instance.</summary>
        public SMFrontControl Assign(SMFrontControl _Control, SMApplication _SMApplication = null)
        {
            if (SM == null)
            {
                if (_SMApplication == null)
                {
                    if (_Control.SM != null) SM = _Control.SM;
                    else SM = SMApplication.CurrentOrNew();
                }
                else SM = _SMApplication;
            }
            Parameters.Assign(_Control.Parameters);
            CalculateScript = _Control.CalculateScript;
            Changed = _Control.Changed;
            Class = _Control.Class;
            EnableScript = _Control.EnableScript;
            Field = _Control.Field;
            FieldExport= _Control.FieldExport;
            FieldImport= _Control.FieldImport;
            Format = _Control.Format;
            GridColumns = _Control.GridColumns;
            Id = _Control.Id;
            LeaveScript = _Control.LeaveScript;
            Length = _Control.Length;
            Name = _Control.Name;
            Options = _Control.Options;
            Order = _Control.Order;
            Required = _Control.Required;
            Text = _Control.Text;
            Type = _Control.Type;
            ValidateScript = _Control.ValidateScript;
            VisibleScript = _Control.VisibleScript;
            return this;
        }

        /// <summary>Clear control instance.</summary>
        public SMFrontControl Clear()
        {
            Parameters.Clear();
            CalculateScript = "";
            Changed = false;
            Class = "";
            EnableScript = "";
            Field = "";
            FieldExport = "";
            FieldImport = "";
            Format = "";
            GridColumns = 0;
            Id = 0;
            LeaveScript = "";
            Length = 0;
            Name = "";
            Options = "";
            Order = 0;
            Required = false;
            Text = "";
            Type = SMFrontControlType.None;
            ValidateScript = "";
            VisibleScript = "";
            return this;
        }

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Read(SMDataset _Dataset)
        {
            try
            {
                Clear();
                Parameters.FromParameters(_Dataset.FieldStr("Parameters"));
                CalculateScript = _Dataset.FieldStr("CalculateScript");
                Changed = false;
                Class = _Dataset.FieldStr("Class");
                EnableScript = _Dataset.FieldStr("EnableScript");
                Field = _Dataset.FieldStr("Field");
                FieldExport = _Dataset.FieldStr("");
                FieldImport = _Dataset.FieldStr("");
                Format = _Dataset.FieldStr("");
                GridColumns = _Dataset.FieldInt("GridColumns");
                Id = _Dataset.FieldInt("Id");
                LeaveScript = _Dataset.FieldStr("LeaveScript");
                Length = _Dataset.FieldInt("Length");
                Name = _Dataset.FieldStr("Name");
                Options = _Dataset.FieldStr("Options");
                Order = _Dataset.FieldInt("Order");
                Required = _Dataset.FieldBool("Required");
                Text = _Dataset.FieldStr("Text");
                Type = SMFrontControlType.None;
                ValidateScript = _Dataset.FieldStr("ValidateScript");
                VisibleScript = _Dataset.FieldStr("VisibleScript");
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Read(DataRow _DataRow)
        {
            try
            {
                Clear();
                Parameters.FromParameters(SM.ToStr(_DataRow["Parameters"]));
                CalculateScript = SM.ToStr(_DataRow["CalculateScript"]);
                Changed = false;
                Class = SM.ToStr(_DataRow["Class"]);
                EnableScript = SM.ToStr(_DataRow["EnableScript"]);
                Field = SM.ToStr(_DataRow["Field"]);
                FieldExport = SM.ToStr(_DataRow[""]);
                FieldImport = SM.ToStr(_DataRow[""]);
                Format = SM.ToStr(_DataRow[""]);
                GridColumns = SM.ToInt(_DataRow["GridColumns"]);
                Id = SM.ToInt(_DataRow["Id"]);
                LeaveScript = SM.ToStr(_DataRow["LeaveScript"]);
                Length = SM.ToInt(_DataRow["Length"]);
                Name = SM.ToStr(_DataRow["Name"]);
                Options = SM.ToStr(_DataRow["Options"]);
                Order = SM.ToInt(_DataRow["Order"]);
                Required = SM.ToBool(_DataRow["Required"]);
                Text = SM.ToStr(_DataRow["Text"]);
                Type = SMFrontControlType.None;
                ValidateScript = SM.ToStr(_DataRow["ValidateScript"]);
                VisibleScript = SM.ToStr(_DataRow["VisibleScript"]);
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Return string containing control rendered HTML code.</summary>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            if (RenderControl != null) RenderControl(this, sb, Type);
            return sb.ToString();
        }

        /// <summary>Return string containing control rendered HTML code.</summary>
        public void Render(StringBuilder _SB)
        {
            if (RenderControl != null) RenderControl(this, _SB, Type);
        }

        /// <summary>Built in control render.</summary>
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
            else if (_ControlType == SMFrontControlType.Related) RenderControlBuiltIn_Remark(_Sender, _Code, _ControlType);
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

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Compare front controls by field.</summary>
        public static int CompareByField(object _A, object _B)
        {
            return ((SMFrontControl)_A).Field.CompareTo(((SMFrontControl)_B).Field);
        }

        /// <summary>Compare front controls by id.</summary>
        public static int CompareById(object _A, object _B)
        {
            return ((SMFrontControl)_A).Id.CompareTo(((SMFrontControl)_B).Id);
        }

        /// <summary>Compare front controls by name.</summary>
        public static int CompareByName(object _A, object _B)
        {
            return ((SMFrontControl)_A).Name.CompareTo(((SMFrontControl)_B).Name);
        }

        /// <summary>Compare front controls by order.</summary>
        public static int CompareByOrder(object _A, object _B)
        {
            return ((SMFrontControl)_A).Order.CompareTo(((SMFrontControl)_B).Order);
        }

        /// <summary>Return string representing front control type.</summary>
        public static string ToType(SMFrontControlType _ControlType)
        {
            if (_ControlType == SMFrontControlType.Accordion) return "ACCORDION";
            else if (_ControlType == SMFrontControlType.Attachment) return "ATTACHMENT";
            else if (_ControlType == SMFrontControlType.Blob) return "BLOB";
            else if (_ControlType == SMFrontControlType.Button) return "BUTTON";
            else if (_ControlType == SMFrontControlType.Caption) return "CAPTION";
            else if (_ControlType == SMFrontControlType.Check) return "CHECK";
            else if (_ControlType == SMFrontControlType.Chips) return "CHIPS";
            else if (_ControlType == SMFrontControlType.Date) return "DATE";
            else if (_ControlType == SMFrontControlType.Details) return "DETAILS";
            else if (_ControlType == SMFrontControlType.EndAccordion) return "ENDACCORDION";
            else if (_ControlType == SMFrontControlType.EndDetails) return "ENDDETAILS";
            else if (_ControlType == SMFrontControlType.EndPanel) return "ENDPANEL";
            else if (_ControlType == SMFrontControlType.EndRepeat) return "ENDREPEAT";
            else if (_ControlType == SMFrontControlType.EndRow) return "ENDROW";
            else if (_ControlType == SMFrontControlType.EndSet) return "ENDSET";
            else if (_ControlType == SMFrontControlType.EndTab) return "ENDTAB";
            else if (_ControlType == SMFrontControlType.EndView) return "ENDVIEW";
            else if (_ControlType == SMFrontControlType.FieldSet) return "FIELDSET";
            else if (_ControlType == SMFrontControlType.Hidden) return "HIDDEN";
            else if (_ControlType == SMFrontControlType.HorizontalLine) return "HLINE";
            else if (_ControlType == SMFrontControlType.Image) return "IMAGE";
            else if (_ControlType == SMFrontControlType.Import) return "IMPORT";
            else if (_ControlType == SMFrontControlType.Include) return "INCLUDE";
            else if (_ControlType == SMFrontControlType.Information) return "INFO";
            else if (_ControlType == SMFrontControlType.Literal) return "LITERAL";
            else if (_ControlType == SMFrontControlType.Location) return "LOCATION";
            else if (_ControlType == SMFrontControlType.Memo) return "MEMO";
            else if (_ControlType == SMFrontControlType.Meta) return "META";
            else if (_ControlType == SMFrontControlType.Number) return "NUMBER";
            else if (_ControlType == SMFrontControlType.Panel) return "PANEL";
            else if (_ControlType == SMFrontControlType.Print) return "PRINT";
            else if (_ControlType == SMFrontControlType.RadioButton) return "RADIO";
            else if (_ControlType == SMFrontControlType.Related) return "RELATED";
            else if (_ControlType == SMFrontControlType.Remark) return "REMARK";
            else if (_ControlType == SMFrontControlType.Repeat) return "REPEAT";
            else if (_ControlType == SMFrontControlType.Script) return "SCRIPT";
            else if (_ControlType == SMFrontControlType.Select) return "SELECT";
            else if (_ControlType == SMFrontControlType.Tab) return "TAB";
            else if (_ControlType == SMFrontControlType.Text) return "TEXT";
            else if (_ControlType == SMFrontControlType.Time) return "TIME";
            else if (_ControlType == SMFrontControlType.Upload) return "UPLOAD";
            else if (_ControlType == SMFrontControlType.VerticalSpacing) return "VSPACE";
            else if (_ControlType == SMFrontControlType.View) return "VIEW";
            else if (_ControlType == SMFrontControlType.Warning) return "WARNING";
            else if (_ControlType == SMFrontControlType.YesNo) return "YESNO";
            else return "";
        }

        /// <summary>Return front control type represented by string.</summary>
        public static SMFrontControlType ToType(string _ControlType)
        {
            _ControlType = _ControlType.ToUpper().Trim();
            if (_ControlType == "ACCORDION") return SMFrontControlType.Accordion;
            else if (_ControlType == "ATTACHMENT") return SMFrontControlType.Attachment;
            else if (_ControlType == "BLOB") return SMFrontControlType.Blob;
            else if (_ControlType == "BUTTON") return SMFrontControlType.Button;
            else if (_ControlType == "CAPTION") return SMFrontControlType.Caption;
            else if (_ControlType == "CHECK") return SMFrontControlType.Check;
            else if (_ControlType == "CHIPS") return SMFrontControlType.Chips;
            else if (_ControlType == "DATE") return SMFrontControlType.Date;
            else if (_ControlType == "DETAILS") return SMFrontControlType.Details;
            else if (_ControlType == "ENDACCORDION") return SMFrontControlType.EndAccordion;
            else if (_ControlType == "ENDDETAILS") return SMFrontControlType.EndDetails;
            else if (_ControlType == "ENDPANEL") return SMFrontControlType.EndPanel;
            else if (_ControlType == "ENDREPEAT") return SMFrontControlType.EndRepeat;
            else if (_ControlType == SMFrontControlType.EndRow) return "ENDROW";
            else if (_ControlType == SMFrontControlType.EndSet) return "ENDSET";
            else if (_ControlType == SMFrontControlType.EndTab) return "ENDTAB";
            else if (_ControlType == SMFrontControlType.EndView) return "ENDVIEW";
            else if (_ControlType == SMFrontControlType.FieldSet) return "FIELDSET";
            else if (_ControlType == SMFrontControlType.Hidden) return "HIDDEN";
            else if (_ControlType == SMFrontControlType.HorizontalLine) return "HLINE";
            else if (_ControlType == SMFrontControlType.Image) return "IMAGE";
            else if (_ControlType == SMFrontControlType.Import) return "IMPORT";
            else if (_ControlType == SMFrontControlType.Include) return "INCLUDE";
            else if (_ControlType == SMFrontControlType.Information) return "INFO";
            else if (_ControlType == SMFrontControlType.Literal) return "LITERAL";
            else if (_ControlType == SMFrontControlType.Location) return "LOCATION";
            else if (_ControlType == SMFrontControlType.Memo) return "MEMO";
            else if (_ControlType == SMFrontControlType.Meta) return "META";
            else if (_ControlType == SMFrontControlType.Number) return "NUMBER";
            else if (_ControlType == SMFrontControlType.Panel) return "PANEL";
            else if (_ControlType == SMFrontControlType.Print) return "PRINT";
            else if (_ControlType == SMFrontControlType.RadioButton) return "RADIO";
            else if (_ControlType == SMFrontControlType.Related) return "RELATED";
            else if (_ControlType == SMFrontControlType.Remark) return "REMARK";
            else if (_ControlType == SMFrontControlType.Repeat) return "REPEAT";
            else if (_ControlType == SMFrontControlType.Script) return "SCRIPT";
            else if (_ControlType == SMFrontControlType.Select) return "SELECT";
            else if (_ControlType == SMFrontControlType.Tab) return "TAB";
            else if (_ControlType == SMFrontControlType.Text) return "TEXT";
            else if (_ControlType == SMFrontControlType.Time) return "TIME";
            else if (_ControlType == SMFrontControlType.Upload) return "UPLOAD";
            else if (_ControlType == SMFrontControlType.VerticalSpacing) return "VSPACE";
            else if (_ControlType == SMFrontControlType.View) return "VIEW";
            else if (_ControlType == SMFrontControlType.Warning) return "WARNING";
            else if (_ControlType == SMFrontControlType.YesNo) return "YESNO";
            else return SMFrontControlType.None;
        }

        #endregion

        /* */

    }

    /* */

}
