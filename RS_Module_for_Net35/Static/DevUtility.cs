using System;
using System.Reflection;
using System.Globalization;

namespace RS_Module_for_Net35.Static
{
    /// <summary>
    /// 자주 사용하는 개발 함수들
    /// </summary>
    static public class DevUtility
    {
        /// <summary>
        /// using System.Reflection;
        /// Version Text로부터 Build된 일시를 구합니다.
        /// AssemblyInfo.cs의 빌드 버전을 1.0.*로 변경하세요. 
        /// </summary>
        /// <param name="asm">Assembly.GetExecutingAssembly()를 호출하여 전달하세요</param>
        /// <returns>Build DateTime 빌드 날짜</returns>
        static public DateTime GetBuildTime(Assembly asm)
        {
            Version v = asm.GetName().Version;
            //int majorV = v.Major;   //주버전
            //int minorV = v.Minor;   //부버전
            int buildV = v.Build;   //빌드번호(2000.01.01 부터 Build 된 날짜 까지의 총 일 수)
            int revisionV = v.Revision; //수정번호(자정 0시부터 Build 된 시간까지의 총 시간(Sec))

            //빌드번호는 2000년 1월 1일 부터 Build 된 날짜 까지의 총 일수 이다.
            DateTime dtBuildDate = new DateTime(2000, 1, 1).AddDays(buildV);
            //수정번호는 자정(0시) 부터 Build 된 시간까지 지나간 시간(Sec) 이다.
            dtBuildDate = dtBuildDate.AddSeconds(revisionV * 2);

            //시차조정
            DaylightTime dayLight = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuildDate, dayLight))
            {
                dtBuildDate = dtBuildDate.Add(dayLight.Delta);
            }

            return dtBuildDate;
        }
    }
}