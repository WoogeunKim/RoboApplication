using ModelsLibrary.Pur;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p441102")]
    public class P441102Controller : ApiController
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
        /// 원재료 발주 관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441102SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///  원재료 발주 관리 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]PurVo vo)
        {
            try
            {
                vo.PUR_ORD_NO = Properties.EntityMapper.QueryForObject<string>("P441102SelectPurOrdNo", vo);
                Properties.EntityMapper.Update("P441102UpdatePurOrdNo", vo);
                return Ok<int>(Properties.EntityMapper.Insert("P441102InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원재료 발주 관리 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441102UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 원재료 발주 관리 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]PurVo vo)
        {
            try
            {
                Properties.EntityMapper.Delete("P441102DeleteDtl", vo);
                return Ok<int>(Properties.EntityMapper.Delete("P441102DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 원재료 발주 관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441102SelectDtlList", vo));
        }


        /// <summary>
        /// 원재료 발주 관리 DTL (바코드) - 조회 + 생성 
        /// </summary>
        [Route("dtl/barcode")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlBarcodeSelect([FromBody] PurVo vo)
        {
            try
            {
                IList<PurVo> _barCodeList = Properties.EntityMapper.QueryForList<PurVo>("P441102SelectDtlBarCodeList", vo);

                Properties.EntityMapper.BeginTransaction();
                //
                // - 바코드 저장
                foreach (PurVo item in _barCodeList)
                {
                    if (vo.R_MM_11.Equals("AUTO"))
                    {
                        if (Convert.ToInt32(_barCodeList[_barCodeList.IndexOf(item)].PUR_QTY) % Convert.ToInt32(_barCodeList[_barCodeList.IndexOf(item)].PK_PER_QTY) > 0)
                        {
                            _barCodeList[_barCodeList.Count - 1].SL_ITM_QTY = Convert.ToInt32(_barCodeList[_barCodeList.IndexOf(item)].PUR_QTY) % Convert.ToInt32(_barCodeList[_barCodeList.IndexOf(item)].PK_PER_QTY);
                        }
                    }

                    Properties.EntityMapper.Insert("P441102InsertBarCodeDtl", item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<IEnumerable<PurVo>>(_barCodeList);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 원재료 발주 관리 DTL (바코드) - 조회
        /// </summary>
        [Route("dtl/barcode/report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlBarcodeReportSelect([FromBody] PurVo vo)
        {
            try
            {
                //IList<PurVo> _barCodeList = Properties.EntityMapper.QueryForList<PurVo>("P441102SelectDtlBarCodeReportList", vo);

                //Properties.EntityMapper.BeginTransaction();
                ////
                //// - 바코드 저장
                //foreach (PurVo item in _barCodeList)
                //{
                //    Properties.EntityMapper.Insert("P441102InsertBarCodeDtl", item);
                //}

                //Properties.EntityMapper.CommitTransaction();
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441102SelectDtlBarCodeReportList", vo));
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 원재료 발주 관리 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]IList<PurVo> voList)
        {
            try
            {
                //return Ok<int>(Properties.EntityMapper.Insert("P4411InsertDtl", vo) == null ? 1 : 0);

                Properties.EntityMapper.BeginTransaction();

                //if (voList.Count > 0)
                //{
                //    Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
                    foreach (PurVo item in voList)
                    {
                        Properties.EntityMapper.Insert("P441102InsertDtl", item);
                    }

                //    Properties.EntityMapper.QueryForObject<int?>("ProcS2211", voList[0]);
                //}

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
        /// 원재료 발주 관리 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441102UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 원재료 발주 관리 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P441102DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 원재료 발주 관리 MST - 리포트
        ///// </summary>
        //[Route("report")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetReportSelect([FromBody]PurVo vo)
        //{
        //    return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectReport", vo));
        //}


        ///// <summary>
        /////원재료 발주 관리 MST - 리포트
        ///// </summary>
        //[Route("report/weekly")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetReportWeeklySelect([FromBody]PurVo vo)
        //{
        //    return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectWeeklyReport", vo));
        //}


        /// <summary>
        ///원재료 발주 관리(차종) POPUP - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup1Select([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441102SelectPopUp", vo));
        }

     

    }
}