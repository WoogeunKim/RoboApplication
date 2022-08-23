using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6630")]
    public class M6630Controller : ApiController
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
        /// 칭량 작업 계획 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6630SelectMaster", vo));
        }

        /// <summary>
        /// 칭량 작업 계획 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M6630InsertMaster", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 칭량 작업 계획 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                //Properties.EntityMapper.Insert("M6631InsertInaudHis", vo);

                return Ok<int>(Properties.EntityMapper.Update("M6630UpdateMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 칭량 작업 계획 MST - 수정
        ///// </summary>
        //[Route("mst/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Delete("M6631DeleteMaster", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 칭량 작업 계획 / 지시 관리 MST - 삭제
        ///// </summary>
        //[Route("mst/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody]ManVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Delete("M6631DeleteMaster", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}





        /// <summary>
        /// 칭량 작업 계획 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6630SelectDetail", vo));
        }


        ///// <summary>
        /////  칭량 작업 계획 DTL - 추가
        ///// </summary>
        [Route("dtl/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] IList<ManVo> voList)
        {
            try
            {
                string _PROD_PLN_NO;
                if (voList[0].GBN.Equals("Y"))
                {
                    //가공 원료
                    //
                    //삭제
                    Properties.EntityMapper.Delete("M6630DeleteDetail", voList[0]);
                    if (string.IsNullOrEmpty(voList[0].PROD_PLN_NO))
                    {
                        _PROD_PLN_NO = voList[0].PROD_PLN_NO = Properties.EntityMapper.QueryForObject<string>("M6630SelectProdPlnNo", voList[0]);
                        Properties.EntityMapper.Insert("M6630InsertMaster", voList[0]);
                    }
                }
                else
                {
                    //벌크
                    //string _PROD_PLN_NO = string.Empty;
                    Properties.EntityMapper.BeginTransaction();

                    _PROD_PLN_NO = voList[0].PROD_PLN_NO;
                    if (string.IsNullOrEmpty(voList[0].PROD_PLN_NO))
                    {
                        // (계획 번호 생성)
                        _PROD_PLN_NO = voList[0].PROD_PLN_NO = Properties.EntityMapper.QueryForObject<string>("M6630SelectProdPlnNo", voList[0]);

                        //Mst 생성 -> (계획 번호 생성)
                        Properties.EntityMapper.Insert("M6630InsertMaster", voList[0]);
                    }
                    else
                    {
                        //비고 (수정)
                        Properties.EntityMapper.Update("M6630UpdateMaster", voList[0]);
                    }

                    //
                    //삭제
                    Properties.EntityMapper.Delete("M6630DeleteDetail", voList[0]);

                    foreach (ManVo _item in voList)
                    {
                        _item.PROD_PLN_NO = _PROD_PLN_NO;

                        //추가
                        Properties.EntityMapper.Insert("M6630InsertDetail", _item);
                    }

                    Properties.EntityMapper.CommitTransaction();
                }
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
        ///// 칭량 작업 계획 / 지시 관리 DTL - 삭제
        ///// </summary>
        //[Route("dtl/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetDtlDelete([FromBody]ManVo vo)
        //{
        //    try
        //    {
        //        Properties.EntityMapper.Delete("M6631DeleteDetail2", vo);

        //        return Ok<int>(Properties.EntityMapper.Delete("M6631DeleteDetail", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}




    }
}