namespace LR2_IoTRobotWorld
{
    partial class LR2_IoTRobotWorld
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.txtD0 = new System.Windows.Forms.TextBox();
            this.txtD1 = new System.Windows.Forms.TextBox();
            this.txtD6 = new System.Windows.Forms.TextBox();
            this.txtD7 = new System.Windows.Forms.TextBox();
            this.groupBoxD = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBumper = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLe = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRe = new System.Windows.Forms.TextBox();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.trbDist = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tmrControl = new System.Windows.Forms.Timer(this.components);
            this.groupBoxAuto = new System.Windows.Forms.GroupBox();
            this.txtDistView = new System.Windows.Forms.TextBox();
            this.txtSpeedView = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnManualMode = new System.Windows.Forms.Button();
            this.groupBoxManual = new System.Windows.Forms.GroupBox();
            this.btnManualStop = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.groupBoxD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbDist)).BeginInit();
            this.groupBoxAuto.SuspendLayout();
            this.groupBoxManual.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstLog
            // 
            this.lstLog.AccessibleDescription = "Лог пришедших сообщений (RX)";
            this.lstLog.FormattingEnabled = true;
            this.lstLog.ItemHeight = 16;
            this.lstLog.Location = new System.Drawing.Point(52, 415);
            this.lstLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(396, 244);
            this.lstLog.TabIndex = 0;
            // 
            // txtD0
            // 
            this.txtD0.Location = new System.Drawing.Point(8, 34);
            this.txtD0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtD0.Name = "txtD0";
            this.txtD0.Size = new System.Drawing.Size(132, 22);
            this.txtD0.TabIndex = 1;
            // 
            // txtD1
            // 
            this.txtD1.Location = new System.Drawing.Point(8, 84);
            this.txtD1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtD1.Name = "txtD1";
            this.txtD1.Size = new System.Drawing.Size(132, 22);
            this.txtD1.TabIndex = 2;
            // 
            // txtD6
            // 
            this.txtD6.Location = new System.Drawing.Point(8, 127);
            this.txtD6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtD6.Name = "txtD6";
            this.txtD6.Size = new System.Drawing.Size(132, 22);
            this.txtD6.TabIndex = 3;
            // 
            // txtD7
            // 
            this.txtD7.Location = new System.Drawing.Point(8, 174);
            this.txtD7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtD7.Name = "txtD7";
            this.txtD7.Size = new System.Drawing.Size(132, 22);
            this.txtD7.TabIndex = 4;
            // 
            // groupBoxD
            // 
            this.groupBoxD.Controls.Add(this.label4);
            this.groupBoxD.Controls.Add(this.label3);
            this.groupBoxD.Controls.Add(this.label2);
            this.groupBoxD.Controls.Add(this.label1);
            this.groupBoxD.Controls.Add(this.txtD0);
            this.groupBoxD.Controls.Add(this.txtD7);
            this.groupBoxD.Controls.Add(this.txtD1);
            this.groupBoxD.Controls.Add(this.txtD6);
            this.groupBoxD.Controls.Add(this.label7);
            this.groupBoxD.Controls.Add(this.txtBumper);
            this.groupBoxD.Controls.Add(this.label6);
            this.groupBoxD.Controls.Add(this.txtLe);
            this.groupBoxD.Controls.Add(this.label5);
            this.groupBoxD.Controls.Add(this.txtRe);
            this.groupBoxD.Location = new System.Drawing.Point(52, 37);
            this.groupBoxD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxD.Name = "groupBoxD";
            this.groupBoxD.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxD.Size = new System.Drawing.Size(367, 361);
            this.groupBoxD.TabIndex = 6;
            this.groupBoxD.TabStop = false;
            this.groupBoxD.Text = "Мониторинг (RX)";
            this.groupBoxD.Enter += new System.EventHandler(this.groupBoxD_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 174);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "d7 лев";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 127);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "d6 перед лев";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 87);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "d1 перед пр";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "d0 перед";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(151, 332);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 16);
            this.label7.TabIndex = 13;
            this.label7.Text = "re энк. прав";
            // 
            // txtBumper
            // 
            this.txtBumper.Location = new System.Drawing.Point(8, 231);
            this.txtBumper.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBumper.Name = "txtBumper";
            this.txtBumper.Size = new System.Drawing.Size(132, 22);
            this.txtBumper.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(151, 286);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "le энк. лев";
            // 
            // txtLe
            // 
            this.txtLe.Location = new System.Drawing.Point(8, 284);
            this.txtLe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLe.Name = "txtLe";
            this.txtLe.Size = new System.Drawing.Size(132, 22);
            this.txtLe.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 231);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "b бампер";
            // 
            // txtRe
            // 
            this.txtRe.Location = new System.Drawing.Point(8, 332);
            this.txtRe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRe.Name = "txtRe";
            this.txtRe.Size = new System.Drawing.Size(132, 22);
            this.txtRe.TabIndex = 11;
            // 
            // trbSpeed
            // 
            this.trbSpeed.Location = new System.Drawing.Point(8, 64);
            this.trbSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trbSpeed.Maximum = 100;
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.Size = new System.Drawing.Size(320, 56);
            this.trbSpeed.TabIndex = 18;
            this.trbSpeed.TickFrequency = 10;
            this.trbSpeed.Value = 45;
            // 
            // trbDist
            // 
            this.trbDist.Location = new System.Drawing.Point(8, 192);
            this.trbDist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trbDist.Maximum = 200;
            this.trbDist.Minimum = 10;
            this.trbDist.Name = "trbDist";
            this.trbDist.Size = new System.Drawing.Size(328, 56);
            this.trbDist.TabIndex = 19;
            this.trbDist.TickFrequency = 10;
            this.trbDist.Value = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 159);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(145, 16);
            this.label10.TabIndex = 20;
            this.label10.Text = "До левой стенки в см";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 33);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 16);
            this.label11.TabIndex = 21;
            this.label11.Text = "Скорость (0..100)";
            // 
            // tmrControl
            // 
            this.tmrControl.Tick += new System.EventHandler(this.TmrControl_Tick);
            // 
            // groupBoxAuto
            // 
            this.groupBoxAuto.Controls.Add(this.txtDistView);
            this.groupBoxAuto.Controls.Add(this.txtSpeedView);
            this.groupBoxAuto.Controls.Add(this.lblStatus);
            this.groupBoxAuto.Controls.Add(this.trbSpeed);
            this.groupBoxAuto.Controls.Add(this.label11);
            this.groupBoxAuto.Controls.Add(this.btnStop);
            this.groupBoxAuto.Controls.Add(this.trbDist);
            this.groupBoxAuto.Controls.Add(this.btnStart);
            this.groupBoxAuto.Controls.Add(this.label10);
            this.groupBoxAuto.Location = new System.Drawing.Point(455, 37);
            this.groupBoxAuto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAuto.Name = "groupBoxAuto";
            this.groupBoxAuto.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAuto.Size = new System.Drawing.Size(368, 361);
            this.groupBoxAuto.TabIndex = 22;
            this.groupBoxAuto.TabStop = false;
            this.groupBoxAuto.Text = "Автоматический режим";
            // 
            // txtDistView
            // 
            this.txtDistView.Location = new System.Drawing.Point(237, 160);
            this.txtDistView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDistView.Name = "txtDistView";
            this.txtDistView.Size = new System.Drawing.Size(89, 22);
            this.txtDistView.TabIndex = 28;
            // 
            // txtSpeedView
            // 
            this.txtSpeedView.Location = new System.Drawing.Point(237, 31);
            this.txtSpeedView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSpeedView.Name = "txtSpeedView";
            this.txtSpeedView.Size = new System.Drawing.Size(89, 22);
            this.txtSpeedView.TabIndex = 27;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(8, 320);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(45, 16);
            this.lblStatus.TabIndex = 26;
            this.lblStatus.Text = "Готов";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(93, 273);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 28);
            this.btnStop.TabIndex = 24;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(8, 273);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(77, 28);
            this.btnStart.TabIndex = 23;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnManualMode
            // 
            this.btnManualMode.Location = new System.Drawing.Point(23, 49);
            this.btnManualMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnManualMode.Name = "btnManualMode";
            this.btnManualMode.Size = new System.Drawing.Size(167, 55);
            this.btnManualMode.TabIndex = 25;
            this.btnManualMode.Text = "Переключиться на ручной режим";
            this.btnManualMode.UseVisualStyleBackColor = true;
            this.btnManualMode.Click += new System.EventHandler(this.btnManualMode_Click);
            // 
            // groupBoxManual
            // 
            this.groupBoxManual.Controls.Add(this.btnManualStop);
            this.groupBoxManual.Controls.Add(this.btnLeft);
            this.groupBoxManual.Controls.Add(this.btnRight);
            this.groupBoxManual.Controls.Add(this.btnBackward);
            this.groupBoxManual.Controls.Add(this.btnForward);
            this.groupBoxManual.Controls.Add(this.btnManualMode);
            this.groupBoxManual.Location = new System.Drawing.Point(879, 37);
            this.groupBoxManual.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxManual.Name = "groupBoxManual";
            this.groupBoxManual.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxManual.Size = new System.Drawing.Size(355, 151);
            this.groupBoxManual.TabIndex = 26;
            this.groupBoxManual.TabStop = false;
            this.groupBoxManual.Text = "Ручное управление";
            // 
            // btnManualStop
            // 
            this.btnManualStop.Location = new System.Drawing.Point(259, 62);
            this.btnManualStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnManualStop.Name = "btnManualStop";
            this.btnManualStop.Size = new System.Drawing.Size(33, 31);
            this.btnManualStop.TabIndex = 29;
            this.btnManualStop.Text = "■";
            this.btnManualStop.UseVisualStyleBackColor = true;
            this.btnManualStop.Click += new System.EventHandler(this.BtnManualStop_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(217, 62);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(33, 31);
            this.btnLeft.TabIndex = 28;
            this.btnLeft.Text = "◀";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(300, 62);
            this.btnRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(33, 31);
            this.btnRight.TabIndex = 27;
            this.btnRight.Text = "▶";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.BtnRight_Click);
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point(259, 96);
            this.btnBackward.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(33, 31);
            this.btnBackward.TabIndex = 26;
            this.btnBackward.Text = "▼";
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.BtnBackward_Click);
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(259, 23);
            this.btnForward.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(33, 31);
            this.btnForward.TabIndex = 6;
            this.btnForward.Text = "▲";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.BtnForward_Click);
            // 
            // LR2_IoTRobotWorld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 674);
            this.Controls.Add(this.groupBoxManual);
            this.Controls.Add(this.groupBoxAuto);
            this.Controls.Add(this.groupBoxD);
            this.Controls.Add(this.lstLog);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LR2_IoTRobotWorld";
            this.Text = "LR2_IoTRobotWorld";
            this.groupBoxD.ResumeLayout(false);
            this.groupBoxD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbDist)).EndInit();
            this.groupBoxAuto.ResumeLayout(false);
            this.groupBoxAuto.PerformLayout();
            this.groupBoxManual.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.TextBox txtD0;
        private System.Windows.Forms.TextBox txtD1;
        private System.Windows.Forms.TextBox txtD6;
        private System.Windows.Forms.TextBox txtD7;
        private System.Windows.Forms.GroupBox groupBoxD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBumper;
        private System.Windows.Forms.TextBox txtLe;
        private System.Windows.Forms.TextBox txtRe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trbSpeed;
        private System.Windows.Forms.TrackBar trbDist;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Timer tmrControl;
        private System.Windows.Forms.GroupBox groupBoxAuto;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnManualMode;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtDistView;
        private System.Windows.Forms.TextBox txtSpeedView;
        private System.Windows.Forms.GroupBox groupBoxManual;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnManualStop;
    }
}

