namespace Hand_Virtual_Mouse
{
    partial class FormTemplateTrain
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
            this.btnStartCam = new System.Windows.Forms.Button();
            this.btnStopCam = new System.Windows.Forms.Button();
            this.menuStripFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pictureBoxCam = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonReject = new System.Windows.Forms.RadioButton();
            this.radioButtonArm = new System.Windows.Forms.RadioButton();
            this.radioButtonHand = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.labelErode = new System.Windows.Forms.Label();
            this.labelDilate = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelFreq = new System.Windows.Forms.Label();
            this.trackBarErode = new System.Windows.Forms.TrackBar();
            this.trackBarDilate = new System.Windows.Forms.TrackBar();
            this.trackBarFreq = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFPS = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbHSD = new System.Windows.Forms.RadioButton();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.checkBoxFlip = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonTrainClassifier = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonScrollUp = new System.Windows.Forms.RadioButton();
            this.radioButtonScrollDown = new System.Windows.Forms.RadioButton();
            this.radioButtonHalt = new System.Windows.Forms.RadioButton();
            this.radioButtonRight = new System.Windows.Forms.RadioButton();
            this.radioButtonLeft = new System.Windows.Forms.RadioButton();
            this.radioButtonOpen = new System.Windows.Forms.RadioButton();
            this.checkBoxTemplateRec = new System.Windows.Forms.CheckBox();
            this.checkBoxRecordFeatures = new System.Windows.Forms.CheckBox();
            this.checkBoxRecordFeaturesHand = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarErode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDilate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFreq)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartCam
            // 
            this.btnStartCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCam.Location = new System.Drawing.Point(26, 633);
            this.btnStartCam.Name = "btnStartCam";
            this.btnStartCam.Size = new System.Drawing.Size(136, 35);
            this.btnStartCam.TabIndex = 49;
            this.btnStartCam.Text = "Start Camera";
            this.btnStartCam.UseVisualStyleBackColor = true;
            this.btnStartCam.Click += new System.EventHandler(this.btnStartCam_Click);
            // 
            // btnStopCam
            // 
            this.btnStopCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopCam.Location = new System.Drawing.Point(168, 633);
            this.btnStopCam.Name = "btnStopCam";
            this.btnStopCam.Size = new System.Drawing.Size(136, 35);
            this.btnStopCam.TabIndex = 50;
            this.btnStopCam.Text = "Stop Camera";
            this.btnStopCam.UseVisualStyleBackColor = true;
            this.btnStopCam.Click += new System.EventHandler(this.btnStopCam_Click);
            // 
            // menuStripFile
            // 
            this.menuStripFile.Name = "menuStripFile";
            this.menuStripFile.Size = new System.Drawing.Size(61, 4);
            this.menuStripFile.Text = "File";
            // 
            // pictureBoxCam
            // 
            this.pictureBoxCam.BackColor = System.Drawing.Color.Black;
            this.pictureBoxCam.Location = new System.Drawing.Point(41, 19);
            this.pictureBoxCam.Name = "pictureBoxCam";
            this.pictureBoxCam.Size = new System.Drawing.Size(1074, 490);
            this.pictureBoxCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCam.TabIndex = 0;
            this.pictureBoxCam.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.radioButtonReject);
            this.groupBox5.Controls.Add(this.radioButtonArm);
            this.groupBox5.Controls.Add(this.radioButtonHand);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.labelErode);
            this.groupBox5.Controls.Add(this.labelDilate);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.labelFreq);
            this.groupBox5.Controls.Add(this.trackBarErode);
            this.groupBox5.Controls.Add(this.trackBarDilate);
            this.groupBox5.Controls.Add(this.trackBarFreq);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(258, 509);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(865, 85);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Parameters";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(657, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 39;
            this.label3.Text = "Classifier";
            // 
            // radioButtonReject
            // 
            this.radioButtonReject.AutoSize = true;
            this.radioButtonReject.Location = new System.Drawing.Point(786, 48);
            this.radioButtonReject.Name = "radioButtonReject";
            this.radioButtonReject.Size = new System.Drawing.Size(66, 21);
            this.radioButtonReject.TabIndex = 38;
            this.radioButtonReject.Text = "Reject";
            this.radioButtonReject.UseVisualStyleBackColor = true;
            this.radioButtonReject.CheckedChanged += new System.EventHandler(this.radioButtonReject_CheckedChanged);
            // 
            // radioButtonArm
            // 
            this.radioButtonArm.AutoSize = true;
            this.radioButtonArm.Location = new System.Drawing.Point(729, 48);
            this.radioButtonArm.Name = "radioButtonArm";
            this.radioButtonArm.Size = new System.Drawing.Size(51, 21);
            this.radioButtonArm.TabIndex = 6;
            this.radioButtonArm.Text = "Arm";
            this.radioButtonArm.UseVisualStyleBackColor = true;
            this.radioButtonArm.CheckedChanged += new System.EventHandler(this.radioButtonArm_CheckedChanged);
            // 
            // radioButtonHand
            // 
            this.radioButtonHand.AutoSize = true;
            this.radioButtonHand.Checked = true;
            this.radioButtonHand.Location = new System.Drawing.Point(659, 48);
            this.radioButtonHand.Name = "radioButtonHand";
            this.radioButtonHand.Size = new System.Drawing.Size(60, 21);
            this.radioButtonHand.TabIndex = 6;
            this.radioButtonHand.TabStop = true;
            this.radioButtonHand.Text = "Hand";
            this.radioButtonHand.UseVisualStyleBackColor = true;
            this.radioButtonHand.CheckedChanged += new System.EventHandler(this.radioButtonHand_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(426, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Dilation";
            // 
            // labelErode
            // 
            this.labelErode.AutoSize = true;
            this.labelErode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelErode.Location = new System.Drawing.Point(390, 21);
            this.labelErode.Name = "labelErode";
            this.labelErode.Size = new System.Drawing.Size(16, 17);
            this.labelErode.TabIndex = 22;
            this.labelErode.Text = "0";
            // 
            // labelDilate
            // 
            this.labelDilate.AutoSize = true;
            this.labelDilate.Location = new System.Drawing.Point(568, 21);
            this.labelDilate.Name = "labelDilate";
            this.labelDilate.Size = new System.Drawing.Size(16, 17);
            this.labelDilate.TabIndex = 7;
            this.labelDilate.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7.Location = new System.Drawing.Point(241, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "Erosion";
            // 
            // labelFreq
            // 
            this.labelFreq.AutoSize = true;
            this.labelFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelFreq.Location = new System.Drawing.Point(208, 22);
            this.labelFreq.Name = "labelFreq";
            this.labelFreq.Size = new System.Drawing.Size(16, 17);
            this.labelFreq.TabIndex = 9;
            this.labelFreq.Text = "0";
            // 
            // trackBarErode
            // 
            this.trackBarErode.LargeChange = 1;
            this.trackBarErode.Location = new System.Drawing.Point(230, 38);
            this.trackBarErode.Name = "trackBarErode";
            this.trackBarErode.Size = new System.Drawing.Size(176, 45);
            this.trackBarErode.TabIndex = 20;
            this.trackBarErode.Scroll += new System.EventHandler(this.trackBarErode_Scroll);
            // 
            // trackBarDilate
            // 
            this.trackBarDilate.LargeChange = 1;
            this.trackBarDilate.Location = new System.Drawing.Point(419, 36);
            this.trackBarDilate.Name = "trackBarDilate";
            this.trackBarDilate.Size = new System.Drawing.Size(165, 45);
            this.trackBarDilate.TabIndex = 6;
            this.trackBarDilate.Value = 1;
            this.trackBarDilate.Scroll += new System.EventHandler(this.trackBarDilate_Scroll);
            // 
            // trackBarFreq
            // 
            this.trackBarFreq.LargeChange = 1;
            this.trackBarFreq.Location = new System.Drawing.Point(10, 37);
            this.trackBarFreq.Maximum = 500;
            this.trackBarFreq.Minimum = 1;
            this.trackBarFreq.Name = "trackBarFreq";
            this.trackBarFreq.Size = new System.Drawing.Size(214, 45);
            this.trackBarFreq.TabIndex = 9;
            this.trackBarFreq.Value = 50;
            this.trackBarFreq.Scroll += new System.EventHandler(this.trackBarFreq_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(18, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "HSD Frequency";
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(1199, 647);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(16, 17);
            this.labelFPS.TabIndex = 37;
            this.labelFPS.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1155, 647);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 17);
            this.label12.TabIndex = 36;
            this.label12.Text = "FPS : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbHSD);
            this.groupBox2.Controls.Add(this.rbNormal);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox2.Location = new System.Drawing.Point(10, 511);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(177, 47);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter Display";
            // 
            // rbHSD
            // 
            this.rbHSD.AutoSize = true;
            this.rbHSD.Location = new System.Drawing.Point(86, 22);
            this.rbHSD.Name = "rbHSD";
            this.rbHSD.Size = new System.Drawing.Size(55, 21);
            this.rbHSD.TabIndex = 1;
            this.rbHSD.Text = "HSD";
            this.rbHSD.UseVisualStyleBackColor = true;
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Checked = true;
            this.rbNormal.Location = new System.Drawing.Point(8, 22);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(71, 21);
            this.rbNormal.TabIndex = 0;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "Normal";
            this.rbNormal.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlip
            // 
            this.checkBoxFlip.AutoSize = true;
            this.checkBoxFlip.Checked = true;
            this.checkBoxFlip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFlip.Location = new System.Drawing.Point(15, 562);
            this.checkBoxFlip.Name = "checkBoxFlip";
            this.checkBoxFlip.Size = new System.Drawing.Size(97, 21);
            this.checkBoxFlip.TabIndex = 15;
            this.checkBoxFlip.Text = "Enable Flip";
            this.checkBoxFlip.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxFlip);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.pictureBoxCam);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(25, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1152, 596);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Output";
            // 
            // buttonTrainClassifier
            // 
            this.buttonTrainClassifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTrainClassifier.Location = new System.Drawing.Point(310, 633);
            this.buttonTrainClassifier.Name = "buttonTrainClassifier";
            this.buttonTrainClassifier.Size = new System.Drawing.Size(136, 35);
            this.buttonTrainClassifier.TabIndex = 66;
            this.buttonTrainClassifier.Text = "Train Classifier";
            this.buttonTrainClassifier.UseVisualStyleBackColor = true;
            this.buttonTrainClassifier.Click += new System.EventHandler(this.buttonTrainClassifier_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonScrollUp);
            this.groupBox3.Controls.Add(this.radioButtonScrollDown);
            this.groupBox3.Controls.Add(this.radioButtonHalt);
            this.groupBox3.Controls.Add(this.radioButtonRight);
            this.groupBox3.Controls.Add(this.radioButtonLeft);
            this.groupBox3.Controls.Add(this.radioButtonOpen);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox3.Location = new System.Drawing.Point(647, 627);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(493, 47);
            this.groupBox3.TabIndex = 50;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gesture Training";
            // 
            // radioButtonScrollUp
            // 
            this.radioButtonScrollUp.AutoSize = true;
            this.radioButtonScrollUp.Location = new System.Drawing.Point(366, 20);
            this.radioButtonScrollUp.Name = "radioButtonScrollUp";
            this.radioButtonScrollUp.Size = new System.Drawing.Size(83, 21);
            this.radioButtonScrollUp.TabIndex = 5;
            this.radioButtonScrollUp.Text = "Scroll Up";
            this.radioButtonScrollUp.UseVisualStyleBackColor = true;
            this.radioButtonScrollUp.CheckedChanged += new System.EventHandler(this.radioButtonScrollUp_CheckedChanged);
            // 
            // radioButtonScrollDown
            // 
            this.radioButtonScrollDown.AutoSize = true;
            this.radioButtonScrollDown.Location = new System.Drawing.Point(260, 20);
            this.radioButtonScrollDown.Name = "radioButtonScrollDown";
            this.radioButtonScrollDown.Size = new System.Drawing.Size(100, 21);
            this.radioButtonScrollDown.TabIndex = 4;
            this.radioButtonScrollDown.Text = "Scroll Down";
            this.radioButtonScrollDown.UseVisualStyleBackColor = true;
            this.radioButtonScrollDown.CheckedChanged += new System.EventHandler(this.radioButtonScrollDown_CheckedChanged);
            // 
            // radioButtonHalt
            // 
            this.radioButtonHalt.AutoSize = true;
            this.radioButtonHalt.Location = new System.Drawing.Point(199, 21);
            this.radioButtonHalt.Name = "radioButtonHalt";
            this.radioButtonHalt.Size = new System.Drawing.Size(51, 21);
            this.radioButtonHalt.TabIndex = 3;
            this.radioButtonHalt.Text = "Halt";
            this.radioButtonHalt.UseVisualStyleBackColor = true;
            this.radioButtonHalt.CheckedChanged += new System.EventHandler(this.radioButtonHalt_CheckedChanged);
            // 
            // radioButtonRight
            // 
            this.radioButtonRight.AutoSize = true;
            this.radioButtonRight.Location = new System.Drawing.Point(131, 21);
            this.radioButtonRight.Name = "radioButtonRight";
            this.radioButtonRight.Size = new System.Drawing.Size(59, 21);
            this.radioButtonRight.TabIndex = 2;
            this.radioButtonRight.Text = "Right";
            this.radioButtonRight.UseVisualStyleBackColor = true;
            this.radioButtonRight.CheckedChanged += new System.EventHandler(this.radioButtonRight_CheckedChanged);
            // 
            // radioButtonLeft
            // 
            this.radioButtonLeft.AutoSize = true;
            this.radioButtonLeft.Location = new System.Drawing.Point(75, 22);
            this.radioButtonLeft.Name = "radioButtonLeft";
            this.radioButtonLeft.Size = new System.Drawing.Size(50, 21);
            this.radioButtonLeft.TabIndex = 1;
            this.radioButtonLeft.Text = "Left";
            this.radioButtonLeft.UseVisualStyleBackColor = true;
            this.radioButtonLeft.CheckedChanged += new System.EventHandler(this.radioButtonLeft_CheckedChanged);
            // 
            // radioButtonOpen
            // 
            this.radioButtonOpen.AutoSize = true;
            this.radioButtonOpen.Checked = true;
            this.radioButtonOpen.Location = new System.Drawing.Point(8, 22);
            this.radioButtonOpen.Name = "radioButtonOpen";
            this.radioButtonOpen.Size = new System.Drawing.Size(61, 21);
            this.radioButtonOpen.TabIndex = 0;
            this.radioButtonOpen.TabStop = true;
            this.radioButtonOpen.Text = "Open";
            this.radioButtonOpen.UseVisualStyleBackColor = true;
            this.radioButtonOpen.CheckedChanged += new System.EventHandler(this.radioButtonOpen_CheckedChanged);
            // 
            // checkBoxTemplateRec
            // 
            this.checkBoxTemplateRec.AutoSize = true;
            this.checkBoxTemplateRec.Location = new System.Drawing.Point(1183, 37);
            this.checkBoxTemplateRec.Name = "checkBoxTemplateRec";
            this.checkBoxTemplateRec.Size = new System.Drawing.Size(136, 21);
            this.checkBoxTemplateRec.TabIndex = 67;
            this.checkBoxTemplateRec.Text = "Record Template";
            this.checkBoxTemplateRec.UseVisualStyleBackColor = true;
            this.checkBoxTemplateRec.CheckedChanged += new System.EventHandler(this.checkBoxTemplateRec_CheckedChanged);
            // 
            // checkBoxRecordFeatures
            // 
            this.checkBoxRecordFeatures.AutoSize = true;
            this.checkBoxRecordFeatures.Location = new System.Drawing.Point(1183, 64);
            this.checkBoxRecordFeatures.Name = "checkBoxRecordFeatures";
            this.checkBoxRecordFeatures.Size = new System.Drawing.Size(126, 21);
            this.checkBoxRecordFeatures.TabIndex = 68;
            this.checkBoxRecordFeatures.Text = "Features (DNT)";
            this.checkBoxRecordFeatures.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecordFeaturesHand
            // 
            this.checkBoxRecordFeaturesHand.AutoSize = true;
            this.checkBoxRecordFeaturesHand.Location = new System.Drawing.Point(1183, 91);
            this.checkBoxRecordFeaturesHand.Name = "checkBoxRecordFeaturesHand";
            this.checkBoxRecordFeaturesHand.Size = new System.Drawing.Size(164, 21);
            this.checkBoxRecordFeaturesHand.TabIndex = 70;
            this.checkBoxRecordFeaturesHand.Text = "Features Hand (DNT)";
            this.checkBoxRecordFeaturesHand.UseVisualStyleBackColor = true;
            // 
            // FormTemplateTrain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 678);
            this.Controls.Add(this.checkBoxRecordFeaturesHand);
            this.Controls.Add(this.checkBoxRecordFeatures);
            this.Controls.Add(this.checkBoxTemplateRec);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.buttonTrainClassifier);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStopCam);
            this.Controls.Add(this.btnStartCam);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormTemplateTrain";
            this.Text = "Template Matching Trainer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTemplateTrain_FormClosing);
            this.Load += new System.EventHandler(this.FormTemplateTrain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarErode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDilate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFreq)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartCam;
        private System.Windows.Forms.Button btnStopCam;
        private System.Windows.Forms.ContextMenuStrip menuStripFile;
        private System.Windows.Forms.PictureBox pictureBoxCam;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelErode;
        private System.Windows.Forms.Label labelDilate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelFreq;
        private System.Windows.Forms.TrackBar trackBarErode;
        private System.Windows.Forms.TrackBar trackBarDilate;
        private System.Windows.Forms.TrackBar trackBarFreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbHSD;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.CheckBox checkBoxFlip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonTrainClassifier;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonLeft;
        private System.Windows.Forms.RadioButton radioButtonOpen;
        private System.Windows.Forms.RadioButton radioButtonRight;
        private System.Windows.Forms.CheckBox checkBoxTemplateRec;
        private System.Windows.Forms.RadioButton radioButtonHalt;
        private System.Windows.Forms.RadioButton radioButtonScrollUp;
        private System.Windows.Forms.RadioButton radioButtonScrollDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonReject;
        private System.Windows.Forms.RadioButton radioButtonArm;
        private System.Windows.Forms.RadioButton radioButtonHand;
        private System.Windows.Forms.CheckBox checkBoxRecordFeatures;
        private System.Windows.Forms.CheckBox checkBoxRecordFeaturesHand;
    }
}