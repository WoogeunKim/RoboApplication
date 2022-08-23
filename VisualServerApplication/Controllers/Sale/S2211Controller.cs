using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2211")]
    public class S2211Controller : ApiController
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
        /// 수주 등록 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2211SelectMstList", vo));
        }


        /// <summary>
        /// 수주 등록 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SaleVo vo)
        {
            try
            {
                vo.SL_RLSE_NO = Properties.EntityMapper.QueryForObject<string>("S2211SelectJbNo", vo);
                Properties.EntityMapper.Update("S2211UpdateJbNo", vo);
                return Ok<int>(Properties.EntityMapper.Insert("S2211InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 수주 등록 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S2211UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 수주 등록 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { SL_RLSE_NO = vo.SL_RLSE_NO, CHNL_CD = vo.CHNL_CD });

                return Ok<int>(Properties.EntityMapper.Delete("S2211DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 수주 등록 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2211SelectDtlList", vo));
        }

        /// <summary>
        /// 수주 등록 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                if (voList.Count > 0)
                {
                    Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
                    foreach (SaleVo item in voList)
                    {
                        Properties.EntityMapper.Insert("S2211InsertDtl", item);
                    }

                    Properties.EntityMapper.QueryForObject<int?>("ProcS2211", voList[0]);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 수주 등록 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S2211UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 수주 등록 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S2211DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 수주 등록 POPUP(Left1) - 조회
        /// </summary>
        [Route("popupleft")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupLeftSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2211SelectSubLeftDtlList", vo));
        }

        /// <summary>
        /// 수주 등록 POPUP(Right2) - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2211SelectSubDtlList", vo));
        }

        /// <summary>
        /// 수주 등록 POPUP 선택 Items(Right1) - 조회
        /// </summary>
        [Route("popupitems")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupItemsSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2211SelectSubItmDtlList", vo));
        }

        ///// <summary>
        ///// 수주 등록 POPUP_RESULT - 조회
        ///// </summary>
        //[Route("popup/r")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetPopupResultSelect([FromBody]SaleVo vo)
        //{
        //    if (vo.RN == 1)
        //    {
        //        return Ok<SaleVo>(Properties.EntityMapper.QueryForObject<SaleVo>("S2211SelectSubDtlResult", vo));
        //    }
        //    else if (vo.RN == 2)
        //    {
        //        return Ok<SaleVo>(Properties.EntityMapper.QueryForObject<SaleVo>("S2211SelectSubDtlResult2", vo));
        //    }
        //    else if (vo.RN == 3)
        //    {
        //        return Ok<SaleVo>(Properties.EntityMapper.QueryForObject<SaleVo>("S2211SelectQuickDtl", vo));
        //    }
        //    else
        //    {
        //        return Ok<string>("");
        //    }
        //}


    }
}