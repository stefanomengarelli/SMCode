/*
 * RichiestaGenerica.js - Funzioni javascript di supporto.
 * Esempio di inclusione: <script type="text/javascript" src="<%= "/Privato/Moduli/RichiestaServizio/RichiestaServizio.js" %>"></script>
 */

/* ********************************************************************************
                                     G L O B A L S
   ******************************************************************************** */

// Indice del tab correntemente selezionato.
var cw_CURRENT_TAB = 0;

// Flag della modalità di sola visualizzazione.
var cw_DISPLAY_MODE = false;

// Flag della modalità di sola visualizzazione.
var cw_DEBUG_MODE = false;

// Flag di rimozione dei tags HTML nei campi di input.
var cw_REMOVE_HTML_TAGS = false;

// Elemento che deve riavere il focus dopo l'errore.
var cw_ERROR_FOCUS = null;

// Messaggio da visualizzare nella dialog box di errore.
var cw_ERROR_MESSAGE = '';

// Messaggio contenente la lista delle descrizioni dei campi che hanno generato l'errore.
var cw_ERROR_FIELDLIST = '';

// Flag fase di convalida del form.
var cw_ERROR_VALIDATING = false;

// Ultimo controllo da cui si è usciti (focus leaved).
var cw_LEAVED_CTRL = null;

// Separatori migliaia e decimali.
var cw_DECIMAL_SEPARATOR = ',', cw_THOUSANDS_SEPARATOR = '.';
if ((1.2).toLocaleString('it-IT').indexOf('.') > -1) {
    cw_DECIMAL_SEPARATOR = '.';
    cw_THOUSANDS_SEPARATOR = ',';
}

// Array di puntatori alle istanze delle tabelle.
var cw_ARRAY_DETAILS_TABLES = [];

// Array di puntatori dei controlli per i dettagli.
var cw_ARRAY_DETAILS_ID = [];

// Array di campi dei controlli per i dettagli.
var cw_ARRAY_DETAILS_FIELDS = [];

// Array di tipo dei controlli per i dettagli.
var cw_ARRAY_DETAILS_TYPE = [];

// JQuery-steps settings.
var cw_STEPS_SETTINGS = {

    /* Appearance */
    headerTag: "h3", /* Original -> headerTag: "h1", */
    bodyTag: "section", /* Original -> bodyTag: "div",*/
    contentContainerTag: "div",
    actionContainerTag: "div",
    stepsContainerTag: "div",
    cssClass: "wizard", /* Original -> cssClass: "wizard", // optional "tabcontrol" */
    stepsOrientation: $.fn.steps.stepsOrientation.horizontal,

    /* Templates */
    titleTemplate: '<span style="font-size:11pt">#title#</span>', /* Original -> titleTemplate: '<span class="number">#index#.</span> #title#',*/
    loadingTemplate: '<span class="spinner"></span> #text#',

    /* Behaviour */
    autoFocus: true, /* Original -> autoFocus: false, */
    enableAllSteps: true, /* Original -> enableAllSteps: false, */
    enableKeyNavigation: true,
    enablePagination: false, /* Original -> enablePagination: true, */
    suppressPaginationOnFocus: true,
    enableContentCache: true,
    enableCancelButton: true,
    enableFinishButton: false, /* Original -> enableFinishButton: true, */
    preloadContent: false,
    showFinishButtonAlways: false,
    forceMoveForward: false,
    saveState: false,
    startIndex: 0,

    /* Transition Effects */
    transitionEffect: "slideLeft", /* Original -> transitionEffect: $.fn.steps.transitionEffect.none, */
    transitionEffectSpeed: 200,

    onInit: function (event, currentIndex, newIndex) {
        // Remove hasDatepicker class from initialized elements and remove the button next to datepicker input    
        $('.hasDatepicker').removeClass('hasDatepicker').siblings('.ui-datepicker-trigger').remove();
        // reinitialize datepicker, offcourse replace .datepickerfields with your specific datepicker identifier     
        $('.date').datepicker();
    },

    /* Events */
    onStepChanging: function (event, currentIndex, newIndex)
    {
        return true;
    },
    onStepChanged: function (event, currentIndex, priorIndex)
    {
        cw_CURRENT_TAB = currentIndex;
        abilitaSalva(currentIndex >= cw_TABSET_COUNTER - 1);
        return true;
    },
    onCanceled: function (event) { },
    onFinishing: function (event, currentIndex) { return true; },
    onFinished: function (event, currentIndex) { return true; },

    /* Labels */
    labels: {
        cancel: "Cancel",
        current: "current step:",
        pagination: "Pagination",
        finish: "Finish",
        next: "Next",
        previous: "Previous",
        loading: "Loading ..."
    }
};

// Contatore pagine tabset.
var cw_TABSET_COUNTER = 0;

/* ********************************************************************************
                            Q U I C K   F U N C T I O N S
   ******************************************************************************** */

// Valuta l'espressione _Test passata e se risulta falsa, visualizza nell'elemento 
// corrispondente al selettore specificato(vedi cw_GetObj) il messaggio di errore.
// Ritorna il risultato del test.
function _AUDIT(_Sel, _Test, _MsgIfFalse) {
    var elem = cw_GetElem(_Sel);
    if (elem) {
        if (elem.length > 0) {
            var id = elem.attr('id'), errElem;
            id = id.substr(id.indexOf('_'));
            errElem = $('#err' + id);
            if (errElem) {
                if (errElem.length > 0) {
                    if (_Test == false) {
                        errElem.html(_MsgIfFalse);
                    }
                    cw_ValidateError(errElem, _Test);
                }
            }
        }
    }
    return _Test;
}

// Valuta l'espressione _Test passata e se false visualizza la dialog box con il 
// messaggio di errore. Ritorna il risultato del test.
function _AUDIT_ERROR(_Test, _ErrorMsg) {
    if (_Test == false) {
        if (cw_ERROR_VALIDATING == true) cw_ERROR_MESSAGE = _ErrorMsg;
        else SM_Error(_ErrorMsg);
    }
    return _Test;
}

// Ritorna il risultato del test e mette nella lista dei campi in errore il testo passato.
function _NOTEMPTY(_Sel, _Caption = null) {
  
    if (cw_Empty(_Sel)) {
        if (_Caption == null) {
            _Caption = '';
            var elem = cw_GetElem(_Sel);
            if (elem) {
                var id = elem.attr('id'), errElem;
                id = id.substr(id.indexOf('_'));
                errElem = $('#lbl' + id);
                if (errElem) _Caption = errElem.html();
            }
        }
        cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        return false;
    }
    else return true;
}

function _NOTCHECKED(_Sel, _Checked = null) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) {
        if (_Checked != null) elem.prop('checked', _Checked == true);
        if (!(elem.is(':checked'))) {
            var id = elem.attr('id'), errElem;
            id = id.substr(id.indexOf('_'));
            errElem = $('#lbl' + id);
            if (errElem) _Caption = errElem.html();
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
        return elem.is(':checked');
    }
    else return false;
}

function _NOTYNCHECKED(_Sel, _Checked = null) {
    if (cw_GetYN(_Sel).trim().length<1) {
        var elem = cw_GetElem(_Sel);
        var id = elem.attr('id'), errElem;
        id = id.substr(id.indexOf('_'));
        if (id.endsWith('_Y') || id.endsWith('_N')) id = id.substr(0, id.length - 2);
        errElem = $('#lbl' + id);
        if (errElem) _Caption = errElem.html();
        cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        return false;
    }
    else return true;
}

// Ritorna il nominativo dell'autore
function _AUT() {
    return cw_Autore;
}

// Ritorna true se gli allegati relativi alla sezione passata raggiungono il numero minimo previsto _MinFiles.
function _ATTACH(_Sezione = 1, _MinFiles = 1) {
    return cw_ValidateAllegati(_Sezione, _MinFiles);
}

// Ritorna il codice fiscale dell'autore
function _CFAUT() {
    return cw_CodiceFiscaleAutore;
}

// Ritorna il sesso dell'autore
function _SEXAUT() {
    if (cw_CodiceFiscaleAutore) {
        if (cw_CodiceFiscaleAutore.length > 15) {
            if (cw_CodiceFiscaleAutore.substr(9, 1) > '3') return 'F';
            else return 'M';
        }
        else return '';
    }
    else return '';
}

