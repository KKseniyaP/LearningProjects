using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IoTRobotController
{
    class Program
    {
        // Настройки сети (должны совпадать с настройками в окне симулятора)
        static string localIp = "127.0.0.1";
        static int listenPort = 5001;  // Порт, на котором мы слушаем данные ОТ робота
        static int sendPort = 5000;    // Порт, на который мы шлем команды РОБОТУ

        static UdpClient udpListener;
        static UdpClient udpSender;
        static IPEndPoint remoteEndPoint;

        // Данные от робота
        static double d0 = 0, d1 = 0, d7 = 0; // Дальномеры (см)
        static int bumper = 0;                 // Бампер (0 - нет контакта, 1 - контакт)
        static int le = 0, re = 0;             // Энкодеры
        static bool isRunning = true;

        // Счетчик команд для протокола
        static int commandCounter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("=== IoTRobot Controller: Variant 1 ===");
            Console.WriteLine("Ожидание подключения к симулятору...");

            try
            {
                InitUdp();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА инициализации портов: {ex.Message}");
                Console.WriteLine("Завершите все предыдущие запуски программы и попробуйте снова.");
                Console.ReadLine();
                return;
            }

            // Запуск потока прослушивания данных от робота
            Thread listenThread = new Thread(ListenLoop);
            listenThread.Start();

            // Основной цикл управления
            ControlLoop();

            // Очистка
            isRunning = false;
            udpListener.Close();
            udpSender.Close();
            Console.WriteLine("Программа завершена.");
        }

        static void InitUdp()
        {
            udpListener = new UdpClient();
            // Разрешаем повторное использование порта для избежания ошибок при перезапуске
            udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpListener.Client.Bind(new IPEndPoint(IPAddress.Any, listenPort));

            udpSender = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(localIp), sendPort);
        }

        // Поток чтения данных от симулятора
        static void ListenLoop()
        {
            while (isRunning)
            {
                try
                {
                    IPEndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpListener.Receive(ref senderEP);
                    string json = Encoding.UTF8.GetString(data).Trim();

                    // Логирование полученных данных (RX)
                    //Console.WriteLine($"[RX <- Server] {json}"); 

                    ParseSensorData(json);
                }
                catch (Exception ex)
                {
                    if (isRunning) Console.WriteLine($"\nОшибка приема: {ex.Message}");
                }
            }
        }

        // Улучшенный парсер строки JSON, устойчивый к пробелам и кавычкам
        static void ParseSensorData(string json)
        {
            double GetVal(string key)
            {
                string searchStr = "\"" + key + "\"";
                int start = json.IndexOf(searchStr);
                if (start == -1) return 0;

                start += searchStr.Length;

                // Пропускаем пробелы и двоеточие
                while (start < json.Length && (json[start] == ' ' || json[start] == ':'))
                {
                    start++;
                }

                int end = start;
                // Ищем конец значения (запятая, закрывающая скобка или конец строки)
                while (end < json.Length && json[end] != ',' && json[end] != '}' && json[end] != '\r' && json[end] != '\n')
                {
                    end++;
                }

                if (end > start)
                {
                    // Убираем кавычки, если значение было строкой ("15" -> 15)
                    string valStr = json.Substring(start, end - start).Trim(' ', '"');
                    if (double.TryParse(valStr, out double res)) return res;
                }
                return 0;
            }

            // Обновляем глобальные переменные
            d0 = GetVal("d0"); // Передний дальномер
            d1 = GetVal("d1"); // Правый передний
            d7 = GetVal("d7"); // Левый передний
            bumper = (int)GetVal("b");
            le = (int)GetVal("le");
            re = (int)GetVal("re");
        }

        // Логика управления (Вариант 1)
        static void ControlLoop()
        {
            Console.WriteLine("Начало миссии: Движение вперед до препятствия (< 15 см)");

            int forwardSpeed = 30; // Скорость вперед (проценты, макс 100)
            int balance = 0;       // Баланс колес (0 - прямо)

            bool obstacleDetected = false;
            string stopReason = "";

            while (!obstacleDetected && isRunning)
            {
                // Проверка условий остановки (Вариант 1)
                if (bumper == 1)
                {
                    obstacleDetected = true;
                    stopReason = "Сработал контактный бампер";
                }
                // Проверка дальномеров (менее 15 см, но больше 0, чтобы игнорировать ошибки считывания)
                else if ((d0 > 0 && d0 < 15) || (d1 > 0 && d1 < 15) || (d7 > 0 && d7 < 15))
                {
                    obstacleDetected = true;
                    stopReason = $"Дальномер обнаружил препятствие (d0:{d0:F0}, d1:{d1:F0}, d7:{d7:F0})";
                }

                if (!obstacleDetected)
                {
                    // Отправляем команду движения
                    SendCommand(0, forwardSpeed, balance, 0);

                    // Вывод статуса в консоль в одну строку (перезаписывает предыдущую)
                    Console.CursorLeft = 0;
                    Console.Write($"[МОНИТОРИНГ] d0: {d0,4:F0} см | d1: {d1,4:F0} см | d7: {d7,4:F0} см | Бампер: {bumper}   ");

                    // Задержка ~100 мс (10 команд в секунду, что безопасно для UDP буфера)
                    Thread.Sleep(100);
                }
            }

            // Команда остановки
            Console.WriteLine("\n");
            Console.WriteLine("=== ОТПРАВКА КОМАНДЫ ОСТАНОВКИ ===");
            SendCommand(0, 0, 0, 0);

            Console.WriteLine($"\n*** РОБОТ ОСТАНОВЛЕН ***");
            Console.WriteLine($"Причина: {stopReason}");
            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }

        // Формирование и отправка JSON команды
        // M - режим (0), F - тяга, B - баланс, T - доп параметр
        static void SendCommand(int M, int F, int B, int T)
        {
            commandCounter++; // Увеличиваем счетчик команд при каждой отправке (требование протокола)

            // Формируем JSON вручную
            string json = $"{{\"N\":{commandCounter}, \"M\":{M}, \"F\":{F}, \"B\":{B}, \"T\":{T}}}\n";
            byte[] data = Encoding.UTF8.GetBytes(json);

            try
            {
                udpSender.Send(data, data.Length, remoteEndPoint);

                // ЛОГИРОВАНИЕ ОТПРАВЛЯЕМЫХ ДАННЫХ (Transmission data to server)
                // .Trim() убирает перенос строки \n для красивого вывода в консоль
                Console.WriteLine($"\n[TX -> Server] {json.Trim()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка отправки: {ex.Message}");
            }
        }
    }
}