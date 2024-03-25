/*  -------------------------------------------------------
 *  SD_CodiceFiscale.cs 
 *  
 *  Libreria di funzioni di supporto per i codici fiscali.
 *  -------------------------------------------------------
 */

namespace SmartData
{

    /* */

    /// <summary>Libreria di funzioni di supporto per i codici fiscali.</summary>
    public partial class SDApplication
    {

        /* */

        #region Declarations

        /*  --------------------------------------------------------------------
         *  Declarations
         *  --------------------------------------------------------------------
         */

        /// <summary>CF months letters.</summary>
        private const string cfMonths = "ABCDEHLMPRST";

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Return "codice fiscale" checksum part or empty string if not valid.</summary>
        public string CF_Check(string _CodiceFiscale)
        {
            int i, sum, c;
            int[] odd = new int[26] { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
            string r = "";
            _CodiceFiscale = _CodiceFiscale.Trim().ToUpper();
            if (_CodiceFiscale.Length > 14)
            {
                sum = 0;
                for (i = 0; i < 15; i++)
                {
                    c = Asc(_CodiceFiscale[i]);
                    if ((_CodiceFiscale[i] >= '0') && (_CodiceFiscale[i] <= '9')) c += 17;
                    c -= 65;
                    if (i % 2 != 0) sum += c;
                    else sum += odd[c];
                }
                sum %= 26;
                r = Chr(sum + 65) + "";
            }
            return r;
        }

        /// <summary>Return "codice fiscale" birthday date and sex part or empty string if not valid.</summary>
        public string CF_Date(DateTime _BirthDay, string _Sex)
        {
            int i;
            string r = "";
            if (_BirthDay > DateTime.MinValue)
            {
                _Sex = _Sex.Trim().ToUpper();
                if (_Sex.Length > 0)
                {
                    r = _BirthDay.Year.ToString().PadLeft(2, '0');
                    r += cfMonths[_BirthDay.Month - 1];
                    i = _BirthDay.Day;
                    if (_Sex[0] == 'F') i += 40;
                    r += i.ToString().PadLeft(2, '0');
                }
            }
            return r;
        }

        /// <summary>Return "codice fiscale" first name part or empty string if not valid.</summary>
        public string CF_FirstName(string _FirstName)
        {
            string c, v;
            _FirstName = _FirstName.Trim().ToUpper();
            if (_FirstName.Length > 0)
            {
                c = Consonants(_FirstName);
                v = Vocals(_FirstName);
                if (c.Length > 3) c = c.Substring(0, 1) + c.Substring(2);
                if (v.Length > 0) return (c + v + "XX").Substring(0, 3);
                else return "";
            }
            else return "";
        }

        /// <summary>Return "codice fiscale" last name part or empty string if not valid.</summary>
        public string CF_LastName(string _LastName)
        {
            string c, v;
            _LastName = _LastName.Trim().ToUpper();
            if (_LastName.Length > 0)
            {
                c = Consonants(_LastName);
                v = Vocals(_LastName);
                if (v.Length > 0) return (c + v + "XX").Substring(0, 3);
                else return "";
            }
            else return "";
        }

        /// <summary>Ritorna il sesso del soggetto rappresentato dal codice fiscale passato.</summary>
        public string CF_Sex(string _CodiceFiscale) 
        {
            if (CF_Validate(_CodiceFiscale)) {
                if (_CodiceFiscale[9] > '3') return "F";
                else return "M";
            }
            else return "";
        }

        /// <summary>Ritorna true se il codice fiscale passato è formalmente corretto.</summary>
        public bool CF_Validate(string _CodiceFiscale)
        {
            if (_CodiceFiscale.Length != 16) return false;
            else return CF_Check(_CodiceFiscale.Substring(0, 15)) == _CodiceFiscale.Substring(15,1);
        }

        #endregion

        /* */

    }

    /* */

}
