using System;
using System.Windows.Forms;

namespace AppTest
{
    public partial class signin : Form
    {
        public signin()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            //проверка на пустые значения полей
            if (textBoxLogin.Text == "")
            {
                MessageBox.Show("Пустое поле логина!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //проверка на пустые значения полей
            if (textBoxPass.Text == "")
            {
                MessageBox.Show("Пустое поле пароля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //отправляем запрос на сервер
            TCPClient Request = new TCPClient();
            string answer = Request.request(String.Format("0:CheckUser,{0},{1}", textBoxLogin.Text, Funcs.ComputeSha256(textBoxPass.Text)));
            if (answer == "true")
            {
                success form = new success();
                form.ShowDialog();
            }
            else
                Funcs.Block();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBoxPass.UseSystemPasswordChar = false;
            else
                textBoxPass.UseSystemPasswordChar = true;
        }
    }
}
