/*  ===========================================================================
 *  
 *  File:       SMFrontControl.cs
 *  Version:    2.0.34
 *  Date:       July 2024
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
using System;
using System.Collections.Generic;
using System.Data;

namespace SMFrontSystem
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

        /// <summary>SM session instance.</summary>
        private SMFront SM = null;

        /// <summary>Memo field (text area) max length.</summary>
        public const int MEMO_MAX_LEN = 16384;

        /// <summary>Number field max length.</summary>
        public const int NUMBER_MAX_LEN = 18;

        /// <summary>Text field max length.</summary>
        public const int TEXT_MAX_LEN = 254;

        /// <summary>Control data max length.</summary>
        private int length = 0;

        /// <summary>Control class list.</summary>
        private string pClass = null;

        /// <summary>Control nullable flag.</summary>
        private bool? pNullable = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set control alias.</summary>
        public string Alias { get; set; } = "";

        /// <summary>Get or set value changed flag.</summary>
        public bool Changed { get; set; } = false;

        /// <summary>Get or set CSS class for control.</summary>
        public string Class
        {
            get
            {
                if (pClass==null) pClass = Parameters.ValueOf("CLASS");
                return pClass;
            }
            set { pClass = value; }
        }

        /// <summary>Get or set control table related column name.</summary>
        public string ColumnName { get; set; } = "";

        /// <summary>Get or set control table related API column name.</summary>
        public string ColumnAPI { get; set; } = "";

        /// <summary>Get or set control table related export column name.</summary>
        public string ColumnExport { get; set; } = "";

        /// <summary>Get or set parent control type.</summary>
        public SMFrontControlType ControlType { get; set; } = SMFrontControlType.None;

        /// <summary>Get or set first value data content as byte array.</summary>
        public byte[] Data
        {
            get { return GetData(0); }
            set { SetData(0, value); }
        }

        /// <summary>Get or set debugger flag.</summary>
        public bool Debugger { get; set; } = false;

        public SMFrontControlEvents Events { get; set; } = new SMFrontControlEvents();

        /// <summary>Get or set control data format.</summary>
        public string Format { get; set; } = "";

        /// <summary>Get or set grid columns.</summary>
        public int GridColumns { get; set; } = 0;

        /// <summary>Indicate if control has values.</summary>
        public bool HasValue { get { return Values.Count > 0; } }

        /// <summary>Get or set control id.</summary>
        public int Id { get; set; } = 0;

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
                    if (ControlType == SMFrontControlType.Number) length = NUMBER_MAX_LEN;
                    else if (ControlType == SMFrontControlType.Chips) length = MEMO_MAX_LEN;
                    else if (ControlType == SMFrontControlType.Text) length = TEXT_MAX_LEN;
                    else if (ControlType == SMFrontControlType.Memo) length = MEMO_MAX_LEN;
                    else length = 0;
                }
            }
        }

        /// <summary>Get or set if value can contain null value.</summary>
        public bool Nullable 
        { 
            get
            {
                if (!pNullable.HasValue) pNullable = Parameters.BoolOf("NULL");
                return pNullable.Value;
            }
            set { pNullable = value;}
        } 

        /// <summary>Get control options.</summary>
        public string Options { get; private set; } = "";

        /// <summary>Get control parameters.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>Parent object.</summary>
        public object Parent { get; set; } = null;

        /// <summary>Get or set value required flag.</summary>
        public bool Required { get; set; } = false;

        /// <summary>Get detail current row index for control or -1 if none.</summary>
        public int Row { get; set; } = -1;

        /// <summary>Get or set short text.</summary>
        public string ShortText { get; set; } = "";

        /// <summary>Get or set control table name.</summary>
        public string TableName { get; set; } = "";

        /// <summary>Get or set control text.</summary>
        public string Text { get; set; } = "";

        /// <summary>Get or set control UID.</summary>
        public string UID { get; set; } = "";

        /// <summary>Indicate if control can manage value.</summary>
        public bool Valuable
        {
            get
            {
                return (ControlType == SMFrontControlType.Button)
                    || (ControlType == SMFrontControlType.Check)
                    || (ControlType == SMFrontControlType.Chips)
                    || (ControlType == SMFrontControlType.Date)
                    || (ControlType == SMFrontControlType.Hidden)
                    || (ControlType == SMFrontControlType.Location)
                    || (ControlType == SMFrontControlType.Memo)
                    || (ControlType == SMFrontControlType.Meta)
                    || (ControlType == SMFrontControlType.Number)
                    || (ControlType == SMFrontControlType.RadioButton)
                    || (ControlType == SMFrontControlType.Select)
                    || (ControlType == SMFrontControlType.Text)
                    || (ControlType == SMFrontControlType.Time)
                    || (ControlType == SMFrontControlType.YesNo);
            }
        }

        /// <summary>Get or set first value content as string.</summary>
        public string Value
        {
            get { return GetValue(0); }
            set { SetValue(0, value); }
        }

        /// <summary>Get values collection</summary>
        public List<object> Values { get; private set; } = new List<object>();

        /// <summary>Get or set control version.</summary>
        public int Version { get; set; } = 0;

        /// <summary>Get or set control view index.</summary>
        public int ViewIndex { get; set; } = 0;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontControl(SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControl(SMFrontControl _OtherInstance, SMFront _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControl(SMDataset _Dataset, SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM); 
            InitializeInstance();
            Read(_Dataset);
        }

        /// <summary>Initialize control instance.</summary>
        private void InitializeInstance()
        {
            Parameters = new SMDictionary(SM);
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Clear control instance.</summary>
        public SMFrontControl Assign(SMFrontControl _OtherInstance)
        {
            int i;
            if (SM == null) SM = _OtherInstance.SM;
            Alias = _OtherInstance.Alias;
            Changed = _OtherInstance.Changed;
            Class = _OtherInstance.Class;
            ColumnName = _OtherInstance.ColumnName;
            ColumnAPI = _OtherInstance.ColumnAPI;
            ColumnExport = _OtherInstance.ColumnExport;   
            ControlType = _OtherInstance.ControlType;
            Debugger = _OtherInstance.Debugger;
            Events.Assign(_OtherInstance.Events);
            Format = _OtherInstance.Format;
            GridColumns = _OtherInstance.GridColumns;
            Id = _OtherInstance.Id;
            Length = _OtherInstance.Length;
            Nullable = _OtherInstance.Nullable;
            Options = _OtherInstance.Options;
            Parameters.Assign(_OtherInstance.Parameters);
            Parent = _OtherInstance.Parent;
            Required = _OtherInstance.Required;
            Row = _OtherInstance.Row;
            ShortText = _OtherInstance.ShortText;
            TableName = _OtherInstance.TableName;
            Text = _OtherInstance.Text;
            //
            Values.Clear();
            for (i = 0; i < _OtherInstance.Values.Count; i++) Values.Add(_OtherInstance.Values[i]);
            //
            Version = _OtherInstance.Version;
            ViewIndex = _OtherInstance.ViewIndex;
            return this;
        }

        /// <summary>Return front HTML attributes assigned to control.</summary>
        public string Attributes(string _Extension = "")
        {
            string rslt = " " + SM.AttributePrefix + "id=" + SM.Quote2(HtmlId());
            if (Parent != null)
            {
                if (Parent is SMFrontControl) rslt += " " + SM.AttributePrefix + "parent=" + SM.Quote2(((SMFrontControl)Parent).HtmlId());
            }
            if (Row > -1) rslt += " "+ SM.AttributePrefix + "row=" + SM.Quote2(Row.ToString());
            rslt += " "+ SM.AttributePrefix + "id=" + SM.Quote2(HtmlId(_Extension));
            if (!SM.Empty(ColumnName)) rslt += " "+ SM.AttributePrefix + "column=" + SM.Quote2(ColumnName.Trim());
            if (!SM.Empty(ControlType)) rslt += " "+ SM.AttributePrefix + "type=" + SM.Quote2(SMFrontControl.ToType(ControlType));
            if (!SM.Empty(Alias)) rslt += " "+ SM.AttributePrefix + "alias=" + SM.Quote2(Alias.Trim());
            if (!SM.Empty(Format)) rslt += " "+ SM.AttributePrefix + "format=" + SM.Quote2(Format.Trim());
            if (Nullable) rslt += " "+ SM.AttributePrefix + "null=" + SM.Quote2("1");
            if (!SM.Empty(_Extension)) rslt+= " "+ SM.AttributePrefix + "ext=" + SM.Quote2(_Extension.Trim());
            return rslt;
        }

        /// <summary>Clear control instance.</summary>
        public SMFrontControl Clear()
        {
            Alias = "";
            Changed = false;
            pClass = null;
            ColumnName = "";
            ColumnAPI = "";
            ColumnExport = "";
            ControlType = SMFrontControlType.None;
            Debugger = false;
            Events.Clear();
            Format = "";
            GridColumns = 0;
            Id = 0;
            Length = 0;
            pNullable = null;
            Options = "";
            Parameters.Clear();
            Parent = null;
            Required = false;
            Row = 0;
            ShortText = "";
            TableName = "";
            Text = "";
            Values.Clear();
            Version = 0;
            ViewIndex = 0;
            return this;
        }

        /// <summary>Get data value at index as byte array or null if fail.</summary>
        public byte[] GetData(int _ValueIndex = 0)
        {
            if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
            {
                if (Values[_ValueIndex] == null) return null;
                else if (Values[_ValueIndex] is byte[]) return (byte[])Values[_ValueIndex];
                else return null;
            }
            else return null;
        }

        /// <summary>Get blob value at index as byte array or null if fail.</summary>
        public string GetValue(int _ValueIndex = 0)
        {
            if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
            {
                if (Values[_ValueIndex] == null) return "";
                else if ((Values[_ValueIndex] != null) && (Values[_ValueIndex] != DBNull.Value)) return Values[_ValueIndex].ToString();
                else return "";
            }
            else return "";
        }

        /// <summary>Return html id assigned to control.</summary>
        public string HtmlId(string _Extension = "")
        {
            string rslt = "sm_" + Id.ToString();
            if (Parent != null)
            {
                if (Parent is SMFrontControl) rslt += '_' + ((SMFrontControl)Parent).Id.ToString();
            }
            if (Row > -1) rslt += '_' + Row.ToString();
            if (!SM.Empty(_Extension)) rslt += '_' + _Extension.Trim();
            return rslt;
        }

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Read(SMDataset _Dataset)
        {
            return Read(_Dataset.Row);
        }

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Read(DataRow _DataRow)
        {
            try
            {
                Clear();
                if (_DataRow != null)
                {
                    Alias = SM.ToStr(_DataRow["Alias"]);
                    ColumnName = SM.ToStr(_DataRow["ColumnName"]);
                    ColumnAPI = SM.ToStr(_DataRow["ColumnAPI"]);
                    ColumnExport = SM.ToStr(_DataRow["ColumnExport"]);
                    ControlType = ToType(SM.ToStr(_DataRow["ControlType"]));
                    Debugger = SM.ToBool(_DataRow["Debugger"]);
                    Events.Read(_DataRow);
                    Format = SM.ToStr(_DataRow["Format"]);
                    GridColumns = SM.ToInt(_DataRow["GridColumns"]);
                    Id = SM.ToInt(_DataRow["IdControl"]);
                    Length = SM.ToInt(_DataRow["Length"]);
                    Options = SM.ToStr(_DataRow["Options"]);
                    Parameters.Clear();
                    Parameters.FromParameters(SM.ToStr(_DataRow["Parameters"]));
                    Required = SM.ToBool(_DataRow["Required"]);
                    ShortText = SM.ToStr(_DataRow["ShortText"]);
                    TableName = SM.ToStr(_DataRow["TableName"]);
                    Text = SM.ToStr(_DataRow["Text"]);
                    UID = SM.ToStr(_DataRow["UIDControl"]);
                    Version = SM.ToInt(_DataRow["Version"]);
                    ViewIndex = SM.ToInt(_DataRow["ViewIndex"]);
                }
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Set data byte array value at first index and return true if succeed.</summary>
        public bool SetData(byte[] _Value)
        {
            return SetData(0, _Value);
        }

        /// <summary>Set data byte array value at index and return true if succeed.</summary>
        public bool SetData(int _ValueIndex, byte[] _Value)
        {
            try
            {
                if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
                {
                    Values[_ValueIndex] = _Value;
                    return true;
                }
                else if (_ValueIndex >= Values.Count)
                {
                    while (Values.Count <= _ValueIndex) Values.Add(null);
                    Values[_ValueIndex] = _Value;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex); 
                return false; 
            }
        }

        /// <summary>Set string value at first index and return true if succeed.</summary>
        public bool SetValue(string _Value)
        {
            return SetValue(0, _Value);
        }

        /// <summary>Set string value at index and return true if succeed.</summary>
        public bool SetValue(int _ValueIndex, string _Value)
        {
            try
            {
                if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
                {
                    Values[_ValueIndex] = SM.Format(_Value, Format);
                    return true;
                }
                else if (_ValueIndex >= Values.Count)
                {
                    while (Values.Count <= _ValueIndex) Values.Add(null);
                    Values[_ValueIndex] = SM.Format(_Value, Format);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Compare front controls by name.</summary>
        public static int CompareByAlias(object _A, object _B)
        {
            return ((SMFrontControl)_A).Alias.CompareTo(((SMFrontControl)_B).Alias);
        }

        /// <summary>Compare front controls by column name.</summary>
        public static int CompareByColumnName(object _A, object _B)
        {
            return ((SMFrontControl)_A).ColumnName.CompareTo(((SMFrontControl)_B).ColumnName);
        }

        /// <summary>Compare front controls by id.</summary>
        public static int CompareById(object _A, object _B)
        {
            return ((SMFrontControl)_A).Id.CompareTo(((SMFrontControl)_B).Id);
        }

        /// <summary>Compare front controls by order.</summary>
        public static int CompareByViewIndex(object _A, object _B)
        {
            return ((SMFrontControl)_A).ViewIndex.CompareTo(((SMFrontControl)_B).ViewIndex);
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
            else if (_ControlType == SMFrontControlType.Constant) return "CONST";
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
            else if (_ControlType == "CONST") return SMFrontControlType.Constant;
            else if (_ControlType == "DATE") return SMFrontControlType.Date;
            else if (_ControlType == "DETAILS") return SMFrontControlType.Details;
            else if (_ControlType == "ENDACCORDION") return SMFrontControlType.EndAccordion;
            else if (_ControlType == "ENDDETAILS") return SMFrontControlType.EndDetails;
            else if (_ControlType == "ENDPANEL") return SMFrontControlType.EndPanel;
            else if (_ControlType == "ENDREPEAT") return SMFrontControlType.EndRepeat;
            else if (_ControlType == "ENDROW") return SMFrontControlType.EndRow;
            else if (_ControlType == "ENDSET") return SMFrontControlType.EndSet;
            else if (_ControlType == "ENDTAB") return SMFrontControlType.EndTab;
            else if (_ControlType == "ENDVIEW") return SMFrontControlType.EndView;
            else if (_ControlType == "FIELDSET") return SMFrontControlType.FieldSet;
            else if (_ControlType == "HIDDEN") return SMFrontControlType.Hidden;
            else if (_ControlType == "HLINE") return SMFrontControlType.HorizontalLine;
            else if (_ControlType == "IMAGE") return SMFrontControlType.Image;
            else if (_ControlType == "IMPORT") return SMFrontControlType.Import;
            else if (_ControlType == "INCLUDE") return SMFrontControlType.Include;
            else if (_ControlType == "INFO") return SMFrontControlType.Information;
            else if (_ControlType == "LITERAL") return SMFrontControlType.Literal;
            else if (_ControlType == "LOCATION") return SMFrontControlType.Location;
            else if (_ControlType == "MEMO") return SMFrontControlType.Memo;
            else if (_ControlType == "META") return SMFrontControlType.Meta;
            else if (_ControlType == "NUMBER") return SMFrontControlType.Number;
            else if (_ControlType == "PANEL") return SMFrontControlType.Panel;
            else if (_ControlType == "PRINT") return SMFrontControlType.Print;
            else if (_ControlType == "RADIO") return SMFrontControlType.RadioButton;
            else if (_ControlType == "RELATED") return SMFrontControlType.Related;
            else if (_ControlType == "REMARK") return SMFrontControlType.Remark;
            else if (_ControlType == "REPEAT") return SMFrontControlType.Repeat;
            else if (_ControlType == "SCRIPT") return SMFrontControlType.Script;
            else if (_ControlType == "SELECT") return SMFrontControlType.Select;
            else if (_ControlType == "TAB") return SMFrontControlType.Tab;
            else if (_ControlType == "TEXT") return SMFrontControlType.Text;
            else if (_ControlType == "TIME") return SMFrontControlType.Time;
            else if (_ControlType == "UPLOAD") return SMFrontControlType.Upload;
            else if (_ControlType == "VSPACE") return SMFrontControlType.VerticalSpacing;
            else if (_ControlType == "VIEW") return SMFrontControlType.View;
            else if (_ControlType == "WARNING") return SMFrontControlType.Warning;
            else if (_ControlType == "YESNO") return SMFrontControlType.YesNo;
            else return SMFrontControlType.None;
        }

        #endregion

        /* */

    }

    /* */

}
