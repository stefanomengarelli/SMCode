/*
 * smartdata-std.js - Funzioni javascript per il supporto delle funzionalità SmartData.
 */

//// Ritorna true se il checkbox individuato dal selettore è marcato (vedi sdObj).
//function sdChecked(_selector) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) return obj.is(':checked');
//    else return false;
//}

// Ritorna la stringa delle chips con l'aggiunta del nuovo valore.
function sdChipsAdd(_chips, _value) {
    var a = null, b = true;
    _value = _value.trim();
    if (_value.length > 0) {
        a = _chips.split(';');
        if (a) {
            for (i = 0; i < a.length; i++) {
                if (a[i]) {
                    if (a[i].trim().length > 0) {
                        if (_value.toLowerCase() == a[i].toLowerCase()) b = false;
                    }
                }
            }
            if (b) a[a.length] = _value;
        }
    }
    return sdChipsStr(a);
}

// Ritorna la stringa delle chips con il valore passato rimosso.
function sdChipsDel(_chips, _value) {
    var a = null, i = 0;
    _value = _value.trim().toLowerCase();
    if (_value.length > 0) {
        a = _chips.split(';');
        if (a) {
            while (i < a.length) {
                if (a[i]) {
                    if (a[i].trim().length > 0) {
                        if (_value == a[i].toLowerCase()) a[i] = '';
                    }
                    else a[i] = '';
                }
                else a[i] = '';
                i++;
            }
        }
    }
    return sdChipsStr(a);
}

// Ritorna la stringa delle chips dall'array passato.
function sdChipsStr(_array) {
    var i, r = '';
    if (_array) {
        if (_array.constructor == Array) {
            _array.sort();
            for (i = 0; i < _array.length; i++) {
                if (_array[i]) {
                    if (_array[i].trim().length > 0) r += _array[i].trim() + ';';
                }
            }
        }
    }
    return r;
}

// Ritorna la data relativa alla stringa passata o all'argomento passato.
// Se il parametro passato è nullo ritorna la data odierna.
// Se la stringa passata (nel formato GG/MM/AAAA) non è valida ritorna 
// la data SD_DATE_MIN (01-01-1900).
function sdDate(_value = null) {
    if (_value == null) return new Date(new Date().toDateString());
    else if (_value instanceof Date) return _value;
    else if (typeof _value == 'number') return new Date(_value);
    else if ((typeof _value == 'string') || (_value instanceof String)) {
        _value = _value.trim();
        if (_value.length > 7) {
            var p = null;
            if (_value.indexOf('/') > -1) p = _value.split('/');
            else if (_value.indexOf('-') > -1) p = _value.split('-');
            if (p != null) {
                if (p.length > 2) return new Date(+p[2], p[1] - 1, +p[0]);
                else return SD_DATE_MIN;
            }
            else return SD_DATE_MIN;
        }
        else return SD_DATE_MIN;
    }
    else return SD_DATE_MIN;
}

// Ritorna il valore della data in millisecondi dal 01-01-1970.
function sdDateInt(_value = null) {
    return sdDate(_value).getTime();
}

// Ritorna la data relativa alla stringa passata viceversa se il parametro passato
// è di tipo data ritorna la stringa di rappresentazione. Se il parametro passato
// non è valido ritorna la data SM_DATE_MIN (01-01-1900).
function sdDateStr(_value = null) {
    _value = sdDate(_value);
    if (_value.getTime() <= SD_DATE_MIN.getTime()) return "";
    else return _value.getDate().toString().padStart(2, '0')
        + SD_DATE_SEPARATOR + _value.getMonth().toString().padStart(2, '0')
        + SD_DATE_SEPARATOR + _value.getFullYear().toString();
}

// Visualizza la finestra modale di conferma.
function sdDlgConfirm(_message, _functionYes = null, _functionNo = null, _title = "") {
    sdModal(
        _message,
        _title,
        'Sì',
        function () { sdModalClose(); if (typeof _functionYes === 'function') _functionYes(); },
        'No',
        function () { sdModalClose(); if (typeof _functionNo === 'function') _functionNo(); },
        'it-help-circle',
        '',
        'icon-warning');
}

// Visualizza la finestra modale di errore.
function sdDlgError(_message, _title = "", _button = 'OK') {
    sdModal(
        _message,
        _title,
        _button,
        function () { sdModalClose(); },
        null,
        null,
        'it-error',
        '',
        'icon-danger');
}

