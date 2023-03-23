using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Mobile
{
    public class MobileVo : BasicVo
    {
        public string USR_ID { get; set; }
        public string USR_PWD { get; set; }
        public string DELT_FLG { get; set; }
        public string USR_NM { get; set; }
        public string VER_NM { get; set; }
        public string FL_NM { get; set; }
        public string ST_NM { get; set; }
        public string CO_NO { get; set; }
        public string CO_NM { get; set; }
        public int? RN { get; set; }
        public string PUR_ORD_NO { get; set; }
        public int? PUR_ORD_SEQ { get; set; }
        public string N1ST_ITM_GRP_NM { get; set; }
        public string N2ND_ITM_GRP_NM { get; set; }
        public string ITM_CD { get; set; }
        public string ITM_NM { get; set; }
        public string ITM_SZ_NM { get; set; }
        public string UOM_NM { get; set; }
        public object TMP_RMK_QTY { get; set; }
        public object PUR_QTY { get; set; }
        public object ITM_MD_QTY { get; set; }
        public object RMN_QTY { get; set; }
        public object CO_UT_PRC { get; set; }
        public object PUR_AMT { get; set; }
        public string PUR_RMK { get; set; }
        public object ITM_QTY { get; set; }
        public string INAUD_ORG_NO { get; set; }
        public string INAUD_ORG_NM { get; set; }
        public string PUR_DUE_DT { get; set; }
        public string AREA_CD { get; set; }
        public string LOT_NO { get; set; }
        public string INAUD_DT { get; set; }
    }
}
