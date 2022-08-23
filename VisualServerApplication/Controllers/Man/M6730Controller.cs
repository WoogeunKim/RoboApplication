using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6730")]
    public class M6730Controller : ApiController
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
        /// 사출 작업실적보고  - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730Select", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730SelectMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 작지 번호 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730SelectPopup", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 작지 번호  - 삭제
        /// </summary>
        [Route("mst/m")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstInsert([FromBody] IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                //Properties.EntityMapper.Delete("M6710DeleteDtl1", voList.GetEnumerator().Current);
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M6730InsertMst", item);

                }

                //Properties.EntityMapper.Insert("M6631InsertInaudHis", voList.GetEnumerator().Current);
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6710InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 작지 번호  - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6730UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 조립 작업실적보고  - 작지 번호  - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6730DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 조립 작업실적보고  - 비가동 현황 조회
        /// </summary>
        [Route("dtl1")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl1Select([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730SelectDtl1", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 조립 작업실적보고  - 비가동 현황 조회 - 저장
        ///// </summary>
        [Route("dtl1/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtl1Insert([FromBody] IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                //Properties.EntityMapper.Delete("M6710DeleteDtl1", voList.GetEnumerator().Current);
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M6730InsertDtl1", item);
                
                }

                //Properties.EntityMapper.Insert("M6631InsertInaudHis", voList.GetEnumerator().Current);
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("S1162InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 비가동 현황 조회  - 삭제
        /// </summary>
        [Route("dtl1/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl1Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6730DeleteDtl1", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }





        /// <summary>
        /// 조립 작업실적보고  - 불량 현황 조회
        /// </summary>
        [Route("dtl2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl2Select([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730SelectDtl2", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 조립 작업실적보고  - 불량 현황 조회 - 저장
        ///// </summary>
        [Route("dtl2/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtl2Insert([FromBody] IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                //Properties.EntityMapper.Delete("M6710DeleteDtl2", voList.GetEnumerator().Current);
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M6730InsertDtl2", item);

                }

                //Properties.EntityMapper.Insert("M6631InsertInaudHis", voList.GetEnumerator().Current);
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("S1162InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 불량 현황 조회  - 삭제
        /// </summary>
        [Route("dtl2/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl2Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6730DeleteDtl2", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }






        /// <summary>
        /// 조립 작업실적보고  - 투입자재 불량 현황 조회
        /// </summary>
        [Route("dtl3/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl3Select_M([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730SelectDtl3_M", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 투입자재 불량 현황 조회
        /// </summary>
        [Route("dtl3/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl3Select_D([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6730SelectDtl3_D", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 조립 작업실적보고  - 투입자재 불량 현황 조회 - 저장
        ///// </summary>
        [Route("dtl3/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtl3Insert([FromBody] IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                //Properties.EntityMapper.Delete("M6710DeleteDtl2", voList.GetEnumerator().Current);
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M6730InsertDtl3", item);

                }

                //Properties.EntityMapper.Insert("M6631InsertInaudHis", voList.GetEnumerator().Current);
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("S1162InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 조립 작업실적보고  - 투입자재 불량 현황 조회  - 수정
        /// </summary>
        [Route("dtl3/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl3Update([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6730UpdateDtl3", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 조립 작업실적보고  - 투입자재 불량 현황 조회  - 삭제
        /// </summary>
        [Route("dtl3/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl3Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6730DeleteDtl3", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }






    }
}