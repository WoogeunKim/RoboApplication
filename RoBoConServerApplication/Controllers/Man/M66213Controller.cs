using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66213")]
    public class M66213Controller : ApiController
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
        /// (PDM)재공품번생성 및 제품데이터관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {

                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66213SelectMaster", vo));
            }
            catch(System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 MST  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M66213InsertMaster", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 MST  - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M66213UpdateMaster", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 MST  - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] ManVo vo)
        {
            try
            {
                if(Properties.EntityMapper.QueryForList<ManVo>("M66213SelectCheckdDelete", vo).Count == 0)
                {
                    return Ok<int>(Properties.EntityMapper.Delete("M66213DeleteMaster", vo));
                }
                else
                {
                    return Ok<string>("하위 데이터 존재합니다.");
                }
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66213SelectDetail", vo));
        }

        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 TREE - 조회
        /// </summary>
        [Route("tree")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetTreeSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66213SelectTree", vo));
        }


        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 TREE - 조회
        /// </summary>
        [Route("tree/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetTreeMstSelect([FromBody] ManVo vo)
        {
            return Ok<ManVo>(Properties.EntityMapper.QueryForObject<ManVo>("M66213SelectTreeMaster", vo));
        }




        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 모델코드 - 조회
        /// </summary>
        [Route("mdl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMdlSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66213SelectMdl", vo));
        }




        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 MST  - 복사
        /// </summary>
        [Route("mst/copy")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstCopy([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Delete("M66213DeleteCopy", vo);
                Properties.EntityMapper.Insert("M66213InsertCopy", vo);

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
        /// (BOM)공정제품연결 MST LIST - 조회
        /// </summary>
        [Route("popup/list")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstListSelect([FromBody] ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66213SelectPopupList", vo));
        }


        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 DTL  - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] IEnumerable<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //

                //Properties.EntityMapper.Delete("M66212DeleteMaster", new ManVo() { CHNL_CD = voList[0]. });

                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M66213InsertDetail", item);
                }
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (PDM)재공품번생성 및 제품데이터관리 DTL  - 추가
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M66213DeleteDetail", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}