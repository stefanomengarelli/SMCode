/*  ===========================================================================
 *  
 *  File:       smtable.js
 *  Version:    2.0.85
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

    // Table object.
    table = null;

    // Current page.
    currentPage = 0;

    // Items per page.
    paging = 16;

    // Current selected column.
    selectedColumn = -1;

    // Current selected row.
    selectedRow = -1;

    // Instance constructor.
    constructor(_jqueryselector) {
        this.table = $(_jqueryselector);
        if (this.table.is('table') == false) this.table = null;
    }

    // Return value of cell at row and column.
    cell(_row, _col, _val = null) {
        if (this.table == null) return '';
        else return this.table.find('tr:eq(' + _row + ')').find('td:eq(' + _col + ')').html(_val);
    }
    
    // Returns columns count.
    columns() {
        if (this.table == null) return -1;
        else return this.table.find("tr:first td").length;
    }

    // Returns rows count.
    rows() {
        if (this.table == null) return -1;
        else return this.table.find('tr:last').index() + 1;
    }

    // Update table layout.
    update() {

    }

}
