/*  ===========================================================================
 *  
 *  File:       SMFrontControls.cs
 *  Version:    2.0.50
 *  Date:       Jul 2024
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

using SMCodeSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
        private List<int> ixOrder = new List<int>();

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

        /// <summary>Get controls count.</summary>
        public int Count
        {
            get { return items.Count; }
        }

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
        public SMFrontControls(SMFrontControls _OtherInstance, SMFront _SM = null)
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
            ixOrder.Add(rslt);
            SM.Sort(ixOrder, items, SMFrontControl.CompareByOrder, true);
            //
            return rslt;
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMFrontControls _OtherInstance)
        {
            int i;
            if (SM == null) SM = _OtherInstance.SM;
            Clear();
            for (i = 0; i < _OtherInstance.items.Count; i++) Add(new SMFrontControl(_OtherInstance[i], SM));
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
            ixAlias.Clear();
            ixColumnName.Clear();
            ixId.Clear();
            ixOrder.Clear();
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
            SMFrontControl item = new SMFrontControl(SM) { Id = _Id };
            int i = SM.Find(item, items, ixId, SMFrontControl.CompareById);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by order.</summary>
        public SMFrontControl FindByOrder(int _Order, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { Order = _Order };
            int i = SM.Find(item, items, ixOrder, SMFrontControl.CompareByOrder);
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
            SMFrontControl item;
            try
            {
                if (_Rows!=null)
                {
                    Clear();
                    for (i=0; i<_Rows.Count; i++)
                    {
                        item = new SMFrontControl(SM);
                        if (item.Read(_Rows[i])) Add(item);
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
        public string RenderControls()
        {
            int i;
            StringBuilder sb = new StringBuilder();
            SMFrontControl control;
            for (i = 0; i < ixOrder.Count; i++)
            {
                control = (SMFrontControl)items[ixOrder[i]];
            }
            return sb.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
