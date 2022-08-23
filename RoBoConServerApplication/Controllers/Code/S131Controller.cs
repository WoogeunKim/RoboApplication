using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s131")]
    public class S131Controller : ApiController
    {
        #region MyRegion
        //[Route("")]
        //[HttpGet]
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //[Route("")]
        //[HttpGet]
        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //[Route("")]
        //[HttpPost]
        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //[Route("")]
        //[HttpPut]
        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //[Route("")]
        //[HttpDelete]
        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        /// <summary>
        /// 시스템 분류 코드 MST - 조회
        /// </summary>
        [Route("mst/{chnl_cd}")]
        [HttpGet]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect(string chnl_cd)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S131SelectMasterCode", new SystemCodeVo() { CHNL_CD = chnl_cd }));
        }

        /// <summary>
        /// 시스템 분류 코드 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
               return Ok <int> (Properties.EntityMapper.Insert("S131InsertMasterCode", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 시스템 분류 코드 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S131UpdateMasterCode", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 시스템 분류 코드 DTL - 조회   (사용 만 조회)
        /// </summary>
        [Route("dtl/{chnl_cd}/{clss_tp_cd}")]
        [HttpGet]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect(string chnl_cd, string clss_tp_cd , string delt_flg = "N")
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S131SelectDetailCode", new SystemCodeVo() { CHNL_CD = chnl_cd, CLSS_TP_CD = clss_tp_cd, DELT_FLG = delt_flg }));
        }

        /// <summary>
        /// 시스템 분류 코드 DTL - 조회 (전체 조회)
        /// </summary>
        [Route("dtl/{chnl_cd}/{clss_tp_cd}/{delt_flg}")]
        [HttpGet]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelectDeltFlg(string chnl_cd, string clss_tp_cd, string delt_flg)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S131SelectDetailCode", new SystemCodeVo() { CHNL_CD = chnl_cd, CLSS_TP_CD = clss_tp_cd}));
        }


        /// <summary>
        /// 시스템 분류 코드 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
                if (Properties.EntityMapper.QueryForList<SystemCodeVo>("S131SelectDetailCode", vo).Count > 0)
                {
                    return Ok<string>("[코드 - 중복] 코드를 다시 입력 하십시오.");
                }

                return Ok<int>(Properties.EntityMapper.Insert("S131InsertDetailCode", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 시스템 분류 코드 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S131UpdateDetailCode", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        ///// <summary>
        ///// 시스템 분류 코드 DTL -  세부 조회
        ///// </summary>
        //[Route("systemcode")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstPrntMenu([FromBody]SystemCodeVo vo)
        //{
        //    return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S131SelectDetailCode", vo));
        //}

    }
}