namespace SliceOfPie_OfflineGUI
{
    partial class MainWindow
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
            this.button_tree = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.button_load = new System.Windows.Forms.Button();
            this.button_synchronize = new System.Windows.Forms.Button();
            this.button_Create = new System.Windows.Forms.Button();
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
            // button_tree
            // 
            this.button_tree.Location = new System.Drawing.Point(359, 52);
            this.button_tree.Name = "button_tree";
            this.button_tree.Size = new System.Drawing.Size(75, 23);
            this.button_tree.TabIndex = 1;
            this.button_tree.Text = "Load Tree";
            this.button_tree.UseVisualStyleBackColor = true;
            this.button_tree.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(547, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(359, 92);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(75, 23);
            this.button_load.TabIndex = 4;
            this.button_load.Text = "Load File";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // button_synchronize
            // 
            this.button_synchronize.Location = new System.Drawing.Point(359, 131);
            this.button_synchronize.Name = "button_synchronize";
            this.button_synchronize.Size = new System.Drawing.Size(75, 23);
            this.button_synchronize.TabIndex = 5;
            this.button_synchronize.Text = "Synchronize";
            this.button_synchronize.UseVisualStyleBackColor = true;
            this.button_synchronize.Click += new System.EventHandler(this.button_synchronize_Click);
            // 
            // button_Create
            // 
            this.button_Create.Location = new System.Drawing.Point(359, 179);
            this.button_Create.Name = "button_Create";
            this.button_Create.Size = new System.Drawing.Size(75, 23);
            this.button_Create.TabIndex = 6;
            this.button_Create.Text = "Create Document";
            this.button_Create.UseVisualStyleBackColor = true;
            this.button_Create.Click += new System.EventHandler(this.button_Create_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 386);
            this.Controls.Add(this.button_Create);
            this.Controls.Add(this.button_synchronize);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_tree);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button_tree;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_synchronize;
        private System.Windows.Forms.Button button_Create;
    }
}