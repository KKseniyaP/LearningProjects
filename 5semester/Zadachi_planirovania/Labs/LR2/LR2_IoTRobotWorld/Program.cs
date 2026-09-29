using System;
using System.Windows.Forms;

namespace LR2_IoTRobotWorld
{
    /// <summary>
    /// Точка входа приложения. Запускает главную форму системы управления роботом.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа. Инициализирует WinForms-приложение и открывает форму.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ВАЖНО: имя класса должно совпадать с именем формы (см. Form1.cs)
            Application.Run(new LR2_IoTRobotWorld());
        }
    }
}