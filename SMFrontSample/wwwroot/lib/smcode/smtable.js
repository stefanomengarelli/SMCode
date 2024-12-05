/*  ===========================================================================
 *  
 *  File:       smtable.js
 *  Version:    2.0.95
 *  Date:       December 2024
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
    sortable = false;
    tag = null;
    text = '';
    visible = true;
    width = 0;
}

/*  ===========================================================================
 *  SMTablePagination class
 *  ===========================================================================
 */

class SMTablePagination {
    begin = '<nav class="pagination-wrapper justify-content-center" aria-label="Navigazione"><ul class="pagination">';
    end = '</ul></nav>';
    next = '<li class="page-item"><a class="page-link" href="%%link%%"><span class="visually-hidden">Pagina successiva</span>'
        + '<svg class="icon icon-primary"><use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-right"></use></svg></a></li>';
    nextDisabled = '<li class="page-item disabled"><a class="page-link" href="%%link%%"><span class="visually-hidden">Pagina successiva</span>'
        + '<svg class="icon icon-primary"><use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-right"></use></svg></a></li>';
    page = '<li class="page-item"><a class="page-link" href="%%link%%">%%page%%</a></li>';
    pageSelected = '<li class="page-item"><a class="page-link" aria-current="page" href="%%link%%" >%%page%%</a></li>';
    pagination = null;
    prior = '<li class="page-item"><a class="page-link" href="%%link%%"><svg class="icon icon-primary">'
        + '<use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-left"></use></svg>'
        + '<span class="visually-hidden">Pagina precedente</span></a></li>';
    priorDisabled = '<li class="page-item disabled"><a class="page-link" href="%%link%%"><svg class="icon icon-primary">'
        + '<use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-left"></use></svg>'
        + '<span class="visually-hidden">Pagina precedente</span></a></li>';

    render(_currentPage, _pages) {
        var r = this.begin, i = 0, lnk, id;
        debugger;
        if (this.pagination != null) {
            id = SM.before(this.pagination.attr('id'), '_pagination');
            lnk = "javascript:$('#" + id + "').data('smtable').page(0);";
            if (_currentPage < 1) r += this.priorDisabled.replaceAll('%%link%%', 'javascript:void(0)');
            else r += this.prior.replaceAll('%%link%%', lnk);
            while (i < _pages) {
                lnk = "javascript:$('#" + id + "').data('smtable').page(" + i + ");";
                if (i == _currentPage) r += this.pageSelected.replaceAll('%%page%%', '' + (i + 1)).replaceAll('%%link%%', 'javascript:void(0)');
                else r += this.page.replaceAll('%%page%%', '' + (i + 1)).replaceAll('%%link%%', lnk);
                i++;
            }
            lnk = "javascript:$('#" + id + "').data('smtable').page(" + (_pages - 1) + ");";
            if (_currentPage < _pages - 1) r += this.next.replaceAll('%%link%%', lnk);
            else r += this.nextDisabled.replaceAll('%%link%%', 'javascript:void(0)');
            r += this.end;
            this.pagination.html(r);
            return true;
        }
        else return false;
    }
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
    pageSize = 10;

    // Pagination object.
    pagination = new SMTablePagination();

    // Rows collection.
    rows = null;

    // Table selector.
    selector = '';

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
        debugger;
        this.selector = _$selector;
        this.table = $(_$selector);
        this.table.data('smtable', this);
        if (this.table.is('table') == true) {
            $('<div id="' + this.table.attr('id') + '_pagination"></div>').insertAfter(this.table);
            this.pagination.pagination = $('#' + this.table.attr('id') + '_pagination');
        }
        else this.table = null;
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

    // Load table data from AJAX JSON call.
    ajaxJson(_url) {
        $.ajax({
            type: "GET",
            url: _url,
            success: function (_value) {
                debugger;
                this.loadJson(_value);
            }
        });
    }

    // Load table data from AJAX HTML raw call.
    ajaxRaw(_url) {
        $.ajax({
            type: "GET",
            url: _url,
            success: function (_value) {
                debugger;
                this.loadRaw(_value);
            }
        });
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
                c.sortable = SM.toBool(c.tag.attr('sm-col-sortable'));
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

    // Load table data from raw HTML.
    loadRaw(_html) {
        if (this.table != null) {
            this.table.find('tbody').html(_html);
            this.update();
        }
    }

    // Set current page.
    page(_page) {
        if (this.table != null) {
            var h = this.pagesCount();
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
            var i = 0, a = 0, b = this.getRows(), pages = this.pagesCount();
            if (this.pageSize > 0) {
                if (this.currentPage < 0) this.currentPage = 0;
                else if (this.currentPage >= pages) this.currentPage = 0;
                a = this.pageSize * this.currentPage;
                b = a + this.pageSize
            }
            this.rows.each(function () {
                if ((i >= a) && (i < b)) $(this).removeClass('sm-hidden');
                else $(this).addClass('sm-hidden');
                i++;
            });
            this.pagination.render(this.currentPage, pages);
        }
    }

    // Return row HTML code.
    rowHtml(_row) {
        var r = '<tr class="sm-hidden">', i, aln;
        if (_row != null) {
            for (i = 0; i < this.columns.length; i++) {
                aln = this.columns[i].align.trim();
                if (aln.length > 0) aln = ' align="' + aln + '"';
                r += '<td'+aln+'>' + SM.toHtml(SM.format(_row[this.columns[i].field], this.columns[i].format)) + '</td>';
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
