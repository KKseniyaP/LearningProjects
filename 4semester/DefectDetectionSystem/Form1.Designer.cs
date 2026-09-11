namespace DefectDetectionSystem
{
    partial class Form1
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
            this.groupBoxRef = new System.Windows.Forms.GroupBox();
            this.btnLoadRef = new System.Windows.Forms.Button();
            this.pictureBoxRef = new System.Windows.Forms.PictureBox();
            this.groupBoxSample = new System.Windows.Forms.GroupBox();
            this.btnLoadSample = new System.Windows.Forms.Button();
            this.pictureBoxSample = new System.Windows.Forms.PictureBox();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.openFileDialogRef = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogSample = new System.Windows.Forms.OpenFileDialog();
            this.lblDefect = new System.Windows.Forms.Label();
            this.groupBoxRef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRef)).BeginInit();
            this.groupBoxSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).BeginInit();
            this.groupBoxResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxRef
            // 
            this.groupBoxRef.Controls.Add(this.btnLoadRef);
            this.groupBoxRef.Controls.Add(this.pictureBoxRef);
            this.groupBoxRef.Location = new System.Drawing.Point(40, 38);
            this.groupBoxRef.Name = "groupBoxRef";
            this.groupBoxRef.Size = new System.Drawing.Size(320, 420);
            this.groupBoxRef.TabIndex = 0;
            this.groupBoxRef.TabStop = false;
            this.groupBoxRef.Text = "Эталон";
            // 
            // btnLoadRef
            // 
            this.btnLoadRef.Location = new System.Drawing.Point(7, 336);
            this.btnLoadRef.Name = "btnLoadRef";
            this.btnLoadRef.Size = new System.Drawing.Size(160, 35);
            this.btnLoadRef.TabIndex = 1;
            this.btnLoadRef.Text = "Загрузить эталон";
            this.btnLoadRef.UseVisualStyleBackColor = true;
            this.btnLoadRef.Click += new System.EventHandler(this.btnLoadRef_Click);
            // 
            // pictureBoxRef
            // 
            this.pictureBoxRef.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxRef.Name = "pictureBoxRef";
            this.pictureBoxRef.Size = new System.Drawing.Size(290, 310);
            this.pictureBoxRef.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRef.TabIndex = 0;
            this.pictureBoxRef.TabStop = false;
            // 
            // groupBoxSample
            // 
            this.groupBoxSample.Controls.Add(this.btnLoadSample);
            this.groupBoxSample.Controls.Add(this.pictureBoxSample);
            this.groupBoxSample.Location = new System.Drawing.Point(378, 38);
            this.groupBoxSample.Name = "groupBoxSample";
            this.groupBoxSample.Size = new System.Drawing.Size(320, 420);
            this.groupBoxSample.TabIndex = 0;
            this.groupBoxSample.TabStop = false;
            this.groupBoxSample.Text = "Образец";
            // 
            // btnLoadSample
            // 
            this.btnLoadSample.Location = new System.Drawing.Point(6, 336);
            this.btnLoadSample.Name = "btnLoadSample";
            this.btnLoadSample.Size = new System.Drawing.Size(160, 35);
            this.btnLoadSample.TabIndex = 2;
            this.btnLoadSample.Text = "Загрузить образец";
            this.btnLoadSample.UseVisualStyleBackColor = true;
            this.btnLoadSample.Click += new System.EventHandler(this.btnLoadSample_Click);
            // 
            // pictureBoxSample
            // 
            this.pictureBoxSample.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxSample.Name = "pictureBoxSample";
            this.pictureBoxSample.Size = new System.Drawing.Size(290, 310);
            this.pictureBoxSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSample.TabIndex = 1;
            this.pictureBoxSample.TabStop = false;
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Controls.Add(this.lblDefect);
            this.groupBoxResult.Controls.Add(this.btnCompare);
            this.groupBoxResult.Controls.Add(this.pictureBoxResult);
            this.groupBoxResult.Location = new System.Drawing.Point(720, 38);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(320, 420);
            this.groupBoxResult.TabIndex = 1;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "Результат";
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(6, 336);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(160, 35);
            this.btnCompare.TabIndex = 3;
            this.btnCompare.Text = "Сравнить и найти дефекты";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(290, 310);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 2;
            this.pictureBoxResult.TabStop = false;
            // 
            // openFileDialogRef
            // 
            this.openFileDialogRef.FileName = "openFileDialog1";
            // 
            // openFileDialogSample
            // 
            this.openFileDialogSample.FileName = "openFileDialog1";
            // 
            // lblDefect
            // 
            this.lblDefect.AutoSize = true;
            this.lblDefect.Location = new System.Drawing.Point(6, 385);
            this.lblDefect.Name = "lblDefect";
            this.lblDefect.Size = new System.Drawing.Size(58, 13);
            this.lblDefect.TabIndex = 4;
            this.lblDefect.Text = "Дефекты:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 561);
            this.Controls.Add(this.groupBoxResult);
            this.Controls.Add(this.groupBoxSample);
            this.Controls.Add(this.groupBoxRef);
            this.Name = "Form1";
            this.Text = "Система контроля печатной продукции";
            this.groupBoxRef.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRef)).EndInit();
            this.groupBoxSample.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).EndInit();
            this.groupBoxResult.ResumeLayout(false);
            this.groupBoxResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxRef;
        private System.Windows.Forms.Button btnLoadRef;
        private System.Windows.Forms.PictureBox pictureBoxRef;
        private System.Windows.Forms.GroupBox groupBoxSample;
        private System.Windows.Forms.Button btnLoadSample;
        private System.Windows.Forms.PictureBox pictureBoxSample;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.OpenFileDialog openFileDialogRef;
        private System.Windows.Forms.OpenFileDialog openFileDialogSample;
        private System.Windows.Forms.Label lblDefect;
    }
}

