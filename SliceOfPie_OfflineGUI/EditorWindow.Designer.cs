namespace SliceOfPie_OfflineGUI
{
    partial class EditorWindow
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.docnameBox = new System.Windows.Forms.TextBox();
            this.EditorBox = new System.Windows.Forms.TextBox();
            this.historyButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.historyButton);
            this.panel1.Controls.Add(this.saveButton);
            this.panel1.Location = new System.Drawing.Point(453, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 59);
            this.panel1.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(153, 17);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(90, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save & Exit";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Document name:";
            // 
            // docnameBox
            // 
            this.docnameBox.Location = new System.Drawing.Point(15, 40);
            this.docnameBox.Name = "docnameBox";
            this.docnameBox.Size = new System.Drawing.Size(120, 20);
            this.docnameBox.TabIndex = 5;
            this.docnameBox.Text = "Default.html";
            this.docnameBox.TextChanged += new System.EventHandler(this.docnameBox_TextChanged);
            // 
            // EditorBox
            // 
            this.EditorBox.Location = new System.Drawing.Point(15, 97);
            this.EditorBox.Multiline = true;
            this.EditorBox.Name = "EditorBox";
            this.EditorBox.Size = new System.Drawing.Size(698, 427);
            this.EditorBox.TabIndex = 6;
            this.EditorBox.Text = "Enter html here!";
            // 
            // historyButton
            // 
            this.historyButton.Location = new System.Drawing.Point(20, 17);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(113, 23);
            this.historyButton.TabIndex = 7;
            this.historyButton.Text = "View History";
            this.historyButton.UseVisualStyleBackColor = true;
            this.historyButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // EditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 536);
            this.Controls.Add(this.EditorBox);
            this.Controls.Add(this.docnameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Name = "EditorWindow";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.EditorWindow_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox docnameBox;
        private System.Windows.Forms.TextBox EditorBox;
        private System.Windows.Forms.Button historyButton;
    }
}