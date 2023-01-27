using System.Runtime.Serialization;

namespace ModelsLibrary.Pur
{
    public class PurVo : BasicVo
    {
        //TB_PUR_ESTM_MST
        
        public string PUR_ESTM_NO
        {
            get;
            set;
        }
        
        public string IN_LOC_CD
        {
            get;
            set;
        }
        
        public string IN_LOC_NM
        {
            get;
            set;
        }


        public string[] A_PUR_CO_CD
        {
            get;
            set;
        }

        public string PUR_CO_CD
        {
            get;
            set;
        }
        
        public string PUR_CO_NM
        {
            get;
            set;
        }
        
        public string PUR_ATCH_USR_ID
        {
            get;
            set;
        }
        
        public string PUR_ATCH_USR_NM
        {
            get;
            set;
        }
        
        public string PUR_FM_USR_ID
        {
            get;
            set;
        }
        
        public string PUR_FM_USR_NM
        {
            get;
            set;
        }
        
        public string PUR_ESTM_AVL_NO
        {
            get;
            set;
        }
        
        public string PUR_ESTM_RMK
        {
            get;
            set;
        }


        //TB_PUR_ESTM_DTL
        
        public int? PUR_ESTM_SEQ
        {
            get;
            set;
        }
        
        public string PUR_ESTM_MTRL
        {
            get;
            set;
        }
        
        public string PUR_ITM_SPEC_CTNT
        {
            get;
            set;
        }
        
        public string PUR_ITM_SZ
        {
            get;
            set;
        }
        
        public int? PUR_ESTM_QTY
        {
            get;
            set;
        }
        
        public string PUR_ESTM_UOM_CD
        {
            get;
            set;
        }
        
        public string PUR_ESTM_UOM_NM
        {
            get;
            set;
        }

        //
        
        public string FM_DT
        {
            get;
            set;
        }
        
        public string TO_DT
        {
            get;
            set;
        }
        
        public string PUR_SJT
        {
            get;
            set;
        }
        
        public string PUR_CNT
        {
            get;
            set;
        }
        
        public string PUR_MAST_REMARK
        {
            get;
            set;
        }
        
        public int? RN
        {
            get;
            set;
        }


        //TB_PUR_ORD_MST
        
        public string PUR_ORD_NO
        {
            get;
            set;
        }
        
        public string JB_NO
        {
            get;
            set;
        }
        
        public string JB_NM
        {
            get;
            set;
        }
        //  //PUR_CO_CD
        //public string CO_CD 
        //{
        //    get;
        //    set;
        //}
        
        public string AVL_REGI_NO
        {
            get;
            set;
        }
        
        public string DE_PLC_NM
        {
            get;
            set;
        }
        
        public string DE_PLC_ADDR
        {
            get;
            set;
        }
        
        public string DE_COND_CD
        {
            get;
            set;
        }
        
        public string DE_COND_NM
        {
            get;
            set;
        }
        
        public string MKR_USR_ID
        {
            get;
            set;
        }
        
        public string MKR_USR_NM
        {
            get;
            set;
        }
        
        public string N1ST_RVW_USR_ID
        {
            get;
            set;
        }
        
        public string N1ST_RVW_USR_NM
        {
            get;
            set;
        }
        
        public string N2ND_RVW_USR_ID
        {
            get;
            set;
        }
        
        public string N2ND_RVW_USR_NM
        {
            get;
            set;
        }
        
        public string APRO_USR_ID
        {
            get;
            set;
        }
        
        public string APRO_USR_NM
        {
            get;
            set;
        }
        
        public string PUR_ORD_RMK
        {
            get;
            set;
        }
        
        public string PS_NO
        {
            get;
            set;
        }

        
        public string PR_NO
        {
            get;
            set;
        }


        //TB_PUR_ORD_DTL
        
        public int? PUR_ORD_SEQ
        {
            get;
            set;
        }
        
        public string PUR_RQST_NO
        {
            get;
            set;
        }
        
        public int? PUR_RQST_SEQ
        {
            get;
            set;
        }
        
        public string ITM_SPEC_CTNT
        {
            get;
            set;
        }
        
