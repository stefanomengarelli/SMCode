/*  ===========================================================================
 *  
 *  File:       SMFrontRender.cs
 *  Version:    2.0.32
 *  Date:       Jun 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront controls render class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront controls render class.</summary>
    public class SMFrontRender
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SMFront session instance.</summary>
        private SMFront SM = null;

        /// <summary>Controls collection.</summary>
        private SMDictionary templates = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Controls collection.</summary>
        public SMFrontControls Controls { get; set; } = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontRender(SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontRender(SMFrontRender _OtherInstance, SMFront _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize control instance.</summary>
        private void InitializeInstance()
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
        public void Assign(SMFrontRender _OtherInstance)
        {
            if (SM == null) SM = _OtherInstance.SM;
            Clear();
            templates.Assign(_OtherInstance.templates);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            templates.Clear();
        }

        /// <summary>Load controls collection from dataset.</summary>
        public bool Load(SMDataset _Dataset)
        {
            try
            {
                if (_Dataset != null)
                {
                    if (_Dataset.Active)
                    {
                        Clear();
                        _Dataset.First();
                        while (!_Dataset.Eof)
                        {
                            templates.Add(_Dataset.FieldStr("ControlType"), _Dataset.FieldStr("Text"));
                            _Dataset.Next();
                        }
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Load controls collection from dataset.</summary>
        public bool Load(string _SQL, string _Alias = "MAIN")
        {
            bool r = false;
            SMDataset ds;
            try
            {
                ds = new SMDataset(_Alias, SM);
                if (ds.Open(_SQL)) r = Load(ds);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = false;
            }
            return r;
        }

        /// <summary>Load controls collection from data row collection.</summary>
        public bool Load(DataRowCollection _Rows)
        {
            int i;
            bool r = false;
            try
            {
                if (_Rows!=null)
                {
                    Clear();
                    for (i=0; i<_Rows.Count; i++)
                    {
                        templates.Add(SM.ToStr(_Rows[i]["ControlType"]), SM.ToStr(_Rows[i]["Text"]));
                    }
                    r = true;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = false;
            }
            return r;
        }

        /// <summary>Return string containing controls rendering.</summary>
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
