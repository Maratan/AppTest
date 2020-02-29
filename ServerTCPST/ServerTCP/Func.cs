using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ServerTCP
{
    class Func
    {
        //функция открытия сединения с БД
        private static MySqlConnection GetDBConnection(string host, int port, string database, string username, string password)
        {
            // Connection String.
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }

        //настройки подключения к серверу
        private static MySqlConnection GetDBConnection()
        {
            string host = "127.0.0.1";
            int port = 3306;
            string database = "AppTest";
            string username = "Toxa";
            string password = "1234";

            return GetDBConnection(host, port, database, username, password);
        }

        private static string SQLSelect(string commandSQL)
        {
            string result = "";

            try
            {
                MySqlConnection conn = GetDBConnection();
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = commandSQL;

                result = Convert.ToString(cmd.ExecuteScalar());

                conn.Close();
            }
            catch
            {
                Console.WriteLine("Произошел разрыв соединения.\nУбедитесь в подключении компьютера к сети.");
            }

            return result;
        }

        private static void SQLInsert(string commandSQL)
        {
            try
            {
                MySqlConnection conn = GetDBConnection();
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = commandSQL;
                int count = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                Console.WriteLine("Произошел разрыв соединения.\nУбедитесь в подключении компьютера к сети.");
            }
        }

        private static void SQLUpdate(string commandSQL)
        {
            SQLInsert(commandSQL);
        }

        private static string AddUser(string request)
        {
            try
            {
                //делим строку для получения логина и пароля
                string[] date = request.Split(new char[',']);

                //добавляем пользователя
                SQLInsert(String.Format("INSERT INTO Users (login, pass) VALUES('{0}', '{1}')", date[0], date[1]));

                //получаем id добавленного пользователя
                string UserID = SQLSelect(String.Format("SELECT id FROM Users WHERE login = '{0}'", date[0]));

                //логирование
                Log(UserID, "Зарегистрирован новый пользователь", DateTime.Today.ToString());

                return "true";
            }
            catch
            {
                return "false";
            }
        }

        private static string CheckUser(string request)
        {
            //делим строку для получения логина и пароля
            string[] date = request.Split(new char[',']);

            //ищем логин в БД
            if (SQLSelect(String.Format("SELECT id FROM Users WHERE login = '{0}'", date[0])) == "")
                return "false";

            //проверяем правильность пароля
            if (SQLSelect(String.Format("SELECT pass FROM Users WHERE login = '{0}'", date[0])) != date[1])
                return "false";

            return "true";
        }

        private static bool CheckSession(string sessionid)
        { 
            //получение записи о состоянии сессии в текущий момент
            string status = SQLSelect("SELECT endTime FROM Sessions WHERE id = " + sessionid);

            if (status != "") return false;
            else return true;
        }

        private static void Log(string userId, string description, string date)
        {
            SQLInsert("INSERT INTO Log (userId, description, date) VALUES ()");
        }

        //проверка имени на уникальность
        private static string UniqueName(string request)
        {
             //делим строку для получения логина и пароля
            string[] date = request.Split(new char[] { ',' });

            string ChechUnique = SQLSelect(String.Format("SELECT id FROM Users WHERE login = '{0}'", date[1]));
            if (ChechUnique == "")
                return "true";
            else
                return "false";
        }
        public static string handler(string sessionid, string request)
        {
            string result = ""; //строка результата

            //разбиваем строку на массив+
            //делим строку для получения логина и пароля
            string[] date = request.Split(new char[] { ',' });

            switch (date[0])
            { 
                case "UniqueName":
                    result = UniqueName(request);
                    break;
                case "AddUser":
                    result = AddUser(request);
                    break;
                case "CheckUser":
                    request = CheckUser(request);
                    break;
            }
            return result;
        }
    }
}
