using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RS_Module_for_Net35.Communication
{
    public class Ethernet_XGT_PLC
    {
        /// <summary>
        /// 현재 객체에 설정된 PLC IP를 가져오거나 설정합니다.
        /// </summary>
        public string PLC_IP_Address = "192.168.0.1";
        /// <summary>
        /// 현재 객체에 설정된 PLC Port를 가져오거나 설정합니다.
        /// 기본값 : 2004
        /// </summary>
        public int PLC_Port_Number = 2004;
        
        /// <summary>
        /// 헤더 프레임을 가져옵니다.
        /// </summary>
        private byte[] GetHeader(int CommandLenth)
        {
            byte[] HeaderFrame = new byte[20];
            HeaderFrame[0] = Convert.ToByte('L');   //Company ID
            HeaderFrame[1] = Convert.ToByte('S');
            HeaderFrame[2] = Convert.ToByte('I');
            HeaderFrame[3] = Convert.ToByte('S');
            HeaderFrame[4] = Convert.ToByte('-');
            HeaderFrame[5] = Convert.ToByte('X');
            HeaderFrame[6] = Convert.ToByte('G');
            HeaderFrame[7] = Convert.ToByte('T');

            HeaderFrame[8] = 0x00;  //예약 영역 : Reserved
            HeaderFrame[9] = 0x00;

            HeaderFrame[10] = 0x00; //PLC Info : MMI -> PLC 인 경우 0x00 고정
            HeaderFrame[11] = 0x00;

            HeaderFrame[12] = 0x00; //CPU Info : Reserved 영역을 통해서 XGK/XGI 시리즈를 판단. (뭔 소리지?)

            HeaderFrame[13] = 0x33; //Source of Frame : MMI -> PLC = 0x33, PLC -> MMI = 0x11 고정

            HeaderFrame[14] = 0x00; //Invoke ID : 응답 프레임에 이 프레임이 붙어서 옴. 동시 통신을 하는경우에 사용될수도 있긴함..
            HeaderFrame[15] = 0x00;

            HeaderFrame[16] = (byte)CommandLenth;    //명령어 길이
            HeaderFrame[17] = (byte)(CommandLenth >> 8);

            HeaderFrame[18] = 0x00; //0~3 bit = FEnet I/F 슬롯 번호, 4~7 = FEnet I/F 베이스 번호
            
            for (int i = 0; i < 19; i++)  //헤더프레임의 Byte Sum
            {
                if (i == 0)
                {
                    HeaderFrame[19] = HeaderFrame[i];
                }
                else
                {
                    HeaderFrame[19] += (byte)HeaderFrame[i];
                }
            }

            return HeaderFrame;
        }

        /// <summary>
        /// Independent는 16개 까지 한번에 가능.
        /// Continuity는 Byte형만 가능합니다.
        /// </summary>
        public enum CommandType { Read, Write };
        /// <summary>
        /// CommandType에 따른 코드를 리턴
        /// </summary>
        /// <returns>byte[2]</returns>
        private byte[] CommandCode(CommandType CT)
        {
            byte[] by = new byte[2];

            by[1] = 0x00;
            switch (CT)
            {
                case CommandType.Read:
                    by[0] = 0x54;
                    break;
                case CommandType.Write:
                    by[0] = 0x58;
                    break;
            }

            return by;
        }

        /// <summary>
        /// 데이터 타입
        /// Bit = X, Word = W
        /// </summary>
        public enum DataType { Bit, Byte, Word, DWord, LWord, Continuity };
        /// <summary>
        /// DataType에 따른 코드 리턴
        /// </summary>
        /// <returns>byte[2]</returns>
        private byte[] DataTypeCode(DataType DT)
        {
            byte[] by = new byte[2];
            by[1] = 0x00;
            switch (DT)
            {
                case DataType.Bit:
                    by[0] = 0x00;
                    break;
                case DataType.Byte:
                    by[0] = 0x01;
                    break;
                case DataType.Word:
                    by[0] = 0x02;
                    break;
                case DataType.DWord:
                    by[0] = 0x03;
                    break;
                case DataType.LWord:
                    by[0] = 0x04;
                    break;
                case DataType.Continuity:
                    by[0] = 0x14;
                    break;
            }

            return by;
        }
        /// <summary>
        /// DataType에 따른 Byte 수
        /// </summary>
        private int DataSize(DataType DT)
        {
            int size = 0;
            switch (DT)
            {
                case DataType.Bit:
                    size = 1;
                    break;
                case DataType.Byte:
                    size = 1;
                    break;
                case DataType.Word:
                    size = 2;
                    break;
                case DataType.DWord:
                    size = 4;
                    break;
                case DataType.LWord:
                    size = 4;
                    break;
                case DataType.Continuity:
                    size = 2;
                    break;
            }
            return size;
        }

        /// <summary>
        /// PLC 개별 읽기
        /// </summary>
        public List<int> PLC_Read_Independent(DataType DT, string Address)
        {
            //명령어 전체가 들어갈 리스트
            List<byte> byteList = new List<byte>();
            //변수 바이트
            List<byte> VariableList = new List<byte>();
            //주소 목록
            string[] sAdrs = Address.Split(new string[] { "_/" }, StringSplitOptions.None);

            if (sAdrs.Length > 16)
            {
                throw new Exception("개별 명령어는 16개를 넘어설 수 없습니다.");
            }

            //주소를 가지고 변수 그룹을 만든다.
            foreach (string Adr in sAdrs)
            {
                VariableList.Add((byte)(Adr.Length + 1));
                VariableList.Add((byte)((Adr.Length + 1) >> 8));

                VariableList.Add(Convert.ToByte('%'));
                foreach (char chr in Adr)
                {
                    VariableList.Add(Convert.ToByte(chr));
                }
            }

            //헤더 생성
            byte[] HeaderFrame = GetHeader(VariableList.Count + 8); //8은 변수그룹을 제외한 기본 프레임 길이(명령어 + 데이터타입 + 예약영역 + 블록수)
            foreach (byte by in HeaderFrame)
            {
                byteList.Add(by);
            }

            //명령어(2 byte)
            byte[] TempByte = CommandCode(CommandType.Read);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            //데이터 타입(2 byte)
            TempByte = DataTypeCode(DT);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            byteList.Add(0x00); //예약영역
            byteList.Add(0x00);

            //블록 수
            byteList.Add((byte)(sAdrs.Length));
            byteList.Add((byte)(sAdrs.Length >> 8));

            //변수 그룹
            foreach (byte by in VariableList)
            {
                byteList.Add(by);
            }

            //TCP 소켓 연결
            Socket ReadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ReadSocket.Connect(new IPEndPoint(IPAddress.Parse(PLC_IP_Address), PLC_Port_Number));
                if (ReadSocket.Connected == false)
                {
                    //실패 시 예외처리
                    throw new Exception();
                }
            }
            catch
            {
                //실패 시 예외처리
                throw new Exception("해당 IP와 TCP 연결에 실패하였습니다.");
            }

            //전송할 프레임
            byte[] SendFrame = byteList.ToArray();
            ReadSocket.Send(SendFrame, 0, SendFrame.Length, 0);     //프레임 전송
            byte[] ReceiveBuffer = new byte[1024];  //수신 버퍼
            int ReceiveLen = ReadSocket.Receive(ReceiveBuffer, ReceiveBuffer.Length, 0);    //데이터 수신

            List<int> ResultList = new List<int>();
            if (ReceiveLen > 0) //가져온 데이터가 있으면
            {
                string ReceiveString = "";
                for (int i = 0; i < sAdrs.Length; i++)
                {
                    ReceiveString = string.Format("{0:x2}", ReceiveBuffer[32 + i * 4 + 1]) + string.Format("{0:x2}", ReceiveBuffer[32 + i * 4]);
                    ResultList.Add(Convert.ToInt32(ReceiveString, 16));
                }
            }
            ReadSocket.Close();

            return ResultList;
        }

        /// <summary>
        /// PLC 연속 읽기
        /// </summary>
        public List<int> PLC_Read_Continuity(DataType DT, string Address, int Count)
        {
            //명령어 전체가 들어갈 리스트
            List<byte> byteList = new List<byte>();
            //변수 바이트
            List<byte> VariableList = new List<byte>();
            
            //변수길이 및 직접변수
            VariableList.Add(0x08);
            VariableList.Add(0x00);

            VariableList.Add(Convert.ToByte('%'));
            VariableList.Add(Convert.ToByte(Address.Substring(0, 1)[0]));
            VariableList.Add(Convert.ToByte('B'));  //Byte 타입 고정
            string AdrNumber = string.Format("{0:D5}", int.Parse(Address.Substring(2, Address.Length - 2)) * 2);
            foreach (char chr in AdrNumber)
            {
                VariableList.Add(Convert.ToByte(chr));
            }
            //헤더 생성
            byte[] HeaderFrame = GetHeader(VariableList.Count + 10); //12는 직접변수 주소를 제외한 나머지 프레임 바이트
            foreach (byte by in HeaderFrame)
            {
                byteList.Add(by);
            }

            //명령어(2 byte)
            byte[] TempByte = CommandCode(CommandType.Read);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            //데이터 타입(2 byte)
            TempByte = DataTypeCode(DataType.Continuity);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            byteList.Add(0x00); //예약영역
            byteList.Add(0x00);

            //블록 수(1 고정)
            byteList.Add(0x01);
            byteList.Add(0x00);

            //변수 그룹
            foreach (byte by in VariableList)
            {
                byteList.Add(by);
            }

            //데이터 갯수
            byteList.Add((byte)(Count * 2));
            byteList.Add((byte)((Count * 2) >> 8));

            //TCP 소켓 연결
            Socket ReadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ReadSocket.Connect(new IPEndPoint(IPAddress.Parse(PLC_IP_Address), PLC_Port_Number));
                if (ReadSocket.Connected == false)
                {
                    //실패 시 예외처리
                    throw new Exception();
                }
            }
            catch
            {
                //실패 시 예외처리
                throw new Exception("해당 IP와 TCP 연결에 실패하였습니다.");
            }

            //전송할 프레임
            byte[] SendFrame = byteList.ToArray();
            ReadSocket.Send(SendFrame, 0, SendFrame.Length, 0);     //프레임 전송
            byte[] ReceiveBuffer = new byte[1024];  //수신 버퍼
            int ReceiveLen = ReadSocket.Receive(ReceiveBuffer, ReceiveBuffer.Length, 0);    //데이터 수신

            List<int> ResultList = new List<int>();
            if (ReceiveLen > 0) //가져온 데이터가 있으면
            {
                string ReceiveString = "";
                for (int i = 0; i < Count; i++)
                {
                    ReceiveString = string.Format("{0:x2}", ReceiveBuffer[32 + i * 2 + 1]) + string.Format("{0:x2}", ReceiveBuffer[32 + i * 2]);
                    ResultList.Add(Convert.ToInt32(ReceiveString, 16));
                }
            }
            ReadSocket.Close();

            return ResultList;
        }

        /// <summary>
        /// PLC 개별 쓰기
        /// </summary>
        /// <param name="DT">데이터 타입</param>
        /// <param name="Address">DW0000_/DW0010_/DW0100</param>
        /// <param name="Data">0_/1_/2</param>
        /// <returns>true or false</returns>
        public bool PLC_Write_Independent(DataType DT, string Address, string Data)
        {
            //명령어 전체가 들어갈 리스트
            List<byte> byteList = new List<byte>();
            //변수 바이트
            List<byte> VariableList = new List<byte>();

            string[] sAdrs = Address.Split(new string[] { "_/" }, StringSplitOptions.None); //주소 목록
            string[] sData = Data.Split(new string[] { "_/" }, StringSplitOptions.None);    //데이터 목록

            if (sAdrs.Length > 16)
            {
                throw new Exception("개별 명령어는 16개를 넘어설 수 없습니다.");
            }

            //주소를 가지고 변수 그룹을 만든다.
            foreach (string Adr in sAdrs)
            {
                //변수명 길이
                VariableList.Add((byte)(Adr.Length + 1));
                VariableList.Add((byte)((Adr.Length + 1) >> 8));
                //변수명
                VariableList.Add(Convert.ToByte('%'));
                foreach (char chr in Adr)
                {
                    VariableList.Add(Convert.ToByte(chr));
                }
            }

            foreach (string sDat in sData)
            {
                int Dat = int.Parse(sDat);  //데이터
                //데이터 크기
                int Size = DataSize(DT);
                VariableList.Add((byte)Size);
                VariableList.Add((byte)(Size >> 8));
                //데이터
                for (int j = 0; j < Size; j++)
                {
                    VariableList.Add((byte)(Dat >> (8 * j)));
                }
            }

            //헤더 생성
            byte[] HeaderFrame = GetHeader(VariableList.Count + 8);
            foreach (byte by in HeaderFrame)
            {
                byteList.Add(by);
            }

            //명령어(2 byte)
            byte[] TempByte = CommandCode(CommandType.Write);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            //데이터 타입(2 byte)
            TempByte = DataTypeCode(DT);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            byteList.Add(0x00); //예약영역
            byteList.Add(0x00);

            //블록 수
            byteList.Add((byte)(sAdrs.Length));
            byteList.Add((byte)(sAdrs.Length >> 8));

            //변수 그룹
            foreach (byte by in VariableList)
            {
                byteList.Add(by);
            }

            //TCP 소켓 연결
            Socket ReadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ReadSocket.Connect(new IPEndPoint(IPAddress.Parse(PLC_IP_Address), PLC_Port_Number));
                if (ReadSocket.Connected == false)
                {
                    //실패 시 예외처리
                    throw new Exception();
                }
            }
            catch
            {
                //실패 시 예외처리
                throw new Exception("해당 IP와 TCP 연결에 실패하였습니다.");
            }

            //전송할 프레임
            byte[] SendFrame = byteList.ToArray();
            ReadSocket.Send(SendFrame, 0, SendFrame.Length, 0);     //프레임 전송
            byte[] ReceiveBuffer = new byte[1024];  //수신 버퍼
            int ReceiveLen = ReadSocket.Receive(ReceiveBuffer, ReceiveBuffer.Length, 0);    //데이터 수신
            ReadSocket.Close();

            if (ReceiveLen > 0) //가져온 데이터가 있으면
            {
                if (ReceiveBuffer[26] == 0x00 && ReceiveBuffer[27] == 0x00)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// PLC 연속 쓰기
        /// </summary>
        /// <param name="DT">Byte단위로 쓰기를 원하는지 Word단위로 쓰기를 원하는지 선택</param>
        /// <param name="Address">DW0000</param>
        /// <param name="Data">0_/1_/2</param>
        /// <returns>true or false</returns>
        public bool PLC_Write_Continuity(DataType DT, string Address, string Data)
        {
            Address = Address.ToUpper();
            //명령어 전체가 들어갈 리스트
            List<byte> byteList = new List<byte>();
            //변수 바이트
            List<byte> VariableList = new List<byte>();
            
            string[] sData = Data.Split(new string[] { "_/" }, StringSplitOptions.None);    //데이터 목록

            //변수길이 및 직접변수
            VariableList.Add(0x08);
            VariableList.Add(0x00);

            VariableList.Add(Convert.ToByte('%'));
            VariableList.Add(Convert.ToByte(Address.Substring(0, 1)[0]));
            VariableList.Add(Convert.ToByte('B'));  //Byte 타입 고정
            string AdrNumber = string.Format("{0:D5}", int.Parse(Address.Substring(2, Address.Length - 2)) * 2);    //왜 x2를 해야하지..?
            foreach (char chr in AdrNumber)
            {
                VariableList.Add(Convert.ToByte(chr));
            }

            //데이터 단위
            int Size = DataSize(DT);
            VariableList.Add((byte)(sData.Length * Size));
            VariableList.Add((byte)((sData.Length * Size) >> 8));
            foreach (string sDat in sData)
            {
                int Dat = int.Parse(sDat);  //데이터
                //데이터
                for (int j = 0; j < Size; j++)
                {
                    VariableList.Add((byte)(Dat >> (8 * j)));
                }
            }

            //헤더 생성
            byte[] HeaderFrame = GetHeader(VariableList.Count + 8);
            foreach (byte by in HeaderFrame)
            {
                byteList.Add(by);
            }

            //명령어(2 byte)
            byte[] TempByte = CommandCode(CommandType.Write);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            //데이터 타입(2 byte)
            TempByte = DataTypeCode(DataType.Continuity);
            foreach (byte by in TempByte)
            {
                byteList.Add(by);
            }

            byteList.Add(0x00); //예약영역
            byteList.Add(0x00);

            //블록 수
            byteList.Add(0x01);
            byteList.Add(0x00);

            //변수 그룹
            foreach (byte by in VariableList)
            {
                byteList.Add(by);
            }

            //TCP 소켓 연결
            Socket ReadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ReadSocket.Connect(new IPEndPoint(IPAddress.Parse(PLC_IP_Address), PLC_Port_Number));
                if (ReadSocket.Connected == false)
                {
                    //실패 시 예외처리
                    throw new Exception();
                }
            }
            catch
            {
                //실패 시 예외처리
                throw new Exception("해당 IP와 TCP 연결에 실패하였습니다.");
            }

            //전송할 프레임
            byte[] SendFrame = byteList.ToArray();
            ReadSocket.Send(SendFrame, 0, SendFrame.Length, 0);     //프레임 전송
            byte[] ReceiveBuffer = new byte[1024];  //수신 버퍼
            int ReceiveLen = ReadSocket.Receive(ReceiveBuffer, ReceiveBuffer.Length, 0);    //데이터 수신
            ReadSocket.Close();

            if (ReceiveLen > 0) //가져온 데이터가 있으면
            {
                if (ReceiveBuffer[26] == 0x00 && ReceiveBuffer[27] == 0x00)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
