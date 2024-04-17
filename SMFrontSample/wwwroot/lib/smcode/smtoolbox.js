/*
 * smtoolbox.js - Libreria di supporto.
 * Esempio di inclusione: <script type="text/javascript" src="<%= "/Privato/Moduli/SMToolBox/smtoolbox.js" %>"></script>
 */

var SM_DATE_MIN = new Date(1900, 1, 1), SM_DATE_SEP = '/';
var SM_MILLISEC_PER_DAY = 86400000;
var SM_MILLISEC_PER_HOUR = 3600000;
var SM_MILLISEC_PER_MINUTE = 60000;

// Effettua la chiamata AJAX con i parametri passati. 
// Esempio: 
// SM_Ajax("/Privato/Moduli/MioModulo/MioModulo.aspx/CaricaDati",
//          JSON.stringify({ 'IdDocumento': IdDocumento }),
//          function (jsonReply) { DoSomething(jsonReply); });
function SM_Ajax(_URL, _JSONData, _OnSuccess) {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: _JSONData,
        url: _URL,
        success: _OnSuccess
    });
}

// Concatena i valori usando il separatore passato.
function SM_Cat(_value, _valueToAdd, _separator) {
    _value = '' + _value;
    _valueToAdd = '' + _valueToAdd;
    if (_valueToAdd.trim().length > 0) {
        if (_value.length > 0) _value += _separator;
        _value += _valueToAdd;
    }
    return _value;
}

// Ritorna true se il checkbox individuato dal selettore è settato.
function SM_Checked(_selector) {
    return $(_selector).is(':checked');
}

// Ritorna la data relativa alla stringa passata o all'argomento passato. 
// Se il parametro passato è nullo ritorna la data odierna.
// Se la stringa passata (nel formato GG/MM/AAAA) non è valida ritorna 
// la data SM_DATE_MIN (01-01-1900).
function SM_Date(_Value = null) {
    if (_Value == null) return new Date(new Date().toDateString());
    else if (_Value instanceof Date) return _Value;
    else if (typeof _Value == 'number') return new Date(_Value);
    else if ((typeof _Value == 'string') || (_Value instanceof String)) {
        _Value = _Value.trim();
        if (_Value.length > 7) {
            var p = null;
            if (_Value.indexOf('/') > -1) p = _Value.split('/');
            else if (_Value.indexOf('-') > -1) p = _Value.split('-');
            if (p != null) {
                if (p.length > 2) return new Date(+p[2], p[1] - 1, +p[0]);
                else return SM_DATE_MIN;
            }
            else return SM_DATE_MIN;
        }
        else return SM_DATE_MIN;
    }
    else return SM_DATE_MIN;
}

// Ritorna il parametro passato come orario nel formato HH:MM.
// Se il valore passato è numerico, viene considerato come numero di millisecondi.
function SM_Time(_Value = null) {
    if (_Value == null) return "";
    else if (typeof _Value == 'number') {
        _Value = _Value / 60000;
        if ((_Value > -1) && (_Value < 1440)) {
            return Math.trunc(_Value / 60).toString().padStart(2, '0') + ':' + (_Value % 60).toString().padStart(2, '0');
        }
        else return "";
    }
    else if (_Value.indexOf(':') > -1) {
        var a = _Value.split(':');
        if (a != null) {
            if (a.length > 1) {
                var hh = SM_Int(a[0]), mm = SM_Int(a[1]);
                if ((hh > -1) && (hh < 24) && (mm > -1) && (mm < 60)) {
                    return hh.toString().padStart(2, '0') + ':' + mm.toString().padStart(2, '0');
                }
                else return "";
            }
            else return "";
        }
        else return "";
    }
    else return "";
}

// Ritorna il numero di millisecondi rappresentato dall'orario passato.
function SM_TimeInt(_Value = null) {
    var s = SM_Time(_Value);
    if (s.trim().length > 0) {
        return (SM_Int(s.substr(0, 2)) * 60 + SM_Int(s.substr(3, 2))) * 60000;
    }
    else return 0;
}

// Ritorna il valore della data in millisecondi da SM_DATE_MIN (01-01-1970).
function SM_DateInt(_Value = null) {
    return SM_Date(_Value).getTime();
}

// Ritorna il numero di millisecondi delle giornate specificate.
function SM_DateDayInt(_Days = 1) {
    return _Days * SM_MILLISEC_PER_DAY;
}

// Ritorna il numero di millisecondi delle ore specificate.
function SM_DateHourInt(_Hours = 1) {
    return _Hours * SM_MILLISEC_PER_HOUR;
}