// Visualizza la finestra modale della lista di errori.
function sdDlgErrorList(_callback = null, _button = 'OK') {
    if (SD_ERROR_LIST.length > 0) {
        sdModal(
            '@Errore nella compilazione dei campi:<br />' + SD_ERROR_LIST,
            '',
            _button,
            function () { sdModalClose(); if (_callback != null) _callback(); },
            null,
            null,
            'it-error',
            'modal-lg',
            'icon-danger'
        );
    }
}

// Visualizza la finestra modale di informazione.
function sdDlgInfo(_message, _title = '', _button = 'OK') {
    sdModal(
        _message,
        _title,
        _button,
        function () { sdModalClose(); },
        null,
        null,
        'it-info-circle',
        '',
        'icon-primary');
}

// Visualizza la finestra modale di avvertenza.
function sdDlgWarning(_message, _title = '', _button = 'OK') {
    sdModal(
        _message,
        _title,
        _button,
        function () { sdModalClose(); },
        null,
        null,
        'it-warning-circle',
        '',
        'icon-warning');
}

// Visualizza la finestra modale di avvertenza.
function sdDlgProcess(_message, _title = '', _button = null) {
    sdModal(
        _message,
        _title,
        _button,
        function () { sdModalClose(); },
        null,
        null,
        'it-exchange-circle',
        '',
        'icon-primary');
}

//// Abilita o disabilita l'elemento con il selettore passato (vedi: sdObj). /* TODO */
//function sdEnable(_selector, _value) {
//    var obj = sdObj(_selector), ty;
//    if (_value == false) _value = false;
//    else _value = true;
//    if (obj && obj.length) {
//        ty = sdGetType(obj);
//        if (ty == 'location') {

//        }
//        obj.prop("disabled", _value);
//    }
//}

// Formatta l'elemento con il selettore passato (vedi: sdObj).
function sdFormat(_selector, _format = null) {
    var obj = sdObj(_selector), ty, rt = '', mx;
    if (obj && obj.length) {
        rt = sdUntag(obj.val());
        ty = sdGetType(obj);
        if (ty.startsWith('NUM')) rt = sdVal(rt);
        else if (ty == ('DATE')) rt = sdDateStr(('' + rt).trim());
        else if (ty == ('TIME')) rt = sdTime(('' + rt).trim());
        if (_format == null) _format = sdGetFormat(obj);
        rt = sdFmt(rt, _format, ty);
        mx = SM.int(obj.attr('maxlength'));
        if ((mx > 0) && (rt.length > mx)) rt = rt.substring(0, mx);
        obj.val(rt);
    }
    return rt;
}

// Funzione che converte il valore _Value in una stringa formattata secondo il parametro 
// _Format che può assumere i valori: EUR, EURNZ, NZ, INT, INTNZ, UPPER, LOWER.
function sdFmt(_value, _format, _type) {
    if (_type.startsWith('NUM') || ('|EUR|EURNZ|NZ|INT|INTNZ|'.indexOf('|' + _format + '|') > -1)) {
        if (typeof _value != 'number') _value = sdVal(_value);
        if ((_format == 'EUR') || (_format == 'EU')) return sdFmtEuro(_value);
        else if ((_format == 'EURNZ') || (_format == 'EUNZ')) {
            if (_value == 0) return '';
            else return sdFmtEuro(_value);
        }
        else if (_format == 'NZ') {
            if (_value == 0) return '';
            else return _value.toLocaleString('it-IT');
        }
        else if (_format == 'INT') return Math.trunc(_value).toString();
        else if (_format == 'INTNZ') {
            _value = Math.trunc(_value);
            if (_value == 0) return '';
            else return _value.toString();
        }
        else if (_format.startsWith('DNZ')) {
            if (_value == 0) return '';
            else return (0 + _value).toLocaleString('it-IT', { minimumFractionDigits: parseInt(_format.substr(3)) });
        }
        else if (_format.startsWith('D')) {
            return (0 + _value).toLocaleString('it-IT', { minimumFractionDigits: parseInt(_format.substr(1)) });
        }
        else return _value.toLocaleString('it-IT');
    }
    else if (_format == 'UPPER') return ('' + _value).toUpperCase();
    else if (_format == 'LOWER') return ('' + _value).toLowerCase();
    else return '' + _value;
}

