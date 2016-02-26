namespace MorseTrainer
{
    partial class ProsignKeyAssigner
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBT = new System.Windows.Forms.TextBox();
            this.txtSK = new System.Windows.Forms.TextBox();
            this.txtAR = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "<BT>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "<SK>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "<AR>";
            // 
            // txtBT
            // 
            this.txtBT.Location = new System.Drawing.Point(82, 12);
            this.txtBT.Name = "txtBT";
            this.txtBT.Size = new System.Drawing.Size(56, 26);
            this.txtBT.TabIndex = 3;
            this.txtBT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // txtSK
            // 
            this.txtSK.Location = new System.Drawing.Point(82, 44);
            this.txtSK.Name = "txtSK";
            this.txtSK.Size = new System.Drawing.Size(56, 26);
            this.txtSK.TabIndex = 4;
            this.txtSK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // txtAR
            // 
            this.txtAR.Location = new System.Drawing.Point(82, 76);
            this.txtAR.Name = "txtAR";
            this.txtAR.Size = new System.Drawing.Size(56, 26);
            this.txtAR.TabIndex = 5;
            this.txtAR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // ProsignKeyAssigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(171, 123);
            this.Controls.Add(this.txtAR);
            this.Controls.Add(this.txtSK);
            this.Controls.Add(this.txtBT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ProsignKeyAssigner";
            this.Text = "Prosign Keys";
            this.VisibleChanged += new System.EventHandler(this.ProsignKeyAssigner_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBT;
        private System.Windows.Forms.TextBox txtSK;
        private System.Windows.Forms.TextBox txtAR;
    }
}