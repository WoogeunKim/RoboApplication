using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6710")]
    public class M6710Controller : ApiController
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
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710Select", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사출 작업실적보고  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사출 작업실적보고  - 작지 번호 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectPopup", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사출 작업실적보고  - 작지 번호  - 삭제
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
                    Properties.EntityMapper.Insert("M6710InsertMst", item);

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
        /// 사출 작업실적보고  - 작지 번호  - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6710UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사출 작업실적보고  - 작지 번호  - 수정
        /// </summary>
        [Route("mst/r/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstResultUpdate([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                ManVo _tmpVo = Properties.EntityMapper.QueryForObject<ManVo>("M6710SelectMstResult", vo);

                Properties.EntityMapper.Update("M6710UpdateMst", _tmpVo);

                Properties.EntityMapper.CommitTransaction();

                return Ok<object>(_tmpVo.WRK_HRS);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 사출 작업실적보고  - 작지 번호  - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6710DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 사출 작업실적보고  - 비가동 현황 조회
        /// </summary>
        [Route("dtl1")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl1Select([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectDtl1", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 사출 작업실적보고  - 비가동 현황 조회 - 저장
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
                    Properties.EntityMapper.Insert("M6710InsertDtl1", item);
                
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
        /// 사출 작업실적보고  - 비가동 현황 조회  - 삭제
        /// </summary>
        [Route("dtl1/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl1Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6710DeleteDtl1", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }





        /// <summary>
        /// 사출 작업실적보고  - 불량 현황 조회
        /// </summary>
        [Route("dtl2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl2Select([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectDtl2", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 사출 작업실적보고  - 불량 현황 조회 - 저장
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
                    Properties.EntityMapper.Insert("M6710InsertDtl2", item);

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
        /// 사출 작업실적보고  - 불량 현황 조회  - 삭제
        /// </summary>
        [Route("dtl2/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl2Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6710DeleteDtl2", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }






        /// <summary>
        /// 사출 작업실적보고  - 투입자재 불량 현황 조회
        /// </summary>
        [Route("dtl3/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl3Select_M([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectDtl3_M", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사출 작업실적보고  - 투입자재 불량 현황 조회
        /// </summary>
        [Route("dtl3/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl3Select_D([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectDtl3_D", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 사출 작업실적보고  - 투입자재 불량 현황 조회 - 저장
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
                    Properties.EntityMapper.Insert("M6710InsertDtl3", item);

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
        /// 사출 작업실적보고  - 투입자재 불량 현황 조회  - 수정
        /// </summary>
        [Route("dtl3/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl3Update([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6710UpdateDtl3", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 사출 작업실적보고  - 투입자재 불량 현황 조회  - 삭제
        /// </summary>
        [Route("dtl3/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl3Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6710DeleteDtl3", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 사출 작업실적보고  - 투입수량 현황 조회
        /// </summary>
        [Route("dtl4/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl4Select_D([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectDtl4_D", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 사출 작업실적보고  - 투입수량 현황 조회 - 저장
        ///// </summary>
        [Route("dtl4/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtl4Insert([FromBody] IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                //Properties.EntityMapper.Delete("M6710DeleteDtl2", voList.GetEnumerator().Current);
                foreach (ManVo item in voList)
                {
                    if (item.isCheckd == true)
                    {
                        Properties.EntityMapper.Insert("M6710InsertDtl4", item);
                    }
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
        /// 사출 작업실적보고  - 투입수량 현황 조회  - 삭제
        /// </summary>
        [Route("dtl4/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl4Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6710DeleteDtl4", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 사출 작업실적보고  - 투입수량(LOT) 현황 조회
        /// </summary>
        [Route("dtl5/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl5Select_D([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6710SelectDtl5_D", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 사출 작업실적보고  - 투입수량(LOT) 현황 조회  - 삭제
        /// </summary>
        [Route("dtl5/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtl5Delete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6710DeleteDtl5", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}