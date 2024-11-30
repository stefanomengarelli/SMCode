/*  ===========================================================================
 *  
 *  File:       SMFrontControl.cs
 *  Version:    2.0.72
 *  Date:       November 2024
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
using System.Text;
using System.Text.Json;

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
        public const int MEMO_MAX_LEN = 65534;

        /// <summary>Number field max length.</summary>
        public const int NUMBER_MAX_LEN = 18;

        /// <summary>Text field max length.</summary>
        public const int TEXT_MAX_LEN = 254;

        /// <summary>Control type.</summary>
        private string controlType = SMFrontControlType.None;

        /// <summary>Control class list.</summary>
        private string cssClass = null;

        /// <summary>Control data max length.</summary>
        private int length = 0;

        /// <summary>Control nullable flag.</summary>
        private bool? nullableControl = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set control id.</summary>
        public int IdControl { get; set; } = 0;

        /// <summary>Get or set control UID.</summary>
        public string UidControl { get; set; } = "";

        /// <summary>Get or set form id.</summary>
        public string IdForm { get; set; } = "";

        /// <summary>Get or set control alias.</summary>
        public string Alias { get; set; } = "";

        /// <summary>Get or set value changed flag.</summary>
        public bool Changed { get; set; } = false;

        /// <summary>Get or set CSS class for control.</summary>
        public string Class
        {
            get
            {
                if (cssClass==null) cssClass = Parameters.ValueOf("CLASS");
                return cssClass;
            }
            set { cssClass = value; }
        }

        /// <summary>Childs controls ordered by viewindex.</summary>
        public List<SMFrontControl> Childs { get; private set; } = new List<SMFrontControl>();

        /// <summary>Get or set control table related column name.</summary>
        public string ColumnName { get; set; } = "";

        /// <summary>Get or set control view column index.</summary>
        public int ColumnView { get; set; } = 0;

        /// <summary>Get or set control table related API column name.</summary>
        public string ColumnAPI { get; set; } = "";

        /// <summary>Get or set control table related export column name.</summary>
        public string ColumnExport { get; set; } = "";

        /// <summary>Get or set parent control type.</summary>
        public string ControlType 
        { 
            get { return controlType; }
            set { controlType = value.Trim().ToUpper(); }
        } 

        /// <summary>Get or set first value data content as byte array.</summary>
        public byte[] Data
        {
            get { return GetBlob(0); }
            set { SetBlob(0, value); }
        }

        /// <summary>Get or set debugger flag.</summary>
        public bool Debugger { get; set; } = false;

        /// <summary>Get front control events.</summary>
        public SMFrontControlEvents Events { get; private set; } = new SMFrontControlEvents();

        /// <summary>Get or set control data format.</summary>
        public string Format { get; set; } = "";

        /// <summary>Get or set grid columns.</summary>
        public int GridColumns { get; set; } = 0;

        /// <summary>Indicate if control has values.</summary>
        public bool HasValue { get { return Values.Count > 0; } }

        /// <summary>Return true if control handle bytes array data.</summary>
        public bool IsBlob
        {
            get
            {
                return (ControlType == SMFrontControlType.Blob)
                    || (ControlType == SMFrontControlType.Image)
                    || (ControlType == SMFrontControlType.Upload);
            }
        }

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

        /// <summary>Get or set note.</summary>
        public string Note { get; set; } = "";

        /// <summary>Get or set if value can contain null value.</summary>
        public bool Nullable 
        { 
            get
            {
                if (!nullableControl.HasValue) nullableControl = Parameters.BoolOf("NULL");
                return nullableControl.Value;
            }
            set { nullableControl = value;}
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

        /// <summary>Get or set object tag.</summary>
        public object Tag { get; set; }

        /// <summary>Get or set control table name.</summary>
        public string TableName { get; set; } = "";

        /// <summary>Get or set control text.</summary>
        public string Text { get; set; } = "";

        /// <summary>Indicate if control can manage value.</summary>
        public bool Valuable
        {
            get
            {
                return (ControlType == SMFrontControlType.Button)
                    || (ControlType == SMFrontControlType.Blob)
                    || (ControlType == SMFrontControlType.Check)
                    || (ControlType == SMFrontControlType.Chips)
                    || (ControlType == SMFrontControlType.Date)
                    || (ControlType == SMFrontControlType.Hidden)
                    || (ControlType == SMFrontControlType.Image)
                    || (ControlType == SMFrontControlType.Location)
                    || (ControlType == SMFrontControlType.Memo)
                    || (ControlType == SMFrontControlType.Meta)
                    || (ControlType == SMFrontControlType.Number)
                    || (ControlType == SMFrontControlType.RadioButton)
                    || (ControlType == SMFrontControlType.Select)
                    || (ControlType == SMFrontControlType.Text)
                    || (ControlType == SMFrontControlType.Time)
                    || (ControlType == SMFrontControlType.Upload)
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
        public List<SMFrontValue> Values { get; private set; } = new List<SMFrontValue>();

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

        /// <summary>Assign control instance from another.</summary>
        public SMFrontControl Assign(SMFrontControl _Control)
        {
            int i;
            if (SM == null) SM = _Control.SM;
            IdControl = _Control.IdControl;
            UidControl = _Control.UidControl;
            IdForm = _Control.IdForm;
            Alias = _Control.Alias;
            Changed = _Control.Changed;
            Childs.Clear();
            for (i=0; i < _Control.Childs.Count; i++) Childs.Add(_Control.Childs[i]);
            Class = _Control.Class;
            ColumnName = _Control.ColumnName;
            ColumnView = _Control.ColumnView;
            ColumnAPI = _Control.ColumnAPI;
            ColumnExport = _Control.ColumnExport;   
            ControlType = _Control.ControlType;
            Debugger = _Control.Debugger;
            Events.Assign(_Control.Events);
            Format = _Control.Format;
            GridColumns = _Control.GridColumns;
            Length = _Control.Length;
            Nullable = _Control.Nullable;
            Note = _Control.Note;
            Options = _Control.Options;
            ViewIndex = _Control.ViewIndex;
            Parameters.Assign(_Control.Parameters);
            Parent = _Control.Parent;
            Required = _Control.Required;
            Row = _Control.Row;
            ShortText = _Control.ShortText;
            Tag = _Control.Tag;
            TableName = _Control.TableName;
            Text = _Control.Text;
            Values.Clear();
            for (i = 0; i < _Control.Values.Count; i++) Values.Add(_Control.Values[i]);
            Version = _Control.Version;
            return this;
        }

        /// <summary>Clear control instance.</summary>
        public SMFrontControl Clear()
        {
            IdControl = 0;
            UidControl = "";
            IdForm = "";
            Alias = "";
            Changed = false;
            Childs.Clear();
            cssClass = null;
            ColumnName = "";
            ColumnView = 0;
            ColumnAPI = "";
            ColumnExport = "";
            ControlType = SMFrontControlType.None;
            Debugger = false;
            Events.Clear();
            Format = "";
            GridColumns = 0;
            Length = 0;
            Note = "";
            nullableControl = null;
            Options = "";
            ViewIndex = 0;
            Parameters.Clear();
            Parent = null;
            Required = false;
            Row = 0;
            ShortText = "";
            TableName = "";
            Tag = null;
            Text = "";
            Values.Clear();
            Version = 0;
            return this;
        }

        /// <summary>Clear control values.</summary>
        public void ClearValues()
        {
            Values.Clear();
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMFrontControl)JsonSerializer.Deserialize(_JSON, null));
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Assign property from JSON64 serialization.</summary>
        public bool FromJSON64(string _JSON64)
        {
            return FromJSON(SM.Base64Decode(_JSON64));
        }

        /// <summary>Get data value at index as byte array or null if fail.</summary>
        public byte[] GetBlob(int _ValueIndex = 0, bool? _Changed = null)
        {
            if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
            {
                if (_Changed != null) Values[_ValueIndex].Changed = _Changed.Value;
                return Values[_ValueIndex].Blob;
            }
            else return null;
        }

        /// <summary>Get blob value at index as byte array or null if fail.</summary>
        public string GetValue(int _ValueIndex = 0, bool? _Changed = null)
        {
            if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
            {
                if (_Changed != null) Values[_ValueIndex].Changed = _Changed.Value;
                return Values[_ValueIndex].Value;
            }
            else return "";
        }

        /// <summary>Return html id assigned to control.</summary>
        public string HtmlId(string _Extension = "")
        {
            string rslt = "sm_" + IdControl.ToString();
            if (Row > -1) rslt += '_' + Row.ToString();
            if (!SM.Empty(_Extension)) rslt += '_' + _Extension.Trim();
            return rslt;
        }

        /// <summary>Return front HTML attributes assigned to control.</summary>
        public string HtmlAttributes(string _Extension = "", bool _IdAttribute = true, bool _CtrlAttributes = true)
        {
            string id = HtmlId(_Extension), pfx = " " + SM.AttributePrefix;
            StringBuilder sb = new StringBuilder();
            if (_IdAttribute) sb.Append(" id=" + SM.Quote2(id) + " name=" + SM.Quote2(id));
            sb.Append(pfx + "id=" + SM.Quote2(id));
            if (Parent != null)
            {
                if (Parent is SMFrontControl)
                {
                    sb.Append(pfx + "parent=" + SM.Quote2(((SMFrontControl)Parent).HtmlId()));
                }
            }
            if (Row > -1) sb.Append(pfx + "row=" + SM.Quote2(Row.ToString()));
            if (_CtrlAttributes) 
            {
                if (!SM.Empty(ColumnName)) sb.Append(pfx + "column=" + SM.Quote2(ColumnName.Trim()));
                if (!SM.Empty(ControlType)) sb.Append(pfx + "type=" + SM.Quote2(ControlType));
                if (!SM.Empty(Alias)) sb.Append(pfx + "alias=" + SM.Quote2(Alias.Trim()));
                if (!SM.Empty(Format)) sb.Append(pfx + "format=" + SM.Quote2(Format.Trim()));
                if (Nullable) sb.Append(pfx + "nullable=" + SM.Quote2("1"));
            }
            string rslt = " " + SM.AttributePrefix + "id=" + SM.Quote2(HtmlId());
            return sb.ToString();
        }

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Read(SMDataset _Dataset)
        {
            try
            {
                Clear();
                if (_Dataset != null)
                {
                    IdControl = _Dataset.FieldInt("IdControl");
                    UidControl = _Dataset.FieldStr("UidControl");
                    IdForm = _Dataset.FieldStr("IdForm");
                    Alias = _Dataset.FieldStr("Alias");
                    ColumnName = _Dataset.FieldStr("ColumnName");
                    ColumnView = _Dataset.FieldInt("ColumnView");
                    ColumnAPI = _Dataset.FieldStr("ColumnAPI");
                    ColumnExport = _Dataset.FieldStr("ColumnExport");
                    ControlType = _Dataset.FieldStr("ControlType").Trim().ToUpper();
                    Debugger = _Dataset.FieldBool("Debugger");
                    Events.Read(_Dataset);
                    Format = _Dataset.FieldStr("Format");
                    GridColumns = _Dataset.FieldInt("GridColumns");
                    Length = _Dataset.FieldInt("Length");
                    Note = _Dataset.FieldStr("Note");
                    Options = _Dataset.FieldStr("Options");
                    Parameters.FromParameters(_Dataset.FieldStr("Parameters"));
                    Required = _Dataset.FieldBool("Required");
                    ShortText = _Dataset.FieldStr("ShortText");
                    TableName = _Dataset.FieldStr("TableName");
                    Text = _Dataset.FieldStr("Text");
                    Version = _Dataset.FieldInt("Version");
                    ViewIndex = _Dataset.FieldInt("ViewIndex");
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

        /// <summary>Set data byte array value at first index and return true if succeed.</summary>
        public bool SetBlob(byte[] _Value, bool _Changed = true)
        {
            return SetBlob(0, _Value, _Changed);
        }

        /// <summary>Set data byte array value at index and return true if succeed.</summary>
        public bool SetBlob(int _ValueIndex, byte[] _Value, bool _Changed = true)
        {
            try
            {
                if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
                {
                    Values[_ValueIndex].Blob = _Value;
                    Values[_ValueIndex].Changed = _Changed;
                    return true;
                }
                else if (_ValueIndex >= Values.Count)
                {
                    while (Values.Count <= _ValueIndex) Values.Add(new SMFrontValue());
                    Values[_ValueIndex].Blob = _Value;
                    Values[_ValueIndex].Changed = _Changed;
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
        public bool SetValue(string _Value, bool _Changed = true)
        {
            return SetValue(0, _Value, _Changed);
        }

        /// <summary>Set string value at index and return true if succeed.</summary>
        public bool SetValue(int _ValueIndex, string _Value, bool _Changed = true)
        {
            try
            {
                if ((_ValueIndex > -1) && (_ValueIndex < Values.Count))
                {
                    Values[_ValueIndex].Value = SM.Format(_Value, Format);
                    Values[_ValueIndex].Changed = _Changed;
                    return true;
                }
                else if (_ValueIndex >= Values.Count)
                {
                    while (Values.Count <= _ValueIndex) Values.Add(new SMFrontValue());
                    Values[_ValueIndex].Value = SM.Format(_Value, Format);
                    Values[_ValueIndex].Changed = _Changed;
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

        /// <summary>Return JSON serialization of instance.</summary>
        public string ToJSON()
        {
            try
            {
                return JsonSerializer.Serialize(this);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return "";
            }
        }

        /// <summary>Return JSON64 serialization of instance.</summary>
        public string ToJSON64()
        {
            return SM.Base64Encode(ToJSON());
        }

        /// <summary>Write control data on current record on dataset.</summary>
        public bool Write(SMDataset _Dataset)
        {
            try
            {
                if (_Dataset != null)
                {
                    _Dataset.Assign("IdControl", IdControl);
                    _Dataset.Assign("UidControl", UidControl);
                    _Dataset.Assign("IdForm", IdForm);
                    _Dataset.Assign("Alias", Alias);
                    _Dataset.Assign("ColumnName", ColumnName);
                    _Dataset.Assign("ColumnView", ColumnView);
                    _Dataset.Assign("ColumnAPI", ColumnAPI);
                    _Dataset.Assign("ColumnExport", ColumnExport);
                    _Dataset.Assign("ControlType", ControlType);
                    _Dataset.Assign("Debugger", Debugger);
                    Events.Write(_Dataset);
                    _Dataset.Assign("Format", Format);
                    _Dataset.Assign("GridColumns", GridColumns);
                    _Dataset.Assign("Length", Length);
                    _Dataset.Assign("Note", Note);
                    _Dataset.Assign("Options", Options);
                    _Dataset.Assign("Parameters", Parameters.ToParameters());
                    _Dataset.Assign("Required", Required);
                    _Dataset.Assign("ShortText", ShortText);
                    _Dataset.Assign("TableName", TableName);
                    _Dataset.Assign("Text", Text);
                    _Dataset.Assign("Version", Version);
                    _Dataset.Assign("ViewIndex", ViewIndex);
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
            return ((SMFrontControl)_A).IdControl.CompareTo(((SMFrontControl)_B).IdControl);
        }

        /// <summary>Compare front controls by view index.</summary>
        public static int CompareByViewIndex(object _A, object _B)
        {
            return ((SMFrontControl)_A).ViewIndex.CompareTo(((SMFrontControl)_B).ViewIndex);
        }

        #endregion

        /* */

    }

    /* */

}
