﻿namespace _8bitVonNeiman.Controller.View {
    partial class ComponentsForm {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.editorButton = new System.Windows.Forms.Button();
            this.memoryButton = new System.Windows.Forms.Button();
            this.cpuButton = new System.Windows.Forms.Button();
            this.debugButton = new System.Windows.Forms.Button();
            this.externalDevicesManagerButton = new System.Windows.Forms.Button();
            this.openAllButton = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // editorButton
            // 
            this.editorButton.Location = new System.Drawing.Point(12, 12);
            this.editorButton.Name = "editorButton";
            this.editorButton.Size = new System.Drawing.Size(95, 23);
            this.editorButton.TabIndex = 0;
            this.editorButton.Text = "Компилятор";
            this.editorButton.UseVisualStyleBackColor = true;
            this.editorButton.Click += new System.EventHandler(this.editorButton_Click);
            // 
            // memoryButton
            // 
            this.memoryButton.Location = new System.Drawing.Point(113, 12);
            this.memoryButton.Name = "memoryButton";
            this.memoryButton.Size = new System.Drawing.Size(75, 23);
            this.memoryButton.TabIndex = 1;
            this.memoryButton.Text = "Память";
            this.memoryButton.UseVisualStyleBackColor = true;
            this.memoryButton.Click += new System.EventHandler(this.memoryButton_Click);
            // 
            // cpuButton
            // 
            this.cpuButton.Location = new System.Drawing.Point(12, 41);
            this.cpuButton.Name = "cpuButton";
            this.cpuButton.Size = new System.Drawing.Size(95, 23);
            this.cpuButton.TabIndex = 2;
            this.cpuButton.Text = "Процессор";
            this.cpuButton.UseVisualStyleBackColor = true;
            this.cpuButton.Click += new System.EventHandler(this.cpuButton_Click);
            // 
            // debugButton
            // 
            this.debugButton.Location = new System.Drawing.Point(113, 41);
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(75, 23);
            this.debugButton.TabIndex = 3;
            this.debugButton.Text = "Отладка";
            this.debugButton.UseVisualStyleBackColor = true;
            this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
            // 
            // externalDevicesManagerButton
            // 
            this.externalDevicesManagerButton.Location = new System.Drawing.Point(12, 70);
            this.externalDevicesManagerButton.Name = "externalDevicesManagerButton";
            this.externalDevicesManagerButton.Size = new System.Drawing.Size(176, 23);
            this.externalDevicesManagerButton.TabIndex = 4;
            this.externalDevicesManagerButton.Text = " Диспетчер внешних устройств";
            this.externalDevicesManagerButton.UseVisualStyleBackColor = true;
            this.externalDevicesManagerButton.Click += new System.EventHandler(this.externalDevicesManagerButton_Click);
            // 
            // openAllButton
            // 
            this.openAllButton.Location = new System.Drawing.Point(12, 128);
            this.openAllButton.Name = "openAllButton";
            this.openAllButton.Size = new System.Drawing.Size(176, 23);
            this.openAllButton.TabIndex = 5;
            this.openAllButton.Text = "Открыть все";
            this.openAllButton.UseVisualStyleBackColor = true;
            this.openAllButton.Click += new System.EventHandler(this.openAllButton_Click);
            // 
            // aboutButton
            // 
            this.aboutButton.Location = new System.Drawing.Point(12, 157);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(176, 23);
            this.aboutButton.TabIndex = 6;
            this.aboutButton.Text = "О программе";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // ComponentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 201);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.openAllButton);
            this.Controls.Add(this.externalDevicesManagerButton);
            this.Controls.Add(this.debugButton);
            this.Controls.Add(this.cpuButton);
            this.Controls.Add(this.memoryButton);
            this.Controls.Add(this.editorButton);
            this.Location = new System.Drawing.Point(80, 40);
            this.Name = "ComponentsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ЭВМ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ComponentsForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button editorButton;
        private System.Windows.Forms.Button memoryButton;
        private System.Windows.Forms.Button cpuButton;
        private System.Windows.Forms.Button debugButton;
		private System.Windows.Forms.Button externalDevicesManagerButton;
        private System.Windows.Forms.Button openAllButton;
        private System.Windows.Forms.Button aboutButton;
    }
}

