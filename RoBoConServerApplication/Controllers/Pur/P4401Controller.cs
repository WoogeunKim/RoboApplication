using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p4401")]
    public class P4401Controller : ApiController
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
        /// 매입단가관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4401SelectMstList", vo));
        }


        /// <summary>
        /// 매입단가관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4401SelectDtlList", vo));
        }

        /// <summary>
        /// 매입단가관리 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("P4401InsertDtl", vo) == null ? 1 : 0);

                //Properties.EntityMapper.BeginTransaction();

                //if (voList.Count > 0)
                //{
                //    Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
                    //foreach (PurVo item in voList)
                    //{
                    //    Properties.EntityMapper.Insert("P4411InsertDtl", item);
                    //}

                //    Properties.EntityMapper.QueryForObject<int?>("ProcS2211", voList[0]);
                //}

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
        /// 매입단가관리 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P4401UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 매입단가관리 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P4401DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}