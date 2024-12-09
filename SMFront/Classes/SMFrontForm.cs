/*  ===========================================================================
 *  
 *  File:       SMFrontForm.cs
 *  Version:    2.0.95
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront form management class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System;
using System.Collections.Generic;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMCode form management class.</summary>
    public class SMFrontForm
	{

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMFront SM = null;

        /// <summary>Hard deletion flag.</summary>
        private bool? hardDeletion = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set form id.</summary>
        public string IdForm { get; set; } = "";

        /// <summary>Get form controls collection.</summary>
        public SMFrontControls Controls { get; private set; } = null;

        /// <summary>Get or set form debugger flag.</summary>
        public bool Debugger { get; set; } = false;

        /// <summary>Get or set form deleted flag.</summary>
        public bool Deleted { get; set; } = false;

        /// <summary>Get current delections collection.</summary>
        public List<int> Deletions { get; private set; } = null;

        /// <summary>Get current form document.</summary>
        public SMFrontDocument Document { get; private set; } = null;

        /// <summary>Get or set form enabled flag.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Get form javascript events.</summary>
        public SMFrontFormEvents Events { get; private set; } = null;

        /// <summary>Get form code provided events.</summary>
        public SMFrontFormEvents EventsFN { get; private set; } = null;

        /// <summary>Get form stored procedure events.</summary>
        public SMFrontFormEvents EventsSP { get; private set; } = null;

        /// <summary>Get or set form expiration datetime.</summary>
        public DateTime Expiration { get; set; } = DateTime.MaxValue;

        /// <summary>Get or set current form type.</summary>
        public string FormType { get; set; } = "";

        /// <summary>Get or set current form icon path.</summary>
        public string Icon { get; set; } = "";

        /// <summary>Get or set current form note.</summary>
        public string Note { get; set; } = "";

        /// <summary>Get or set form opening datetime.</summary>
        public DateTime Opening { get; set; } = DateTime.MinValue;

        /// <summary>Get parameters dictionary.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>Return hard deletion flag.</summary>
        public bool HardDeletion
        {
            get
            {
                if (hardDeletion == null) hardDeletion = Parameters.BoolOf("HARDDELETION", false);
                return hardDeletion.Value;
            }
            set { hardDeletion = value; }
        }

        /// <summary>Get state dictionary.</summary>
        public SMDictionary State { get; private set; } = null;

        /// <summary>Get or set current form related table name.</summary>
        public string TableName { get; set; } = "";

        /// <summary>Get or set current form text.</summary>
        public string Text { get; set; } = "";

        /// <summary>Get or set current form version.</summary>
        public int Version { get; set; } = 0;

        /// <summary>Get or set current form view width.</summary>
        public int ViewWidth { get; set; } = 0;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontForm(SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontForm(SMFrontForm _Form, SMFront _SM = null)
        {
            if (_SM==null) _SM = _Form.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_Form);
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
            Controls = new SMFrontControls(SM);
            Deletions = new List<int>();
            Document = new SMFrontDocument(this, SM);
            Events = new SMFrontFormEvents(SM);
            EventsFN = new SMFrontFormEvents(SM);
            EventsSP = new SMFrontFormEvents(SM);
            Parameters = new SMDictionary(SM);
            State = new SMDictionary(SM);
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMFrontForm _Form)
        {
            int i;
            IdForm = _Form.IdForm;
            Document.Assign(_Form.Document);
            Controls.Assign(_Form.Controls);
            Debugger = _Form.Debugger;
            Deleted = _Form.Deleted;
            Deletions.Clear();
            for (i=0; i<_Form.Deletions.Count; i++) Deletions.Add(_Form.Deletions[i]);
            Enabled = _Form.Enabled;
            Events.Assign(_Form.Events);
            EventsFN.Assign(_Form.EventsFN);
            EventsSP.Assign(_Form.EventsSP);
            Expiration = _Form.Expiration;
            Icon = _Form.Icon;
            Note = _Form.Note;
            Opening = _Form.Opening;
            Parameters.Assign(_Form.Parameters);
            State.Assign(_Form.State);
            TableName = _Form.TableName;
            Text = _Form.Text;
            FormType = _Form.FormType;
            Version = _Form.Version;
            ViewWidth = _Form.ViewWidth;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            IdForm = "";
            Document.Clear();
            Controls.Clear();
            Debugger = false;
            Deleted = false;
            Deletions.Clear();
            Enabled = true;
            Events.Clear();
            EventsFN.Clear();
            EventsSP.Clear();
            Expiration = DateTime.MaxValue;
            Icon = "";
            Note = "";
            Opening = DateTime.MinValue;
            Parameters.Clear();
            State.Clear();
            TableName = "";
            Text = "";
            FormType = "";
            Version = 0;
            ViewWidth = 0;
        }

        /// <summary>Clear item.</summary>
        public bool Read(SMDataset _Dataset)
        {
            try
            {
                Clear();
                if (_Dataset != null)
                {
                    IdForm = _Dataset.FieldStr("IdForm");
                    Debugger = _Dataset.FieldBool("Debugger");
                    Deleted = _Dataset.FieldBool("Deleted");
                    Enabled = _Dataset.FieldBool("Enabled"); ;
                    Events.Read(_Dataset);
                    EventsFN.Read(_Dataset, "FN_");
                    EventsSP.Read(_Dataset, "SP_");
                    Expiration = _Dataset.FieldDateTime("Expiration");
                    Icon = _Dataset.FieldStr("Icon");
                    Note = _Dataset.FieldStr("Note");
                    Opening = _Dataset.FieldDateTime("Opening");
                    Parameters.FromParameters(_Dataset.FieldStr("Parameters"));
                    TableName = _Dataset.FieldStr("TableName");
                    Text = _Dataset.FieldStr("Text");
                    FormType = _Dataset.FieldStr("FormType");
                    Version = _Dataset.FieldInt("Version");
                    ViewWidth = 0;
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

        /// <summary>Clear item.</summary>
        public bool Write(SMDataset _Dataset)
        {
            try
            {
                if (_Dataset != null)
                {
                    _Dataset.Assign("IdForm", IdForm);
                    _Dataset.Assign("Debugger", Debugger);
                    _Dataset.Assign("Deleted", Deleted);
                    _Dataset.Assign("Enabled", Enabled);
                    Events.Write(_Dataset);
                    EventsFN.Write(_Dataset, "FN_");
                    EventsSP.Write(_Dataset, "SP_");
                    _Dataset.Assign("Expiration", Expiration);
                    _Dataset.Assign("Icon", Note);
                    _Dataset.Assign("Note", Note);
                    _Dataset.Assign("Opening", Opening);
                    _Dataset.Assign("Parameters", Parameters.ToParameters());
                    _Dataset.Assign("TableName", TableName);
                    _Dataset.Assign("Text", Text);
                    _Dataset.Assign("FormType", FormType);
                    _Dataset.Assign("Version", Version);
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

        #region Methods - Delete

        /*  ===================================================================
         *  Methods - Delete
         *  ===================================================================
         */

        #endregion

        /* */

        #region Methods - Load

        /*  ===================================================================
         *  Methods - Load
         *  ===================================================================
         */

        /// <summary>Load form structure and data if document is specified. Return true if succeed.</summary>
        public bool Load(string _IdForm, int _IdDocument = 0)
        {
            bool rslt = false;
            SMDataset ds;
            if (_IdForm != null)
            {
                _IdForm = _IdForm.Trim();
                if (_IdForm.Length > 0)
                {
                    try
                    {
                        Clear();
                        ds = new SMDataset("MAIN");
                        if (ds.Open("SELECT * FROM " + SMDefaults.FormsTableName + " WHERE (IdForm=" + SM.Quote(_IdForm) + ")" + SM.SqlNotDeleted()))
                        {
                            if (!ds.Eof)
                            {
                                rslt = true;
                                Read(ds);
                            }
                            ds.Close();
                        }
                        if (rslt) rslt = Controls.LoadByIdForm(IdForm);
                        if (rslt && (_IdDocument > 0))
                        {
                            if (Document.Load(_IdDocument))
                            {
                                if (SM.Empty(TableName)) rslt = LoadValues(_IdDocument);
                                else rslt = LoadTable(_IdDocument, TableName, Controls.Childs);
                            }
                            else rslt = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex);
                        rslt = false;
                    }
                }
            }
            return rslt;
        }

        /// <summary>Load document data from values collection table. Return true if succeed.</summary>
        public bool LoadValues(int _IdDocument)
        {
            int rowIndex;
            bool rslt = false;
            string sql;
            SMDataset ds;
            SMFrontControl control = null;
            if (_IdDocument > 0)
            {
                try
                {
                    ds = new SMDataset("MAIN");
                    sql = "SELECT * FROM " + SMDefaults.ValuesTableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                        + ")AND(IdDocument=" + _IdDocument.ToString() + ")"
                        + SM.SqlNotDeleted() + " ORDER BY IdControl,RowIndex";
                    Controls.ClearValues();
                    if (ds.Open(sql))
                    {
                        rslt = true;
                        while (!ds.Eof)
                        {
                            control = Controls.FindById(ds.FieldInt("IdControl"));
                            if (control != null)
                            {
                                if (control.Valuable)
                                {
                                    rowIndex = ds.FieldInt("RowIndex");
                                    if (rowIndex > -1)
                                    {
                                        if (control.IsBlob) control.SetBlob(rowIndex, ds.FieldBlob("Blob"), false);
                                        else control.SetValue(rowIndex, ds.FieldStr("Value"), false);
                                    }
                                }
                            }
                            ds.Next();
                        }
                        ds.Close();
                    }
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    rslt = false;
                }
            }
            return rslt;
        }

        /// <summary>Load document data from table. Return true if succeed.</summary>
        public bool LoadTable(int _IdDocument, string _TableName, List<SMFrontControl> _Controls, string _OrderBy = "")
        {
            int i, rowIndex;
            bool rslt = false, hasRowIndex;
            string sql;
            SMDataset ds;
            SMFrontControl control = null;
            if (_IdDocument > 0)
            {
                try
                {
                    ds = new SMDataset("MAIN");
                    sql = "SELECT * FROM " + _TableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                        + ")AND(IdDocument=" + _IdDocument.ToString() + ")" + SM.SqlNotDeleted();
                    if (!SM.Empty(_OrderBy)) sql += " ORDER BY " + _OrderBy;
                    if (ds.Open(sql))
                    {
                        hasRowIndex = ds.IsField("RowIndex");
                        Controls.ClearValues();
                        while (!ds.Eof)
                        {
                            rslt = true;
                            if (hasRowIndex) rowIndex = ds.FieldInt("RowIndex");
                            else rowIndex = 0;
                            i = 0;
                            while (i < _Controls.Count)
                            {
                                control = _Controls[i];
                                if (control.ControlType == SMFrontControlType.Details)
                                {
                                    if (!LoadTable(_IdDocument, control.TableName, control.Childs, control.Parameters.ValueOf("ORDERBY", "RowIndex"))) rslt = false;
                                }
                                else if (!SM.Empty(control.ColumnName) && control.Valuable && (SM.Empty(control.TableName) || (control.TableName == TableName)))
                                {
                                    if (control.IsBlob) control.SetBlob(rowIndex, ds.FieldBlob(control.ColumnName), false);
                                    else control.SetValue(rowIndex, ds.FieldStr(control.ColumnName), false);
                                }
                                i++;
                            }
                            ds.Next();
                        }
                        ds.Close();
                    }
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    rslt = false;
                }
            }
            return rslt;
        }

        #endregion

        /* */

        #region Methods - Save

        /*  ===================================================================
         *  Methods - Save
         *  ===================================================================
         */

        /// <summary>Save document data contents. Return document id or -1 if fails.</summary>
        public int Save()
        {
            int rslt = -1;
            if (Document.IdDocument > 0)
            {
                rslt = Document.Save();
                if (rslt > 0)
                {
                    Controls.SetChanges(true);
                    if (SM.Empty(TableName))
                    {
                        if (!SaveValues()) rslt = -1;
                    }
                    else if (!SaveTable(TableName, Controls.Childs)) rslt = -1;
                }
            }
            return rslt;
        }

        /// <summary>Save document data to values collection table. Return true if succeed.</summary>
        public bool SaveValues()
        {
            bool rslt = false;
            int i, j, rowIndex;
            string sql;
            SMDataset ds;
            SMFrontControl control = null;
            if (Document.IdDocument > 0)
            {
                try
                {
                    ds = new SMDataset("MAIN");
                    //
                    // update all existing contents
                    //
                    sql = "SELECT * FROM "+ SMDefaults.ValuesTableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                        + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")"
                        + SM.SqlNotDeleted() + " ORDER BY IdControl,RowIndex";
                    if (ds.Open(sql))
                    {
                        rslt = true;
                        while (!ds.Eof)
                        {
                            if (ds.Edit())
                            {
                                control = Controls.FindById(ds.FieldInt("IdControl"));
                                if (control != null)
                                {
                                    if (control.Valuable)
                                    {
                                        rowIndex = ds.FieldInt("RowIndex");
                                        if ((rowIndex > -1) && (rowIndex < control.Values.Count))
                                        {
                                            if (control.IsBlob) ds.Assign("Blob", control.GetBlob(rowIndex, false));
                                            else ds.Assign("Value", control.GetValue(rowIndex, false));
                                        }
                                        else ds.Assign("Deleted", 1);
                                    }
                                    else ds.Assign("Deleted", 1);
                                }
                                else ds.Assign("Deleted", 1);
                                if (!ds.Post())
                                {
                                    ds.Cancel();
                                    rslt = false;
                                }
                            }
                            else rslt = false;
                            ds.Next();
                        }
                        ds.Close();
                        //
                        // add new contents
                        //
                        sql = "SELECT * FROM " + SMDefaults.ValuesTableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                            + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")AND(IdValue<0)";
                        if (ds.Open(sql))
                        {
                            for (i = 0; i < Controls.Count; i++)
                            {
                                control = Controls[i];
                                if (control.Valuable)
                                {
                                    for (j = 0; j < control.Values.Count; j++)
                                    {
                                        if (control.Values[j].Changed)
                                        {
                                            if (ds.Append())
                                            {
                                                ds.Assign("IdDocument", Document.IdDocument);
                                                ds.Assign("IdForm", Document.Form.IdForm);
                                                ds.Assign("IdControl", control.IdControl);
                                                ds.Assign("RowIndex", j);
                                                if (control.IsBlob) ds.Assign("Blob", control.Values[j].Blob);
                                                else ds.Assign("Value", control.Values[j].Value);
                                                ds.Assign("Deleted", 0);
                                                if (ds.Post()) control.Values[j].Changed = false;
                                                else
                                                {
                                                    ds.Cancel();
                                                    rslt = false;
                                                }
                                            }
                                            else rslt = false;
                                        }
                                    }
                                }
                            }
                            ds.Close();
                        }
                        else rslt = false;
                        //
                        // perform hard deletion if specified
                        //
                        if (rslt && HardDeletion)
                        {
                            sql = "DELETE FROM " + SMDefaults.ValuesTableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                                + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")AND(Deleted=1)";
                            if (ds.Exec(sql) < 0) rslt = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    rslt = false;
                }
            }
            return rslt;
        }

        /// <summary>Save document data to table. Return true if succeed.</summary>
        public bool SaveTable(string _TableName, List<SMFrontControl> _Controls)
        {
            int i;
            bool rslt = false;
            string sql;
            SMDataset ds;
            SMFrontControl control = null;
            try
            {
                if ((Document.IdDocument > 0) && (_Controls != null))
                {
                    ds = new SMDataset("MAIN");
                    sql = "SELECT * FROM " + _TableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                        + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")" + SM.SqlNotDeleted();
                    if (ds.Open(sql))
                    {
                        //
                        // insert or edit
                        //
                        if (ds.Eof)
                        {
                            rslt = ds.Append();
                            if (rslt)
                            {
                                ds.Assign("IdForm", IdForm);
                                ds.Assign("IdDocument", Document.IdDocument);
                                ds.Assign("Delete", 0);
                            }
                        }
                        else rslt = ds.Edit();
                        //
                        // write values
                        //
                        if (rslt)
                        {
                            i = 0;
                            while (i < _Controls.Count)
                            {
                                control = _Controls[i];
                                if (control != null)
                                {
                                    if (control.Valuable)
                                    {
                                        if (!SM.Empty(control.ColumnName) && (SM.Empty(control.TableName) || (control.TableName == _TableName)))
                                        {
                                            if (control.IsBlob) ds.Assign(control.ColumnName, control.GetBlob(0, false));
                                            else ds.Assign(control.ColumnName, control.GetValue(0, false));
                                        }
                                    }
                                }
                                i++;
                            }
                            if (!ds.Post())
                            {
                                ds.Cancel();
                                rslt = false;
                            }
                            ds.Close();
                            //
                            // write details
                            //
                            i = 0;
                            while (i < _Controls.Count)
                            {
                                if (control.ControlType == SMFrontControlType.Details)
                                {
                                    if (!SaveDetails(control.TableName, control.Childs, control.Parameters.ValueOf("ORDERBY", "RowIndex"))) rslt = false;
                                }
                                i++;
                            }
                        }
                        ds.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = false;
            }
            return rslt;
        }

        /// <summary>Save document details to table. Return true if succeed.</summary>
        public bool SaveDetails(string _TableName, List<SMFrontControl> _Controls, string _OrderBy = "RowIndex")
        {
            int i, j, h, rowIndex;
            bool rslt = false;
            bool[] map;
            string sql;
            SMDataset ds;
            SMFrontControl control = null;
            try
            {
                if ((Document.IdDocument > 0) && (_Controls != null))
                {
                    ds = new SMDataset("MAIN");
                    sql = "SELECT * FROM " + _TableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                        + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")" + SM.SqlNotDeleted();
                    if (!SM.Empty(_OrderBy)) sql += " ORDER BY " + _OrderBy;
                    if (ds.Open(sql))
                    {
                        rslt = true;
                        h = SM.ControlsMaxValues(_Controls);
                        map = new bool[h > 0 ? h : 1];
                        for (i = 0; i < h; i++) map[i] = true;
                        //
                        // update all existing records
                        //
                        while (!ds.Eof)
                        {
                            rowIndex = ds.FieldInt("RowIndex");
                            if (ds.Edit())
                            {
                                if ((rowIndex > -1) && (rowIndex < h))
                                {
                                    i = 0;
                                    while (i < _Controls.Count)
                                    {
                                        control = _Controls[i];
                                        if (control != null)
                                        {
                                            if (control.Valuable)
                                            {
                                                if (!SM.Empty(control.ColumnName) && (SM.Empty(control.TableName) || (control.TableName == _TableName)))
                                                {
                                                    if (control.IsBlob) ds.Assign(control.ColumnName, control.GetBlob(rowIndex, false));
                                                    else ds.Assign(control.ColumnName, control.GetValue(rowIndex, false));
                                                }
                                            }
                                        }
                                        i++;
                                    }
                                    map[rowIndex] = false;
                                }
                                else ds.Assign("Delete", 1);
                                if (!ds.Post())
                                {
                                    ds.Cancel();
                                    rslt = false;
                                }
                            }
                            else rslt = false;
                            ds.Next();
                        }
                        ds.Close();
                        //
                        // add new contents
                        //
                        sql = "SELECT * FROM " + _TableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                            + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")AND(RowIndex<0)";
                        if (ds.Open(sql))
                        {
                            for (i = 0; i < h; i++)
                            {
                                if (map[i])
                                {
                                    if (ds.Append())
                                    {
                                        ds.Assign("IdDocument", Document.IdDocument);
                                        ds.Assign("IdForm", Document.Form.IdForm);
                                        ds.Assign("IdControl", control.IdControl);
                                        ds.Assign("RowIndex", i);
                                        j = 0;
                                        while (j < Controls.Count)
                                        {
                                            control = Controls[j];
                                            if (control != null)
                                            {
                                                if (control.Valuable)
                                                {
                                                    if (!SM.Empty(control.ColumnName) && (SM.Empty(control.TableName) || (control.TableName == _TableName)))
                                                    {
                                                        if (control.IsBlob) ds.Assign(control.ColumnName, control.GetBlob(i, false));
                                                        else ds.Assign(control.ColumnName, control.GetValue(i, false));
                                                    }
                                                }
                                            }
                                            j++;
                                        }
                                        if (!ds.Post())
                                        {
                                            ds.Cancel();
                                            rslt = false;
                                        }
                                    }
                                    else rslt = false;
                                    map[i] = false;
                                }
                            }
                            ds.Close();
                        }
                        else rslt = false;
                        //
                        // perform hard deletion if specified
                        //
                        if (rslt && HardDeletion)
                        {
                            sql = "DELETE FROM " + _TableName + " WHERE (IdForm=" + SM.Quote(IdForm)
                                + ")AND(IdDocument=" + Document.IdDocument.ToString() + ")AND(Deleted=1)";
                            if (ds.Exec(sql) < 0) rslt = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = false;
            }
            return rslt;
        }

        /// <summary>Save form structure. Return true if succeed.</summary>
        public bool SaveStructure(bool _SaveControls = true)
        {
            bool rslt = false;
            SMDataset ds;
            if (!SM.Empty(IdForm))
            {
                IdForm = IdForm.Trim();
                try
                {
                    ds = new SMDataset("MAIN");
                    if (ds.Open("SELECT * FROM " + SMDefaults.FormsTableName + " WHERE (IdForm=" + SM.Quote(IdForm) + ")" + SM.SqlNotDeleted()))
                    {
                        if (ds.Eof) rslt = ds.Append();
                        else rslt = ds.Edit();
                        if (rslt)
                        {
                            if (Write(ds))
                            {
                                rslt = ds.Post();
                                if (!rslt) ds.Cancel();
                            }
                            else
                            {
                                ds.Cancel();
                                rslt = false;
                            }
                        }
                        ds.Close();
                    }
                    if (_SaveControls)
                    {
                        Controls.Save(IdForm, "MAIN", HardDeletion);
                    }
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    rslt = false;
                }
            }
            return rslt;
        }

        #endregion

        /* */

        #region Methods - JSON

        /*  ===================================================================
         *  Methods - JSON
         *  ===================================================================
         */

        /// <summary>Acquire data from JSON string. Return true if succeed.</summary>
        public bool FromJSON(string _JSON)
        {
            return true;
        }

        /// <summary>Acquire data from base 64 JSON string. Return true if succeed.</summary>
        public bool FromJSON64(string _JSON64)
        {
            return FromJSON(SM.Base64Decode(_JSON64));
        }

        /// <summary>Return JSON string with form strucure and data.</summary>
        public string ToJSON()
        {
            string rslt = "";
            return rslt;
        }

        /// <summary>Return base 64 JSON string with form strucure and data.</summary>
        public string ToJSON64()
        {
            return SM.Base64Encode(ToJSON());
        }

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Return id comparison between passed forms.</summary>
        public static int CompareById(object _A, object _B)
        {
            return ((SMFrontForm)_A).IdForm.CompareTo(((SMFrontForm)_B).IdForm);
        }

        /// <summary>Return text comparison between passed forms.</summary>
        public static int CompareByText(object _A, object _B)
        {
            return ((SMFrontForm)_A).Text.CompareTo(((SMFrontForm)_B).Text);
        }

        #endregion

        /* */

    }

    /* */

}
