/*  ===========================================================================
 *  
 *  File:       SMFrontDocument.cs
 *  Version:    2.0.82
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront dummy class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMCode dummy class.</summary>
    public class SMFrontDocument
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

		/// <summary>Get or set document id.</summary>
		public int IdDocument { get; set; } = 0;

		/// <summary>Get or set document uid.</summary>
		public string UidDocument { get; set; } = "";

		/// <summary>Get form instance.</summary>
		public SMFrontForm Form { get; private set; } = null;

		/// <summary>Get or set user id.</summary>
		public int IdUser { get; set; } = 0;

		/// <summary>Get or set organization id.</summary>
		public int IdOrganization { get; set; } = 0;

		/// <summary>Get or set document icon path.</summary>
		public string Icon { get; set; } = "";

		/// <summary>Get or set document image path.</summary>
		public string Image { get; set; } = "";

		/// <summary>Get or set document note.</summary>
		public string Note { get; set; } = "";

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMFrontDocument(SMFrontForm _Form, SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            Form = _Form;
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontDocument(SMFrontDocument _Document, SMFront _SM = null)
        {
            if (_SM==null) _SM = _Document.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_Document);
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
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
        public void Assign(SMFrontDocument _Document)
        {
            IdDocument = _Document.IdDocument;
			UidDocument = _Document.UidDocument;
			Form = _Document.Form;
			IdUser = _Document.IdUser;
			IdOrganization = _Document.IdOrganization;
			Icon = _Document.Icon;
			Image = _Document.Image;
			Note = _Document.Note;
		}

		/// <summary>Clear item.</summary>
		public void Clear()
        {
            IdDocument = 0;
			UidDocument = "";
			IdUser = 0;
			IdOrganization = 0;
			Icon = "";
			Image = "";
			Note = "";
		}

		/// <summary>Load document with id.</summary>
		public bool Load(int _IdDocument)
        {
            bool rslt = false;
            string sql;
            SMDataset ds;
            Clear();
            try
            {
                if (Form != null)
                {
                    if (!SM.Empty(Form.IdForm))
                    {
                        ds = new SMDataset("MAIN");
                        sql = "SELECT * FROM sm_documents WHERE (IdDocument=" + _IdDocument.ToString()
                            + ")AND(IdForm=" + SM.Quote(Form.IdForm) + ")" + SM.SqlNotDeleted();
                        if (ds.Open(sql))
                        {
                            rslt = Read(ds);
                            ds.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                Clear();
            }
            return rslt;
        }

        /// <summary>Read document from current record on dataset.</summary>
        public bool Read(SMDataset _Dataset)
        {
            try
            {
                Clear();
                if (_Dataset != null)
                {
                    IdDocument = _Dataset.FieldInt("IdDocument");
                    UidDocument = _Dataset.FieldStr("UidDocument");
                    IdUser = _Dataset.FieldInt("IdUser");
                    IdOrganization = _Dataset.FieldInt("IdOrganization");
                    Icon = _Dataset.FieldStr("Icon");
                    Image = _Dataset.FieldStr("Image");
                    Note = _Dataset.FieldStr("Note");
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

		/// <summary>Save document and return id.</summary>
		public int Save()
		{
			int rslt = -1;
			string sql;
			SMDataset ds;
			Clear();
			try
			{
                if (Form != null)
                {
                    if (!SM.Empty(Form.IdForm))
                    {
                        ds = new SMDataset("MAIN");
                        if (IdDocument < 1)
                        {
                            UidDocument = SM.GUID();
                            sql = "SELECT * FROM sm_documents WHERE UidDocument=" + SM.Quote(UidDocument);
                            if (ds.Open(sql))
                            {
                                if (ds.Append())
                                {
                                    if (Write(ds))
                                    {
                                        if (ds.Post())
                                        {
                                            if (ds.Open(sql))
                                            {
                                                if (Read(ds)) rslt = IdDocument;
                                                ds.Close();
                                            }
                                        }
                                        else ds.Cancel();
                                    }
                                    else ds.Cancel();
                                }
                                ds.Close();
                            }
                        }
                        else
                        {
                            sql = "SELECT * FROM sm_documents WHERE (IdDocument=" + IdDocument.ToString()
                                + ")AND(IdForm=" + SM.Quote(Form.IdForm) + ")" + SM.SqlNotDeleted();
                            if (ds.Open(sql))
                            {
                                if (!ds.Eof)
                                {
                                    if (ds.Edit())
                                    {
                                        if (Write(ds))
                                        {
                                            if (ds.Post()) rslt = IdDocument;
                                            else ds.Cancel();
                                        }
                                        else ds.Cancel();
                                    }
                                }
                                ds.Close();
                            }
                        }
                    }
                }
			}
			catch (Exception ex)
			{
				SM.Error(ex);
				Clear();
                rslt = -1;
			}
			return rslt;
		}

		/// <summary>Write document on current record on dataset.</summary>
        public bool Write(SMDataset _Dataset)
		{
			try
			{
                if (_Dataset != null)
                {
                    _Dataset.Assign("IdDocument", IdDocument);
                    _Dataset.Assign("UidDocument", UidDocument);
                    _Dataset.Assign("IdForm", Form.IdForm);
                    _Dataset.Assign("IdUser", IdUser);
                    _Dataset.Assign("IdOrganization", IdOrganization);
                    _Dataset.Assign("Icon", Icon);
                    _Dataset.Assign("Image", Image);
                    _Dataset.Assign("Note", Note);
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

	}

	/* */

}
