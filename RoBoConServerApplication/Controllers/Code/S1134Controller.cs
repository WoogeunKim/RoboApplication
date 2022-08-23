using ModelsLibrary.Code;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1134")]
    public class S1134Controller : ApiController
    {
        /*// GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }*/

        [Route("snd/usr")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1134GetUsrSelectMaster", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /*[Route("key")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetKeySelect([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1134SelectDate", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }*/


        [Route("msg/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] List<SystemCodeVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                // 키 호출
                string _MSG_NO =  Properties.EntityMapper.QueryForObject<string>("S1134SelectDate", new SystemCodeVo());

                // 메시지 내용 저장
                voList[0].MSG_NO = _MSG_NO;
                Properties.EntityMapper.Insert("S1134MailListInsert", voList[0]);
                voList.RemoveAt(0);

                // 받는 사람 저장
                foreach (SystemCodeVo item in voList)
                {
                    item.MSG_NO = _MSG_NO;
                    Properties.EntityMapper.Insert("S1134MailGetInsert", item);
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

        /*[Route("usr/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetUsrInsert([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S1134MailGetInsert", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }*/


        [Route("get/usr")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstUsrSelect([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1134GetMailMaster", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        [Route("get/msg")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> MstSelect([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1134MailCheckSelect", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 메시지 확인하는 날짜 MST - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] SystemCodeVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                // 날짜 호출
                string _ACPT_DT = Properties.EntityMapper.QueryForObject<string>("S1134GetDateSelect", new SystemCodeVo());
                vo.ACPT_DT = _ACPT_DT;

                Properties.EntityMapper.Update("S1134GetDateUpdate", vo);

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