        public string ITM_SZ
        {
            get;
            set;
        }
        
        public string PUR_THICK_SZ_CTNT
        {
            get;
            set;
        }
        
        public string PUR_WDT_SZ_CTNT
        {
            get;
            set;
        }
        
        public string PUR_LEN_SZ_CTNT
        {
            get;
            set;
        }
        
        public string PUR_HGT_SZ_CTNT
        {
            get;
            set;
        }
        
        public string PUR_UOM_CD
        {
            get;
            set;
        }
        
        public string PUR_UOM_NM
        {
            get;
            set;
        }
        
        public object PUR_UT_PRC
        {
            get;
            set;
        }
        
        public object PUR_QTY
        {
            get;
            set;
        }
        
        public object PUR_AMT
        {
            get;
            set;
        }
        
        public object SL_ITM_TAX_AMT
        {
            get;
            set;
        }
        
        public string DE_DUE_DT
        {
            get;
            set;
        }
        
        public string PUR_DT
        {
            get;
            set;
        }


        
        public string CO_ADDR
        {
            get;
            set;
        }
        
        public string DWG_NO
        {
            get;
            set;
        }
        
        public string DWG_NM
        {
            get;
            set;
        }
        
        public string PRT_NO
        {
            get;
            set;
        }
        
        public string PRT_NM
        {
            get;
            set;
        }
        
        public string MTRL_NM
        {
            get;
            set;
        }


        //P4401 - MST
        
        public string ITM_GRP_CLSS_CD
        {
            get;
            set;
        }
        
        public string ITM_GRP_CLSS_NM
        {
            get;
            set;
        }
        
        public string N1ST_ITM_GRP_NM
        {
            get;
            set;
        }
        public string N1ST_ITM_GRP_CD
        {
            get;
            set;
        }

        public string N2ND_ITM_GRP_NM
        {
            get;
            set;
        }
        
        public string ITM_CD
        {
            get;
            set;
        }
        
        public string CO_NM
        {
            get;
            set;
        }

        public string CO_RGST_NO
        {
            get;
            set;
        }

        public string PRSD_NM
        {
            get;
            set;
        }

        public string HDQTR_PST_NO
        {
            get;
            set;
        }

        public string HDQTR_ADDR
        {
            get;
            set;
        }

        public string ITM_NM
        {
            get;
            set;
        }
        
        public string ITM_SZ_NM
        {
            get;
            set;
        }
        
        public string CNG_APLY_DT
        {
            get;
            set;
        }
        
        public string UOM_NM
        {
            get;
            set;
        }
        
        public string UOM_CD
        {
            get;
            set;
        }
        
        public object CO_UT_PRC
        {
            get;
            set;
        }
        
        public string CURR_NM
        {
            get;
            set;
        }

        public string CAR_ITM_NM
        {
            get;
            set;
        }

        //P4401 - DTL

        public string SL_YR
        {
            get;
            set;
        }
        
        public string CO_NO
        {
            get;
            set;
        }
        //
        //public string CO_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string CNG_APLY_DT
        //{
        //    get;
        //    set;
        //}
        
        public string CURR_CD
        {
            get;
            set;
        }
        //
        //public string CURR_NM
        //{
        //    get;
        //    set;
        //}
        
        public object MN_CO_FLG
        {
            get;
            set;
        }
        
        public object CRNT_PRC_FLG
        {
            get;
            set;
        }
        
        public string CNG_HIS_DESC
        {
            get;
            set;
        }
        
        public string CO_ITM_CD
        {
            get;
            set;
        }
        
        public string CO_ITM_NM
        {
            get;
            set;
        }
        //
        //public string ITM_CD 
        //{
        //    get;
        //    set;
        //}
        
        public string ITM_BZTP_FLG
        {
            get;
            set;
        }
        //
        //public string ITM_BZTP_FLG
        //{
        //    get;
        //    set;
        //}
        
        public string LOT_VAL
        {
            get;
            set;
        }




        //P4411 - MST
        public string PUR_EMPE_ID
        {
            get;
            set;
        }
        public string PUR_CLZ_FLG
        {
            get;
            set;
        }
        public string ORD_RMK
        {
            get;
            set;
        }
        public string LST_FMT_NO
        {
            get;
            set;
        }
        
