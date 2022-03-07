namespace Minesweeper
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minesweeperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reversiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gamesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1878, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gamesToolStripMenuItem
            // 
            this.gamesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minesweeperToolStripMenuItem,
            this.reversiToolStripMenuItem});
            this.gamesToolStripMenuItem.Name = "gamesToolStripMenuItem";
            this.gamesToolStripMenuItem.Size = new System.Drawing.Size(82, 29);
            this.gamesToolStripMenuItem.Text = "Games";
            // 
            // minesweeperToolStripMenuItem
            // 
            this.minesweeperToolStripMenuItem.Name = "minesweeperToolStripMenuItem";
            this.minesweeperToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.minesweeperToolStripMenuItem.Text = "Minesweeper";
            this.minesweeperToolStripMenuItem.Click += new System.EventHandler(this.minesweeperToolStripMenuItem_Click);
            // 
            // reversiToolStripMenuItem
            // 
            this.reversiToolStripMenuItem.Name = "reversiToolStripMenuItem";
            this.reversiToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.reversiToolStripMenuItem.Text = "Reversi";
            this.reversiToolStripMenuItem.Click += new System.EventHandler(this.reversiToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1878, 1581);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Games";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minesweeperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reversiToolStripMenuItem;
    }
}

