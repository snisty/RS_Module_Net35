using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using RS_Module_for_Net35;
using RS_Module_for_Net35.Communication;
using RS_Module_for_Net35.Static;
using RS_Module_for_Net35.Network;

namespace Module_Test_Form
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            
            this.Text = this.Text + "(Build Date : " + DevUtility.GetBuildTime(Assembly.GetExecutingAssembly()).ToString() + ")";
        }

        #region RS232C_Scale
        private bool bRS232C_Scale = false;

        public RS232C_Scale CommScale = new RS232C_Scale();

        //통신 시작
        public void Start_CommScale()
        {
            CommScale.Parsing = new RS232C_Scale.CommDelegate(Scale_Parsing_Big);
            CommScale.Open("COM2", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
        }

        //통신 종료
        public void End_CommScale()
        {
            CommScale.Close();
        }

        //통신 값 파싱하여 double만 남김
        static double Scale_Parsing_Big(string sData)
        {
            if (sData != "")
            {
                int index1 = sData.LastIndexOf("NET:") + 4;
                int index2 = sData.IndexOf("kg\r\n", index1);
                if (index2 == -1)
                {
                    throw new Exception();
                }
                string sum = sData.Substring(index1, index2 - index1);
                try
                {
                    return double.Parse(sum.Trim());
                }
                catch
                {
                    throw new Exception();
                }
            }
            throw new Exception();
        }

        //테스트 시작 종료 버튼
        private void button_RS232C_Scale_Click(object sender, EventArgs e)
        {
            if (bRS232C_Scale == true)
            {   //핑 체크 종료
                End_CommScale();
                label_RS232C_Scale.BackColor = Color.Red;
                label_RS232C_Scale.ForeColor = Color.White;
                label_RS232C_Scale.Text = "X";
                timer_RS232C_Scale.Enabled = false;
            }
            else
            {   //핑 체크 실행
                Start_CommScale();
                label_RS232C_Scale.BackColor = Color.Lime;
                label_RS232C_Scale.ForeColor = Color.Black;
                label_RS232C_Scale.Text = "O";
                timer_RS232C_Scale.Enabled = true;
            }
            bRS232C_Scale = !bRS232C_Scale;
        }

        //타이머
        private void timer_RS232C_Scale_Tick(object sender, EventArgs e)
        {
            textBox_RS232C_Scale.Text = CommScale.Value.ToString();
        }
        #endregion

        #region PingCheck Test
        private void PingCheckTest()
        {
            PingObj.HostIP = "www.google.com";
            PingObj.Max_Write_ms = 50;
            PingObj.Start();
        }
        private bool bPingCheck = false;
        PingCheck PingObj = new PingCheck();
        //핑 체크 테스트 버튼
        private void button_PingCheck_Click(object sender, EventArgs e)
        {
            if (bPingCheck == true)
            {   //핑 체크 종료
                PingObj.End();
                label_PingCheck.BackColor = Color.Red;
                label_PingCheck.ForeColor = Color.White;
                label_PingCheck.Text = "X";
            }
            else
            {   //핑 체크 실행
                PingCheckTest();
                label_PingCheck.BackColor = Color.Lime;
                label_PingCheck.ForeColor = Color.Black;
                label_PingCheck.Text = "O";
            }
            bPingCheck = !bPingCheck;
        }
        #endregion

        #region DevUtility Test
        #endregion

        private bool bEXGT_Connected = false;
        Ethernet_XGT_PLC EXGT = new Ethernet_XGT_PLC();

        private void button_Ethernet_XGT_Click(object sender, EventArgs e)
        {
            if (bEXGT_Connected == false)
            {
                EXGT = new Ethernet_XGT_PLC();
                EXGT.PLC_IP_Address = "192.168.1.219";
                bEXGT_Connected = true;
                label_Ethernet_XGT.BackColor = Color.Lime;
                label_Ethernet_XGT.ForeColor = Color.Black;
                label_Ethernet_XGT.Text = "O";

                rsGridView_EXGT.Clear();
                rsGridView_EXGT.AddColumn("주소");
                rsGridView_EXGT.AddColumn("값");
                //개별 읽기 테스트용
                rsGridView_EXGT.AddRow();
                rsGridView_EXGT.AddRow();
                rsGridView_EXGT.SetText(0, "주소", "DW0000");
                rsGridView_EXGT.SetText(1, "주소", "DW0001");
                //연속 읽기 테스트용
                rsGridView_EXGT.AddRow();
                rsGridView_EXGT.AddRow();
                rsGridView_EXGT.SetText(2, "주소", "DW0010");
                rsGridView_EXGT.SetText(3, "주소", "DW0011");
                timer_EXGT.Enabled = true;
            }
            else
            {
                bEXGT_Connected = false;
                label_Ethernet_XGT.BackColor = Color.Red;
                label_Ethernet_XGT.ForeColor = Color.White;
                label_Ethernet_XGT.Text = "X";
                timer_EXGT.Enabled = false;
            }
        }

        private void timer_EXGT_Tick(object sender, EventArgs e)
        {
            //개별읽기
            List<int> TempList = EXGT.PLC_Read_Independent(Ethernet_XGT_PLC.DataType.Word, "DW0000_/DW0001");
            rsGridView_EXGT.SetText(0, "값", TempList[0].ToString());
            rsGridView_EXGT.SetText(1, "값", TempList[1].ToString());

            //연속읽기
            TempList = EXGT.PLC_Read_Continuity(Ethernet_XGT_PLC.DataType.Continuity, "DB0010", 2);
            rsGridView_EXGT.SetText(2, "값", TempList[0].ToString());
            rsGridView_EXGT.SetText(3, "값", TempList[1].ToString());
        }

        private void button_EXGT_Send_Click(object sender, EventArgs e)
        {
            EXGT.PLC_Write_Independent(Ethernet_XGT_PLC.DataType.Word, textBox_EXGT_Adr.Text, textBox_EXGT_Data.Text);
        }
    }
}