// Ritorna una stringa rappresentante il valore della descrizione selezionata
// della combo relativa al selettore passato(vedi: cw_GetCombo, cw_GetObj).
function _COMBO(_Sel) {
    return cw_GetCombo(_Sel);
}

// Ritorna una stringa contenente il valore dell'attributo OPTION_DATA dell'elemento
// selezionato nella combo relativa identificata dal selettore passato.
function _COMBODATA(_Sel, _Pos = -1, _Sep = '|') {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) {
        var so = elem.find(":selected");
        if (so && so.length) {
            var r = so.attr('option_data');
            if (r === undefined) r = '';
            else r = '' + r;
            if (_Pos < 0) return r;
            else {
                var a = (r + _Sep).split(_Sep);
                return a[_Pos];
            }
        }
        else return '';
    }
    else return '';
}

// Reimposta le opzioni della combo identificata dal selettore passato 
// con gli elementi dell'array che deve essere nella forma [<value1>,<text1>...<valueN>,<textN>]
function _COMBOSET(_Sel, _OptionsArray) {
    var elem = cw_GetElem(_Sel), i;
    if (elem && elem.length) {
        elem.empty();
        i = 0;
        while (i < _OptionsArray.length - 1) {
            elem.append($("<option></option>").attr("value", _OptionsArray[i]).text(_OptionsArray[i + 1]));
            i += 2;
        }
    }
    return true;
}

// Ritorna la data relativa alla stringa passata o all'argomento passato. 
// Se il parametro passato è nullo ritorna la data odierna.
// Se la stringa passata non è valida ritorna la data SM_DATE_MIN.
function _DATE(_Value = null) {
    return SM_Date(_IFSEL(_Value));
}

// Ritorna il valore della data in millisecondi dal 01-01-1970.
function _DATEINT(_Value = null) {
    return SM_DateInt(_IFSEL(_Value));
}

// Ritorna il valore dei giorni passati in millisecondi.
function _DAYSINT(_Value = null) {
    return SM_DateDayInt(_IFSEL(_Value));
}

// Ritorna il valore delle ore passate in millisecondi.
function _HOURSINT(_Value = null) {
    return SM_DateHourInt(_IFSEL(_Value));
}

// Ritorna il valore dei minuti passati in millisecondi.
function _MINUTESINT(_Value = null) {
    return SM_DateMinuteInt(_IFSEL(_Value));
}

// Ritorna la data relativa alla stringa passata viceversa se il parametro passato
// è di tipo data ritorna la stringa di rappresentazione. Se il parametro passato
// non è valido ritorna una stringa vuota.
function _DATESTR(_Value = null) {
    return SM_DateStr(_IFSEL(_Value));
}

// Ritorna il numero di giorni compresi tra le date specificate (estremi compresi.)
// Oppure 0 se la seconda data è inferiore alla prima o le date non sono valide.
function _DATEDAYS(_SelDalGiorno, _SelAlGiorno) {
    var r = 0, d1 = _DATE(_SelDalGiorno), d2 = _DATE(_SelAlGiorno), ms = d2.getTime() - d1.getTime();
    if ((d1.getFullYear() > 1999) && (d2.getFullYear() > 1999)) {
        if (ms >= 0) r = 1 + (ms / 1000 / 3600 / 24);
    }
    return r;
}

// Ritorna true se l'ambiente è in modalità di debug
function _DEBUG() {
    if (cw_DEBUG_MODE == true) return true;
    else return false;
}

// Ritorna il numero delle righe effettive presenti nella tabella del dettaglio indicato.
function _DETAILROWS(_DetailIndex) {
    return cw_DetailRows(_DetailIndex);
}

// Ritorna il nominativo del dirigente
function _DIR() {
    return cw_Dirigente;
}

// Ritorna true se il modulo è in modalità visualizzazione.
function _DISPLAY() {
    if (Visualizzazione == true) return true;
    else return false;
}

// Ritorna true se il modulo è in modalità di modifica.
function _EDIT() {
    if ((Visualizzazione == false) && (Inserimento == false)) return true;
    else return false;
}

// Ritorna il risultato del test e mette nella lista dei campi in errore il testo passato.
function _ERRLST(_Test, _Caption) {
    if (_Test == false) {
        cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        return false;
    }
    else return true;
}

// Esegue il metodo di GiustoProvider specificato e ne ritorna il risultato.
function _FN_EXEC(_MetodoGiustoProvider, _Parameters = "", _OnSuccess = null, _OnFail = null) {
    var pageUrl = '/Privato/Moduli/RichiestaGenerica/RichiestaGenericaAjax.ashx';
    var parameter = {
        'Call': _MetodoGiustoProvider, 'Parms': _Parameters, 'IdDocumento': IdDocumento, 'IdCodiceDocumento': cw_IdCodiceDocumento,
        'MatricolaAutore': cw_MatricolaAutore, 'CodiceFiscaleAutore': cw_CodiceFiscaleAutore
    };
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        url: pageUrl,
        data: parameter,
        success: function (returnValue) {
            if (_OnSuccess != null) _OnSuccess(returnValue);
        },
        failure: function (data, success, errorValue) {
            if (_OnFail != null) _OnFail(errorValue);
        }
    });
}

// Ritorna true se il modulo è in modalità di inserimento.
function _INSERT() {
    if (Inserimento == true) return true;
    else return false;
}

// Ritorna true se l'elemento corrispondente al selettore specificato (vedi: cw_Empty, cw_GetObj) è vuoto.
function _EMPTY(_Sel) {
    return cw_Empty(_Sel);
}

// Ritorna 1 se il codice fiscale passato ha il carattere di controllo corretto,
// -1 se errato o 0 se il codice fiscale è vuoto.
function _CHECKCF(_CF) {
    return cw_CheckCodiceFiscale(_CF);
}

// Ritorna true se la stringa passata rappresenta una e-mail valida. 
// E' possibile anche indicare se l'e-mail può essere vuota.
function _CHECKEMAIL(_Email, _CanBeEmpty = true) {
    var fmt = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (('' + _Email).trim().length < 1) return _CanBeEmpty;
    else if (_Email.match(fmt)) return true;
    else return false;
}

// Ritorna true se la stringa passata rappresenta una numero telefonico valido. 
// E' possibile anche indicare se il numero telefonico può essere vuoto.
function _CHECKPHONE(_PhoneNumber, _CanBeEmpty = true) {
    var fmt = /^\+?\d[0-9 .]{7,12}\d$/;
    if (('' + _PhoneNumber).trim().length < 1) return _CanBeEmpty;
    else if (_PhoneNumber.match(fmt)) return true;
    else return false;
}

// Ritorna true se l'elemento corrispondente al selettore specificato (vedi: cw_Checked, cw_GetObj) è marcato.
// Se l'id inizia per # viene restituito il valore del campo con l'id domanda
// altrimenti viene restituito il campo con l'attributo cw-field uguale al valore passato.
function _CHECK(_Sel) {
    return cw_Checked(_Sel);
}

// Ritorna il numero di checkboxes impostati a true tra quelli individuati dai selettori.
function _CHECKCOUNT(_Sel1 = null, _Sel2 = null, _Sel3 = null, _Sel4 = null, _Sel5 = null, _Sel6 = null, _Sel7 = null, _Sel8 = null,
    _Sel9 = null, _Sel10 = null, _Sel11 = null, _Sel12 = null, _Sel13 = null, _Sel14 = null, _Sel15 = null, _Sel16 = null) {
    var r = 0;
    if (_Sel1 != null) if (cw_Checked(_Sel1)) r++;
    if (_Sel2 != null) if (cw_Checked(_Sel2)) r++;
    if (_Sel3 != null) if (cw_Checked(_Sel3)) r++;
    if (_Sel4 != null) if (cw_Checked(_Sel4)) r++;
    if (_Sel5 != null) if (cw_Checked(_Sel5)) r++;
    if (_Sel6 != null) if (cw_Checked(_Sel6)) r++;
    if (_Sel7 != null) if (cw_Checked(_Sel7)) r++;
    if (_Sel8 != null) if (cw_Checked(_Sel8)) r++;
    if (_Sel9 != null) if (cw_Checked(_Sel9)) r++;
    if (_Sel10 != null) if (cw_Checked(_Sel10)) r++;
    if (_Sel11 != null) if (cw_Checked(_Sel11)) r++;
    if (_Sel12 != null) if (cw_Checked(_Sel12)) r++;
    if (_Sel13 != null) if (cw_Checked(_Sel13)) r++;
    if (_Sel14 != null) if (cw_Checked(_Sel14)) r++;
    if (_Sel15 != null) if (cw_Checked(_Sel15)) r++;
    if (_Sel16 != null) if (cw_Checked(_Sel16)) r++;
    return r;
}

