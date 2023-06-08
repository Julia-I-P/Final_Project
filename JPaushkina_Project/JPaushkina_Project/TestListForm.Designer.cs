namespace JPaushkina_Project
{
    partial class TestListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AnxietyButton = new System.Windows.Forms.Button();
            this.SelfesteemButton = new System.Windows.Forms.Button();
            this.StressButton = new System.Windows.Forms.Button();
            this.ProgressButton = new System.Windows.Forms.Button();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AnxietyButton
            // 
            this.AnxietyButton.Location = new System.Drawing.Point(14, 185);
            this.AnxietyButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.AnxietyButton.Name = "AnxietyButton";
            this.AnxietyButton.Size = new System.Drawing.Size(162, 55);
            this.AnxietyButton.TabIndex = 0;
            this.AnxietyButton.Text = "Тревожность";
            this.AnxietyButton.UseVisualStyleBackColor = true;
            this.AnxietyButton.Click += new System.EventHandler(this.AnxietyButton_Click);
            // 
            // SelfesteemButton
            // 
            this.SelfesteemButton.Location = new System.Drawing.Point(279, 185);
            this.SelfesteemButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.SelfesteemButton.Name = "SelfesteemButton";
            this.SelfesteemButton.Size = new System.Drawing.Size(162, 55);
            this.SelfesteemButton.TabIndex = 1;
            this.SelfesteemButton.Text = "Самооценка";
            this.SelfesteemButton.UseVisualStyleBackColor = true;
            this.SelfesteemButton.Click += new System.EventHandler(this.SelfesteemButton_Click);
            // 
            // StressButton
            // 
            this.StressButton.Location = new System.Drawing.Point(542, 184);
            this.StressButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.StressButton.Name = "StressButton";
            this.StressButton.Size = new System.Drawing.Size(162, 56);
            this.StressButton.TabIndex = 2;
            this.StressButton.Text = "Стресс";
            this.StressButton.UseVisualStyleBackColor = true;
            this.StressButton.Click += new System.EventHandler(this.StressButton_Click);
            // 
            // ProgressButton
            // 
            this.ProgressButton.Location = new System.Drawing.Point(14, 13);
            this.ProgressButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ProgressButton.Name = "ProgressButton";
            this.ProgressButton.Size = new System.Drawing.Size(207, 55);
            this.ProgressButton.TabIndex = 3;
            this.ProgressButton.Text = "Пройденные тесты";
            this.ProgressButton.UseVisualStyleBackColor = true;
            this.ProgressButton.Click += new System.EventHandler(this.ProgressButton_Click);
            // 
            // LogOutButton
            // 
            this.LogOutButton.Location = new System.Drawing.Point(600, 275);
            this.LogOutButton.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(104, 55);
            this.LogOutButton.TabIndex = 4;
            this.LogOutButton.Text = "Выход";
            this.LogOutButton.UseVisualStyleBackColor = true;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(230, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "Выберите изучаемую сферу";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(80, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(592, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Добро пожаловать в программу экспресс-диагностики!";
            // 
            // TestListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 352);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.ProgressButton);
            this.Controls.Add(this.StressButton);
            this.Controls.Add(this.SelfesteemButton);
            this.Controls.Add(this.AnxietyButton);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "TestListForm";
            this.Text = "Список тестов";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AnxietyButton;
        private System.Windows.Forms.Button SelfesteemButton;
        private System.Windows.Forms.Button StressButton;
        private System.Windows.Forms.Button ProgressButton;
        private System.Windows.Forms.Button LogOutButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}