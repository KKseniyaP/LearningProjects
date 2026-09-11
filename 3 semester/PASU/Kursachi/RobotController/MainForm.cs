using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach
{
    public partial class MainForm : Form
    {
        // Элементы интерфейса
        private Button btnStart, btnStop, btnReset;
        private TextBox txtLog;
        private Label lblStatus, lblPosition, lblTarget, lblDistance, lblZone;
        private Timer timer;

        // Сеть
        private readonly string robotIp = "127.0.0.1";
        private readonly int robotPort = 8080;
        private readonly int localPort = 9090;
        private UdpClient udpSend;
        private UdpClient udpReceive;
        private IPEndPoint remoteEndPoint;

        // Состояние робота
        private enum RobotState { Idle, LeavingStart, MovingToP1, AtP1, MovingToP2, AtP2, Error }
        private RobotState currentState = RobotState.Idle;

        // Данные робота
        private double robotX = 0.75;
        private double robotY = 0.75;
        private double robotCourse = 0;
        private int bumper = 0;
        private int cameraCode = 0;
        private int commandNumber = 1;

        // Координаты точек
        private readonly double p1X = 15.0;  // P1 - паллеты
        private readonly double p1Y = 2.0;

        private readonly double p2X = 7.5;   // P2 - столик
        private readonly double p2Y = 1.5;

        // Текущая цель
        private double targetX = 0.75;
        private double targetY = 0.75;
        private string targetName = "Старт";

        // Конструктор
        public MainForm()
        {
            // Настройка окна
            this.Size = new Size(800, 500);
            this.Text = "Управление роботом";
            this.BackColor = Color.White;

            // Кнопки
            btnStart = new Button
            {
                Text = "Начать",
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                BackColor = Color.LightGreen
            };
            btnStart.Click += (s, e) => StartMission();

            btnStop = new Button
            {
                Text = "Стоп",
                Location = new Point(120, 10),
                Size = new Size(100, 30),
                BackColor = Color.LightCoral,
                Enabled = false
            };
            btnStop.Click += (s, e) => Stop();

            btnReset = new Button
            {
                Text = "Сброс",
                Location = new Point(230, 10),
                Size = new Size(100, 30),
                BackColor = Color.LightGray
            };
            btnReset.Click += (s, e) => Reset();

            // Статус
            lblStatus = new Label
            {
                Text = "Состояние: Ожидание",
                Location = new Point(10, 50),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10)
            };

            lblPosition = new Label
            {
                Text = "Позиция: X=0.75 Y=0.75",
                Location = new Point(10, 75),
                Size = new Size(300, 20)
            };

            lblTarget = new Label
            {
                Text = "Цель: Старт",
                Location = new Point(10, 100),
                Size = new Size(300, 20)
            };

            lblDistance = new Label
            {
                Text = "Расстояние: --",
                Location = new Point(10, 125),
                Size = new Size(300, 20)
            };

            lblZone = new Label
            {
                Text = "Зона: --",
                Location = new Point(10, 150),
                Size = new Size(300, 20)
            };

            // Информация
            Label lblInfo = new Label
            {
                Text = "Коды зон:\n" +
                       "1 - Старт/ожидание\n" +
                       "2 - П1 (паллеты)\n" +
                       "# - P2 (столик)\n" +
                       "# - P3 (товары)\n" +
                       "5-9 - линии",
                Location = new Point(10, 180),
                Size = new Size(200, 100),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Lavender
            };

            // Лог
            txtLog = new TextBox
            {
                Location = new Point(350, 10),
                Size = new Size(430, 440),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 8)
            };

            // Добавление элементов
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);
            this.Controls.Add(btnReset);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblPosition);
            this.Controls.Add(lblTarget);
            this.Controls.Add(lblDistance);
            this.Controls.Add(lblZone);
            this.Controls.Add(lblInfo);
            this.Controls.Add(txtLog);

            // Инициализация
            InitializeNetwork();
            InitializeTimer();

            Log("Система запущена");
            Log("Ожидание данных от симулятора...");
        }

        // Инициализация сети
        private void InitializeNetwork()
        {
            try
            {
                udpSend = new UdpClient();
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(robotIp), robotPort);

                udpReceive = new UdpClient(localPort);
                udpReceive.Client.ReceiveTimeout = 100;

                Task.Run(ReceiveData);

                Log("Сеть подключена");
            }
            catch (Exception ex)
            {
                Log($"Ошибка сети: {ex.Message}");
            }
        }

        // Инициализация таймера
        private void InitializeTimer()
        {
            timer = new Timer { Interval = 100 };
            timer.Tick += (s, e) =>
            {
                // Обновление интерфейса
                UpdateUI();

                // Управление роботом
                if (currentState != RobotState.Idle && currentState != RobotState.Error)
                {
                    ProcessState();
                }
            };
            timer.Start();
        }

        // Обновление интерфейса
        private void UpdateUI()
        {
            lblStatus.Text = $"Состояние: {currentState}";
            lblPosition.Text = $"Позиция: X={robotX:F2} Y={robotY:F2} Курс={robotCourse:F1}";
            lblTarget.Text = $"Цель: {targetName}";

            // Расстояние до цели
            double dx = targetX - robotX;
            double dy = targetY - robotY;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            lblDistance.Text = $"Расстояние: {distance:F2} м";

            // Зона
            if (cameraCode > 0)
                lblZone.Text = $"Зона: код {cameraCode}";
            else
                lblZone.Text = "Зона: --";
        }

        // Обработка состояний
        private void ProcessState()
        {
            // Проверка на столкновение
            if (bumper > 0)
            {
                Log("Столкновение!");
                HandleCollision();
                return;
            }

            // Проверка зоны
            if (cameraCode == 1 && currentState == RobotState.Idle)
            {
                Log("Нахожусь в стартовой зоне");
            }
            else if (cameraCode == 2 && currentState != RobotState.AtP1)
            {
                Log("Обнаружена зона П1!");
                currentState = RobotState.AtP1;
                SendCommand(0, 0);
                return;
            }

            switch (currentState)
            {
                case RobotState.LeavingStart:
                    LeaveStartZone();
                    break;

                case RobotState.MovingToP1:
                    MoveToTarget(p1X, p1Y, "П1", RobotState.AtP1);
                    break;

                case RobotState.AtP1:
                    // Забрали паллету
                    Log("Забрал паллету с П1");
                    currentState = RobotState.MovingToP2;
                    targetX = p2X;
                    targetY = p2Y;
                    targetName = "П2";
                    break;

                case RobotState.MovingToP2:
                    MoveToTarget(p2X, p2Y, "П2", RobotState.AtP2);
                    break;

                case RobotState.AtP2:
                    Log("Достиг П2, миссия завершена");
                    Stop();
                    break;
            }
        }

        // Выезд из стартовой зоны
        private void LeaveStartZone()
        {
            // Просто едем вперед 2 секунды, чтобы выехать из зоны
            SendCommand(50, 0);

            // Если выехали достаточно далеко от старта
            if (robotY > 1.5)
            {
                Log("Выехал из стартовой зоны");
                currentState = RobotState.MovingToP1;
                targetX = p1X;
                targetY = p1Y;
                targetName = "П1";
                SendCommand(0, 0);
            }
        }

        // Движение к цели
        private void MoveToTarget(double targetX, double targetY, string name, RobotState nextState)
        {
            this.targetX = targetX;
            this.targetY = targetY;
            this.targetName = name;

            double dx = targetX - robotX;
            double dy = targetY - robotY;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Если достигли цели
            if (distance < 0.5) // 50 см
            {
                Log($"Достиг {name}");
                SendCommand(0, 0);
                currentState = nextState;
                return;
            }

            // Вычисление направления
            double targetAngle = Math.Atan2(dy, dx) * 180.0 / Math.PI;
            double angleDiff = targetAngle - robotCourse;

            // Нормализация
            while (angleDiff > 180) angleDiff -= 360;
            while (angleDiff < -180) angleDiff += 360;

            // Управление
            int speed = 0;
            int turn = 0;

            if (Math.Abs(angleDiff) > 30)
            {
                // Поворачиваем
                turn = (int)(-angleDiff * 0.4);
                speed = 0;
            }
            else
            {
                // Едем вперед
                speed = 35;
                turn = (int)(-angleDiff * 0.2);

                if (distance < 1.0) speed = 25;
                if (distance < 0.7) speed = 15;
            }

            // Ограничения
            turn = Math.Max(-70, Math.Min(70, turn));
            speed = Math.Max(-50, Math.Min(50, speed));

            SendCommand(speed, turn);
        }

        // Обработка столкновения
        private void HandleCollision()
        {
            SendCommand(0, 0);

            // Отъезжаем назад и поворачиваем
            SendCommand(-25, 30);
            Task.Delay(800).ContinueWith(t =>
            {
                SendCommand(0, 0);
                Log("Объезд выполнен");
            });
        }

        // Отправка команды
        private void SendCommand(int speed, int turn)
        {
            try
            {
                string json = $"{{\"N\":{commandNumber},\"M\":0,\"F\":{speed},\"B\":{turn},\"T\":0}}\n";
                byte[] data = Encoding.UTF8.GetBytes(json);
                udpSend.Send(data, data.Length, remoteEndPoint);

                commandNumber++;
            }
            catch (Exception ex)
            {
                Log($"Ошибка отправки: {ex.Message}");
            }
        }

        // Прием данных
        private async Task ReceiveData()
        {
            while (true)
            {
                try
                {
                    var result = await udpReceive.ReceiveAsync();
                    string json = Encoding.UTF8.GetString(result.Buffer);
                    ParseData(json);
                }
                catch
                {
                    await Task.Delay(100);
                }
            }
        }

        // Парсинг данных
        private void ParseData(string json)
        {
            try
            {
                json = json.Trim().Trim('{', '}');

                foreach (string pair in json.Split(','))
                {
                    string[] parts = pair.Split(':');
                    if (parts.Length < 2) continue;

                    string key = parts[0].Trim().Trim('"');
                    string value = parts[1].Trim().Trim('"');

                    switch (key)
                    {
                        case "x":
                            robotX = ParseNumber(value);
                            break;
                        case "y":
                            robotY = ParseNumber(value);
                            break;
                        case "t":
                            robotCourse = ParseNumber(value);
                            break;
                        case "b":
                            if (int.TryParse(value, out int b))
                                bumper = b;
                            break;
                        case "c":
                            if (int.TryParse(value, out int c))
                                cameraCode = c;
                            break;
                    }
                }
            }
            catch
            {
                // Игнорируем ошибки
            }
        }

        private double ParseNumber(string str)
        {
            str = str.Replace(',', '.');
            if (double.TryParse(str, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double result))
                return result;
            return 0;
        }

        // Начать миссию
        private void StartMission()
        {
            if (cameraCode == 1)
            {
                currentState = RobotState.LeavingStart;
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                Log("Начинаю миссию");
                Log("Выезжаю из стартовой зоны...");
            }
            else
            {
                Log("Ошибка: не в стартовой зоне!");
            }
        }

        // Стоп
        private void Stop()
        {
            currentState = RobotState.Idle;
            SendCommand(0, 0);

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            Log("Миссия остановлена");
        }

        // Сброс
        private void Reset()
        {
            currentState = RobotState.Idle;
            SendCommand(0, 0);

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            Log("Сброс системы");
        }

        // Логирование
        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                    txtLog.ScrollToCaret();
                }));
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                txtLog.ScrollToCaret();
            }
        }

        // Закрытие формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SendCommand(0, 0);
            udpSend?.Close();
            udpReceive?.Close();
            base.OnFormClosing(e);
        }
    }
}