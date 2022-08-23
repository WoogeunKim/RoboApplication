namespace ModelsLibrary.Auth
{
    public class ProgramVo : BasicVo
    {
        public string MDL_ID
        {
            get;
            set;
        }
        public string MDL_NM
        {
            get;
            set;
        }
        public string MDL_DESC
        {
            get;
            set;
        }
        public string PRNT_MDL_ID
        {
            get;
            set;
        }
        public string PRNT_MDL_NM
        {
            get;
            set;
        }
        public string PGM_CD 
        {
            get;
            set;
        }
        public string PGM_NM
        {
            get;
            set;
        }
        public string PGM_IMG_NM
        {
            get;
            set;
        }
        public int? SRT_ORD_SEQ
        {
            get;
            set;
        }
        public string SYS_AREA_CD
        {
            get;
            set;
        }
        public string SYS_AREA_NM
        {
            get;
            set;
        }
        public string DELT_FLG
        {
            get;
            set;
        }
        public byte[] IMAGE
        {
            get;
            set;
        }
        public object VIS_FLG
        {
            get;
            set;
        }
        public object UPD_FLG
        {
            get;
            set;
        }
        public string USR_ID
        {
            get;
            set;
        }
        public string GRP_ID
        {
            get;
            set;
        }
        public string CHNL_NM
        {
            get;
            set;
        }
        public byte[] USR_IMG
        {
            get;
            set;
        }
        public byte[] CHNL_LOG_IMG
        {
            get;
            set;
        }

        public byte[] CHNL_SIGN_IMG
        {
            get;
            set;
        }
    }
}