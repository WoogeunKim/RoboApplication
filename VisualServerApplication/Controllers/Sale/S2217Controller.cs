using ModelsLibrary.Sale;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2217")]
    public class S2217Controller : ApiController
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
        /// 반품등록관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2217SelectMstList", vo));
        }


        /// <summary>
        /// 반품등록관리 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SaleVo vo)
        {
            try
            {
                vo.SL_BIL_RTN_NO = Properties.EntityMapper.QueryForObject<string>("S2217SelectSlBilRtnNo", vo);
                Properties.EntityMapper.Update("S2217UpdateSlBilRtnNo", vo);
                return Ok<int>(Properties.EntityMapper.Insert("S2217InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 반품등록관리 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S2217UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 반품등록관리 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.Delete("S2217DeleteDtl", vo);
                return Ok<int>(Properties.EntityMapper.Delete("S2217DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 반품등록관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2217SelectDtlList", vo));
        }

        /// <summary>
        /// 반품등록관리 DTL - 추가
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
                    //Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
                    foreach (SaleVo item in voList)
                    {
                        Properties.EntityMapper.Insert("S2217InsertDtl", item);
                    }

                    //Properties.EntityMapper.QueryForObject<int?>("ProcS2211", voList[0]);
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
        /// 반품등록관리 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S2217UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 반품등록관리 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S2217DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 반품등록관리 POPUP - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2217SelectDtlPopupList", vo));
        }

        ///// <summary>
        ///// 반품등록관리 - 입고 승인
        ///// </summary>
        //[Route("Apply")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetApply([FromBody]SaleVo vo)
        //{
        //    try
        //    {
        //        return Ok<int?>(Properties.EntityMapper.QueryForObject<int?>("ProcS2217Apply", vo));
        //    }
        //    catch(Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        /// <summary>
        /// 반품등록관리 - 승인 등록
        /// </summary>
        [Route("Apply")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetIoApply([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int?>(Properties.EntityMapper.QueryForObject<int?>("ProcS2216Apply", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 반품등록관리 - 승인 해제
        /// </summary>
        [Route("Cancel")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetIoCancel([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int?>(Properties.EntityMapper.QueryForObject<int?>("ProcS2216Delete", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}