// Resetta i checkbox corrispondenti ai selettori specificati.
function _CHECKRESET(_Sel1 = null, _Sel2 = null, _Sel3 = null, _Sel4 = null, _Sel5 = null, _Sel6 = null, _Sel7 = null, _Sel8 = null,
    _Sel9 = null, _Sel10 = null, _Sel11 = null, _Sel12 = null, _Sel13 = null, _Sel14 = null, _Sel15 = null, _Sel16 = null) {
    if (_Sel1 != null) cw_Checked(_Sel1, false);
    if (_Sel2 != null) cw_Checked(_Sel2, false);
    if (_Sel3 != null) cw_Checked(_Sel3, false);
    if (_Sel4 != null) cw_Checked(_Sel4, false);
    if (_Sel5 != null) cw_Checked(_Sel5, false);
    if (_Sel6 != null) cw_Checked(_Sel6, false);
    if (_Sel7 != null) cw_Checked(_Sel7, false);
    if (_Sel8 != null) cw_Checked(_Sel8, false);
    if (_Sel9 != null) cw_Checked(_Sel9, false);
    if (_Sel10 != null) cw_Checked(_Sel10, false);
    if (_Sel11 != null) cw_Checked(_Sel11, false);
    if (_Sel12 != null) cw_Checked(_Sel12, false);
    if (_Sel13 != null) cw_Checked(_Sel13, false);
    if (_Sel14 != null) cw_Checked(_Sel14, false);
    if (_Sel15 != null) cw_Checked(_Sel15, false);
    if (_Sel16 != null) cw_Checked(_Sel16, false);
    return r;
}

// Resetta i checkbox corrispondenti ai selettori specificati se il primo è marcato.
function _CHECKRADIO(_Sel, _Sel1 = null, _Sel2 = null, _Sel3 = null, _Sel4 = null, _Sel5 = null, _Sel6 = null, _Sel7 = null, _Sel8 = null,
    _Sel9 = null, _Sel10 = null, _Sel11 = null, _Sel12 = null, _Sel13 = null, _Sel14 = null, _Sel15 = null, _Sel16 = null) {
    if (cw_Checked(_Sel)) {
        if (_Sel1 != null) cw_Checked(_Sel1, false);
        if (_Sel2 != null) cw_Checked(_Sel2, false);
        if (_Sel3 != null) cw_Checked(_Sel3, false);
        if (_Sel4 != null) cw_Checked(_Sel4, false);
        if (_Sel5 != null) cw_Checked(_Sel5, false);
        if (_Sel6 != null) cw_Checked(_Sel6, false);
        if (_Sel7 != null) cw_Checked(_Sel7, false);
        if (_Sel8 != null) cw_Checked(_Sel8, false);
        if (_Sel9 != null) cw_Checked(_Sel9, false);
        if (_Sel10 != null) cw_Checked(_Sel10, false);
        if (_Sel11 != null) cw_Checked(_Sel11, false);
        if (_Sel12 != null) cw_Checked(_Sel12, false);
        if (_Sel13 != null) cw_Checked(_Sel13, false);
        if (_Sel14 != null) cw_Checked(_Sel14, false);
        if (_Sel15 != null) cw_Checked(_Sel15, false);
        if (_Sel16 != null) cw_Checked(_Sel16, false);
    }
    return r;
}

// Ritorna l'id del codice documento corrente.
function _IDCODDOC() {
    return cw_IdCodiceDocumento;
}

// Ritorna l'id del documento corrente.
function _IDDOC() {
    return IdDocumento;
}

// Se il valore passato rappresenta un selettore viene restituito il contenuto 
// dell'elemento individuato, altrimenti viene restituito il semplice valore passato.
function _IFSEL(_Value) {
    if (_Value == null) return null;
    else if ((typeof _Value == 'string') || (_Value instanceof String)) {
        var s = _Value.trim();
        if (s.startsWith('@') || s.startsWith('!') || s.startsWith('?')
            || s.startsWith('#') || s.startsWith('.') || s.startsWith('[')) {
            return cw_GetStr(s);
        }
        else return _Value;
    }
    else return _Value;
}

// Ritorna il valore _IfTrue se l'espressione _Test risulta true, altrimenti ritorna il valore _IfFalse.
function _IIF(_Test, _IfTrue, _IfFalse) {
    if (_Test == true) return _IfTrue;
    else return _IfFalse;
}

// Compara il valore _Value con gli elementi contenuti nell'array _StrArray, comparandoli
// con il metodo indicato da _Matchmode (0=uguali, 1=inizia per, 2=termina per, 3=contiene).
// Se il valore corrisponde ad un elemento dell'array viene restituito l'elemento dell'array
// dei risultati _ResultArray con indice uguale, altrimenti viene restituito _DefaultValue.
function _IIFF(_Value, _StrArray, _ResultArray, _DefaultValue, _MatchMode = 0) {
    var i = 0, q = -1;
    while ((q < 0) && (i < _StrArray.length)) {
        if (_MatchMode == 1) {
            if (_StrArray[i].startsWith(_Value)) q = i;
        }
        else if (_MatchMode == 2) {
            if (_StrArray[i].endsWith(_Value)) q = i;
        }
        else if (_MatchMode == 3) {
            if (_StrArray[i].indexOf(_Value) > -1) q = i;
        }
        else if (_StrArray[i] == _Value) q = i;
    }
    if (q < 0) return _DefaultValue;
    else return _ResultArray[q];
}

// Ritorna un intero rappresentante il valore del campo relativo al selettore passato (vedi: cw_GetInt, cw_GetObj).
function _INT(_Sel) {
    return cw_GetInt(_Sel);
}

// Ritorna la matricola dell'autore.
function _MATAUT() {
    return cw_MatricolaAutore;
}

// Ritorna la matricola del dirigente.
function _MATDIR() {
    return cw_MatricolaDirigente;
}
 
// Ritorna la stringa passata senza tags HTML.
function _NOTAG(_Value) {
    return cw_RemoveTags(_Value);
}

// Funzione che restituisce il valore _Value convertito in una stringa (vedi: cw_FormatValue).
function _OUT(_Value, _Format = '') {
    return cw_FormatValue(_Value, _Format);
}

// Imposta il valore del campo relativo al selettore passato, formattandolo se previsto (vedi: cw_SetElem, cw_GetObj).
function _SET(_Sel, _Value) {
    cw_SetElem(_Sel, _Value);
}

// Imposta il contenuto HTML del controllo relativo al selettore passato.
function _SETHTML(_Sel, _Html) {
    cw_SetHtml(_Sel, _Html);
}

// Ritorna true se il documento corrente è in uno dei passi specificati.
function _PASSO(_Passo1 = null, _Passo2 = null, _Passo3 = null, _Passo4 = null, _Passo5 = null, _Passo6 = null, _Passo7 = null, _Passo8 = null) {
    var p = SM_Int(cw_PassoDocumento);
    if (_Passo1 == null) return p;
    else if (_Passo1 == p) return true;
    else if ((_Passo2 != null) && (_Passo2 == p)) return true;
    else if ((_Passo3 != null) && (_Passo3 == p)) return true;
    else if ((_Passo4 != null) && (_Passo4 == p)) return true;
    else if ((_Passo5 != null) && (_Passo5 == p)) return true;
    else if ((_Passo6 != null) && (_Passo6 == p)) return true;
    else if ((_Passo7 != null) && (_Passo7 == p)) return true;
    else if ((_Passo8 != null) && (_Passo8 == p)) return true;
    else return false;
}

