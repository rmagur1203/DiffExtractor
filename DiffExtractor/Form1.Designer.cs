namespace DiffExtractor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.originalGroupBox = new System.Windows.Forms.GroupBox();
            this.selectOriginal = new System.Windows.Forms.Button();
            this.originalFile = new System.Windows.Forms.TextBox();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.selectTarget = new System.Windows.Forms.Button();
            this.targetFile = new System.Windows.Forms.TextBox();
            this.extractButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.originalGroupBox.SuspendLayout();
            this.targetGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.originalGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.targetGroupBox);
            this.splitContainer1.Size = new System.Drawing.Size(800, 88);
            this.splitContainer1.SplitterDistance = 311;
            this.splitContainer1.TabIndex = 0;
            // 
            // originalGroupBox
            // 
            this.originalGroupBox.Controls.Add(this.selectOriginal);
            this.originalGroupBox.Controls.Add(this.originalFile);
            this.originalGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originalGroupBox.Location = new System.Drawing.Point(0, 0);
            this.originalGroupBox.Name = "originalGroupBox";
            this.originalGroupBox.Size = new System.Drawing.Size(311, 88);
            this.originalGroupBox.TabIndex = 0;
            this.originalGroupBox.TabStop = false;
            this.originalGroupBox.Text = "원본";
            // 
            // selectOriginal
            // 
            this.selectOriginal.Location = new System.Drawing.Point(12, 51);
            this.selectOriginal.Name = "selectOriginal";
            this.selectOriginal.Size = new System.Drawing.Size(75, 23);
            this.selectOriginal.TabIndex = 1;
            this.selectOriginal.Text = "폴더 열기";
            this.selectOriginal.UseVisualStyleBackColor = true;
            this.selectOriginal.Click += new System.EventHandler(this.selectOriginal_Click);
            // 
            // originalFile
            // 
            this.originalFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originalFile.Location = new System.Drawing.Point(12, 22);
            this.originalFile.Name = "originalFile";
            this.originalFile.Size = new System.Drawing.Size(293, 23);
            this.originalFile.TabIndex = 0;
            // 
            // targetGroupBox
            // 
            this.targetGroupBox.Controls.Add(this.selectTarget);
            this.targetGroupBox.Controls.Add(this.targetFile);
            this.targetGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetGroupBox.Location = new System.Drawing.Point(0, 0);
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.Size = new System.Drawing.Size(485, 88);
            this.targetGroupBox.TabIndex = 0;
            this.targetGroupBox.TabStop = false;
            this.targetGroupBox.Text = "대상";
            // 
            // selectTarget
            // 
            this.selectTarget.Location = new System.Drawing.Point(6, 51);
            this.selectTarget.Name = "selectTarget";
            this.selectTarget.Size = new System.Drawing.Size(75, 23);
            this.selectTarget.TabIndex = 1;
            this.selectTarget.Text = "폴더 열기";
            this.selectTarget.UseVisualStyleBackColor = true;
            this.selectTarget.Click += new System.EventHandler(this.selectTarget_Click);
            // 
            // targetFile
            // 
            this.targetFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFile.Location = new System.Drawing.Point(6, 22);
            this.targetFile.Name = "targetFile";
            this.targetFile.Size = new System.Drawing.Size(467, 23);
            this.targetFile.TabIndex = 0;
            // 
            // extractButton
            // 
            this.extractButton.Location = new System.Drawing.Point(713, 415);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 23);
            this.extractButton.TabIndex = 1;
            this.extractButton.Text = "추출";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 94);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(776, 315);
            this.treeView1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.originalGroupBox.ResumeLayout(false);
            this.originalGroupBox.PerformLayout();
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private SplitContainer splitContainer1;
        private GroupBox originalGroupBox;
        private Button selectOriginal;
        private TextBox originalFile;
        private GroupBox targetGroupBox;
        private Button selectTarget;
        private TextBox targetFile;
        private Button extractButton;
        private TreeView treeView1;
    }
}