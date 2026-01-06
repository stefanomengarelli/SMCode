/*  ===========================================================================
 *  File:       smcode.js
 *  Version:    2.0.321
 *  Date:       January 2026
 *  
 *  info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode javascript support library.
 *  
 *  MIT License
 *  ===========
 *  SMCode Javascript Rapid Application Development Code Library
 *  Copyright (c) 2010-2026 Stefano Mengarelli - All rights reserved.
 * 
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 * 
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 * 
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 *  ===========================================================================
 */

/*  ===========================================================================
 *  SMCode support library class
 *  ===========================================================================
 */
class SMCode {

    // Attribute prefix.
    attributePrefix = 'sm-';

    // Base-64 hidden element suffix.
    base64Suffix = '_b64';

    // Base 32 string.
    base32 = '0123456789ABCDEFGHKLMNPQRSTUVWXZ';

    // Base chars (digits + uppercase + lowercase).
    baseChars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';

    // Base symbols
    baseSymbols = 'àèéìòùç !?.,:;+-*/=<>#()[]{}@&%$£_§°\\~';

    // Base quotes.
    baseQuotes = '\'"«»´';

    // Class prefix.
    classPrefix = 'sm-';

    // Decimal point.
    decimalPoint = ',';

    // Element prefix.
    elementPrefix = 'sm_';

    // Last error code.
    errorCode = 0;

    // Last error message.
    errorMessage = '';

    // Locale string.
    localeString = 'it-IT';

    // Macro prefix
    macroPrefix = '%%';

    // Macro suffix
    macroSuffix = '%%';

    // Main container id.
    mainContainer = 'SM_FORM';

    // No element suffix.
    noElementSuffix = '_no';

    // Current state JSON.
    state = [];

    // Thousands separator. 
    thousandsSeparator = '.';

    // Ticks per day
    ticksPerDay = 86400000;

    // Ticks per hour
    ticksPerHour = 3600000;

    // Ticks per minute
    ticksPerMinute = 60000;

    // Year 2 digit century.
    year2DigitCentury = Math.floor(new Date().getFullYear() / 100) * 100;

    // Year 2 digit leap.
    year2DigitLeap = (new Date().getFullYear() - 70) % 100;

    // Instance constructor.
    constructor() {
        this.getState();
    }

    // Returns absolute value of number n.
    abs(_val) {
        _val = this.toVal(_val);
        if (_val < 0) return -_val;
        else return _val;
    }

    // Returns date adding days.
    addDays(_date, _days) {
        return this.date(this.dateTicks(_date) + _days * this.ticksPerDay);
    }

    // Returns part of string after first recourrence of sub string.
    // If sub string is not present returns empty string.
    after(_val, _find) {
        var i;
        _val = this.toStr(_val);
        _find = this.toStr(_find);
        i = _val.indexOf(_find);
        if (i < 0) return '';
        else if (i < _val.length - _find.length) return _val.substr(i + _find.length);
        else return '';
    }

    // Call URL via jQuery ajax function and is succeed perform success function.
    ajax(_url, _success, _error = null, _method = "GET", _data = null, _postForm = false, _async = false) {
        var _contentType = false;
        if (!_postForm) _contentType = 'application/x-www-form-urlencoded; charset=UTF-8';
        $.ajax({
            type: _method,
            data: _data,
            url: _url,
            async: _async,
            success: _success,
            error: _error,
            cache: false,
            processData: !_postForm,
            contentType: _contentType
        });
    }

    // Return decoded base 64 value.
    base64Decode(_val) {
        try {
            if (_val === undefined) return '';
            else if (_val == null) return '';
            else {
                _val = this.toStr(_val);
                if (_val.length > 0) return decodeURIComponent(escape(window.atob(_val)));
                else return '';
            }
        }
        catch {
            return '';
        }
    }

    // Return value encoded base 64.
    base64Encode(_val) {
        try {
            if (_val === undefined) return '';
            else if (_val == null) return '';
            else {
                _val = this.toStr(_val);
                if (_val.length > 0) return window.btoa(unescape(encodeURIComponent(_val)));
                else return '';
            }
        }
        catch {
            return '';
        }
    }

    // Returns part of string before first recurrence of substring.
    // If substring is not present returns empty string.
    before(_val, _find) {
        var i;
        _val = this.toStr(_val);
        _find = this.toStr(_find);
        i = _val.indexOf(_find);
        if (i > 0) return _val.substr(0, i);
        else return '';
    }

    // Returns part of string between start and end substrings.
    btw(_val, _start, _end, _ignoreCase = false) {
        var rslt = '', s, i;
        _val = this.toStr(_val);
        _start = this.toStr(_start);
        _end = this.toStr(_end);
        if (_ignoreCase) i = _val.toLowerCase().indexOf(_start.toLowerCase());
        else i = _val.indexOf(_start);
        if (i > -1) {
            s = this.mid(_val, i + _start.length);
            if (_ignoreCase) i = s.toLowerCase().indexOf(_end.toLowerCase());
            else i = s.indexOf(_end);
            if (i > -1) rslt = this.mid(s, 0, i);
        }
        return rslt;
    }

    // Returns string passed adding new string divided by separator.
    cat(_val, _new, _separator = '') {
        _val = this.toStr(_val);
        _new = this.toStr(_new);
        _separator = this.toStr(_separator);
        if (_new.length < 1) return _val;
        else if (_val.length > 0) return _val + _separator + _new;
        else return _new;
    }

    // Return true if selected control is checked or set it if specified.
    checked(_sel, _checked = null) {
        _sel = this.select(_sel);
        if (_sel && _sel.length) {
            if (_checked == null) return _sel.is(':checked');
            else {
                _checked = this.toBool(_checked);
                _sel.prop('checked', _checked);
                return _checked;
            }
        }
        else return false;
    }

    // Add value to chips with separator
    chipsAdd(_chips, _val, _sep = ';') {
        _chips = this.chipsArr(_chips, _sep);
        _val = this.toStr(_val).trim();
        if (_val.length > 0) {
            _sep = (('' + _sep).trim() + ';').substring(0, 1);
            if (_sep == '_') _sep = ';';
            if (!this.chipsHas(_chips, _val, _sep)) {
                _chips[_chips.length] = _val;
            }
        }
        return this.chipsStr(_chips, _sep);
    }

    // Return array containing chips elements
    chipsArr(_chips, _sep = ';') {
        try {
            if (_chips.constructor != Array) {
                _chips = this.toStr(_chips).trim();
                if (_chips.length > 0) {
                    _sep = (('' + _sep).trim() + ';').substring(0, 1);
                    if (_sep == '_') _sep = ';';
                    _chips = _chips.split(_sep);
                    if (_chips != null) _chips.sort();
                }
                else _chips = null;
            }
        }
        catch {
            _chips = null;
        }
        if (_chips == null) return new Array();
        else return _chips;
    }

