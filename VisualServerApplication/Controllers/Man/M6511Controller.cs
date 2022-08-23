using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6511")]
    public class M6511Controller : ApiController
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
        /// 조립 작업지시등록 MST  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6511SelectMst", vo));
        }




        /// <summary>
        /// 조립 작업지시등록 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6511SelectDtl", vo));
        }


        /// <summary>
        /// 조립 작업지시등록  - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();


                vo.LOT_DIV_NO = Properties.EntityMapper.QueryForObject<string>("M6511SelectLotDivNo", vo);

                //작지 발행
                Properties.EntityMapper.Insert("M6511InsertDtl", vo);

                //작지 발행 업데이트
                Properties.EntityMapper.Update("M6511UpdateMst", vo);
                //작지 실적 생성
                //Properties.EntityMapper.Insert("M6511InsertDtlNew", vo);
                //작지 투입 자재
                //Properties.EntityMapper.Insert("M6511InsertDtlItm", vo);


                Properties.EntityMapper.CommitTransaction();

                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6500InsertDtl", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업지시등록 - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6511UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업지시등록 - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6511DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 작업지시서 [조립] 발행 - 추가
        /// </summary>
        [Route("report")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetWrkReport([FromBody] ManVo vo)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();

                //voList[0].PROD_WKY_PLN_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnNo", voList[0]);
                //voList[0].PROD_ORD_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdOrdNo", voList[0]);
                //voList[0].ROUT_ITM_CD = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdCmpoCd", voList[0]);
                //voList[0].SL_ORD_NO = Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnPurNo", voList[0]);
                //voList[0].SL_ORD_SEQ = Convert.ToInt16(Properties.EntityMapper.QueryForObject<string>("M665101SelectProdWkyPlnPurSeq", voList[0]));

                ////
                //ManVo _tmpItmVo = Properties.EntityMapper.QueryForObject<ManVo>("M665101SelectProdItm", voList[0]);
                //voList[0].ROUT_ITM_CD = _tmpItmVo.ROUT_ITM_CD;
                //voList[0].ITM_CD = _tmpItmVo.ITM_CD;
                //voList[0].ASSY_CD = _tmpItmVo.ITM_CD;


                //ManVo _keyMstVo = Properties.EntityMapper.QueryForObject<ManVo>("M665101SelectWrkTmp", voList[0]);

                ////MST
                //voList[0].LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                //Properties.EntityMapper.Insert("M665101InsertWrk", voList[0]);

                ////투입 자재
                //Properties.EntityMapper.Insert("M665101InsertItm", voList[0]);

                //string _rout_cd = string.Empty;
                //ManVo _keyDtlVo = new ManVo();
                //foreach (ManVo item in voList)
                //{
                //    //if (!item.ROUT_CD.Equals(_rout_cd))
                //    //{
                //    _rout_cd = item.ROUT_CD;
                //    _keyDtlVo = Properties.EntityMapper.QueryForObject<ManVo>("M665101SelectWrkDtlTmp", voList[0]);
                //    item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                //    item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;

                //    Properties.EntityMapper.Insert("M665101InsertWrkDtl", item);
                //    //}

                //    //item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                //    //item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                //    //Properties.EntityMapper.Insert("M66540InsertDtlItm", item);
                //}

                //Properties.EntityMapper.CommitTransaction();

                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6511SelectReport", vo));
                //return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }
    }
}