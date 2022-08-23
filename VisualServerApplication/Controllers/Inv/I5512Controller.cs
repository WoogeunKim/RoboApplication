using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i5512")]
    public class I5512Controller : ApiController
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
        ///품목 출고 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5512SelectMstList", vo));
        }

        /// <summary>
        /// 품목 출고 MST  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]InvVo vo)
        {
            try
            {
                vo.INSRL_NO = Properties.EntityMapper.QueryForObject<string>("I5512SelectInsrlNo", vo);
                Properties.EntityMapper.Update("I5512UpdateInsrlNo", vo);

                return Ok<int>(Properties.EntityMapper.Insert("I5512InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 출고 MST  - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]InvVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("I5512UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 출고 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]InvVo vo)
        {
            try
            {
                if (Properties.EntityMapper.QueryForList<InvVo>("I5512SelectDtlList", vo).Count > 0)
                {
                    return Ok<string>(vo.INSRL_NM + " - 품목 출고 자재 내역이 존재 합니다");
                }

                return Ok<int>(Properties.EntityMapper.Delete("I5512DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///품목 출고 DTL  -  조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5512SelectDtlList", vo));
        }

        /// <summary>
        ///품목 출고 DTL  - POPUP (기타 출고) 조회
        /// </summary>
        [Route("popup/etc")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopEtcOutSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5512SelectDtlEtcOutList", vo));
        }


        /// <summary>
        ///품목 출고 DTL  - POPUP (외주 출고) 조회
        /// </summary>
        [Route("popup/out")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopOutOutSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5512SelectDtlOutOutList", vo));
        }


        /// <summary>
        ///품목 출고 DTL  - POPUP (샘플 출고) 조회
        /// </summary>
        [Route("popup/sample")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopSampleOutSelect([FromBody] InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5512SelectDtlSampleOutList", vo));
        }




        /// <summary>
        /// 품목 출고 DTL  - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]IList<InvVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (InvVo item in voList)
                {
                    Properties.EntityMapper.Insert("I5512InsertDtl", item);
                }
                //return Ok<int>(Properties.EntityMapper.Insert("I6610InsertPurMst", vo) == null ? 1 : 0);
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Delete("I6610DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 출고 MST - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]InvVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("I5512DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}