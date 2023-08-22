using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m661010")]
    public class M661010Controller : ApiController
    {

        /// <summary>
        /// 오더매니저 설비 MST-  설비번호, 설비명, 배정건수 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 오더매니저 설비 MST - 강종 및 규격별 이미지 조회
        /// </summary>
        [Route("dtl/img")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetImgSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectImg", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 오더매니저 설비 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }


        }

        /// <summary>
        /// 오더매니저 설비 DTL - 작업지시리스트 조회
        /// </summary>
        [Route("dtl/lov")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlLovSelect([FromBody] ManVo vo)
        {
            try
            {

                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M661010SelectLotDivList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        // <summary>
        /// 오더매니저 - 작업지시리스트 중량 컬럼 조회
        /// </summary>
        [Route("lov/wgt")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetLovWgtSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<ManVo>(Properties.EntityMapper.QueryForObject<ManVo>("M661010SelectWeight", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 오더매니저 - 생산 관리 중량 컬럼 업데이트
        /// </summary>
        [Route("wgt/u")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetWgtUpdate([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Update("M661010UpdateWeight", item);
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



        // <summary>
        /// 오더매니저 - 작업지시리스트 중량 컬럼 업데이트
        /// </summary>
        [Route("lov/wgt/u")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetLovWgtUpdate([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                Properties.EntityMapper.Update("M661010UpdateLovWeight", vo);
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
        /// 오더매니저 - 생산계획관리 추가
        /// </summary>
        [Route("dtl/prod/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlProdInsert([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                //Properties.EntityMapper.Delete("M6710DeleteDtl1", voList.GetEnumerator().Current);
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M661010InsertProdDetail", item);
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
        /// 오더매니저 - 작업지시리스트 수정
        /// </summary>
        [Route("lov/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlLovUpdate([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Update("M661010UpdateLov", vo);

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
        /// 오더매니저 - 작업지시리스트 삭제
        /// </summary>
        [Route("lov/d")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlLovDelete([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                foreach (ManVo vo in voList)
                {
                    Properties.EntityMapper.Update("M661010DeleteLov", vo);
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
    }
}