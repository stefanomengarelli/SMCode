/*  ===========================================================================
 *  
 *  File:       SMUser.cs
 *  Version:    2.0.30
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode user class.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode user class.</summary>
    public class SMUser
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

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set user id.</summary>
        public string Id { get; set; }

        /// <summary>Get or set user name.</summary>
        public string Name { get; set; }

        /// <summary>Get or set user email.</summary>
        public string Email { get; set; }

        /// <summary>Return true if user is empty.</summary>
        public bool Empty { get { return SM.Empty(Id) || SM.Empty(Name); } }

        /// <summary>Get or set user UID.</summary>
        public string Uid { get; set; }

        /// <summary>Get or set user properties.</summary>
        public SMDictionary Properties { get; private set; } = new SMDictionary();

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMUser(SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMUser(SMUser _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Assign(_OtherInstance);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMUser _OtherInstance)
        {
            Id = _OtherInstance.Id;
            Name = _OtherInstance.Name;
            Email = _OtherInstance.Email;
            Uid = _OtherInstance.Uid;
            Properties.Assign(_OtherInstance.Properties);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = "";
            Name = "";
            Email = "";
            Uid = "";
            Properties.Clear();
        }

        /// <summary>Read item from current record of dataset.</summary>
        public void Read(SMDataset _Dataset)
        {
            int i;
            string c;
            if (_Dataset != null)
            {
                if (!_Dataset.Eof)
                {
                    Id = _Dataset.FieldStr("Id");
                    Name = _Dataset.FieldStr("Name");
                    Email = _Dataset.FieldStr("Email");
                    Uid = _Dataset.FieldStr("Uid");
                    Properties.Clear();
                    for (i = 0; i < _Dataset.Columns.Count; i++)
                    {
                        c = _Dataset.Columns[i].ColumnName;
                        Properties.Add(new SMDictionaryItem(c, _Dataset.FieldStr(c), _Dataset.Field(c)));
                    }
                }
            }
        }

        #endregion

        /* */

    }

    /* */

}