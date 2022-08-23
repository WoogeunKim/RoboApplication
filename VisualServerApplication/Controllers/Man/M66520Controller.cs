using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66520")]
    public class M66520Controller : ApiController
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
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520SelectMaster", vo));
        }

        /// <summary>
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520SelectDetail", vo));
        }

        /// <summary>
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("dtl/two")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl_TwoSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520SelectDetail_two", vo));
        }

        /// <summary>
        /// 작업지시서 발행 - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520SelectPopup", vo));
        }



        /// <summary>
        /// 작업지시서 발행(충전) - 추가
        /// </summary>
        [Route("mst/m/c")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMst1Insert([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                ManVo _keyMstVo = Properties.EntityMapper.QueryForObject<ManVo>("M66520SelectMstTmp", voList[0]);

                //MST
                voList[0].LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                for (int x = 1; x <= 3; x++)
                {
                    voList[0].LOT_DIV_SEQ = x;
                    if (x == 1)
                    {
                        voList[0].DY_NGT_FLG = "A";
                        voList[0].PROD_PLN_QTY = voList[0].PROD_PLN_QTY_A;
                    }
                    else if (x == 2)
                    {
                        voList[0].DY_NGT_FLG = "B";
                        voList[0].PROD_PLN_QTY = voList[0].PROD_PLN_QTY_B;
                    }
                    else if (x == 3)
                    {
                        voList[0].DY_NGT_FLG = "C";
                        voList[0].PROD_PLN_QTY = voList[0].PROD_PLN_QTY_C;
                    }

                    Properties.EntityMapper.Insert("M66520InsertMst", voList[0]);
                }

                string _rout_cd = string.Empty;
                ManVo _keyDtlVo = new ManVo();
                foreach (ManVo item in voList)
                {
                    if (!item.ROUT_CD.Equals(_rout_cd))
                    {
                        _rout_cd = item.ROUT_CD;
                        for (int x = 1; x <= 3; x++)
                        {
                            _keyDtlVo = Properties.EntityMapper.QueryForObject<ManVo>("M66520SelectDtlTmp", voList[0]);
                            item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                            item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                            item.LOT_DIV_SEQ = x;

                            ////
                            ////[오전에 투입 자재 다 넣기]
                            //item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                            //item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;

                            //Properties.EntityMapper.Insert("M66520InsertDtlItm", item);

                            //구분
                            //if (item.DY_NGT_RMK.StartsWith("오전"))
                            if (x == 1)
                            {
                                ////[오전에 투입 자재 다 넣기]
                                //item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                                //item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;

                                //Properties.EntityMapper.Insert("M66520InsertDtlItm", item);
                                //
                                item.MAKE_ST_DT =  System.DateTime.Now.ToString("yyyy-MM-dd ") + "09:00";
                                item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "12:30";
                            }
                            //else if (item.DY_NGT_RMK.StartsWith("오후"))
                            else if (x == 2)
                            {
                                item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "13:30";
                                item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "18:00";
                            }
                            //else if (item.DY_NGT_RMK.StartsWith("연장"))
                            else if (x == 3)
                            {
                                item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "19:00";
                                item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "21:00";
                            }


                            ////End
                            //if (item.DY_NGT_RMK.EndsWith("오전"))
                            //{
                            //    //item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "09:00";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "12:30";
                            //}
                            //else if (item.DY_NGT_RMK.EndsWith("오후"))
                            //{
                            //    //item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "13:30";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "18:00";
                            //}
                            //else if (item.DY_NGT_RMK.EndsWith("연장"))
                            //{
                            //    //item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "19:00";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "21:00";
                            //}

                            Properties.EntityMapper.Insert("M66520InsertDtl", item);
                        }
                    }

                    //오전 -  [자재 투입 실적 등록]
                    item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                    item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                    item.LOT_DIV_SEQ = 1;

                    Properties.EntityMapper.Insert("M66520InsertDtlItm", item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 작업지시서 발행(포장) - 추가
        /// </summary>
        [Route("mst/m/p")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMst2Insert([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                ManVo _keyMstVo = Properties.EntityMapper.QueryForObject<ManVo>("M66520SelectMstTmp", voList[0]);

                //MST
                voList[0].LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                voList[0].LOT_DIV_SEQ = 1;
                Properties.EntityMapper.Insert("M66520InsertMst", voList[0]);
               
                string _rout_cd = string.Empty;
                ManVo _keyDtlVo = new ManVo();
                foreach (ManVo item in voList)
                {
                    if (!item.ROUT_CD.Equals(_rout_cd))
                    {
                        _rout_cd = item.ROUT_CD;
                        //for (int x = 1; x <= 3; x++)
                        //{
                            _keyDtlVo = Properties.EntityMapper.QueryForObject<ManVo>("M66520SelectDtlTmp", voList[0]);
                            item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                            item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;
                            item.LOT_DIV_SEQ = 1;

                            //구분
                            //if (item.DY_NGT_RMK.StartsWith("오전"))
                            //if (x == 1)
                            //{
                                //[오전에 투입 자재 다 넣기]
                                //item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                                //item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;

                                //Properties.EntityMapper.Insert("M66520InsertDtlItm", item);

                                //
                                //
                                item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "09:00";
                                item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "12:30";
                            //}
                            //else if (item.DY_NGT_RMK.StartsWith("오후"))
                            //else if (x == 2)
                            //{
                            //   item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "13:30";
                            //   item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "18:00";
                            //}
                            //else if (item.DY_NGT_RMK.StartsWith("연장"))
                            //else if (x == 3)
                            //{
                            //    item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "19:00";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "21:00";
                            //}


                            ////End
                            //if (item.DY_NGT_RMK.EndsWith("오전"))
                            //{
                            //    //item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "09:00";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "12:30";
                            //}
                            //else if (item.DY_NGT_RMK.EndsWith("오후"))
                            //{
                            //    //item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "13:30";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "18:00";
                            //}
                            //else if (item.DY_NGT_RMK.EndsWith("연장"))
                            //{
                            //    //item.MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "19:00";
                            //    item.MAKE_END_DT = System.DateTime.Now.ToString("yyyy-MM-dd ") + "21:00";
                            //}

                            Properties.EntityMapper.Insert("M66520InsertDtl", item);
                        //}
                    }

                    item.PROC_LOT_NO = _keyDtlVo.PROC_LOT_NO;
                    item.LOT_DIV_NO = _keyMstVo.LOT_DIV_NO;


                    Properties.EntityMapper.Insert("M66520InsertDtlItm", item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 작업지시서 발행 - 추가
        ///// </summary>
        //[Route("mst/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody] IEnumerable<ManVo> voList)
        //{
        //    try
        //    {
        //        Properties.EntityMapper.BeginTransaction();
        //        foreach (ManVo item in voList)
        //        {
        //            Properties.EntityMapper.Insert("M6651InsertMst", item);
        //        }
        //        Properties.EntityMapper.CommitTransaction();
        //        return Ok<int>(1);
        //        //return Ok<int>(Properties.EntityMapper.Insert("M6651InsertMst", vo) == null ? 1 : 0);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        Properties.EntityMapper.RollBackTransaction();
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        /// <summary>
        /// 작업지시서 발행 - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M66520UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 작업지시서 발행 - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66520DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 작업지시서 발행 - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66520DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }






        /// <summary>
        /// 작업지시서 발행 - Report
        /// </summary>
        [Route("report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520SelectReport", vo));
        }





        /// <summary>
        /// 작업지시서 발행(포장) - 팝업창에 공정별 개수 조회
        /// </summary>
        [Route("popup/routqty")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupRoutQty([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66520PopupRoutQty", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}