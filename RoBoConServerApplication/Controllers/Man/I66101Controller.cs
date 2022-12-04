using ModelsLibrary.Inv;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;


namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i66101")]
    public class I66101Controller : ApiController
    {

        /// <summary>
        /// 품목 입고  - 발주 조회
        /// </summary>
        [Route("pur")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPurSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I66101SelectDtlPurList", vo));

            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목 입고  - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] IList<InvVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                voList[0].INAUD_TMP_NO = Properties.EntityMapper.QueryForObject<string>("I66101SelectNo", voList[0]);
                Properties.EntityMapper.Update("I66101UpdateNo", voList[0]);

                foreach (InvVo item in voList)
                {
                    item.INAUD_TMP_NO = voList[0].INAUD_TMP_NO;
                    Properties.EntityMapper.Insert("I66101InsertPurMst", item);
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
        /// 품목 입고  - 삭제
        /// </summary>
        [Route("d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] IList<InvVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (InvVo item in voList)
                {
                    Properties.EntityMapper.Delete("I66101DeleteMst", item);
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