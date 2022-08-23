using ModelsLibrary.Fproof;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Fproof
{
    [RoutePrefix("wootest")]
    public class PurRegController : ApiController
    {
        #region MyRegion
        // GET api/<controller>
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
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion




        /// <summary>
        /// 매입등록 - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstSelect([FromBody] FproofVo vo)
        {
            try
            {
                return Ok<IEnumerable<FproofVo>>(Properties.EntityMapper.QueryForList<FproofVo>("SelectPurRegMstList", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 매입등록  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstInsert([FromBody] FproofVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("InsertPurRegMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 매입등록  - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] FproofVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("UpdatePurRegMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 매입등록 - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] FproofVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("DeletePurRegMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 채널 코드 체크
        /// </summary>
        [Route("test/{chnl_cd}")]
        [HttpGet]
        // GET api/<controller>/5
        public async Task<IHttpActionResult> GetChnl(string chnl_cd)
        {
            return Ok<FproofVo>(Properties.EntityMapper.QueryForObject<FproofVo>("SelectChnlList", new FproofVo() { CHNL_CD = chnl_cd, DELT_FLG = "N" }));
        }

    }
}