using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("S22223")]
    public class S22223Controller : ApiController
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
        /// 주문별 생산공장/GR확정 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22223SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 주문별 생산공장/GR확정 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22223SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 주문별 생산공장/GR확정 - GR번호 부여
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                #region 이전 작업
                //GR 번호 생성
                //voList[0].TBL_ID = "GR";

                //string _LST_FMT_NO = Properties.EntityMapper.QueryForObject<string>("S22223SelectRlseCmdNo", voList[0]);
                //Properties.EntityMapper.Update("S22223UpdateRlseCmdNo", voList[0]);

                //foreach (SaleVo item in voList)
                //{
                //    voList[0].TBL_ID = "GR";

                //    string _LST_FMT_NO = Properties.EntityMapper.QueryForObject<string>("S22223SelectRlseCmdNo", voList[0]);
                //    Properties.EntityMapper.Update("S22223UpdateRlseCmdNo", voList[0]);

                //    //GR 번호 
                //    item.LST_FMT_NO = _LST_FMT_NO;

                //    //바코드 번호 생성
                //    //item.TBL_ID = "BC";
                //    //item.GBN = Properties.EntityMapper.QueryForObject<string>("S22223SelectRlseCmdNo", item);
                //    //Properties.EntityMapper.Update("S22223UpdateRlseCmdNo", item);


                //    Properties.EntityMapper.Update("S22223UpdateMst", item);
                //} 
                #endregion

                foreach (SaleVo _item in voList)
                {
                    // TBL_ID 를 GR로 세팅
                    _item.TBL_ID = "GR";

                    // 키 주머니에서 번호 부여받음
                    string _LST_FMT_NO = Properties.EntityMapper.QueryForObject<string>("S22223SelectRlseCmdNo", _item);

                    // 현재 데이터의 키값을 넣어줌
                    _item.LST_FMT_NO = _LST_FMT_NO;

                    // 키 주머니에서 받은 번호에따라 키 주머니 테이블을 업데이트
                    Properties.EntityMapper.Update("S22223UpdateRlseCmdNo", _item);

                    // 키값을 업데이트
                    Properties.EntityMapper.Update("S22223UpdateMst", _item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 주문별 생산공장/GR확정 - GR번호 확정
        /// </summary>
        [Route("mst/gr")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdateGR([FromBody] SaleVo vo)
        {
            try
            {
                
                return Ok <int>( Properties.EntityMapper.Update("S22223UpdateGR", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        ///// <summary>
        ///// 주문 등록 MST - 추가
        ///// </summary>
        //[Route("mst/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody] SaleVo vo)
        //{
        //    try
        //    {
        //        //vo.CLT_BIL_DELT_NO = Properties.EntityMapper.QueryForObject<string>("S2222SelectCltBilDeltNo", vo);
        //        //Properties.EntityMapper.Update("S2222UpdateCltBilDeltNo", vo);
        //        return Ok<int>(Properties.EntityMapper.Insert("S3311InsertMst", vo) == null ? 1 : 0);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}




        ///// <summary>
        ///// 거래처별 판가기준표 MST - 삭제
        ///// </summary>
        //[Route("mst/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody] SaleVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Delete("S3311DeleteMst", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}




        /// <summary>
        /// 납품확인서 Dialog - 조회
        /// </summary>
        [Route("dlg")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDlgSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<SaleVo>(Properties.EntityMapper.QueryForObject<SaleVo>("S22223GrDlgSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 납품확인서 Dialog - 추가
        /// </summary>
        [Route("dlg/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDlgInsert([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S22223GrDlgInsert", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 납품확인서 Dialog - 수정
        /// </summary>
        [Route("dlg/u")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDlgUpdate([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S22223GrDlgUpdate", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 납품확인서 Report1 - 조회
        /// </summary>
        [Route("rpt1")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetRpt1Select([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22223GrRpt1Select", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 납품리스트 Report2 - 조회
        /// </summary>
        [Route("rpt2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetRpt2Select([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22223GrRpt2Select", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}