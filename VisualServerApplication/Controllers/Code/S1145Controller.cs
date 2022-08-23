using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1145")]
    public class S1145Controller : ApiController
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
        /// 제품 단가 등록 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1145SelectMstList", vo));
        }


        /// <summary>
        /// 제품 단가 등록 MST - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
                if (vo.RN == 1)
                {
                    Properties.EntityMapper.Insert("S1145InsertMst_1", vo);
                    return Ok<int>(Properties.EntityMapper.QueryForObject<int>("ProcS1145_1", vo) == null ? 1 : 0);
                }
                else
                {
                    Properties.EntityMapper.Insert("S1145InsertMst_2", vo);
                    return Ok<int>(Properties.EntityMapper.QueryForObject<int>("ProcS1145_2", vo) == null ? 1 : 0);
                }
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 제품 단가 등록 이력 - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1145SelectMstList", vo));
        }


    }
}