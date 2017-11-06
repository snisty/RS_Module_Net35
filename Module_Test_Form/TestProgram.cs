using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RS_Module_for_Net35;
using RS_Module_for_Net35.Communication;
using RS_Module_for_Net35.Static;
using RS_Module_for_Net35.Network;

namespace Module_Test_Form
{
    public partial class TestProgram : Form
    {
        public TestProgram()
        {
            InitializeComponent();
        }

        private bool bEXGT_Connected = false;
        Ethernet_XGT_PLC EXGT = new Ethernet_XGT_PLC();

        private void button_EXGT_Send_Click(object sender, EventArgs e)
        {
            EXGT.PLC_Write_Independent(Ethernet_XGT_PLC.DataType.Word, textBox_EXGT_Adr.Text, textBox_EXGT_Data.Text);
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

        private void button_EXGT_Clear_Click(object sender, EventArgs e)
        {
            EXGT.PLC_Write_Continuity(Ethernet_XGT_PLC.DataType.Word, "DW0000", "0_/1_/2_/3_/4_/5_/6_/7_/8_/9_/10_/11_/12_/13_/14_/15_/16_/17_/18_/19");
        }
    }
}
