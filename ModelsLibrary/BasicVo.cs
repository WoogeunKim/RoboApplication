using System;

namespace ModelsLibrary
{
    public class BasicVo
    {
        //0이면 풀기 1 이상이면 선택
        public bool isCheckd
        {
            get;
            set;
        }

        //0이면 실패 1 이상이면 성공
        public bool isSuccess
        {
            get;
            set;
        }

        //Error 메시지 전달
        public string Message
        {
            get;
            set;
        }

        public string CRE_USR_ID
        {
            get;
            set;
        }

        public string CRE_USR_NM
        {
            get;
            set;
        }

        public string UPD_USR_ID
        {
            get;
            set;
        }

        public string UPD_USR_NM
        {
            get;
            set;
        }

        public Object CRE_DT
        {
            get;
            set;
        }

        public Object UPD_DT
        {
            get;
            set;
        }

        public string CHNL_CD
        {
            get;
            set;
        }
        public string CHNL_NM
        {
            get;
            set;
        }
        //public string SQL
        //{
        //    get;
        //    set;
        //}
        //public string TB_NM
        //{
        //    get;
        //    set;
        //}
    }
}