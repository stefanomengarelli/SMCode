/*  ===========================================================================
 *  
 *  File:       SMGeoPoint.cs
 *  Version:    2.0.14
 *  Date:       April 2024
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

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode maps geographical point.</summary>
    public class SMGeoPoint
    {

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Class constructor.</summary>
        public SMGeoPoint()
        {
            Clear(); 
        }

        /// <summary>Class constructor.</summary>
        public SMGeoPoint(SMGeoPoint _GeoPoint)
        {
            Assign(_GeoPoint);
        }

        /// <summary>Class constructor.</summary>
        public SMGeoPoint(double _Latidude, double _Longitude)
        {
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
