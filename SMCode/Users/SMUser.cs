/*  ===========================================================================
 *  
 *  File:       SMUser.cs
 *  Version:    2.1.1
 *  Date:       April 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode user class.
 *
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode user class.</summary>
    public partial class SMUser
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        public readonly SMCode SM = null;

        /// <summary>User sex flag.</summary>
        private char sex = ' ';

        /// <summary>User text.</summary>
        private string text = "";

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set user id.</summary>
        public int IdUser { get; set; }

        /// <summary>Get or set user UID.</summary>
        public string UidUser { get; set; }

        /// <summary>Get or set user name.</summary>
        public string UserName { get; set; }

        /// <summary>Get or set user description.</summary>
        public string Text 
        {
            get
            {
                if (SM.Empty(text)) return (FirstName.Trim() + " " + LastName.Trim()).Trim();
                else return text;
            }
            set { text = value; }
        }

        /// <summary>Get or set user first name.</summary>
        public string FirstName { get; set; }

        /// <summary>Get or set user last name.</summary>
        public string LastName { get; set; }

        /// <summary>Get or set user email.</summary>
        public string Email { get; set; }

        /// <summary>Return true if user is empty.</summary>
        public bool Empty { get { return (IdUser < 1) || SM.Empty(UserName) || SM.Empty(Text); } }

        /// <summary>Get or set user password.</summary>
        public string Password { get; set; }

        /// <summary>Get or set user PIN.</summary>
        public int Pin { get; set; }

        /// <summary>Get or set user register code.</summary>
        public int Register { get; set; }

        /// <summary>Get or set user tax code.</summary>
        public string TaxCode { get; set; }

        /// <summary>Get or set user birth date.</summary>
        public DateTime BirthDate { get; set; }

        /// <summary>Get or set user sex.</summary>
        public char Sex 
        {
            get { return sex; }
            set
            {
                if ((value == 'f') || (value == 'F')) sex = 'F';
                else if ((value == 'm') || (value == 'M')) sex = 'M';
                else sex = ' ';
            }
        }

        /// <summary>Get or set user icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set user image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set user note.</summary>
        public string Note { get; set; }

        /// <summary>Get or set user selected organization.</summary>
        public SMOrganization Organization { get; private set; }

        /// <summary>Get or set user related organizations.</summary>
        public SMOrganizations Organizations { get; set; }

        /// <summary>Get or set user properties.</summary>
        public SMDictionary Properties { get; private set; }

        /// <summary>Get user related rules.</summary>
        public SMRules Rules { get; private set; }

        /// <summary>Get or set object tag.</summary>
        public object Tag { get; set; }

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
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
        }

        /// <summary>Class constructor.</summary>
        public SMUser(SMUser _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
        {
            Organization = new SMOrganization(SM);
            Organizations = new SMOrganizations(SM);
            Properties = new SMDictionary(SM);
            Rules = new SMRules(SM);
            Clear();
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
            IdUser = _OtherInstance.IdUser;
            UidUser = _OtherInstance.UidUser;
            UserName = _OtherInstance.UserName;
            Text = _OtherInstance.Text;
            FirstName = _OtherInstance.FirstName;
            LastName = _OtherInstance.LastName;
            Email = _OtherInstance.Email;
            Password = _OtherInstance.Password;
            Pin = _OtherInstance.Pin;
            Register = _OtherInstance.Register;
            TaxCode = _OtherInstance.TaxCode;
            BirthDate = _OtherInstance.BirthDate;
            Sex = _OtherInstance.Sex;
            Icon = _OtherInstance.Icon;
            Image= _OtherInstance.Image;
            Note = _OtherInstance.Note;
            Organization.Assign(_OtherInstance.Organization);
            Organizations.Assign(_OtherInstance.Organizations);
            Properties.Assign(_OtherInstance.Properties);
            Rules.Assign(_OtherInstance.Rules);
            Tag = _OtherInstance.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            IdUser = 0;
            UidUser = "";
            UserName = "";
            Text = "";
            FirstName = "";
            LastName = "";
            Email = "";
            Password = "";
            Pin = 0;
            Register = 0;
            TaxCode = "";
            BirthDate = DateTime.MinValue;
            Sex = ' ';
            Icon = "";
            Image = "";
            Note = "";
            Organization.Clear();
            Organizations.Clear();
            Properties.Clear();
            Rules.Clear();
            Tag = null;
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMUser)JsonSerializer.Deserialize(_JSON, null));
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

        /// <summary>Returns a string based on the user's gender.</summary>
        public string Gender(string _IfMale, string _IfFemale, string _IfNeutral = null)
        {
            if (sex == 'F') return _IfFemale;
            else if (sex == 'M') return _IfMale;
            else if (_IfNeutral == null) return _IfMale;
            else return _IfNeutral;
        }

        /// <summary>Return HASH code of user and password.</summary>
        public string Hash(string _User = null, string _Password=null)
        {
            if (_User != null) _User = UserName;
            if (_Password != null) _Password = Password;
            return SM.HashSHA256(_User + _Password + _User.Length.ToString() + _Password.Length.ToString());
        }

        /// <summary>Set current organization by id.</summary>
        public bool SetOrganization(int _IdOrganization)
        {
            int i = Organizations.Find(_IdOrganization);
            if (i > -1)
            {
                Organization.Assign(Organizations[i]);
                return true;
            }
            else return false;
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
        public string ToJSON64()
        {
            return SM.Base64Encode(ToJSON());
        }

        #endregion

        /* */

    }

    /* */

}