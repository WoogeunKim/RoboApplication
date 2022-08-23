using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66110")]
    public class M66110Controller : ApiController
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
        ///휴무일 관리 - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66110SelectMaster", vo));
        }

        /// <summary>
        /// 휴무일 관리  - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody]ManVo vo)
        public async Task<IHttpActionResult> GetMstInsert([FromBody]IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M66110InsertMst", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);

                //return Ok<int>(Properties.EntityMapper.Insert("M66110InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 휴무일 관리  - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M66110UpdateMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 휴무일 관리 - 삭제
        /// </summary>
        [Route("d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66110DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}