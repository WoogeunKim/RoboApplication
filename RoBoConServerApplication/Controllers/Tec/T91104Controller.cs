using ModelsLibrary.Tec;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Tec
{
    [RoutePrefix("t91104")]
    public class T91104Controller : ApiController
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
        /// 협력업체 반품 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] TecVo vo)
        {
            return Ok<IEnumerable<TecVo>>(Properties.EntityMapper.QueryForList<TecVo>("T91104SelectMstList", vo));
        }


        /// <summary>
        /// 협력업체 반품 MST - 프로시저 
        /// </summary>
        [Route("proc")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetProc([FromBody] IEnumerable<TecVo> voList)
        {
            try
            {

                Properties.EntityMapper.BeginTransaction();
                foreach (TecVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int>("ProcT91104", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 공정별 불량코드 관리 MST - 추가
        ///// </summary>
        //[Route("i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody] TecVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Insert("T91103InsertMaster", vo) == null ? 1 : 0);

        //        //Properties.EntityMapper.BeginTransaction();

        //        //if (voList.Count > 0)
        //        //{
        //        //    Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
        //        //foreach (PurVo item in voList)
        //        //{
        //        //    Properties.EntityMapper.Insert("P4411InsertDtl", item);
        //        //}

        //        //    Properties.EntityMapper.QueryForObject<int?>("ProcS2211", voList[0]);
        //        //}

        //        //Properties.EntityMapper.CommitTransaction();
        //        //return Ok<int>(1);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        //Properties.EntityMapper.RollBackTransaction();
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        ///// <summary>
        ///// 공정별 불량코드 관리 MST - 수정
        ///// </summary>
        //[Route("u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstUpdate([FromBody] TecVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("T91103UpdateMaster", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 공정별 불량코드 관리 MST - 삭제
        ///// </summary>
        //[Route("d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody] TecVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Delete("T91103DeleteMaster", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        /////// <summary>
        /////// 공정별 불량코드 관리 MST - 수정
        /////// </summary>
        ////[Route("m")]
        ////[HttpPost]
        ////// PUT api/<controller>/5
        ////public async Task<IHttpActionResult> GetMstUpdate([FromBody] IEnumerable<TecVo> voList)
        ////{
        ////    //try
        ////    //{
        ////    //    return Ok<int>(Properties.EntityMapper.Update("T8813UpdateMst", vo));
        ////    //}
        ////    //catch (System.Exception eLog)
        ////    //{
        ////    //    return Ok<string>(eLog.Message);
        ////    //}
        ////    try
        ////    {
        ////        Properties.EntityMapper.BeginTransaction();
        ////        foreach (TecVo item in voList)
        ////        {
        ////            Properties.EntityMapper.QueryForObject<int?>("T91100UpdateMst", item);
        ////        }
        ////        Properties.EntityMapper.CommitTransaction();
        ////        return Ok<int?>(1);
        ////    }
        ////    catch (System.Exception eLog)
        ////    {
        ////        Properties.EntityMapper.RollBackTransaction();
        ////        return Ok<string>(eLog.Message);
        ////    }
        ////}


    }
}