        public string GRP_ID
        {
            get;
            set;
        }
        
        public string PUR_HIST
        {
            get;
            set;
        }
        
        public string USR_NM
        {
            get;
            set;
        }

        //P4411 - DTL
        
        public int? PUR_SEQ
        {
            get;
            set;
        }
        
        public string DUE_DT
        {
            get;
            set;
        }
        //
        //public string ITM_CD
        //{
        //    get;
        //    set;
        //}
        //
        //public string ITM_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string ITM_SZ_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string UOM_CD
        //{
        //    get;
        //    set;
        //}
        //
        //public string UOM_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public double? CO_UT_PRC
        //{
        //    get;
        //    set;
        //}
        //
        //public double? PUR_QTY
        //{
        //    get;
        //    set;
        //}
        //
        //public double? PUR_AMT
        //{
        //    get;
        //    set;
        //}
        
        public string PUR_RMK
        {
            get;
            set;
        }

        
        public string PUR_DEPT_NM
        {
            get;
            set;
        }

        
        public string PUR_EMPE_NM
        {
            get;
            set;
        }


        
        public string IO_CD
        {
            get;
            set;
        }

        
        public string YRMON
        {
            get;
            set;
        }

        
        public string GBN
        {
            get;
            set;
        }



        
        public long? MM_01
        {
            get;
            set;
        }
        
        public long? MM_02
        {
            get;
            set;
        }
        
        public long? MM_03
        {
            get;
            set;
        }
        
        public long? MM_04
        {
            get;
            set;
        }
        
        public long? MM_05
        {
            get;
            set;
        }
        
        public long? MM_06
        {
            get;
            set;
        }
        
        public long? MM_07
        {
            get;
            set;
        }
        
        public long? MM_08
        {
            get;
            set;
        }
        
        public long? MM_09
        {
            get;
            set;
        }
        
        public long? MM_10
        {
            get;
            set;
        }
        
        public long? MM_11
        {
            get;
            set;
        }
        
        public long? MM_12
        {
            get;
            set;
        }
        
        public long? MM_SUM
        {
            get;
            set;
        }

        
        public string QC_CD
        {
            get;
            set;
        }



        //
        
        public string PUR_DT_NM
        {
            get;
            set;
        }
        
        public string PUR_GRP_NM
        {
            get;
            set;
        }
        
        public string DUE_DT_NM
        {
            get;
            set;
        }
        //
        //public int? CO_UT_PRC
        //{
        //    get;
        //    set;
        //}
        //
        //public float? PUR_QTY
        //{
        //    get;
        //    set;
        //}
        //
        //public int? PUR_AMT
        //{
        //    get;
        //    set;
        //}
        
        public object INAUD_QTY
        {
            get;
            set;
        }
        
        public object PUR_RMN_QTY
        {
            get;
            set;
        }
        
        public string CLZ_CD
        {
            get;
            set;
        }
        
        public string HDQTR_PHN_NO
        {
            get;
            set;
        }
        
        public string HDQTR_FAX_NO
        {
            get;
            set;
        }


        //TB_PUR_IMP_CTRT
        
        public string IMP_PUR_NO
        {
            get;
            set;
        }
        
        public string IMP_CTRT_NO
        {
            get;
            set;
        }
        
        public string IMP_CO_CD
        {
            get;
            set;
        }
        
        public string IMP_CO_NM
        {
            get;
            set;
        }
        
        public string IMP_CTRT_DT
        {
            get;
            set;
        }
        
        public string IMP_ITM_CLSS_CD
        {
            get;
            set;
        }
        
        public string IMP_ITM_CLSS_NM
        {
            get;
            set;
        }
        
        public string IMP_BIL_CD
        {
            get;
            set;
        }
        
        public string IMP_BIL_NM
        {
            get;
            set;
        }
        
        public string IMP_SHP_DT
        {
            get;
            set;
        }
        
        public string IMP_INV_DT
        {
            get;
            set;
        }
        
        public string IMP_EXP_DT
        {
            get;
            set;
        }
        
