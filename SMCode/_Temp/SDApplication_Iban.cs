/*  -------------------------------------------------------
 *  SD_Iban.cs 
 *  
 *  Libreria di funzioni di supporto per i codici IBAN.
 *  -------------------------------------------------------
 */

namespace SmartData
{

    /* */

    /// <summary>Libreria di funzioni di supporto per i codici IBAN.</summary>
    public partial class SDApplication
    {

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Returns IBAN CIN code relative to passed parameters.</summary>
        public string CIN(string _ABI_Code, string _CAB_Code, string _CC_Account_Number)
        {
            const string numbers = @"0123456789";
            const string letters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ-. ";
            const int divisor = 26;
            int i, j, k;
            int[] evens = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
            int[] odds = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23, 27, 28, 26 };
            string s, r = "";
            _ABI_Code = _ABI_Code.Trim().ToUpper();
            _CAB_Code = _CAB_Code.Trim().ToUpper();
            _CC_Account_Number = _CC_Account_Number.Trim().ToUpper();
            if (IsDigits(_ABI_Code) && IsDigits(_CAB_Code) && IsAlphaNum(_CC_Account_Number))
            {
                _ABI_Code = Right(@"00000" + _ABI_Code, 5);
                _CAB_Code = Right(@"00000" + _CAB_Code, 5);
                _CC_Account_Number = Right(@"000000000000" + _CC_Account_Number, 12);
                s = _ABI_Code + _CAB_Code + _CC_Account_Number;
                k = 0;
                i = 0;
                while (i < s.Length)
                {
                    j = numbers.IndexOf(s[i]);
                    if (j < 0) j = letters.IndexOf(s[i]);
                    if (j < 0)
                    {
                        k = -1;
                        i = s.Length;
                    }
                    else
                    {
                        if (i % 2 == 0) k += odds[j];
                        else k += evens[j];
                    }
                    i++;
                }
                if (k > -1) r = letters.Substring(k % divisor, 1);
            }
            return r;
        }

        /// <summary>Returns true  if CIN code passed is valid and compliant with others parameters.
        /// If CIN code passed is empty returns specified validate value.</summary>
        public bool ValidateCIN(string _CIN_Code, string _ABI_Code, string _CAB_Code, string _CC_Account_Number, bool _ValidateIfEmpty)
        {
            _CIN_Code = _CIN_Code.Trim().ToUpper();
            if (_CIN_Code.Length > 0) return _CIN_Code == CIN(_ABI_Code, _CAB_Code, _CC_Account_Number);
            else return _ValidateIfEmpty;
        }

        #endregion

        /* */

    }

    /* */

}