// Ritorna true il valore percentuale rispetto al totale.
function _PERC(_Value, _Total) {
    if (_Total != 0) return _Value * 100 / _Total;
    else return 0;
}

// Ritorna true se all'utente corrente è assegnato uno dei ruoli specificati.
function _RUOLO(_Ruolo1, _Ruolo2 = null, _Ruolo3 = null, _Ruolo4 = null, _Ruolo5 = null, _Ruolo6 = null, _Ruolo7 = null, _Ruolo8 = null) {
    if (cw_RuoliUtente.indexOf('|' + _Ruolo1 + '|') > -1) return true;
    else if ((_Ruolo2 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo2 + '|') > -1)) return true;
    else if ((_Ruolo3 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo3 + '|') > -1)) return true;
    else if ((_Ruolo4 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo4 + '|') > -1)) return true;
    else if ((_Ruolo5 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo5 + '|') > -1)) return true;
    else if ((_Ruolo6 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo6 + '|') > -1)) return true;
    else if ((_Ruolo7 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo7 + '|') > -1)) return true;
    else if ((_Ruolo8 != null) && (cw_RuoliUtente.indexOf('|' + _Ruolo8 + '|') > -1)) return true;
    else return false;
}

// Attiva il bottone "Salva dati" se il test ritorna un valore true. Ritorna il valore del test.
function _SALVADATI(_Test) {
    if (_Test == true) {
        $(".cwAggiornaDatiModuloBtn").removeClass('hidden');
        return true;
    }
    else {
        $(".cwAggiornaDatiModuloBtn").addClass('hidden');
        return false;
    }
}

// Ritorna una stringa rappresentante il valore del campo relativo al selettore passato (vedi: cw_GetStr, cw_GetObj).
function _STR(_Sel) {
    return cw_GetStr(_Sel);
}

// Somma gli elementi con i selettori passati (max 16)
function _SUM(_Sel0, _Sel1, _Sel2, _Sel3, _Sel4, _Sel5, _Sel6, _Sel7,
    _Sel8, _Sel9, _Sel10, _Sel11, _Sel12, _Sel13, _Sel14, _Sel15,
    _Sel16, _Sel17, _Sel18, _Sel19, _Sel20, _Sel21, _Sel22, _Sel23) {
    var r = 0;
    if (_Sel0 != undefined) r += cw_GetVal(_Sel0);
    if (_Sel1 != undefined) r += cw_GetVal(_Sel1);
    if (_Sel2 != undefined) r += cw_GetVal(_Sel2);
    if (_Sel3 != undefined) r += cw_GetVal(_Sel3);
    if (_Sel4 != undefined) r += cw_GetVal(_Sel4);
    if (_Sel5 != undefined) r += cw_GetVal(_Sel5);
    if (_Sel6 != undefined) r += cw_GetVal(_Sel6);
    if (_Sel7 != undefined) r += cw_GetVal(_Sel7);
    if (_Sel8 != undefined) r += cw_GetVal(_Sel8);
    if (_Sel9 != undefined) r += cw_GetVal(_Sel9);
    if (_Sel10 != undefined) r += cw_GetVal(_Sel10);
    if (_Sel11 != undefined) r += cw_GetVal(_Sel11);
    if (_Sel12 != undefined) r += cw_GetVal(_Sel12);
    if (_Sel13 != undefined) r += cw_GetVal(_Sel13);
    if (_Sel14 != undefined) r += cw_GetVal(_Sel14);
    if (_Sel15 != undefined) r += cw_GetVal(_Sel15);
    if (_Sel16 != undefined) r += cw_GetVal(_Sel16);
    if (_Sel17 != undefined) r += cw_GetVal(_Sel17);
    if (_Sel18 != undefined) r += cw_GetVal(_Sel18);
    if (_Sel19 != undefined) r += cw_GetVal(_Sel19);
    if (_Sel20 != undefined) r += cw_GetVal(_Sel20);
    if (_Sel21 != undefined) r += cw_GetVal(_Sel21);
    if (_Sel22 != undefined) r += cw_GetVal(_Sel22);
    if (_Sel23 != undefined) r += cw_GetVal(_Sel23);
    return r;
}

// Ritorna la somma degli elementi del dettaglio con l'indice specificato 
// e corrispondenti alla colonna specificata.
function _SUMDETAIL(_DetailIndex, _ColumnIndex, _Count = false) {
    var r = 0;
    $('#tbDetails' + _DetailIndex + ' > tbody > tr').each(function (index, value) {
        var h = '' + $('td:eq(' + _ColumnIndex + ')', this).html().trim().toLowerCase();
        if (_Count === true) {
            if ('sty123456789'.indexOf((h + ' ').substr(0, 1)) > -1) r += 1;
        }
        else r += cw_StrVal(h);
    });
    return r;
}

// Esegue la funzione _ScanFunction(rowIndex, colsArray) che riceve come parametri l'indice della riga
// e l'array dei contenuti delle colonne, per tutte le righe della tabella di dettaglio
// con l'indice passato.
function _SCANDETAIL(_DetailIndex, _ColumnsCount, _ScanFunction) {
    var r = 0;
    $('#tbDetails' + _DetailIndex + ' > tbody > tr').each(function (index, value) {
        var a = [], i;
        for (i = 0; i < _ColumnsCount; i++) a[i] = '' + $('td:eq(' + i + ')', this).html().trim().toLowerCase();
        _ScanFunction(_DetailIndex, r, a);
        r++;
    });
    return r;
}

// Ritorna la descrizione della struttura
function _STRUT() {
    return cw_Struttura;
}

// Ritorna in formato orario HH:MM la durata
// dell'intervallo tra i due estremi orari.
function _TIMEELAPSED(_Start, _End) {
    var a = SM_TimeInt(_Start), b = SM_TimeInt(_End);
    if (b > a) return SM_Time(b - a);
    else return "";
}

// Ritorna un float rappresentante il valore del campo relativo al selettore passato.
function _VAL(_Sel) {
    return cw_GetVal(_Sel);
}

// Ritorna true se l'elemento identificato dal selettore è visibile.
function _VISIBLE(_Sel, _SetVisible = null) {
    if (_SetVisible === true) cw_Visible(_Sel, true);
    else if (_SetVisible === false) cw_Visible(_Sel, false);
    return cw_IsVisible(_Sel);
}

// Ritorna il valore (Y,N o stringa vuota) del controllo YES/NO relativo al selettore passato.
function _YESNO(_Sel) {
    return cw_GetYN(_Sel);
}

/* ********************************************************************************
                                     M E T H O D S
   ******************************************************************************** */

// Codifica in Base64
function cw_Base64Encode(_value) {
    if (_value) {
        if (('' + _value).trim().length > 0) return window.btoa(_value);
        else return '';
    }
    else return '';
}

// Decodifica in Base64
function cw_Base64Decode(_value) {
    if (_value) {
        if (('' + _value).trim().length > 0) return window.atob(_value);
        else return '';
    }
    else return '';
}

// Ritorna 1 se il codice fiscale passato ha il carattere di controllo corretto,
// -1 se errato o 0 se il codice fiscale è vuoto.
function cw_CheckCodiceFiscale(_CodFisc) {
    var i, s,
        cifre = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
        set1 = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ',
        set2 = 'ABCDEFGHIJABCDEFGHIJKLMNOPQRSTUVWXYZ',
        pari = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ',
        dispari = 'BAKPLCQDREVOSFTGUHMINJWZYX';
    _CodFisc = _CodFisc.trim().toUpperCase();
    if (_CodFisc.length < 1) return 0;
    else if (_CodFisc.length != 16) return -1;
    {
        for (i = 0; i < 16; i++) {
            if (cifre.indexOf(_CodFisc.charAt(i)) < 0) return -1;
        }
        s = 0;
        for (i = 1; i <= 13; i += 2) {
            s += pari.indexOf(set2.charAt(set1.indexOf(_CodFisc.charAt(i))));
        }
        for (i = 0; i <= 14; i += 2) {
            s += dispari.indexOf(set2.charAt(set1.indexOf(_CodFisc.charAt(i))));
        }
        if (s % 26 != _CodFisc.charCodeAt(15) - 'A'.charCodeAt(0)) return -1;
        else return 1;
    }
}

