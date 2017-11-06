/// <summary>
/// 2017-09-19 최민현
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Data;

namespace RS_Module_for_Net35.Communication
{
    /// <summary>
    /// RS-232C 통신을 이용한 LS산전-XGT/XGB PLC와의 통신을 위한 클래스
    /// </summary>
    public class RS232C_XGT_PLC
    {
        /// <summary>
        /// 시리얼포트 객체
        /// </summary>
        public SerialPort sp232C = new SerialPort();
        /// <summary>
        /// 포트오픈 여부
        /// </summary>
        public bool bConnected = false;

        #region 통신을 위한 신호 상수
        readonly string ENQ = Convert.ToChar(5).ToString();   //ENQ = Chr(5)
        readonly string ACK = Convert.ToChar(6).ToString();   //ACK = Chr(6)
        readonly string STX = Convert.ToChar(2).ToString();   //STX = Chr(2)
        readonly string EOT = Convert.ToChar(4).ToString();   //EOT = Chr(4)
        readonly string ETX = Convert.ToChar(3).ToString();   //ETX = Chr(3)
        readonly string NAK = Convert.ToChar(21).ToString();  //NAK = Chr(21)
        #endregion

        /// <summary>
        /// RS-232C 통신 시작
        /// </summary>
        /// <param name="pPortName">(ex:COM1)</param>
        /// <param name="pBaudRate">(ex:9600)</param>
        /// <param name="pParity">(ex:Parity.None)</param>
        /// <param name="pDataBits">(ex:8)</param>
        /// <param name="pStopBits">(ex:StopBits.One)</param>
        /// <returns>true or false</returns>
        public bool Open(string pPortName, int pBaudRate, Parity pParity, int pDataBits, StopBits pStopBits)
        {
            if (sp232C.IsOpen == true)
            {   //이미 연결되어 있으면
                sp232C.Close(); //연결 끊음
            }

            sp232C.PortName = pPortName;
            sp232C.BaudRate = pBaudRate;
            sp232C.Parity = pParity;
            sp232C.DataBits = pDataBits;
            sp232C.StopBits = pStopBits;
            try
            {
                sp232C.Open();
            }
            catch
            {
                bConnected = false;
                return false;
            }
            bConnected = true;
            return true;
        }

        public enum DataType { Word, Bit }
        /// <summary>
        /// PLC 랜덤 읽기(주소 숫자는 4자리, 5자리 관계 없음.)
        /// 워드영역과 비트영역을 혼용하여 읽을 수 없습니다.
        /// 예1) PLC_DW_Read_Random(DataType.Word, "DW0100_/DW0101_/DW0102_/DW0103");
        /// 예2) PLC_DW_Read_Random(DataType.Bit, "MX0100_/MX0101_/MX00102_/MX0103");
        /// </summary>
        /// <param name="sAddress">주소 리스트</param>
        /// <returns>주소 보낸 순서대로 Value List</returns>
        public List<int> PLC_Read_Random(DataType dType, string sAddress)
        {
            if (bConnected == false) throw new Exception("통신이 연결되어 있지 않습니다.");

            List<int> listRtn = new List<int>();
            string[] sAdrs = sAddress.Split(new string[] { "_/" }, StringSplitOptions.None);  //주소 목록
            int iReadCount = sAdrs.Length;    //주소 갯수
            int iAdrTotalLenth = 0;           //주소 전체 길이
            string sADD = "";

            //16개 초과할 경우 분리하여야 함.
            if (sAdrs.Length > 16)
            {
                string sAdrParam = "";
                for (int i = 0; i < sAdrs.Length; i++)
                {
                    sAdrParam += sAdrs[i] + "_/";
                    if ((i + 1) % 10 == 0)    //10개 마다.
                    {
                        sAdrParam = sAdrParam.Substring(0, sAdrParam.Length - 2);
                        listRtn.AddRange(PLC_Read_Random(dType, sAdrParam));    //재귀호출
                        sAdrParam = "";
                    }
                }
                if (sAdrParam != "")    //마지막
                {
                    sAdrParam = sAdrParam.Substring(0, sAdrParam.Length - 2);
                    listRtn.AddRange(PLC_Read_Random(dType, sAdrParam));    //재귀호출
                }
                return listRtn;
            }

            foreach (string sAdr in sAdrs)
            {
                if (sAdr.Length > 99)
                {
                    Exception tex = new Exception("PLC 주소가 2자리보다 클 수 없습니다.");
                }
                sADD += (sAdr.Length + 1).ToString("00") + "%" + sAdr;
                iAdrTotalLenth += sAdr.Length;
            }

            int iRcvLenth = 0;
            switch (dType)
            {
                case DataType.Word:
                    iRcvLenth = (sAdrs.Length * 6) + 9; //리턴 받아야 하는 데이터 길이. 데이터 주소의 갯수 * 6 + 기본값(9개)
                    break;
                case DataType.Bit:  //비트 타입은 리턴값이 2자리이다.
                    iRcvLenth = (sAdrs.Length * 4) + 9; //리턴 받아야 하는 데이터 길이. 데이터 주소의 갯수 * 4 + 기본값(9개)
                    break;
            }
            if (dType == DataType.Word)
            {
            }
            else if (dType == DataType.Bit)
            {
            }

            string sHEX = "0" + string.Format("{0:X}", Convert.ToInt32(iReadCount));    //읽을 주소 갯수를 16진수로 변경
            sHEX = sHEX.Substring(sHEX.Length - 2, 2);  //끝에 두개를 뺀다. 0xFF 에서 0x를 삭제?

            string sRcv = "";
            do
            {
                sp232C.Write(ENQ + "00RSS" + sHEX + sADD + EOT); //쓰기
                Thread.Sleep(200);  //응답 대기 시간
                sRcv = sp232C.ReadExisting(); //응답 읽어 오기
            } while (sRcv.Length < iRcvLenth || sRcv.Substring(6, 2) != sHEX || sRcv.Substring(0, 1) != ACK || sRcv.Substring(iRcvLenth - 1, 1) != ETX);
            //1) 받은 길이가 보낸길이와 다름
            //2) 읽어온 갯수가 다름(sHEX)
            //3) 첫 데이터가 ACK가 아님
            //4) 마지막 데이터가 ETX가 아님

            sRcv = sRcv.Substring(8, iRcvLenth - 9);   //필요없는 부분 제거
            //리턴 할 리스트
            for (int i = 0, j = 0; i < sRcv.Length; i = i + 6, j++)
            {
                string Temp = "";
                switch (dType)
                {
                    case DataType.Word:
                        Temp = sRcv.Substring(i + 2, 4);
                        break;
                    case DataType.Bit:
                        Temp = sRcv.Substring(i + 2, 2);
                        break;
                }
                listRtn.Add(Convert.ToInt32(string.Format("{0:x2}", Temp), 16));
            }
            return listRtn;
        }

