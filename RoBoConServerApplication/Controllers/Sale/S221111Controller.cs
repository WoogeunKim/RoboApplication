using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s221111")]
    public class S221111Controller : ApiController
    {
        /// <summary>
        /// 출하 등록 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S221111SelectMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 출하 등록 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstInsert([FromBody] SaleVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                vo.SL_BIL_NO = Properties.EntityMapper.QueryForObject<string>("S221111SelectSLBILNo", vo);
                Properties.EntityMapper.Insert("S221111InsertMst", vo);
                Properties.EntityMapper.Update("S221111UpdateSLBILNo", vo);

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
        /// 출하 등록 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S221111UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 출하 등록 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstDelete([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S221111DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 출하 등록 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S221111SelectDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 출하 등록 DTL POPUP - 조회
        /// </summary>
        [Route("dtl/pop")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlPopSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S221111SelectDtlPopupList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 출하 등록 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach(SaleVo item in voList)
                {
                    Properties.EntityMapper.Insert("S221111InsertDtl", item);
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
        /// 출하 등록 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.Delete("S221111DeleteDtl", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}