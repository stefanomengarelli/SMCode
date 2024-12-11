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

using System.Collections.Generic;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode node item class.</summary>
    public class SMNode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set item key.</summary>
        public string Key { get; set; }

        /// <summary>Child nodes collection.</summary>
        public List<SMNode> Nodes { get; private set; } = new List<SMNode>();

        /// <summary>Get or set item parent.</summary>
        public SMNode Parent { get; set; }

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
        public SMNode()
        {
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMNode(SMNode _Node)
        {
            Assign(_Node);
        }

        /// <summary>Class constructor.</summary>
        public SMNode(SMNode _Parent, string _Key, string _Value, object _Tag)
        {
            Key = _Key;
            Parent = _Parent;
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
            int i;
            SMNode node;
            Key = _Node.Key;
            Parent = _Node.Parent;
            Tag = _Node.Tag;
            Value = _Node.Value;
            Nodes.Clear();
            for (i=0; i < _Node.Nodes.Count; i++)
            {
                node = new SMNode(_Node.Nodes[i]);
                node.Parent = this;
                Nodes.Add(node);
            }
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Key = "";
            Nodes.Clear();
            Parent = null;
            Tag = null;
            Value = "";
        }

        /// <summary>Return node with key.</summary>
        public SMNode Find(string _Key, bool _Recursive = true)
        {
            int i = 0;
            SMNode r = null;
            if (_Key != Key)
            {
                if (_Recursive)
                {
                    while ((r == null) && (i < Nodes.Count))
                    {
                        r = Nodes[i].Find(_Key);
                        i++;
                    }
                }
            }
            else r = this;
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