        public string IMP_CTRT_COND_CD
        {
            get;
            set;
        }
        
        public string IMP_CTRT_COND_NM
        {
            get;
            set;
        }
        
        public object IMP_XCH_RT
        {
            get;
            set;
        }
        
        public string IMP_CURR_CD
        {
            get;
            set;
        }
        
        public string IMP_CURR_NM
        {
            get;
            set;
        }
        
        public string IMP_RMK
        {
            get;
            set;
        }





        //TB_PUR_IMP_CTRT_DTL
        
        public int? IMP_PUR_SEQ
        {
            get;
            set;
        }
        
        public string IMP_ITM_CD
        {
            get;
            set;
        }
        //
        //public string ITM_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string ITM_SZ_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string UOM_CD
        //{
        //    get;
        //    set;
        //}
        //
        //public string UOM_NM
        //{
        //    get;
        //    set;
        //}
        
        public object IMP_ITM_QTY
        {
            get;
            set;
        }
        
        public object MD_IMP_ITM_QTY
        {
            get;
            set;
        }
        
        public object IMP_ITM_PRC
        {
            get;
            set;
        }
        
        public object IMP_ITM_AMT
        {
            get;
            set;
        }
        
        public object PK_PER_QTY
        {
            get;
            set;
        }
        
        public object PK_QTY
        {
            get;
            set;
        }
        
        public object MD_IMP_ITM_AMT
        {
            get;
            set;
        }
        
        public string IMP_ITM_RMK
        {
            get;
            set;
        }
        
        public object ITM_SUM_AMT
        {
            get;
            set;
        }
        
        public object RMN_AMT
        {
            get;
            set;
        }

        //TB_PUR_IMP_INV
        
        public string IMP_INV_NO
        {
            get;
            set;
        }
        
        public string CO_INV_NO
        {
            get;
            set;
        }
        //
        //public string IMP_SHP_DT
        //{
        //    get;
        //    set;
        //}
        
        public string IMP_ARR_PORT_CD
        {
            get;
            set;
        }
        
        public string IMP_ARR_PORT_NM
        {
            get;
            set;
        }
        //
        //public string IMP_CURR_NM
        //{
        //    get;
        //    set;
        //}
        
        public string IMP_PORT_ENTR_DT
        {
            get;
            set;
        }
        
        public string IMP_LC_NO
        {
            get;
            set;
        }
        
        public string IMP_LC_OPN_DT
        {
            get;
            set;
        }
        
        public string IMP_OPN_BANK_CD
        {
            get;
            set;
        }
        
        public string IMP_OPN_BANK_NM
        {
            get;
            set;
        }
        
        public string BANK_ACCT_NO
        {
            get;
            set;
        }
        
        public string IMP_INSUR_DT
        {
            get;
            set;
        }
        
        public string IMP_BL_NO
        {
            get;
            set;
        }
        
        public string IMP_DTY_CO_CD
        {
            get;
            set;
        }
        //
        //public string IMP_CURR_CD
        //{
        //    get;
        //    set;
        //}
        //
        //public string IMP_XCH_RT
        //{
        //    get;
        //    set;
        //}
        
        public string IMP_CLR_DT
        {
            get;
            set;
        }
        
        public string IMP_DTY_CO_NM
        {
            get;
            set;
        }
        
        public string IMP_INV_RMK
        {
            get;
            set;
        }
        //
        //public string ITM_SUM_AMT
        //{
        //    get;
        //    set;
        //}
        //
        //public string IMP_CO_NM
        //{
        //    get;
        //    set;
        //}






        //TB_PUR_IMP_INV_DTL
        //
        //public string IMP_CTRT_NO
        //{
        //    get;
        //    set;
        //}
        //
        //public string IMP_ITM_CD
        //{
        //    get;
        //    set;
        //}
        //
        //public string ITM_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string ITM_SZ_NM
        //{
        //    get;
        //    set;
        //}
        //
        //public string UOM_CD
        //{
        //    get;
        //    set;
        //}
        //
        //public string UOM_NM
        //{
        //    get;
        //    set;
        //}
        
