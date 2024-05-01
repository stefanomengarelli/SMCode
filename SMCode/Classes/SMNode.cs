/*  ===========================================================================
 *  
 *  File:       SMNode.cs
 *  Version:    2.0.16
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode node item class.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode node item class.</summary>
    public class SMNode
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set item key.</summary>
        public string Key { get; set; }

        /// <summary>Get or set item parent.</summary>
        public object Parent { get; set; }

        /// <summary>Get or set item tag object.</summary>
        public object Tag { get; set; }

        /// <summary>Get or set item value.</summary>
        public string Value { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMNode(SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMNode(SMNode _Node, SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Assign(_Node);
        }

        /// <summary>Class constructor.</summary>
        public SMNode(object _Parent, string _Key, string _Value, object _Tag, SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Parent = _Parent;
            Key = _Key;
            Tag = _Tag;
            Value = _Value;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMNode _Node)
        {
            Key = _Node.Key;
            Parent = _Node.Parent;
            Tag = _Node.Tag;
            Value = _Node.Value;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Key = "";
            Parent = null;
            Tag = null;
            Value = "";
        }

        /// <summary>Compare this node item instance key with passed.</summary>
        public int Compare(SMNode _Node)
        {
            return Key.CompareTo(_Node.Key);
        }

        #endregion

        /* */

    }

    /* */

}