// Ritorna true se l'elemento corrispondente al selettore specificato (vedi cw_GetObj) è marcato.
// Se l'id inizia per # viene restituito il valore del campo con l'id domanda
// altrimenti viene restituito il campo con l'attributo cw-field uguale al valore passato.
// Se il parametro _Checked viene specificato l'elemento verrà marcato o meno a seconda di tale valore.
function cw_Checked(_Sel, _Checked = null) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) {
        if (_Checked != null) elem.prop('checked', _Checked == true);
        return elem.is(':checked');
    }
    else return false;
}

// Ritorna true se l'elemento corrispondente al selettore specificato (vedi cw_GetObj) è vuoto.
function cw_Empty(_Sel) {
    return cw_GetStr(_Sel).trim().length < 1;
}

// Imposta l'abilitazione dell'elemento individuato dal selettore (vedi cw_GetElem).
function cw_Enable(_Sel, _Enabled) {
    var elem = cw_GetElem(_Sel), lbl = null, tipo, id;
    if (elem && elem.length) {
        tipo = elem.attr('cw-type');
        id = '#' + elem.attr('id') + ' ';
        if (_Enabled == true) {
            //
            $(id + '.cw-input-control').attr('disabled', false);
            $(id + '.cw-input-control').removeClass('disabled');
            $(id + '.cw-input-control').prop('readonly', false);
            $(id + '.cw-anchor-control').removeClass("cw-anchor-disabled");
            //
            if (tipo == 'TAB') {
                $(id + "label.btn").removeClass("disabled");
                $(id + "input[data-switch]").bootstrapSwitch('disabled', false, false);
                $(id + "select").prop("disabled", false);
                $(id + ".form-control").prop('disabled', false);
                $(id + ".dRimuovi").show();
                $(id + ".giorni").attr('disabled', false);
                $(id + ".edit_tool_panel").show();
                $(id + ".elemento-compilazione").attr('disabled', false);
            }
            else {
                elem.prop('readonly', false);
                elem.prop('disabled', false);
                elem.removeClass('disabled');
            }
        }
        else {
            //
            $(id + '.cw-input-control').attr('disabled', true);
            $(id + '.cw-input-control').addClass('disabled');
            $(id + '.cw-input-control').prop('readonly', true);
            $(id + '.cw-anchor-control').addClass("cw-anchor-disabled");
            //
            if (tipo == 'FIELDSET') {
                elem.prop('disabled', true);
                elem.addClass('disabled');
            }
            else if (tipo == 'TAB') {
                var id = '#' + elem.attr('id') + ' ';
                $(id + "label.btn").addClass("disabled");
                $(id + "input[data-switch]").bootstrapSwitch('disabled', true, true);
                $(id + "select").prop("disabled", true);
                $(id + ".form-control").prop("disabled", true);
                $(id + ".dRimuovi").hide();
                $(id + ".giorni").attr("disabled", true);
                $(id + ".edit_tool_panel").hide();
                $(id + ".elemento-compilazione").attr("disabled", true);
            }
            else {
                elem.prop("readonly", true);
                elem.prop('disabled', true);
                elem.addClass('disabled');
            }
        }
        lbl = $('#' + ('' + elem.attr('id')).replace('qst_', 'lbl_'));
    }
    else lbl = cw_GetElem(_Sel, 'lbl_');
    if (lbl && lbl.length) {
        if (_Enabled == true) {
            lbl.prop("readonly", false);
            lbl.prop('disabled', false);
            lbl.removeClass('disabled');
        }
        else {
            lbl.prop("readonly", true);
            lbl.prop('disabled', true);
            lbl.addClass('disabled');
        }
    }
    return _Enabled == true;
}

// Formatta l'elemento con il selettore passato (vedi cw_GetObj).
function cw_Format(_Sel, _Format, _Type, _RemoveHTML = false) {
    var elem = cw_GetElem(_Sel);
    var s = elem.val();
    if (_RemoveHTML == true) s = cw_RemoveTags(s);
    _Type = _Type.toUpperCase();
    if ((_Type == 'NUM') || (_Type == 'NUMBER') || (_Type == 'NUMERO')) s = cw_StrVal(s);
    else if (_Type == 'TIME') s = SM_Time(('' + s).trim());
    elem.val(cw_FormatValue(s, _Format));
}

// Funzione che converte il valore _Value in una stringa formattata secondo il parametro 
// _Format che può assumere i valori: EUR, EURNZ, NZ, INT, INTNZ, UPPER, LOWER.
function cw_FormatValue(_Value, _Format = '') {
    _Format = _Format.toUpperCase();
    if ((typeof _Value == 'number') || ('|EUR|EURNZ|NZ|INT|INTNZ|'.indexOf('|' + _Format + '|') > -1)) {
        if (typeof _Value != 'number') _Value = cw_StrVal(_Value);
        if (_Format == 'EUR') return cw_FormatEuro(_Value);
        else if (_Format == 'EURNZ') {
            if (_Value == 0) return '';
            else return cw_FormatEuro(_Value);
        }
        else if (_Format == 'NZ') {
            if (_Value == 0) return '';
            else return _Value.toLocaleString('it-IT');
        }
        else if (_Format == 'INT') return Math.trunc(_Value).toString();
        else if (_Format == 'INTNZ') {
            _Value = Math.trunc(_Value);
            if (_Value == 0) return '';
            else return _Value.toString();
        }
        else if (_Format.startsWith('DNZ')) {
            if (_Value == 0) return '';
            else return (0 + _Value).toLocaleString('it-IT', { minimumFractionDigits: parseInt(_Format.substr(3)) });
        }
        else if (_Format.startsWith('D')) {
            return (0 + _Value).toLocaleString('it-IT', { minimumFractionDigits: parseInt(_Format.substr(1)) });
        }
        else return _Value.toLocaleString('it-IT');
    }
    else if (_Format == 'UPPER') return ('' + _Value).toUpperCase();
    else if (_Format == 'LOWER') return ('' + _Value).toLowerCase();
    else return '' + _Value;
}

// Ritorna il valore passato come stringa formattata in EURO
function cw_FormatEuro(_Value) {
    if (isNaN(_Value)) return '';
    else return (0 + _Value).toLocaleString('it-IT', { minimumFractionDigits: 2 });
}

// Ritorna una stringa rappresentante il valore della descrizione selezionata
// della combo relativa al selettore passato(vedi: cw_GetObj).
function cw_GetCombo(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem) return $('#' + elem.attr('id') +' option:selected').text();
    else return "";
}

// Ritorna l'oggetto JQuery rappresentante l'elemento relativo al campo con il
// selettore specificato. Se il selettore inizia per #,[ oppure . si intende un 
// selettore puro JQuery. Se inizia per ! o ? si intende relativo al campo associato
// alla domanda con id uguale al numero che segue.
// Se inizia per @ si intende relativo al campo a cui è stato associato l'alias che segue.
// Se non inizia per i caratteri citati si intende relativo al campo a cui è
// associato il nome della colonna della tabella.
function cw_GetElem(_Sel, _Prefix = 'qst_') {
    
    var obj = null;
    if (_Sel) {
        _Sel = _Sel.trim();
        if (_Sel.length > 1) {
            if ("#[.".indexOf(_Sel.substr(0, 1)) < 0) {
                if (_Sel.startsWith("!") || _Sel.startsWith("?")) _Sel = "#" + _Prefix + _Sel.substr(1);
                else if (_Sel.startsWith("@")) _Sel = "[cw-alias='" + _Sel.substr(1) + "']";
                else _Sel = "[cw-field='" + _Sel + "']";
            }
            obj = $(_Sel);
            if (obj && obj.length) return obj;
            else if (_Sel.startsWith('#') && !_Sel.endsWith('_Y')) return cw_GetElem(_Sel + '_Y');
            else return obj;
        }
        else return null;
    }
    else return null;
}

// Ritorna il campo dell'elemento specificato dal selettore passato (vedi: cw_GetElem).
function cw_GetField(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) return elem.attr("cw-field");
    else return '';
}