// Ritorna il valore passato come stringa formattata in EURO
function sdFmtEuro(_value) {
    if (isNaN(_value)) return '';
    else return (0 + _value).toLocaleString('it-IT', { minimumFractionDigits: 2 });
}

//// Ritorna il valore dell'elemento corrispondente al selettore specificato (vedi sdObj).
//function sdGet(_selector, _attrib = null) {
//    var obj = sdObj(_selector), ty, vl = '';
//    if (obj && obj.length) {
//        ty = sdGetType(obj);
//        if (ty == 'YES/NO') {
//            if (_attrib != null) vl = obj.attr(_attrib);
//            else if (obj.is(':checked')) vl = 'Y';
//            else {
//                var obx = $('#' + obj.attr('id') + '_N');
//                if (obx && obx.length) {
//                    if (obx.is(':checked')) vl = 'N';
//                    else vl = '';
//                }
//                else vl = '';
//            }
//        }
//        else if (ty == 'CHECK') {
//            if (_attrib != null) vl = obj.attr(_attrib);
//            else if (obj.is(':checked')) vl = '1';
//            else vl = '0';
//        }
//        else {
//            if (_attrib != null) vl = obj.attr(_attrib);
//            vl = obj.val();
//        }
//    }
//    return vl;
//}

//// Ritorna l'alias dell'elemento corrispondente al selettore specificato (vedi sdObj).
//function sdGetAlias(_selector) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) return ('' + obj.attr('sd-alias')).trim();
//    else return '';
//}

// Ritorna l'alias dell'elemento corrispondente al selettore specificato (vedi sdObj).
function sdGetErrorLabel(_selector) {
    var obj = sdObj(_selector), id, ix, rt = null;
    if (obj && obj.length) {
        id = obj.attr('id').trim();
        ix = id.indexOf('_');
        if (ix > -1) {
            rt = sdObj("#err" + id.substring(ix));
            if (!(rt && rt.length)) rt = null;
        }
    }
    return rt;
}

//// Ritorna l'attributo for specifico dell'elemento corrispondente al selettore specificato (vedi sdObj).
//function sdGetFor(_selector) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) return ('' + obj.attr('sd-for')).trim();
//    else return '';
//}

//// Ritorna il nome del campo associato all'elemento corrispondente al selettore specificato (vedi sdObj).
//function sdGetField(_selector) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) return ('' + obj.attr('sd-field')).trim();
//    else return '';
//}

//// Ritorna il formato associato all'elemento corrispondente al selettore specificato (vedi sdObj).
//function sdGetFormat(_selector) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) return ('' + obj.attr('sd-format')).trim();
//    else return '';
//}

// Ritorna il valore dell'id passato contenuto in SD_STATE, 
// se non lo trova ritorna il valore di default.
function sdGetState(_id, _default = '') {
    try {
        if (SD_STATE == null) SD_STATE = sdJson64Decode($('#SD_STATE').val());
        if (SD_STATE[_id] == null) return _default;
        else return '' + SD_STATE[_id];
    }
    catch {
        return _default;
    }
}

//// Ritorna il tipo dell'elemento corrispondente al selettore specificato (vedi sdObj).
//function sdGetType(_selector) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) return ('' + obj.attr('sd-type')).trim().toUpperCase();
//    else return '';
//}

//// Restituisce uno dei valori passati a seconda del risultato del test.
//function sdIif(_test, _ifTrue, _ifFalse) {
//    if (_test != false) return _ifTrue;
//    else return _ifFalse;
//}

// Inizializza gli elementi selezionati per l'utilizzo con la libreria Select2 via AJAX.
function sdInitSelect2(_selector, _placeHolder) {
    var sel2 = sdObj(_selector).select2({
        allowClear: true,
        dropdownParent: null,
        placeholder: _placeHolder,
        language: 'it',
        // ajax
        ajax: {
            url: '/Ajax?handler=Select2',
            dataType: 'json',
            theme: 'bootstrap-5',
            type: 'GET',
            data: function (_Parameters) {
                debugger;
                var rdo = {}, c = $(this).attr('sd-ajc');
                if (c == undefined) c = '';
                rdo["sd_ajc"] = c;
                rdo["sd_ajs"] = _Parameters.term;
                c = $(this).attr('sd-ajq');
                rdo["sd_ajq"] = c;
                c = $(this).attr('sd-dat');
                if (c == undefined) c = '';
                rdo["sd_dat"] = c;
                return rdo;
            },
            processResults: function (_ResultData) {
                debugger;
                var json = JSON.parse(_ResultData);
                return json;
            }
        }
    });
    sel2.on('change', function (e) {
        var obj = $(this), data = obj.select2('data'), ele = $("#" + obj.attr("id")), json;
        debugger;
        if (data && (data.length > 0)) {
            ele.val(data[0].id);
            json = sdJson64Decode(ele.attr("sd-dat"));
            json["sd_dat_id"] = data[0].id;
            json["sd_dat_text"] = data[0].text;
            json["sd_dat_tag"] = data[0].tag;
            ele.attr("sd-dat", sdJson64Encode(json));
        }
    });
    return sel2;
}

