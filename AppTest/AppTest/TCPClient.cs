using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AppTest
{
    class TCPClient
    {
        static int port = 8888; //порт подключения
        static string address = "127.0.0.1"; //адрес сервера

        string result = ""; //строка для ответа

        //функция отправки запроса на сервер
        public string request(string dataRequest) //параметр типа string
        {
            //инициализация переменной для создания подключения с сервером
            TcpClient client = null; 

            try
            {
                //задаём настройки подключения
                client = new TcpClient(address, port);

                //выполняем подключение
                NetworkStream stream = client.GetStream();

                //остаёмся подключенными до тех пор, пока не закрыто соединение
                while (true)
                {
                    //преобразуем сообщения
                    byte[] data = Encoding.UTF8.GetBytes(dataRequest);

                    //отправка сообщения
                    stream.Write(data, 0, data.Length);

                    //получаем ответ
                    data = new byte[64]; //буфер для получаемых данных

                    //собирательная строка
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    //получаем ответ порциями по 64 байта
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    //собираем порции в единый ответ
                    result = builder.ToString();

                    //проверяем наличие ошибок
                    if (result == "Error")
                        continue;
                    else
                        return (result);
                }
            }
            catch (Exception ex) //обработка исключений
            {
                return "no_connect: \n" + ex;
            }
            finally
            {
                client.Close(); //закрытие соединения
            }
        }
    }
}
