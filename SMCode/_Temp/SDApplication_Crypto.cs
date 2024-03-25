/*  -------------------------------------------------------
 *  SD_Crypto.cs 
 *  
 *  Libreria di funzioni di crittografia per SmartData.
 *  -------------------------------------------------------
 */

using System.Security.Cryptography;
using System.Text;

namespace SmartData
{

    /* */

    /// <summary>Classe di funzioni per il supporto della funzionalità low-code.</summary>
    public partial class SDApplication
    {

        /* */

        #region Properties

        /*  --------------------------------------------------------------------
         *  Properties
         *  --------------------------------------------------------------------
         */

        /// <summary>Get or set internal basic password.</summary>
        private string InternalPassword = "Sm4rtD4t4-R3g10n3M4rch4";

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Ritorna l'array di byte per l'implementazione della crittografia.</summary>
        public byte[] CryptoSalt(string _SaltKey, int _Length)
        {
            int b = 0, i = -1, j = 0;
            const string alfa = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+-*/=[](){}#°_.,:;!\"£$%&?^~";
            byte[] r = new byte[_Length];
            while (j < _Length)
            {
                i++;
                if (i >= _SaltKey.Length) i = 0;
                b = (b + alfa.IndexOf(_SaltKey[i])) % 256;
                r[j] = (byte)b;
                j++;
            }
            return r;
        }

        /// <summary>Ritorna la stringa decodificata con la chiave passata.</summary>
        public string DecryptString(string _Value, string _Key = null)
        {
            int iterations;
            byte[] bytes;
            _Value = _Value.Replace(" ", "+");
            bytes = Convert.FromBase64String(_Value);
            if (_Key == null) _Key = InternalPassword;
            iterations = 8 * (256 - _Key.Length);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Key, CryptoSalt(_Key, 32), iterations, HashAlgorithmName.SHA512);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    _Value = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return _Value;
        }

        /// <summary>Ritorna la stringa codificata con la chiave passata.</summary>
        public string EncryptString(string _Value, string _Key = null)
        {
            int iterations;
            byte[] bytes;
            bytes = Encoding.Unicode.GetBytes(_Value);
            if (_Key == null) _Key = InternalPassword;
            iterations = 8 * (256 - _Key.Length);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Key, CryptoSalt(_Key, 32), iterations, HashAlgorithmName.SHA512);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    _Value = Convert.ToBase64String(ms.ToArray());
                }
            }
            return _Value;
        }

        #endregion

        /* */

    }

    /* */

}
