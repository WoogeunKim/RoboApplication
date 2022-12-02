using ModelsLibrary.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Mobile
{
    [RoutePrefix("mobile")]

    public class MobileController : ApiController
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
        /// 로그인 - 조회
        /// </summary>
        [Route("login")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMobileLoginSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<MobileVo>(Properties.EntityMapper.QueryForObject<MobileVo>("MobileLoginSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 프로그램 버전 - 조회
        /// </summary>
        [Route("ver")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMobileVerSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<MobileVo>(Properties.EntityMapper.QueryForObject<MobileVo>("MobileVerSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 입고 거래처 - 조회
        /// </summary>
        [Route("m/co")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMInCoListSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<IEnumerable<MobileVo>>(Properties.EntityMapper.QueryForList<MobileVo>("MInCoSelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 입고 발주 - 조회
        /// </summary>
        [Route("m/ord")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMInOrdListSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<IEnumerable<MobileVo>>(Properties.EntityMapper.QueryForList<MobileVo>("MInOrdSelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 품목 가입고  - 추가
        /// </summary>
        [Route("m/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] MobileVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                voList[0].INAUD_TMP_NO = Properties.EntityMapper.QueryForObject<string>("I6610SelectNo", voList[0]);
                Properties.EntityMapper.Update("I6610UpdateNo", voList[0]);

                foreach (InvVo item in voList)
                {
                    item.INAUD_TMP_NO = voList[0].INAUD_TMP_NO;
                    Properties.EntityMapper.Insert("I6610InsertPurMst", item);
                }
                //return Ok<int>(Properties.EntityMapper.Insert("I6610InsertPurMst", vo) == null ? 1 : 0);
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