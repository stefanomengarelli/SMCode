/*  ===========================================================================
 *  
 *  File:       SMParserAtom.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode parser atom class.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode parser atom class.</summary>
    public class SMParserAtom
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set atom name.</summary>
        public string Name { get; set; }

        /// <summary>Get or set atom value.</summary>
        public double Value { get; set; }

        /// <summary>Get or set atom type.</summary>
        public SMParserAtomType Type { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Atom constructor.</summary>
        public SMParserAtom()
        {
            Name = "";
            Value = 0.0d;
            Type = SMParserAtomType.None;
        }

        /// <summary>Atom constructor.</summary>
        public SMParserAtom(string _Name, double _Value, SMParserAtomType _Type)
        {
            Name = _Name;
            Value = _Value;
            Type = _Type;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Set atom error.</summary>
        public void Error(string _Name)
        {
            Name = _Name;
            Type = SMParserAtomType.Error;
        }

        #endregion

        /* */

    }

    /* */

}
