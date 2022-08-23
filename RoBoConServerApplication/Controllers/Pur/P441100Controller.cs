using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;
using System.Linq;
using System;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p441100")]
    public class P441100Controller : ApiController
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
        /// 원자재 소요량 전개 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441100SelectMstList", vo));
        }


        /// <summary>
        /// 원자재 소요량 전개 (BULK) DTL - 조회
        /// </summary>
        [Route("bulk")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetBulkDtlSelect([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441100SelectBulkDtlList", vo));
        }


        /// <summary>
        /// 원자재 소요량 전개 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441100SelectDtlList", vo));
        }




        /// <summary>
        /// 원자재 소요량 전개 - 프로시저 
        /// </summary>
        [Route("proc")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlProc([FromBody] PurVo vo)
        {
            try
            {
                return Ok<int?>(Properties.EntityMapper.QueryForObject<int?>("ProcP441100", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 원자재 소요량 전개 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody] PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441100UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 원자재 Loss 소요량 전개 DTL - 수정
        /// </summary>
        [Route("dtl/loss/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetLossDtlUpdate([FromBody] PurVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                IEnumerable<PurVo> _purList = Properties.EntityMapper.QueryForList<PurVo>("P441100SelectUpdateDtlList", vo);
                foreach (PurVo item in _purList)
                {
                    Properties.EntityMapper.Update("P441100UpdateDtl", item);
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



        /// <summary>
        /// 원자재 소요량 전개 MEMO - 수정
        /// </summary>
        [Route("mst/memo")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMemoUpdate([FromBody] PurVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Delete("P441100DeleteMemo", vo);
                Properties.EntityMapper.Insert("P441100InsertMemo", vo);

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
        /// 원자재 소요량 전개 DTL - 소요량 취합 조회
        /// </summary>
        [Route("marge")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMargeSelect([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441100SelectMargeList", vo));
        }

        /// <summary>
        /// 원자재 소요량 전개 일괄 - 저장
        /// </summary>
        [Route("marge/m")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMargeInsert([FromBody] IList<PurVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                //거래처 별 저장
                List<string> _coCdList = voList.Select(w => w.CO_CD).ToList();
                //중복 제거
                _coCdList = _coCdList.Distinct().ToList();

                PurVo _tmpMst = new PurVo();
                _tmpMst.CHNL_CD = voList[0].CHNL_CD;
                _tmpMst.PUR_DT = voList[0].PUR_DT;
                _tmpMst.PUR_DUE_DT = voList[0].PUR_DUE_DT;
                _tmpMst.PUR_RMK = voList[0].GBN;
                _tmpMst.CRE_USR_ID = voList[0].CRE_USR_ID;
                _tmpMst.UPD_USR_ID = voList[0].UPD_USR_ID;
                _tmpMst.PUR_CLZ_FLG = "N";
                _tmpMst.PUR_ITM_CD = "M";
                _tmpMst.AREA_CD= "001";



                IList<PurVo> _dtlList;
                //거래처 별
                foreach (string _coCd in _coCdList)
                {

                    //Mst
                    _tmpMst.PUR_CO_CD = _coCd;
                    _tmpMst.PUR_ORD_NO = Properties.EntityMapper.QueryForObject<string>("P441102SelectPurOrdNo", _tmpMst);
                    Properties.EntityMapper.Update("P441102UpdatePurOrdNo", _tmpMst);
                    //
                    Properties.EntityMapper.Insert("P441102InsertMst", _tmpMst);

                    //Dtl
                    _dtlList = voList.Where<PurVo>(x => x.CO_CD.Equals(_coCd)).ToList();
                    foreach (PurVo _item in _dtlList)
                    {
                        _item.PUR_ORD_NO = _tmpMst.PUR_ORD_NO;
                        _item.PUR_QTY = _item.REQ_ORD_QTY;
                        _item.DUE_DT = _tmpMst.PUR_DUE_DT;
                        _item.IN_REQ_DT = _tmpMst.PUR_DT;
                        _item.PUR_AMT =  Convert.ToDecimal(_item.PUR_QTY) * Convert.ToDecimal(_item.CO_UT_PRC);
                        _item.ORD_SEQ = _item.MRP_SEQ;
                        _item.ORD_NO = _item.UN_FOL_NO;
                        _item.CRE_USR_ID = _tmpMst.CRE_USR_ID;
                        _item.UPD_USR_ID = _tmpMst.UPD_USR_ID;

                        Properties.EntityMapper.Insert("P441102InsertDtl", _item);

                    }
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
        ///// 원자재 소요량 전개 DTL - 추가
        ///// </summary>
        //[Route("dtl/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetDtlInsert([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Insert("P4401InsertDtl", vo) == null ? 1 : 0);

        //        //Properties.EntityMapper.BeginTransaction();

        //        //if (voList.Count > 0)
        //        //{
        //        //    Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
        //            //foreach (PurVo item in voList)
        //            //{
        //            //    Properties.EntityMapper.Insert("P4411InsertDtl", item);
        //            //}

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
        ///// 원자재 소요량 전개 DTL - 수정
        ///// </summary>
        //[Route("dtl/u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetDtlUpdate([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("P4401UpdateDtl", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 원자재 소요량 전개 DTL - 삭제
        ///// </summary>
        //[Route("dtl/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetDtlDelete([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Delete("P4401DeleteDtl", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


    }
}