// Ritorna true se l'elemento individuato dal selettore (vedi sdObj) è visibile.
function sdIsVisible(_selector) {
    var obj = sdObj(_selector), r = true;
    while (r && obj && obj.length) {
        if ((obj.css('display') == 'none') || (obj.hasClass('sd-hidden'))) r = false;
        if (obj.is('form')) break;
        obj = obj.parent();
    }
    return r;
}

// Decodifica JSON in Base64
function sdJson64Decode(_json, _default = {}) {
    if (_json) {
        if (_json.length < 1) return _default;
        else {
            try {
                return JSON.parse(SM.base64Decode(_json));
            }
            catch {
                return _default;
            }
        }
    }
    else return _default;
}

// Codifica JSON in Base64
function sdJson64Encode(_object) {
    if (typeof _object === 'object') return SM.base64Encode(JSON.stringify(_object));
    else if (_object != null) return SM.base64Encode(JSON.stringify({ _object }));
    else return '[]';
}

// Ritorna il valore del parametro contenuto nel JSON codificato in base 64
function sdJson64Get(_json64, _parm) {
    var json = sdJson64Decode(_json64);
    var r = json[_parm];
    if (r) return r;
    else return '';
}

// Ritorna il JSON codificato in base 64 con il valore del parametro impostato
function sdJson64Set(_json64, _parm, _value) {
    var json = sdJson64Decode(_json64);
    json[_parm] = _value;
    return sdJson64Encode(json);
}

// Imposta nell'elemento JSON passato il valore della chiave.
function sdJsonSetKeyValue(_json, _key, _value) {
    var r = new Array(), q = new Array();
    if (_json != null) {
        if (typeof _json === 'string') {
            try {
                if (!_json.startsWith('[')) _json = '[' + _json + ']';
                r = JSON.parse(_json);
            } catch (error) {
                r = new Array();
            }
        }
    }
    if (!SM.empty(_key)) {
        for (var i = 0; i < r.length; i++) if (!r[i].hasOwnProperty(_key)) q.push(r[i]);
        if (!SM.empty(_value)) {
            var o = {};
            o[_key] = _value;
            q.push(o);
        }
    }
    return q;
}

// Visualizza la finestra modale.
function sdModal(
    _message = '',
    _title = '',
    _button = null,
    _buttonFn = null,
    _buttonAlt = null,
    _buttonAltFn = null,
    _sprite = 'it-info-circle',
    _modalClass = '',
    _iconClass = '',
    _initFn = null,

) {
    $("#sdModalDialogClient").attr('class', 'modal-dialog ' + _modalClass);
    $("#sdModalIconUse").attr('href', '/lib/bootstrap-italia/svg/sprites.svg#' + _sprite);
    $("#sdModalIconSvg").attr('class', 'icon ' + _iconClass);
    $("#sdModalDialogTitle").html(sdToHtml(_title, '@'));
    $("#sdModalDialogMessage").html(sdToHtml(_message, '@'));
    //
    //if (SM.empty(_button) && SM.empty(_buttonAlt)) $("#sdModalDialogClose").show();
    //else 
    $("#sdModalDialogClose").hide();
    //
    $("#sdModalDialogButton").html(sdToHtml(_button, '@'));
    $("#sdModalDialogButton").off('click');
    if (SM.empty(_button)) $("#sdModalDialogButton").hide();
    else {
        $("#sdModalDialogButton").on('click', _buttonFn);
        $("#sdModalDialogButton").show();
    }
    //
    $("#sdModalDialogButtonAlt").html(sdToHtml(_buttonAlt, '@'));
    $("#sdModalDialogButtonAlt").off('click');
    if (SM.empty(_buttonAlt)) $("#sdModalDialogButtonAlt").hide();
    else {
        $("#sdModalDialogButtonAlt").on('click', _buttonAltFn);
        $("#sdModalDialogButtonAlt").show();
    }
    //   
    if ((_initFn != null) && (typeof _initFn === 'function')) _initFn();
    else $('#sdModalDialog_Standard').removeClass('sd-hidden');
    $('#sdModalDialog').modal('show');
    return true;
}

