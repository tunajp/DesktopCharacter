using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopCharacter
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
            //Application.Run(new Form1());

            try
            {
                Form1 form = new Form1();
                form.Show();
                // Application.Runではなく自分でループを作成
                while (form.Created)
                {
                    form.MainLoop();
                    Application.DoEvents();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
