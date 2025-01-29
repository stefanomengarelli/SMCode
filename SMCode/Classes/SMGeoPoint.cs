/*  ===========================================================================
 *  
 *  File:       SMGeoPoint.cs
 *  Version:    2.0.124
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *  
 *  SMCode maps geographical point.
 *  
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode maps geographical point.</summary>
    public class SMGeoPoint
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Class constructor.</summary>
        public SMGeoPoint(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear(); 
        }

        /// <summary>Class constructor.</summary>
        public SMGeoPoint(SMGeoPoint _OtherInstance, SMCode _SM)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMGeoPoint(double _Latidude, double _Longitude, SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Latitude = _Latidude;
            Longitude = _Longitude;
        }

        #endregion

        /* */

        #region Properties

        /*  --------------------------------------------------------------------
         *  Properties
         *  --------------------------------------------------------------------
         */

        /// <summary>Get or set X coordinate.</summary>
        public double Latitude { get; set; }

        /// <summary>Get or set Y coordinate.</summary>
        public double Longitude { get; set; }

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMGeoPoint _Point)
        {
            Latitude = _Point.Latitude;
            Longitude = _Point.Longitude;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Latitude = 0.0d;
            Longitude = 0.0d;
        }

        /// <summary>Return distance between points.</summary>
        public double DistanceTo(SMGeoPoint _GeoPoint, SMGeoUnits _Units)
        {
            double lat1 = Math.PI * Latitude / 180.0d;
            double lat2 = Math.PI * _GeoPoint.Latitude / 180.0d;
            double lon = Math.PI * (Longitude - _GeoPoint.Longitude) / 180.0d;
            double r = Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon);
            r = Math.Acos(r) * 10800.0d * 1.1515d / Math.PI;
            if (_Units == SMGeoUnits.Kilometers) return r * 1.609344d;
            else if (_Units == SMGeoUnits.NauticalMiles) return r * 0.8684d;
            else if (_Units == SMGeoUnits.Miles) return r;
            else return r * 0.001609344d;
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMGeoPoint)JsonSerializer.Deserialize(_JSON, null));
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Assign property from JSON64 serialization.</summary>
        public bool FromJSON64(string _JSON64)
        {
            return FromJSON(SM.Base64Decode(_JSON64));
        }

        /// <summary>Return JSON serialization of instance.</summary>
        public string ToJSON()
        {
            try
            {
                return JsonSerializer.Serialize(this);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return "";
            }
        }

        /// <summary>Return JSON64 serialization of instance.</summary>
        public string ToJSON64(SMCode _SM)
        {
            return SM.Base64Encode(ToJSON());
        }

        /// <summary>Return point as string.</summary>
        public override string ToString()
        {
            return "(" + Latitude.ToString() + "," + Longitude.ToString() + ")";
        }

        #endregion

        /* */

    }

    /* */

}