// Chiude la finestra modale.
function sdModalClose() {
    $('#sdModalDialog').modal('hide');
    $('#sdModalDialog_Standard').addClass('sd-hidden');
    $('#sdModalDialog_Input').addClass('sd-hidden');
    $('#sdModalDialog_KeyValue').addClass('sd-hidden');
    $('#sdModalDialog_Ajax').addClass('sd-hidden');
    return true;
}

// Restituisce l'oggetto jQuery rappresentante l'elemento relativo al campo con il
// selettore specificato. Se il selettore inizia per #,[ oppure . si intende un 
// selettore puro JQuery. Se inizia per ! o ? si intende relativo al campo associato
// alla domanda con id uguale al numero che segue. 
// Se inizia per * si intende l'oggetto relativo al messaggio di errore della
// domanda con id uguale al numero che segue, mentre
// se inizia per $ si intende l'oggetto relativo all'etichetta (label) della domanda
// con id uguale al numero che segue. 
// Se inizia per @ si intende relativo al campo a cui è stato associato l'alias che segue.
// Se non inizia per i caratteri citati si intende relativo al campo a cui è
// associato il nome della colonna della tabella.
// Se il selettore è già un oggetto di tipo jQuery viene restituito il suo valore.
function sdObj(_selector) {
    if (_selector) {
        if (_selector instanceof jQuery) return _selector;
        else {
            _selector = _selector.trim();
            if (_selector.length > 1) {
                if ('#[.'.indexOf(_selector.substr(0, 1)) < 0) {
                    if (_selector.startsWith("!") || _selector.startsWith("?")) _selector = '#qst_' + _selector.substr(1);
                    else if (_selector.startsWith('*')) _selector = '#err_' + _selector.substr(1);
                    else if (_selector.startsWith('$')) _selector = '#lbl_' + _selector.substr(1);
                    else if (_selector.startsWith('@')) _selector = "[sd-alias='" + _selector.substr(1) + "']";
                    else _selector = "[sd-field='" + _selector + "']";
                }
                return $(_selector);
            }
            else return null;
        }
    }
    else return null;
}

// Ritorna true se l'elemento corrispondente al selettore specificato (vedi sdObj) 
// non è stato compilato.
function sdOmitted(_selector, _message) {
    var obj = sdObj(_selector), ty;
    if (obj && obj.length) {
        ty = sdGetType(obj);
        if (ty == 'YES/NO') {
            if (obj.is(':checked')) return false;
            else {
                var obx = $('#' + obj.attr('id') + '_N');
                if (obx && obx.length) {
                    if (obx.is(':checked')) return false;
                    else return true;
                }
                else return true;
            }
        }
        else if (ty == 'CHECK') return !obj.is(':checked');
        else return SM.empty(obj.val());
    }
    else return false;
}

// Gestisce l'evento di gestione della visualizzazione delle chips.
function sdOnChips(_selector = null) {
    var obj;
    if (_selector == null) obj = $(".sd-item-chips");
    else obj = sdObj(_selector);
    if (obj && obj.length > 0) {
        obj.each(function () {
            var id = $(this).attr('id'), idx = id + '_chips', v = $(this).val();
            var a = v.split(';'), html = "";
            if (a) {
                var i;
                for (i = 0; i < a.length; i++) {
                    if (!SM.empty(a[i])) {
                        html += '<div class="chip chip-primary chip-lg alert">';
                        html += '<span class="chip-label">' + a[i] + '</span>';
                        html += '<button data-bs-dismiss="alert" onclick="sdOnChipsDel(' + "'#" + id + "','" + a[i] + "'" + ');">';
                        html += '<svg class="icon icon-primary"><use href="/lib/bootstrap-italia/svg/sprites.svg#it-close"></use></svg>';
                        html += '<span class="visually-hidden">Elimina elemento</span>';
                        html += '</button>';
                        html += '</div>';
                    }
                }
                if (html == "") {
                    html += '<div class="chip chip-lg chip-disabled">';
                    html += '<span class="chip-label">Nessun elemento specificato.</span>';
                    html += '</button>';
                    html += '</div>';
                }
            }
            $('#' + idx).html(html);
        });
    }
}

