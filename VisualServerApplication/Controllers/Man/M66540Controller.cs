using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ModelsLibrary.Sale;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66540")]
    public class M66540Controller : ApiController
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
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66540SelectMaster", vo));
        }

        /// <summary>
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66540SelectDetail", vo));
        }

        /// <summary>
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("dtl/two")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl_TwoSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66540SelectDetail_two", vo));
        }

        /// <summary>
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66540SelectPopup", vo));
        }



        /// <summary>
        /// 작업지시서 [사출] 발행 - 추가
        /// </summary>
        [Route("mst/tmp1/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetTmp1Insert([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M66540InsertTmp1", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 작업지시서 [사출] 발행 - 추가
        /// </summary>
        [Route("mst/tmp2/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetTmp2nsert([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M66540InsertTmp2", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 작업지시서 [사출] 발행 - 추가
        /// </summary>
        [Route("mst/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                ManVo _keyMstVo = Properties.EntityMapper.QueryForObject<ManVo>("M66540SelectMstTmp", voList[0]);

                //MST
                voList[0].LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                Properties.EntityMapper.Insert("M66540InsertMst", voList[0]);

                string _rout_cd = string.Empty;
                ManVo _keyDtlVo = new ManVo();
                foreach (ManVo item in voList)
                {
                    //if (!item.ROUT_CD.Equals(_rout_cd))
                    //{
                        _rout_cd = item.ROUT_CD;
                        _keyDtlVo = Properties.EntityMapper.QueryForObject<ManVo>("M66540SelectDtlTmp", voList[0]);
                        item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                        item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;

                        Properties.EntityMapper.Insert("M66540InsertDtl", item);
                    //}

                    item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                    item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                    Properties.EntityMapper.Insert("M66540InsertDtlItm", item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 작업지시서 발행 - 추가
        ///// </summary>
        //[Route("mst/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody] IEnumerable<ManVo> voList)
        //{
        //    try
        //    {
        //        Properties.EntityMapper.BeginTransaction();
        //        foreach (ManVo item in voList)
        //        {
        //            Properties.EntityMapper.Insert("M6651InsertMst", item);
        //        }
        //        Properties.EntityMapper.CommitTransaction();
        //        return Ok<int>(1);
        //        //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        Properties.EntityMapper.RollBackTransaction();
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 작업지시서 발행 - 수정
        ///// </summary>
        //[Route("u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstUpdate([FromBody]ManVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("M6651UpdateMaster", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        /// <summary>
        /// 작업지시서 발행 - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66540DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 작업지시서 발행 - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66540DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }






        /// <summary>
        /// 작업지시서 발행 - Report
        /// </summary>
        [Route("report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520SelectReport", vo));
        }


    }
}