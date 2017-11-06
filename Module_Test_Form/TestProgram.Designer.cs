namespace Module_Test_Form
{
    partial class TestProgram
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Ethernet_XGT = new System.Windows.Forms.Button();
            this.button_EXGT_Send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_EXGT_Adr = new System.Windows.Forms.TextBox();
            this.textBox_EXGT_Data = new System.Windows.Forms.TextBox();
            this.label_Ethernet_XGT = new System.Windows.Forms.Label();
            this.timer_EXGT = new System.Windows.Forms.Timer(this.components);
            this.button_EXGT_Clear = new System.Windows.Forms.Button();
            this.rsGridView_EXGT = new RS_Module_for_Net35.Windows_Controls.RsGridView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_EXGT_Data);
            this.groupBox1.Controls.Add(this.textBox_EXGT_Adr);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label_Ethernet_XGT);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_EXGT_Send);
            this.groupBox1.Controls.Add(this.button_EXGT_Clear);
            this.groupBox1.Controls.Add(this.button_Ethernet_XGT);
            this.groupBox1.Controls.Add(this.rsGridView_EXGT);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 491);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ethernet XGT";
            // 
            // button_Ethernet_XGT
            // 
            this.button_Ethernet_XGT.Location = new System.Drawing.Point(7, 21);
            this.button_Ethernet_XGT.Name = "button_Ethernet_XGT";
            this.button_Ethernet_XGT.Size = new System.Drawing.Size(251, 23);
            this.button_Ethernet_XGT.TabIndex = 0;
            this.button_Ethernet_XGT.Text = "통신 시작/종료";
            this.button_Ethernet_XGT.UseVisualStyleBackColor = true;
            this.button_Ethernet_XGT.Click += new System.EventHandler(this.button_Ethernet_XGT_Click);
            // 
            // button_EXGT_Send
            // 
            this.button_EXGT_Send.Location = new System.Drawing.Point(264, 50);
            this.button_EXGT_Send.Name = "button_EXGT_Send";
            this.button_EXGT_Send.Size = new System.Drawing.Size(69, 55);
            this.button_EXGT_Send.TabIndex = 3;
            this.button_EXGT_Send.Text = "전송";
            this.button_EXGT_Send.UseVisualStyleBackColor = true;
            this.button_EXGT_Send.Click += new System.EventHandler(this.button_EXGT_Send_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(7, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "주소";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(7, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "값";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_EXGT_Adr
            // 
            this.textBox_EXGT_Adr.Location = new System.Drawing.Point(96, 50);
            this.textBox_EXGT_Adr.Name = "textBox_EXGT_Adr";
            this.textBox_EXGT_Adr.Size = new System.Drawing.Size(162, 21);
            this.textBox_EXGT_Adr.TabIndex = 1;
            // 
            // textBox_EXGT_Data
            // 
            this.textBox_EXGT_Data.Location = new System.Drawing.Point(96, 82);
            this.textBox_EXGT_Data.Name = "textBox_EXGT_Data";
            this.textBox_EXGT_Data.Size = new System.Drawing.Size(162, 21);
            this.textBox_EXGT_Data.TabIndex = 2;
            // 
            // label_Ethernet_XGT
            // 
            this.label_Ethernet_XGT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Ethernet_XGT.Location = new System.Drawing.Point(264, 21);
            this.label_Ethernet_XGT.Name = "label_Ethernet_XGT";
            this.label_Ethernet_XGT.Size = new System.Drawing.Size(69, 26);
            this.label_Ethernet_XGT.TabIndex = 2;
            this.label_Ethernet_XGT.Text = "상태";
            this.label_Ethernet_XGT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_EXGT
            // 
            this.timer_EXGT.Tick += new System.EventHandler(this.timer_EXGT_Tick);
            // 
            // button_EXGT_Clear
            // 
            this.button_EXGT_Clear.Location = new System.Drawing.Point(6, 109);
            this.button_EXGT_Clear.Name = "button_EXGT_Clear";
            this.button_EXGT_Clear.Size = new System.Drawing.Size(327, 23);
            this.button_EXGT_Clear.TabIndex = 4;
            this.button_EXGT_Clear.Text = "Clear";
            this.button_EXGT_Clear.UseVisualStyleBackColor = true;
            this.button_EXGT_Clear.Click += new System.EventHandler(this.button_EXGT_Clear_Click);
            // 
            // rsGridView_EXGT
            // 
            this.rsGridView_EXGT.Location = new System.Drawing.Point(6, 138);
            this.rsGridView_EXGT.Name = "rsGridView_EXGT";
            this.rsGridView_EXGT.Size = new System.Drawing.Size(327, 347);
            this.rsGridView_EXGT.TabIndex = 5;
            // 
            // TestProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 516);
            this.Controls.Add(this.groupBox1);
            this.Name = "TestProgram";
            this.Text = "TestProgram";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private RS_Module_for_Net35.Windows_Controls.RsGridView rsGridView_EXGT;
        private System.Windows.Forms.TextBox textBox_EXGT_Data;
        private System.Windows.Forms.TextBox textBox_EXGT_Adr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_EXGT_Send;
        private System.Windows.Forms.Button button_Ethernet_XGT;
        private System.Windows.Forms.Label label_Ethernet_XGT;
        private System.Windows.Forms.Timer timer_EXGT;
        private System.Windows.Forms.Button button_EXGT_Clear;
    }
}