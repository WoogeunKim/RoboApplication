using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6631")]
    public class M6631Controller : ApiController
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
        /// 칭량 작업 계획 / 지시 관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6631SelectMaster", vo));
        }

        ///// <summary>
        ///// 칭량 작업 계획 / 지시 관리 MST - 추가
        ///// </summary>
        //[Route("mst/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody]ManVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Insert("M6623InsertMaster", vo) == null ? 1 : 0);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]ManVo vo)
        {
            try
            {
                //Properties.EntityMapper.Insert("M6631InsertInaudHis", vo);

                return Ok<int>(Properties.EntityMapper.Update("M6631UpdateMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6631DeleteMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 - 프로시저 
        /// </summary>
        [Route("mst/proc")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetProc([FromBody]ManVo vo)
        {
            try
            {
                int? nResult = Properties.EntityMapper.QueryForObject<int?>("ProcM6631", vo);

                //if (nResult == 1)
                //{
                //    //성공
                return Ok<int>(1);
                //}
                //else if (nResult == 0)
                //{
                //    //실패
                //    return Ok<string>("조회되지 않는 바코드 번호 입니다");
                //}
                //else if (nResult == 10)
                //{
                //    //실패
                //    return Ok<string>("One Label LOT-NO 중복 등록 되었습니다");
                //}
                //else if (nResult == 20)
                //{
                //    //실패
                //    return Ok<string>("해당 생산 LOT-NO에 투입 원자재가 아닙니다");
                //}
                //else
                //{
                //    //실패
                //    return Ok<string>("관리자 문의 부탁 드립니다");
                //}
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6631SelectDetail", vo));
        }

        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6631UpdateDetail", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]ManVo vo)
        {
            try
            {
                Properties.EntityMapper.Delete("M6631DeleteDetail2", vo);

                return Ok<int>(Properties.EntityMapper.Delete("M6631DeleteDetail", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 칭량 작업 계획 / 지시 관리 DTL - 시작 ~ 끝
        /// </summary>
        [Route("dtl/time")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlTime([FromBody] ManVo vo)
        {
            try
            {
                return Ok<string>(Properties.EntityMapper.QueryForObject<string>("M6631SelectTime", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}