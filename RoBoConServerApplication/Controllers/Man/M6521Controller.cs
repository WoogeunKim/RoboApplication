using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6521")]
    public class M6521Controller : ApiController
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
        /// 생산계획/작업지시 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6521SelectMst", vo));

            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 생산계획/작업지시 - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] ManVo vo)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();


                //vo.PROD_PLN_NO = Properties.EntityMapper.QueryForObject<string>("M5210SelectNo", vo);
                //Properties.EntityMapper.Update("M5210UpdateNo", vo);

                //
                //Properties.EntityMapper.Insert("M5210InsertMst", vo);
                return Ok<int>(Properties.EntityMapper.Insert("M6521InsertDtl", vo) == null ? 1 : 0);
                //Properties.EntityMapper.CommitTransaction();
                //return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 생산계획/작업지시 - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6521UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 생산계획/작업지시 - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6521DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 생산계획/작업지시 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6521SelectDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 생산계획/작업지시 DIALOG 공정 - 조회
        /// </summary>
        [Route("rout")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDlgRoutSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<ManVo>(Properties.EntityMapper.QueryForObject<ManVo>("M6521SelectRoutCd", vo));

            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 생산계획/작업지시 DIALOG 목형 - 조회
        /// </summary>
        [Route("mold")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDlgMoldSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6521SelectMoldNo", vo));

            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 생산계획/작업지시 DIALOG 목형 - 조회
        /// </summary>
        [Route("pur/itm/sz")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDlgPurItmSzSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6521SelectPurItmSz", vo));

            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}