using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AppTest
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (AppTest.Properties.Settings.Default.Block_Count < 10)
                Application.Run(new Form1());
            else
                Application.Run(new block());
        }
    }
}
