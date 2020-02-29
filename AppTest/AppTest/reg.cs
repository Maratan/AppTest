using System;
using System.Windows.Forms;

namespace AppTest
{
    public partial class reg : Form
    {
        public reg()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, EventArgs e)
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

            TCPClient client = new TCPClient();
            //проверяем имя на уникальность
            string result_answer = client.request(String.Format("0:UniqueName,{0}", textBoxLogin.Text));
            if (result_answer == "false")
            {
                MessageBox.Show("Данное имя пользователя занято!\nПовторите попытку с другим именем.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //регистрируем
            result_answer = client.request(String.Format("0:AddUser,{0},{1}", textBoxLogin.Text, Funcs.ComputeSha256(textBoxPass.Text)));
            if (result_answer == "false")
            {
                MessageBox.Show("Произошла ошибка при регистрации, повторите попытку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else MessageBox.Show("Поздравляем, регистрация прошла успешно, введите логин и пароль для получения доступа.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
