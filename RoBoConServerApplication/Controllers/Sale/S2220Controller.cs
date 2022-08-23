﻿using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2220")]
    public class S2220Controller : ApiController
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
        /// (월)판매계획 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2220SelectMstList", vo));
        }

        ///// <summary>
        ///// 수금 관리 MST - 대상조회
        ///// </summary>
        //[Route("mst/amt")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetMstAmtSelect([FromBody]SaleVo vo)
        //{
        //    return Ok<string>(Properties.EntityMapper.QueryForObject<string>("S2221SelectCltRmnAmt", vo));
        //}


        /// <summary>
        /// (월)판매계획  MST - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S2220InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (월)판매계획 MST - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S2220UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// (월)판매계획 MST - 삭제
        /// </summary>
        [Route("d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S2220DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



    }
}