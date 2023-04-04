using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i5511")]
    public class I5511Controller : ApiController
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
        ///품목 입고 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5511SelectMstList", vo));
        }

        /// <summary>
        /// 품목 입고 MST  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]InvVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                vo.INSRL_NO = Properties.EntityMapper.QueryForObject<string>("I5511SelectInsrlNo", vo);
                Properties.EntityMapper.Update("I5511UpdateInsrlNo", vo);
                Properties.EntityMapper.Insert("I5511InsertMst", vo);

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
        /// 품목 입고 MST  - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]InvVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("I5511UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 입고 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]InvVo vo)
        {
            try
            {
                if (Properties.EntityMapper.QueryForList<InvVo>("I5511SelectDtlList", vo).Count > 0)
                {
                    return Ok<string>(vo.INSRL_NM + " - 품목 입고 자재 내역이 존재 합니다");
                }

                return Ok<int>(Properties.EntityMapper.Delete("I5511DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///품목 입고 DTL  -  조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5511SelectDtlList", vo));
        }

        /// <summary>
        ///품목 입고 DTL  - POPUP (가입고에서 정입고) 조회
        /// </summary>
        [Route("popup/imp")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopInSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5511SelectDtlImpInList", vo));
        }

        /// <summary>
        ///품목 입고 DTL  - POPUP (기타 입고) 조회
        /// </summary>
        [Route("popup/etc")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopEtcInSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5511SelectDtlEtcInList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
        /// <summary>
        ///품목 입고 DTL  - POPUP (외주 입고) 조회
        /// </summary>
            [Route("popup/out")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopOutInSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5511SelectDtlOutInList", vo));
        }


        /// <summary>
        /// 품목 입고 DTL  - 추가
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
                    Properties.EntityMapper.Insert("I5511InsertDtl", item);
                }
                //return Ok<int>(Properties.EntityMapper.Insert("I6610InsertPurMst", vo) == null ? 1 : 0);
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Delete("I6610DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 입고 MST - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]InvVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("I5511DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}