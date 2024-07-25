/*  ===========================================================================
 *  
 *  File:       SMUsersRules.cs
 *  Version:    2.0.38
 *  Date:       July 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode users-rules relation class.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode users-rules relation class.</summary>
    public class SMUsersRules
    {

        /* */

        #region Static Properties

        /*  ===================================================================
         *  Static Properties
         *  ===================================================================
         */

        /// <summary>Database alias.</summary>
        public static string Alias { get; set; } = "MAIN";

        /// <summary>Users table name.</summary>
        public static string TableName { get; set; } = "sm_users_rules";

        /// <summary>Users table deleted column.</summary>
        public static string DeletedColumn { get; set; } = "Deleted";

        /// <summary>Users table user id column.</summary>
        public static string UserIdColumn { get; set; } = "UserId";

        /// <summary>Users table user id column.</summary>
        public static string RuleIdColumn { get; set; } = "RuleId";

        #endregion

        /* */

    }

    /* */

}