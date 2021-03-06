﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Settings.Read();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Log.WriteLine("");
            Application.Run(new mainWindow());
        }
        static void OnProcessExit(object sender, EventArgs e)
        {
            mainWindow.CloseEngineInstances();
            Settings.save();
        }
        internal static string appname()
        {
           return "Epd Tester";
        }
        internal static string versionStr()
        {
            return "0.0.1";
        }
    }
}
