﻿using ModelsLibrary.Man;
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
    [RoutePrefix("m66333")]
    public class M66333Controller : ApiController
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
        ///  수동수주출력 / 생산 활용 - MST 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66333SelectMst", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///  수동수주출력 / 생산 활용 - DTL 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66333SelectDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}