// Ritorna il numero di millisecondi dei minuti specificati.
function SM_DateMinuteInt(_Minutes = 1) {
    return _Minutes * SM_MILLISEC_PER_MINUTE;
}

// Ritorna la data relativa alla stringa passata viceversa se il parametro passato
// è di tipo data ritorna la stringa di rappresentazione. Se il parametro passato
// non è valido ritorna la data SM_DATE_MIN (01-01-1900).
function SM_DateStr(_Value = null) {
    _Value = SM_Date(_Value);
    if (_Value.getTime() <= SM_DATE_MIN.getTime()) return "";
    else {
        return _Value.getDate().toString().padStart(2, '0')
            + SM_DATE_SEP + (_Value.getMonth() + 1).toString().padStart(2, '0')
            + SM_DATE_SEP + _Value.getFullYear().toString();
    }
}

// Ritorna true se il valore passato è null o una stringa vuota.
function SM_Empty(_value) {
    if (_value) return ('' + _value).trim().length < 1;
    else return true;
}

// Visualizza un errore con il messaggio passato.
function SM_Error(_message) {
    message_box.show_message('Errore', "" + _message, '/Immagini/2.gif', 'sm', 2, '');
}

// Ritorna la stringa passata con l'esplicitazione delle entità HTML
function SM_Html(_Value) {
    if (_Value) {
        _Value = '' + _Value;
        if (_Value.trim().length > 0) {
            return _Value.replace(/[\u00A0-\u9999<>\&]/g, function (i) {
                return '&#' + i.charCodeAt(0) + ';';
            });
        }
        else return _Value;
    }
    else return '';
}

// Visualizza una informazione con il messaggio passato.
function SM_Info(_message) {
    message_box.show_message('Informazione', "" + _message, '/Immagini/0.gif', 'sm', 0, '');
}

// Ritorna una stringa contenente il valore valido per l'input di dati dall'elemento specificato dal selettore.
function SM_InputVal$(_selector) {
    return SM_ValidInput($(_selector).val());
}

// Ritorna l'intero espresso dalla stringa passata.
function SM_Int(_value) {
    if (_value) {
        if (isNaN(_value)) {
            _value = ("" + _value).trim();
            if (_value.length < 1) return 0;
            else return parseInt(_value);
        }
        else return Math.trunc(_value);
    }
    else return 0;
}

// Ritorna il valore  incluso in singoli apici.
function SM_Quote(_value) {
    return "'" + ("" + _value).replaceAll("'", "\\\'") + "'";
}

// Ritorna il valore  incluso in doppi apici.
function SM_Quote2(_value) {
    return '"' + ('' + _value).replaceAll('"', '\\\"') + '"';
}

// Ritorna il testo valido dell'opzione selezionata della dropdownlist specificata dal selettore.
function SM_SelectedText$(_selector) {
    return SM_ValidInput($(_selector + " :selected").text());
}

// Ritorna il valore dell'opzione selezionata della dropdownlist specificata dal selettore.
function SM_SelectedVal(_selector) {
    var sl = $(_selector);
    if (sl && (sl.length>0)) {
        var vl = sl.val();
        if (vl) return ("" + vl).trim();
        else return "";
    }
    else return "";
}

// Imposta la visibilità dell'elemento specificato dal selettore.
function SM_SetVisibility(_selector, _visibility, _display='block') {
    
    var sl = $(_selector);
    if (sl && (sl.length > 0)) {
        if (_visibility == true) {
            sl.css('visibility', 'visible');
            sl.css('display', _display);
        }
        else {
            sl.css('visibility', 'hidden');
            sl.css('display', 'none');
        }
    }
}

// Ritorna una stringa valida per l'input di dati da form.
function SM_ValidInput(_value) {
    if (_value) return _value = ("" + _value).trim().replace("§", "");
    else return "";
}

// Visualizza un avviso con il messaggio passato.
function SM_Warning(_message) {
    message_box.show_message('Avviso', "" + _message, '/Immagini/1.gif', 'sm', 1, '');
}

// Codifica in Base64
function SM_Base64Encode(_value) {
    if (_value) {
        if (('' + _value).trim().length > 0) return window.btoa(_value);
        else return '';
    }
    else return '';
}

// Decodifica in Base64
function SM_Base64Decode(_value) {
    if (_value) {
        if (('' + _value).trim().length > 0) return window.atob(_value);
        else return '';
    }
    else return '';
}

