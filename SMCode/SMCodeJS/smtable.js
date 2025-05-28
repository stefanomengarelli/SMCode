/*  ===========================================================================
 *  
 *  File:       smtable.js
 *  Version:    2.0.216
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode javascript support library.
 *  
 *  MIT License
 *  ===========
 *  SMCode Javascript Rapid Application Development Code Library
 *  Copyright (c) 2010-2025 Stefano Mengarelli - All rights reserved.
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

    // Rows collection.
    rows = null;

    // Table object.
    table = null;

    // Instance constructor.
    constructor(_jqueryselector) {
        this.table = $(_jqueryselector);
        if (SM.isJQuery(this.table)) {
            this.table.data('smtable', this);
            this.getColumns();
            this.update();
        }
        else this.table = null;
    }

    // Get all table rows.
    clear() {
        if (this.table != null) {
            this.rows = null;
            this.table.find('tbody').html('');
        }
    }

    // Delete row by index.
    deleteRow(_index) {
        if (this.table != null) {
            var h = this.getRows();
            if ((_index > -1) && (_index < h)) {
                this.rows.eq(_index).remove();
                this.rows = null;
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
        if (this.table != null) {
            if (this.columns == null) {
                var h = this.table.find('thead > tr > th'), cols = [], i = 0;
                h.each(function () {
                    var c = new SMTableColumn();
                    c.tag = $(this);
                    c.align = SM.toStr(c.tag.attr('sm-col-align'));
                    c.field = SM.toStr(c.tag.attr('sm-col-field'));
                    c.format = SM.toStr(c.tag.attr('sm-col-format'));
                    c.format = SM.toStr(c.tag.attr('sm-col-name'));
                    c.text = SM.toStr(c.tag.text());
                    c.visible = SM.toBool(c.tag.hasClass('sm-hidden')) == false;
                    cols[i++] = c;
                });
                this.columns = cols;
            }
            if (this.columns != null) return this.columns.length;
            else return 0;
        }
        else return 0;
    }

    // Get json of row at index.
    getRowJson(_Index) {
        return JSON.stringify(this.getRowObj(_Index));
    }

    // Get json of row at index.
    getRowObj(_Index) {
        var r = null, f;
        if (this.table != null) {
            var h = this.getRows(), k = this.getColumns();
            r = {};
            if ((_Index > -1) && (_Index < h)) {
                var i = 0, td = this.rows.eq(_Index).find('td');
                while (i < k) {
                    f = this.columns[i].field;
                    if (f.length > 0) {
                        r[f] = SM.toStr(td.eq(i).text());
                        r.length += 1;
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
            if (this.rows == null) this.rows = this.table.find('tbody > tr');
            if (this.rows != null) return this.rows.length;
            else return 0;
        }
    }

    // Get all table rows as object array.
    getRowsJson() {
        var obj = this.getRowsObj();
        var r = JSON.stringify({ obj });
        return r;
    }

    // Get all table rows as object array.
    getRowsObj() {
        var r = [], i = 0, h = this.getRows(), o;
        if (this.table != null) {
            while (i < h) {
                o = this.getRowObj(i);
                r[i] = { o };
                i++;
            }
            r.length = i;
        }
        return r;
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

    // Add page navigation.
    pageNav() {
        var i, h, html = '', nid, nav, pag;
        if (this.table != null) {
            pag = "javascript:$('#" + this.table.attr('id') + "').data('smtable').page(";
            // add first
            html += '<ul class="pagination"><li class="page-item' + SM.iif(this.currentPage < 1, ' disabled', '') + '"><a class="page-link" href = "' + pag + (this.currentPage - 1) + ');" >';
            html += '<svg class="icon icon-primary"><use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-left"></use></svg>';
            html += '<span class="visually-hidden">Pagina precedente</span></a></li>';
            // add pages
            h = this.pagesCount();
            for (i = 0; i < h; i++) {
                html += '<li class="page-item"><a class="page-link"' + SM.iif(this.currentPage == i, ' aria-current="page"') + ' href="' + pag + (i) + ');" >' + (i + 1) + '</a></li>';
            }
            // add last
            html += '<li class="page-item' + SM.iif(this.currentPage >= h - 1, ' disabled', '') + '"><a class="page-link" href = "' + pag + (h - 1) + ');" >';
            html += '<span class="visually-hidden">Pagina succesiva</span>';
            html += '<svg class="icon icon-primary"><use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-right"></use></svg></a></li></ul>';
            // add navigation html
            nid = this.table.attr('id') + '_nav';
            nav = $('#' + nid);
            if (!SM.isJQuery(nav)) {
                this.table.after('<nav id="' + nid + '" class="pagination-wrapper justify-content-center col-sd-12 col-md-12" aria-label="Navigazione tabella"></nav>');
                nav = $('#' + nid);
            }
            nav.html(html);
        }
    }

    // Show current page items.
    paging() {
        if (this.table != null) {
            this.getRows();
            var i = 0, a = this.pageSize * this.currentPage, b = a + this.pageSize;
            this.rows.each(function () {
                if ((i >= a) && (i < b)) $(this).removeClass('sm-hidden');
                else $(this).addClass('sm-hidden');
                i++;
            });
            this.pageNav();
        }
    }

    // Update table layout.
    update() {
        this.paging();
    }

}