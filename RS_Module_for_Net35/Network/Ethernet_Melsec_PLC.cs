using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace RS_Module_for_Net35.Communication
{
    public class Ethernet_Melsec_PLC
    {
        /// <summary>
        /// PLC의 IP 주소를 가져오거나 설정합니다.
        /// (Default : 192.168.0.1)
        /// </summary>
        public string PLC_IP_Address = "192.168.0.1";

        /// <summary>
        /// PLC의 포트번호를 가져오거나 설정합니다.
        /// (Default : 5001)
        /// </summary>
        public int PLC_Port_Number = 5001;

        /// <summary>
        /// 이더넷 통신 모듈 모델 리스트
        /// </summary>
        public enum ModuleList { QJ71E71_100 }

        /// <summary>
        /// 이더넷 통신 모듈을 가져오거나 설정합니다.
        /// (Default : QJ71E71-100)
        /// </summary>
        public ModuleList SelectModule = ModuleList.QJ71E71_100;
        
        /// <summary>
        /// PLC 연속 읽기
        /// </summary>
        public List<int> PLC_Read_Continuity(string StartAdr, int ReadCount)
        {
            switch (SelectModule)
            {
                case ModuleList.QJ71E71_100:
                    return QJ71E71_100_Read_Continuity(StartAdr, ReadCount);
                default:
                    throw new Exception("정의되지 않은 모듈을 선택하였습니다.");
            }
        }
        
        /// <summary>
        /// QJ71E71-100용 연속 읽기 메소드
        /// </summary>
        /// <param name="StartAdr">시작 주소</param>
        /// <param name="ReadCount">시작 주소 부터 # 개</param>
        /// <returns>숫서대로 입력된 List</returns>
        private List<int> QJ71E71_100_Read_Continuity(string StartAdr, int ReadCount)
        {
            Socket SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            List<int> ResultList = new List<int>();

            List<byte> SendBuffer = new List<byte>();
            SendBuffer.Add(0x50);   //어플리케이션 데이터(최대 2040 byte)
            SendBuffer.Add(0x00);
            SendBuffer.Add(0x00);
            SendBuffer.Add(0xFF);
            SendBuffer.Add(0xFF);
            SendBuffer.Add(0x03);
            SendBuffer.Add(0x00);
            SendBuffer.Add(0x0C);

            SendBuffer.Add(0x00);
            SendBuffer.Add(0x10);   //10
            SendBuffer.Add(0x00);
            SendBuffer.Add(0x01);
            SendBuffer.Add(0x04);
            SendBuffer.Add(0x00);
            SendBuffer.Add(0x00);   //15

            byte[] bAdr = BitConverter.GetBytes(int.Parse(StartAdr.Replace("D", "")));
            SendBuffer.Add(bAdr[0]);
            SendBuffer.Add(bAdr[1]);
            SendBuffer.Add(bAdr[2]);
            SendBuffer.Add(0xA8);
            SendBuffer.Add((byte)ReadCount);
            SendBuffer.Add((byte)(ReadCount >> 8));
            
            SocketClient.Connect(IPAddress.Parse(PLC_IP_Address), PLC_Port_Number);
            SocketClient.Send(SendBuffer.ToArray(), 0, SendBuffer.Count, SocketFlags.None);
            byte[] ReceiveByte = new byte[2040];
            int ReceiveLen = SocketClient.Receive(ReceiveByte, ReceiveByte.Length, SocketFlags.None);
            if (ReceiveLen > 0)
            {
                string TempTxtValueHex;
                for (int i = 0; i < ReadCount; i++)
                {
                    TempTxtValueHex = string.Format("{0:x2}", ReceiveByte[11 + i * 2 + 1]) + string.Format("{0:x2}", ReceiveByte[11 + i * 2]);
                    ResultList.Add(Convert.ToInt32(TempTxtValueHex, 16));
                }
            }
            SocketClient.Disconnect(true);

            return ResultList;
        }

        /// <summary>
        /// PLC 연속 쓰기
        /// </summary>
        public bool PLC_Write_Continuity(string StartAdr, List<int> Data)
        {
            switch (SelectModule)
            {
                case ModuleList.QJ71E71_100:
                    return QJ71E71_100_Write_Continuity(StartAdr, Data);
                default:
                    throw new Exception("정의되지 않은 모듈을 선택하였습니다.");
            }
        }

        /// <summary>
        /// QJ71E71-100 용 연속 쓰기 메소드
        /// </summary>
        /// <param name="StartAdr">시작 주소</param>
        /// <param name="Data">시작 주소 이후의 데이터</param>
        /// <returns>성공 여부</returns>
        protected bool QJ71E71_100_Write_Continuity(string StartAdr, List<int> Data)
        {
            Socket SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                List<byte> SendBuffer = new List<byte>();

                SendBuffer.Add(0x50);
                SendBuffer.Add(0x00);
                SendBuffer.Add(0x00);
                SendBuffer.Add(0xFF);
                SendBuffer.Add(0xFF);
                SendBuffer.Add(0x03);
                SendBuffer.Add(0x00);
                SendBuffer.Add(0x16);
                SendBuffer.Add(0x00);
                SendBuffer.Add(0x10);
                SendBuffer.Add(0x00);

                SendBuffer.Add(0x01);
                SendBuffer.Add(0x14);
                SendBuffer.Add(0x00);
                SendBuffer.Add(0x00);

                byte[] bAdr = BitConverter.GetBytes(int.Parse(StartAdr.Replace("D", "")));
                SendBuffer.Add(bAdr[0]);
                SendBuffer.Add(bAdr[1]);
                SendBuffer.Add(bAdr[2]);
                SendBuffer.Add(0xA8);

                SendBuffer.Add((byte)(Data.Count));     //데이터 갯수
                SendBuffer.Add((byte)(Data.Count >> 8));

                foreach (int iData in Data)
                {
                    SendBuffer.Add((byte)(iData));      //데이터
                    SendBuffer.Add((byte)(iData >> 8));
                }

                SocketClient.Connect(IPAddress.Parse(PLC_IP_Address), PLC_Port_Number);
                SocketClient.Send(SendBuffer.ToArray(), 0, SendBuffer.ToArray().Length, 0);
                SocketClient.Disconnect(true);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