// Ritorna la formattazione dell'elemento specificato dal selettore passato (vedi: cw_GetElem).
function cw_GetFormat(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) return elem.attr("cw-format");
    else return '';
}

// Ritorna una stringa rappresentante il valore del campo relativo al selettore passato (vedi: cw_GetObj).
function cw_GetStr(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) return elem.val();
    else return '';
}

// Ritorna un intero rappresentante il valore del campo relativo al selettore passato (vedi: cw_GetObj).
function cw_GetInt(_Sel) {
    var r = parseInt(cw_GetStr(_Sel));
    if (isNaN(r)) return 0;
    else return r;
}

// Ritorna il tipo dell'elemento specificato dal selettore passato (vedi: cw_GetElem).
function cw_GetType(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) return elem.attr("cw-type");
    else return '';
}

// Ritorna un float rappresentante il valore del campo relativo al selettore passato (vedi: cw_GetObj).
function cw_GetVal(_Sel) {
    var r = cw_StrVal(cw_GetStr(_Sel));
    if (isNaN(r)) return 0;
    else return r;
}

// Ritorna il valore Y, N o stringa vuota relativa al controllo YES/NO corrispondente al selettore passato.
function cw_GetYN(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) {
        var id = elem.attr('id'), idY, idN;
        if (id.endsWith('_Y')) {
            idY = id;
            idN = id.replace('_Y', '_N');
        }
        else if (id.endsWith('_N')) {
            idY = id;
            idN = id.replace('_N', '_Y');
        }
        else {
            idY = id + '_Y';
            idN = id + '_N';
        }
        if ($('#' + idY).is(':checked')) return "Y";
        else if ($('#' + idN).is(':checked')) return "N";
        else return "";
    }
    else return "";
}

// Ritorna true se l'elemento individuato dal selettore (vedi cw_GetElem) è abilitato.
function cw_IsEnabled(_Sel) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) return elem.prop('readonly') == true;
    else return false;
}

// Ritorna true se l'elemento individuato dal selettore (vedi cw_GetElem) è visibile.
function cw_IsVisible(_Sel) {
    var elem = cw_GetElem(_Sel), id;
    var r = false;
    if (elem && elem.length) {
        r = true;
        if (elem.css('display') == 'none') r = false;
        while (r && (elem && elem.length)) {
            elem = elem.parent();
            if (elem && elem.length) {
                if (elem.css('display') == 'none') r = false;
                id = elem.attr('id');
                if (id && id.length) if (id == 'dform_utente') break;
            }
        }
    }
    return r;
}

// Seleziona il tab successivo a quello corrente.
function cw_NextTab() {
    var nextTab = 1 + cw_CURRENT_TAB;
    $('#tabsModulo-t-' + nextTab.toString()).click();
}

// Evento click del controllo YES/NO.
function cw_OnClickYesNo(_Ctrl) {
    var id = _Ctrl.id;
    if ($('#' + id).is(':checked')) {
        id = id.substr(0, id.length - 2);
        if (_Ctrl.id == id + '_Y') $('#' + id + '_N').prop('checked', false);
        else if (_Ctrl.id == id + '_N') $('#' + id + '_Y').prop('checked', false);
    }
}

// Funzione di controllo e formattazione richiamata dagli eventi di tipo input blur o combo change.
function cw_OnLeaveControl(_Ctrl, _Id, _Field, _Format, _Type) {
    if (cw_LEAVED_CTRL != _Ctrl) cw_LEAVED_CTRL = _Ctrl;
    cw_Format('#' + _Id, _Format, _Type, cw_REMOVE_HTML_TAGS);
    if (((_Type == 'YES/NO') || (_Type == 'SI/NO'))
        && (!_Id.endsWith('_Y') && (!_Id.endsWith('_N')))) cw_HiddenBuff('#' + _Id + '_Y');
    else cw_HiddenBuff('#' + _Id);
    if (typeof _ONLEAVE === 'function') _ONLEAVE(_Ctrl, _Id, _Field, _Format, _Type);
    cw_RecalcForm();
}

// Seleziona il tab con l'indice passato.
function cw_SelectTab(_TabIndex) {
    $('#tabsModulo-t-' + _TabIndex.toString()).click();
}

// Imposta il valore del campo relativo al selettore passato, formattandolo se previsto (vedi: cw_GetObj).
function cw_SetElem(_Sel, _Value) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) {
        elem.val(cw_FormatValue(_Value, elem.attr('cw-format')));
        cw_HiddenBuff(_Sel);
    }
}

// Imposta il contenuto html del controllo relativo al selettore passato.
function cw_SetHtml(_Sel, _Html) {
    var elem = cw_GetElem(_Sel);
    if (elem && elem.length) elem.html(_Html);
}

// Imposta il valore dell'elemento passato nell'hidden-buffer.
function cw_HiddenBuff(_Sel) {
    var elem = cw_GetElem(_Sel), id;
    if (!(elem && elem.length) && _Sel.startsWith('#')) {
        elem = cw_GetElem(_Sel + '_Y');
    }
    if (elem && elem.length) {
        id = elem.attr('id');
        ty = elem.attr('cw-type');
        if (ty == 'CHECK') {
            if ($('#' + id).is(':checked')) $('#' + id + '_CK').val('1');
            else $('#' + id + '_CK').val('0');
        }
        else if (ty == 'COMBO') {
            $('#' + id + '_CB').val($('#' + id).val());
        }
        else if ((ty == 'YES/NO') || (ty == 'SI/NO')) {
            if (id.endsWith('_N') || id.endsWith('_Y')) {
                id = id.substr(0, id.length - 2);
            }
            if ($('#' + id + '_Y').is(':checked')) $('#' + id + '_YN').val('Y');
            else if ($('#' + id + '_N').is(':checked')) $('#' + id + '_YN').val('N');
            else $('#' + id + '_YN').val('');
        }
        else {
            $('#' + id + '_TX').val($('#' + id).val());
        }
    }
}

// Remove all HTML tags from value.
function cw_RemoveTags(_Value) {
    return ('' + _Value).replace(/<[^>]*>/g, ' ');
}

// Ritorna il valore numerico della stringa passata secondo le impostazioni di localizzazione italiane.
function cw_StrVal(_Value) {
    _Value = ('' + _Value).replaceAll(cw_THOUSANDS_SEPARATOR, '').replaceAll(cw_DECIMAL_SEPARATOR, '.');
    _Value = parseFloat(_Value);
    if (isNaN(_Value)) return 0;
    else return 0 + _Value;
}

// Ritorna un float con la somma dei campi indicati nell'array di selettori passato.
function cw_Sum(_SelArray) {
    var r = 0, i;
    if (_SelArray) {
        for (i = 0; i < _SelArray.length; i++) r += cw_GetVal(_SelArray[i]);
    }
    return r;
}

// Ritorna un intero con la somma dei campi indicati nell'array di selettori passato.
function cw_SumInt(_SelArray) {
    var r = 0, i;
    if (_SelArray) {
        for (i = 0; i < _SelArray.length; i++) r += cw_GetInt(_SelArray[i]);
    }
    return r;
}

