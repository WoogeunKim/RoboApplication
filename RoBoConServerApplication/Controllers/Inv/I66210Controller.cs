using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i66210")]
    public class I66210Controller : ApiController
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
        ///품목 출고  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I66110SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 품목 출고  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody] IList<InvVo> voList)
        public async Task<IHttpActionResult> GetMstInsert([FromBody] InvVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("I66210InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 품목 출고  - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] InvVo vo)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();
                //foreach (InvVo item in voList)
                //{
                    Properties.EntityMapper.Delete("I66110DeleteMst", vo);
                //}
                //return Ok<int>(Properties.EntityMapper.Insert("I6610InsertPurMst", vo) == null ? 1 : 0);
                //Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Delete("I6610DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}