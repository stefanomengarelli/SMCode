/*  ===========================================================================
 *  
 *  File:       SMDateFormat.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode date format enumeration.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode date format enumeration.</summary>
    public enum SMDateFormat
    {
        /// <summary>automatic (system) data format detection.</summary>
        auto = -1,
        /// <summary>dd-mm-yyyy date format (day and month 2 digits, year 4 digits).</summary>
        ddmmyyyy = 0,
        /// <summary>mm-dd-yyyy date format (month and day 2 digits, year 4 digits).</summary>
        mmddyyyy = 1,
        /// <summary>yyyy-mm-dd date format (year 4 digits, month and day 2 digits).</summary>
        yyyymmdd = 2,
        /// <summary>dd-mm-yy date format (day, month and year 2 digits).</summary>
        ddmmyy = 3,
        /// <summary>mm-dd-yy date format (month, day and year 2 digits).</summary>
        mmddyy = 4,
        /// <summary>yy-mm-dd date format (year, month and day 2 digits).</summary>
        yymmdd = 5,
        /// <summary>d-m-y date format (day, month and year with all digits).</summary>
        dmy = 6,
        /// <summary>m-d-y date format (month, day and year with all digits).</summary>
        mdy = 7,
        /// <summary>y-m-d date format (year, month and day with all digits).</summary>
        ymd = 8,
        /// <summary>yyyy-mm-dd date ISO 8601 format (year 4 digits, month and day 2 digits).</summary>
        iso8601 = 9,
        /// <summary>yyyymmdd date format (year 4 digits, month and day 2 digits) with no separator.</summary>
        compact = 10
    }

    /* */

}
