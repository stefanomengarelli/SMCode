/*  ===========================================================================
 *  
 *  File:       Google.cs
 *  Version:    2.0.18
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Google services management functions.
 *  
 *  ===========================================================================
 */

using System.Net;

namespace SMCodeSystem
{

    /* */

    /// <summary>Google services management functions.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Open Google Maps url with request query.</summary>
        public void GoogleMaps(string _RequestQuery)
        {
            RunShell(@"https://maps.google.com?q=" + WebUtility.UrlEncode(_RequestQuery), "", false, false);
        }

        /// <summary>Open Google Maps url with query.</summary>
        public void GoogleMaps(string _Address, string _Zip, string _City, string _Province, string _Nation)
        {
            GoogleMaps(Cat(new string[] { _Address.Trim(), _Zip.Trim(), _City.Trim(), _Province.Trim(), _Nation.Trim() }, @" ", true));
        }

        #endregion

        /* */

    }

    /* */

}
