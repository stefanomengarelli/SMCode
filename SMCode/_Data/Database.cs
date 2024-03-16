/*  ===========================================================================
 *  
 *  File:       Database.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: database.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: database.</summary>
    public partial class SMApplication
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Database connections collection.</summary>
        public SMDatabases Databases { get; set; } = null;

        #endregion

        /* */

    }

    /* */

}
