/*  ===========================================================================
 *  
 *  File:       smtable.js
 *  Version:    2.0.92
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
 *  SMTable class (SMCode required)
 *  ===========================================================================
 */

class SMTable {

    // Current page.
    currentPage = 0;

    // Items per page.
    pageSize = 25;

    // Rows collection.
    rows = null;

    // Table object.
    table = null;

    // Instance constructor.
    constructor(_jqueryselector) {
        this.table = $(_jqueryselector);
        if (this.table.is('table') == false) this.table = null;
    }

    // Get all table rows.
    clear() {
        if (this.table != null) {
            this.rows = null;
            this.table.find('tbody').html('');
        }
    }

    // Get all table rows and return count.
    getRows() {
        if (this.rows == null) this.rows = this.table.find('tbody > tr');
        if (this.rows != null) return this.rows.length;
        else return 0;
    }

    // Set current page.
    page(_page) {
        if (this.table != null) {
            h = this.pagesCount();
            if ((_page > -1) && (_page < h)) {
                this.currentPage = _page;
                update();
            }
        }
    }

    // Return current pages count.
    pagesCount() {
        var h = Math.trunc(this.getRows() / pageSize);
        if (h * this.pageSize < rows.length) h++;
        return h;
    }

    // Show current page items.
    paging() {
        if (this.table != null) {
            this.getRows();
            var i = 0, a = this.pageSize * this.currentPage, b = a + this.pageSize;
            rows.each(function () {
                if ((i >= a) && (i < b)) $(this).removeClass('sm-hidden');
                else $(this).addClass('sm-hidden');
                i++;
            });
        }
    }

    // Update table layout.
    update() {
        this.paging();
    }

}