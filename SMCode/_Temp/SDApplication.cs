/*  -------------------------------------------------------
 *  SD_Lib.cs 
 *  
 *  Libreria di funzioni base per SmartData.
 *  -------------------------------------------------------
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Xml;

namespace SmartData
{

    /* */

    /// <summary>Classe di funzioni per il supporto della funzionalità low-code.</summary>
    public partial class SDApplication
    {

        /* */

        #region Declarations

        /*  --------------------------------------------------------------------
         *  Declarations
         *  --------------------------------------------------------------------
         */

        /// <summary>Seme per la codifica veloce degli interi.</summary>
        private const int INT_SEED = 235713;

        /// <summary>Lunghezza massima dei campi memo.</summary>
        public const int MEMO_MAX_LEN = 4800;

        /// <summary>Lunghezza massima dei campi numerici.</summary>
        public const int NUMBER_MAX_LEN = 18;

        /// <summary>Lunghezza massima dei campi di testo.</summary>
        public const int TEXT_MAX_LEN = 250;

        /// <summary>Formattazione ISO della data.</summary>
        public const string FMT_DATE_ISO = "yyyy-MM-ddTHH:mm:ss.fff";

        /// <summary>Generatore di numeri casuali.</summary>
        private Random random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000 + DateTime.Now.Minute * 60000);

        /// <summary>Id dell'istanza SmartData.</summary>
        public readonly string Id;

        #endregion

        /* */

        #region Delegates

        /*  --------------------------------------------------------------------
         *  Delegates
         *  --------------------------------------------------------------------
         */

        /// <summary>Delegato per l'evento generico di comparazione.</summary>
        public delegate int SmartDataCompare(object _A, object _B);

        #endregion

        /* */

        #region Properties

        /*  --------------------------------------------------------------------
         *  Properties
         *  --------------------------------------------------------------------
         */

        /// <summary>Legge o imposta il nome del dominio di applicazione corrente.</summary>
        public string ApplicationDomainName { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        /// <summary>Legge o imposta il separatore decimale.</summary>
        public string DecimalSeparator { get; set; } = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        /// <summary>Ultima eccezione rilevata.</summary>
        public Exception ErrorException { get; set; } = null;

        /// <summary>Ultimo messaggio di errore rilevato.</summary>
        public string ErrorMessage { get; set; } = "";

        /// <summary>Restituisce true se si è verificato un errore.</summary>
        public bool IsError
        {
            get
            {
                if (Empty(ErrorMessage) && (ErrorException == null)) return false;
                else return true;
            }
        }

        /// <summary>Legge o imposta il configuratore JSON dell'applicazione.</summary>
        public IConfigurationRoot JsonConfigurationBuilder { get; set; } = null;

        /// <summary>Legge o imposta il percorso del file di log.</summary>
        public string LogPath { get; set; } = "";

        public SDParametri Parametri { get; private set; } = null;

        /// <summary>Legge o imposta il percorso principale del server.</summary>
        public static string ServerRootPath { get; set; } = "";

        /// <summary>Legge o imposta il separatore delle migliaia.</summary>
        public string ThousandsSeparator { get; set; } = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;

        #endregion

        /* */

        #region Properties - Collezioni

        /*  --------------------------------------------------------------------
         *  Properties - Collezioni
         *  --------------------------------------------------------------------
         */

        /// <summary>Collezione di allegati corrente.</summary>
        public SDAllegati Allegati { get; private set; }

        /// <summary>Collezione di categorie corrente.</summary>
        public SDCategorie Categorie { get; private set; }

        /// <summary>Collezione di form corrente.</summary>
        public SDForms Forms { get; private set; }

        /// <summary>Restituisce la collezione di menu corrente.</summary>
        public SDMenus Menus { get; private set; }

        /// <summary>Legge o imposta l'istanza di render corrente.</summary>
        public SDRender Render { get; private set; }

        /// <summary>Collezione di ruoli corrente.</summary>
        public SDRuoli Ruoli { get; private set; }

        /// <summary>Collezione di utenti corrente.</summary>
        public SDUtenti Utenti { get; private set; }

        #endregion

        /* */

        #region Initialization

        /*  --------------------------------------------------------------------
         *  Initialization
         *  --------------------------------------------------------------------
         */

        /// <summary>Costruttore di istanza.</summary>
        public SDApplication()
        {
            Id = GUID();
            Allegati = new SDAllegati(this);
            Categorie = new SDCategorie(this);
            Forms = new SDForms(this);
            Menus = new SDMenus(this);
            Parametri = new SDParametri(this);
            Render = new SDRender(this);
            Ruoli = new SDRuoli(this);
            Utenti = new SDUtenti(this);
        }

        #endregion
        
        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Ritorna l'espressione SQL per includere i record in funzione del valore di debugger.</summary>
        public string AndDebugger(string _TableName = "")
        {
            _TableName = _TableName.Trim();
            if (_TableName.Length > 0) _TableName = _TableName + ".";
            else _TableName = "";
            if (IsDebugger()) return "AND((" + _TableName + "Debugger IS NULL)OR(" + _TableName + "Debugger>-1))";
            else return "AND((" + _TableName + "Debugger IS NULL)OR(" + _TableName + "Debugger<1))";
        }

        /// <summary>Ritorna l'espressione SQL per includere i record dell'ente passato o, se omesso, di quello relativo all'utente connesso.</summary>
        public string AndEnte(int _IdEnte, string _TableName = "")
        {
            if (_IdEnte > 0)
            {
                _TableName = _TableName.Trim();
                if (_TableName.Length > 0) _TableName = _TableName + ".";
                else _TableName = "";
                return "AND(" + _TableName + "IdEnte=" + _IdEnte.ToString() + ")";
            }
            else return "";
        }

        /// <summary>Ritorna l'espressione anti caching sql per il campo passato che deve essere diverso di un numero negativo casuale.</summary>
        public string AndNoCacheInt(string _FieldName, string _Operator = "<>")
        {
            return "AND(" + _FieldName + _Operator + (-Rnd(32000) - 32000).ToString() + ")";
        }

        /// <summary>Ritorna l'espressione anti caching sql per il campo passato che deve essere diverso di una stringa casuale.</summary>
        public string AndNoCacheStr(string _FieldName, string _Operator = "<>")
        {
            return "AND(" + _FieldName + _Operator + Quote(RndPassword(24)) + ")";
        }

        /// <summary>Ritorna l'espressione SQL per escludere i record con il campo Eliminato non nullo e diverso da 0.</summary>
        public string AndNotEliminato(string _TableName = "")
        {
            _TableName = _TableName.Trim();
            if (_TableName.Length > 0) _TableName = _TableName + ".";
            else _TableName = "";
            return "AND((" + _TableName + "Eliminato IS NULL)OR(" + _TableName + "Eliminato=0))";
        }

        /// <summary>Ritorna una stringa con una rappresentazione
        /// numerica, non biunivoca, di alcuni caratteri.</summary>
        public string DinkyStr(string _Value)
        {
            if (Empty(_Value)) return "";
            else
            {
                int i,j;
                const string a = "oizeas", b = "012345";
                StringBuilder r = new StringBuilder();
                for (i=0;i<_Value.Length;i++)
                {
                    j = a.IndexOf(_Value[i]);
                    if (j < 0) r.Append(_Value[i]);
                    else r.Append(b[j]);
                }
                return r.ToString();
            }
        }

        /// <summary>Redirige sulla pagina di errore con il callback passato 
        /// o se omesso con la pagina di default.</summary>
        public void ErrorRedirect(HttpResponse _Response, string _BackUrl = null, string _ErrorMessage = null, string _ErrorException = null, string _ErrorStack = null)
        {
            if (Empty(_BackUrl)) _BackUrl = "/Index";
            if (Empty(_ErrorMessage)) _ErrorMessage = ErrorMessageStr();
            if (Empty(_ErrorException)) _ErrorException = ErrorExceptionStr();
            if (Empty(_ErrorStack)) _ErrorStack = ErrorStackStr();
            _Response.Cookies.Append("SD_BKU", _BackUrl);
            _Response.Cookies.Append("SD_ERR", _ErrorMessage);
            _Response.Cookies.Append("SD_EXC", _ErrorException);
            _Response.Cookies.Append("SD_STK", _ErrorStack);
            _Response.Redirect("/Error");
        }

        /// <summary>Redirige sulla pagina di errore con il callback passato 
        /// o se omesso con la pagina di default.</summary>
        public string ErrorRedirectStr(HttpResponse _Response, string _BackUrl = null, string _ErrorMessage = null, string _ErrorException = null, string _ErrorStack = null)
        {
            if (Empty(_BackUrl)) _BackUrl = "/Index";
            if (Empty(_ErrorMessage)) _ErrorMessage = ErrorMessageStr();
            if (Empty(_ErrorException)) _ErrorException = ErrorExceptionStr();
            if (Empty(_ErrorStack)) _ErrorStack = ErrorStackStr();
            _Response.Cookies.Append("SD_BKU", _BackUrl);
            _Response.Cookies.Append("SD_ERR", _ErrorMessage);
            _Response.Cookies.Append("SD_EXC", _ErrorException);
            _Response.Cookies.Append("SD_STK", _ErrorStack);
            return "/Error";
        }

        /// <summary>Ritorna la stringa di visualizzazione del messaggio dell'eccezione.</summary>
        public string ErrorStackStr()
        {
            if (ErrorException == null) return "";
            else return ToStr(ErrorException.StackTrace);
        }

        /// <summary>Estrae l'id dell'elemento, il dettaglio e la riga dal nome dell'elemento.</summary>
        public int ExtractElementId(string _ElementId, ref int _IdDetail, ref int _IdRow, bool _SkipPrefix = false)
        {
            int i, id = -1;
            _ElementId += '_';
            _IdDetail = -1;
            _IdRow = -1;
            if (!_SkipPrefix)
            {
                i = _ElementId.IndexOf('_');
                if (i > -1) _ElementId = _ElementId.Substring(i + 1);
            }
            i = _ElementId.IndexOf('_');
            if (i > -1)
            {
                id = ToInt(_ElementId.Substring(0, i));
                _ElementId = _ElementId.Substring(i + 1);
                i = _ElementId.IndexOf('_');
                if (i>-1)
                {
                    _IdDetail = ToInt(_ElementId.Substring(0, i));
                    _ElementId = _ElementId.Substring(i + 1);
                    i = _ElementId.IndexOf('_');
                    if (i > -1) _IdRow = ToInt(_ElementId.Substring(0, i));
                }
            }
            return id;
        }

        /// <summary>Ritorna l'indice dell'elemento dell'array comparato con quello passato oppure -1 se fallisce.</summary>
        public static int Find(object _Value, List<object> _Objects, SmartDataCompare _CompareEvent, bool _BinarySearch = true)
        {
            int i, max, mid, min, r = -1;
            if ((_Value != null) && (_Objects != null))
            {
                if (_BinarySearch)
                {
                    min = 0;
                    max = _Objects.Count - 1;
                    while ((r < 0) && (min <= max))
                    {
                        mid = (min + max) / 2;
                        i = _CompareEvent(_Value, _Objects[mid]);
                        if (i == 0) r = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    while (r > 0)
                    {
                        if (_CompareEvent(_Value, _Objects[r - 1]) == 0) r--;
                        else break;
                    }
                }
                else
                {
                    i = 0;
                    while ((r < 0) && (i < _Objects.Count))
                    {
                        if (_CompareEvent(_Value, _Objects[i]) == 0) r = i;
                        i++;
                    }
                }
            }
            return r;
        }

        /// <summary>Ritorna l'indice dell'elemento dell'array comparato con quello passato e ordinato secondo l'indice passato oppure -1 se fallisce.</summary>
        public static int Find(object _Value, List<object> _Objects, List<int> _Index, SmartDataCompare _CompareEvent, bool _BinarySearch = true)
        {
            int i, max, mid, min, r = -1;
            if ((_Value != null) && (_Objects != null))
            {
                if (_BinarySearch)
                {
                    min = 0;
                    max = _Index.Count - 1;
                    while ((r < 0) && (min <= max))
                    {
                        mid = (min + max) / 2;
                        i = _CompareEvent(_Value, _Objects[_Index[mid]]);
                        if (i == 0) r = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    while (r > 0)
                    {
                        if (_CompareEvent(_Value, _Objects[_Index[r - 1]]) == 0) r--;
                        else break;
                    }
                    if (r > -1) r = _Index[r];
                }
                else
                {
                    i = 0;
                    while ((r < 0) && (i < _Objects.Count))
                    {
                        if (_CompareEvent(_Value, _Objects[i]) == 0) r = i;
                        i++;
                    }
                }
            }
            return r;
        }

        /// <summary>Ritorna la stringa passata con la formattazione specificata.</summary>
        public string Format(string _Value, string _Format)
        {
            double d;
            if (_Value != null)
            {
                if (_Format == null) return _Value;
                else _Format = _Format.Trim().ToUpper();
                if (_Format.Length > 0)
                {
                    if (_Format.Trim().Length < 1) return _Value;
                    else if (double.TryParse(_Value.Trim(), out d))
                    {
                        if ((_Format == "EUR")|| (_Format == "EU"))
                        {
                            return d.ToString("###,###,###,###,##0.00");
                        }
                        else if ((_Format == "EURNZ")|| (_Format == "EUNZ"))
                        {
                            if (d == 0.0d) return "";
                            else return d.ToString("###,###,###,###,##0.00");
                        }
                        else if (_Format == "NZ")
                        {
                            if (d == 0) return "";
                            else return d.ToString("###,###,###,###,###.#########");
                        }
                        else if (_Format == "INT") return Math.Floor(d).ToString("###############");
                        else if (_Format == "INTNZ")
                        {
                            if (d == 0.0) return "";
                            else return Math.Floor(d).ToString("###############");
                        }
                        else if (_Format.StartsWith("DNZ"))
                        {
                            int q = ToInt(_Format.Substring(3));
                            string v = "";
                            if ((q > 0) && (q < 10)) v = ".#########".Substring(0, q + 1);
                            if (d == 0) return "";
                            else return d.ToString("###,###,###,###,###" + v);
                        }
                        else if (_Format.StartsWith("D"))
                        {
                            int q = ToInt(_Format.Substring(1));
                            string v = "";
                            if ((q > 0) && (q < 10)) v = ".#########".Substring(0, q + 1);
                            return d.ToString("###,###,###,###,###" + v);
                        }
                        else return d.ToString("###,###,###,###,###.#########");
                    }
                    else if (_Format == "UPPER") return _Value.ToUpper();
                    else if (_Format == "LOWER") return _Value.ToLower();
                    else return _Value;
                }
                else return _Value;
            }
            else return "";
        }

        /// <summary>Ritorna l'hash SHA256 dell'array di byte passato.</summary>
        public string HashSHA256(byte[] _Bytes)
        {
            int i;
            byte[] data;
            SHA256 sha;
            StringBuilder sb;
            try
            {
                sb = new StringBuilder();
                sha = SHA256.Create();
                data = sha.ComputeHash(_Bytes);
                for (i = 0; i < data.Length; i++) sb.Append(data[i].ToString("x2"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>Ritorna l'hash SHA256 della stringa passata.</summary>
        public string HashSHA256(string _Value)
        {
            byte[] data;
            try
            {
                data = Encoding.UTF8.GetBytes(_Value);
                return HashSHA256(data);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>Ritorna l'hash SHA256 dello stream passato.</summary>
        public string HashSHA256(Stream _Stream)
        {
            MemoryStream memory;
            try
            {
                memory = new MemoryStream();
                _Stream.CopyTo(memory);
                return HashSHA256(memory.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>Ritorna la stringa esadecimale corrispondente al byte array passato.</summary>
        public byte[] HexToByteArray(string _Value)
        {
            int i, h;
            byte[] r = null;
            try
            {
                if (_Value != null)
                {
                    h = _Value.Length / 2;
                    if (h > 0)
                    {
                        r = new byte[h];
                        i = 0;
                        while (i < h)
                        {
                            r[i] = (byte)(int.Parse(_Value.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber) % 256);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                r = null;
            }
            return r;
        }

        /// <summary>Esegue la codifica forte in HTML rimpiazzando i singoli apici con &quot;
        /// e i doppi con &quot;.</summary>
        public string HtmlEncode(string _Value, char _ReplaceSpecialCharsWith='\0')
        {
            int i;
            StringBuilder r = new StringBuilder();
            char[] cs = HttpUtility.HtmlEncode(_Value).Replace("\"", "&quot;").Replace("'", "&apos;").ToCharArray();
            if (_ReplaceSpecialCharsWith == '\0')
            {
                foreach (char c in cs)
                {
                    i = Convert.ToInt32(c);
                    if (i > 127) r.AppendFormat("&#{0};", i);
                    else r.Append(c);
                }
            }
            else
            {
                foreach (char c in cs)
                {
                    i = Convert.ToInt32(c);
                    if (i > 127) r.Append(_ReplaceSpecialCharsWith);
                    else r.Append(c);
                }
            }
            return r.ToString();
        }

        /// <summary>Ritorna uno dei valori specificati in funzione del risultato del test.</summary>
        public string Iif(bool _Test, string _ValueIfTrue, string _ValueIfFalse)
        {
            if (_Test) return _ValueIfTrue;
            else return _ValueIfFalse;
        }

        /// <summary>Ritorna uno dei valori specificati in funzione del risultato del test.</summary>
        public int Iif(bool _Test, int _ValueIfTrue, int _ValueIfFalse)
        {
            if (_Test) return _ValueIfTrue;
            else return _ValueIfFalse;
        }

        /// <summary>Ritorna true se la procedura è in modalità di debug.</summary>
        public bool IsDebugger()
        {
            return Debugger.IsAttached;
        }

        /// <summary>Returns true if all characters included in string are alphabet characters.</summary>
        public bool IsAlpha(string _String)
        {
            int l = _String.Length;
            bool r = l > 0;
            while (r && (l > 0))
            {
                l--;
                r = ((_String[l] >= 'a') && (_String[l] <= 'z'))
                    || ((_String[l] >= 'A') && (_String[l] <= 'Z'));
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are alphabet characters or digits.</summary>
        public bool IsAlphaNum(string _String)
        {
            int l = _String.Length;
            bool r = l > 0;
            while (r && (l > 0))
            {
                l--;
                r = ((_String[l] >= '0') && (_String[l] <= '9'))
                    || ((_String[l] >= 'a') && (_String[l] <= 'z'))
                    || ((_String[l] >= 'A') && (_String[l] <= 'Z'));
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are included in passed char set.</summary>
        public bool IsCharSet(string _String, string _CharSet)
        {
            int l = _String.Length;
            bool r = true;
            while (r && l > 0)
            {
                l--;
                r = _CharSet.IndexOf(_String[l]) > -1;
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are digits.</summary>
        public bool IsDigits(string _String)
        {
            int l = _String.Length;
            bool r = l > 0;
            while (r && (l > 0))
            {
                l--;
                r = (_String[l] >= '0') && (_String[l] <= '9');
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are hex digits.</summary>
        public bool IsHex(string _String)
        {
            int l = _String.Length;
            bool r = l > 0;
            while (r && (l > 0))
            {
                l--;
                r = ((_String[l] >= '0') && (_String[l] <= '9'))
                    || ((_String[l] >= 'A') && (_String[l] <= 'F'))
                    || ((_String[l] >= 'a') && (_String[l] <= 'f'));
            }
            return r;
        }

        /// <summary>Ritorna il valore del parametro di configurazione specificato nella chiave
        /// e contenuto nel file di configurazione appsettings.json.</summary>
        public string JsonConfiguration(string _Key)
        {
            try
            {
                if (JsonConfigurationBuilder == null)
                {
                    JsonConfigurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                }
                return JsonConfigurationBuilder[_Key];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>Ritorna la stringa JSON contenente il risultato con il parametri passati.</summary>
        public string JsonResult(int _Result, string _Message = null, bool _AppendError = false) {
            if (_Message == null) _Message = ErrorMessageStr(); 
            else if (_AppendError) _Message = ErrorMessageStr(_Message);
            return JsonSerializer.Serialize(new { Status = Iif(_Result < 0, "KO", "OK"), Result = _Result, Message = _Message });
        }

        /// <summary>Returns first length characters of string from left.</summary>
        public string Left(string _String, int _Length)
        {
            if (_Length > _String.Length) _Length = _String.Length;
            if (_Length > 0) return _String.Substring(0, _Length);
            else return "";
        }

        /// <summary>Write log.</summary>
        public bool Log(SDLogType _Type, string _Message = "", bool _ConsoleOutput = true)
        {
            string l;
            try
            {
                //
                // Separators
                //
                if (_Message.Trim().Length < 1)
                {
                    if (_Type == SDLogType.Separator)
                    {
                        _Message = "====================================================================================================";
                    }
                    else if (_Type == SDLogType.Line)
                    {
                        _Message = "----------------------------------------------------------------------------------------------------";
                    }
                }
                //
                // Console output
                //
                if (_ConsoleOutput && IsDebugger())
                {
                    if (_Type == SDLogType.Error) Console.WriteLine(@"(X) Errore: " + _Message);
                    else if (_Type == SDLogType.Warning) Console.WriteLine(@"/!\ Attenzione: " + _Message);
                    else Console.WriteLine(_Message);
                }
                //
                // Log output
                //
                l = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss.fff") + " - " + ApplicationDomainName;
                if (_Type == SDLogType.Error) l += @" - (X)";
                else if (_Type == SDLogType.Warning) l += @" - /!\)";
                else if (_Type == SDLogType.Information) l += @" - [i]";
                else if (_Type == SDLogType.Separator) l += @" - ===";
                else if (_Type == SDLogType.Line) l += @" - ---";
                else l += @" - ???";
                l += @" - " + _Message.Trim().Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
                l += "\r\n";
                if (LogPath.Trim().Length < 1)
                {
                    LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDomainName + "_Log.log");
                }
                if (File.Exists(LogPath))
                {
                    FileInfo fi = new FileInfo(LogPath);
                    if (fi.Length > 128 * 1024)
                    {
                        File.Move(LogPath, ApplicationDomainName + "_Log_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss") + ".log");
                    }
                }
                File.AppendAllText(LogPath, l);
                //
                // Return
                //
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>Returns portion of string starting at position index and getting length chars.</summary>
        public string Mid(string _String, int _Index, int _Length)
        {
            if (_String.Length > 0)
            {
                if (_Length > 0)
                {
                    if (_Index < 0) _Index = 0;
                    if (_Index < _String.Length)
                    {
                        if (_Index + _Length > _String.Length) return _String.Substring(_Index);
                        else return _String.Substring(_Index, _Length);
                    }
                    else return "";
                }
                else return "";
            }
            else return "";
        }

        /// <summary>Ritorna l'argomento in base 64 contenente il valore del parametro sd_q per la query string, 
        /// contenente le informazioni da passare nella chiamata.</summary>
        public string Q(SDForm _Form = null, int _IdDettaglio = -1, int _IdRiga = -1)
        {
            SDParametri p = new SDParametri(this);
            if (_Form == null) _Form = Forms.Current;
            // user informations
            if (Utenti.Current != null)
            {
                if (Utenti.Current.IdUtente > 0) p.Set("sd_uid", Utenti.Current.IdUtente.ToString());
            }
            // form informations
            if (_Form != null)
            {
                if (!Empty(_Form.SessionId)) p.Set("sd_fid", _Form.SessionId.ToString());
                if (!Empty(_Form.IdForm)) p.Set("sd_frm", _Form.IdForm);
                if (_Form.IdDocumento > 0) p.Set("sd_doc", _Form.IdDocumento.ToString());
            }
            // details informations
            if (_IdDettaglio > -1)
            {
                p.Set("sd_det", _IdDettaglio.ToString());
                if (_IdRiga > -1) p.Set("sd_row", _IdRiga.ToString());
            }
            return p.ToJSON64();
        }

        /// <summary>Ritorna una stringa da usare come identificatore nelle richieste HTTP.
        /// Il valore dell'id varia ogni 30 secondi.</summary>
        public string RequestId()
        {
            return (DateTime.Now.Ticks / (TimeSpan.TicksPerSecond * 30)).ToString();
        }

        /// <summary>Restituisce i parametri passati in una riga JSON in base 64.</summary>
        public string RigaJSON64(SDForm _Form = null, int _IdDocumento = -1, int _IdDettagli = -1, int _IdRiga = -1)
        {
            SDParametri p = new SDParametri(this);
            return p.ToRigaJSON64(_Form, _IdDocumento, _IdDettagli, _IdRiga);
        }

        /// <summary>Ritorna un numero casuale compreso tra 0 ed il massimo specificato.</summary>
        public int Rnd(int _MaxValue)
        {
            return random.Next(_MaxValue + 1);
        }

        /// <summary>Ritorna una password casuale di 10 caratteri.</summary>
        public string RndPassword(int _Length=10)
        {
            string a = "abcdefghijklmnopqrstuvwxyz", n = "0123456789", s = "@#-&$!:";
            StringBuilder r = new StringBuilder();
            r.Append(("" + a[Rnd(a.Length - 1)]).ToUpper());
            r.Append(a[Rnd(a.Length - 1)]);
            r.Append(n[Rnd(n.Length - 1)]);
            r.Append(s[Rnd(s.Length - 1)]);
            r.Append(("" + a[Rnd(a.Length - 1)]).ToUpper());
            r.Append(a[Rnd(a.Length - 1)]);
            r.Append(n[Rnd(n.Length - 1)]);
            r.Append(s[Rnd(s.Length - 1)]);
            r.Append(n[Rnd(n.Length - 1)]);
            r.Append(a[Rnd(a.Length - 1)]);
            while (r.Length < _Length) r.Append(a[Rnd(a.Length - 1)]);
            if (r.Length>_Length) return r.ToString().Substring(0,_Length);
            else return r.ToString();
        }

        /// <summary>Ordina la lista degli oggetti passata con la funzione di comparazione indicata.</summary>
        public static void Sort(List<object> _Objects, SmartDataCompare _CompareEvent, bool _SortOnAddToSortedArray = false)
        {
            int i;
            bool b = true;
            object w;
            while (b)
            {
                b = false;
                i = _Objects.Count - 1;
                while (i > 0)
                {
                    if (_CompareEvent(_Objects[i], _Objects[i - 1]) < 0)
                    {
                        b = true;
                        w = _Objects[i];
                        _Objects[i] = _Objects[i - 1];
                        _Objects[i - 1] = w;
                    }
                    else if (_SortOnAddToSortedArray)
                    {
                        b = false;
                        i = 0;
                    }
                    i--;
                }
            }
        }

        /// <summary>Ordina l'indice relativo alla lista degli oggetti passati con la funzione di comparazione indicata.</summary>
        public static void SortIndex(List<int> _Index, List<object> _Objects, SmartDataCompare _CompareEvent, bool _SortOnAddToSortedArray = false)
        {
            int i, w;
            bool b = true;
            while (b)
            {
                b = false;
                i = _Index.Count - 1;
                while (i > 0)
                {
                    if (_CompareEvent(_Objects[_Index[i]], _Objects[_Index[i - 1]]) < 0)
                    {
                        b = true;
                        w = _Index[i];
                        _Index[i] = _Index[i - 1];
                        _Index[i - 1] = w;
                    }
                    else if (_SortOnAddToSortedArray)
                    {
                        b = false;
                        i = 0;
                    }
                    i--;
                }
            }
        }

        /// <summary>Ritorna una collezione di stringhe ottenuta facendo 
        /// lo split del valore con i separatori passati.</summary>
        public List<string> Split(string _Value, string _Separators)
        {
            int i;
            string[] a;
            List<string> r = new List<string>();
            a = _Value.Split(_Separators.ToArray());
            if (a != null)
            {
                for (i = 0; i < a.Length; i++) r.Add(a[i]);
            }
            return r;
        }

        /// <summary>Ritorna una collezione di stringhe ottenuta facendo 
        /// lo split del valore con i separatori passati.</summary>
        public List<int> SplitInt(string _Value, string _Separators)
        {
            int i;
            string[] a;
            List<int> r = new List<int>();
            a = _Value.Split(_Separators.ToArray());
            if (a != null)
            {
                for (i = 0; i < a.Length; i++)
                {
                    if (a[i] != null) r.Add(ToInt(a[i].Trim()));
                }
            }
            return r;
        }

        /// <summary>Ritorna l'oggetto passato come array di bytes oppure null se non valido.</summary>
        public byte[] ToBlob(object _Value)
        {
            if (_Value == null) return null;
            else if (_Value == DBNull.Value) return null;
            else if (_Value is string) return Base64DecodeBytes(_Value.ToString());
            else return (byte[])_Value;
        }

        /// <summary>Ritorna il valore booleano passato oppure FALSE se null.</summary>
        public bool ToBool(bool? _Value)
        {
            if (_Value.HasValue) return _Value.Value;
            else return false;
        }

        /// <summary>Ritorna il valore booleano passato oppure quello previsto se null.</summary>
        public bool ToBool(bool? _Value, bool _ValueIfNull)
        {
            if (_Value.HasValue) return _Value.Value;
            else return _ValueIfNull;
        }

        /// <summary>Ritorna il valore booleano espresso dalla stringa passata.</summary>
        public bool ToBool(string _Value)
        {
            try
            {
                if (_Value == null) return false;
                else
                {
                    char c = (_Value.Trim().ToLower() + ' ')[0];
                    if ((c == 't') || (c == 'v') || (c == '1') || (c == 's') || (c == 'y')) return true;
                    else if ((c == 'f') || (c == '0') || (c == 'n') || (c == ' ')) return false;
                    else return Convert.ToBoolean(_Value);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Ritorna il valore booleano passato oppure FALSE se null.</summary>
        public bool ToBool(object _Value)
        {
            try
            {
                if (_Value == null) return false;
                else if (_Value is bool) return (bool)_Value;
                else return ToBool(_Value.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>Ritorna il valore booleano se l'intero passato è superiore a 0.</summary>
        public bool ToBool(int _Value)
        {
            try
            {
                return _Value > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>Ritorna la data espressa dalla stringa passata senza orario.</summary>
        public DateTime ToDate(string _Value)
        {
            try
            {
                if (Empty(_Value)) return DateTime.MinValue;
                else return ToDate(Convert.ToDateTime(_Value));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>Ritorna la data passata senza l'orario.</summary>
        public DateTime ToDate(DateTime _Value)
        {
            try
            {
                return new DateTime(_Value.Year, _Value.Month, _Value.Day);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>Ritorna la data passata senza l'orario.</summary>
        public DateTime ToDate(Object _Value)
        {
            try
            {
                if (_Value == null) return DateTime.MinValue;
                else if (_Value == DBNull.Value) return DateTime.MinValue;
                else return ToDate(_Value.ToString());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>Ritorna la data passata senza l'orario.</summary>
        public DateTime ToDate(DateTime? _Value)
        {
            if (_Value.HasValue) return new DateTime(_Value.Value.Year, _Value.Value.Month, _Value.Value.Day);
            else return DateTime.MinValue;
        }

        /// <summary>Ritorna la data espressa dalla stringa passata.</summary>
        public DateTime ToDateTime(string _Value)
        {
            DateTime r;
            if (DateTime.TryParse(_Value, out r)) return r;
            else return DateTime.MinValue;
        }

        /// <summary>Ritorna la data passata.</summary>
        public DateTime ToDateTime(Object _Value)
        {
            try
            {
                if (_Value == null) return DateTime.MinValue;
                else if (_Value == DBNull.Value) return DateTime.MinValue;
                else return ToDateTime(_Value.ToString());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>Ritorna la data espressa dalla stringa passata.</summary>
        public DateTime ToDateTime(DateTime? _Value)
        {
            if (_Value.HasValue) return _Value.Value;
            else return DateTime.MinValue;
        }

        /// <summary>Ritorna il decimale espresso dalla stringa passata.</summary>
        public decimal ToDecimal(decimal? _Value)
        {
            if (_Value.HasValue) return _Value.Value;
            else return 0;
        }

        /// <summary>Ritorna l'intero espresso dalla stringa passata.</summary>
        public double ToDouble(string _Value, string _ThousandsSeparator = null)
        {
            try
            {
                if (_ThousandsSeparator == null) _ThousandsSeparator = ThousandsSeparator;
                if (_Value == null) return 0;
                else
                {
                    _Value = _Value.Replace(_ThousandsSeparator, "");
                    if (_Value.Trim().Length < 1) return 0;
                    else return Double.Parse(_Value);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Ritorna il valore passato convertito in doppia precisione.</summary>
        public double ToDouble(decimal? _Value)
        {
            if (_Value.HasValue) return Convert.ToDouble(_Value.Value);
            else return 0.0d;
        }

        /// <summary>Ritorna l'intero espresso dalla stringa passata.</summary>
        public double ToDouble(object _Value)
        {
            try
            {
                if (_Value == null) return 0;
                return ToDouble(_Value.ToString(), ThousandsSeparator);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Ritorna l'indirizzo composto dai parametri passati.</summary>
        public string ToIndirizzo(string _Via, string _CAP, string _Localita)
        {
            string r = "";
            if (!Empty(_Via)) r = Cat(r, _Via.Trim(), " ");
            if (!Empty(_CAP)) r = Cat(r, _CAP.Trim(), " ");
            if (!Empty(_Localita)) r = Cat(r, _Localita.Trim(), " ");
            return r;
        }

        /// <summary>Ritorna l'intero passato oppure 0 se null.</summary>
        public int ToInt(int? _Value)
        {
            if (_Value.HasValue) return _Value.Value;
            else return 0;
        }

        /// <summary>Ritorna l'intero passato oppure il valore di default se null.</summary>
        public int ToInt(int? _Value, int _Default)
        {
            if (_Value.HasValue) return _Value.Value;
            else return _Default;
        }

        /// <summary>Ritorna l'intero espresso dalla stringa passata.</summary>
        public int ToInt(string _Value)
        {
            try
            {
                if (_Value == null) return 0;
                else if (_Value.Trim().Length < 1) return 0;
                else return Int32.Parse(_Value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Ritorna l'intero espresso dal valore boolean passato.</summary>
        public int ToInt(bool _Value)
        {
            try
            {
                if (_Value) return 1;
                else return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Ritorna l'intero espresso dall'oggetto passato oppure 0 se null.</summary>
        public int ToInt(object _Value)
        {
            try
            {
                if (_Value == null) return 0;
                else return ToInt(_Value.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Ritorna l'intero espresso dalla stringa passata.</summary>
        public int ToInt(string _Value, int _IfNullOrEmpty)
        {
            try
            {
                if (_Value == null) return _IfNullOrEmpty;
                else if (_Value.Trim().Length < 1) return _IfNullOrEmpty;
                else return Int32.Parse(_Value);
            }
            catch
            {
                return _IfNullOrEmpty;
            }
        }

        /// <summary>Ritorna l'intero espresso dalla stringa passata.</summary>
        public int? ToIntNull(string _Value)
        {
            try
            {
                if (_Value == null) return null;
                else if (_Value.Trim().Length < 1) return 0;
                else return Int32.Parse(_Value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Ritorna il valore stringa espresso dal carattere passato.</summary>
        public string ToStr(char? _Value)
        {
            if (_Value.HasValue) return "" + _Value;
            else return "";
        }

        /// <summary>Ritorna il valore stringa espresso dal carattere passato.</summary>
        public string ToStr(object _Value)
        {
            if (_Value == null) return "";
            else if (_Value is DateTime) return ((DateTime)_Value).ToShortDateString();
            else return _Value.ToString();
        }

        /// <summary>Ritorna il valore stringa passato. Se la stringa è null ritorna "".</summary>
        public string ToStr(string _Value)
        {
            if (_Value == null) return "";
            else return _Value;
        }

        /// <summary>Ritorna il valore stringa espresso dal carattere passato.</summary>
        public string ToStr(DateTime? _Value, string _Format = "")
        {
            if (_Value.HasValue) return ToStr((DateTime)_Value, _Format);
            else return "";
        }

        /// <summary>Ritorna il valore stringa espresso dal carattere passato.</summary>
        public string ToStr(DateTime _Value, string _Format="")
        {
            if (_Value > DateTime.MinValue)
            {
                if (_Format == "") return _Value.ToShortDateString();
                else return _Value.ToString(_Format);
            }
            else return "";
        }

        /// <summary>Ritorna la stringa contenente il documento XML oppure 
        /// una stringa vuota se fallisce.</summary>
        public string ToStr(XmlDocument _Document)
        {
            if (_Document != null)
            {
                try
                {
                    StringWriter sw = new StringWriter();
                    XmlTextWriter xw = new XmlTextWriter(sw);
                    xw.Formatting = Formatting.Indented;
                    _Document.Save(xw);
                    return sw.ToString();
                }
                catch (Exception ex)
                {
                    Error(ex);
                    return "";
                }
            }
            else return "";
        }

        /// <summary>Ritorna il valore stringa passato. Se la stringa è null o inizia per // oppure per $$ e non si è in debug, ritorna "".
        /// *** Rev. 12-01-2023.</summary>
        public string ToStrArg(string _Value)
        {
            if (_Value == null) return "";
            else if (_Value.Trim().StartsWith("//")) return "";
            else if (_Value.Trim().StartsWith("$$"))
            {
                if (IsDebugger() && (_Value.Trim().Length > 2)) return _Value.Trim().Substring(2).Trim();
                else return "";
            }
            else return _Value.Trim();
        }

        /// <summary>Ritorna l'intero codificato con il seme passato e convertito in esadecimale.</summary>
        public string ToStrSeed(int _Value, int _Seed = INT_SEED)
        {
            return (_Value + _Seed).ToString("X");
        }

        /// <summary>Ritorna l'intero codificato con il seme passato e convertito in esadecimale.</summary>
        public int ToIntSeed(string _Value, int _Seed = INT_SEED)
        {
            return int.Parse(_Value, System.Globalization.NumberStyles.HexNumber) - _Seed;
        }

        /// <summary>Ritorna il valore Y, N o  passato oppure una stringa vuota se null.</summary>
        public string ToYesNo(string _Value)
        {
            if (Empty(_Value)) return "";
            else if (ToBool(_Value)) return "Y";
            else return "N";
        }

        /// <summary>Se la stringa passata ha una lunghezza superiore a max length viene troncata aggiungendo il suffisso al termine.</summary>
        public string TruncStr(string _Value, int _MaxLength, string _Suffix)
        {
            if (_Value != null)
            {
                _Value = _Value.Trim();
                _Suffix = _Suffix.Trim();
                if (_Value.Length > _MaxLength)
                {
                    if (_MaxLength > _Suffix.Length)
                    {
                        return _Value.Substring(0, _MaxLength - _Suffix.Length).Trim() + _Suffix;
                    }
                    else return _Value.Substring(0, _MaxLength).Trim();
                }
                return _Value;
            }
            else return "";
        }

        /// <summary>Aggiorna i campi di informazione del record. Ritorna la marcatura temporale del record.</summary>
        public void UpdateRecordInformations(DataRow _Row = null)
        {
            string nowStr;
            DateTime insertDate;
            try
            {
                if (_Row != null)
                {
                    if (_Row.Table != null)
                    {
                        nowStr = ToStr(DateTime.Now, FMT_DATE_ISO);
                        if (_Row.Table.Columns.Contains("DataInserimento"))
                        {
                            insertDate = ToDateTime(_Row["DataInserimento"]);
                            if (insertDate.Year < 1970) _Row["DataInserimento"] = ToDateTime(nowStr);
                        }
                        if (_Row.Table.Columns.Contains("UltimaModifica")) _Row["UltimaModifica"] = ToDateTime(nowStr);
                        if ((Utenti.Current != null) && _Row.Table.Columns.Contains("UltimoUtente")) _Row["UltimoUtente"] = Utenti.Current.IdUtente;
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        #endregion

        /* */

    }

    /* */

}
