/*  ===========================================================================
 *  
 *  File:       smcode.js
 *  Version:    2.0.28
 *  Date:       June 2024
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
        var r = this.toVal(_val);
        if (r < 0) r = -r;
        return r;
    }

    // Returns part of string after first recourrence of sub string.
    // If sub string is not present returns empty string.
    after(_val, _find) {
        var i, r = '';
        _val = this.toStr(_val);
        _find = this.toStr(_find);
        i = _val.indexOf(_find);
        if ((i > -1) && (i < _val.length - _find.length)) {
            r = _val.substring(i + _find.length);
        }
        return r;
    }

    // Return value of attribute of element corresponding to selection.
    attr(_sel, _attr, _val = null) {
        var o = this.select(_sel), r = '';
        if (o && o.length) {
            if (_val == null) r = this.toStr(o.attr(_attr));
            else {
                _val = this.toStr(_val);
                o.attr(_attr, _val);
                r = _val;
            }
        }
        return r;
    }

    // Return decoded base 64 value.
    base64Decode(_val) {
        var r = '';
        _val = this.toStr(_val);
        if (_val.trim().length > 0) {
            r = decodeURIComponent(escape(window.atob(_val)));
        }
        return r;
    }

    // Return value encoded base 64.
    base64Encode(_val) {
        var r = '';
        _val = this.toStr(_val);
        if (_val.trim().length > 0) {
            r = window.btoa(unescape(encodeURIComponent(_val)));
        }
        return r;
    }

    // Returns part of string before first recurrence of substring.
    // If substring is not present returns empty string.
    before(_val, _find) {
        var i, r = '';
        _val = this.toStr(_val);
        _find = this.toStr(_find);
        i = _val.indexOf(_find);
        if (i > -1) r = _val.substring(0, i);
        return r;
    }

    // Returns part of string between start and end substrings.
    btw(_val, _start, _end, _ignoreCase = false) {
        var i, r = '', s;
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
        var r = this.toStr(_val);
        _new = this.toStr(_new);
        if (_new.length > 0) {
            if (r.length > 0) r += this.toStr(_separator) + _new;
            else r = _new;
        }
        return r;
    }

    // Returns true if check box selected is marked.
    checked(_selector) {
        var o = this.select(_selector), r = false;
        if (o && o.length) r = o.is(':checked');
        return r;
    }

    // Return first string not null or empty string if not found.
    coalesce(_p0, _p1, _p2, _p3, _p4, _p5, _p6, _p7, _p8, _p9, _p10, _p11, _p12, _p13, _p14, _p15) {
        var r = null;
        if ((_p0 != undefined) && (_p0 != null)) r = _p0;
        else if ((_p1 != undefined) && (_p1 != null)) r = _p1;
        else if ((_p2 != undefined) && (_p2 != null)) r = _p2;
        else if ((_p3 != undefined) && (_p3 != null)) r = _p3
        else if ((_p4 != undefined) && (_p4 != null)) r = _p4;
        else if ((_p5 != undefined) && (_p5 != null)) r = _p5;
        else if ((_p6 != undefined) && (_p6 != null)) r = _p6;
        else if ((_p7 != undefined) && (_p7 != null)) r = _p7;
        else if ((_p8 != undefined) && (_p8 != null)) r = _p8;
        else if ((_p9 != undefined) && (_p9 != null)) r = _p9;
        else if ((_p10 != undefined) && (_p10 != null)) r = _p10;
        else if ((_p11 != undefined) && (_p11 != null)) r = _p11;
        else if ((_p12 != undefined) && (_p12 != null)) r = _p12;
        else if ((_p13 != undefined) && (_p13 != null)) r = _p13;
        else if ((_p14 != undefined) && (_p14 != null)) r = _p14;
        else if ((_p15 != undefined) && (_p15 != null)) r = _p15;
        return r;
    }

    // Expire cookie by name.
    cookieExpire(_cookie) {
        return cookieWrite(_cookie, '', -1);
    }

    // Returns value of cookie by name.
    cookieRead(_cookie) {
        var a, c, i, id, r = '';
        _cookie = this.toStr(_cookie);
        if (document.cookie) {
            id = _cookie + '=';
            a = document.cookie.split(';');
            i = 0;
            while ((r == '') && (i < a.length)) {
                c = a[i];
                while (c.charAt(0) == ' ') c = c.substring(1);
                if (c.indexOf(id) == 0) r = c.substring(id.length);
                i++;
            }
        }
        return r;
    }

    // Write value on cookie by name with expiration days.
    cookieWrite(_cookie, _val, _days) {
        var xp = '';
        _cookie = this.toStr(_cookie);
        _val = this.toVal(_val);
        _days = this.toVal(_days);
        if (document.cookie) {
            if (_days) {
                var d = new Date();
                d.setTime(d.getTime() + _days * 86400000);
                xp = '; expires=' + d.toGMTString();
            }
            document.cookie = _cookie + '=' + _val + xp + '; path=/';
        }
        return document.cookie;
    }

    // Returns new date with year, month and day.
    date(_year, _month, _day, _hours = 0, _minutes = 0, _seconds = 0) {
        var r = new Date(
            this.toVal(_year),
            this.toVal(_month),
            this.toVal(_day),
            _hours,
            _minutes,
            _seconds); 
        return r;
    }

    // Returns day value of date.
    dateDay(_date) {
        return _date.getDate();
    }

    // Returns day of week of date.
    dateDayOfWeek(_date) {
        var r = _date.getDay();
        if (r < 1) r = 7;
        return r;
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
        var r = false;
        if (val === undefined) r = true;
        else if (val == null) r = true;
        else if (this.toStr(val).trim().length < 1) r = true;
        return r;
    }

    // Returns true if object selected is enabled. If value specified set enable to value.
    enabled(_selector, _value = null) {
        var id, o = this.select(_selector), r = false;
        if (o && o.length) {
            if (value == null) r = !this.toBool(o.prop('disabled')); 
            else {
                if (_value == false) r = true;
                else r = false;
                o.prop('disabled', r);
                id = this.toStr(o.attr('id'));
                if (!this.empty(id)) {
                    $("[sm-for='" + id + "']").each(function () {
                        $(this).prop('disabled', r);
                    });
                }
            }
        }
        return r;
    }

    // Returns value with all carriage-return and tabs replaced by spaces.
    flat(_val) {
        var r = this.toStr(_val);
        r = r.replaceAll("\t", " ").replaceAll("\r\n", " ").replaceAll("\r", " ").replaceAll("\n", " ");
        return r;
    }

    // Returns decimal part of number.
    frac(_val) {
        var r = this.toVal(_val);
        r = r - Math.floor(r);
        return r;
    }

    // Return object from parsing JSON string.
    fromJson(_json) {
        var r = null;
        if (_json) {
            if (_json.length > 0) {
                r = JSON.parse(_json);
            }
        }
        return r;
    }

    // Return object from parsing JSON base 64 string.
    fromJson64(_json64) {
        var r = this.fromJson(this.base64Decode(_json64));
        return r;
    }

    // Returns value of element selected.
    get(_sel) {
        var o = this.select(_sel), r = '', t;
        if (o && o.length) {
            t = this.toStr(o.attr('sm-type')).trim().toUpperCase();
            if (t == 'YESNO') {
                if (o.is(':checked')) r = 'Y';
                else {
                    o = $('#' + o.attr('id') + '_N');
                    if (o && o.length) {
                        if (o.is(':checked')) r = 'N';
                        else r = '';
                    }
                    else r = '';
                }
            }
            else if (t == 'CHECK') {
                if (o.is(':checked')) r = '1';
                else r = '0';
            }
            else r = this.toStr(o.val());
        }
        return r;
    }

    // Return DOM element by id or null if not found.
    getDOMElement(_id) {
        var r = null;
        if (document.getElementById) r = document.getElementById(_id);
        return r;
    }

    // Evaluate test is true or false and return corresponding parameter.
    iif(_test, _ifTrue, _ifFalse) {
        if (test == true) return _ifTrue;
        else return _ifFalse;
    }

    // Insert new value between start and end substrings.
    insBtw(_val, _new, _start, _end, _ignoreCase = false) {
        var i, a, b, r;
        if (_ignoreCase) i = _val.toLowerCase().indexOf(_start.toLowerCase());
        else i = _val.indexOf(_start);
        if (i > -1) {
            a = this.mid(_val, 0, i) + _start + _new;
            b = this.mid(_val, i + _start.length, _val.length);
            if (_ignoreCase) i = b.toLowerCase().indexOf(_end.toLowerCase());
            else i = b.indexOf(_end);
            if (i > -1) r = a + this.mid(b, i, b.length);
            else r = _val;
        }
        else r = _val + _start + newstring + _end;
        return r;
    }

    // Returns integer part of number.
    int(_val) {
        return Math.floor(_val);
    }

    // Return  true if object is a jQuery instance.
    isJQuery(_obj) {
        var r;
        if (_obj === undefined) r = false;
        else if (_obj == null) r = false;
        else r = _obj instanceof jQuery;
        return r;
    }

    // Return language code of client browser (it, en, de, fr, nl).
    language() {
        var r = '';
        if (navigator.language) r = navigator.language.toLowerCase();
        else if (navigator.userLanguage) r = navigator.userLanguage.toLowerCase();
        else if (navigator.browserLanguage) r = navigator.browserLanguage.toLowerCase();
        if (r.indexOf('it') > -1) r = 'it';
        else if (r.indexOf('en') > -1) r = 'en';
        else if (r.indexOf('de') > -1) r = 'de';
        else if (r.indexOf('fr') > -1) r = 'fr';
        else if (r.indexOf('nl') > -1) r = 'nl';
        else r = 'en';
        return r;
    }

    // Returns first length characters of string from left.
    left(_val, _len) {
        var r = '';
        _val = this.toStr(_val);
        _len = this.toVal(_len);
        if (_len > _val.length) _len = _val.length;
        if (_len > 0) r = _val.substring(0, _len);
        return r;
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
        var r = '';
        if (_len == null) _len = _val.length;
        else _len = this.toVal(_len);
        if (_val.length > 0) {
            if (_len > 0) {
                if (_start < 0) _start = 0;
                if (_start < _val.length) {
                    if (_start + _len > _val.length) r = _val.substring(_start);
                    else r = _val.substring(_start, _start + _len);
                }
            }
        }
        return r;
    }

    // Returns current date-time.
    now() {
        return new Date();
    }

    // Returns string filled at left with char until length.
    padL(_val, _len, _char = ' ') {
        var r = this.toStr(_val);
        _len = this.toVal(_len);
        _char = this.toStr(_char);
        if (_char.length < 1) _char = ' ';
        else if (_char.length > 1) _char = _char.substring(0, 1);
        while (r.length < _len) r = _char + r;
        return r;
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

    // Return page name without path and extension.
    pageId() {
        var p = window.location.pathname;
        p = p.split('/').pop();
        return this.before(p + '.', '.');
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

    // Return jQuery element by selector.
    // If selector starts by #, [ or . will be considered as standard jQuery selector.
    // If starts by ! or ? will be considered as numeric id.
    // If starts by * will be considered as error label related to element with numeric id.
    // If starts by $ will be considered as text label related to element with numeric id.
    // If starts by @ will be considered as element alias.
    // Otherwise will be considered as field name related to element.
    // If selector is already an jQuery object will be returned itself.
    // If selector contains : following part will be considered as row id.
    select(_selector) {
        var i, row = null;
        if (_selector) {
            if (_selector instanceof jQuery) return _selector;
            else {
                _selector = _selector.trim();
                if (_selector.length > 1) {
                    if ('#[.'.indexOf(_selector.substr(0, 1)) < 0) {
                        i = _selector.indexOf(':');
                        if (i > -1) {
                            row = _selector.substr(i + 1);
                            _selector = _selector.substr(0, i);
                        }
                        if (_selector.startsWith("!") || _selector.startsWith("?")) _selector = "[sd-id='" + _selector.substr(1) + "']";
                        else if (_selector.startsWith('*')) _selector = _selector = "[sd-id='" + _selector.substr(1) + "'][sd-ext='err']";
                        else if (_selector.startsWith('$')) _selector = "[sd-id='" + _selector.substr(1) + "'][sd-ext='lbl']";
                        else if (_selector.startsWith('@')) _selector = "[sd-alias='" + _selector.substr(1) + "']";
                        else _selector = "[sd-field='" + _selector + "']";
                        if (row != null) _selector += "[sd-row='" + row + "']";
                    }
                    return $(_selector);
                }
                else return null;
            }
        }
        else return null;
    }

    // Convert value to bool.
    toBool(_val) {
        if (_val === undefined) return false;
        else if (_val == null) return false;
        else if (_val instanceof Boolean) return _val;
        else if (_val instanceof jQuery) return this.toBool(this.toStr(_val.val()));
        else if ('tTvVsS1+'.indexOf(this.toStr(_val).substr(0, 1)) < 0) return false;
        else return true;
    }

    // Return value with esplicit HTML entities.
    toHtml(_val, _notIfStartWith = null) {
        if (_val) {
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
var sm = new SMCode();