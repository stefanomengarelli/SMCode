/*  ===========================================================================
 *  
 *  File:       SMFrontControls.cs
 *  Version:    2.0.25
 *  Date:       Jun 2024
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

using SMCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMFront
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

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        /// <summary>Controls collection.</summary>
        private List<object> items = new List<object>();

        /// <summary>Controls id index.</summary>
        private List<int> ix_id = new List<int>();

        /// <summary>Controls name index.</summary>
        private List<int> ix_name = new List<int>();

        /// <summary>Controls order index.</summary>
        private List<int> ix_order = new List<int>();

        /// <summary>Controls field index.</summary>
        private List<int> ix_field = new List<int>();

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

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontControls(SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControls(SMFrontControls _OtherInstance, SMApplication _SMApplication = null)
        {
            if (_SMApplication == null)
            {
                if (_OtherInstance.SM != null) SM = _OtherInstance.SM;
                else SM = SMApplication.CurrentOrNew();
            }
            else SM = _SMApplication;
            Assign(_OtherInstance);
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
            items.Add(new SMFrontControl(_Control, SM));
            ix_id.Add(rslt);
            SM.Sort(ix_id, items, SMFrontControl.CompareById, true);
            ix_name.Add(rslt);
            SM.Sort(ix_name, items, SMFrontControl.CompareByName, true);
            ix_order.Add(rslt);
            SM.Sort(ix_order, items, SMFrontControl.CompareByOrder, true);
            ix_field.Add(rslt);
            SM.Sort(ix_field, items, SMFrontControl.CompareByField, true);
            return rslt;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
            ix_id.Clear();
            ix_name.Clear();
            ix_order.Clear();
            ix_field.Clear();
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMFrontControls _OtherInstance)
        {
            int i;
            Clear();
            for (i = 0; i < _OtherInstance.items.Count; i++) Add(new SMFrontControl(_OtherInstance[i], SM));
        }

        /// <summary>Return front control at index of collection sorted by field.</summary>
        public SMFrontControl FindByField(string _Field, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { Field = _Field };
            int i = SM.Find(item, items, ix_field, SMFrontControl.CompareByField);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by id.</summary>
        public SMFrontControl FindById(int _Id, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { Id = _Id };
            int i = SM.Find(item, items, ix_id, SMFrontControl.CompareById);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by name.</summary>
        public SMFrontControl FindByName(string _Name, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { Name = _Name };
            int i = SM.Find(item, items, ix_name, SMFrontControl.CompareByName);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by order.</summary>
        public SMFrontControl FindByOrder(int _Order, bool _NullOnInvalidIndex = true)
        {
            SMFrontControl item = new SMFrontControl(SM) { Order = _Order };
            int i = SM.Find(item, items, ix_order, SMFrontControl.CompareByOrder);
            if (i > -1) return (SMFrontControl)items[i];
            else if (_NullOnInvalidIndex) return null;
            else return item.Clear();
        }

        /// <summary>Return front control at index of collection sorted by field.</summary>
        public SMFrontControl ItemByField(int _Index, bool _NullOnInvalidIndex = true)
        {
            if ((_Index > -1) && (_Index < ix_field.Count)) return (SMFrontControl)items[ix_field[_Index]];
            else if (_NullOnInvalidIndex) return null;
            else return new SMFrontControl(SM);
        }

        /// <summary>Return front control at index of collection sorted by id.</summary>
        public SMFrontControl ItemById(int _Index, bool _NullOnInvalidIndex = true)
        {
            if ((_Index > -1) && (_Index < ix_id.Count)) return (SMFrontControl)items[ix_id[_Index]];
            else if (_NullOnInvalidIndex) return null;
            else return new SMFrontControl(SM);
        }

        /// <summary>Return front control at index of collection sorted by name.</summary>
        public SMFrontControl ItemByName(int _Index, bool _NullOnInvalidIndex = true)
        {
            if ((_Index > -1) && (_Index < ix_name.Count)) return (SMFrontControl)items[ix_name[_Index]];
            else if (_NullOnInvalidIndex) return null;
            else return new SMFrontControl(SM);
        }

        /// <summary>Return front control at index of collection sorted by order.</summary>
        public SMFrontControl ItemByOrder(int _Index, bool _NullOnInvalidIndex = true)
        {
            if ((_Index > -1) && (_Index < ix_order.Count)) return (SMFrontControl)items[ix_order[_Index]];
            else if (_NullOnInvalidIndex) return null;
            else return new SMFrontControl(SM);
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
        public bool Load(string _SQL = "SELECT * FROM SMFrontControls WHERE (Deleted IS NULL)OR(Deleted=0)", string _Alias = "MAIN")
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
            for (i = 0; i < ix_order.Count; i++)
            {
                ((SMFrontControl)items[ix_order[i]]).Render(sb);
            }
            return sb.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
