using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s133")]
    public class S133Controller : ApiController
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
        /// 품목 그룹 등록 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S133SelectCodeItemGroupList", vo));
        }

        /// <summary>
        /// 품목 그룹 등록 MST - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S133InsertItmGroupCode", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 그룹 등록 MST - (자동 증가) 추가
        /// </summary>
        [Route("i/auto")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstAutoInsert([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S1414InsertItmGroupCode", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
        /// <summary>
        /// 품목 그룹 등록 MST - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S133UpdateItemGroupCode", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 그룹 등록 MST - 삭제
        /// </summary>
        [Route("d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SystemCodeVo vo)
        {
            try
            {
                //#CHNL_CD#, #SQL#, #TB_NM#, #CRE_USR_ID#
                //vo.SQL = " DELETE FROM TB_CD_ITM_GRP WHERE 1 = 1 AND ITM_GRP_CD = '" + vo.ITM_GRP_CD + "' AND ITM_GRP_CLSS_CD = '" + vo.ITM_GRP_CLSS_CD + "'  AND PRNT_ITM_GRP_CD = '" + vo.PRNT_ITM_GRP_CD + "' AND CHNL_CD = '" + vo.CHNL_CD + "'";
                //vo.TB_NM = "TB_CD_ITM_GRP";

                //return Ok<int>(Properties.EntityMapper.Delete("system_ProcDelete", vo));
                return Ok<int>(Properties.EntityMapper.Delete("S133DeleteItemGroupCode", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }





    }
}