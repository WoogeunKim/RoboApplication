using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6740")]
    public class M6740Controller : ApiController
    {
        /// <summary>
        /// 실적등록 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6740SelectMst", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 실적등록 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6740SelectDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 실적등록 - 생산입고
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlGrOriginUpdate([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                foreach (ManVo _item in voList)
                {
                    Properties.EntityMapper.Update("M6740UpdateDtl", _item);
                    Properties.EntityMapper.Update("M6740UpdateDtlProd", _item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }
    }
}