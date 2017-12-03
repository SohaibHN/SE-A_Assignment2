using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_A_Assignment2
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
            //Application.Run(new AddUser());
           

            // this is done to allow the login form to be closed (not hidden) after a sucessful login.
            loginform fLogin = new loginform();
            if (fLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainApp());

            }
            else
            {
                Application.Exit();
            }
           

        }
    }
}
