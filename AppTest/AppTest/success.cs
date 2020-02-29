using System;
using System.Windows.Forms;

namespace AppTest
{
    public partial class success : Form
    {
        public success()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //Завершение авторизации
            Environment.Exit(0);
        }
    }
}
