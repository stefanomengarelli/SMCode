/*  ===========================================================================
 *  File:       smtable.js
 *  Version:    2.0.300
 *  Date:       September 2025
 *  
 *  info@stefanomengarelli.it
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
    id = '';
    name = '';
    style = '';
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

    // Data string column separator
    sepCol = ';';

    // Data string row separator
    sepRow = '|';

    // Last getCols() count result.
    colsCount = 0;

    // Columns collection.
    columns = null;

    // Current page.
    currentPage = 0;

    // Empty rows message.
    emptyRowsMessage = "Non sono presenti dati da visualizzare.";

    // Table id.
    id = '';

    // Items per page.
    pageSize = 10;

    // Rows collection.
    rows = null;

    // Last getRows() count result.
    rowsCount = 0;

    // Row counter column index.
    rowCountColumn = -1;

    // Table object.
    table = null;

    // Instance constructor.
    constructor(_jqueryselector) {
        this.table = $(_jqueryselector);
        if (SM.isJQuery(this.table)) {
            this.table.data('smtable', this);
            this.id = SM.toStr(this.table.attr('id'));
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

    // Get or set content of cell at row and column.
    cell(_row, _col, _val = null) {
        var o = this.rows.eq(_row);
        if (o && o.length) {
            o = o.find('td');
            if (o && o.length) {
                o = o.eq(_col);
                if (o && o.length) {
                    if (_val == null) return SM.toStr(o.html());
                    else {
                        _val = SM.toStr(_val);
                        o.html(_val);
                        return _val;
                    }
                }
            }
        }
        return '';
    }

    // Get table data string or set table data from string.
    // "#;fld1;...;fldN|#;val1;...;valN|"
    dataStr(_dat = null) {
        var cols = [], i, j, nc = this.getColumns(), nh, nr = this.getRows(), r = '', row, td, tr, id;
        if (_dat == null) {
            // get columns
            nh = 0;
            for (j = 0; j < nc; j++) {
                if (!SM.empty(this.columns[j].field)) {
                    r += this.sepCol + SM.base64Encode(this.columns[j].field);
                    cols[nh] = j;
                    nh++;
                }
            }
            r = nh + r;
            // get rows
            for (i = 0; i < nr; i++) {
                r += this.sepRow;
                row = this.rows.eq(i);
                id = SM.toInt(row.attr('sm-row-id'));
                if (id < 1) id = i;
                r += id;
                tr = row.find("td");
                for (j = 0; j < nh; j++) {
                    td = tr.eq(cols[j]);
                    r += this.sepCol + SM.base64Encode(td.html());
                }
            }
        }
        else {
            // return
            r = _dat;
        }
        return r;
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

    // If no rows on table add default empty row.
    emptyRow() {
        if (this.getRows() < 1) {
            var tr = "<tr id='" + this.id + "_row_empty' class='sm-table-row'><td id='" + this.id
                + "_row_0_err' colspan='" + this.colsCount + "'>" + this.emptyRowsMessage + "</td></tr>",
                o = $("#" + this.id + "_row_empty");
            if (o && o.length) o.remove();
            this.table.find('tbody:last-child').append(tr);
        }
    }

    // Ensure visible row by index (goto page including row).
    ensureVisible(_row) {
        page(Math.trunc(_row / this.pageSize));
    }

    // Return index of first row matching text in column index or -1 if not found.
    // If column index is -1 search in all columns.
    find(_text, _colIndex = -1, _caseSensitive = false, _contains = false, _force = true) {
        var h = this.getRows(_force), i = 0, j, r = -1, q;
        if (!SM.empty(_text)) {
            _text = SM.toStr(_text);
            if (!_caseSensitive) _text = _text.toLowerCase();
            if ((_colIndex > -1) && (_colIndex < this.colsCount)) {
                while ((r < 0) && (i < h)) {
                    q = this.rows.eq(i).find('td').eq(_colIndex).text();
                    if (!_caseSensitive) q = q.toLowerCase();
                    if ((_text == q) || (_contains && (q.indexOf(_text) > -1))) r = i;
                    i++;
                }
            }
            else {
                while ((r < 0) && (i < h)) {
                    j = 0;
                    while ((r < 0) && (j < this.colsCount)) {
                        q = this.rows.eq(i).find('td').eq(j).text();
                        if (!_caseSensitive) q = q.toLowerCase();
                        if ((_text == q) || (_contains && (q.indexOf(_text) > -1))) r = i;
                        j++;
                    }
                    i++;
                }
            }
        }
        return r;
    }

    // Get all table columns and return count.
    getColumns(_force = false) {
        if (this.table != null) {
            if ((_force == true) || (this.columns == null)) {
                var h = this.table.find('thead > tr > th'), cols = [], i = 0, self = this;
                this.rowCountColumn = -1;
                this.colsCount = 0;
                h.each(function () {
                    var c = new SMTableColumn();
                    c.tag = $(this);
                    c.align = SM.toStr(c.tag.attr('sm-col-align'));
                    c.field = SM.toStr(c.tag.attr('sm-col-field'));
                    c.format = SM.toStr(c.tag.attr('sm-col-format'));
                    c.id = SM.toStr(c.tag.attr('id'));
                    if ((self.rowCountColumn < 0) && c.id.endsWith("_count")) self.rowCountColumn = i;
                    c.name = SM.toStr(c.tag.attr('sm-col-name'));
                    c.style = SM.toStr(c.tag.attr('style'));
                    c.text = SM.toStr(c.tag.text());
                    c.visible = SM.toBool(c.tag.hasClass('sm-hidden')) == false;
                    c.width = SM.toStr(c.tag.attr("width"));
                    cols[i] = c;
                    i++;
                });
                this.columns = cols;
                this.colsCount = cols.length;
            }
        }
        return this.colsCount;
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
    getRows(_force = true) {
        if (this.table == null) return -1;
        else {
            if ((_force == true) || (this.rows == null)) {
                this.rows = this.table.find('tbody > tr');
                if (this.rows != null) {
                    this.rowsCount = this.rows.length;
                    if (this.rowsCount == 1) {
                        var o = $("#" + this.id + "_row_empty");
                        if (o && o.length) this.rowsCount = 0;
                    }
                }
                else this.rowsCount = 0;
            }
            return this.rowsCount;
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

    // Return max value of attribute on table rows
    max(_attr, _force = true) {
        var r = null;
        this.getRows(_force);
        this.rows.each(function () {
            var a = SM.toVal($(this).attr(_attr));
            if ((r == null) || (a > r)) r = a;
        });
        return r;
    }

    // Add new row to table.
    newRow(_controlId = -1) {
        var dtl, i, id, rw, tr, wd, ww;
        dtl = this.max("sm-row-id") + 100001;
        rw = this.rows.length + 1;
        tr = "<tr id='" + this.id + "_row_" + rw + "' sm-row-id='" + dtl + "' class='sm-table-row'>";
        //
        for (i = 0; i < this.colsCount; i++) {
            //
            id = this.columns[i].id;
            //
            ww = SM.toInt(this.columns[i].width);
            if (ww < 1) wd = "";
            else wd = " width='" + ww + "%'";
            //
            tr += "<td id='" + id.replace("_col_", "_row_" + rw + "_") + "' style='" + this.columns[i].style + "'";
            //
            if (id.endsWith("_count")) {
                tr += wd + " class='sm-table-row-count'>";
            }
            else if (id.endsWith("_edit")) {
                tr += wd + " class='sm-table-row-edit'>";
                tr += '<a href="javascript:$_OnDetailEdit(' + _controlId + ',' + dtl + ');" class="" data-focus-mouse="false"><img src="/lib/teamcode/images/tools/tool-pencil-24-dk.png" title="Modifica"></a>';
            }
            else if (id.endsWith("_del")) {
                tr += wd + " class='sm-table-row-del'>";
                tr += '<a href="javascript:$_OnDetailDelete(' + _controlId + ',' + dtl + ');" class="" data-focus-mouse="false"><img src="/lib/teamcode/images/tools/tool-delete-24-dk.png" title="Modifica"></a>';
            }
            else if (id.endsWith("_menu")) {
                tr += wd + " class='sm-table-row-menu'>";
            }
            else if (ww > 0) {
                tr += wd + ">";
            }
            else {
                tr += wd + ">";
            }
            //
            tr += "</td>";
        }
        //
        tr += "</tr>";
        this.table.find('tbody:last-child').append(tr);
        this.update();
        this.page(999999);
        return dtl;
    }

    // Set current page.
    page(_page) {
        if (this.table != null) {
            var h = this.pagesCount();
            if (_page >= h) _page = h - 1;
            if ((_page > -1) && (_page < h)) {
                this.currentPage = _page;
                this.update();
            }
        }
    }

    // Return current pages count.
    pagesCount(_force = true) {
        var h = Math.trunc(this.getRows(_force) / this.pageSize);
        if (h * this.pageSize < this.rows.length) h++;
        return h;
    }

    // Add page navigation.
    pageNav(_force = true) {
        var i, h, html = '', nid, nav, pag;
        if (this.table != null) {
            pag = "javascript:$('#" + this.table.attr('id') + "').data('smtable').page(";
            // add first
            html += '<ul class="pagination"><li class="page-item' + SM.iif(this.currentPage < 1, ' disabled', '') + '"><a class="page-link" href = "' + pag + (this.currentPage - 1) + ');" >';
            html += '<svg class="icon icon-primary"><use href="/lib/bootstrap-italia/svg/sprites.svg#it-chevron-left"></use></svg>';
            html += '<span class="visually-hidden">Pagina precedente</span></a></li>';
            // add pages
            h = this.pagesCount(_force);
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

    // Renum rows counter
    renum() {
        var o = $("#" + this.id + "_row_empty");
        if (o && o.length) o.remove();
        if (this.rowCountColumn > -1) {
            var i = 0, self = this;
            this.rows.each(function () {
                $(this).find('td').eq(self.rowCountColumn).html('' + (i + 1));
                i++;
            });
        }
        this.emptyRow();
    }

    // Return sum of table values of column index
    sum(_columnindex, _force = true) {
        var r = 0;
        this.getRows(_force);
        this.rows.each(function () {
            r += SM.toVal($(this).find('td').eq(_columnindex).text());
        });
        return r;
    }

    // Update table layout.
    update() {
        if (this.table != null) {
            this.getRows(true);
            var i = 0, a, b, h = this.pagesCount(false);
            if (h < 1) this.currentPage = 0;
            else if (this.currentPage >= h) this.currentPage = h - 1;
            a = this.pageSize * this.currentPage;
            b = a + this.pageSize;
            this.rows.each(function () {
                if ((i >= a) && (i < b)) $(this).removeClass('sm-hidden');
                else $(this).addClass('sm-hidden');
                i++;
            });
            this.renum();
            this.pageNav(false);
        }
    }

}

/*  ===========================================================================
 *  Initialization 
 *  ===========================================================================
 */

// Initialize all tables in jQuery selection as SMTable.
function SMTableBoot(_jqueryselector = ".sm-table") {
    $(_jqueryselector).each(function () {
        var t = new SMTable('#' + $(this).attr('id'));
    });
}