// Aggiunge il valore passato alle chips dell'elemento corrispondente al selettore.
function sdOnChipsAdd(_selector, _value) {
    var obj = sdObj(_selector);
    if (obj && obj.length > 0) {
        sdSet(_selector, sdChipsAdd(obj.val(), _value))
        sdOnChips(_selector);
    }
}

// Rimuove la chips relativa al valore passato dall'elemento corrispondente al selettore.
function sdOnChipsDel(_selector, _value) {
    var obj = sdObj(_selector);
    if (obj && obj.length > 0) {
        sdSet(_selector, sdChipsDel(obj.val(), _value))
        sdOnChips(_selector);
    }
}

// Gestisce l'evento di click sui controlli di tipo YES/NO.
function sdOnClickYN(_jquery_selector) {
    var obj = $(_jquery_selector);
    if (obj && obj.length) {
        if (_jquery_selector.endsWith('_N')) {
            if (obj.is(':checked')) $(_jquery_selector.substr(0, _jquery_selector.length - 2)).prop('checked', false);
        }
        else if (obj.is(':checked')) $(_jquery_selector + '_N').prop('checked', false);
        return true;
    }
    else return false;
}

// Gestisce l'evento on leave dell'elemento corrispondente al selettore specificato (vedi sdObj).
function sdOnLeave(_selector) {
    if (typeof _selector === 'string') {
        var obj = sdObj(_selector);
        if (obj && obj.length) {
            if (SD_LEAVED_CTRL != obj) SD_LEAVED_CTRL = obj;
            sdSet(_selector, sdGet(_selector));
            if (typeof SD_ONLEAVE === 'function') SD_ONLEAVE(_selector);
            sdOnUpdate();
        }
    }
}

// Gestisce le funzioni di inizializzazione previste per l'evento ready di jQuery.
function sdOnReady() {
    //
    debugger;
    SD_STATE = sdJson64Decode($("#SD_STATE").val());
    //
    $('a.sd-nav-link').removeClass('active');
    $('a.sd-nav-link[data-element="@SmartDataRender.Render.PageId.ToLower()"]').addClass('active');
    $('form input').keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            return false;
        }
        else if (e.keyCode == 27) {
            e.preventDefault();
            return false;
        }
    });
    //
    sdInitSelect2(".sd-select2-addrel", "Seleziona un elemento da aggiungere...")
    //
    if (typeof sdInitialize === 'function') sdInitialize();
    if (typeof sdInitializeTabs === 'function') sdInitializeTabs();
    if (typeof sdInitializeTables === 'function') sdInitializeTables();
    if (typeof sdInitializeDetails === 'function') sdInitializeDetails();
    if (typeof sdInitializeAttachments === 'function') sdInitializeAttachments();
    if (typeof sdInitializeGeoMaps === 'function') sdInitializeGeoMaps();
    if (typeof sdInitializeUploads === 'function') sdInitializeUploads();
    //
    if (typeof SD_ONREADY === 'function') SD_ONREADY();
    sdOnUpdate();
    //
    $('#sdLoader').hide();
}

// Gestisce l'evento on calculate nel form visualizzato.
function sdOnUpdate() {
    if (typeof sdEventUpdate === 'function') sdEventUpdate();
    if (typeof SD_ONCALCULATE === 'function') SD_ONCALCULATE();
    if (typeof sdEventControl === 'function') sdEventControl();
    sdOnChips();
    if (typeof sdEventEnable === 'function') sdEventEnable();
    if (typeof sdEventVisible === 'function') sdEventVisible();
}

// Gestisce l'evento on validate del form.
function sdOnValidate() {
    if (typeof sdEventValidate === 'function') return sdEventValidate();
    else return true;
}

// Ritorna il nome della pagina corrente senza percorso ne estensione.
function sdPageId() {
    var p = window.location.pathname;
    p = p.split('/').pop();
    return SM.before(p + '.', '.');
}

