using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ModelsLibrary.Fproof;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6637")]
    public class M6637Controller : ApiController
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
        /// 칭량 작업 계획 / 지시 관리(추가/폐기) MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6637SelectMaster", vo));
        }

        /// <summary>
        /// 칭량 작업 계획 / 지시 관리(추가/폐기) MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M6637InsertMaster", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 칭량 작업 계획 / 지시 관리(추가/폐기) MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6637UpdateMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 칭량 작업 계획 / 지시 관리(추가/폐기) MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6637DeleteMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        // [Route("mst")]
        // [HttpPost]
        // // GET api/<controller>
        // public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        // {
        //     return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6632SelectMaster", vo));
        // }


        // /// <summary>
        // /// 칭량 작업 계획 / 지시 관리 - 프로시저 
        // /// </summary>
        // [Route("mst/proc")]
        // [HttpPost]
        // // PUT api/<controller>/5
        // public async Task<IHttpActionResult> GetProc([FromBody]ManVo vo)
        // {
        //     try
        //     {
        //         //return Ok<int>(Properties.EntityMapper.QueryForObject<int>("ProcM6632", vo));
        //         int? nResult = Properties.EntityMapper.QueryForObject<int?>("ProcM6632", vo);
        //         if (nResult == 0)
        //         {
        //             //성공
        //             return Ok<int>(1);
        //         }
        //         else if (nResult == 5)
        //         {
        //             //실패
        //             return Ok<string>("투입재고부족위배");
        //         }
        //         else if (nResult == 10)
        //         {
        //             //실패
        //             return Ok<string>("유효기간위배");
        //         }
        //         else if (nResult == 20)
        //         {
        //             //실패
        //             return Ok<string>("선입선출위배");
        //         }
        //         else if (nResult == 30)
        //         {
        //             //실패
        //             return Ok<string>("투입부품위배");
        //         }
        //         else if (nResult == 40)
        //         {
        //             //실패
        //             return Ok<string>("칭량-선택한자재와 일치하지 않습니다.");
        //         }
        //         else if (nResult == 100)
        //         {
        //             //실패
        //             return Ok<string>("이미 투입 LOT가 존재합니다. 투입  LOT  삭제 후 생성이 가능합니다.");
        //         }
        //         else if (nResult == 200)
        //         {
        //             //실패
        //             return Ok<string>("해당 LOT는 재단포장에 투입이 되었습니다. 수정불가 ");
        //         }
        //         else if (nResult == 300)
        //         {
        //             //실패
        //             return Ok<string>("해당 LOT는 보관/숙성 작업실적에 투입이 되었습니다.");
        //         }
        //         else
        //         {
        //             //실패
        //             return Ok<string>("관리자 문의 부탁 드립니다");
        //         }
        //     }
        //     catch (System.Exception eLog)
        //     {
        //         return Ok<string>(eLog.Message);
        //     }
        // }




        // ///// <summary>
        // ///// 칭량 관리 DTL - 조회
        // ///// </summary>
        // //[Route("dtl")]
        // //[HttpPost]
        // //// GET api/<controller>
        // //public async Task<IHttpActionResult> GetDtlSelect([FromBody]ManVo vo)
        // //{
        // //    return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6632InsertDetail", vo));
        // //}

        // /// <summary>
        // /// 칭량 작업 계획 / 지시 관리 DTL - 수정
        // /// </summary>
        //// [Route("dtl/u")]
        //// [HttpPost]
        // // PUT api/<controller>/5
        //// public async Task<IHttpActionResult> GetDtlUpdate([FromBody]ManVo vo)
        //// {
        ////     try
        ////     {
        ////         return Ok<int>(Properties.EntityMapper.Update("M6632DeleteDetail", vo));
        ////     }
        ////     catch (System.Exception eLog)
        ////     {
        ////         return Ok<string>(eLog.Message);
        ////     }
        //// }



        // ///// <summary>
        // ///// 칭량 작업 계획 / 지시 관리 DTL - 수정
        // ///// </summary>
        // [Route("dtl/m")]
        // [HttpPost]
        // // POST api/<controller>
        // public async Task<IHttpActionResult> GetDtlUpdateList([FromBody] IEnumerable<ManVo> voList)
        // {
        //     try
        //     {
        //         Properties.EntityMapper.BeginTransaction();
        //         //
        //         foreach (ManVo item in voList)
        //         {
        //             Properties.EntityMapper.Update("M6631UpdateDetail", item);
        //             if (!string.IsNullOrEmpty(item.GRP_LOT_NO))
        //             {
        //                 Properties.EntityMapper.Update("M6631UpdateMapDetail", item);
        //             }
        //         }
        //         //
        //         Properties.EntityMapper.CommitTransaction();
        //         return Ok<int>(1);
        //         //return Ok<int>(Properties.EntityMapper.Insert("S1162InsertMst", vo) == null ? 1 : 0);
        //     }
        //     catch (System.Exception eLog)
        //     {
        //         Properties.EntityMapper.RollBackTransaction();
        //         return Ok<string>(eLog.Message);
        //     }
        // }


        // /// <summary>
        // /// 칭량 관리 - 시험번호 DTL - 조회
        // /// </summary>
        // [Route("dtl/insp")]
        // [HttpPost]
        // // GET api/<controller>
        // public async Task<IHttpActionResult> GetInspSelect([FromBody] ManVo vo)
        // {
        //     return Ok<string>(Properties.EntityMapper.QueryForObject<string>("M6632SelectInspNo", vo));
        // }

        ///// <summary>
        ///// 칭량 작업 계획 / 지시 관리 DTL - 삭제
        ///// </summary>
        //[Route("dtl/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetDtlDelete([FromBody]ManVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Delete("M6631DeleteDetail", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}



        //[Route("mst/item")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetItemSelect([FromBody] FproofVo vo)
        //{
        //    return Ok<IEnumerable<FproofVo>>(Properties.EntityMapper.QueryForList<FproofVo>("SelectPurRegMstList", vo));
        //}



    }
}