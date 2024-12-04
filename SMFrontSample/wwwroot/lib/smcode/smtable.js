/*  ===========================================================================
 *  
 *  File:       smtable.js
 *  Version:    2.0.94
 *  Date:       November 2024
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
 *  SMTableColumn class
 *  ===========================================================================
 */

class SMTableColumn {
    align = '';
    field = '';
    format = '';
    name = '';
    tag = null;
    text = '';
    visible = true;
    width = 0;
}

/*  ===========================================================================
 *  SMTable class (SMCode required)
 *  ===========================================================================
 */

class SMTable {

    // Columns collection
    columns = null;

    // Current page.
    currentPage = 0;

    // Items per page.
    pageSize = 16;

    // Rows collection.
    rows = null;

    // Sort column
    sortColumn = -1;

    // Sort verse
    sortVerse = 1;

    // Table object.
    table = null;

    // Table width.
    width = 0;

    // Instance constructor.
    constructor(_$selector) {
        this.table = $(_$selector);
        if (this.table.is('table') == false) this.table = null;
        this.getColumns();
        this.update();
    }

    // Append row from JSON object.
    add(_obj) {
        if ((this.table != null) && (_obj != null)) {
            this.table.find('tbody').append(this.rowHtml(_obj));
            this.update();
        }
    }

    // Append row from JSON string.
    addJson(_json) {
        add(JSON.parse(_json));
    }

    // Get all table rows.
    clear() {
        if (this.table != null) {
            this.table.find('tbody').html('');
            this.update();
        }
    }

    // Delete row by index.
    deleteRow(_index) {
        if (this.table != null) {
            var h = this.getRows();
            if ((_index > -1) && (_index < h)) {
                this.rows.eq(_index).remove();
                this.update();
                if (this.rows == null) return 0;
                else return this.rows.length;
            }
            else return h;
        }
        else return -1;
    }

    // Get all table columns and return count.
    getColumns() {
        if (this.columns == null) {
            var h = this.table.find('thead > tr > th'), cols = [], i = 0, w = 0;
            h.each(function () {
                var c = new SMTableColumn();
                c.tag = $(this);
                c.align = SM.toStr(c.tag.attr('sm-col-align'));
                c.field = SM.toStr(c.tag.attr('sm-col-field'));
                c.format = SM.toStr(c.tag.attr('sm-col-format'));
                c.format = SM.toStr(c.tag.attr('sm-col-name'));
                c.text = SM.toStr(c.tag.text());
                c.visible = SM.toBool(c.tag.hasClass('sm-hidden')) == false;
                c.width = SM.toInt(c.tag.attr('sm-col-width'));
                w += c.width;
                cols[i++] = c;
            });
            this.columns = cols;
            this.width = w;
        }
        if (this.columns != null) return Object.keys(this.columns).length;
        else return 0;
    }

    // Get json of row at index.
    getRowJson(_index) {
        return JSON.stringify(this.getRowObj(_index));
    }

    // Get json of row at index.
    getRowObj(_index) {
        var r = null, f;
        if (this.table != null) {
            var h = this.getRows(), k = this.getColumns();
            r = {};
            if ((_index > -1) && (_index < h)) {
                var i = 0, td = this.rows.eq(_index).find('td'), z = 0;
                while (i < k) {
                    f = this.columns[i].field;
                    if (f.length > 0) {
                        r[f] = SM.toStr(td.eq(i).text());
                        z++;
                    }
                    i++;
                }
            }
        }
        return r;
    }

    // Get all table rows and return count.
    getRows() {
        if (this.table == null) return -1;
        else {
            this.rows = this.table.find('tbody > tr');
            if (this.rows != null) return this.rows.length;
            else return 0;
        }
    }

    // Get all table rows as object array.
    getRowsJson() {
        return JSON.stringify(this.getRowsObj());
    }

    // Get all table rows as object array.
    getRowsObj() {
        var r = {}, i = 0, h = this.getRows();
        if (this.table != null) {
            while (i < h) {
                r[i] = this.getRowObj(i);
                i++;
            }
        }
        return r;
    }

    // Load table data from JSON object.
    load(_obj) {
        if ((this.table != null) && (_obj != null)) {
            var i, h = Object.keys(_obj).length, r = '';
            this.clear();
            for (i = 0; i < h; i++) r += this.rowHtml(_obj[i]);
            this.table.find('tbody').html(r);
            this.update();
        }
    }

    // Load table data from JSON string.
    loadJson(_json) {
        this.load(JSON.parse(_json));
    }

    // Set current page.
    page(_page) {
        if (this.table != null) {
            h = this.pagesCount();
            if ((_page > -1) && (_page < h)) {
                this.currentPage = _page;
                this.update();
            }
        }
    }

    // Return current pages count.
    pagesCount() {
        var h = Math.trunc(this.getRows() / this.pageSize);
        if (h * this.pageSize < this.rows.length) h++;
        return h;
    }

    // Show current page items.
    paging() {
        if (this.table != null) {
            this.rows = null;
            var i = 0, a = 0, b = this.getRows();
            if (this.pageSize > 0) {
                if (this.currentPage < 0) this.currentPage = 0;
                else if (this.currentPage >= this.pagesCount()) this.currentPage = 0;
                a = this.pageSize * this.currentPage;
                b = a + this.pageSize
            }
            this.rows.each(function () {
                if ((i >= a) && (i < b)) $(this).removeClass('sm-hidden');
                else $(this).addClass('sm-hidden');
                i++;
            });
        }
    }

    // Set row at index.
    rowHtml(_row) {
        var r = '<tr class="sm-hidden">', i;
        if (_row != null) {
            for (i = 0; i < this.columns.length; i++) {
                r += '<td>' + SM.toHtml(SM.format(_row[this.columns[i].field], this.columns[i].format)) + '</td>';
            }
        }
        return r + '</tr>';
    }

    // Sort table.
    sort() {
        var a, b, i, lp = true, rw, tb, w;
        if (this.table != null) {
            if ((this.sortColumn > -1) && (this.sortColumn < this.columns.length)) {
                if (this.sortVerse != -1) this.sortVerse = 1;
                rw = this.table.find('tbody tr');
                debugger;
                while (lp) {
                    lp = false;
                    i = rw.length - 1;
                    while (i > 0) {
                        a = $('td', rw[i]).eq(this.sortColumn).text();
                        b = $('td', rw[i - 1]).eq(this.sortColumn).text();
                        if (this.sortVerse == -1) w = a > b;
                        else w = a < b;
                        if (w == true) {
                            w = rw[i];
                            rw[i] = rw[i - 1];
                            rw[i - 1] = w;
                            lp = true;
                        }
                        i--;
                    }
                }
                debugger;
                tb = this.table.find('tbody');
                tb.html('');
                for (i = 0; i < rw.length; i++) tb.append(rw[i]);
                this.update();
            }
        }
    }

    // Update table layout.
    update() {
        this.paging();
    }

}