        public object IMP_CTRT_QTY
        {
            get;
            set;
        }
        //
        //public object IMP_ITM_QTY
        //{
        //    get;
        //    set;
        //}
        //
        //public object IMP_ITM_PRC
        //{
        //    get;
        //    set;
        //}
        //
        //public object IMP_ITM_AMT
        //{
        //    get;
        //    set;
        //}
        //
        //public string IMP_ITM_RMK
        //{
        //    get;
        //    set;
        //}
        
        public object CTRT_RMN_QTY
        {
            get;
            set;
        }
        
        public object MD_CTRT_RMN_QTY
        {
            get;
            set;
        }
        //
        //public string IMP_INV_NO
        //{
        //    get;
        //    set;
        //}
        
        public object IMP_INV_SEQ
        {
            get;
            set;
        }
        //
        //public string IMP_PUR_NO
        //{
        //    get;
        //    set;
        //}
        //
        //public int? IMP_PUR_SEQ
        //{
        //    get;
        //    set;
        //}
        
        public object IMP_INV_QTY
        {
            get;
            set;
        }
        
        public object IMP_ITM_M_PRC
        {
            get;
            set;
        }
        
        public object CUR_XCH_AMT
        {
            get;
            set;
        }
        
        public object CUR_APLY_XCH_AMT
        {
            get;
            set;
        }
        //
        //public object IMP_ITM_PRC
        //{
        //    get;
        //    set;
        //}
        //
        //public object IMP_ITM_AMT
        //{
        //    get;
        //    set;
        //}






        
        public string CTRT_NM
        {
            get;
            set;
        }
        
        public string PUR_ITM_CD
        {
            get;
            set;
        }
        
        public string PUR_ITM_NM
        {
            get;
            set;
        }
        
        public string PUR_DUE_DT
        {
            get;
            set;
        }
        
        public string AREA_CD
        {
            get;
            set;
        }
        
        public string AREA_NM
        {
            get;
            set;
        }
        
        public object PUR_SUM_AMT
        {
            get;
            set;
        }

        
        public string PUR_ITM_RMK
        {
            get;
            set;
        }




        //
        
        public object MD_PER_QTY
        {
            get;
            set;
        }
        
        public object STK_QTY
        {
            get;
            set;
        }
        
        public object SL_AVG_QTY
        {
            get;
            set;
        }
        
        public object SFTK_QTY
        {
            get;
            set;
        }
        
        public object MON_QTY
        {
            get;
            set;
        }
        
        public object SLCK_QTY
        {
            get;
            set;
        }
        
        public object IN_QTY
        {
            get;
            set;
        }
        
        public object IN_RTE
        {
            get;
            set;
        }
        
        public object IN_PRC
        {
            get;
            set;
        }
        
        public object IN_AMT
        {
            get;
            set;
        }
        
        public object INV_RTE
        {
            get;
            set;
        }
        
        public object MD_QTY
        {
            get;
            set;
        }
        
        public object PRV_IN_QTY
        {
            get;
            set;
        }
        //
        //public object IN_QTY
        //{
        //    get;
        //    set;
        //}
        
        public object RMN_QTY
        {
            get;
            set;
        }
        
        public object PUR_PRC
        {
            get;
            set;
        }
        
        public object RMN_PRC
        {
            get;
            set;
        }
        //
        //public object PUR_AMT
        //{
        //    get;
        //    set;
        //}
        
        public string MN_ITM_FLG
        {
            get;
            set;
        }


        //
        //
        
        public string PUR_FM_DT
        {
            get;
            set;
        }
        
        public string PUR_TO_DT
        {
            get;
            set;
        }
        
        public string AVG_FM_DT
        {
            get;
            set;
        }
        
        public string AVG_TO_DT
        {
            get;
            set;
        }
        
        public object STK_MON
        {
            get;
            set;
        }
        //
        //public object IN_RTE
        //{
        //    get;
        //    set;
        //}
        
        public object TGT_RTE
        {
            get;
            set;
        }
        
        public string CLZ_FLG
        {
            get;
            set;
        }
        
        public string PUR_CLZ_YRMON
        {
            get;
            set;
        }
        
        public string CO_TP_CD
        {
            get;
            set;
        }
        
