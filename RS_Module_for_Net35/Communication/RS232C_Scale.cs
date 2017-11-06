using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RS_Module_for_Net35.Communication
{
    /// <summary>
    /// COM Port. RS-232C 통신을 이용한 저울 연결.
    /// </summary>
    public class RS232C_Scale
    {
        /// <summary>
        /// 데이터를 한번이라도 제대로 받은적이 있으면 true가 됩니다.
        /// </summary>
        public bool Connected = false;

        /// <summary>
        /// 계산된 저울값을 가져오거나 설정합니다.
        /// </summary>
        public double Value = 0;

        /// <summary>
        /// 마지막으로 발생된 예외 오류
        /// </summary>
        public Exception LastExc = new Exception();

        /// <summary>
        /// 시리얼포트 객체
        /// </summary>
        public SerialPort sp232C = new SerialPort();

        /// <summary>
        /// delegate용 메서드
        /// </summary>
        /// <param name="sData"></param>
        /// <returns></returns>
        public delegate double CommDelegate(string sData);

        /// <summary>
        /// 데이터 스트림을 누적시킬 최대 길이를 가져오거나 설정합니다.
        /// 기본값 = 0
        /// 데이터 스트림이 100자리가되기 전에는 메서드를 호출하지 않습니다.
        /// </summary>
        public int BufferSize = 0;

        /// <summary>
        /// 통신으로 넘어온 Stream을 파싱하여 double형태로 만들어주는 함수를 연결해야 합니다.
        /// 파싱에 실패 할 경우 예외를 throw 하십시오.
        /// ex : Parsing = new CommDelegate(메서드 명)
        /// </summary>
        /// <param name="sData">string 형태의 DataStream이 넘어갑니다.</param>
        /// <returns>double값을 넘겨야합니다.</returns>
        public CommDelegate Parsing;

        /// <summary>
        /// 저울 통신 시작. Delegate Parsing를 먼저 선언하십시오.
        /// </summary>
        /// <param name="pPortName">(ex:COM1)</param>
        /// <param name="pBaudRate">(ex:9600)</param>
        /// <param name="pParity">(ex:Parity.None)</param>
        /// <param name="pDataBits">(ex:8)</param>
        /// <param name="pStopBits">(ex:StopBits.One)</param>
        /// <returns>true or false</returns>
        public bool Open(string pPortName, int pBaudRate, Parity pParity, int pDataBits, StopBits pStopBits)
        {
            if (Parsing == null)
            {
                System.Windows.Forms.MessageBox.Show("RS-232C 통신을 위한 Parsing Delegate를 먼저 선언 하십시오.");
                return false;
            }

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
                sp232C.DataReceived += RS232C_DataReceivedEvent;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 통신 종료
        /// </summary>
        public void Close()
        {
            if (sp232C.IsOpen == true)
                sp232C.Close();
        }

        /// <summary>
        /// 데이터 스트림이 누적되고 있는 버퍼를 가져오거나 설정합니다.
        /// </summary>
        public string DataStreamBuffer = "";

        /// <summary>
        /// 데이터 이벤트 처리
        /// </summary>
        private void RS232C_DataReceivedEvent(object sender, SerialDataReceivedEventArgs e)
        {
            DataStreamBuffer += sp232C.ReadExisting();

            //데이터 버퍼를 모아서 파싱함.
            if (DataStreamBuffer.Length < BufferSize)
                return;

            try
            {
                double TempV = Parsing(DataStreamBuffer);
                Value = TempV;
            }
            catch (Exception ex)
            {
                LastExc = ex;
            }
            DataStreamBuffer = "";
            LastExc = new Exception("Success! Value = " + Value.ToString());
            Connected = true;
        }
    }
}