// Chiama la pagina passata come action post del form SD_FORM.
function sdPost(_page = null, _validate = true) {
    var f = $("#SD_FORM");
    if (f && f.length) {
        if (_page != null) f.attr("action", _page);
        if (_validate != false) {
            $(".sd-form-control").attr("disabled", true);
            sdDlgProcess('Aggiornamento dati in corso...');
            f.submit();
        }
    }
}

// Ritorna la codifica dei parametri di default per la query string.
function sdQueryParametersToX64(_idform = null, _iddocumento = -1, _iddettagli = -1, _idriga = -1) {
    if (_idform == null) _idform = SD_FORM_ID;
    if (_iddocumento == null) _idform = SD_DOCUMENT_ID;
    var v = {
        "IdUtente": SD_USER_ID,
        "IdDocumento": _iddocumento,
        "IdForm": _idform,
        "IdDettagli": _iddettagli,
        "IdRiga": _idriga,
        "BackUrl": SD_BACK_URL,
    };
    v = JSON.stringify(v);
    return SM.base64Encode(v);
}

// Ritorna il valore  incluso in singoli apici.
function sdQuote(_value) {
    return "'" + ('' + _value).replaceAll("'", "\\\'") + "'";
}

// Ritorna il valore  incluso in doppi apici.
function sdQuote2(_value) {
    return '"' + ('' + _value).replaceAll('"', '\\\"') + '"';
}

// Seleziona l'ente con l'id passato.
function sdSelectEnte(_ente) {
    debugger;
    var l = '' + window.location;
    l = l.substr(0, (l + '?').indexOf('?'));
    window.location = l + '?sd_q=' + SM.setJson64(SD_Q, 'sd_ete', '' + _ente);
}

// Imposta il valore dell'elemento corrispondente al selettore specificato (vedi sdObj).
function sdSet(_selector, _value, _format = true) {
    var obj = sdObj(_selector), ty, mx, hid, obx;
    if (SM.empty(_value)) _value = '';
    if (obj && obj.length) {
        ty = sdGetType(obj);
        if (ty == 'YES/NO') {
            if (_value === true) _value = 'Y';
            else {
                _value = (_value + ' ').trim().toUpperCase().substring(0, 1);
                if ((_value == '1') || (_value == 'T') || (_value == 'V') || (_value == 'Y') || (_value == '+')) _value = 'Y';
                else if ((_value == '0') || (_value == 'F') || (_value == 'N') || (_value == '-')) _value = 'N';
                else _value = '';
                obx = $('#' + obj.attr('id') + '_N');
                if (_value == 'Y') {
                    obj.prop('checked', true);
                    if (obx && obx.length) obx.prop('checked', false);
                }
                else if (_value == 'N') {
                    obj.prop('checked', false);
                    if (obx && obx.length) obx.prop('checked', true);
                }
                else {
                    obj.prop('checked', false);
                    if (obx && obx.length) obx.prop('checked', false);
                }
            }
        }
        else if (ty == 'CHECK') {
            if (_value === true) _value = '1';
            else if (_value === false) _value = '0';
            else {
                _value = (_value + ' ').trim().toUpperCase().substring(0, 1);
                if ((_value == '1') || (_value == 'T') || (_value == 'V') || (_value == 'Y') || (_value == '+')) _value = '1';
                else _value = '0';
            }
            obj.prop('checked', _value === '1');
        }
        else {
            if (_format) _value = sdFmt(_value, sdGetFormat(obj), sdGetType(obj));
            mx = SM.int(obj.attr('maxlength'));
            if ((mx > 0) && (_value.length > mx)) _value = _value.substring(0, mx);
            obj.val(_value);
        }
        hid = sdObj('#' + obj.attr('id') + '_b64');
        if (hid && hid.length) hid.val(SM.base64Encode(_value));
    }
    else _value = null;
    return _value;
}

// Ritorna il valore dell'id passato contenuto in SD_STATE, 
// se non lo trova ritorna il valore di default.
function sdSetState(_id, _value) {
    try {
        if (SD_STATE == null) SD_STATE = sdJson64Decode($('#SD_STATE').val());
        SD_STATE[_id] = '' + _value;
        $('#SD_STATE').val(sdJson64Encode(SD_STATE));
        return true;
    }
    catch {
        return false;
    }
}

