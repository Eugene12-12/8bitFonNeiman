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
            this.tempTactButton = new System.Windows.Forms.Button();
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
            // tempTactButton
            // 
            this.tempTactButton.Location = new System.Drawing.Point(69, 122);
            this.tempTactButton.Name = "tempTactButton";
            this.tempTactButton.Size = new System.Drawing.Size(75, 23);
            this.tempTactButton.TabIndex = 3;
            this.tempTactButton.Text = "Такт";
            this.tempTactButton.UseVisualStyleBackColor = true;
            this.tempTactButton.Click += new System.EventHandler(this.tempTactButton_Click);
            // 
            // ComponentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tempTactButton);
            this.Controls.Add(this.cpuButton);
            this.Controls.Add(this.memoryButton);
            this.Controls.Add(this.editorButton);
            this.Name = "ComponentsForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ComponentsForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button editorButton;
        private System.Windows.Forms.Button memoryButton;
        private System.Windows.Forms.Button cpuButton;
        private System.Windows.Forms.Button tempTactButton;
    }
}

