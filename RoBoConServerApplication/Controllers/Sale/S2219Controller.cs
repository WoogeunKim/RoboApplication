using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2219")]
    public class S2219Controller : ApiController
    {
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
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

        /// <summary>
        /// 견적서 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2219SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 견적서 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                // 견적번호 조회
                vo.ESTM_NO = Properties.EntityMapper.QueryForObject<string>("S2219SelectEstmNo", vo);
                Properties.EntityMapper.Insert("S2219InsertPopupMst", vo);
                Properties.EntityMapper.Update("S2219UpdateEstmNo", vo);

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
        /// 견적서 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S2219UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 견적서 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Delete("S2219DeleteMst", vo);
                Properties.EntityMapper.Delete("S2219DeleteDtl", vo);

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
        /// 견적서 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<SaleVo>(Properties.EntityMapper.QueryForObject<SaleVo>("S2219SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 견적서 내역 - 조회
        /// </summary>
        [Route("estm")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetEstmSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2219SelectEstmList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 견적서 내역 - 추가
        /// </summary>
        [Route("estm/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetEstmInsert([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S2219InsertEstm", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 견적서 내역 - 수정
        /// </summary>
        [Route("estm/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetEstmUpdate([FromBody] SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.Update("S2219UpdateEstm", vo);

                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2219SelectEstmList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 견적서 내역 - 삭제
        /// </summary>
        [Route("estm/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetEstmDelete([FromBody] SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Delete("S2219DeleteEstm", vo);
                Properties.EntityMapper.Update("S2219UpdateEstmSeq", vo);

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
        /// 공정과정 선택 - 조회
        /// </summary>
        [Route("prod")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetProdSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2219SelectProdList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}