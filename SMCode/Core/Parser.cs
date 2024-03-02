/*  ===========================================================================
 *  
 *  File:       Parser.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode core class: parser.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode core class: parser.</summary>
    public partial class SM
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Restituisce una nuova istanza di parser.</summary>
        public SMParser NewParser()
        {
            return new SMParser(this);
        }

        /// <summary>Restituisce il risultato del parser con la formula passata.</summary>
        public double Parse(string _Formula)
        {
            SMParser parser = new SMParser(this);
            return parser.Result(_Formula);
        }

        #endregion

        /* */

    }

    /* */

}
