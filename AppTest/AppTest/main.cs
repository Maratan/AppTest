using System;
using System.Windows.Forms;

namespace AppTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, EventArgs e)
        {
            signin form = new signin();
            form.ShowDialog();
        }

        private void Reg_Click(object sender, EventArgs e)
        {
            reg form = new reg();
            form.ShowDialog();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
