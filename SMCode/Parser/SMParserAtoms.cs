/*  ===========================================================================
 *  
 *  File:       SMParserAtoms.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode parser atoms collection class.
 *  
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode parser atoms collection class.</summary>
    public class SMParserAtoms
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Max atoms number.</summary>
        private const int maxAtoms = 512;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set atoms count.</summary>
        public int Count { get; set; }

        /// <summary>Get atoms array.</summary>
        public SMParserAtom[] Items { get; private set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Atoms constructor.</summary>
        public SMParserAtoms()
        {
            Items = new SMParserAtom[maxAtoms];
            Count = 0;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add atom with name, value and type.</summary>
        public void Add(string _Name, double _Value, SMParserAtomType _Type)
        {
            if (Count < maxAtoms)
            {
                Items[Count] = new SMParserAtom(_Name, _Value, _Type);
                Count++;
            }
        }

        /// <summary>Reset atoms.</summary>
        public void Clear()
        {
            Count = 0;
        }

        /// <summary>Returns index of atom with name, -1 if not found.</summary>
        public int Find(string _Name)
        {
            int i = 0, r = -1;
            while (r < 0 && i < Count) if (_Name == Items[i].Name) r = i; else i++;
            return r;
        }

        /// <summary>Returns value of atom with name, 0 if not found.</summary>
        public double Get(string _Name)
        {
            int i = Find(_Name);
            if (i > -1)
            {
                if (Items[i].Type == SMParserAtomType.Variable) return Items[i].Value;
                else return 0.0d;
            }
            else return 0.0d;
        }

        /// <summary>Set value of atom with name.</summary>
        public void Set(string _Name, double _Value)
        {
            int i = Find(_Name);
            if (i > -1)
            {
                if (Items[i].Type == SMParserAtomType.Variable) Items[i].Value = _Value;
            }
            else Add(_Name, _Value, SMParserAtomType.Variable);
        }

        #endregion

        /* */

    }

    /* */

}
