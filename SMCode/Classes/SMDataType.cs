/*  ===========================================================================
 *  
 *  File:       SMDataType.cs
 *  Version:    2.0.303
 *  Date:       October 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode data type class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode data type class.</summary>
    public class SMDataType
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>System.Boolean data type value.</summary>
        public static readonly Type Boolean = System.Type.GetType("System.Boolean");

        /// <summary>System.Byte data type value.</summary>
        public static readonly Type Byte = System.Type.GetType("System.Byte");

        /// <summary>System.Byte[] data type value.</summary>
        public static readonly Type BytesArray = System.Type.GetType("System.Byte[]");

        /// <summary>System.Char data type value.</summary>
        public static readonly Type Char = System.Type.GetType("System.Char");

        /// <summary>System.DateTime data type value.</summary>
        public static readonly Type DateTime = System.Type.GetType("System.DateTime");

        /// <summary>System.Decimal data type value.</summary>
        public static readonly Type Decimal = System.Type.GetType("System.Decimal");

        /// <summary>System.Double data type value.</summary>
        public static readonly Type Double = System.Type.GetType("System.Double");

        /// <summary>System.Guid data type value.</summary>
        public static readonly Type Guid = Type.GetType("System.Guid");

        /// <summary>System.Int16 data type value.</summary>
        public static readonly Type Int16 = System.Type.GetType("System.Int16");

        /// <summary>System.Int32 data type value.</summary>
        public static readonly Type Int32 = System.Type.GetType("System.Int32");

        /// <summary>System.Int64 data type value.</summary>
        public static readonly Type Int64 = System.Type.GetType("System.Int64");

        /// <summary>System.SByte data type value.</summary>
        public static readonly Type SByte = System.Type.GetType("System.SByte");

        /// <summary>System.Single data type value.</summary>
        public static readonly Type Single = System.Type.GetType("System.Single");

        /// <summary>System.String data type value.</summary>
        public static readonly Type String = System.Type.GetType("System.String");

        /// <summary>System.TimeSpan data type value.</summary>
        public static readonly Type TimeSpan = System.Type.GetType("System.TimeSpan");

        /// <summary>System.UInt16 data type value.</summary>
        public static readonly Type UInt16 = System.Type.GetType("System.UInt16");

        /// <summary>System.UInt32 data type value.</summary>
        public static readonly Type UInt32 = System.Type.GetType("System.UInt32");

        /// <summary>System.UInt64 data type value.</summary>
        public static readonly Type UInt64 = System.Type.GetType("System.UInt64");

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return true if data type is blob.</summary>
        public static bool IsBlob(Type _Type)
        {
            return _Type == BytesArray;
        }

        /// <summary>Return true if data type is boolean.</summary>
        public static bool IsBoolean(Type _Type)
        {
            return _Type == Boolean;
        }

        /// <summary>Return true if data type is date.</summary>
        public static bool IsDate(Type _Type)
        {
            return (_Type == DateTime) || (_Type == TimeSpan);
        }

        /// <summary>Return true if data type is GUID.</summary>
        public static bool IsGuid(Type _Type)
        {
            return _Type == Guid;
        }

        /// <summary>Return true if column field data type is integer.</summary>
        public static bool IsInteger(Type _Type)
        {
            return (_Type == Int32)
                || (_Type == Byte)
                || (_Type == Char)
                || (_Type == Int16)
                || (_Type == Int64)
                || (_Type == SByte)
                || (_Type == UInt16)
                || (_Type == UInt32)
                || (_Type == UInt64);
        }

        /// <summary>Return true if column field data type is numeric.</summary>
        public static bool IsNumeric(Type _Type)
        {
            return (_Type == Int32)
                || (_Type == Double)
                || (_Type == Byte)
                || (_Type == Char)
                || (_Type == Int16)
                || (_Type == Int64)
                || (_Type == Decimal)
                || (_Type == Single)
                || (_Type == SByte)
                || (_Type == UInt16)
                || (_Type == UInt32)
                || (_Type == UInt64);
        }

        /// <summary>Return true if data type is text.</summary>
        public static bool IsText(Type _Type)
        {
            return _Type == String;
        }

        #endregion

        /* */

    }

    /* */

}
