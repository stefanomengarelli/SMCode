/*  ===========================================================================
 *  
 *  File:       SMDefaults.cs
 *  Version:    2.0.85
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode defaults configuration static class.
 *
 *  ===========================================================================
 */

using System.Diagnostics;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode defaults configuration static class.</summary>
    public class SMDefaults
    {
        /// <summary>Organizations table name.</summary>
        public static string OrganizationsTableName = "sm_organizations";

        /// <summary>Rules table name.</summary>
        public static string RulesTableName = "sm_rules";

        /// <summary>Users table name.</summary>
        public static string UsersTableName = "sm_users";

        /// <summary>Users table name.</summary>
        public static string UsersOrganizationsTableName = "sm_users_organizations";

        /// <summary>Users rules table name.</summary>
        public static string UsersRulesTableName = "sm_users_rules";
    }

    /* */

}
