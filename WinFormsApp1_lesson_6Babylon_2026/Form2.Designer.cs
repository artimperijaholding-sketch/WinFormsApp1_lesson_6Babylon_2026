namespace WinFormsApp1_lesson_6Babylon_2026
{
    partial class Form2
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
            buttonScan = new Button();
            buttonLoadScripts = new Button();
            listBoxScripts = new ListBox();
            textBoxSkanResult = new TextBox();
            textBoxInput = new TextBox();
            objectName = new Label();
            pictureBoxLogo = new PictureBox();
            textBoxTranslationResult = new TextBox();
            buttonTranslate = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // buttonScan
            // 
            buttonScan.Location = new Point(12, 12);
            buttonScan.Name = "buttonScan";
            buttonScan.Size = new Size(75, 23);
            buttonScan.TabIndex = 0;
            buttonScan.Text = "Scan";
            buttonScan.UseVisualStyleBackColor = true;
            buttonScan.Click += buttonScan_Click;
            // 
            // buttonLoadScripts
            // 
            buttonLoadScripts.Location = new Point(242, 12);
            buttonLoadScripts.Name = "buttonLoadScripts";
            buttonLoadScripts.Size = new Size(75, 23);
            buttonLoadScripts.TabIndex = 1;
            buttonLoadScripts.Text = "Scripts";
            buttonLoadScripts.UseVisualStyleBackColor = true;
            buttonLoadScripts.Click += buttonLoadScripts_Click;
            // 
            // listBoxScripts
            // 
            listBoxScripts.FormattingEnabled = true;
            listBoxScripts.Location = new Point(242, 41);
            listBoxScripts.Name = "listBoxScripts";
            listBoxScripts.Size = new Size(151, 289);
            listBoxScripts.TabIndex = 2;
            listBoxScripts.SelectedIndexChanged += listBoxScripts_SelectedIndexChanged;
            // 
            // textBoxSkanResult
            // 
            textBoxSkanResult.Location = new Point(12, 41);
            textBoxSkanResult.Multiline = true;
            textBoxSkanResult.Name = "textBoxSkanResult";
            textBoxSkanResult.ScrollBars = ScrollBars.Vertical;
            textBoxSkanResult.Size = new Size(211, 289);
            textBoxSkanResult.TabIndex = 3;
            // 
            // textBoxInput
            // 
            textBoxInput.Location = new Point(426, 165);
            textBoxInput.Name = "textBoxInput";
            textBoxInput.Size = new Size(181, 23);
            textBoxInput.TabIndex = 4;
            // 
            // objectName
            // 
            objectName.AutoSize = true;
            objectName.Location = new Point(426, 41);
            objectName.Name = "objectName";
            objectName.Size = new Size(77, 15);
            objectName.TabIndex = 5;
            objectName.Text = "Object_name";
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Location = new Point(426, 59);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(100, 100);
            pictureBoxLogo.TabIndex = 6;
            pictureBoxLogo.TabStop = false;
            // 
            // textBoxTranslationResult
            // 
            textBoxTranslationResult.Location = new Point(426, 212);
            textBoxTranslationResult.Multiline = true;
            textBoxTranslationResult.Name = "textBoxTranslationResult";
            textBoxTranslationResult.Size = new Size(330, 189);
            textBoxTranslationResult.TabIndex = 7;
            // 
            // buttonTranslate
            // 
            buttonTranslate.Location = new Point(681, 165);
            buttonTranslate.Name = "buttonTranslate";
            buttonTranslate.Size = new Size(75, 23);
            buttonTranslate.TabIndex = 8;
            buttonTranslate.Text = "Translate";
            buttonTranslate.UseVisualStyleBackColor = true;
            buttonTranslate.Click += buttonTranslate_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(768, 413);
            Controls.Add(buttonTranslate);
            Controls.Add(textBoxTranslationResult);
            Controls.Add(pictureBoxLogo);
            Controls.Add(objectName);
            Controls.Add(textBoxInput);
            Controls.Add(textBoxSkanResult);
            Controls.Add(listBoxScripts);
            Controls.Add(buttonLoadScripts);
            Controls.Add(buttonScan);
            Name = "Form2";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonScan;
        private Button buttonLoadScripts;
        private ListBox listBoxScripts;
        private TextBox textBoxSkanResult;
        private TextBox textBoxInput;
        private Label objectName;
        private PictureBox pictureBoxLogo;
        private TextBox textBoxTranslationResult;
        private Button buttonTranslate;
    }
}