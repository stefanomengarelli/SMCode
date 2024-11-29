/*  ===========================================================================
 *  
 *  File:       SMFrontControls.cs
 *  Version:    2.0.72
 *  Date:       Npvember 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront controls collection class.
 *
 *  ===========================================================================
 */

using Org.BouncyCastle.Asn1.X509;
using SMCodeSystem;
using System;
using System.Collections.Generic;
using System.Data;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront controls collection class.</summary>
    public class SMFrontControls
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
        private List<object> items = new List<object>();

        /// <summary>Controls alias index.</summary>
        private List<int> ixAlias = new List<int>();

        /// <summary>Controls column name index.</summary>
        private List<int> ixColumnName = new List<int>();

        /// <summary>Controls id index.</summary>
        private List<int> ixId = new List<int>();

        /// <summary>Controls view order index.</summary>
        private List<int> ixViewIndex = new List<int>();

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set front control at index.</summary>
        public SMFrontControl this[int _Index]
        {
            get
            {
                if ((_Index > -1) && (_Index < items.Count))
                {
                    return (SMFrontControl)items[_Index];
                }
                else return null;
            }

            set
            {
                if ((_Index > -1) && (_Index < items.Count))
                {
                    ((SMFrontControl)items[_Index]).Assign(value);
                }
            }
        }

        /// <summary>Childs controls ordered by viewindex.</summary>
        public List<SMFrontControl> Childs { get; private set; } = new List<SMFrontControl>();

        /// <summary>Get controls count.</summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>Return items indices sorted by aliases.</summary>
        public List<int> IndicesByAliases { get { return ixAlias; } }

        /// <summary>Return items indices sorted by aliases.</summary>
        public List<int> IndicesByColumName { get { return ixAlias; } }

        /// <summary>Return items indices sorted by id.</summary>
        public List<int> IndicesById { get { return ixId; } }

        /// <summary>Return items indices sorted by view index.</summary>
        public List<int> IndicesByViewIndex { get { return ixViewIndex; } }

        /// <summary>Parent object.</summary>
        public object Parent { get; set; } = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontControls(SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControls(SMFrontControls _Controls, SMFront _SM = null)
        {
            if (_SM == null) _SM = _Controls.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_Controls);
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

        /// <summary>Add new control to collection. Return control index.</summary>
        public int Add(SMFrontControl _Control)
        {
            int rslt = items.Count;
            SMFrontControl item = new SMFrontControl(_Control, SM);
            //
            item.Parent = this;
            items.Add(item);
            //
            ixAlias.Add(rslt);
            SM.Sort(ixAlias, items, SMFrontControl.CompareByAlias, true);
            //
            ixColumnName.Add(rslt);
            SM.Sort(ixColumnName, items, SMFrontControl.CompareByColumnName, true);
            //
            ixId.Add(rslt);
            SM.Sort(ixId, items, SMFrontControl.CompareById, true);
            //
            ixViewIndex.Add(rslt);
            SM.Sort(ixViewIndex, items, SMFrontControl.CompareByViewIndex, true);
            //
            return rslt;
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMFrontControls _Controls)
        {
            int i;
            if (SM == null) SM = _Controls.SM;
            Clear();
            for (i = 0; i < _Controls.items.Count; i++) Add(new SMFrontControl(_Controls[i], SM));
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
            ixAlias.Clear();
            ixColumnName.Clear();
            ixId.Clear();
            ixViewIndex.Clear();
            Childs.Clear();
        }

        /// <summary>Clear controls contents.</summary>
        public void ClearContents()
        {
            int i = 0;
            SMFrontControl control;
            while (i < ixViewIndex.Count)
            {
                control = (SMFrontControl)items[ixViewIndex[i]];
                control.ClearData();
                i++;
            }
        }

        /// <summary>Calculate controls dependencies.</summary>
        private void Dependencies()
        {
            int i = 0;
            SMFrontControl parent = null, control;
            Childs.Clear();
            while (i < ixViewIndex.Count)
            {
                control = (SMFrontControl)items[ixViewIndex[i]];
                control.Parent = parent;
                control.Childs.Clear();
                //
                if (parent == null) Childs.Add(control);
                else parent.Childs.Add(control);
                //
                if ((control.ControlType == SMFrontControlType.Details)
                    || (control.ControlType == SMFrontControlType.FieldSet)
                    || (control.ControlType == SMFrontControlType.Tab)
                    || (control.ControlType == SMFrontControlType.View))
                {
                    parent = control;
                }
                else if ((control.ControlType == SMFrontControlType.EndDetails)
                    || (control.ControlType == SMFrontControlType.EndSet)
                    || (control.ControlType == SMFrontControlType.EndTab)
                    || (control.ControlType == SMFrontControlType.EndView))
                {
                    parent = (SMFrontControl)parent.Parent;
                }
                i++;
            }
        }

        /// <summary>Return front control at index of collection sorted by name.</summary>
        public SMFrontControl FindByAlias(string _Alias, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { Alias = _Alias };
            int i = SM.Find(item, items, ixAlias, SMFrontControl.CompareByAlias);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by column name.</summary>
        public SMFrontControl FindByColumnName(string _ColumnName, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { ColumnName = _ColumnName };
            int i = SM.Find(item, items, ixColumnName, SMFrontControl.CompareByColumnName);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by id.</summary>
        public SMFrontControl FindById(int _Id, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { IdControl = _Id };
            int i = SM.Find(item, items, ixId, SMFrontControl.CompareById);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by order.</summary>
        public SMFrontControl FindByViewIndex(int _ViewIndex, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { ViewIndex = _ViewIndex };
            int i = SM.Find(item, items, ixViewIndex, SMFrontControl.CompareByViewIndex);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Load controls collection from dataset.</summary>
        public bool Load(SMDataset _Dataset)
        {
            SMFrontControl item;
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
                            item = new SMFrontControl(SM);
                            if (item.Read(_Dataset)) Add(item);
                            _Dataset.Next();
                        }
                        Dependencies();
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

        /// <summary>Load controls collection by form id.</summary>
        public bool LoadByIdForm(string _IdForm, string _Alias = "MAIN")
        {
            return Load("SELECT * FROM sm_controls WHERE (IdForm=" + SM.Quote(_IdForm) + ")AND" + SM.SqlNotDeleted(), _Alias);
        }

        /// <summary>Set all controls list values changed flag as specified. 
        /// If controls list not specified current instance items will be assumed.</summary>
        public void SetChanges(bool _Changed, List<object> _Controls = null)
        {
            int i, j;
            SMFrontControl control;
            if (_Controls == null) _Controls = items;
            for (i = 0; i < _Controls.Count; i++)
            {
                control = (SMFrontControl)_Controls[i];
                for (j = 0; j < control.Values.Count; j++)
                {
                    control.Values[j].Changed = _Changed;
                }
            }
        }

        #endregion

        /* */

    }

    /* */

}
