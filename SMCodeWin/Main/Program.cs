/*  ===========================================================================
 *  
 *  File:       Program.cs
 *  Version:    1.0.0
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

namespace SMCodeWin
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
            Application.Run(new Form1(new SMCode.SMApplication(args, "")));
        }

        /* */

    }

    /* */

}