        public string SL_ADJ_CD
        {
            get;
            set;
        }
        
        public string SL_ADJ_NM
        {
            get;
            set;
        }
        
        public string SL_ADJ_RESON_CD
        {
            get;
            set;
        }
        
        public string SL_ADJ_RESON_NM
        {
            get;
            set;
        }
        
        public object SL_ITM_QTY
        {
            get;
            set;
        }
        
        public object SL_ITM_PRC
        {
            get;
            set;
        }
        
        public object SL_ITM_AMT
        {
            get;
            set;
        }
        
        public object IMP_XCH_APLY_RT
        {
            get;
            set;
        }
        
        public string PRN_DT
        {
            get;
            set;
        }
        
        public string INAUD_RMK
        {
            get;
            set;
        }
        
        public string INAUD_DT
        {
            get;
            set;
        }

        
        public string IMP_XCH_APLY_DT
        {
            get;
            set;
        }

        
        public string IMP_XCH_APLY_FLG
        {
            get;
            set;
        }

        
        public object IMP_ITM_XCH_PRC
        {
            get;
            set;
        }
        
        public string INSRL_NO
        {
            get;
            set;
        }
        
        public object INIT_AMT
        {
            get;
            set;
        }
        
        public object CLT_AMT
        {
            get;
            set;
        }
        
        public object CLT_TAX_AMT
        {
            get;
            set;
        }
        
        public object IMP_ITM_XCH_AMT
        {
            get;
            set;
        }
        
        public string IN_NO
        {
            get;
            set;
        }

        
        public string IMP_XCH_APLY_RMK
        {
            get;
            set;
        }

        
        public string IMP_XCH_APLY_CRE_DT
        {
            get;
            set;
        }

        
        public string IMP_XCH_APLY_UPD_DT
        {
            get;
            set;
        }
        public int? PUR_WK_CD
        {
            get;
            set;
        }


        public object R_MM_01
        {
            get;
            set;
        }
        public object R_MM_02
        {
            get;
            set;
        }

        public object R_MM_03
        {
            get;
            set;
        }

        public object R_MM_04
        {
            get;
            set;
        }

        public object R_MM_05
        {
            get;
            set;
        }

        public object R_MM_06
        {
            get;
            set;
        }

        public object R_MM_07
        {
            get;
            set;
        }

        public object R_MM_08
        {
            get;
            set;
        }

        public object R_MM_09
        {
            get;
            set;
        }

        public object R_MM_10
        {
            get;
            set;
        }

        public object R_MM_11
        {
            get;
            set;
        }


        public object isCheckd_Del
        {
            get;
            set;
        }

        
        public object IMP_XCH_APLY_PRC
        {
            get;
            set;
        }

        public object N1ST_QTY
        {
            get;
            set;
        }
        public object N2ND_QTY
        {
            get;
            set;
        }
        public object N3RD_QTY
        {
            get;
            set;
        }
        public object N4TH_QTY
        {
            get;
            set;
        }
        public object N5TH_QTY
        {
            get;
            set;
        }

        public string SO_DT
        {
            get;
            set;
        }

        public string SL_ORD_NO
        {
            get;
            set;
        }

        public int? SL_ORD_SEQ
        {
            get;
            set;
        }
        public string ORD_ITM_CD
        {
            get;
            set;
        }


        public int? ASSY_SEQ
        {
            get;
            set;
        }
        public string CMPO_CD
        {
            get;
            set;
        }
        public object INP_QTY
        {
            get;
            set;
        }
        public string UN_FOL_NO
        {
            get;
            set;
        }
        public string[] A_UN_FOL_NO
        {
            get;
            set;
        }

        public object WEIH_VAL
        {
            get;
            set;
        }
        public object LSS_VAL
        {
            get;
            set;
        }
        public object TOT_USE_QTY
        {
            get;
            set;
        }
        public int? MRP_SEQ
        {
            get;
            set;
        }
        public string CO_CD
        {
            get;
            set;
        }
        public object PRE_OCPY_QTY
        {
            get;
            set;
        }
        public object MGR_ITM_QTY
        {
            get;
            set;
        }
        public object REQ_ORD_QTY
        {
            get;
            set;
        }

