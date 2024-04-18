/*  ===========================================================================
 *  
 *  File:       smcode.js
 *  Version:    2.0.10
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode javascript support library.
 *  
 *  MIT License
 *  ===========
 *  SMCode Javascript Rapid Application Development Code Library
 *  Copyright (c) 2010-2024 Stefano Mengarelli - All rights reserved.
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

    // Decimal point.
    decimalPoint = ',';

    // Last error code.
    errorCode = 0;

    // Last error message.
    errorMessage = '';

    // Current state JSON.
    state = {};

    // Thousands separator. 
    thousandsSeparator = '.';

    // Instance constructor.
    constructor() {

    }

    // Returns absolute value of number n.
    abs(_val) {
        _val = this.toVal(_val);
        if (_val < 0) return -_val;
        else return _val;
    }

    // Returns part of string after first recourrence of sub string.
    // If sub string is not present returns empty string.
    after(_val, _find) {
        var i;
        _val = this.toStr(_val);
        _find = this.toStr(_find);
        i = _val.indexOf(_find);
        if (i < _val.length - _find.length) return _val.substr(i + _find.length);
        else return "";
    }

    // Return decoded base 64 value.
    base64Decode(_val) {
        if (_val) {
            if (('' + _val).trim().length > 0) return decodeURIComponent(escape(window.atob(_val)));
            else return '';
        }
        else return '';
    }

    // Return value encoded base 64.
    base64Encode(_val) {
        if (_val) {
            if (('' + _val).trim().length > 0) return window.btoa(unescape(encodeURIComponent(_val)));
            else return '';
        }
        else return '';
    }

    // Returns part of string before first recurrence of substring.
    // If substring is not present returns empty string.
    before(_val, _find) {
        var i;
        _val = '' + _val;
        _find = this.toStr(_find);
        i = _val.indexOf(_find);
        if (i > 0) return _val.substr(0, i);
        else return "";
    }

    // Returns part of string between start and end substrings.
    btw(_val, _start, _end, _ignoreCase = false) {
        var r = '', s, i;
        if (_ignoreCase) i = _val.toLowerCase().indexOf(_start.toLowerCase());
        else i = _val.indexOf(_start);
        if (i > -1) {
            s = this.mid(_val, i + _start.length);
            if (_ignoreCase) i = s.toLowerCase().indexOf(_end.toLowerCase());
            else i = s.indexOf(_end);
            if (i > -1) r = this.mid(s, 0, i);
        }
        return r;
    }

    // Returns string passed adding new string divided by separator.
    cat(_val, _new, _separator = '') {
        _val = this.toStr(_val);
        _new = this.toStr(_new);
        if (_new.length < 1) return _val;
        else if (_val.length > 0) return _val + this.toStr(_separator) + _new;
        else return _new;
    }

    // Return first string not null or empty string if not found.
    coalesce(_p0, _p1, _p2, _p3, _p4, _p5, _p6, _p7, _p8, _p9, _p10, _p11, _p12, _p13, _p14, _p15) {
        if ((_p0 != undefined) && (_p0 != null)) return _p0;
        else if ((_p1 != undefined) && (_p1 != null)) return _p1;
        else if ((_p2 != undefined) && (_p2 != null)) return _p1;
        else if ((_p3 != undefined) && (_p3 != null)) return _p1;
        else if ((_p4 != undefined) && (_p4 != null)) return _p1;
        else if ((_p5 != undefined) && (_p5 != null)) return _p1;
        else if ((_p6 != undefined) && (_p6 != null)) return _p1;
        else if ((_p7 != undefined) && (_p7 != null)) return _p1;
        else if ((_p8 != undefined) && (_p8 != null)) return _p1;
        else if ((_p9 != undefined) && (_p9 != null)) return _p1;
        else if ((_p10 != undefined) && (_p10 != null)) return _p1;
        else if ((_p11 != undefined) && (_p11 != null)) return _p1;
        else if ((_p12 != undefined) && (_p12 != null)) return _p1;
        else if ((_p13 != undefined) && (_p13 != null)) return _p1;
        else if ((_p14 != undefined) && (_p14 != null)) return _p1;
        else if ((_p15 != undefined) && (_p15 != null)) return _p1;
    }

    // Expire cookie by name.
    cookieExpire(_cookie) {
        return cookieWrite(_cookie, '', -1);
    }

    // Returns value of cookie by name.
    cookieRead(_cookie) {
        _cookie = this.toStr(_cookie);
        if (document.cookie) {
            var id = _cookie + '=', ar = document.cookie.split(';'), i = 0, c, r = '';
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
    cookieWrite(_cookie, _val, _days) {
        _cookie = this.toStr(_cookie);
        _val = this.toVal(_val);
        _days = this.toVal(_days);
        if (document.cookie) {
            var xp = '';
            if (_days) {
                var d = new Date();
                d.setTime(d.getTime() + _days * 86400000);
                xp = '; expires=' + d.toGMTString();
            }
            document.cookie = _cookie + '=' + _val + xp + '; path=/';
        }
        return void (0);
    }

    // Returns new date with year, month and day.
    date(_year, _month, _day, _hours = 0, _minutes = 0, _seconds = 0) {
        return new Date(this.toVal(_year), this.toVal(_month), this.toVal(_day),
            _hours, _minutes, _seconds);
    }

    // Returns day value of date.
    dateDay(_date) {
        return _date.getDate();
    }

    // Returns day of week of date.
    dateDayOfWeek(_date) {
        var r = _date.getDay();
        if (r < 1) return 7;
        else return r;
    }

    // Returns month value of date.
    dateMonth(_date) {
        return _date.getMonth() + 1;
    }

    // Returns year value of date.
    dateYear(_date) {
        return _date.getFullYear();
    }

    // Returns true if string is null, empty or contains only spaces.
    empty(_val) {
        if (val === undefined) return true;
        else if (val == null) return true;
        else if (this.toStr(val).trim().length < 1) return true;
        else return false;
    }

    // Returns value with all carriage-return and tabs replaced by spaces.
    flat(_val) {
        _val = _val.replaceAll("\t", " ").replaceAll("\r\n", " ").replaceAll("\r", " ").replaceAll("\n", " ");
    }

    // Returns decimal part of number.
    frac(_val) {
        _val = this.toVal(_val);
        return _val - Math.floor(_val);
    }

    // Return object from parsing JSON string.
    fromJson(_json) {
        if (_json) {
            if (_json.length < 1) return null;
            else return JSON.parse(_json);
        }
        else return null;
    }

    // Return object from parsing JSON base 64 string.
    fromJson64(_json64) {
        return this.fromJson(this.base64Decode(_json64));
    }

    // Return DOM element by id or null if not found.
    getDOMElement(_id) {
        if (document.getElementById) {
            return document.getElementById(_id);
        }
        else return null;
    }

    // Return value of attribute of element corresponding to selection.
    getAttr(_sel, _attr) {
        if (!this.isJQuery(_sel)) {
            _sel = this.select(_sel);
        }
        if (_sel && _sel.length) return o.attr(_attr);
        else return '';
    }

    // Return value of attrib sm-format of element corresponding to selection.
    getFormat(_sel) {
        return this.getAttr(sel, 'sm-format');
    }

    // Return value of attrib sm-type of element corresponding to selection.
    getType(_sel) {
        return this.getAttr(sel, 'sm-type');
    }

    // Evaluate test is true or false and return corresponding parameter.
    iif(_test, _ifTrue, _ifFalse) {
        if (test == true) return _ifTrue;
        else return _ifFalse;
    }

    // Insert new value between start and end substrings.
    insBtw(_val, _new, _start, _end, _ignoreCase = false) {
        var i, a, b;
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
        else return _val + _start + newstring + _end;
    }

    // Returns integer part of number.
    int(_val) {
        return Math.floor(_val);
    }

    // Return  true if object is a jQuery instance.
    isJQuery(_obj) {
        if (_obj === undefined) return false;
        else if (_obj == null) return false;
        else return _obj instanceof jQuery;
    }

    // Return language code of client browser (it, en, de, fr, nl).
    language() {
        var s = "";
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

    // Returns first length characters of string from left.
    left(_val, _len) {
        _val = this.toStr(_val);
        _len = this.toVal(_len);
        if (_len > _val.length) _len = _val.length;
        if (_len > 0) return _val.substr(0, _len);
        else return "";
    }

    //	Returns length of string.
    len(_val) {
        return this.toStr(_val).length;
    }

    // Return string converted to lower-case.
    lower(_val) {
        return this.toStr(_val).toLowerCase();
    }

    // Returns portion of string starting at position index and getting length chars.
    mid(_val, _start, _len = null) {
        if (_len == null) _len = _val.length;
        else _len = this.toVal(_len);
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

    // Returns string filled at left with char until length.
    padL(_val, _len, _char = ' ') {
        _val = this.toStr(_val);
        _len = this.toVal(_len);
        _char = this.toStr(_char);
        if (_char.length < 1) _char = ' ';
        else if (_char.length > 1) _char = _char.substr(0, 1);
        while (_val.length < _len) _val = _char + _val;
        return _val;
    }

    // Returns string filled at right with char until length.
    padR(_val, _len, _char = ' ') {
        _val = this.toStr(_val);
        _len = this.toVal(_len);
        _char = this.toStr(_char);
        if (_char.length < 1) _char = ' ';
        else if (_char.length > 1) _char = _char.substr(0, 1);
        while (_val.length < _len) _val = _val + _char;
        return _val;
    }

    // Return position of substring to find in value.
    pos(_val, _find) {
        return this.toStr(_val).indexOf(this.toStr(_find));
    }

    // Print current page.
    print() {
        window.print();
    }

    // Return value with single quote.
    quote(_val) {
        return "'" + _val.replaceAll("'", "''") + "'";
    }

    // Return value with double quote.
    quote2(_val) {
        return '"' + _val.replaceAll('"', '""') + '"';
    }

    // Redirect to url.
    redir(url) {
        if (window.location) window.location = url;
        else document.location = url;
    }

    // Reload current page.
    reload() {
        if (location) location.reload();
    }

    // Returns string replacing all old string occurrences with new string.
    replace(_val, _old, _new) {
        return this.toStr(_val).replaceAll(_old, _new);
    }

    // Returns last length characters (from right) of string.
    right(_val, _len) {
        _val = this.toStr(_val);
        _len = this.toVal(_len);
        if (_len < 1) return "";
        else if (_val.length > _len) return _val.substr(_val.length - _len, _len);
        else return _val;
    }

    // Returns random number between 0 and _val.
    rnd(_val) {
        return Math.floor(Math.random() * (this.toVal(_val) + 1));
    }

    // Return object by jquery selector or by following special chars:
    // !{id} or ?{id} --> sd-id="{id}"
    // @{alias} --> sd-alias="{alias}"
    // ${field} --> sd-field="{field}"
    select(_sel) {
        _sel = this.toStr(_sel).trim();
        if (_sel.startsWith('!') || _sel.startsWith('?')) {
            _sel = "[sm-id='" + _sel.substr(1) + "']";
        }
        else if (_sel.startsWith('@')) {
            _sel = "[sm-alias='" + _sel.substr(1) + "']";
        }
        else if (_sel.startsWith('$')) {
            _sel = "[sm-field='" + _sel.substr(1) + "']";
        }
        return $(_sel);
    }

    // Return value with esplicit HTML entities.
    toHtml(_val, _notIfStartWith = null) {
        if (_val) {
            _val = this.ToSt(_val);
            if (_notIfStartWith != null) {
                if (_val.startsWith(_notIfStartWith)) {
                    if (_val.length > _notIfStartWith.length) return _val.substr(_notIfStartWith.length);
                    else return '';
                }
            }
            if (_val.trim().length > 0) {
                return _val.replace(/[\u00A0-\u9999<>\&]/g, function (i) {
                    return '&#' + i.charCodeAt(0) + ';';
                });
            }
            else return _val;
        }
        else return '';
    }

    // Convert to integer value.
    toInt(_val) {
        return Math.floor(this.toVal(_val));
    }

    // Return object converted to JSON string.
    toJson(_obj) {
        if (typeof _obj === 'object') return JSON.stringify(_obj);
        else if (_object != null) return JSON.stringify({ _obj });
        else return '[]';
    }

    // Return object converted to JSON string base 64 encoded.
    toJson64(_obj) {
        return this.base64Encode(this.toJson(_obj));
    }

    // Convert value to string.
    toStr(_val) {
        if (_val === undefined) return '';
        else if (_val == null) return '';
        else if (_val instanceof jQuery) return '' + _val.val();
        else return '' + _val;
    }

    // Convert to float value.
    toVal(_val) {
        try {
            var r = parseFloat(this.toStr(_val).replaceAll(this.thousandsSeparator, '').replaceAll(this.decimalPoint, '.'));
            if (isNaN(r)) return 0;
            else return r;
        }
        catch {
            return 0;
        }
    }

    // Returns value trimming all occurrences of string at begin or end.
    trim(_val, _str = ' ') {
        while (this.left(_val, _str.length) == _str) _val = this.right(_val, _val.length - _str.length);
        while (this.right(_val, _str.length) == _str) _val = this.left(_val, _val.length - _str.length);
        return _val;
    }

    // Returns value trimming all occurrences of string at start.
    trimStart(_val, _str = ' ') {
        while (this.left(_val, _str.length) == _str) _val = this.right(_val, _val.length - _str.length);
        return _val;
    }

    // Returns value trimming all occurrences of string at end.
    trimEnd(_val, _str = ' ') {
        while (this.right(_val, _str.length) == _str) _val = this.left(_val, _val.length - _str.length);
        return _val;
    }

    // Returns value without HTML tags.
    untag(_val) {
        return ('' + _val).replace(/<[^>]*>/g, ' ');
    }

    // Return string converted to upper-case.
    upper(_val) {
        return this.toStr(_val).toUpperCase();
    }

    // Stop execution for specified seconds.
    waitSecs(_val) {
        _val = Math.floor(this.toVal(_val) * 1000) + (new Date()).getTime();
        while ((new Date()).getTime() < s) {
            // nop
        }
    }

}

/*  ===========================================================================
 *  Globals
 *  ===========================================================================
 */

// SMCode support library main instance
var SM = new SMCode();