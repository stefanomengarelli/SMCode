/*  ===========================================================================
 *  
 *  File:       SMControlResize.cs
 *  Version:    2.0.45
 *  Date:       September 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMDesk UI control resize class. When control has to be resized, 
 *  set all properties call Begin method, then call Update method each 
 *  time mouse pointer moving and call Stop method when resizing has 
 *  completed.
 *  
 *  ===========================================================================
 */

using System.Drawing;
using System.Windows.Forms;

namespace SMDeskSystem
{

    /* */

    /// <summary>SMDesk UI control resize class. When control has to be resized, 
    /// set all properties call Begin method, then call Update method each
    /// time mouse pointer moving and call Stop method when resizing has
    /// completed.</summary>
    public class SMControlResize
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        private int x = 0, y = 0, w = 0, h = 0;
        private int cx = 0, cy = 0;
        private int minWidth, maxWidth, minHeight, maxHeight;
        private Control control;
        private Control parent = null;
        private bool resizing = false;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Read or set constraint client flag.</summary>
        public bool ConstraintClient { get; set; }

        /// <summary>Read or set moving control.</summary>
        public Control Control
        {
            get { return control; }
            set
            {
                control = value;
                if (control != null) parent = control.Parent;
                else parent = null;
            }
        }

        /// <summary>Read or set resize mode.</summary>
        public SMControlResizeMode Mode { get; set; }

        /// <summary>Read resizing state.</summary>
        public bool Resizing 
        {
            get { return resizing || (Mode != SMControlResizeMode.None); }
        }

        /// <summary>Read or set resizing trigger.</summary>
        public int Trigger { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMControlResize()
        {
            ConstraintClient = false;
            Mode = SMControlResizeMode.None;
            resizing = false;
            Trigger = 3;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Start control resizing session.</summary>
        public bool Begin()
        {
            if ((control != null) && (Mode != SMControlResizeMode.None))
            {
                x = control.Left;
                y = control.Top;
                w = control.Width;
                h = control.Height;
                cx = Cursor.Position.X;
                cy = Cursor.Position.Y;
                minWidth = control.MinimumSize.Width;
                if (minWidth < 0) minWidth = 0;
                minHeight = control.MinimumSize.Height;
                if (minHeight < 0) minHeight = 0;
                maxWidth = control.MaximumSize.Width;
                if (maxWidth < 1) maxWidth = Screen.PrimaryScreen.WorkingArea.Width;
                maxHeight = control.MaximumSize.Height;
                if (maxHeight < 1) maxHeight = Screen.PrimaryScreen.WorkingArea.Height;
                control.PerformLayout();
                Update();
                return true;
            }
            else return false;
        }

        /// <summary>Start resizing session assignin control and mode.</summary>
        public void Begin(Control _Control, SMControlResizeMode _ControlResizeMode)
        {
            control = _Control;
            if (control != null) parent = control.Parent;
            else parent = null;
            Mode = _ControlResizeMode;
            Begin();
        }

        /// <summary>End resizing session.</summary>
        public void End()
        {
            resizing = false;
            Mode = SMControlResizeMode.None;
            if (control != null) control.Update();
        }

        /// <summary>Update form size and position.</summary>
        public bool Update()
        {
            bool b = false;
            int dx, dy, x0, y0, w0, h0;
            if ((control != null) && (Mode != SMControlResizeMode.None))
            {
                dx = Cursor.Position.X - cx;
                dy = Cursor.Position.Y - cy;
                if (resizing)
                {
                    x0 = x;
                    y0 = y;
                    w0 = w;
                    h0 = h;
                    if (Mode == SMControlResizeMode.Move)
                    {
                        if (ConstraintClient && (parent != null))
                        {
                            if (x0 + dx + w0 > parent.Width) x0 = parent.Width - w0;
                            else if (x0 + dx < 0) x0 = 0;
                            else x0 += dx;
                            if (y0 + dy + h0 > parent.Height) y0 = parent.Height - h0;
                            else if (y0 + dy < 0) y0 = 0;
                            else y0 += dy;
                        }
                        else
                        {
                            x0 += dx;
                            y0 += dy;
                        }
                    }
                    else if (Mode == SMControlResizeMode.SizeN)
                    {
                        if (ConstraintClient && (parent != null))
                        {
                            if (y0 + dy < 0) dy = -y0;
                        }
                        h0 -= dy;
                        if (h0 < minHeight)
                        {
                            h0 = minHeight;
                            dy = h - minHeight;
                        }
                        y0 += dy;
                    }
                    else if (Mode == SMControlResizeMode.SizeE)
                    {
                        if (ConstraintClient && (parent != null))
                        {
                            if (x0 + dx + w0 > parent.Width) w0 = parent.Width - x0;
                            else w0 += dx;
                        }
                        else w0 += dx;
                    }
                    else if (Mode == SMControlResizeMode.SizeS)
                    {
                        if (ConstraintClient && (parent != null))
                        {
                            if (y0 + dy + h0 > parent.Height) h0 = parent.Height - y0;
                            else h0 += dy;
                        }
                        else h0 += dy;
                    }
                    else if (Mode == SMControlResizeMode.SizeW)
                    {
                        if (ConstraintClient && (parent != null))
                        {
                            if (x0 + dx < 0) dx = -x0;
                        }
                        w0 -= dx;
                        if (w0 < minWidth)
                        {
                            w0 = minWidth;
                            dx = w - minWidth;
                        }
                        x0 += dx;
                    }
                    else if (Mode == SMControlResizeMode.SizeSE)
                    {
                        if (ConstraintClient && (parent != null))
                        {
                            if (x0 + dx + w0 > parent.Width) w0 = parent.Width - x0;
                            else w0 += dx;
                            if (y0 + dy + h0 > parent.Height) h0 = parent.Height - y0;
                            else h0 += dy;
                        }
                        else
                        {
                            w0 += dx;
                            h0 += dy;
                        }
                    }
                    if (w0 < minWidth) w0 = minWidth; else if (w0 > maxWidth) w0 = maxWidth;
                    if (h0 < minHeight) h0 = minHeight; else if (h0 > maxHeight) h0 = maxHeight;
                    if ((control.Left != x0) || (control.Top != y0))
                    {
                        control.Location = new Point(x0, y0);
                        b = true;
                    }
                    if ((control.Width != w0) || (control.Height != h0))
                    {
                        control.Size = new Size(w0, h0);
                        b = true;
                    }
                }
                else if ((dx > Trigger) || (-dx > Trigger) || (dy > Trigger) || (-dy > Trigger)) resizing = true;
            }
            return b;
        }

        #endregion

        /* */

    }

    /* */

}
