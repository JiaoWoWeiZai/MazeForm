namespace MazeForm
{
    partial class FrmMaze
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCreateMaze = new System.Windows.Forms.Button();
            this.tBSize = new System.Windows.Forms.TextBox();
            this.pBMain = new System.Windows.Forms.PictureBox();
            this.btnShowFind = new System.Windows.Forms.Button();
            this.cmbCreateType = new System.Windows.Forms.ComboBox();
            this.cmbFindType = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnShowCreate = new System.Windows.Forms.Button();
            this.lbFindTime = new System.Windows.Forms.Label();
            this.lbCreateTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pBMain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateMaze
            // 
            this.btnCreateMaze.Location = new System.Drawing.Point(436, 102);
            this.btnCreateMaze.Name = "btnCreateMaze";
            this.btnCreateMaze.Size = new System.Drawing.Size(62, 23);
            this.btnCreateMaze.TabIndex = 1;
            this.btnCreateMaze.Text = "生成迷宫";
            this.btnCreateMaze.UseVisualStyleBackColor = true;
            this.btnCreateMaze.Click += new System.EventHandler(this.btnCreateMaze_Click);
            // 
            // tBSize
            // 
            this.tBSize.Location = new System.Drawing.Point(436, 21);
            this.tBSize.Name = "tBSize";
            this.tBSize.Size = new System.Drawing.Size(39, 21);
            this.tBSize.TabIndex = 2;
            // 
            // pBMain
            // 
            this.pBMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pBMain.Location = new System.Drawing.Point(1, 1);
            this.pBMain.Name = "pBMain";
            this.pBMain.Size = new System.Drawing.Size(412, 440);
            this.pBMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBMain.TabIndex = 0;
            this.pBMain.TabStop = false;
            // 
            // btnShowFind
            // 
            this.btnShowFind.Location = new System.Drawing.Point(436, 300);
            this.btnShowFind.Name = "btnShowFind";
            this.btnShowFind.Size = new System.Drawing.Size(61, 23);
            this.btnShowFind.TabIndex = 4;
            this.btnShowFind.Text = "演示寻路";
            this.btnShowFind.UseVisualStyleBackColor = true;
            this.btnShowFind.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // cmbCreateType
            // 
            this.cmbCreateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCreateType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbCreateType.FormattingEnabled = true;
            this.cmbCreateType.Items.AddRange(new object[] {
            "普利姆",
            "递归分割",
            "递归回溯",
            "深度遍历图"});
            this.cmbCreateType.Location = new System.Drawing.Point(436, 60);
            this.cmbCreateType.Name = "cmbCreateType";
            this.cmbCreateType.Size = new System.Drawing.Size(70, 20);
            this.cmbCreateType.TabIndex = 6;
            // 
            // cmbFindType
            // 
            this.cmbFindType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFindType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbFindType.FormattingEnabled = true;
            this.cmbFindType.Items.AddRange(new object[] {
            "DFS",
            "BFS",
            "AStar"});
            this.cmbFindType.Location = new System.Drawing.Point(436, 232);
            this.cmbFindType.Name = "cmbFindType";
            this.cmbFindType.Size = new System.Drawing.Size(70, 20);
            this.cmbFindType.TabIndex = 6;
            this.cmbFindType.SelectedIndexChanged += new System.EventHandler(this.cmbFindType_SelectedIndexChanged);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(436, 264);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(62, 23);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "寻找路径";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnShowCreate
            // 
            this.btnShowCreate.Location = new System.Drawing.Point(436, 140);
            this.btnShowCreate.Name = "btnShowCreate";
            this.btnShowCreate.Size = new System.Drawing.Size(61, 23);
            this.btnShowCreate.TabIndex = 4;
            this.btnShowCreate.Text = "演示生成";
            this.btnShowCreate.UseVisualStyleBackColor = true;
            this.btnShowCreate.Click += new System.EventHandler(this.btnShowCreate_Click);
            // 
            // lbFindTime
            // 
            this.lbFindTime.AutoSize = true;
            this.lbFindTime.Location = new System.Drawing.Point(434, 338);
            this.lbFindTime.Name = "lbFindTime";
            this.lbFindTime.Size = new System.Drawing.Size(0, 12);
            this.lbFindTime.TabIndex = 8;
            // 
            // lbCreateTime
            // 
            this.lbCreateTime.AutoSize = true;
            this.lbCreateTime.Location = new System.Drawing.Point(434, 175);
            this.lbCreateTime.Name = "lbCreateTime";
            this.lbCreateTime.Size = new System.Drawing.Size(0, 12);
            this.lbCreateTime.TabIndex = 8;
            // 
            // FrmMaze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 442);
            this.Controls.Add(this.lbCreateTime);
            this.Controls.Add(this.lbFindTime);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.cmbFindType);
            this.Controls.Add(this.cmbCreateType);
            this.Controls.Add(this.btnShowCreate);
            this.Controls.Add(this.btnShowFind);
            this.Controls.Add(this.tBSize);
            this.Controls.Add(this.btnCreateMaze);
            this.Controls.Add(this.pBMain);
            this.DoubleBuffered = true;
            this.Name = "FrmMaze";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "迷宫游戏";
            this.Load += new System.EventHandler(this.frmMaze_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBMain;
        private System.Windows.Forms.Button btnCreateMaze;
        private System.Windows.Forms.TextBox tBSize;
        private System.Windows.Forms.Button btnShowFind;
        private System.Windows.Forms.ComboBox cmbCreateType;
        private System.Windows.Forms.ComboBox cmbFindType;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnShowCreate;
        private System.Windows.Forms.Label lbFindTime;
        private System.Windows.Forms.Label lbCreateTime;
    }
}

