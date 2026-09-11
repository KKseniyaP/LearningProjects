using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DefectDetectionSystem
{
    public partial class Form1 : Form
    {
        private Mat refImage;
        private Mat sampleImage;
        private string rootPath;

        private const int TARGET_WIDTH = 800;   // фиксированная ширина
        private const int TARGET_HEIGHT = 1000; // фиксированная высота

        public Form1()
        {
            InitializeComponent();

            Logger.Clear();
            Logger.Log("=== ПРОГРАММА ЗАПУЩЕНА ===");

            pictureBoxRef.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSample.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxResult.SizeMode = PictureBoxSizeMode.Zoom;

            string executablePath = Application.StartupPath;
            DirectoryInfo di = new DirectoryInfo(executablePath);
            rootPath = di.Parent.Parent.Parent.FullName;

            CreateFolders();
        }

        // Приводим все изображения к ОДИНАКОВОМУ размеру
        private Mat ResizeImageToFixed(Mat src)
        {
            Mat dst = new Mat();
            Cv2.Resize(src, dst, new Size(TARGET_WIDTH, TARGET_HEIGHT));
            Logger.Log($"Изменён размер: {src.Width}x{src.Height} -> {dst.Width}x{dst.Height}");
            return dst;
        }

        private void CreateFolders()
        {
            string templatesPath = Path.Combine(rootPath, "Templates");
            string samplesPath = Path.Combine(rootPath, "Samples");

            if (!Directory.Exists(templatesPath))
                Directory.CreateDirectory(templatesPath);
            if (!Directory.Exists(samplesPath))
                Directory.CreateDirectory(samplesPath);
        }

        private void btnLoadRef_Click(object sender, EventArgs e)
        {
            string templatesPath = Path.Combine(rootPath, "Templates");

            openFileDialogRef.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialogRef.Title = "Выберите эталонное изображение";
            openFileDialogRef.InitialDirectory = templatesPath;

            if (openFileDialogRef.ShowDialog() == DialogResult.OK)
            {
                Mat original = Cv2.ImRead(openFileDialogRef.FileName);
                refImage = ResizeImageToFixed(original);  // ← фиксированный размер
                original.Dispose();

                pictureBoxRef.Image = BitmapConverter.ToBitmap(refImage);
                groupBoxRef.Text = $"Эталон: {Path.GetFileName(openFileDialogRef.FileName)}";
            }
        }

        private void btnLoadSample_Click(object sender, EventArgs e)
        {
            string samplesPath = Path.Combine(rootPath, "Samples");

            openFileDialogSample.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialogSample.Title = "Выберите контрольный образец";
            openFileDialogSample.InitialDirectory = samplesPath;

            if (openFileDialogSample.ShowDialog() == DialogResult.OK)
            {
                Mat original = Cv2.ImRead(openFileDialogSample.FileName);
                sampleImage = ResizeImageToFixed(original);  // ← фиксированный размер
                original.Dispose();

                pictureBoxSample.Image = BitmapConverter.ToBitmap(sampleImage);
                groupBoxSample.Text = $"Образец: {Path.GetFileName(openFileDialogSample.FileName)}";
            }
        }

        // Анализ типа дефекта на основе разницы
        private (string defectType, double percentage, Mat heatmap) AnalyzeDefect(Mat diff, Mat grayRef, Mat graySample)
        {
            // Статистика по разнице
            double minVal, maxVal;
            Cv2.MinMaxLoc(diff, out minVal, out maxVal);

            float totalPixels = diff.Width * diff.Height;

            // Светлые пиксели (разница > 50) — возможный засвет / недопечатка
            Mat brightDiff = new Mat();
            Cv2.Threshold(diff, brightDiff, 50, 255, ThresholdTypes.Binary);
            int brightPixels = Cv2.CountNonZero(brightDiff);
            double brightPercent = brightPixels * 100.0 / totalPixels;

            // Тёмные пиксели (разница > 30) — возможный смаз / лишняя краска
            Mat darkDiff = new Mat();
            Cv2.Threshold(diff, darkDiff, 30, 255, ThresholdTypes.Binary);
            int darkPixels = Cv2.CountNonZero(darkDiff);
            double darkPercent = darkPixels * 100.0 / totalPixels;

            // Сравнение средней яркости
            Scalar meanRef = Cv2.Mean(grayRef);
            Scalar meanSample = Cv2.Mean(graySample);
            double brightnessDiff = meanSample.Val0 - meanRef.Val0;

            // Классификация
            string defectType;
            double percentage;
            Mat heatmap = new Mat();

            if (brightnessDiff > 30 && brightPercent > 10)
            {
                defectType = "ЗАСВЕТ / НЕДОПЕЧАТКА";
                percentage = brightPercent;
                Cv2.Threshold(diff, heatmap, 80, 255, ThresholdTypes.Binary);
            }
            else if (brightnessDiff < -20 && darkPercent > 15)
            {
                defectType = "СМАЗ / ПЕРЕПЕЧАТКА";
                percentage = darkPercent;
                Cv2.Threshold(diff, heatmap, 30, 255, ThresholdTypes.Binary);
            }
            else if (brightPercent > 5)
            {
                defectType = "ЗАСВЕТ";
                percentage = brightPercent;
                Cv2.Threshold(diff, heatmap, 80, 255, ThresholdTypes.Binary);
            }
            else if (darkPercent > 8)
            {
                defectType = "ЛЁГКИЙ СМАЗ";
                percentage = darkPercent;
                Cv2.Threshold(diff, heatmap, 30, 255, ThresholdTypes.Binary);
            }
            else
            {
                defectType = "НОРМА";
                percentage = 0;
                heatmap = new Mat(diff.Size(), MatType.CV_8UC1, Scalar.Black);
            }

            // Очистка
            brightDiff.Dispose();
            darkDiff.Dispose();

            return (defectType, percentage, heatmap);
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            Logger.Log("=== НАЖАТА КНОПКА СРАВНЕНИЯ ===");

            if (refImage == null || sampleImage == null)
            {
                MessageBox.Show("Загрузите и эталон, и образец!", "Ошибка");
                return;
            }

            // Проверка размеров
            if (refImage.Width != sampleImage.Width || refImage.Height != sampleImage.Height)
            {
                Logger.Log($"ОШИБКА РАЗМЕРОВ: эталон={refImage.Width}x{refImage.Height}, образец={sampleImage.Width}x{sampleImage.Height}");
                MessageBox.Show($"Изображения разного размера!\nЭталон: {refImage.Width}x{refImage.Height}\nОбразец: {sampleImage.Width}x{sampleImage.Height}\n\nПожалуйста, перезагрузите изображения.", "Ошибка");
                return;
            }

            try
            {
                // Конвертация в серый
                Mat grayRef = new Mat();
                Mat graySample = new Mat();
                Cv2.CvtColor(refImage, grayRef, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(sampleImage, graySample, ColorConversionCodes.BGR2GRAY);

                // Выравнивание гистограммы
                Cv2.EqualizeHist(grayRef, grayRef);
                Cv2.EqualizeHist(graySample, graySample);

                // Вычитание
                Mat diff = new Mat();
                Cv2.Absdiff(graySample, grayRef, diff);

                // Анализ дефекта
                var (defectType, percentage, heatmap) = AnalyzeDefect(diff, grayRef, graySample);

                // Подготовка результата для отображения
                Mat result = sampleImage.Clone();

                // Если есть дефект, показываем тепловую карту на изображении
                if (defectType != "НОРМА")
                {
                    // Превращаем тепловую карту в цветную для наложения
                    Mat heatmapColor = new Mat();
                    Cv2.CvtColor(heatmap, heatmapColor, ColorConversionCodes.GRAY2BGR);

                    // Накладываем красным цветом на оригинал
                    for (int y = 0; y < heatmap.Height; y++)
                    {
                        for (int x = 0; x < heatmap.Width; x++)
                        {
                            byte val = heatmap.At<byte>(y, x);
                            if (val > 0)
                            {
                                Vec3b color = result.At<Vec3b>(y, x);
                                color[0] = (byte)(color[0] * 0.3 + 0 * 0.7);     // B
                                color[1] = (byte)(color[1] * 0.3 + 0 * 0.7);     // G
                                color[2] = 255;                                    // R - красный
                                result.Set(y, x, color);
                            }
                        }
                    }
                    heatmapColor.Dispose();
                }

                // Добавляем текст с результатом
                string resultText;
                if (defectType == "НОРМА")
                {
                    resultText = "ГОДЕН - дефектов не обнаружено";
                    lblDefect.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    resultText = $"{defectType}: {percentage:F1}% области";
                }

                // Рисуем текст на изображении
                Cv2.PutText(result, resultText, new Point(10, 30),
                    HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 2);
                Cv2.PutText(result, $"Средняя яркость эталона: {Cv2.Mean(grayRef).Val0:F0}", new Point(10, 60),
                    HersheyFonts.HersheySimplex, 0.5, new Scalar(255, 255, 0), 1);
                Cv2.PutText(result, $"Средняя яркость образца: {Cv2.Mean(graySample).Val0:F0}", new Point(10, 85),
                    HersheyFonts.HersheySimplex, 0.5, new Scalar(255, 255, 0), 1);

                // Показываем результат
                pictureBoxResult.Image = BitmapConverter.ToBitmap(result);
                lblDefect.Text = defectType == "НОРМА" ? "ГОДЕН" : $"ДЕФЕКТ: {defectType}";

                // Дополнительно показываем карту различий
                Cv2.ImShow("Карта дефектов (белое = отличие)", diff);
                Cv2.MoveWindow("Карта дефектов (белое = отличие)", 100, 100);

                Logger.Log($"РЕЗУЛЬТАТ: {defectType}, процент={percentage:F1}");

                // Сообщение
                if (defectType == "НОРМА")
                {
                    MessageBox.Show("Брак не обнаружен. Изделие годно.", "Результат контроля");
                }
                else
                {
                    MessageBox.Show($"{defectType}: {percentage:F1}% изображения отличается от эталона.\n\nСредняя яркость эталона: {Cv2.Mean(grayRef).Val0:F0}\nСредняя яркость образца: {Cv2.Mean(graySample).Val0:F0}", "Результат контроля");
                }

                // Очистка
                grayRef.Dispose();
                graySample.Dispose();
                diff.Dispose();
                heatmap.Dispose();
                result.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log($"ОШИБКА: {ex.Message}");
                MessageBox.Show($"Ошибка: {ex.Message}", "Критическая ошибка");
            }
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            Logger.ShowLog();
        }
    }
}