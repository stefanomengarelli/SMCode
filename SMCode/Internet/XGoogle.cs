/*  ------------------------------------------------------------------------
 *  
 *  File:       XGoogle.cs
 *  Version:    1.0.0
 *  Date:       March 2021
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@microrun.it
 *  
 *  Copyright (C) 2022 by Stefano Mengarelli - All rights reserved - Use, permission and restrictions under license.
 *
 *  Microrun XRag Google services management functions.
 *  
 *  Dependencies: XStr, XSystem
 *
 *  ------------------------------------------------------------------------
 */

using System.Web;

namespace XRadSharp
{

    /* */

    /// <summary>Microrun XRag Google services management functions.</summary>
    public static partial class XGoogle
    {

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Open Google Maps url with request query.</summary>
        public static void Maps(string _RequestQuery)
        {
            XSystem.RunShell(@"https://maps.google.com?q=" + HttpUtility.UrlEncode(_RequestQuery), "", false, false);
        }

        /// <summary>Open Google Maps url with query.</summary>
        public static void Maps(string _Address, string _Zip, string _City, string _Province, string _Nation)
        {
            Maps(XStr.Cat(new string[] { _Address.Trim(), _Zip.Trim(), _City.Trim(), _Province.Trim(), _Nation.Trim() }, @" ", true));
        }

        #endregion

        /* */

    }

    /* */

}
