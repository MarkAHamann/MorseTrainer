namespace MorseTrainer
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

                // Manually entered disposals
                if (_player != null)
                {
                    _player.Dispose();
                    _player = null;
                }
                if (_runner != null)
                {
                    _runner.Dispose();
                    _runner = null;
                }
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sliderFrequency = new System.Windows.Forms.TrackBar();
            this.sliderWPM = new System.Windows.Forms.TrackBar();
            this.sliderDuration = new System.Windows.Forms.TrackBar();
            this.sliderFarnsworth = new System.Windows.Forms.TrackBar();
            this.txtAnalysis = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.sliderStartDelay = new System.Windows.Forms.TrackBar();
            this.sliderStopDelay = new System.Windows.Forms.TrackBar();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.txtWPM = new System.Windows.Forms.TextBox();
            this.txtFarnsworth = new System.Windows.Forms.TextBox();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnKoch = new System.Windows.Forms.RadioButton();
            this.btnCustom = new System.Windows.Forms.RadioButton();
            this.cmbKoch = new System.Windows.Forms.ComboBox();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.chkFavorNew = new System.Windows.Forms.CheckBox();
            this.sliderVolume = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStartDelay = new System.Windows.Forms.TextBox();
            this.txtStopDelay = new System.Windows.Forms.TextBox();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.mnuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuContextRestoreDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContextSetProsignKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContextAbout = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderWPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFarnsworth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStartDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStopDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderVolume)).BeginInit();
            this.mnuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Frequency";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "WPM";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Send Duration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Farnsworth WPM";
            // 
            // sliderFrequency
            // 
            this.sliderFrequency.LargeChange = 100;
            this.sliderFrequency.Location = new System.Drawing.Point(147, 9);
            this.sliderFrequency.Maximum = 1000;
            this.sliderFrequency.Minimum = 400;
            this.sliderFrequency.Name = "sliderFrequency";
            this.sliderFrequency.Size = new System.Drawing.Size(663, 69);
            this.sliderFrequency.SmallChange = 50;
            this.sliderFrequency.TabIndex = 4;
            this.sliderFrequency.TickFrequency = 50;
            this.sliderFrequency.Value = 700;
            this.sliderFrequency.Scroll += new System.EventHandler(this.sliderFrequency_Scroll);
            // 
            // sliderWPM
            // 
            this.sliderWPM.Location = new System.Drawing.Point(147, 53);
            this.sliderWPM.Maximum = 80;
            this.sliderWPM.Minimum = 8;
            this.sliderWPM.Name = "sliderWPM";
            this.sliderWPM.Size = new System.Drawing.Size(663, 69);
            this.sliderWPM.TabIndex = 5;
            this.sliderWPM.Value = 20;
            this.sliderWPM.Scroll += new System.EventHandler(this.sliderWPM_Scroll);
            // 
            // sliderDuration
            // 
            this.sliderDuration.Location = new System.Drawing.Point(147, 134);
            this.sliderDuration.Minimum = 1;
            this.sliderDuration.Name = "sliderDuration";
            this.sliderDuration.Size = new System.Drawing.Size(663, 69);
            this.sliderDuration.TabIndex = 7;
            this.sliderDuration.Value = 2;
            this.sliderDuration.Scroll += new System.EventHandler(this.sliderDuration_Scroll);
            // 
            // sliderFarnsworth
            // 
            this.sliderFarnsworth.Location = new System.Drawing.Point(147, 97);
            this.sliderFarnsworth.Maximum = 80;
            this.sliderFarnsworth.Minimum = 8;
            this.sliderFarnsworth.Name = "sliderFarnsworth";
            this.sliderFarnsworth.Size = new System.Drawing.Size(663, 69);
            this.sliderFarnsworth.TabIndex = 6;
            this.sliderFarnsworth.Value = 13;
            this.sliderFarnsworth.Scroll += new System.EventHandler(this.sliderFarnsworth_Scroll);
            // 
            // txtAnalysis
            // 
            this.txtAnalysis.Location = new System.Drawing.Point(20, 288);
            this.txtAnalysis.Name = "txtAnalysis";
            this.txtAnalysis.ReadOnly = true;
            this.txtAnalysis.ShortcutsEnabled = false;
            this.txtAnalysis.Size = new System.Drawing.Size(884, 177);
            this.txtAnalysis.TabIndex = 8;
            this.txtAnalysis.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Your Recording/Analysis";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Start Delay";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(357, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "Stop Delay";
            // 
            // sliderStartDelay
            // 
            this.sliderStartDelay.Location = new System.Drawing.Point(147, 185);
            this.sliderStartDelay.Maximum = 5;
            this.sliderStartDelay.Name = "sliderStartDelay";
            this.sliderStartDelay.Size = new System.Drawing.Size(146, 69);
            this.sliderStartDelay.TabIndex = 12;
            this.sliderStartDelay.Value = 2;
            this.sliderStartDelay.Scroll += new System.EventHandler(this.sliderStartDelay_Scroll);
            // 
            // sliderStopDelay
            // 
            this.sliderStopDelay.Location = new System.Drawing.Point(450, 185);
            this.sliderStopDelay.Maximum = 5;
            this.sliderStopDelay.Name = "sliderStopDelay";
            this.sliderStopDelay.Size = new System.Drawing.Size(128, 69);
            this.sliderStopDelay.TabIndex = 13;
            this.sliderStopDelay.Value = 2;
            this.sliderStopDelay.Scroll += new System.EventHandler(this.sliderStopDelay_Scroll);
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(816, 9);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(88, 26);
            this.txtFrequency.TabIndex = 14;
            this.txtFrequency.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrequency_KeyPress);
            // 
            // txtWPM
            // 
            this.txtWPM.Location = new System.Drawing.Point(816, 53);
            this.txtWPM.Name = "txtWPM";
            this.txtWPM.Size = new System.Drawing.Size(88, 26);
            this.txtWPM.TabIndex = 15;
            this.txtWPM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWPM_KeyPress);
            // 
            // txtFarnsworth
            // 
            this.txtFarnsworth.Location = new System.Drawing.Point(816, 97);
            this.txtFarnsworth.Name = "txtFarnsworth";
            this.txtFarnsworth.Size = new System.Drawing.Size(88, 26);
            this.txtFarnsworth.TabIndex = 16;
            this.txtFarnsworth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFarnsworth_KeyPress);
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(816, 141);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(88, 26);
            this.txtDuration.TabIndex = 17;
            this.txtDuration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDuration_KeyPress);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(786, 514);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(118, 43);
            this.btnStartStop.TabIndex = 18;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnKoch
            // 
            this.btnKoch.AutoSize = true;
            this.btnKoch.Location = new System.Drawing.Point(24, 484);
            this.btnKoch.Name = "btnKoch";
            this.btnKoch.Size = new System.Drawing.Size(70, 24);
            this.btnKoch.TabIndex = 19;
            this.btnKoch.TabStop = true;
            this.btnKoch.Text = "Koch";
            this.btnKoch.UseVisualStyleBackColor = true;
            this.btnKoch.Click += new System.EventHandler(this.btnKoch_Click);
            // 
            // btnCustom
            // 
            this.btnCustom.AutoSize = true;
            this.btnCustom.Location = new System.Drawing.Point(24, 522);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(89, 24);
            this.btnCustom.TabIndex = 20;
            this.btnCustom.TabStop = true;
            this.btnCustom.Text = "Custom";
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            // 
            // cmbKoch
            // 
            this.cmbKoch.FormattingEnabled = true;
            this.cmbKoch.Location = new System.Drawing.Point(139, 483);
            this.cmbKoch.Name = "cmbKoch";
            this.cmbKoch.Size = new System.Drawing.Size(100, 28);
            this.cmbKoch.TabIndex = 21;
            this.cmbKoch.SelectedIndexChanged += new System.EventHandler(this.cmbKoch_SelectedIndexChanged);
            // 
            // txtCustom
            // 
            this.txtCustom.Location = new System.Drawing.Point(139, 522);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.Size = new System.Drawing.Size(602, 26);
            this.txtCustom.TabIndex = 22;
            // 
            // chkFavorNew
            // 
            this.chkFavorNew.AutoSize = true;
            this.chkFavorNew.Location = new System.Drawing.Point(259, 484);
            this.chkFavorNew.Name = "chkFavorNew";
            this.chkFavorNew.Size = new System.Drawing.Size(192, 24);
            this.chkFavorNew.TabIndex = 23;
            this.chkFavorNew.Text = "Favor New Characters";
            this.chkFavorNew.UseVisualStyleBackColor = true;
            this.chkFavorNew.CheckStateChanged += new System.EventHandler(this.chkFavorNew_CheckStateChanged);
            // 
            // sliderVolume
            // 
            this.sliderVolume.LargeChange = 10;
            this.sliderVolume.Location = new System.Drawing.Point(747, 185);
            this.sliderVolume.Name = "sliderVolume";
            this.sliderVolume.Size = new System.Drawing.Size(116, 69);
            this.sliderVolume.TabIndex = 25;
            this.sliderVolume.TickFrequency = 2;
            this.sliderVolume.Value = 10;
            this.sliderVolume.Scroll += new System.EventHandler(this.sliderVolume_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(678, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 20);
            this.label8.TabIndex = 24;
            this.label8.Text = "Volume";
            // 
            // txtStartDelay
            // 
            this.txtStartDelay.Location = new System.Drawing.Point(299, 185);
            this.txtStartDelay.Name = "txtStartDelay";
            this.txtStartDelay.Size = new System.Drawing.Size(35, 26);
            this.txtStartDelay.TabIndex = 26;
            // 
            // txtStopDelay
            // 
            this.txtStopDelay.Location = new System.Drawing.Point(584, 185);
            this.txtStopDelay.Name = "txtStopDelay";
            this.txtStopDelay.Size = new System.Drawing.Size(35, 26);
            this.txtStopDelay.TabIndex = 27;
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(869, 185);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(35, 26);
            this.txtVolume.TabIndex = 28;
            // 
            // mnuStrip
            // 
            this.mnuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuContextRestoreDefaults,
            this.mnuContextSetProsignKeys,
            this.mnuContextAbout});
            this.mnuStrip.Name = "mnuStrip";
            this.mnuStrip.Size = new System.Drawing.Size(241, 94);
            // 
            // mnuContextRestoreDefaults
            // 
            this.mnuContextRestoreDefaults.Name = "mnuContextRestoreDefaults";
            this.mnuContextRestoreDefaults.Size = new System.Drawing.Size(240, 30);
            this.mnuContextRestoreDefaults.Text = "Restore Defaults";
            this.mnuContextRestoreDefaults.Click += new System.EventHandler(this.mnuContextRestoreDefaults_Click);
            // 
            // mnuContextSetProsignKeys
            // 
            this.mnuContextSetProsignKeys.Name = "mnuContextSetProsignKeys";
            this.mnuContextSetProsignKeys.Size = new System.Drawing.Size(240, 30);
            this.mnuContextSetProsignKeys.Text = "Set Prosign Keys...";
            this.mnuContextSetProsignKeys.Click += new System.EventHandler(this.mnuContextSetProsignKeys_Click);
            // 
            // mnuContextAbout
            // 
            this.mnuContextAbout.Name = "mnuContextAbout";
            this.mnuContextAbout.Size = new System.Drawing.Size(240, 30);
            this.mnuContextAbout.Text = "About...";
            this.mnuContextAbout.Click += new System.EventHandler(this.mnuContextAbout_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 580);
            this.ContextMenuStrip = this.mnuStrip;
            this.Controls.Add(this.txtVolume);
            this.Controls.Add(this.txtStopDelay);
            this.Controls.Add(this.txtStartDelay);
            this.Controls.Add(this.sliderVolume);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkFavorNew);
            this.Controls.Add(this.txtCustom);
            this.Controls.Add(this.cmbKoch);
            this.Controls.Add(this.btnCustom);
            this.Controls.Add(this.btnKoch);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.txtFarnsworth);
            this.Controls.Add(this.txtWPM);
            this.Controls.Add(this.txtFrequency);
            this.Controls.Add(this.sliderStopDelay);
            this.Controls.Add(this.sliderStartDelay);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAnalysis);
            this.Controls.Add(this.sliderDuration);
            this.Controls.Add(this.sliderFarnsworth);
            this.Controls.Add(this.sliderWPM);
            this.Controls.Add(this.sliderFrequency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Morse Code Trainer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.sliderFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderWPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFarnsworth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStartDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStopDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderVolume)).EndInit();
            this.mnuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar sliderFrequency;
        private System.Windows.Forms.TrackBar sliderWPM;
        private System.Windows.Forms.TrackBar sliderDuration;
        private System.Windows.Forms.TrackBar sliderFarnsworth;
        private System.Windows.Forms.RichTextBox txtAnalysis;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar sliderStartDelay;
        private System.Windows.Forms.TrackBar sliderStopDelay;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.TextBox txtWPM;
        private System.Windows.Forms.TextBox txtFarnsworth;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.RadioButton btnKoch;
        private System.Windows.Forms.RadioButton btnCustom;
        private System.Windows.Forms.ComboBox cmbKoch;
        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.CheckBox chkFavorNew;
        private System.Windows.Forms.TrackBar sliderVolume;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStartDelay;
        private System.Windows.Forms.TextBox txtStopDelay;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.ContextMenuStrip mnuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuContextRestoreDefaults;
        private System.Windows.Forms.ToolStripMenuItem mnuContextSetProsignKeys;
        private System.Windows.Forms.ToolStripMenuItem mnuContextAbout;
    }
}

