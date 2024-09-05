/*  ===========================================================================
 *  
 *  File:       ShortCuts.cs
 *  Version:    2.0.45
 *  Date:       September 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMDesk UI shortcuts functions.
 *  
 *  ===========================================================================
 */

using SMCodeSystem;
using System.Windows.Forms;

namespace SMDeskSystem
{

    /* */

    /// <summary>SMDesk UI shortcuts functions.</summary>
    public partial class SMDesk : SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return true if key event args contain CTRL + ALT + F12
        /// to activate or disactivate application debug mode.</summary>
        public bool ShortCutDebug(KeyEventArgs _KeyEventsArgs)
        {
            return (_KeyEventsArgs.KeyCode == Keys.F12) 
                && _KeyEventsArgs.Control && _KeyEventsArgs.Alt;
        }

        /// <summary>Return true if key event args contain CTRL + ALT + F8
        /// to open maintenance panel.</summary>
        public bool ShortCutMaintenance(KeyEventArgs _KeyEventsArgs)
        {
            return ((_KeyEventsArgs.KeyCode == Keys.F8) || (_KeyEventsArgs.KeyCode == Keys.M))
                && _KeyEventsArgs.Control && _KeyEventsArgs.Alt;
        }

        /// <summary>Return true if key event args contain SHIFT + CTRL + ALT + F4
        /// to activate special functions.</summary>
        public bool ShortCutSpecial(KeyEventArgs _KeyEventsArgs)
        {
            return ((_KeyEventsArgs.KeyCode == Keys.F4) || (_KeyEventsArgs.KeyCode == Keys.M))
                && _KeyEventsArgs.Shift && _KeyEventsArgs.Control && _KeyEventsArgs.Alt;
        }

        /// <summary>Return true if string passed match passepartout.</summary>
        public bool Passepartout(string _String)
        {
            return SM.HashSHA256(_String) == "df97d3a2df4b5846d5a12ff76a226485d612cac53fb2d6afd85712779c721a96";
        }

        #endregion

        /* */

    }

    /* */

}
