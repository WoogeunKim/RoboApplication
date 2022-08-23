using ModelsLibrary.Pur;
using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s3321")]
    public class S3321Controller : ApiController
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
        /// 수주등록 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S3321SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///  수주등록 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] SaleVo vo)
        {
            try
            {
                vo.SL_RLSE_NO = Properties.EntityMapper.QueryForObject<string>("S3321SelectSlRlseNo", vo);  // 키생성
                Properties.EntityMapper.Update("S3321UpdatePurOrdNo", vo);                              // 생성된 키를 업데이트
                return Ok<int>(Properties.EntityMapper.Insert("S3321InsertMst", vo) == null ? 1 : 0);  // 업데이트된걸 인서트
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 수주등록 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S3321UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 수주등록 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] SaleVo vo)
        {
            try
            {
                //Properties.EntityMapper.Delete("P441106DeleteDtl", vo);
                return Ok<int>(Properties.EntityMapper.Delete("S3321DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 수주등록 POPUP(도면등록) - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("S3321SelectPopupList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 수주등록 DTL(도면관리) - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S3321SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 수주등록 DTL - 추가(도면등록)
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();  // ????

                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.Insert("S3321InsertDtl", item);
                }

                Properties.EntityMapper.CommitTransaction();  // ?????
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /////// <summary>
        /////// 원재료 발주 관리 DTL - 수정
        /////// </summary>
        ////[Route("dtl/u")]
        ////[HttpPost]
        ////// PUT api/<controller>/5
        ////public async Task<IHttpActionResult> GetDtlUpdate([FromBody] PurVo vo)
        ////{
        ////    try
        ////    {
        ////        return Ok<int>(Properties.EntityMapper.Update("P441106UpdateDtl", vo));
        ////    }
        ////    catch (System.Exception eLog)
        ////    {
        ////        return Ok<string>(eLog.Message);
        ////    }
        ////}


        /// <summary>
        /// 고객사발주등록 DTL - 삭제(도면삭제)
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] IEnumerable<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();  // ????

                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.Delete("S3321DeleteDtl", item);
                }

                Properties.EntityMapper.CommitTransaction();  // ?????
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }

            //try
            //{
            //    return Ok<int>(Properties.EntityMapper.Delete("S3321DeleteDtl", vo));
            //}
            //catch (System.Exception eLog)
            //{
            //    return Ok<string>(eLog.Message);
            //}
        }


    }
}