    // Return chips string removing value element.
    chipsDel(_chips, _val, _sep = ';') {
        var i, rslt = '', itm;
        _val = this.toStr(_val).trim().toLowerCase();
        if (_val.length > 0) {
            _chips = this.chipsArr(this.toStr(_chips), _sep);
            _sep = (('' + _sep).trim() + ';').substring(0, 1);
            if (_sep == '_') _sep = ';';
            for (i = 0; i < _chips.length; i++) {
                itm = this.toStr(_chips[i]).trim().toLowerCase();
                if (_val != itm) {
                    if (rslt.length > 0) rslt += _sep;
                    rslt += itm;
                }
            }
        }
        return rslt;
    }

    // Return true if chips contain value.
    chipsHas(_chips, _val, _sep = ';') {
        var i = 0, rslt = false;
        _val = this.toStr(_val).trim().toLowerCase();
        if (_val.length > 0) {
            _sep = (('' + _sep).trim() + ';').substring(0, 1);
            if (_sep == '_') _sep = ';';
            _chips = this.chipsArr(this.toStr(_chips), _sep);
            while (!rslt && (i < _chips.length)) {
                rslt = this.toStr(_chips[i]).trim().toLowerCase() == _val;
                i++;
            }
        }
        return rslt;
    }

    // Return chips string containing array elements
    chipsStr(_array, _sep = ';') {
        var i, rslt = '', itm;
        _array = this.chipsArr(_array, _sep);
        _sep = (('' + _sep).trim() + ';').substring(0, 1);
        if (_sep == '_') _sep = ';';
        for (i = 0; i < _array.length; i++) {
            itm = this.toStr(_array[i]).trim();
            if (itm.length > 0) {
                if (rslt.length > 0) rslt += _sep;
                rslt += itm;
            }
        }
        return rslt;
    }

    // Return first string not null or empty string if not found.
    coalesce(_val0, _val1, _val2, _val3, _val4, _val5, _val6, _val7, _val8, _val9,
        _val10, _val11, _val12, _val13, _val14, _val15, _val16, _val17, _val18, _val19) {
        if ((_val0 != undefined) && (_val0 != null)) return _val0;
        else if ((_val1 != undefined) && (_val1 != null)) return _val1;
        else if ((_val2 != undefined) && (_val2 != null)) return _val2;
        else if ((_val3 != undefined) && (_val3 != null)) return _val3;
        else if ((_val4 != undefined) && (_val4 != null)) return _val4;
        else if ((_val5 != undefined) && (_val5 != null)) return _val5;
        else if ((_val6 != undefined) && (_val6 != null)) return _val6;
        else if ((_val7 != undefined) && (_val7 != null)) return _val7;
        else if ((_val8 != undefined) && (_val8 != null)) return _val8;
        else if ((_val9 != undefined) && (_val9 != null)) return _val9;
        else if ((_val10 != undefined) && (_val10 != null)) return _val10;
        else if ((_val11 != undefined) && (_val11 != null)) return _val11;
        else if ((_val12 != undefined) && (_val12 != null)) return _val12;
        else if ((_val13 != undefined) && (_val13 != null)) return _val13;
        else if ((_val14 != undefined) && (_val14 != null)) return _val14;
        else if ((_val15 != undefined) && (_val15 != null)) return _val15;
        else if ((_val16 != undefined) && (_val16 != null)) return _val16;
        else if ((_val17 != undefined) && (_val17 != null)) return _val17;
        else if ((_val18 != undefined) && (_val18 != null)) return _val18;
        else if ((_val19 != undefined) && (_val19 != null)) return _val19;
        else return null;
    }

    // Ritorna 1 se il codice fiscale passato ha il carattere di controllo corretto,
    // -1 se errato o 0 se il codice fiscale è vuoto.
    codiceFiscale(_codicefiscale) {
        var i, s,
            cifre = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
            set1 = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ',
            set2 = 'ABCDEFGHIJABCDEFGHIJKLMNOPQRSTUVWXYZ',
            pari = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ',
            dispari = 'BAKPLCQDREVOSFTGUHMINJWZYX';
        _codicefiscale = _codicefiscale.trim().toUpperCase();
        if (_codicefiscale.length < 1) return 0;
        else if (_codicefiscale.length != 16) return -1;
        {
            for (i = 0; i < 16; i++) {
                if (cifre.indexOf(_codicefiscale.charAt(i)) < 0) return -1;
            }
            s = 0;
            for (i = 1; i <= 13; i += 2) {
                s += pari.indexOf(set2.charAt(set1.indexOf(_codicefiscale.charAt(i))));
            }
            for (i = 0; i <= 14; i += 2) {
                s += dispari.indexOf(set2.charAt(set1.indexOf(_codicefiscale.charAt(i))));
            }
            if (s % 26 != _codicefiscale.charCodeAt(15) - 'A'.charCodeAt(0)) return -1;
            else return 1;
        }
    }

    // Show confirmation box with message. Return true if yes answered.
    confirmDlg(_msg) {
        return confirm(_msg) == true;
    }

    // Expire cookie by name.
    cookieExpire(_cookie) {
        return this.cookieWrite(this.toStr(_cookie), '', -1);
    }

    // Returns value of cookie by name.
    cookieRead(_cookie) {
        if (document.cookie) {
            var id = this.toStr(_cookie) + '=', ar = document.cookie.split(';'), i = 0, c, r = '';
            while ((r == '') && (i < ar.length)) {
                c = ar[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(id) == 0) r = c.substring(id.length, c.length);
                i++;
            }
            return r;
        }
        else return '';
    }

    // Write value on cookie by name with expiration days.
    cookieWrite(_cookie, _val, _days = null) {
        if (document.cookie) {
            var xp = '';
            if (_days != null) {
                var d = new Date();
                d.setTime(d.getTime() + this.toVal(_days) * 86400000);
                xp = '; expires=' + d.toGMTString();
            }
            document.cookie = this.toStr(_cookie) + '=' + this.toStr(_val) + xp + '; path=/';
            return true;
        }
        else return false;
    }

    // Returns new date with year, month and day or convert it from string if only one parameter passed.
    date(_year = null, _month = null, _day = 1, _hours = 0, _minutes = 0, _seconds = 0) {
        try {
            if (_year == null) return new Date();
            else if (_year instanceof Date) return _year;
            else if (typeof _year == "string") {
                var a;
                _year = _year.trim();
                if (_year.length > 0) {
                    a = this.split(_year, '-/.: ;.');
                    if (a.length > 2) {
                        _day = this.toInt(a[0]);
                        _month = this.toInt(a[1]) - 1;
                        _year = this.toInt(a[2]);
                        if (a.length > 3) _hours = this.toInt(a[3]);
                        if (a.length > 4) _minutes = this.toInt(a[4]);
                        if (a.length > 5) _seconds = this.toInt(a[5]);
                        if (_day > 1000) return new Date(_day, _month, _year, _hours, _minutes, _seconds);
                        else if (this.localeString == 'it-IT') return new Date(_year, _month, _day, _hours, _minutes, _seconds);
                        else return new Date(_year, _day, _month, _hours, _minutes, _seconds);
                    }
                    else {
                        _year = Date.parse(_year);
                        if (isNaN(_year)) return null;
                        else return new Date(_year);
                    }
                }
                else return null;
            }
            else if (_month == null) return new Date(_year);
            else return new Date(this.toInt(_year), this.toInt(_month) - 1, this.toInt(_day),
                this.toInt(_hours), this.toInt(_minutes), this.toInt(_seconds));
        }
        catch {
            return null;
        }
    }

