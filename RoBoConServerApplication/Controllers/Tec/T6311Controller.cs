using ModelsLibrary.Tec;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Tec
{
    [RoutePrefix("t6311")]

    public class T6311Controller : ApiController
    {
        #region
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
        /// 출하검사 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] TecVo vo)
        {
            try
            {
                return Ok<IEnumerable<TecVo>>(Properties.EntityMapper.QueryForList<TecVo>("T6311SelectMst", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 출하검사 - 출하검사
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] TecVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("T6311UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }

            //try
            //{
            //    Properties.EntityMapper.BeginTransaction();
            //    foreach (TecVo item in voList)
            //    {
            //        Properties.EntityMapper.QueryForObject<int?>("T6311UpdateMst", item);
            //    }
            //    Properties.EntityMapper.CommitTransaction();
            //    return Ok<int?>(1);
            //}
            //catch (System.Exception eLog)
            //{
            //    Properties.EntityMapper.RollBackTransaction();
            //    return Ok<string>(eLog.Message);
            //}
        }
    }
}