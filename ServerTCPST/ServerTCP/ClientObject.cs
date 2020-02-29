using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerTCP
{
    class ClientObject
    {
        public TcpClient client;

        //конструктор класса
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        //обработка запроса TCP
        public void ProcessTCP()
        {
            NetworkStream stream = null;

            try
            {
                //принимаем подключение клиента
                stream = client.GetStream();

                //получаем порции данных
                byte[] data = new byte[64]; //буфер для получаемых данных

                while (true)
                { 
                    //получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    //читаем порции данных
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    //собираем пакет
                    string message =  builder.ToString();

                    //разбиваем пакет 
                    string[] masMes = message.Split(new char[] {':'});
                    
                    //проверяем структуру запроса
                    if (masMes.Length != 2)
                    {
                        Console.WriteLine(message);
                        Console.WriteLine("Неверный формат запроса!\nОтказ в обслуживании");

                        if (stream != null)
                            stream.Close();

                        if (client != null)
                            client.Close();

                        return;
                    }

                    //получаем id сессии пользователя
                    string sessionId = masMes[0];

                    //обрезаем сообщение, оставляя только запрос
                    message = masMes[1];

                    //делаем задержку 
                    Thread.Sleep(1000);

                    //выводим в консоль
                    Console.WriteLine(message);
                    Console.WriteLine(sessionId);
                       
                    //передаём запрос на обработку
                    string answerM = Func.handler(sessionId, message);
                   
                    //делаем задержку 
                    Thread.Sleep(1000);

                    //отправляем обратно
                    byte[] answer = Encoding.UTF8.GetBytes(answerM);
                    //data = Encoding.UTF8.GetBytes(cert);
                    stream.Write(answer, 0, answer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:\n", ex);
                //отправляем обратно

                byte[] data = new byte[64]; //буфер для получаемых данных
                data = Encoding.UTF8.GetBytes("false");

                stream.Write(data, 0, data.Length);
            }
            finally
            {
                if (stream != null)
                    stream.Close();

                if (client != null)
                    client.Close();
            }
        }
    }
}
