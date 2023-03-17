using ModelsLibrary.Code;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1415")]
    public class S1415Controller : ApiController
    {
        /// <summary>
        /// 마스터 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<SystemCodeVo>(Properties.EntityMapper.QueryForObject<SystemCodeVo>("S1415SelectMaster", vo));
            }
            catch(Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 이미지 조회
        /// </summary>
        [Route("img")]
        [HttpPost]
        public async Task<IHttpActionResult> GetImgArraySelect([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<SystemCodeVo>(Properties.EntityMapper.QueryForObject<SystemCodeVo>("S1415SelectImgArray", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 마스터 - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] SystemCodeVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                var posDao = Properties.EntityMapper.QueryForObject<SystemCodeVo>("S1415SelectMaster", vo);

                if(posDao == null)
                {
                    Properties.EntityMapper.Insert("S1415InsertShapePos", vo);
                }
                else
                {
                    Properties.EntityMapper.Update("S1415UpdateShapePos", vo);
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