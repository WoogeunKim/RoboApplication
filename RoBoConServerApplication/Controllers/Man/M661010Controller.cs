using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m661010")]
    public class M661010Controller : ApiController
    {
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


        /// <summary>
        /// 오더매니저 - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 오더매니저 - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 오더매니저 - 조회
        /// </summary>
        [Route("dtl/img")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetImgSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectImg", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 오더매니저 - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Update("M661010UpdateDtl_1", vo);
                Properties.EntityMapper.Update("M661010UpdateDtl_2", vo);

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }
    }
}