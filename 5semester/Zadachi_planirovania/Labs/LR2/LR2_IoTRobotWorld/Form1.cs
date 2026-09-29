using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LR2_IoTRobotWorld
{
    /// <summary>
    /// Главная форма приложения-сервера для управления мобильным роботом
    /// в симуляторе IoTRobotWorld (лабораторная работа № 2, Вариант 1).
    ///
    /// Реализует:
    /// <list type="bullet">
    ///   <item>UDP-сервер для приёма телеметрии от симулятора;</item>
    ///   <item>UDP-клиент для отправки управляющих команд роботу;</item>
    ///   <item>парсинг JSON-сообщений с датчиков робота;</item>
    ///   <item>алгоритм движения «обход стены слева» по кольцевой трассе;</item>
    ///   <item>визуализацию мониторинговых данных и лога обмена.</item>
    /// </list>
    /// </summary>
    /// 
    public partial class LR2_IoTRobotWorld : Form
    {
        // ==== СЕТЬ ====
        private UdpClient udpListener;   // приём данных от робота
        private UdpClient udpSender;     // отправка команд роботу
        private IPEndPoint remoteEndPoint;
        private readonly string localIp = "127.0.0.1";
        private readonly int listenPort = 5001;
        private readonly int sendPort = 5000;

        private Thread listenThread;
        private volatile bool isRunning = false;
        private int commandCounter = 0;

        // ==== ДАННЫЕ ОТ РОБОТА ====
        private double d0 = 0, d1 = 0, d6 = 0, d7 = 0;
        private int bumper = 0;
        private int le = 0, re = 0;

        // ==== ЛОГИКА УПРАВЛЕНИЯ (Вариант 1: обход стены слева) ====
        private const int BASE_SPEED = 45;         // базовая скорость
        private const int TARGET_DIST = 30;        // целевое расстояние до левой стены, см
        private const double DIST_DEADZONE = 5.0;  // мёртвая зона по дистанции
        private const double FRONT_STOP = 25.0;    // порог срабатывания переднего дальномера
        private const int MAX_BALANCE = 40;        // ограничение руления

        // ==== РЕЖИМ РАБОТЫ ====
        private enum ControlMode { Idle, Auto, Manual }
        private ControlMode mode = ControlMode.Idle;

        // ==== СОСТОЯНИЕ РУЧНОГО РЕЖИМА ====
        private int manualF = 0; //Текущая тяга в ручном режиме (F).
        private int manualB = 0; //Текущий баланс в ручном режиме (B).
        private bool udpReady = false;//Признак того, что UDP-сокеты открыты для обмена.

        public LR2_IoTRobotWorld()
        {
            InitializeComponent();
            SetManualEnabled(false);

            txtSpeedView.Text = trbSpeed.Value.ToString();
            txtDistView.Text = trbDist.Value.ToString();

            trbSpeed.Scroll += (s, e) => txtSpeedView.Text = trbSpeed.Value.ToString();
            trbDist.Scroll += (s, e) => txtDistView.Text = trbDist.Value.ToString();

            this.FormClosing += (s, e) => Shutdown();
        }

        // =================================================================
        //  СЕТЬ
        // =================================================================

        /// <summary>
        /// Инициализирует UDP-сокеты: bind на listenPort для приёма,
        /// создаёт endpoint симулятора для отправки.
        /// </summary>----------------------------------------------------------
        private void InitUdp()
        {
            udpListener = new UdpClient();
            udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpListener.Client.Bind(new IPEndPoint(IPAddress.Any, listenPort));

            udpSender = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(localIp), sendPort);
        }

        /// <summary>
        /// Проверяет, готов ли UDP к отправке команд. Если нет — пытается открыть
        /// сокеты «на лету». Возвращает true, если можно отправлять.
        /// </summary>
        private bool EnsureUdpReady()
        {
            if (udpReady) return true;

            try
            {
                InitUdp();
                udpReady = true;
                SafeLog("=== UDP открыт для ручного режима ===");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть UDP: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Запускает фоновый поток приёма телеметрии (если ещё не запущен).
        /// </summary>
        private void StartListening()
        {
            if (isRunning) return;
            isRunning = true;
            listenThread = new Thread(ListenLoop) { IsBackground = true };
            listenThread.Start();
        }

        /// <summary>
        /// Закрывает сокеты и останавливает таймер при выходе.
        /// </summary>
        private void Shutdown()
        {
            tmrControl?.Stop();
            isRunning = false;
            udpReady = false;
            mode = ControlMode.Idle;

            try { udpListener?.Close(); } catch { }
            try { udpSender?.Close(); } catch { }
        }

        /// <summary>
        /// Фоновый цикл приёма UDP-пакетов. Парсит JSON и обновляет поля формы.
        /// </summary>-----------------------------------------------------------
        private void ListenLoop()
        {
            while (isRunning)
            {
                try
                {
                    IPEndPoint senderEp = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = udpListener.Receive(ref senderEp);
                    string json = Encoding.UTF8.GetString(data).Trim();

                    ParseSensorData(json);
                    SafeLog($"[RX] {json}");
                }
                catch (Exception ex)
                {
                    if (isRunning) SafeLog($"[RX-ERR] {ex.Message}");
                }
            }
        }

        // =================================================================
        //  ЛОГ И ПАРСИНГ
        // =================================================================

        /// <summary>
        /// Потокобезопасно добавляет строку в лог.
        /// </summary>
        private void SafeLog(string text)
        {
            if (lstLog.InvokeRequired)
                lstLog.BeginInvoke(new Action(() => AppendLog(text)));
            else
                AppendLog(text);
        }

        /// <summary>
        /// Добавляет строку в listBox лога с ограничением размера.
        /// </summary>
        private void AppendLog(string text)
        {
            // Ограничение размера лога
            if (lstLog.Items.Count > 300) lstLog.Items.RemoveAt(0);
            lstLog.Items.Add(text);
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }

        /// <summary>
        /// Разбирает JSON-строку с датчиков и обновляет глобальные переменные + UI.
        /// Устойчив к пробелам, кавычкам и мусору в значениях.
        /// </summary>-----------------------------------------------------
        private void ParseSensorData(string json)
        {
            double GetVal(string key)
            {
                string search = "\"" + key + "\"";
                int start = json.IndexOf(search);
                if (start == -1) return 0;

                start += search.Length;
                while (start < json.Length && (json[start] == ' ' || json[start] == ':')) start++;

                int end = start;
                while (end < json.Length && json[end] != ',' && json[end] != '}' &&
                       json[end] != '\r' && json[end] != '\n') end++;

                if (end > start)
                {
                    string s = json.Substring(start, end - start).Trim(' ', '"');
                    if (double.TryParse(s, System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out double r))
                        return r;
                }
                return 0;
            }

            // Обновляем поля (потокобезопасно)
            UpdateTextBox(txtD0, d0 = GetVal("d0"));
            UpdateTextBox(txtD1, d1 = GetVal("d1"));
            UpdateTextBox(txtD6, d6 = GetVal("d6"));
            UpdateTextBox(txtD7, d7 = GetVal("d7"));
            bumper = (int)GetVal("b");
            le = (int)GetVal("le");
            re = (int)GetVal("re");

            UpdateTextBox(txtBumper, bumper);
            UpdateTextBox(txtLe, le);
            UpdateTextBox(txtRe, re);
        }

        /// <summary>
        /// Потокобезопасно обновляет текст текстового поля.
        /// </summary>
        private void UpdateTextBox(TextBox tb, double value)
        {
            if (tb == null) return;
            if (tb.InvokeRequired)
                tb.BeginInvoke(new Action(() => tb.Text = value.ToString("F1")));
            else
                tb.Text = value.ToString("F1");
        }


        // ---------------------------------------------------------------
        // КНОПКИ
        // ---------------------------------------------------------------

        /// <summary>
        /// Обработчик кнопки «СТАРТ»: инициализирует UDP, запускает приём
        /// и включает таймер автоматического управления.
        /// </summary>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (mode == ControlMode.Auto) return;

            if (mode == ControlMode.Manual)
            {
                MessageBox.Show("Сначала остановите ручной режим кнопкой «Стоп».",
                    "Ручной режим активен",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!EnsureUdpReady()) return;
            StartListening();

            mode = ControlMode.Auto;

            tmrControl.Start();

            SetManualEnabled(false);
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnManualMode.Enabled = false;

            lblStatus.Text = "АВТО-РЕЖИМ: обход стены слева";
            lblStatus.ForeColor = Color.Green;
            SafeLog("=== АВТО-РЕЖИМ ===");
        }


        /// <summary>
        /// Обработчик кнопки «СТОП»: останавливает робота и таймер.
        /// </summary>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            StopAll();
        }

        /// <summary>
        /// Останавливает автоматический режим: посылает нулевую команду,
        /// останавливает таймер, закрывает сокеты.
        /// </summary>
        private void StopAll()
        {
            tmrControl.Stop();

            try { SendCommand(0, 0, 0, 0); } catch { /* ignore */ }

            mode = ControlMode.Idle;
            manualF = 0;
            manualB = 0;

            SetManualEnabled(false);
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnManualMode.Enabled = true;

            lblStatus.Text = "ОСТАНОВЛЕНО";
            lblStatus.ForeColor = Color.Red;
            SafeLog("=== СТОП ===");
        }

        /// <summary>
        /// Переключение в ручной режим управления.
        /// Открывает UDP (если ещё не открыт), запускает поток приёма телеметрии,
        /// запускает таймер и разрешает кнопки ▲ ▼ ◀ ▶ ■.
        ///
        /// Вызывается по нажатию кнопки «▶ Ручной режим» (btnSendCustom).
        /// </summary>
        private void btnManualMode_Click(object sender, EventArgs e)
        {
            if (mode == ControlMode.Manual) return;

            if (mode == ControlMode.Auto)
            {
                MessageBox.Show("Сначала остановите авто-режим кнопкой «Стоп».",
                    "Авто-режим активен",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!EnsureUdpReady()) return;
            StartListening();

            mode = ControlMode.Manual;
            manualF = 0;
            manualB = 0;

            tmrControl.Start();

            SetManualEnabled(true);
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnManualMode.Enabled = false;

            lblStatus.Text = "РУЧНОЙ РЕЖИМ";
            lblStatus.ForeColor = Color.DarkOrange;
            SafeLog("=== РУЧНОЙ РЕЖИМ ===");
        }

        /// <summary>
        /// Включает или выключает все элементы панели ручного управления.
        /// Используется для взаимоисключения ручного и авто-режимов.
        /// </summary>
        private void SetManualEnabled(bool enabled)
        {
            btnForward.Enabled = enabled;
            btnBackward.Enabled = enabled;
            btnLeft.Enabled = enabled;
            btnRight.Enabled = enabled;
            btnManualStop.Enabled = enabled;
        }

        // =================================================================
        //  РУЧНОЕ УПРАВЛЕНИЕ
        // =================================================================

        /// <summary>Кнопка «▲»: движение по прямой с текущей тягой.</summary>
        private void BtnForward_Click(object sender, EventArgs e)
        {
            if (mode != ControlMode.Manual) return;

            manualF = trbSpeed.Value;
            manualB = 0;
            SafeLog($"[MANUAL] Вперёд: F={manualF}, B={manualB}");
        }

        /// <summary>Кнопка «▼»: движение назад по прямой.</summary>
        private void BtnBackward_Click(object sender, EventArgs e)
        {
            if (mode != ControlMode.Manual) return;

            manualF = -trbSpeed.Value;
            manualB = 0;
            SafeLog($"[MANUAL] Назад: F={manualF}, B={manualB}");
        }

        /// <summary>Кнопка «◀»: вращение на месте влево (правое колесо вперёд).</summary>
        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (mode != ControlMode.Manual) return;

            manualF = 0;
            manualB = -40;
            SafeLog($"[MANUAL] Вправо: F={manualF}, B={manualB}");
        }

        /// <summary>Кнопка «▶»: вращение на месте вправо (левое колесо вперёд).</summary>
        private void BtnRight_Click(object sender, EventArgs e)
        {
            if (mode != ControlMode.Manual) return;

            manualF = 0;
            manualB = 40;
            SafeLog("[MANUAL] Стоп: F={manualF}, B={manualB}");
        }

        /// <summary>Кнопка «■»: полная остановка моторов.</summary>
        private void BtnManualStop_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            if (!EnsureUdpReady()) return;

            manualF = 0;
            manualB = 0;
            SafeLog($" [MANUAL] Стоп: F=0, B=0");
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        // =================================================================
        //  ТАЙМЕР И АЛГОРИТМЫ
        // =================================================================
        /// <summary>
        /// Тик таймера (10 Гц). Поведение зависит от текущего режима:
        /// <list type="bullet">
        ///   <item><b>Auto</b> — алгоритм обхода стены слева.</item>
        ///   <item><b>Manual</b> — непрерывная отправка состояния ручных кнопок.</item>
        ///   <item><b>Idle</b> — ничего не шлём (таймер обычно остановлен).</item>
        /// </list>
        /// </summary>
        private void TmrControl_Tick(object sender, EventArgs e)
        {
            switch (mode)
            {
                case ControlMode.Auto:
                    AutoTick();
                    break;

                case ControlMode.Manual:
                    SendCommand(0, manualF, manualB, 0);
                    // В логе не спамим — только статус в label
                    lblStatus.Text = $"РУЧНОЙ: F={manualF}, B={manualB}";
                    lblStatus.ForeColor = Color.DarkOrange;
                    break;

                case ControlMode.Idle:
                    // Ничего не делаем
                    break;
            }
        }

        /// <summary>
        /// Один шаг авто-алгоритма: обход стены слева.
        /// </summary>
        private void AutoTick()
        {
            int speed = trbSpeed.Value;
            int targetDist = trbDist.Value;

            // ---- 1. Аварийные условия ----
            bool frontObstacle = (d0 > 0 && d0 < FRONT_STOP) ||
                                 (d1 > 0 && d1 < FRONT_STOP);

            if (bumper == 1 || frontObstacle)
            {
                SendCommand(0, -35, 30, 0);
                lblStatus.Text = bumper == 1
                    ? "БАМПЕР: откат назад + поворот вправо"
                    : $"ПРЕПЯТСТВИЕ (d0={d0:F0}): откат + поворот";
                lblStatus.ForeColor = Color.OrangeRed;
                return;
            }

            // ---- 2. Основной алгоритм ----
            double wallDist = d7 > 0 ? d7 : d6;
            double error = targetDist - wallDist;

            if (Math.Abs(error) < DIST_DEADZONE || wallDist <= 0)
            {
                SendCommand(0, speed, 0, 0);
                lblStatus.Text = $"РОВНО: стена={wallDist:F0} см";
            }
            else
            {
                int balance = (int)(error * 1.2);
                balance = Math.Max(-MAX_BALANCE, Math.Min(MAX_BALANCE, balance));
                balance = -balance;

                int curSpeed = speed;
                if (Math.Abs(balance) > MAX_BALANCE * 0.7)
                    curSpeed = Math.Max(15, (int)(speed * 0.6));

                SendCommand(0, curSpeed, balance, 0);
                lblStatus.Text = $"ПОДРУЛИВАНИЕ: стена={wallDist:F0}, B={balance}";
            }

            lblStatus.ForeColor = Color.Green;
        }

        /// <summary>
        /// Отправляет управляющую команду роботу в формате JSON.
        /// </summary>
        /// <param name="M">Режим управления (0 — непосредственный).</param>
        /// <param name="F">Продольная тяга обоих моторов, -100..100.</param>
        /// <param name="B">Баланс между моторами, -100..100 (+ в пользу правого).</param>
        /// <param name="T">Дополнительный параметр (не используется).</param>
        private void SendCommand(int M, int F, int B, int T)
        {
            if (udpSender == null) return;

            commandCounter++;
            // Ограничение тяги -100..100
            F = Math.Max(-100, Math.Min(100, F));
            B = Math.Max(-100, Math.Min(100, B));

            string json = $"{{\"N\":{commandCounter}, \"M\":{M}, \"F\":{F}, \"B\":{B}, \"T\":{T}}}\n";
            byte[] data = Encoding.UTF8.GetBytes(json);

            try
            {
                udpSender.Send(data, data.Length, remoteEndPoint);
                SafeLog($"[TX] {json.Trim()}");
            }
            catch (Exception ex)
            {
                SafeLog($"[TX-ERR] {ex.Message}");
            }
        }

        // ---------------------------------------------------------------
        // ЗАГЛУШКИ
        // ---------------------------------------------------------------
        private void groupBoxD_Enter(object sender, EventArgs e)
        {

        }
    }
}
