using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS_Module_for_Net35.PLC
{
    /// <summary>
    /// PLC 통신에 사용되는 Enums
    /// </summary>
    public class PLC_Enums
    {
        /// <summary>
        /// 데이터 타입 리스트
        /// </summary>
        public enum PLC_DataTypes
        {
            Unknown,
            BIT,
            BYTE,
            WORD,
            DWORD,
            LWORD,
            ByteBlock
        }

        /// <summary>
        /// PLC 종류
        /// </summary>
        public enum PLC_Types
        {
            MELSEC,     //미쯔비시
            MASTER_K,   //LS 산전
            XGT,        //LS 산전
            Cimon,      //사이몬
            S5_Series,  //지멘스
            S7_Series,  //지멘스
            CJ,         //옴론
        }
    }
}
