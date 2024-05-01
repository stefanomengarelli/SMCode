/*  ===========================================================================
 *  
 *  File:       Dictionary.cs
 *  Version:    2.0.16
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: dictionary.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: dictionary.</summary>
    public partial class SMApplication
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Restituisce una nuova istanza di dizionario.</summary>
        public SMDictionary NewDictionary()
        {
            return new SMDictionary(this);
        }

        /// <summary>Restituisce una nuova istanza di dizionario.</summary>
        public SMDictionary NewDictionary(SMDictionary _Dictionary)
        {
            return new SMDictionary(_Dictionary, this);
        }

        #endregion

        /* */

    }

    /* */

}
