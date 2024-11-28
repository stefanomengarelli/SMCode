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

        /// <summary>Get or set current document id.</summary>
        public int IdDocument { get; set; } = 0;

        /// <summary>Get form controls collection.</summary>
        public SMFrontControls Controls { get; private set; } = null;

        /// <summary>Get or set form debugger flag.</summary>
        public bool Debugger { get; set; } = false;

        /// <summary>Get or set form deleted flag.</summary>
        public bool Deleted { get; set; } = false;

        /// <summary>Get delections collection.</summary>
        public List<int> Deletions { get; private set; } = null;

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
            IdDocument = _Form.IdDocument;
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
            IdDocument = 0;
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
        public void Read(SMDataset _Dataset)
        {
            IdForm = _Dataset.FieldStr("IdForm");
            Debugger = _Dataset.FieldBool("Debugger");
            Deleted = _Dataset.FieldBool("Deleted");
            Enabled = _Dataset.FieldBool("IdForm"); ;
            Events.Read(_Dataset.Row);
            EventsFN.Read(_Dataset.Row,"FN_");
            EventsSP.Read(_Dataset.Row, "SP_");
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
