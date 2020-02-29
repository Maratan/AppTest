using System.Security.Cryptography;
using System.Text;

namespace AppTest
{
    class Funcs
    {
        //Вычисление ХЭШ-функции для пароля
        public static string ComputeSha256(string rawData)
        {
            // Создаём SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Вычисляем ХЭШ - возвращаем массив битов  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Переводим биты в строку 
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }  

        //Функция блокировки пользователя за 10 неправильных попыток
        public static void Block()
        {
            if (AppTest.Properties.Settings.Default.Block_Count < 10)
            {
                AppTest.Properties.Settings.Default.Block_Count++;
                AppTest.Properties.Settings.Default.Save();
            }
            else
            { 
            
            }
        }
    }
}
