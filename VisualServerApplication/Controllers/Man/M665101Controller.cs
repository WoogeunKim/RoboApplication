using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m665101")]
    public class M665101Controller : ApiController
    {
        #region MyRegion
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        /// <summary>
        ///주간 생산계획 관리 - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M665101SelectMaster", vo));
        }

        /// <summary>
        ///주간 생산계획 팝업 관리 - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M665101SelectPopup", vo));
        }

        /// <summary>
        /// 주간 계획 관리  - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] ManVo vo)
        //public async Task<IHttpActionResult> GetMstInsert([FromBody]IEnumerable<ManVo> voList)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();
                //foreach (ManVo item in voList)
                //{
                //    Properties.EntityMapper.Insert("M665100InsertMst", item);
                //}
                //Properties.EntityMapper.CommitTransaction();
                //return Ok<int?>(1);

                return Ok<int>(Properties.EntityMapper.Insert("M665101InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주간 계획 관리  - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                vo.PROD_WKY_PLN_NO =  Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnNo", vo);

                return Ok<int>(Properties.EntityMapper.Update("M665101UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주간 계획 관리 - 삭제
        /// </summary>
        [Route("d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                vo.PROD_WKY_PLN_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnNo", vo);

                return Ok<int>(Properties.EntityMapper.Delete("M665101DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 작업지시서 [사출] 발행 - 추가
        /// </summary>
        [Route("wrk")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetWrkInsert([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                voList[0].PROD_WKY_PLN_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnNo", voList[0]);
                voList[0].PROD_ORD_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdOrdNo", voList[0]);
                voList[0].ROUT_ITM_CD = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdCmpoCd", voList[0]);
                voList[0].SL_ORD_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnPurNo", voList[0]);
                voList[0].SL_ORD_SEQ = Convert.ToInt16(Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnPurSeq", voList[0]));

                //
                ManVo _tmpItmVo = Properties.EntityMapper.QueryForObject<ManVo>("M665101SelectProdItm", voList[0]);
                voList[0].ROUT_ITM_CD = _tmpItmVo.ROUT_ITM_CD;
                voList[0].ITM_CD = _tmpItmVo.ITM_CD;
                voList[0].ASSY_CD = _tmpItmVo.ITM_CD;


                ManVo _keyMstVo = Properties.EntityMapper.QueryForObject<ManVo>("M665101SelectWrkTmp", voList[0]);

                //MST
                voList[0].LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                Properties.EntityMapper.Insert("M665101InsertWrk", voList[0]);

                //투입 자재
                Properties.EntityMapper.Insert("M665101InsertItm", voList[0]);

                string _rout_cd = string.Empty;
                ManVo _keyDtlVo = new ManVo();
                foreach (ManVo item in voList)
                {
                    //if (!item.ROUT_CD.Equals(_rout_cd))
                    //{
                    _rout_cd = item.ROUT_CD;
                    _keyDtlVo = Properties.EntityMapper.QueryForObject<ManVo>("M665101SelectWrkDtlTmp", voList[0]);
                    item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                    item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;

                    Properties.EntityMapper.Insert("M665101InsertWrkDtl", item);
                    //}

                    //item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                    //item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                    //Properties.EntityMapper.Insert("M66540InsertDtlItm", item);
                }

                Properties.EntityMapper.CommitTransaction();

                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M665101SelectReport", voList[0]));
                //return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


    }
}