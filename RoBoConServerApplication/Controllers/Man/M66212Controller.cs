using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66212")]
    public class M66212Controller : ApiController
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
        /// (BOM)공정제품연결 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66212SelectMaster", vo));
        }

        /// <summary>
        /// (BOM)공정제품연결 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66212SelectDetail", vo));
        }

        /// <summary>
        /// (BOM)공정제품연결 POPUP - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66212SelectPopup", vo));
        }


        /// <summary>
        /// (BOM)공정제품연결 MST LIST - 조회
        /// </summary>
        [Route("mst/list")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstListSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66212SelectMasterList", vo));
        }


        /// <summary>
        /// (BOM)공정제품연결 MST  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]ManVo vo)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();
                //Properties.EntityMapper.Delete("M66212DeleteMaster", vo);
                return Ok<int>(Properties.EntityMapper.Insert("M66212InsertMaster", vo) == null ? 1 : 0);
                //Properties.EntityMapper.CommitTransaction();
                //return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("M66212InsertMaster", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (BOM)공정제품연결 MST  - 추가
        /// </summary>
        [Route("mst/i/list")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstListInsert([FromBody]IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //

                //Properties.EntityMapper.Delete("M66212DeleteMaster", new ManVo() { CHNL_CD = voList[0]. });

                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M66212InsertMaster", item);
                }
                //
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
        /// (BOM)공정제품연결 MST  - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstDelete([FromBody]ManVo vo)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();
                //Properties.EntityMapper.Delete("M66212DeleteMaster", vo);
                //Properties.EntityMapper.Insert("M66212InsertMaster", vo);
                //Properties.EntityMapper.CommitTransaction();
                return Ok<int>(Properties.EntityMapper.Delete("M66212DeleteMaster", vo));
                //return Ok<int>(Properties.EntityMapper.Insert("M66212InsertMaster", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (BOM)공정제품연결 DTL  - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M66212InsertDetail", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (BOM)공정제품연결 DTL  - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M66212UpdateDetail", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (BOM)공정제품연결 DTL  - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66212DeleteDetail", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// (BOM)공정제품연결 Sync
        /// </summary>
        [Route("dtl/sync")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlSync([FromBody]ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                //Itm
                Properties.EntityMapper.Delete("M66212DeleteDetailSyncItm", vo);
                Properties.EntityMapper.Insert("M66212InsertDetailSyncItm", vo);

                ////Bom
                //Properties.EntityMapper.Delete("M66212DeleteDetailSyncBom", vo);
                //Properties.EntityMapper.Insert("M66212InsertDetailSyncBom", vo);
                    
                Properties.EntityMapper.CommitTransaction();

                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


    }
}