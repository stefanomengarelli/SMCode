/*  ===========================================================================
 *  
 *  File:       Dialogs.cs
 *  Version:    2.0.45
 *  Date:       September 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMDesk UI system dialogs form functions.
 *  
 *  ===========================================================================
 */

using SMCodeSystem;
using System;
using System.Drawing;

namespace SMDeskSystem
{

    /* */

    /// <summary>SMDesk UI system dialogs form functions.</summary>
    public partial class SMDesk : SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Show color dialog box.</summary>
        public bool Color(ref Color _Color, bool _CustomizedColor)
        {
            bool r;
            System.Windows.Forms.ColorDialog f = new System.Windows.Forms.ColorDialog();
            f.Color = _Color;
            f.AllowFullOpen = _CustomizedColor;
            f.ShowHelp = false;
            r = f.ShowDialog() == System.Windows.Forms.DialogResult.OK;
            if (r) _Color = f.Color;
            return r;
        }

        /// <summary>Show confirmation dialog box with message. 
        /// Returns true if user click Yes button otherwise returns false. </summary>
        public bool ConfirmBox(string _Message, string _Title = null)
        {
            if (_Title == null) _Title = SM.T(new string[] { "en:Confirm", "it:Conferma", "fr:Confirmation", "de:Bestätigung" });
            return System.Windows.Forms.MessageBox.Show(
                _Message, _Title,
                System.Windows.Forms.MessageBoxButtons.YesNo,
                System.Windows.Forms.MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>Show error dialog box with message. 
        /// Returns true when user click Ok button. </summary>
        public bool ErrorBox(string _Message, string _Title = null)
        {
            if (_Title == null) _Title = SM.T(new string[] { "en:Error", "it:Errore", "fr:Erreur", "de:Fehler" });
            return System.Windows.Forms.MessageBox.Show(
                _Message, _Title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error)
                == System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>Show folder browser dialog box with title caption and return true
        /// if user selected a valid path. If is not empty, browser start at path.
        /// New selected valid path are stored in pth variable. New folder button
        /// is visible if parameter setted to true.</summary>
        public bool Folder(ref string _Path, string _Title = null, bool _NewFolderButton = false)
        {
            try
            {
                if (_Title == null) _Title = SM.T(new string[] { "en:Select folder", "it:Selezione cartella", "fr:Sélectionnez le dossier", "de:Ordner auswählen" });
                System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();
                f.Description = _Title;
                f.RootFolder = System.Environment.SpecialFolder.MyComputer;
                f.SelectedPath = _Path;
                f.ShowNewFolderButton = _NewFolderButton;
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _Path = f.SelectedPath;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Show font dialog box.</summary>
        public bool Font(ref Font _Font)
        {
            bool r;
            System.Windows.Forms.FontDialog f = new System.Windows.Forms.FontDialog();
            f.Font = _Font;
            f.ShowHelp = false;
            r = f.ShowDialog() == System.Windows.Forms.DialogResult.OK;
            if (r) _Font = f.Font;
            return r;
        }

        /// <summary>Return image dialog filter.</summary>
        public string ImageFilter()
        {
            return SM.T(new string[] {
                "en:JPeg images (*.jpg)|*.jpg|PNG images (*.png)|*.png|Bitmap images (*.bmp)|*.bmp|GIF images (*.gif)|*.gif",
                "it:Immagini JPeg (*.jpg)|*.jpg|Immagini PNG (*.png)|*.png|Immagini Bitmap (*.bmp)|*.bmp|Immagini GIF (*.gif)|*.gif",
                "fr:Images sur JPeg (*.jpg)|*.jpg|Images sur PNG (*.png)|*.png|Images sur Bitmap (*.bmp)|*.bmp|Images sur GIF (*.gif)|*.gif",
                "de:JPeg-Bilder (*.jpg)|*.jpg|PNG-Bilder (*.png)|*.png|Bitmap-Bilder (*.bmp)|*.bmp|GIF-Bilder (*.gif)|*.gif"});
        }

        /// <summary>Show information dialog box with title and message. 
        /// Returns true when user click Ok button. </summary>
        public bool InfoBox(string _Message, string _Title = null)
        {
            if (_Title == null) _Title = SM.T(new string[] { "en:Information", "it:Informazioni", "fr:Informations", "de:Information" });
            return System.Windows.Forms.MessageBox.Show(
                _Message, _Title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Information)
                == System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>Show open file dialog box with title caption and file filter.
        /// If user select a valid file, function return true and full file path are stored in
        /// file name variable. ext represent default extension of selected file.</summary>
        public bool Open(string _Title, string _Filter, string _Extension, ref string _FileName)
        {
            string s;
            try
            {
                System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog();
                f.DefaultExt = _Extension;
                f.Filter = _Filter;
                f.Title = _Title;
                s = SM.FilePath(_FileName).Trim();
                if (s.Length < 1) s = SM.DocumentsPath;
                f.InitialDirectory = s;
                if (_FileName.Trim().Length > 0) f.FileName = SM.FileName(_FileName);
                else if (_Extension.Trim().Length > 0) f.FileName = "*." + _Extension.Trim();
                else f.FileName = "*.*";
                f.CheckFileExists = true;
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _FileName = f.FileName;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Show open multiple file dialog box with title caption and file filter.
        /// If user select a valid file, function return true and full file path are stored in
        /// file names array. ext represent default extension of selected file.</summary>
        public bool Open(string _Title, string _Filter, string _Extension, ref string[] _FileNames)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog();
                f.DefaultExt = _Extension;
                f.Filter = _Filter;
                f.Title = _Title;
                f.InitialDirectory = SM.DocumentsPath;
                f.Multiselect = true;
                f.CheckPathExists = true;
                f.CheckFileExists = true;
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _FileNames = f.FileNames;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>If file at full path file name exists show the confirmation dialog box for overwriting.
        /// Return true if user allow the overwrite.</summary>
        public bool OverwriteBox(string _FileName)
        {
            if (System.IO.File.Exists(_FileName)) return ConfirmBox(SM.T(new string[] {
                    "en:Overwrite file %%0%% ?",
                    "it:Sovrascrivere il file %%0%% ?",
                    "fr:Ecraser le fichier %%0%% ?",
                    "de:Datei überschreiben %%0%% ?" },
                    new string[] { SM.FileName(_FileName) }));
            else return true;
        }

        /// <summary>Show save file dialog box with title caption and filter.
        /// If user select a valid file, function return true and full file path are stored in
        /// file name variable. Extension represent default extension of selected file.</summary>
        public bool Save(string _Title, string _Filter, string _Extension, ref string _FileName)
        {
            string s;
            try
            {
                System.Windows.Forms.SaveFileDialog f = new System.Windows.Forms.SaveFileDialog();
                f.Title = _Title;
                f.Filter = _Filter;
                f.DefaultExt = _Extension;
                s = SM.FilePath(_FileName).Trim();
                if (s.Length < 1) s = SM.DocumentsPath;
                f.InitialDirectory = s;
                if (_FileName.Trim().Length > 0) f.FileName = SM.FileName(_FileName);
                f.AddExtension = true;
                f.CheckPathExists = true;
                f.CheckFileExists = false;
                f.OverwritePrompt = true;
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _FileName = f.FileName;
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                ErrorBox(SM.ErrorMessage);
                return false;
            }
        }

        /// <summary>Show warning dialog box with msg message. 
        /// Returns true when user click Ok button. </summary>
        public bool WarningBox(string _Message, string _Title = null)
        {
            if (_Title == null) _Title = SM.T(new string[] { "en:Warning", "it:Attenzione", "fr:Attention", "de:Warnung" });
            return System.Windows.Forms.MessageBox.Show(
                _Message, _Title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Warning)
                == System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>Show yes/no/cancel dialog box with msg message. 
        /// Returns -1 if user click "cancel" button, 0 for "no" button, 1 for "yes" button.</summary>
        public int YesNoCancelBox(string _Message, string _Title = null)
        {
            if (_Title == null) _Title = SM.T(new string[] { "en:Confirm", "it:Conferma", "fr:Confirmation", "de:Bestätigung" });
            System.Windows.Forms.DialogResult r = System.Windows.Forms.MessageBox.Show(
                   _Message, _Title,
                   System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                   System.Windows.Forms.MessageBoxIcon.Warning);
            if (r == System.Windows.Forms.DialogResult.Yes) return 1;
            else if (r == System.Windows.Forms.DialogResult.No) return 0;
            else return -1;
        }

        #endregion

        /* */

    }

    /* */

}
