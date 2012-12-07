namespace SliceOfPie_OfflineGUI
{
    partial class Form3
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
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.button1 = new System.Windows.Forms.Button();
      this.treeView2 = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.Location = new System.Drawing.Point(21, 52);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(305, 307);
      this.treeView1.TabIndex = 0;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(359, 52);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // treeView2
      // 
      this.treeView2.Location = new System.Drawing.Point(385, 162);
      this.treeView2.Name = "treeView2";
      this.treeView2.Size = new System.Drawing.Size(121, 97);
      this.treeView2.TabIndex = 2;
      // 
      // Form3
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(547, 386);
      this.Controls.Add(this.treeView2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.treeView1);
      this.Name = "Form3";
      this.Text = "Form3";
      this.Load += new System.EventHandler(this.Form3_Load);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView2;
    }
}