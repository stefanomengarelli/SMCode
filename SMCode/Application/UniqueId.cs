/*  ===========================================================================
 *  
 *  File:       UniqueId.cs
 *  Version:    2.0.274
 *  Date:       June 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: unique id.
 *  
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: unique id.</summary>
    public partial class SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Last unique id generated.</summary>
        private static string[] LastUniqueId { get; set; } = new string[8] { "", "", "", "", "", "", "", "" };

        /// <summary>Last unique id generated array index.</summary>
        private static int LastUniqueIdIndex { get; set; } = 0;

        /// <summary>Get unique id base year.</summary>
        public int UniqueIdBaseYear { get; private set; } = 2010;

        /// <summary>Get unique id length.</summary>
        public int UniqueIdLength { get; private set; } = 12;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return string containing new GUID.</summary>
        public string GUID()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>Return string representing GUID passed.</summary>
        public string GUID(Guid? _Value)
        {
            if (_Value.HasValue) return _Value.Value.ToString();
            else return Guid.Empty.ToString();
        }

        /// <summary>Return GUID represented by string passed.</summary>
        public Guid GUID(string _Value)
        {
            try
            {
                if (Empty(_Value)) return Guid.Empty;
                else return new Guid(_Value);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>Returns a string to use as an identifier in HTTP requests. 
        /// The id value changes every 30 seconds.</summary>
        public string RequestId()
        {
            return (DateTime.Now.Ticks / (TimeSpan.TicksPerSecond * 30)).ToString();
        }

        /// <summary>Returns date-time represented by unique id.</summary>
        public DateTime UniqueIdDate(string _String)
        {
            DateTime r = DateTime.MinValue;
            if (ValidateUniqueId(_String))
            {
                r = new DateTime((int)BaseToInt(_String.Substring(0, 2), Base32) + UniqueIdBaseYear, 1, 1, 0, 0, 0, 0);
                r = r.AddTicks(BaseToInt(_String.Substring(2, 7), Base32) * TimeSpan.TicksPerMillisecond);
            }
            return r;
        }

        /// <summary>Returns a string representing a new high probability of unicity time related 12 chars id.</summary>
        public string UniqueId()
        {
            int q;
            bool v;
            long n;
            string a, b;
            DateTime d = DateTime.Now, tout = d.AddTicks(TimeSpan.TicksPerMinute * 2), yst = new DateTime(d.Year, 1, 1);
            DoEvents();

            //
            //  validate current datetime
            //
            if ((d.Year < UniqueIdBaseYear) || (d.Year > UniqueIdBaseYear + 240))
            {
                Raise("System time not properly setted. Cannot generate a unique id.", true);
                return "";
            }
            else
            {
                //
                // year base calc
                //
                a = "0" + IntToBase(d.Year - UniqueIdBaseYear, Base32);
                a = a.Substring(a.Length - 2, 2);
                //
                // calc milliseconds from first of the year
                //
                q = Base32.Length - 1;
                n = (d.Ticks - yst.Ticks) / TimeSpan.TicksPerMillisecond;
                b = a + Right("0000000" + IntToBase(n, Base32), 7)
                    + Base32[Rnd(q)]
                    + Base32[Rnd(q)]
                    + Base32[Rnd(q)];
                //
                // if calculated id is next to prior recalc adding 1 milliseconds
                //
                v = NewUniqueId(b);
                while (!v && (DateTime.Now < tout))
                {
                    n++;
                    b = a + Right("0000000" + IntToBase(n, Base32), 7)
                        + Base32[Rnd(q)]
                        + Base32[Rnd(q)]
                        + Base32[Rnd(q)];
                    v = NewUniqueId(b);
                    DoEvents();
                }
                //
                // if calculated id is valid return value
                //
                if (v) return b;
                else return "";
            }
        }

        /// <summary>Return true if string passed is a valid unique id.</summary>
        public bool ValidateUniqueId(string _String)
        {
            if (_String.Length == UniqueIdLength) return IsCharSet(_String, Base32);
            else return false;
        }

        /// <summary>Return true if unique id passed are not already present in last unique id array.</summary>
        private static bool NewUniqueId(string _String)
        {
            int i = LastUniqueId.Length;
            bool r = true;
            while (r && (i > 0)) r = _String.CompareTo(LastUniqueId[--i]) > 0;
            if (r)
            {
                LastUniqueId[LastUniqueIdIndex] = _String;
                LastUniqueIdIndex = (LastUniqueIdIndex + 1) % LastUniqueId.Length;
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
