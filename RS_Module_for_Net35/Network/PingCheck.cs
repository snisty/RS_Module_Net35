/// <summary>
/// 2017-09-19 최민현.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;

namespace RS_Module_for_Net35.Network
{
    /// <summary>
    /// ping 테스트를 통하여, 응답시간이 설정시간을 초과하면 기록합니다.
    /// 객체 생성 후 Start()를 통해 백그라운드로 작업됩니다.
    /// 1) HostIP 설정. (Default : www.google.com)
    /// 2) FilePath 설정. (Default : C:\\RS_Log\\)
    /// 3) FileName 설정. (Default : PingCheck)
    /// 4) Max_Write_ms 설정. (Default : 100)
    /// 5) Start();
    /// </summary>
    public class PingCheck
    {
        /// <summary>
        /// 테스트 할 서버를 가져오거나, 설정합니다.
        /// Default : "www.google.com"
        /// </summary>
        public string HostIP = "www.google.com";
        /// <summary>
        /// Log가 출력될 경로를 가져오거나, 설정합니다.
        /// Default : "\\RS_Log\\"
        /// </summary>
        public string FilePath = "C:\\RS_Log\\";
        /// <summary>
        /// Log 파일 이름을 가져오거나, 설정합니다.
        /// Default : "PingCheck"
        /// </summary>
        public string FileName = "PingCheck";
        /// <summary>
        /// Log 기록 할 응답시간을 가져오거나, 설정합니다.
        /// Default : 100ms
        /// ping에 대한 응답이 100ms를 초과할 시 Log에 기록합니다.
        /// </summary>
        public int Max_Write_ms = 100;
        /// <summary>
        /// 백그라운드 작업되는 스레드 입니다.
        /// </summary>
        private Thread thNetCheck;
        /// <summary>
        /// 검사 시작
        /// </summary>
        public void Start()
        {
            if (thNetCheck != null)
                return;

            DirectoryCheck(FilePath);

            string FullPath = FilePath + "PingCheck" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            thNetCheck = new Thread(new ParameterizedThreadStart(fnNetCheck))
            {
                IsBackground = true
            };
            thNetCheck.Start(FullPath);
        }
        /// <summary>
        /// 검사 종료
        /// </summary>
        public void End()
        {
            thNetCheck.Abort();
        }
        /// <summary>
        /// 경로가 존재하지 않으면 생성합니다.
        /// </summary>
        /// <param name="sPath">파일 경로</param>
        private void DirectoryCheck(string sPath)
        {
            DirectoryInfo DirInfo = new DirectoryInfo(sPath);
            if (DirInfo.Exists == false)
            {
                DirInfo.Create();
            }
        }
        /// <summary>
        /// 실 작업 함수
        /// </summary>
        private void fnNetCheck(object param)
        {
            string FullPath = param.ToString();

            Ping netMon = new Ping();
            while (true)
            {
                Thread.Sleep(100);
                try
                {
                    string LogMsg = DateTime.Now + '\t'.ToString();
                    PingReply prply = netMon.Send(HostIP);
                    if (prply.Status == IPStatus.Success)
                    {
                        LogMsg += prply.RoundtripTime.ToString() + "ms";
                        if (prply.RoundtripTime > Max_Write_ms)
                        {
                            WriteText(FullPath, LogMsg);
                        }
                    }
                    else
                    {
                        LogMsg += "Connected Fail";
                        WriteText(FullPath, LogMsg);
                    }
                }
                catch
                {
                    WriteText(FullPath, Text: "Expection");
                }
            }
        }
        /// <summary>
        /// 파일에 글을 씁니다.
        /// </summary>
        /// <param name="FullPath">파일 경로 및 이름</param>
        /// <param name="Text">텍스트</param>
        private void WriteText(string FullPath, string Text)
        {            
            using (StreamWriter fp = new StreamWriter(FullPath, true))
            {
                fp.WriteLine(Text);
            }
        }
    }
}