// Convalida l'input di tipo CHECK controllandone l'obbligatorietà.
function cw_ValidateCheck(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var r = (_Required != true) || _Ctrl.is(':checked') || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Convalida l'input di tipo COMBO controllandone l'obbligatorietà.
function cw_ValidateCombo(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var s = _Ctrl.find('option:selected').text().trim();
    if (s.toLowerCase() == '(selezionare una opzione valida)') s = '';
    var r = (_Required != true) || (s.length > 0) || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Convalida l'input di tipo DATE controllandone l'obbligatorietà.
function cw_ValidateDate(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var s = ('' + _Ctrl.val()).trim();
    var r = (_Required != true) || (s.length > 0) || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    _Ctrl.val(s);
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Convalida l'input di tipo DATE controllandone l'obbligatorietà.
function cw_ValidateTime(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var s = SM_Time(('' + _Ctrl.val()).trim());
    var r = (_Required != true) || (s.length > 0) || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    _Ctrl.val(s);
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Visualizza il controllo di visualizzazione dell'errore _ErrorControl se l'espressione _AllowedValue risulta false.
function cw_ValidateError(_ErrorControl, _AllowedValue) {
    if (_AllowedValue == false) {
        _ErrorControl.css('visibility', 'visible');
        _ErrorControl.css('display', 'block');
    }
    else {
        _ErrorControl.css('visibility', 'hidden');
        _ErrorControl.css('display', 'none');
    }
}

// Convalida l'input di tipo MEMO controllandone l'obbligatorietà.
function cw_ValidateMemo(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var s = ('' + _Ctrl.val()).trim();
    var r = (_Required != true) || (s.length > 0) || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    _Ctrl.val(s);
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Convalida l'input di tipo NUMBER controllandone l'obbligatorietà.
function cw_ValidateNumber(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var s = ('' + _Ctrl.val()).trim();
    var r = (_Required != true) || (s.length > 0) || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    _Ctrl.val(s);
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Convalida l'input di tipo TEXT controllandone l'obbligatorietà.
function cw_ValidateText(_Ctrl, _Required, _Page, _ErrElem, _Caption = '') {
    var s = ('' + _Ctrl.val()).trim();
    var r = (_Required != true) || (s.length > 0) || (!cw_IsVisible('#' + _Ctrl.attr('id')));
    _Ctrl.val(s);
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _Ctrl;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Convalida l'input di tipo YES/NO controllandone l'obbligatorietà.
function cw_ValidateYesNo(_CtrlSI, _CtrlNO, _Required, _Page, _ErrElem, _Caption = '') {
    var r = (_Required != true) || _CtrlSI.is(':checked') || _CtrlNO.is(':checked') || (!cw_IsVisible('#' + _CtrlSI.attr('id')));
    cw_ValidateError(_ErrElem, r);
    if (!r) {
        if (cw_ERROR_FOCUS == null) {
            if (cw_TABSET_COUNTER > 0) cw_SelectTab(_Page);
            cw_ERROR_FOCUS = _CtrlSI;
        }
        if (_Caption.trim().length > 0) {
            cw_ERROR_FIELDLIST += '<li>' + _Caption + '</li>';
        }
    }
    return r;
}

// Ritorna il valore numerico in formato stringa secondo le impostazioni italiane.
function cw_ValStr(_Value) {
    if (isNaN(_Value)) return '';
    else return (0 + _Value).toLocaleString('it-IT');
}

// Imposta la visibilità dell'elemento individuato dal selettore (vedi cw_GetElem).
function cw_Visible(_Sel, _Visible) {
    var elem = cw_GetElem(_Sel), lbl = null, xfo = null, yn = null;
    if (elem && elem.length) {

        // elemento selezionato
        if (_Visible == true) {
            elem.css('visibility', 'visible');
            elem.css('display', 'block');
        }
        else {
            elem.css('visibility', 'hidden');
            elem.css('display', 'none');
        }

        // div associati cw-for
        xfo = $('[cw-for="' + elem.attr("id") + '"]');
        cw_JQVisible(xfo, _Visible);

        //// label associate
        //xfo = $('label[for="' + elem.attr("id") + '"]');
        //cw_JQVisible(xfo, _Visible);

        //// div associati
        //xfo = $('div[for="' + elem.attr("id") + '"]');
        //cw_JQVisible(xfo, _Visible);

        // cerca label di default
        lbl = $('#' + ('' + elem.attr('id')).replace('qst_', 'lbl_'));
    }
    else lbl = cw_GetElem(_Sel, 'lbl_');
    if (lbl && lbl.length) {
        // label di default
        if (_Visible == true) {
            lbl.css('visibility', 'visible');
            lbl.css('display', 'block');
        }
        else {
            lbl.css('visibility', 'hidden');
            lbl.css('display', 'none');
        }
        // div associati
        xfo = $('div[for="' + lbl.attr("id") + '"]');
        cw_JQVisible(xfo, _Visible);
    }
    return _Visible == true;
}

// Imposta la visibilità dell'elemento JQuery passato.
function cw_JQVisible(_JQElem, _Visible) {
    if (_JQElem && _JQElem.length) {
        if (_Visible == true) {
            _JQElem.css('visibility', 'visible');
            _JQElem.css('display', 'block');
        }
        else {
            _JQElem.css('visibility', 'hidden');
            _JQElem.css('display', 'none');
        }
    }
}

/* ********************************************************************************
                            AJAX AUTOCOMPLETION SELECT
   ******************************************************************************** */

// Inizializza il combo selezionato
// initSelectAjax($("#sResidenza"), "Seleziona il comune di residenza", ["id_codCitta", "text_nomeCitta"], formatRepo, formatRepoSelection);
function cw_InitSelectAjax(_SelectJQueryObject, _Placeholder, _DataQuery, formatRepoFunc, formatRepoSelectionFunc) {
    var combo = $(_SelectJQueryObject);
    var sel2 = combo.select2({
        theme: "bootstrap",
        language: "it",
        placeholder:cw_Base64Decode(_Placeholder),
        allowClear: true,
        //dropdownParent abbiamo bisogno del perent della nostra select2 quando la select è dentro una modal. Serve per Firefox
        dropdownParent: null,
        //minimumInputLength: 3,
        ajax: {
            url: '/Privato/Moduli/RichiestaGenerica/RichiestaGenericaAjax.ashx',
            dataType: 'json',
            data: function (_Parameters) {
                var returnDataObject = {};
                returnDataObject["Call"] = "AUTOCOMP";
                returnDataObject["Search"] = cw_Base64Encode(_Parameters.term);
                returnDataObject["DataQ"] = _DataQuery;
                return returnDataObject;
            },
            processResults: function (_ResultData) {
                return _ResultData;
            },
        },
        escapeMarkup: function (markup) { return markup; },
        templateResult: formatRepoFunc,
        templateSelection: formatRepoSelectionFunc
    });
    sel2.on('change', function (e) {
        var obj = $(this), data = obj.select2('data'), id, txt; 
        debugger;
        if (data && (data.length > 0)) {
            id = data[0].id;
            // txt = data[0].text;
            $("#qst_" + obj.attr("id") + "_CB").val(id);
        }
    });

}

/* ********************************************************************************
                            D E T A I L S     M E T H O D S
   ******************************************************************************** */

// Convalida e aggiunge una riga alla tabella relativa al dettaglio passato popolata
// con i valori della maschera di compilazione. Questa funzione effettua il test 
// della modalità di visualizzazione. In tal caso impedisce l'inserimento.
function cw_AddTableDetail(_DetailIndex, _Validate = null) {
    if ((_Validate == null) || (_Validate == true)) {
        if (cw_DISPLAY_MODE === true) {
            SM_Warning("Nella modalità di visualizzazione non è consentito apportare delle modifiche ai contenuti.");
        }
        else if (cw_ValidateDetails(_DetailIndex)) {
            cw_AppendTableDetail(_DetailIndex, 99999);
        }
        return true;
    }
    else return false;
}

// Convalida e aggiunge una riga alla tabella relativa al dettaglio passato popolata
// con i valori della maschera di compilazione.
function cw_AppendTableDetail(_DetailIndex, _GotoPage = -1) {
    var ix = '' + _DetailIndex, nm, vl, id, ty;
    var cols = cw_ARRAY_DETAILS_ID[_DetailIndex].length;
    var rows = 0 + SM_Int($('#tbDetails' + ix + "_Rows").val());
    var htm = "<tr id='tbDetails" + ix + "_Row" + rows + "' name='tbDetails" + ix + "_Row" + rows + "' >\r\n";
    var hid = '', clss = " class='tbDetails" + ix + "_Row" + rows + "_Hid'";
    el = $('#tbDetails' + ix + ' tbody');
    if (el.length > 0) {
        for (i = 0; i < cols; i++) {
            id = cw_ARRAY_DETAILS_ID[_DetailIndex][i];
            ty = cw_ARRAY_DETAILS_TYPE[_DetailIndex][i];
            if (ty == 'COMBO') {
                vl = $("#qst_" + id).val();
                htm += '<td>' + SM_Html($("#qst_" + id + " option:selected").text());
            }
            else if (ty == 'CHECK') {
                if ($("#qst_" + id).is(':checked')) {
                    vl = '1';
                    htm += '<td>' + SM_Html("SI");
                }
                else {
                    vl = '0';
                    htm += '<td>' + SM_Html("no");
                }
            }
            else if (ty == 'YES/NO') {
                if ($("#qst_" + id+ '_Y').is(':checked')) {
                    vl = 'Y';
                    htm += '<td>' + SM_Html("SI");
                }
                else if ($("#qst_" + id + '_N').is(':checked')) {
                    vl = 'N';
                    htm += '<td>' + SM_Html("NO");
                }
                else {
                    vl = '';
                    htm += '<td>' + SM_Html("-");
                }
            }
            else {
                vl = $("#qst_" + id).val();
                htm += '<td>' + SM_Html(vl);
            }
            nm = 'tbDetails' + ix + '_Row' + rows + '_' + cw_ARRAY_DETAILS_FIELDS[_DetailIndex][i];
            hid += "<input type='hidden' name='" + nm + "' id='" + nm + "' value='" + SM_Html(vl) + "'" + clss + " >\r\n";
        }
        htm += '</td><td><a href="javascript:cw_DeleteTableRow(' + _DetailIndex + ',&quot;#tbDetails' + ix + '_Row' + rows + '&quot;);" class="cw-anchor-control" >';
        htm += '<img id="tbDetails' + ix + '_Row' + rows + '_Delete" title="Elimina record" src="/Immagini/tableDelete28px.png"></a>';
        nm = 'tbDetails' + ix + '_Row' + rows + '_IdDettaglio';
        hid += "<input type='hidden' name='" + nm + "' id='" + nm + "' value='-1'" + clss + " >\r\n";
        htm += "</td>\r\n</tr>";
        el.append(htm);
        $('#tbDetails' + ix + '_Rows').val(rows + 1);
        //
        htm = $('#cwDetailsHiddenBuffer').html();
        htm += hid;
        $('#cwDetailsHiddenBuffer').html(htm);
    }
    // ripulisce i campi di input
    cw_ClearTableDetailInput(_DetailIndex);
    if (_GotoPage > -1) cw_ARRAY_DETAILS_TABLES[_DetailIndex].page(_GotoPage);
    if (typeof _ONDETAILS === 'function') _ONDETAILS(_DetailIndex);
}

// Aggiunge la riga specificata con i valori delle contenuti nell'array, alla tabella relativa al dettaglio indicato.
function cw_AppendTableDetailRow(_DetailIndex, _Columns, _CanBeDeleted = false, _DetailId=-1, _GotoPage=-1) {
    var ix = '' + _DetailIndex, nm, hc = _Columns.length;
    var cols = cw_ARRAY_DETAILS_ID[_DetailIndex].length;
    var rows = 0 + SM_Int($('#tbDetails' + ix + "_Rows").val());
    var htm = "<tr id='tbDetails" + ix + "_Row" + rows + "' name='tbDetails" + ix + "_Row" + rows + "' >\r\n";
    var hid = '', clss = " class='tbDetails" + ix + "_Row" + rows + "_Hid'";
    el = $('#tbDetails' + ix + ' tbody');
    if (el.length > 0) {
        for (i = 0; (i < cols)&&(i<hc); i++) {
            htm += '<td>' + SM_Html('' + _Columns[i]);
            nm = 'tbDetails' + ix + '_Row' + rows + '_' + cw_ARRAY_DETAILS_FIELDS[_DetailIndex][i];
            hid += "<input type='hidden' name='" + nm + "' id='" + nm + "' value='" + SM_Html('' + _Columns[i]) + "'" + clss + " ></td>";
        }
        if (_CanBeDeleted) {
            htm += '<td><a href="javascript:cw_DeleteTableRow(' + _DetailIndex + ',&quot;#tbDetails' + ix + '_Row' + rows + '&quot;);">';
            htm += '<img id="tbDetails' + ix + '_Row' + rows + '_Delete" title="Elimina record" src="/Immagini/tableDelete28px.png"></a></td></tr>';
        }
        else {
            htm += '<td><a href="javascript:void(0);">';
            htm += '<img id="tbDetails' + ix + '_Row' + rows + '_Void" title="" src="/Immagini/tableVoid28px.png"></a></td>' + "\r\n" + '</tr>';
        }
        //
        nm = 'tbDetails' + ix + '_Row' + rows + '_IdDettaglio';
        hid += "<input type='hidden' name='" + nm + "' id='" + nm + "' value='" + _DetailId + "'" + clss + " >\r\n";
        //
        el.append(htm);
        $('#tbDetails' + ix + '_Rows').val(rows + 1);
        //
        htm = $('#cwDetailsHiddenBuffer').html();
        htm += hid;
        $('#cwDetailsHiddenBuffer').html(htm);
    }
    if (_GotoPage > -1) cw_ARRAY_DETAILS_TABLES[_DetailIndex].page(_GotoPage);
    if (typeof _ONDETAILS === 'function') _ONDETAILS(_DetailIndex);
}

// Cancella tutte le righe della tabella relativa al dettaglio indicato
function cw_ClearTable(_DetailIndex) {
    cw_ARRAY_DETAILS_TABLES[_DetailIndex].clear();
    $("[id^='tbDetails" + _DetailIndex + "_Row'][type=hidden]").remove();
    if (typeof _ONDETAILS === 'function') _ONDETAILS(_DetailIndex);
}

// Ripulisce i campi di input della tabella relativa al dettaglio indicato.
function cw_ClearTableDetailInput(_DetailIndex) {
    var cols = cw_ARRAY_DETAILS_ID[_DetailIndex].length, id, ty;
    for (i = 0; i < cols; i++) {
        id = '#qst_' + cw_ARRAY_DETAILS_ID[_DetailIndex][i];
        ty = cw_ARRAY_DETAILS_TYPE[_DetailIndex][i];
        if (ty == 'CHECK') $(id).prop('checked', false);
        else if (ty == 'YES/NO') {
            $(id + '_Y').prop('checked', false);
            $(id + '_N').prop('checked', false);
        }
        else $(id).val('');
    }
}

// Cancella la riga della tabella con il selettore JQuery passato.
function cw_DeleteTableRow(_DetailIndex, _JQSel) {
    var dlt;
    if (cw_DISPLAY_MODE === true) {
        SM_Warning("Nella modalità di visualizzazione non è consentito apportare delle modifiche ai contenuti.");
    }
    else {
        dlt = $('#CW_DETAILS_DELETION' + _DetailIndex).val();
        dlt = dlt + ',' + $(_JQSel + '_IdDettaglio').val();
        $('#CW_DETAILS_DELETION' + _DetailIndex).val(dlt);
        var elem = cw_GetElem(_JQSel);
        if ((elem != null) && (elem.length > 0)) {
            var id = elem.attr("id");
            if (!SM_Empty(id)) {
                $('input.' + id + '_Hid:hidden').remove();
            }
        }
        $(_JQSel).remove();
        cw_ARRAY_DETAILS_TABLES[_DetailIndex].page(99999);
        if (typeof _ONDETAILS === 'function') _ONDETAILS(_DetailIndex);
    }
}

// Ritorna il numero delle righe effettive presenti nella tabella del dettaglio indicato.
function cw_DetailRows(_DetailIndex) {
    return $('#tbDetails' + _DetailIndex + ' tr').length - 1;
}

// Ritorna il valore dell'elemento della tabella corrispondente ai parametri passati.
function cw_TableDetailValue(_DetailIndex, _Row, _Field) {
    var r = cw_GetStr('#tbDetails' + _DetailIndex + '_Row' + _Row + '_' + _Field);
}

// Ritorna il numero delle righe presenti nella tabella (comprese le eventuali cancellate).
function cw_TableDetailRows(_DetailIndex) {
    var r = cw_GetInt('#tbDetails' + _DetailIndex + '_Rows');
}

// Ritorna true se la riga con l'indice specificato esiste
function cw_TableDetailRowExists(_DetailIndex, _RowIndex) {
    var q = $("#tbDetails" + _DetailIndex + "_Row" + _RowIndex);
    if (q && q.length > 0) return true;
    else return false;
}
