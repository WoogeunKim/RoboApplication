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
        [Route("inp/m/co")]
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
        [Route("inp/m/ord")]
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
        /// 품목 입고  - 추가
        /// </summary>
        [Route("inp/m/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetInpMstInsert([FromBody] MobileVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                vo.LOT_NO = Properties.EntityMapper.QueryForObject<string>("InpLotNoSelect", vo);
                Properties.EntityMapper.Update("InpLotNoUpdate", vo);

                Properties.EntityMapper.Insert("InpMstInsert", vo);

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        

        /// <summary>
        /// 원자재 입고 LOT_NO 중복 여부 조회
        /// </summary>
        [Route("inp/lot/count")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetLotNoCountSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.QueryForObject<int>("InpLotNoCountSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 품목그룹관리 - 조회
        /// </summary>
        [Route("itm/grp")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetItmGrpListSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<IEnumerable<MobileVo>>(Properties.EntityMapper.QueryForList<MobileVo>("ItmGrpSelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 품목관리 김이정보 - 조회
        /// </summary>
        [Route("itm/len")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetItmLenListSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<IEnumerable<MobileVo>>(Properties.EntityMapper.QueryForList<MobileVo>("ItmLenSelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 입고시 시스템관리 - 조회
        /// </summary>
        [Route("inp/sys")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetInpSysListSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<IEnumerable<MobileVo>>(Properties.EntityMapper.QueryForList<MobileVo>("InpOtherCaseSelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 품목 잔량 - 조회
        /// </summary>
        [Route("itm/qty")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetItmQtyDataSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<MobileVo>(Properties.EntityMapper.QueryForObject<MobileVo>("ItmQtyDataSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 품목 출고  - 추가
        /// </summary>
        [Route("out/mtrl")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetOutMtrlInsert([FromBody] MobileVo vo)
        {
            try
            {
                Properties.EntityMapper.Insert("OutMtrlInsert", vo);
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 기존 바코드 입고  - 추가
        /// </summary>
        [Route("inp/ret/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetInpReturnInsert([FromBody] MobileVo vo)
        {
            try
            {
                Properties.EntityMapper.Insert("InpMstInsert", vo);
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



    }
}