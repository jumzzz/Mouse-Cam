namespace Hand_Virtual_Mouse
{
    partial class FormHVMouse
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
            this.components = new System.ComponentModel.Container();
            this.checkBoxRecTmp = new System.Windows.Forms.CheckBox();
            this.checkBoxFlip = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelClickRemark = new System.Windows.Forms.Label();
            this.btnTrainHSD = new System.Windows.Forms.Button();
            this.btnStopCam = new System.Windows.Forms.Button();
            this.btnStartCam = new System.Windows.Forms.Button();
            this.menuStripFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateColorSkinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainClickGesturesToolStripGesture = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.checkBoxMouseMove = new System.Windows.Forms.CheckBox();
            this.checkBoxLeftClick = new System.Windows.Forms.CheckBox();
            this.checkBoxRightClick = new System.Windows.Forms.CheckBox();
            this.checkBoxLogging = new System.Windows.Forms.CheckBox();
            this.checkBoxScroll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFPS = new System.Windows.Forms.Label();
            this.checkBoxShowDefects = new System.Windows.Forms.CheckBox();
            this.tabPageDefect = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pictureBoxDefect = new System.Windows.Forms.PictureBox();
            this.tabPageHSD = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBoxHSD = new System.Windows.Forms.PictureBox();
            this.tabPageDefault = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxCam = new System.Windows.Forms.PictureBox();
            this.tab = new System.Windows.Forms.TabControl();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPageDefect.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDefect)).BeginInit();
            this.tabPageHSD.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHSD)).BeginInit();
            this.tabPageDefault.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).BeginInit();
            this.tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxRecTmp
            // 
            this.checkBoxRecTmp.AutoSize = true;
            this.checkBoxRecTmp.Location = new System.Drawing.Point(19, 328);
            this.checkBoxRecTmp.Name = "checkBoxRecTmp";
            this.checkBoxRecTmp.Size = new System.Drawing.Size(108, 17);
            this.checkBoxRecTmp.TabIndex = 16;
            this.checkBoxRecTmp.Text = "Record Template";
            this.checkBoxRecTmp.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlip
            // 
            this.checkBoxFlip.AutoSize = true;
            this.checkBoxFlip.Checked = true;
            this.checkBoxFlip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFlip.Location = new System.Drawing.Point(19, 305);
            this.checkBoxFlip.Name = "checkBoxFlip";
            this.checkBoxFlip.Size = new System.Drawing.Size(78, 17);
            this.checkBoxFlip.TabIndex = 15;
            this.checkBoxFlip.Text = "Enable Flip";
            this.checkBoxFlip.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelClickRemark);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox2.Location = new System.Drawing.Point(19, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(115, 78);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter Display";
            // 
            // labelClickRemark
            // 
            this.labelClickRemark.AutoSize = true;
            this.labelClickRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClickRemark.Location = new System.Drawing.Point(27, 33);
            this.labelClickRemark.Name = "labelClickRemark";
            this.labelClickRemark.Size = new System.Drawing.Size(51, 17);
            this.labelClickRemark.TabIndex = 49;
            this.labelClickRemark.Text = "OPEN";
            // 
            // btnTrainHSD
            // 
            this.btnTrainHSD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrainHSD.Location = new System.Drawing.Point(12, 134);
            this.btnTrainHSD.Name = "btnTrainHSD";
            this.btnTrainHSD.Size = new System.Drawing.Size(136, 35);
            this.btnTrainHSD.TabIndex = 11;
            this.btnTrainHSD.Text = "Train HSD";
            this.btnTrainHSD.UseVisualStyleBackColor = true;
            this.btnTrainHSD.Click += new System.EventHandler(this.btnTrainHSD_Click);
            // 
            // btnStopCam
            // 
            this.btnStopCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopCam.Location = new System.Drawing.Point(12, 93);
            this.btnStopCam.Name = "btnStopCam";
            this.btnStopCam.Size = new System.Drawing.Size(136, 35);
            this.btnStopCam.TabIndex = 7;
            this.btnStopCam.Text = "Stop Camera";
            this.btnStopCam.UseVisualStyleBackColor = true;
            this.btnStopCam.Click += new System.EventHandler(this.btnStopCam_Click);
            // 
            // btnStartCam
            // 
            this.btnStartCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCam.Location = new System.Drawing.Point(12, 52);
            this.btnStartCam.Name = "btnStartCam";
            this.btnStartCam.Size = new System.Drawing.Size(136, 35);
            this.btnStartCam.TabIndex = 6;
            this.btnStartCam.Text = "Start Camera";
            this.btnStartCam.UseVisualStyleBackColor = true;
            this.btnStartCam.Click += new System.EventHandler(this.btnStartCam_Click);
            // 
            // menuStripFile
            // 
            this.menuStripFile.Name = "menuStripFile";
            this.menuStripFile.Size = new System.Drawing.Size(61, 4);
            this.menuStripFile.Text = "File";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1294, 27);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateColorSkinToolStripMenuItem,
            this.trainClickGesturesToolStripGesture,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(102, 23);
            this.fileToolStripMenuItem.Text = "Data Training";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // updateColorSkinToolStripMenuItem
            // 
            this.updateColorSkinToolStripMenuItem.Name = "updateColorSkinToolStripMenuItem";
            this.updateColorSkinToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.updateColorSkinToolStripMenuItem.Text = "Update Color Skin";
            this.updateColorSkinToolStripMenuItem.Click += new System.EventHandler(this.updateColorSkinToolStripMenuItem_Click);
            // 
            // trainClickGesturesToolStripGesture
            // 
            this.trainClickGesturesToolStripGesture.Name = "trainClickGesturesToolStripGesture";
            this.trainClickGesturesToolStripGesture.Size = new System.Drawing.Size(197, 24);
            this.trainClickGesturesToolStripGesture.Text = "Train Click Gestures";
            this.trainClickGesturesToolStripGesture.Click += new System.EventHandler(this.trainClickGesturesToolStripGesture_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(197, 24);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblRemarks);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(16, 489);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(129, 62);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemarks.Location = new System.Drawing.Point(27, 20);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(78, 17);
            this.lblRemarks.TabIndex = 0;
            this.lblRemarks.Text = "Press Start";
            this.lblRemarks.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBoxMouseMove
            // 
            this.checkBoxMouseMove.AutoSize = true;
            this.checkBoxMouseMove.Location = new System.Drawing.Point(19, 350);
            this.checkBoxMouseMove.Name = "checkBoxMouseMove";
            this.checkBoxMouseMove.Size = new System.Drawing.Size(88, 17);
            this.checkBoxMouseMove.TabIndex = 16;
            this.checkBoxMouseMove.Text = "Mouse Move";
            this.checkBoxMouseMove.UseVisualStyleBackColor = true;
            this.checkBoxMouseMove.CheckedChanged += new System.EventHandler(this.checkBoxMouseMove_CheckedChanged);
            // 
            // checkBoxLeftClick
            // 
            this.checkBoxLeftClick.AutoSize = true;
            this.checkBoxLeftClick.Location = new System.Drawing.Point(19, 370);
            this.checkBoxLeftClick.Name = "checkBoxLeftClick";
            this.checkBoxLeftClick.Size = new System.Drawing.Size(106, 17);
            this.checkBoxLeftClick.TabIndex = 17;
            this.checkBoxLeftClick.Text = "Enable Left-Click";
            this.checkBoxLeftClick.UseVisualStyleBackColor = true;
            this.checkBoxLeftClick.CheckedChanged += new System.EventHandler(this.checkBoxLeftClick_CheckedChanged);
            // 
            // checkBoxRightClick
            // 
            this.checkBoxRightClick.AutoSize = true;
            this.checkBoxRightClick.Location = new System.Drawing.Point(19, 391);
            this.checkBoxRightClick.Name = "checkBoxRightClick";
            this.checkBoxRightClick.Size = new System.Drawing.Size(113, 17);
            this.checkBoxRightClick.TabIndex = 18;
            this.checkBoxRightClick.Text = "Enable Right-Click";
            this.checkBoxRightClick.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogging
            // 
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Location = new System.Drawing.Point(19, 412);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new System.Drawing.Size(100, 17);
            this.checkBoxLogging.TabIndex = 19;
            this.checkBoxLogging.Text = "Enable Logging";
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            // 
            // checkBoxScroll
            // 
            this.checkBoxScroll.AutoSize = true;
            this.checkBoxScroll.Location = new System.Drawing.Point(19, 433);
            this.checkBoxScroll.Name = "checkBoxScroll";
            this.checkBoxScroll.Size = new System.Drawing.Size(102, 17);
            this.checkBoxScroll.TabIndex = 22;
            this.checkBoxScroll.Text = "Enable Scrolling";
            this.checkBoxScroll.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 565);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "FPS :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFPS.Location = new System.Drawing.Point(104, 565);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(17, 17);
            this.labelFPS.TabIndex = 25;
            this.labelFPS.Text = "0";
            this.labelFPS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBoxShowDefects
            // 
            this.checkBoxShowDefects.AutoSize = true;
            this.checkBoxShowDefects.Location = new System.Drawing.Point(19, 456);
            this.checkBoxShowDefects.Name = "checkBoxShowDefects";
            this.checkBoxShowDefects.Size = new System.Drawing.Size(93, 17);
            this.checkBoxShowDefects.TabIndex = 26;
            this.checkBoxShowDefects.Text = "Show Defects";
            this.checkBoxShowDefects.UseVisualStyleBackColor = true;
            // 
            // tabPageDefect
            // 
            this.tabPageDefect.Controls.Add(this.groupBox5);
            this.tabPageDefect.Location = new System.Drawing.Point(4, 22);
            this.tabPageDefect.Name = "tabPageDefect";
            this.tabPageDefect.Size = new System.Drawing.Size(1120, 546);
            this.tabPageDefect.TabIndex = 2;
            this.tabPageDefect.Text = "Hand Defects";
            this.tabPageDefect.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.pictureBoxDefect);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(9, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1102, 531);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // pictureBoxDefect
            // 
            this.pictureBoxDefect.BackColor = System.Drawing.Color.Black;
            this.pictureBoxDefect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxDefect.Location = new System.Drawing.Point(13, 19);
            this.pictureBoxDefect.Name = "pictureBoxDefect";
            this.pictureBoxDefect.Size = new System.Drawing.Size(1074, 490);
            this.pictureBoxDefect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxDefect.TabIndex = 0;
            this.pictureBoxDefect.TabStop = false;
            // 
            // tabPageHSD
            // 
            this.tabPageHSD.Controls.Add(this.groupBox3);
            this.tabPageHSD.Location = new System.Drawing.Point(4, 22);
            this.tabPageHSD.Name = "tabPageHSD";
            this.tabPageHSD.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHSD.Size = new System.Drawing.Size(1120, 546);
            this.tabPageHSD.TabIndex = 1;
            this.tabPageHSD.Text = "Histogram Skin Detection";
            this.tabPageHSD.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBoxHSD);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(11, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1102, 529);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // pictureBoxHSD
            // 
            this.pictureBoxHSD.BackColor = System.Drawing.Color.Black;
            this.pictureBoxHSD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxHSD.Location = new System.Drawing.Point(13, 19);
            this.pictureBoxHSD.Name = "pictureBoxHSD";
            this.pictureBoxHSD.Size = new System.Drawing.Size(1074, 490);
            this.pictureBoxHSD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxHSD.TabIndex = 0;
            this.pictureBoxHSD.TabStop = false;
            // 
            // tabPageDefault
            // 
            this.tabPageDefault.Controls.Add(this.groupBox1);
            this.tabPageDefault.Location = new System.Drawing.Point(4, 22);
            this.tabPageDefault.Name = "tabPageDefault";
            this.tabPageDefault.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDefault.Size = new System.Drawing.Size(1120, 546);
            this.tabPageDefault.TabIndex = 0;
            this.tabPageDefault.Text = "Default Image";
            this.tabPageDefault.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxCam);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1095, 529);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pictureBoxCam
            // 
            this.pictureBoxCam.BackColor = System.Drawing.Color.Black;
            this.pictureBoxCam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCam.Location = new System.Drawing.Point(13, 19);
            this.pictureBoxCam.Name = "pictureBoxCam";
            this.pictureBoxCam.Size = new System.Drawing.Size(1074, 490);
            this.pictureBoxCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCam.TabIndex = 0;
            this.pictureBoxCam.TabStop = false;
            this.pictureBoxCam.Click += new System.EventHandler(this.pictureBoxCam_Click);
            this.pictureBoxCam.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCam_MouseDown);
            this.pictureBoxCam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCam_MouseUp);
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPageDefault);
            this.tab.Controls.Add(this.tabPageHSD);
            this.tab.Controls.Add(this.tabPageDefect);
            this.tab.Location = new System.Drawing.Point(165, 30);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(1128, 572);
            this.tab.TabIndex = 23;
            // 
            // FormHVMouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 612);
            this.Controls.Add(this.checkBoxShowDefects);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkBoxFlip);
            this.Controls.Add(this.checkBoxRecTmp);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.checkBoxScroll);
            this.Controls.Add(this.checkBoxLogging);
            this.Controls.Add(this.btnTrainHSD);
            this.Controls.Add(this.checkBoxRightClick);
            this.Controls.Add(this.checkBoxLeftClick);
            this.Controls.Add(this.checkBoxMouseMove);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnStopCam);
            this.Controls.Add(this.btnStartCam);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.Name = "FormHVMouse";
            this.Text = "Cam Mouse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHVMouse_FormClosing);
            this.Load += new System.EventHandler(this.FormHVMouse_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPageDefect.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDefect)).EndInit();
            this.tabPageHSD.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHSD)).EndInit();
            this.tabPageDefault.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).EndInit();
            this.tab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStopCam;
        private System.Windows.Forms.Button btnStartCam;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTrainHSD;
        private System.Windows.Forms.ContextMenuStrip menuStripFile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateColorSkinToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.CheckBox checkBoxMouseMove;
        private System.Windows.Forms.CheckBox checkBoxLeftClick;
        private System.Windows.Forms.CheckBox checkBoxRightClick;
        private System.Windows.Forms.CheckBox checkBoxLogging;
        private System.Windows.Forms.CheckBox checkBoxFlip;
        private System.Windows.Forms.CheckBox checkBoxRecTmp;
        private System.Windows.Forms.Label labelClickRemark;
        private System.Windows.Forms.ToolStripMenuItem trainClickGesturesToolStripGesture;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.CheckBox checkBoxScroll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.CheckBox checkBoxShowDefects;
        private System.Windows.Forms.TabPage tabPageDefect;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pictureBoxDefect;
        private System.Windows.Forms.TabPage tabPageHSD;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBoxHSD;
        private System.Windows.Forms.TabPage tabPageDefault;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxCam;
        private System.Windows.Forms.TabControl tab;
    }
}

