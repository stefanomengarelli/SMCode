/*  ===========================================================================
 *  
 *  File:       SMFrontForm.cs
 *  Version:    2.0.82
 *  Date:       November 2024
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

        /// <summary>Get or set current form icon path.</summary>
        public string Icon { get; set; } = "";

        /// <summary>Get or set current form note.</summary>
        public string Note { get; set; } = "";

        /// <summary>Get or set form opening datetime.</summary>
        public DateTime Opening { get; set; } = DateTime.MinValue;

        /// <summary>Get parameters dictionary.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>Get form render.</summary>
        public SMFrontRender Render { get; private set; } = null;

        /// <summary>Get state dictionary.</summary>
        public SMDictionary State { get; private set; } = null;

        /// <summary>Get or set current form related table name.</summary>
        public string TableName { get; set; } = "";

        /// <summary>Get or set current form text.</summary>
        public string Text { get; set; } = "";

        /// <summary>Get or set current form type.</summary>
        public string FormType { get; set; } = "";

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
            Render = new SMFrontRender(this, SM);
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

        #region Methods - Load

        /*  ===================================================================
         *  Methods - Load
         *  ===================================================================
         */

        /// <summary>Load form structure and data if document is specified.</summary>
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
                        ds = new SMDataset("MAIN");
                        if (ds.Open("SELECT * FROM sm_forms WHERE (IdForm=" + SM.Quote(_IdForm) + ")" + SM.SqlNotDeleted()))
                        {
                            Clear();
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
                                if (SM.Empty(TableName)) rslt = LoadContents(_IdDocument);
                                else rslt = LoadContentsChilds(_IdDocument, TableName, Controls.Childs);
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

        /// <summary>Load document data from generic contents.</summary>
        public bool LoadContents(int _IdDocument)
        {
            int idControl;
            bool rslt = false;
            string sql;
            SMDataset ds;
            SMFrontControl control = null;
            if (_IdDocument > 0)
            {
                try
                {
                    ds = new SMDataset("MAIN");
                    sql = "SELECT * FROM sm_contents WHERE (IdForm=" + SM.Quote(IdForm)
                        + ")AND(IdDocument=" + _IdDocument.ToString() + ")"
                        + SM.SqlNotDeleted() + " ORDER BY IdControl,ValueIndex";
                    if (ds.Open(sql))
                    {
                        rslt = true;
                        while (!ds.Eof)
                        {
                            idControl = ds.FieldInt("IdControl");
                            if (control != null)
                            {
                                if (control.IdControl != idControl) control = null;
                            }
                            if (control == null) control = Controls.FindById(idControl);
                            if (control != null)
                            {
                                if (control.IsBlob)
                                {
                                    control.SetData(ds.FieldInt("ValueIndex"), ds.FieldBlob("Blob"));
                                }
                                else control.SetValue(ds.FieldInt("ValueIndex"), ds.FieldStr("Value"));
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

        /// <summary>Load document data from table.</summary>
        public bool LoadContentsChilds(int _IdDocument, string _TableName, List<SMFrontControl> _Controls, string _OrderBy = "")
        {
            int i, j;
            bool rslt = false;
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
                        Clear();
                        j = 0;
                        while (!ds.Eof)
                        {
                            rslt = true;
                            i = 0;
                            while (i < _Controls.Count)
                            {
                                control = _Controls[i];
                                if (control.ControlType == SMFrontControlType.Details)
                                {
                                    if (!LoadContentsChilds(_IdDocument, control.TableName, control.Childs, control.Parameters.ValueOf("ORDERBY", "IdRow"))) rslt = false;
                                }
                                else if (!SM.Empty(control.ColumnName) && (SM.Empty(control.TableName) || (control.TableName == TableName)))
                                {
                                    if ((control.ControlType == SMFrontControlType.Blob)
                                        || (control.ControlType == SMFrontControlType.Image)
                                        || (control.ControlType == SMFrontControlType.Upload))
                                    {
                                        control.SetData(j, ds.FieldBlob(control.ColumnName));
                                    }
                                    else control.SetValue(j, ds.FieldStr(control.ColumnName));
                                }
                                i++;
                            }
                            ds.Next();
                            j++;
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

        /// <summary>Save form data.</summary>
        public bool Save()
        {
            bool rslt = false;

            return rslt;
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