    // Returns day value of date.
    dateDay(_date) {
        return _date.getDate();
    }

    // Returns day of week of date.
    dateDayOfWeek(_date) {
        var rslt = _date.getDay();
        if (rslt < 1) return 7;
        else return rslt;
    }

    // Returns true if date is null, invalid or minimum.
    dateEmpty(_date) {
        try {
            var d = this.date(_date);
            if (!(d instanceof Date)) return true;
            return d.getFullYear() < 1000;
        } catch (e) {
            return true;
        }
    }

    // Returns month value of date.
    dateMonth(_date) {
        return _date.getMonth() + 1;
    }

    // Return date as string 
    dateStr(_date, _includeTime = false, _fmt = null) {
        var d = this.date(_date), r = '';
        if (this.empty(_date)) return '';
        else if (d != null) {
            if (d instanceof Date) {
                if (this.empty(_fmt)) _fmt = this.localeString;
                else _fmt = ('' + _fmt).toLowerCase();
                if (_fmt.startsWith('it')) {
                    r = this.padL(d.getDate(), 2, '0') + '/' + this.padL(d.getMonth() + 1, 2, '0') + '/' + this.padL(d.getFullYear(), 4, '0');
                }
                else if (_fmt.startsWith('iso')) {
                    r = this.padL(d.getFullYear(), 4, '0') + '-' + this.padL(d.getMonth() + 1, 2, '0') + '-' + this.padL(d.getDate(), 2, '0');
                }
                else {
                    r = this.padL(d.getMonth() + 1, 2, '0') + '-' + this.padL(d.getDate(), 2, '0') + '-' + this.padL(d.getFullYear(), 4, '0');
                }
                if (_includeTime == true) {
                    if (_fmt.startsWith('iso')) r += 'T';
                    else r += ' ';
                    r += this.padL(d.getHours(), 2, '0') + ':' + this.padL(d.getMinutes(), 2, '0') + ':' + this.padL(d.getSeconds(), 2, '0');
                }
            }
        }
        return r;
    }

    // Return date ticks (milliseconds since 1-1-1970)
    dateTicks(_date) {
        return this.date(_date).getTime();
    }

    // Returns year value of date.
    dateYear(_date) {
        return _date.getFullYear();
    }

    // Returns true if string is null, empty or contains only spaces.
    empty(_val) {
        if (_val === undefined) return true;
        else if (_val == null) return true;
        else if (this.toStr(_val).trim().length < 1) return true;
        else return false;
    }

    // Returns easter date of year.
    easter(_year) {
        var m = 24,
            n = 5,
            a = _year % 19,
            b = _year % 4,
            c = _year % 7,
            d = (19 * a + m) % 30,
            e = (2 * b + 4 * c + 6 * d + n) % 7;
        c = 22 + d + e;
        if (c > 31) return this.date(_year, 4, d + e - 9);
        else return this.date(_year, 3, c);
    }

    // Return true if element selected is enabled or set enabled if specified.
    enabled(_sel, _enabled = null) {
        var pfx = this.classPrefix, self = this;
        _sel = this.select(_sel);
        if (_sel && _sel.length) {
            if (_enabled == null) {
                return !_sel.attr('disabled') && !_sel.hasClass(this.classPrefix + 'disabled');
            }
            else {
                _sel.each(function () {
                    var o = $(this);
                    if (self.toBool(_enabled)) {
                        o.attr('disabled', false);
                        o.removeClass(pfx + 'disabled');
                        self.select("*" + o.attr("id")).each(function () {
                            var oo = $(this);
                            oo.attr('disabled', false);
                            oo.removeClass('disabled');
                            oo.removeClass(pfx + 'disabled');
                        });
                    }
                    else {
                        o.attr('disabled', true);
                        o.addClass(pfx + 'disabled');
                        self.select("*" + o.attr("id")).each(function () {
                            var oo = $(this);
                            oo.attr('disabled', true);
                            oo.addClass('disabled');
                            oo.addClass(pfx + 'disabled');
                        });
                    }
                });
            }
        }
    }

    // Set last error message and code.
    error(_errmsg = null, _errcode = 0) {
        if (_errmsg == null) {
            this.errorMessage = '';
            this.errorCode = _errcode;
        }
        else {
            this.errorMessage = this.toStr(_errmsg);
            this.errorCode = this.toInt(_errcode);
        }
    }

    // Returns value with all carriage-return and tabs replaced by spaces.
    flat(_val) {
        return this.toStr(_val).replaceAll("\t", " ").replaceAll("\r\n", " ").replaceAll("\r", " ").replaceAll("\n", " ");
    }

    // Return date first of month.
    firstOfMonth(_date) {
        _date = this.date(_date);
        return this.date(_date.getFullYear(), _date.getMonth() + 1, 1);
    }

    // Return date first of week.
    firstOfWeek(_date) {
        _date = this.date(_date);
        return this.addDays(_date, 1 - this.dateDayOfWeek(_date));
    }

    // Return date first of year.
    firstOfYear(_date) {
        _date = this.date(_date);
        return this.date(_date.getFullYear(), 1, 1);
    }