        /// <summary>
        /// PLC 랜덤 쓰기
        /// 예1) PLC_DW_Write_Random("DW0100_/DW0101_/DW00102", "1_/0_/999");
        /// </summary>
        /// <param name="sAddress">주소 리스트</param>
        /// <param name="sData">데이터 리스트</param>
        /// <returns>성공 true / 실패 false</returns>
        public bool PLC_Write_Random(DataType dType, string sAddress, string sData)
        {
            if (bConnected == false) throw new Exception("통신이 연결되어 있지 않습니다.");

            string[] Array_Adr = sAddress.Split(new string[] { "_/" }, StringSplitOptions.None);  //주소 목록
            string[] Array_Data = sData.Split(new string[] { "_/" }, StringSplitOptions.None);  //주소 목록

            if (Array_Adr.Length != Array_Data.Length)
            {
                throw new Exception("주소 수와 데이터 수가 일치하지 않습니다.");
            }
            string Hex_Count = "00" + String.Format("{0:X}", Array_Adr.Length); //총 데이터 쓰기 갯수
            Hex_Count = Hex_Count.Substring(Hex_Count.Length - 2, 2);   //끝 2자리 자름

            string Stream = "";
            for (int i = 0; i < Array_Adr.Length; i++)
            {
                switch (dType)
                {
                    case DataType.Word:
                        Stream = Stream + (Array_Adr[i].Length + 1).ToString("00") + "%" + Array_Adr[i] + (int.Parse(Array_Data[i])).ToString("0000");
                        break;
                    case DataType.Bit:
                        Stream = Stream + (Array_Adr[i].Length + 1).ToString("00") + "%" + Array_Adr[i] + (int.Parse(Array_Data[i])).ToString("00");
                        break;
                }
            }

            string sRCV = "";
            do
            {
                sp232C.ReadExisting();  //버퍼 비우기
                sp232C.Write(ENQ + "00WSS" + Hex_Count + Stream + EOT); //쓰기
                Thread.Sleep(200);  //PLC에서 데이터를 받아오는 동안 기다립니다.
                sRCV = sp232C.ReadExisting();    //데이터 읽어오기
            } while (sRCV == "" || sRCV.Substring(0, 1) != ACK);    //받아온 데이터가 없거나, ACK 명령이 오지 않았으면 다시 시도

            return true;
        }
    }
}
