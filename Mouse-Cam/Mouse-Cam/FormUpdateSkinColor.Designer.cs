namespace Hand_Virtual_Mouse
{
    partial class FormUpdateSkinColor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.btnSaveSamples = new System.Windows.Forms.Button();
            this.btnGetSample = new System.Windows.Forms.Button();
            this.btnCapVid = new System.Windows.Forms.Button();
            this.btnStartVid = new System.Windows.Forms.Button();
            this.pictureBoxCrop = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSampleCnter = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbNonSkin = new System.Windows.Forms.RadioButton();
            this.rbSkin = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrop)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxVideo);
            this.groupBox1.Location = new System.Drawing.Point(20, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1142, 562);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video";
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.BackColor = System.Drawing.Color.Black;
            this.pictureBoxVideo.Location = new System.Drawing.Point(26, 26);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(1074, 490);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxVideo.TabIndex = 1;
            this.pictureBoxVideo.TabStop = false;
            this.pictureBoxVideo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVideo_MouseDown);
            this.pictureBoxVideo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVideo_MouseUp);
            // 
            // btnSaveSamples
            // 
            this.btnSaveSamples.Location = new System.Drawing.Point(1168, 385);
            this.btnSaveSamples.Name = "btnSaveSamples";
            this.btnSaveSamples.Size = new System.Drawing.Size(125, 38);
            this.btnSaveSamples.TabIndex = 4;
            this.btnSaveSamples.Text = "Save Samples";
            this.btnSaveSamples.UseVisualStyleBackColor = true;
            this.btnSaveSamples.Click += new System.EventHandler(this.btnSaveSamples_Click);
            // 
            // btnGetSample
            // 
            this.btnGetSample.Location = new System.Drawing.Point(1168, 341);
            this.btnGetSample.Name = "btnGetSample";
            this.btnGetSample.Size = new System.Drawing.Size(125, 38);
            this.btnGetSample.TabIndex = 3;
            this.btnGetSample.Text = "Get Sample";
            this.btnGetSample.UseVisualStyleBackColor = true;
            this.btnGetSample.Click += new System.EventHandler(this.btnGetSample_Click);
            // 
            // btnCapVid
            // 
            this.btnCapVid.Enabled = false;
            this.btnCapVid.Location = new System.Drawing.Point(1168, 297);
            this.btnCapVid.Name = "btnCapVid";
            this.btnCapVid.Size = new System.Drawing.Size(125, 38);
            this.btnCapVid.TabIndex = 2;
            this.btnCapVid.Text = "Capture";
            this.btnCapVid.UseVisualStyleBackColor = true;
            this.btnCapVid.Click += new System.EventHandler(this.btnCapVid_Click);
            // 
            // btnStartVid
            // 
            this.btnStartVid.Location = new System.Drawing.Point(1168, 253);
            this.btnStartVid.Name = "btnStartVid";
            this.btnStartVid.Size = new System.Drawing.Size(125, 38);
            this.btnStartVid.TabIndex = 0;
            this.btnStartVid.Text = "Start";
            this.btnStartVid.UseVisualStyleBackColor = true;
            this.btnStartVid.Click += new System.EventHandler(this.btnStartVid_Click);
            // 
            // pictureBoxCrop
            // 
            this.pictureBoxCrop.BackColor = System.Drawing.Color.Black;
            this.pictureBoxCrop.Location = new System.Drawing.Point(1175, 18);
            this.pictureBoxCrop.Name = "pictureBoxCrop";
            this.pictureBoxCrop.Size = new System.Drawing.Size(114, 82);
            this.pictureBoxCrop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCrop.TabIndex = 3;
            this.pictureBoxCrop.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1175, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sample Count";
            // 
            // lblSampleCnter
            // 
            this.lblSampleCnter.AutoSize = true;
            this.lblSampleCnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleCnter.Location = new System.Drawing.Point(1275, 111);
            this.lblSampleCnter.Name = "lblSampleCnter";
            this.lblSampleCnter.Size = new System.Drawing.Size(29, 31);
            this.lblSampleCnter.TabIndex = 5;
            this.lblSampleCnter.Text = "0";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbNonSkin);
            this.groupBox4.Controls.Add(this.rbSkin);
            this.groupBox4.Location = new System.Drawing.Point(1168, 145);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(136, 85);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Training Mode";
            // 
            // rbNonSkin
            // 
            this.rbNonSkin.AutoSize = true;
            this.rbNonSkin.Location = new System.Drawing.Point(23, 47);
            this.rbNonSkin.Name = "rbNonSkin";
            this.rbNonSkin.Size = new System.Drawing.Size(81, 20);
            this.rbNonSkin.TabIndex = 1;
            this.rbNonSkin.TabStop = true;
            this.rbNonSkin.Text = "Non-Skin";
            this.rbNonSkin.UseVisualStyleBackColor = true;
            this.rbNonSkin.CheckedChanged += new System.EventHandler(this.rbNonSkin_CheckedChanged);
            // 
            // rbSkin
            // 
            this.rbSkin.AutoSize = true;
            this.rbSkin.Checked = true;
            this.rbSkin.Location = new System.Drawing.Point(23, 21);
            this.rbSkin.Name = "rbSkin";
            this.rbSkin.Size = new System.Drawing.Size(52, 20);
            this.rbSkin.TabIndex = 0;
            this.rbSkin.TabStop = true;
            this.rbSkin.Text = "Skin";
            this.rbSkin.UseVisualStyleBackColor = true;
            this.rbSkin.CheckedChanged += new System.EventHandler(this.rbSkin_CheckedChanged);
            // 
            // FormUpdateSkinColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1316, 606);
            this.Controls.Add(this.btnSaveSamples);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnGetSample);
            this.Controls.Add(this.lblSampleCnter);
            this.Controls.Add(this.btnCapVid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartVid);
            this.Controls.Add(this.pictureBoxCrop);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormUpdateSkinColor";
            this.Text = "Update Skin Color";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdateSkinColor_FormClosing);
            this.Load += new System.EventHandler(this.FormUpdateSkinColor_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrop)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.Button btnCapVid;
        private System.Windows.Forms.Button btnStartVid;
        private System.Windows.Forms.Button btnGetSample;
        private System.Windows.Forms.PictureBox pictureBoxCrop;
        private System.Windows.Forms.Button btnSaveSamples;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSampleCnter;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbNonSkin;
        private System.Windows.Forms.RadioButton rbSkin;
    }
}