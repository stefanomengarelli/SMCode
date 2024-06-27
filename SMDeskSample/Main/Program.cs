/*  ===========================================================================
 *  
 *  File:       Program.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Application entry point. 
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using SMDeskSystem;
using System.ComponentModel.Design.Serialization;

namespace SMDeskSample
{

    /* */

    /// <summary>Application entry point.</summary>
    internal static class Program
    {

        /* */

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            SMDesk desk = new SMDesk(args, "", "");
            desk.Databases.Add(
                _Alias: "MAIN",
                _Type: SMDatabaseType.Mdb,
                _Hist
                _Path:
                _User:"Admin");:
            Application.Run(new Form1(desk));
        }

        /* */

    }

    /* */

}