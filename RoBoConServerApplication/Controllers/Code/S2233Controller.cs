using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2233")]
    public class S2233Controller : ApiController
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

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2233SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        
        [Route("d")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstDelete([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S2233DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

       [Route("dtl")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDialogSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2233SelectDialogList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        [Route("i")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstInsert([FromBody] IEnumerable<SaleVo> voList)
        {
            /*try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int?>("S2233InsertMst", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }*/

            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int?>("S2233InsertMst", item);
                    SaleVo _NewData = Properties.EntityMapper.QueryForObject<SaleVo>("S2233SelectNewInsertData", item);
                    Properties.EntityMapper.QueryForObject<int?>("ProcS2233INSERT", _NewData);

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