        public object ITM_USE_QTY
        {
            get;
            set;
        }

        public object ITM_PRC
        {
            get;
            set;
        }
        public string IN_REQ_DT
        {
            get;
            set;
        }
        public string MRP_DESC
        {
            get;
            set;
        }
        public string SCM_OK_FLG
        {
            get;
            set;
        }
        public string ORD_NO
        {
            get;
            set;
        }

        public int? ORD_SEQ
        {
            get;
            set;
        }

        public object SPLY_AMT
        {
            get;
            set;
        }
        public object VAT_AMT
        {
            get;
            set;
        }

        public object TOT_AMT
        {
            get;
            set;
        }

        public string ROUT_CD
        {
            get;
            set;
        }
        public string ROUT_NM
        {
            get;
            set;
        }


        public string PRNT_ROUT_CD
        {
            get;
            set;
        }
        public string PRNT_ROUT_NM
        {
            get;
            set;
        }


        public string SL_PLN_NO
        {
            get;
            set;
        }
        public string SL_PLN_YRMON
        {
            get;
            set;
        }
        public string SL_CO_CD
        {
            get;
            set;
        }

        public string STK_SER_NO
        {
            get;
            set;
        }

        public string GRP_SER_NO
        {
            get;
            set;
        }

        public string COLR_CD
        {
            get;
            set;
        }
        public string COLR_NM
        {
            get;
            set;
        }
        public string CNTC_MAN_EML
        {
            get;
            set;
        }


        ///////// 11/05고객발주관리 마스터 추가했음 //////

        public string PUR_NO
        {
            get;
            set;
        }

        public string DELT_FLG
        {
            get;
            set;
        }

        //////// 11/08 고객발주관리 디테일 ///////////////

        public string DE_CO_NM
        {
            get;
            set;
        }

        public string CNTR_NM
        {
            get;
            set;
        }

        public string CNTR_PSN_NM
        {
            get;
            set;
        }

        public string FLR_NM
        {
            get;
            set;
        }

        public string FLR_NO
        {
            get;
            set;
        }

        public object PUR_WGT
        {
            get;
            set;
        }

        public byte[] FLR_FILE
        {
            get;
            set;
        }

        public string FLR_FILE_ID
        {
            get;
            set;
        }

        public string ITM_LEN_NM
        {
            get;
            set;
        }

        public object BUN_WGT
        {
            get;
            set;
        }
        public int? ROW_NUMBER
        {
            get;
            set;
        }
        public string CO_TP
        {
            get;
            set;
        }
        public string APLY_PRC_DT
        {
            get;
            set;
        }
        public byte[] PRC_FILE
        {
            get;
            set;
        }
        public string N2ND_ITM_GRP_CD
        {
            get;
            set;
        }

        public int? BSS_WGT
        {
            get;
            set;
        }
        public int? PPR_KNT_PER_WGT
        {
            get;
            set;
        }
        public object WGT_PER_PRC
        {
            get;
            set;
        }
        public object ROLL_PER_PRC
        {
            get;
            set;
        }
        public string PAY_TP_CD
        {
            get;
            set;
        }

        // 2022-12-03 
        public object PUR_ITM_QTY
        {
            get;
            set;
        }
        public object DC_RT
        {
            get;
            set;
        }
        public object ITM_GRP_NM
        {
            get;
            set;
        }
        public object ITM_GRP_CD
        {
            get;
            set;
        }

        public string SL_RLSE_NO
        {
            get;
            set;
        }

        public int? SL_RLSE_SEQ
        {
            get;
            set;
        }

        public string SL_RLSE_DT
        {
            get;
            set;
        }

        public string SL_ITM_CD
        {
            get;
            set;
        }

        public string N1ST_ITM_SZ_NM
        {
            get;
            set;
        }
        public string N2ND_ITM_SZ_NM
        {
            get;
            set;
        }

        public object TOT_QTY
        {
            get;
            set;
        }
        public string PRG_TP_CD
        {
            get;
            set;
        }
        public string PRG_TP_NM
        {
            get;
            set;
        }
    }
}