    // Returns value formatted.
    format(_val, _fmt, _dateFormat = 'iso') {
        _fmt = this.toStr(_fmt).trim().toUpperCase();
        if (_fmt.startsWith('&')) _fmt = _fmt.substr(1).trim();
        if (this.empty(_fmt)) return this.toStr(_val);
        else if ((typeof _val == 'number') || ('|EU|EUNZ|EUR|EURNZ|NZ|INT|INTNZ|'.indexOf('|' + _fmt + '|') > -1)
            || (_fmt.startsWith('D') && (_fmt.length > 1) && (_fmt != 'DT'))) {
            _val = this.toVal(_val);
            if ((_fmt == 'EU') || (_fmt == 'EUR')) return _val.toLocaleString(this.localeString, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            else if ((_fmt == 'EUNZ') || (_fmt == 'EURNZ')) {
                if (_val == 0) return '';
                else return _val.toLocaleString(this.localeString, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            }
            else if (_fmt == 'NZ') {
                if (_val == 0) return '';
                else return _val.toLocaleString(this.localeString);
            }
            else if (_fmt == 'INT') return Math.trunc(_val).toString();
            else if (_fmt == 'INTNZ') {
                _val = Math.trunc(_val);
                if (_val == 0) return '';
                else return _val.toString();
            }
            else if (_fmt.startsWith('DNZ')) {
                if (_val == 0) return '';
                else return (0 + _val).toLocaleString(this.localeString,
                    {
                        minimumFractionDigits: parseInt(_fmt.substr(3)),
                        maximumFractionDigits: parseInt(_fmt.substr(3))
                    });
            }
            else if (_fmt.startsWith('D')) {
                return (0 + _val).toLocaleString(this.localeString,
                    {
                        minimumFractionDigits: parseInt(_fmt.substr(1)),
                        maximumFractionDigits: parseInt(_fmt.substr(1))
                    });
            }
            else return _val.toLocaleString(this.localeString);
        }
        else if ((_fmt == 'UP') || (_fmt == 'UPPER')) return ('' + _val).toUpperCase();
        else if ((_fmt == 'LOW') || (_fmt == 'LOWER')) return ('' + _val).toLowerCase();
        else if (_fmt == 'D') return this.dateStr(this.date('' + _val), false, _dateFormat);
        else if (_fmt == 'DT') return this.dateStr(this.date('' + _val), true, _dateFormat);
        else return '' + _val;
    }

    // Returns decimal part of number.
    frac(_val) {
        _val = this.toVal(_val);
        return _val - Math.floor(_val);
    }

    // Return object from parsing JSON string.
    fromJson(_json) {
        _json = this.toStr(_json).trim();
        if (_json.length > 0) return JSON.parse(_json);
        else return {};
    }

    // Return object from parsing JSON base 64 string.
    fromJson64(_json64) {
        return this.fromJson(this.base64Decode(_json64));
    }

    // Return value of selected control as string. If specified selected option text will be returned.
    // Alternate value indicate if control is a CHECK the value returned if is not checked (default = '0').
    // If control is a SELECT alternate value indicate the name of attribute to return of selected option.
    get(_sel, _selectOptionText = false, _alternateValue = '0') {
        var ty, id, op;
        _sel = this.select(_sel);
        if (_sel && _sel.length) {
            id = _sel.attr('id');
            ty = ('' + _sel.attr(this.attributePrefix + 'type')).trim().toUpperCase();
            if (ty.startsWith('YES')) {
                if (_sel.is(':checked')) return '1';
                else if ($('#' + id + this.noElementSuffix).is(':checked')) return '0';
                else return '';
            }
            else if (ty == 'CHECK') {
                if (_sel.is(':checked')) return '1';
                else return _alternateValue;
            }
            else if (_selectOptionText && ((ty == 'SELECT') || (ty == 'RELATED'))) {
                op = $('#' + id + ' option:selected');
                if (op && op.length) {
                    if (_alternateValue == '0') return this.toStr(op.text());
                    else return this.toStr(op.attr(_alternateValue));
                }
                else return '';
            }
            else return this.toStr(_sel.val());
        }
        else return '';
    }

    // Return value of attribute of element corresponding to selection.
    getAttr(_sel, _attr) {
        _sel = this.select(_sel);
        if (_sel && _sel.length) return _sel.attr(_attr);
        else return '';
    }

    // Return value of selected control as boolean.
    getBool(_sel) {
        return this.toBool(this.get(_sel));
    }

    // Return value of selected control as date.
    getDate(_sel) {
        return this.date(this.get(_sel));
    }

    // Return DOM element by id or null if not found.
    getDOM(_id) {
        if (document.getElementById) return document.getElementById(_id);
        else return null;
    }

    // Return value of attrib sm-format of element corresponding to selection.
    getFormat(_sel) {
        return this.getAttr(_sel, this.attributePrefix + 'format');
    }

    // Return value of selected control as integer.
    getInt(_sel) {
        return this.toInt(this.get(_sel));
    }

    // Return JSON string with key related value.
    getJson(_json, _key, _default = '') {
        var obj = this.fromJson(_json);
        if (obj == null) return _default;
        else return this.toStr(obj[_key]);
    }

    // Return JSON string with key related value.
    getJson64(_json64, _key, _default = '') {
        var obj = this.fromJson64(_json64);
        if (obj == null) return _default;
        else return this.toStr(obj[_key]);
    }

    // Read state control by id and save value on state property.
    getState(_sel = '#SM_STATE') {
        this.state = this.fromJson64(this.get(this.toStr(_sel)));
    }

    // Return value of attrib sm-type of element corresponding to selection.
    getType(_sel) {
        return ('' + this.getAttr(_sel, this.attributePrefix + 'type')).trim().toUpperCase();
    }

    // Return value of selected control as float.
    getVal(_sel) {
        return this.toVal(this.get(_sel));
    }

    // Evaluate test is true or false and return corresponding parameter.
    iif(_test, _ifTrue, _ifFalse) {
        if (this.toBool(_test)) return _ifTrue;
        else return _ifFalse;
    }

    // Show input dialog box with message and return string typed.
    inputDlg(_msg) {
        return prompt(_msg);
    }

    // Insert new value between start and end substrings.
    insBtw(_val, _new, _start, _end, _ignoreCase = false) {
        var i, a, b;
        _val = this.toStr(_val);
        _new = this.toStr(_new);
        _start = this.toStr(_start);
        _end = this.toStr(_end);
        if (_ignoreCase) i = _val.toLowerCase().indexOf(_start.toLowerCase());
        else i = _val.indexOf(_start);
        if (i > -1) {
            a = this.mid(_val, 0, i) + _start + _new;
            b = this.mid(_val, i + _start.length, _val.length);
            if (_ignoreCase) i = b.toLowerCase().indexOf(_end.toLowerCase());
            else i = b.indexOf(_end);
            if (i > -1) return a + this.mid(b, i, b.length);
            else return _val;
        }
        else return _val + _start + _new + _end;
    }

    // Return lesser integer near to value.
    int(_val) {
        if (_val == null) return 0;
        else return Math.trunc(0 + _val);
    }

    // Return  true if object is a jQuery instance.
    isJQuery(_obj) {
        if (_obj === undefined) return false;
        else if (_obj == null) return false;
        else if (_obj instanceof jQuery) return _obj.length > 0;
        else return false;
    }

    // Return 0 if value is not a JSON object or string, 1 if is a JSON object, 2 if is a JSON string.
    isJSON(_val) {
        try {
            if (_val) {
                if ((typeof _val === 'object') && (_val.constructor === Object)) return 1;
                else {
                    _val = this.toStr(_val);
                    JSON.parse(_val);
                    return 2;
                }
            }
            else return 0;
        }
        catch (e) {
            return 0;
        }
    }

    // Return language code of client browser (it, en, de, fr, nl).
    language() {
        var s = '';
        if (navigator.language) s = navigator.language.toLowerCase();
        else if (navigator.userLanguage) s = navigator.userLanguage.toLowerCase();
        else if (navigator.browserLanguage) s = navigator.browserLanguage.toLowerCase();
        if (s.indexOf('it') > -1) return 'it';
        else if (s.indexOf('en') > -1) return 'en';
        else if (s.indexOf('de') > -1) return 'de';
        else if (s.indexOf('fr') > -1) return 'fr';
        else if (s.indexOf('nl') > -1) return 'nl';
        else return 'en';
    }

    // Return date last of month.
    lastOfMonth(_date) {
        _date = this.date(_date);
        if (_date.getMonth() < 11) return this.addDays(this.date(_date.getFullYear(), _date.getMonth() + 2, 1), -1);
        else return this.date(_date.getFullYear(), 12, 31);
    }

    // Return date last of week.
    lastOfWeek(_date) {
        _date = this.date(_date);
        return this.addDays(_date, 7 - this.dateDayOfWeek(_date));
    }

    // Return date last of year.
    lastOfYear(_date) {
        _date = this.date(_date);
        return this.date(_date.getFullYear(), 12, 31);
    }

    // Returns true if year is a leap year.
    leapYear(_year) {
        return (_year % 4 == 0) && ((_year % 100 != 0) || (_year % 400 == 0));
    }

    // Returns first length characters of string from left.
    left(_val, _len) {
        _val = this.toStr(_val);
        _len = this.toInt(_len);
        if (_len > _val.length) _len = _val.length;
        if (_len > 0) return _val.substr(0, _len);
        else return '';
    }

    //	Returns length of string.
    len(_val) {
        return this.toStr(_val).length;
    }

    // Return string converted to lower-case.
    lower(_val) {
        return this.toStr(_val).toLowerCase();
    }

    // Return value with macro replaced by new value or if macro is an array
    // return value with all macro replaced by content of array ([macro1],[value1]...[macroN],[valueN]).
    macro(_val, _macro, _new = null) {
        var i = 0, h;
        _val = this.toStr(_val);
        if (_val.length > (this.macroPrefix + this.macroSuffix).length) {
            if (_macro.constructor == Array) {
                h = _macro.length;
                while (i < h - 1) {
                    _val = _val.replaceAll(this.macroPrefix + _macro[i] + this.macroSuffix, _macro[i + 1]);
                    i += 2;
                }
            }
            else if (_new != null) _val = _val.replaceAll(this.macroPrefix + _macro + this.macroSuffix, _new);
        }
        return _val;
    }

    // Returns portion of string starting at position index and getting length chars.
    mid(_val, _start, _len = null) {
        if (_len == null) _len = _val.length;
        else _len = this.toInt(_len);
        _val = this.toStr(_val);
        _start = this.toInt(_start);
        if (_val.length > 0) {
            if (_len > 0) {
                if (_start < 0) _start = 0;
                if (_start < _val.length) {
                    if (_start + _len > _val.length) return _val.substr(_start);
                    else return _val.substr(_start, _len);
                }
                else return '';
            }
            else return '';
        }
        else return '';
    }

    // Returns current date-time.
    now() {
        return new Date();
    }

    // Return true if selected control is omitted.
    omitted(_sel, _selectOptionText = false, _ignoreHidden = true) {
        return this.empty(this.get(_sel, _selectOptionText, '')) && (!_ignoreHidden || this.visible(_sel));
    }

    // Set SELECT element options from string or array.
    options(_sel, _options = null) {
        var obj = this.select(_sel), i, q;
        if (obj && obj.length) {
            if (obj.is('select')) {
                obj.empty();
                if (_options != null) {
                    // if an array is passed, fill options with array elements.
                    if (_options.constructor == Array) {
                        for (i = 0; i < _options.length; i++) {
                            obj.append($('<option>', { value: _options[i], text: _options[i] }));
                        }
                    }
                    else {
                        q = this.isJSON(_options);
                        // JSON object passed, fill options with key-values.
                        if (q > 0) {
                            if (q > 1) _options = JSON.parse(_options);
                            for (i = 0; i < _options.length; i++) {
                                obj.append($('<option>', { value: _options[i].value, text: _options[i].text }));
                            }
                        }
                        // string passed, fill options with string elements.
                        else {
                            _options = this.toStr(_options).trim();
                            if (_options.length > 0) {
                                _options = _options.split('|');
                                for (i = 0; i < _options.length; i++) {
                                    obj.append($('<option>', { value: _options[i], text: _options[i] }));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Returns string filled at left with char until length.
    padL(_val, _len, _char = ' ') {
        _val = this.toStr(_val);
        _len = this.toInt(_len);
        _char = this.toStr(_char);
        if (_char.length < 1) _char = ' ';
        else if (_char.length > 1) _char = _char.substr(0, 1);
        while (_val.length < _len) _val = _char + _val;
        return _val;
    }

    // Returns string filled at right with char until length.
    padR(_val, _len, _char = ' ') {
        _val = this.toStr(_val);
        _len = this.toInt(_len);
        _char = this.toStr(_char);
        if (_char.length < 1) _char = ' ';
        else if (_char.length > 1) _char = _char.substr(0, 1);
        while (_val.length < _len) _val = _val + _char;
        return _val;
    }

    // Return current page pathname including origin if specified.
    page(_origin = true) {
        if (_origin) return location.origin + location.pathname;
        else return location.pathname;
    }

    // Return position of substring to find in value.
    pos(_val, _find) {
        return this.toStr(_val).indexOf(this.toStr(_find));
    }

    // Print current page.
    print() {
        window.print();
    }

    // Return base64 string serialization of JSON containing parameters represented
    // in key-values array (i.e. { "Key1", "Value1",... "KeyN","ValueN"}).
    // Function also add following parameters: sm_usr, sm_org, sm_tim, sm_bku.
    Q(_array = null) {
        var r = '';
        //
        if (!this.empty($_SESSION)) r = this.setJson(r, this.elementPrefix + 'ses', this.toStr($_SESSION));
        //
        if (!this.empty($_USERUID)) r = this.setJson(r, this.elementPrefix + 'usr', this.toStr($_USERUID));
        //
        if (!this.empty($_ORGUID)) r = this.setJson(r, this.elementPrefix + 'org', this.toStr($_ORGUID));
        //
        r = this.setJson(r, this.elementPrefix + 'tim', '' + this.int(new Date().getTime() / 1000));
        //
        if (!this.empty($_BACKURL)) r = this.setJson(r, this.elementPrefix + 'bku', this.toStr($_BACKURL));
        //
        if (_array != null) {
            if (_array.constructor == Array) {
                var i = 0;
                while (i < _array.length - 1) {
                    r = this.setJson(r, _array[i], this.toStr(_array[i + 1]));
                    i += 2;
                }
            }
        }
        return this.base64Encode(r);
    }

    // Get query string parameter by name from url (default current).
    queryParam(_name, _search = window.location.search) {
        const p = new URLSearchParams(_search);
        return p.get(_name);
    }

    // Return value with single quote.
    quote(_val) {
        return "'" + _val.replaceAll("'", "''") + "'";
    }

    // Return value with double quote.
    quote2(_val) {
        return '"' + _val.replaceAll('"', '""') + '"';
    }

    // If first selected control is checked set all other control to unchecked.
    radio(_sel, _sel2 = null, _sel3 = null, _sel4 = null,
        _sel5 = null, _sel6 = null, _sel7 = null, _sel8 = null,
        _sel9 = null, _sel10 = null, _sel11 = null, _sel12 = null,
        _sel13 = null, _sel14 = null, _sel15 = null, _sel16 = null) {
        if (this.checked(_sel)) {
            if (_sel2 != null) this.checked(_sel2, false);
            if (_sel3 != null) this.checked(_sel3, false);
            if (_sel4 != null) this.checked(_sel4, false);
            if (_sel5 != null) this.checked(_sel5, false);
            if (_sel6 != null) this.checked(_sel6, false);
            if (_sel7 != null) this.checked(_sel7, false);
            if (_sel8 != null) this.checked(_sel8, false);
            if (_sel9 != null) this.checked(_sel9, false);
            if (_sel10 != null) this.checked(_sel10, false);
            if (_sel11 != null) this.checked(_sel11, false);
            if (_sel12 != null) this.checked(_sel12, false);
            if (_sel13 != null) this.checked(_sel13, false);
            if (_sel14 != null) this.checked(_sel14, false);
            if (_sel15 != null) this.checked(_sel15, false);
            if (_sel16 != null) this.checked(_sel16, false);
        }
    }

    // Redirect to url.
    redir(url) {
        if (window.location) window.location = this.toStr(url);
        else document.location = this.toStr(url);
    }

    // Reload current page.
    reload() {
        if (location) location.reload();
    }

    // Returns string replacing all old string occurrences with new string.
    replace(_val, _old, _new) {
        return this.toStr(_val).replaceAll(this.toStr(_old), this.toStr(_new));
    }

    // Returns last length characters (from right) of string.
    right(_val, _len) {
        _val = this.toStr(_val);
        _len = this.toInt(_len);
        if (_len < 1) return '';
        else if (_val.length > _len) return _val.substr(_val.length - _len, _len);
        else return _val;
    }

    // Returns random number between 0 and _val.
    rnd(_val) {
        return Math.floor(Math.random() * (this.toVal(_val) + 1));
    }

    // Return portion of string from index for length. If portion 
    // exceed string size, string will be considered in circular mode.
    rot(_string, _index, _length = 1) {
        var r = '';
        if ((_string != null) && (_length > 0)) {
            if (_string.length > 0) {
                _index = this.rotLength(_index, _string.length);
                while (_length > 0) {
                    _length--;
                    r += _string.substr(_index, 1);
                    _index++;
                    if (_index >= _string.length) _index = 0;
                }
            }
        }
        return r;
    }

    // Encrypt string with password applying rotational Caesar cypher algorithm.
    rotEncrypt(_string, _password) {
        var a = this.baseChars + this.baseSymbols + this.baseQuotes,
            c, i, j, k, q, z = 0, r = '';
        if (_string != null) {
            if (_string.length > 0) {
                if (_password == null) r = _string;
                else if (_password.length < 1) r = _string;
                else {
                    // calculate password base offset
                    z = _password.length;
                    for (i = 0; i < _password.length; i++) z += i * a.indexOf(_password.substr(i, 1));
                    z = z % a.length;
                    // encrypting loop
                    j = 0;
                    for (i = 0; i < _string.length; i++) {
                        c = _string.substr(i, 1);
                        q = a.indexOf(c);
                        if (q < 0) r += c;
                        else {
                            k = a.indexOf(_password.substr(j, 1));
                            z += k;
                            r += this.rot(a, q + z, 1);
                            z += q;
                            //
                            j++;
                            if (j >= _password.length) j = 0;
                        }
                    }
                }
            }
        }
        return r;
    }

    // Decrypt string with password applying rotational Caesar cypher algorithm.
    rotDecrypt(_string, _password) {
        var a = this.baseChars + this.baseSymbols + this.baseQuotes,
            c, i, j, k, q, z = 0, r = '';
        if (_string != null) {
            if (_string.length > 0) {
                if (_password == null) r = _string;
                else if (_password.length < 1) r = _string;
                else {
                    // calculate password base offset
                    z = _password.length;
                    for (i = 0; i < _password.length; i++) z += i * a.indexOf(_password.substr(i, 1));
                    z = z % a.length;
                    // decrypting loop
                    j = 0;
                    for (i = 0; i < _string.length; i++) {
                        c = _string.substr(i, 1);
                        q = a.indexOf(c);
                        if (q < 0) r += c;
                        else {
                            k = a.indexOf(_password.substr(j, 1));
                            z += k;
                            c = this.rot(a, q - z, 1);
                            r += c;
                            z += a.indexOf(c);
                            //
                            j++;
                            if (j >= _password.length) j = 0;
                        }
                    }
                }
            }
        }
        return r;
    }

    // Return length normalized with rotational module.
    rotLength(_length, _module) {
        while (_length < 0) _length += _module;
        return _length % _module;
    }

    // Return object by jquery selector or by following special chars:
    // !{id} or ?{id} --> sm-id="{id}"
    // @{alias} --> sm-alias="{alias}"
    // ${field} --> sm-field="{field}"
    // *{field} --> sm-for="{field}"
    // *ERR{id} --> sm-for-error="{id}"
    // *LBL{id} --> sm-for-label="{id}"
    // *VAL{id} --> sm-for-validate="{id}"
    select(_sel) {
        if (_sel === undefined) return null;
        else if (_sel == null) return null;
        else if (_sel instanceof jQuery) return _sel;
        else {
            _sel = ('' + _sel).trim();
            if (_sel.length < 1) return '';
            else if (_sel.startsWith('!') || _sel.startsWith('?')) {
                _sel = "[" + this.attributePrefix + "id='" + this.elementPrefix + + _sel.substr(1) + "']";
            }
            else if (_sel.startsWith('@')) {
                _sel = "[" + this.attributePrefix + "alias='" + _sel.substr(1) + "']";
            }
            else if (_sel.startsWith('$')) {
                _sel = "[" + this.attributePrefix + "field='" + _sel.substr(1) + "']";
            }
            else if (_sel.startsWith('*ERR')) {
                _sel = "[" + this.attributePrefix + "id='" + _sel.substr(4) + "_err']";
            }
            else if (_sel.startsWith('*LBL')) {
                _sel = "[" + this.attributePrefix + "id='" + _sel.substr(4) + "_lbl']";
            }
            else if (_sel.startsWith('*CTN')) {
                _sel = "[" + this.attributePrefix + "id='" + _sel.substr(4) + "_ctn']";
            }
            else if (_sel.startsWith('*')) {
                _sel = "[" + this.attributePrefix + "for='" + _sel.substr(1) + "']";
            }
            return $(_sel);
        }
    }

    // Set value of selected control and related hidden base-64 element.
    set(_sel, _val) {
        var b64, id, no, ty;
        _sel = this.select(_sel);
        if (_sel && _sel.length) {
            id = _sel.attr('id');
            b64 = $('#' + id + this.base64Suffix);
            ty = ('' + _sel.attr(this.attributePrefix + 'type')).trim().toUpperCase();
            if (ty.startsWith('YES')) {
                no = $('#' + id + this.noElementSuffix);
                if (this.empty(_val)) {
                    _sel.prop('checked', false);
                    no.prop('checked', false);
                    if (b64 && b64.length) b64.val(this.base64Encode(''));
                }
                else if (this.toBool(_val)) {
                    _sel.prop('checked', true);
                    no.prop('checked', false);
                    if (b64 && b64.length) b64.val(this.base64Encode('1'));
                }
                else {
                    _sel.prop('checked', false);
                    no.prop('checked', true);
                    if (b64 && b64.length) b64.val(this.base64Encode('0'));
                }
            }
            else if (ty.startsWith('CHECK')) {
                if (this.toBool(_val)) {
                    _sel.prop('checked', true);
                    if (b64 && b64.length) b64.val(this.base64Encode('1'));
                }
                else {
                    _sel.prop('checked', false);
                    if (b64 && b64.length) b64.val(this.base64Encode('0'));
                }
            }
            else {
                no = _sel.attr(this.attributePrefix + 'format');
                if (ty.startsWith('NUM') && this.empty(no)) no = 'NZ';
                _val = this.format(_val, no);
                _sel.val(_val);
                if (b64 && b64.length) b64.val(this.base64Encode(_val));
            }
            return true;
        }
        else return false;
    }

    // Set hidden base-64 element value as related control.
    setBase64(_sel) {
        var b64;
        _sel = this.select(_sel);
        if (_sel && _sel.length) {
            b64 = $('#' + _sel.attr('id') + this.base64Suffix);
            if (b64 && b64.length) b64.val(this.base64Encode(this.get(_sel)));
        }
    }

    // Set datetime value of selected control and related hidden base-64 element.
    setDate(_sel, _val, _includeTime = false) {
        this.set(_sel, this.dateStr(this.date(_val), _includeTime));
    }

    // Set integer value of selected control and related hidden base-64 element.
    setInt(_sel, _val) {
        this.set(_sel, this.toStr(this.toInt(_val)));
    }

    // Return JSON string with key setted to value.
    setJson(_json, _key, _val) {
        var obj = this.fromJson(_json);
        if (obj == null) {
            obj = {};
            obj[this.toStr(_key)] = _val;
        }
        else obj[this.toStr(_key)] = _val;
        return this.toJson(obj);
    }

    // Return JSON string with key setted to value.
    setJson64(_json64, _key, _val) {
        var obj = this.fromJson64(_json64);
        if (obj == null) {
            obj = {};
            obj[this.toStr(_key)] = _val;
        }
        else obj[this.toStr(_key)] = _val;
        return this.toJson64(obj);
    }

    // Write key value to state and update control by id.
    setState(_key, _val, _sel = '#SM_STATE') {
        this.state[this.toStr(_key)] = this.toStr(_val);
        this.set(this.toStr(_sel), this.toJson64(this.state));
    }

    // Return random shuffled string blocks.
    shuffle(_string, _blockLength = 1) {
        var i, h, r = '';
        if (_string != null) {
            h = this.toInt(_string.length / _blockLength);
            while (_string.length > 0) {
                i = this.rnd(h - 1);
                r += this.mid(_string, i * _blockLength, _blockLength);
                _string = this.mid(_string, 0, i * _blockLength) + this.mid(_string, (i + 1) * _blockLength);
            }
        }
        return r;
    }

    // Split value in to array elements detected by separators.
    split(_val, _separators = ';') {
        var r = new Array(), i, j = 0, c, q = '';
        if (_val != null) {
            _val = '' + _val;
            for (i = 0; i < _val.length; i++) {
                c = _val.charAt(i);
                if (_separators.indexOf(c) > -1) {
                    r[j] = q;
                    j++;
                    q = '';
                }
                else q += c;
            }
            r[j] = q;
        }
        return r;
    }

    // Return sum of not null values.
    sum(_val0, _val1, _val2, _val3, _val4, _val5, _val6, _val7, _val8, _val9, _val10, _val11, _val12, _val13, _val14, _val15,
        _val16, _val17, _val18, _val19, _val20, _val21, _val22, _val23, _val24, _val25, _val26, _val27, _val28, _val29, _val30, _val31,
        _val32, _val33, _val34, _val35, _val36, _val37, _val38, _val39, _val40, _val41, _val42, _val43, _val44, _val45, _val46, _val47,
        _val48, _val49, _val50, _val51, _val52, _val53, _val54, _val55, _val56, _val57, _val58, _val59, _val60, _val61, _val62, _val63) {
        return this.toVal(_val0) + this.toVal(_val1) + this.toVal(_val2) + this.toVal(_val3) + this.toVal(_val4)
            + this.toVal(_val5) + this.toVal(_val6) + this.toVal(_val7) + this.toVal(_val8) + this.toVal(_val9)
            + this.toVal(_val10) + this.toVal(_val11) + this.toVal(_val12) + this.toVal(_val13) + this.toVal(_val14)
            + this.toVal(_val15) + this.toVal(_val16) + this.toVal(_val17) + this.toVal(_val18) + this.toVal(_val19)
            + this.toVal(_val20) + this.toVal(_val21) + this.toVal(_val22) + this.toVal(_val23) + this.toVal(_val24)
            + this.toVal(_val25) + this.toVal(_val26) + this.toVal(_val27) + this.toVal(_val28) + this.toVal(_val29)
            + this.toVal(_val30) + this.toVal(_val31) + this.toVal(_val32) + this.toVal(_val33) + this.toVal(_val34)
            + this.toVal(_val35) + this.toVal(_val36) + this.toVal(_val37) + this.toVal(_val38) + this.toVal(_val39)
            + this.toVal(_val40) + this.toVal(_val41) + this.toVal(_val42) + this.toVal(_val43) + this.toVal(_val44)
            + this.toVal(_val45) + this.toVal(_val46) + this.toVal(_val47) + this.toVal(_val48) + this.toVal(_val49)
            + this.toVal(_val50) + this.toVal(_val51) + this.toVal(_val52) + this.toVal(_val53) + this.toVal(_val54)
            + this.toVal(_val55) + this.toVal(_val56) + this.toVal(_val57) + this.toVal(_val58) + this.toVal(_val59)
            + this.toVal(_val60) + this.toVal(_val61) + this.toVal(_val62) + this.toVal(_val63);
    }

    // Convert value to boolean.
    toBool(_val) {
        try {
            if (_val === undefined) return false;
            else if (_val == null) return false;
            else if (typeof _val == 'boolean') return _val;
            else {
                _val = (this.toStr(_val).trim() + '0').toUpperCase().charAt(0);
                return (_val == '1') || (_val == 'T') || (_val == 'Y') || (_val == 'V') || (_val == 'S') || (_val == '+');
            }
        }
        catch {
            return false;
        }
    }

    // Return value with esplicit HTML entities.
    toHtml(_val, _notIfStartWith = null) {
        try {
            if (_val === undefined) return '';
            else if (_val == null) return '';
            else {
                _val = this.toStr(_val);
                if (_notIfStartWith != null) {
                    if (_val.startsWith(_notIfStartWith)) {
                        if (_val.length > _notIfStartWith.length) return _val.substr(_notIfStartWith.length);
                        else return '';
                    }
                }
                if (_val.trim().length > 0) {
                    return _val.replace(/[\u00A0-\u9999<>\&]/g, function (i) {
                        return '&#' + i.charCodeAt(0) + ';';
                    }).replaceAll('"', '&quot;').replaceAll("'", '&apos;');
                }
                else return _val;
            }
        }
        catch {
            return '';
        }
    }

    // Convert to integer value.
    toInt(_val) {
        return Math.floor(this.toVal(_val));
    }

    // Return object converted to JSON string.
    toJson(_obj) {
        try {
            if (_obj === undefined) return '[]';
            else if (_obj == null) return '[]';
            else if (typeof _obj === 'object') return JSON.stringify(_obj);
            else return JSON.stringify({ _obj });
        }
        catch {
            return '[]';
        }
    }

    // Return object converted to JSON string base 64 encoded.
    toJson64(_obj) {
        return this.base64Encode(this.toJson(_obj));
    }

    // Convert value to string.
    toStr(_val) {
        try {
            if (_val === undefined) return '';
            else if (_val == null) return '';
            else if (typeof _val == 'number') return _val.toLocaleString(this.localeString).replaceAll(this.thousandsSeparator, '');
            else if (_val instanceof jQuery) return '' + _val.val();
            else return _val.toString();
        }
        catch {
            return '';
        }
    }

    // Convert to float value.
    toVal(_val) {
        try {
            if (_val === undefined) return 0;
            else if (_val == null) return 0;
            else if (typeof _val == 'number') return _val;
            else {
                _val = parseFloat(this.toStr(_val).replaceAll(this.thousandsSeparator, '').replaceAll(this.decimalPoint, '.'));
                if (isNaN(_val)) return 0;
                else return _val;
            }
        }
        catch {
            return 0;
        }
    }

    // Returns value trimming all occurrences of string at begin or end.
    trim(_val, _str = ' ') {
        _val = this.toStr(_val);
        _str = this.toStr(_str);
        while (this.left(_val, _str.length) == _str) _val = this.right(_val, _val.length - _str.length);
        while (this.right(_val, _str.length) == _str) _val = this.left(_val, _val.length - _str.length);
        return _val;
    }

    // Returns value trimming all occurrences of string at start.
    trimStart(_val, _str = ' ') {
        _val = this.toStr(_val);
        _str = this.toStr(_str);
        while (this.left(_val, _str.length) == _str) _val = this.right(_val, _val.length - _str.length);
        return _val;
    }

    // Returns value trimming all occurrences of string at end.
    trimEnd(_val, _str = ' ') {
        _val = this.toStr(_val);
        _str = this.toStr(_str);
        while (this.right(_val, _str.length) == _str) _val = this.left(_val, _val.length - _str.length);
        return _val;
    }

    // Returns UID value
    uid() {
        var c, i = 0, r, u = '', v;
        try { u = this.toStr(crypto.randomUUID()); }
        catch { u = ''; }
        if (this.empty(u)) {
            u = '';
            while (i++ < 36) {
                c = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'[i - 1];
                r = Math.random() * 16 | 0;
                v = c == 'x' ? r : (r & 0x3 | 0x8);
                u += (c == '-' || c == '4') ? c : v.toString(16)
            }
        }
        return u;
    }

    // Returns value without HTML tags.
    untag(_val) {
        return (this.toStr(_val)).replace(/<[^>]*>/g, ' ');
    }

    // Return string converted to upper-case.
    upper(_val) {
        return this.toStr(_val).toUpperCase();
    }

    // If test is false error message will be displayed at control selected.
    validate(_sel, _test, _err) {
        var o = this.select(_sel);
        if (o && o.length) {
            o = this.select('*ERR' + o.attr('id'));
            if (o && o.length) {
                if (_test) {
                    o.text('');
                    o.addClass('sm-hidden');
                }
                else {
                    o.text(this.toStr(_err));
                    o.removeClass('sm-hidden');
                }
            }
        }
        return _test;
    }

    // Return true if element selected is visible or set visibility if specified.
    visible(_sel, _visible = null) {
        var pfx = this.classPrefix, self = this;
        _sel = this.select(_sel);
        if (_sel && _sel.length) {
            if (_visible == null) {
                var r = true, id = '';
                while (r && (_sel && _sel.length)) {
                    id = _sel.attr('id');
                    if (!_sel.is('body') && (id != this.mainContainer)) {
                        if ((_sel.css('display') == 'none') || _sel.hasClass(pfx + 'hidden')) r = false;
                        _sel = _sel.parent();
                    }
                    else break;
                }
                return r;
            }
            else {
                _sel.each(function () {
                    var o = $(this);
                    if (self.toBool(_visible)) {
                        o.removeClass(pfx + 'hidden');
                        self.select('*' + _sel.attr('id')).each(function () {
                            $(this).removeClass(pfx + 'hidden');
                        });
                        return true;
                    }
                    else {
                        o.addClass(pfx + 'hidden');
                        self.select('*' + _sel.attr('id')).each(function () {
                            $(this).addClass(pfx + 'hidden');
                        });
                        return false;
                    }
                });
            }
        }
    }

    // Stop execution for specified seconds.
    waitSecs(_val) {
        var r = 0;
        _val = Math.floor(this.toVal(_val) * 1000) + (new Date()).getTime();
        while ((new Date()).getTime() < _val) r++;
        return r;
    }

    // Show warning message box.
    warningDlg(_msg) {
        alert(_msg);
    }

    // Returns year with 2 digit to 4 fitted to next 30 years or 70 previous year.
    yearFit(_year) {
        if (_year > 99) return _year;
        else if (_year > this.year2DigitLeap) return 2000 + _year - 100;
        else return this.year2DigitCentury + _year;
    }

}

/*  ===========================================================================
 *  Globals
 *  ===========================================================================
 */

// SMCode support library main instance.
var SM = new SMCode();