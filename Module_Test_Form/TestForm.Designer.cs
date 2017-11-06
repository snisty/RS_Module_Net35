namespace Module_Test_Form
{
    partial class TestForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_PingCheck = new System.Windows.Forms.Button();
            this.label_PingCheck = new System.Windows.Forms.Label();
            this.button_RS232C_Scale = new System.Windows.Forms.Button();
            this.label_RS232C_Scale = new System.Windows.Forms.Label();
            this.textBox_RS232C_Scale = new System.Windows.Forms.TextBox();
            this.timer_RS232C_Scale = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Ethernet_XGT = new System.Windows.Forms.Button();
            this.label_Ethernet_XGT = new System.Windows.Forms.Label();
            this.timer_EXGT = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox_EXGT_Adr = new System.Windows.Forms.TextBox();
            this.textBox_EXGT_Data = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_EXGT_Send = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_PingCheck
            // 
            this.button_PingCheck.Location = new System.Drawing.Point(3, 3);
            this.button_PingCheck.Name = "button_PingCheck";
            this.button_PingCheck.Size = new System.Drawing.Size(343, 23);
            this.button_PingCheck.TabIndex = 0;
            this.button_PingCheck.Text = "PingCheck Module";
            this.button_PingCheck.UseVisualStyleBackColor = true;
            this.button_PingCheck.Click += new System.EventHandler(this.button_PingCheck_Click);
            // 
            // label_PingCheck
            // 
            this.label_PingCheck.BackColor = System.Drawing.Color.Red;
            this.label_PingCheck.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_PingCheck.ForeColor = System.Drawing.Color.White;
            this.label_PingCheck.Location = new System.Drawing.Point(349, 3);
            this.label_PingCheck.Name = "label_PingCheck";
            this.label_PingCheck.Size = new System.Drawing.Size(36, 23);
            this.label_PingCheck.TabIndex = 1;
            this.label_PingCheck.Text = "X";
            this.label_PingCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_RS232C_Scale
            // 
            this.button_RS232C_Scale.Location = new System.Drawing.Point(3, 32);
            this.button_RS232C_Scale.Name = "button_RS232C_Scale";
            this.button_RS232C_Scale.Size = new System.Drawing.Size(217, 23);
            this.button_RS232C_Scale.TabIndex = 0;
            this.button_RS232C_Scale.Text = "RS232C_Scale Module";
            this.button_RS232C_Scale.UseVisualStyleBackColor = true;
            this.button_RS232C_Scale.Click += new System.EventHandler(this.button_RS232C_Scale_Click);
            // 
            // label_RS232C_Scale
            // 
            this.label_RS232C_Scale.BackColor = System.Drawing.Color.Red;
            this.label_RS232C_Scale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_RS232C_Scale.ForeColor = System.Drawing.Color.White;
            this.label_RS232C_Scale.Location = new System.Drawing.Point(349, 32);
            this.label_RS232C_Scale.Name = "label_RS232C_Scale";
            this.label_RS232C_Scale.Size = new System.Drawing.Size(36, 23);
            this.label_RS232C_Scale.TabIndex = 1;
            this.label_RS232C_Scale.Text = "X";
            this.label_RS232C_Scale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_RS232C_Scale
            // 
            this.textBox_RS232C_Scale.Font = new System.Drawing.Font("굴림", 10F);
            this.textBox_RS232C_Scale.Location = new System.Drawing.Point(223, 32);
            this.textBox_RS232C_Scale.Name = "textBox_RS232C_Scale";
            this.textBox_RS232C_Scale.Size = new System.Drawing.Size(123, 23);
            this.textBox_RS232C_Scale.TabIndex = 2;
            this.textBox_RS232C_Scale.Text = "Data";
            this.textBox_RS232C_Scale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timer_RS232C_Scale
            // 
            this.timer_RS232C_Scale.Tick += new System.EventHandler(this.timer_RS232C_Scale_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(343, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "RS232C_XGT_PLC Module";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(349, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Ethernet_XGT
            // 
            this.button_Ethernet_XGT.Location = new System.Drawing.Point(0, 0);
            this.button_Ethernet_XGT.Name = "button_Ethernet_XGT";
            this.button_Ethernet_XGT.Size = new System.Drawing.Size(100, 100);
            this.button_Ethernet_XGT.TabIndex = 0;
            this.button_Ethernet_XGT.Text = "Ethernet_XGT_PLC Module";
            this.button_Ethernet_XGT.UseVisualStyleBackColor = true;
            this.button_Ethernet_XGT.Click += new System.EventHandler(this.button_Ethernet_XGT_Click);
            // 
            // label_Ethernet_XGT
            // 
            this.label_Ethernet_XGT.BackColor = System.Drawing.Color.Red;
            this.label_Ethernet_XGT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Ethernet_XGT.ForeColor = System.Drawing.Color.White;
            this.label_Ethernet_XGT.Location = new System.Drawing.Point(349, 3);
            this.label_Ethernet_XGT.Name = "label_Ethernet_XGT";
            this.label_Ethernet_XGT.Size = new System.Drawing.Size(36, 23);
            this.label_Ethernet_XGT.TabIndex = 1;
            this.label_Ethernet_XGT.Text = "X";
            this.label_Ethernet_XGT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_EXGT
            // 
            this.timer_EXGT.Tick += new System.EventHandler(this.timer_EXGT_Tick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_PingCheck);
            this.panel3.Controls.Add(this.button_RS232C_Scale);
            this.panel3.Controls.Add(this.label_PingCheck);
            this.panel3.Controls.Add(this.textBox_RS232C_Scale);
            this.panel3.Controls.Add(this.label_RS232C_Scale);
            this.panel3.Location = new System.Drawing.Point(413, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(389, 180);
            this.panel3.TabIndex = 6;
            // 
            // textBox_EXGT_Adr
            // 
            this.textBox_EXGT_Adr.Font = new System.Drawing.Font("굴림", 10F);
            this.textBox_EXGT_Adr.Location = new System.Drawing.Point(87, 192);
            this.textBox_EXGT_Adr.Name = "textBox_EXGT_Adr";
            this.textBox_EXGT_Adr.Size = new System.Drawing.Size(228, 23);
            this.textBox_EXGT_Adr.TabIndex = 2;
            this.textBox_EXGT_Adr.Text = "Data";
            this.textBox_EXGT_Adr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_EXGT_Data
            // 
            this.textBox_EXGT_Data.Font = new System.Drawing.Font("굴림", 10F);
            this.textBox_EXGT_Data.Location = new System.Drawing.Point(87, 221);
            this.textBox_EXGT_Data.Name = "textBox_EXGT_Data";
            this.textBox_EXGT_Data.Size = new System.Drawing.Size(228, 23);
            this.textBox_EXGT_Data.TabIndex = 2;
            this.textBox_EXGT_Data.Text = "Data";
            this.textBox_EXGT_Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("굴림", 10F);
            this.label2.Location = new System.Drawing.Point(3, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "주소";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("굴림", 10F);
            this.label3.Location = new System.Drawing.Point(3, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "데이터";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_EXGT_Send
            // 
            this.button_EXGT_Send.Location = new System.Drawing.Point(322, 192);
            this.button_EXGT_Send.Name = "button_EXGT_Send";
            this.button_EXGT_Send.Size = new System.Drawing.Size(62, 52);
            this.button_EXGT_Send.TabIndex = 5;
            this.button_EXGT_Send.Text = "전송";
            this.button_EXGT_Send.UseVisualStyleBackColor = true;
            this.button_EXGT_Send.Click += new System.EventHandler(this.button_EXGT_Send_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 461);
            this.Controls.Add(this.panel3);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_PingCheck;
        private System.Windows.Forms.Label label_PingCheck;
        private System.Windows.Forms.Button button_RS232C_Scale;
        private System.Windows.Forms.Label label_RS232C_Scale;
        private System.Windows.Forms.TextBox textBox_RS232C_Scale;
        private System.Windows.Forms.Timer timer_RS232C_Scale;
        private RS_Module_for_Net35.Windows_Controls.RsGridView rsGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Ethernet_XGT;
        private System.Windows.Forms.Label label_Ethernet_XGT;
        private RS_Module_for_Net35.Windows_Controls.RsGridView rsGridView_EXGT;
        private System.Windows.Forms.Timer timer_EXGT;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_EXGT_Data;
        private System.Windows.Forms.TextBox textBox_EXGT_Adr;
        private System.Windows.Forms.Button button_EXGT_Send;
    }
}

