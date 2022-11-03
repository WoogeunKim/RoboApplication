using ModelsLibrary.Pur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p4402")]
    public class P4402Controller : ApiController
    {
        /// <summary>
        /// 원자재 원가표 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4402SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            } 
        }

        /// <summary>
        /// 원자재 원가표 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<PurVo>(Properties.EntityMapper.QueryForObject<PurVo>("P4402SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 원가표 DTL - 추가 및 수정
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] PurVo[] volist)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                PurVo vo = new PurVo();
                vo = volist[0];

                // 마스터에 파일 넣기
                Properties.EntityMapper.Insert("P4402InsertMst", vo);

                // 디테일 데이터 삭제
                Properties.EntityMapper.Delete("P4402DeleteDtl", vo);
                
                // 디테일 데이터 추가
                foreach(PurVo _vo in volist)
                {
                    Properties.EntityMapper.Insert("P4402InsertDtl", _vo);
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
        /// 원자재 원가표 DTL 추가 페이지 - 매입처명 조회
        /// </summary>
        [Route("dig/co")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> CoNmSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4402SelectCoNmList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 원가표 파일 - 조회
        /// </summary>
        [Route("file")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetFileSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<PurVo>(Properties.EntityMapper.QueryForObject<PurVo>("P4402SelectFile", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        

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
    }
}