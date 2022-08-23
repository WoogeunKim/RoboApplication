using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1162")]
    public class S1162Controller : ApiController
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
        /// 근무보고서 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1162SelectMstList", vo));
        }

        /// <summary>
        /// 근무보고서 MST - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S1162InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        [Route("m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsertList([FromBody]IEnumerable<SystemCodeVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                foreach (SystemCodeVo item in voList)
                {
                    Properties.EntityMapper.Insert("S1162InsertMst", item);
                }
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("S1162InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 근무보고서 MST - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S1162UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 근무보고서 MST - 삭제
        /// </summary>
        [Route("d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S1162DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



    }
}