// Ritorna il parametro passato come orario nel formato HH:MM.
// Se il valore passato è numerico, viene considerato come numero di millisecondi.
function sdTime(_value = null) {
    if (_value == null) return "";
    else if (typeof _value == 'number') {
        _value = _value / 60000;
        if ((_value > -1) && (_value < 1440)) {
            return Math.trunc(_value / 60).toString().padStart(2, '0') + ':' + (_value % 60).toString().padStart(2, '0');
        }
        else return "";
    }
    else if (_value.indexOf(':') > -1) {
        var a = _value.split(':');
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
function sdTimeInt(_value = null) {
    var s = sdTime(_value);
    if (s.trim().length > 0) return (SM.int(s.substr(0, 2)) * 60 + SM.int(s.substr(3, 2))) * 60000;
    else return 0;
}

// Ritorna la stringa passata con l'esplicitazione delle entità HTML
function sdToHtml(_value, _notIfStartWith = null) {
    if (_value) {
        _value = '' + _value;
        if (_notIfStartWith != null) {
            if (_value.startsWith(_notIfStartWith)) {
                if (_value.length > _notIfStartWith.length) return _value.substr(_notIfStartWith.length);
                else return '';
            }
        }
        if (_value.trim().length > 0) {
            return _value.replace(/[\u00A0-\u9999<>\&]/g, function (i) {
                return '&#' + i.charCodeAt(0) + ';';
            });
        }
        else return _value;
    }
    else return '';
}

/// Ritorna l'intero codificato con il seme passato e convertito in esadecimale.
function sdToIntSeed(_value, _seed = null) {
    if (_seed == null) _seed = SD_INT_SEED;
    return parseInt(_value, 16) - _seed;
}

// Ritorna l'intero codificato con il seme passato e convertito in esadecimale.
function sdToStrSeed(_value, _seed = null) {
    if (_seed == null) _seed = SD_INT_SEED;
    return (_value + _seed).ToString(16);
}

// Remove all HTML tags from value.
function sdUntag(_value) {
    return ('' + _value).replace(/<[^>]*>/g, ' ');
}

// Ritorna il valore numerico della stringa passata secondo le impostazioni di localizzazione italiane.
function sdVal(_value, _thousands_sep = null, _decimal_sep = null) {
    if (_thousands_sep == null) _thousands_sep = SD_THOUSANDS_SEPARATOR;
    if (_decimal_sep == null) _decimal_sep = SD_DECIMAL_SEPARATOR;
    _value = ('' + _value).replaceAll(_thousands_sep, '').replaceAll(_decimal_sep, '.');
    _value = parseFloat(_value);
    if (isNaN(_value)) return 0;
    else return 0 + _value;
}

// Se il parametro test restituisce true convalida il controllo relativo al selettore passato (vedi sdObj)
// altrimenti viene mostrata la label di errore con il messaggio indicato.
function sdValidate(_selector, _test, _messageIfNotValid, _focusOnControl = true) {
    var obj = sdObj(_selector), eid, s, ix;
    if (obj && obj.length) {
        eid = '#' + obj.attr('id').replace('qst_', 'err_');
        if (_test === false) {
            s = '#lbl_' + SM.after(obj.attr('id'), '_');
            s = $(s);
            if (s && s.length) {
                s = s.html().trim();
                ix = s.indexOf("<span");
                if (ix > 0) s = s.substr(0, ix);
                if (s.length > 0) SD_ERROR_LIST += "&bull; " + sdToHtml(s) + '<br />';
            }
            $(eid).html(sdToHtml(_messageIfNotValid));
            sdVisible(eid, true);
            if (_focusOnControl && (SD_ERROR_FOCUS == null)) {
                SD_ERROR_FOCUS = obj;
            }
        }
        else {
            $(eid).html('');
            sdVisible(eid, false);
        }
    }
    return _test != false;
}

//// Imposta la visibilità dell'oggetto individuato dal selettore passato (vedi sdObj).
//// Ritorna true se l'oggetto risulta visibile.
//function sdVisible(_selector, _setVisible = null) {
//    var obj = sdObj(_selector);
//    if (obj && obj.length) {
//        if (_setVisible != null) {
//            if (_setVisible == false) obj.addClass('sd-hidden');
//            else obj.removeClass('sd-hidden');
//            $('[sd-for="' + obj.attr('id') + '"]').each(function (index) {
//                if (_setVisible == false) $(this).addClass('sd-hidden');
//                else $(this).removeClass('sd-hidden');
//            });
//        }
//        return sdIsVisible(obj);
//    }
//    else return false;
//}
